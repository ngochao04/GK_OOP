using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace GKOOP
{
    public partial class ExamRoom : Form
    {
        private readonly Guid _examId;
        private readonly Home.CurrentUser _user;
        private readonly DateTime _startAt = DateTime.Now;

        public double Score { get; private set; }

        private Timer _uiTimer;
        private TimeSpan _remain = TimeSpan.Zero;

        // dữ liệu đề
        private List<QuestionDTO> _questions = new List<QuestionDTO>();
        private int _index = 0;

        // lựa chọn của thí sinh theo từng câu
        private readonly Dictionary<Guid, HashSet<Guid>> _selectedByQuestion =
            new Dictionary<Guid, HashSet<Guid>>();

        public ExamRoom(Guid examId, Home.CurrentUser user)
        {
            if (user == null || !string.Equals(user.Role, "STUDENT", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Yêu cầu đăng nhập bằng tài khoản Học viên.");

            InitializeComponent();
            _examId = examId;
            _user = user;
        }

        #region DTO
        private class AnswerDTO
        {
            public Guid AnswerId;
            public string Text;
            public bool IsCorrect;
        }

        private class QuestionDTO
        {
            public int OrderNo;
            public Guid QuestionId;
            public string Content;
            public int Level;
            public string Explanation;
            public readonly List<AnswerDTO> Answers = new List<AnswerDTO>();
        }
        #endregion

        private async void ExamRoom_Load(object sender, EventArgs e)
        {
            try
            {
                SetInfo("Đang tải đề thi...");
                await LoadExamAsync();      // tên đề + thời lượng
                await LoadQuestionsAsync(); // câu hỏi + đáp án

                // bind danh sách câu
                lstQuestions.Items.Clear();
                foreach (var q in _questions.OrderBy(x => x.OrderNo))
                    lstQuestions.Items.Add($"{q.OrderNo}. {TrimText(q.Content, 44)}");

                if (_questions.Count > 0)
                {
                    lstQuestions.SelectedIndex = 0;
                    _index = 0;
                    RenderQuestion();
                }

                StartUiTimer();
                SetInfo("Bắt đầu làm bài.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp đề: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
                Close();
            }
        }

        private void SetInfo(string text)
        {
            try { if (sbInfo != null) sbInfo.Text = text; } catch { /* ignore */ }
        }

        private static string TrimText(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= max ? s : s.Substring(0, max - 1) + "…";
        }

        #region Load data
        private async Task LoadExamAsync()
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"]?.ConnectionString
                     ?? throw new Exception("Thiếu chuỗi kết nối PgConn.");

            const string sql = @"SELECT e.name, e.duration_minutes
                                 FROM exams e
                                 WHERE e.id = @id;";

            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _examId);
                    using (var rd = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (!await rd.ReadAsync())
                            throw new Exception("Không tìm thấy đề thi.");

                        var name = rd.GetString(0);
                        var minutes = rd.GetInt32(1);

                        _remain = TimeSpan.FromMinutes(minutes);
                        lblTitle.Text = name;
                        lblTimer.Text = _remain.ToString(@"hh\:mm\:ss");
                    }
                }
            }
        }

        private async Task LoadQuestionsAsync()
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"]?.ConnectionString
                     ?? throw new Exception("Thiếu chuỗi kết nối PgConn.");

            const string sqlQ = @"
              SELECT eq.order_no, q.id, q.content, q.level, q.explanation
              FROM exam_questions eq
              JOIN questions q ON q.id = eq.question_id
              WHERE eq.exam_id = @examId
              ORDER BY eq.order_no;";

            const string sqlA = @"
              SELECT q.id, a.id, a.content, a.is_correct
              FROM exam_questions eq
              JOIN questions q ON q.id = eq.question_id
              JOIN answers a   ON a.question_id = q.id
              WHERE eq.exam_id = @examId
              ORDER BY eq.order_no, a.id;";

            var list = new List<QuestionDTO>();

            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();

                // câu hỏi
                using (var cmd = new NpgsqlCommand(sqlQ, conn))
                {
                    cmd.Parameters.AddWithValue("@examId", _examId);
                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            list.Add(new QuestionDTO
                            {
                                OrderNo = rd.GetInt32(0),
                                QuestionId = rd.GetGuid(1),
                                Content = rd.GetString(2),
                                Level = rd.IsDBNull(3) ? 0 : rd.GetInt32(3),
                                Explanation = rd.IsDBNull(4) ? null : rd.GetString(4)
                            });
                        }
                    }
                }

                var map = list.ToDictionary(x => x.QuestionId, x => x);

                // đáp án
                using (var cmd = new NpgsqlCommand(sqlA, conn))
                {
                    cmd.Parameters.AddWithValue("@examId", _examId);
                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            var qid = rd.GetGuid(0);
                            QuestionDTO q;
                            if (map.TryGetValue(qid, out q))
                            {
                                q.Answers.Add(new AnswerDTO
                                {
                                    AnswerId = rd.GetGuid(1),
                                    Text = rd.GetString(2),
                                    IsCorrect = rd.GetBoolean(3)
                                });
                            }
                        }
                    }
                }
            }

            _questions = list;
        }
        #endregion

        #region UI rendering & navigation
        private void RenderQuestion()
        {
            if (_index < 0 || _index >= _questions.Count) return;

            var q = _questions[_index];

            // hiển thị nội dung
            if (lblQuestion != null)
                lblQuestion.Text = $"{q.OrderNo}. {q.Content}";

            // render đáp án
            flpAnswers.SuspendLayout();
            flpAnswers.Controls.Clear();

            int correct = q.Answers.Count(a => a.IsCorrect);
            bool multi = correct > 1;

            HashSet<Guid> selected;
            if (!_selectedByQuestion.TryGetValue(q.QuestionId, out selected))
            {
                selected = new HashSet<Guid>();
                _selectedByQuestion[q.QuestionId] = selected;
            }

            if (multi)
            {
                // nhiều đáp án đúng -> CheckBox
                foreach (var a in q.Answers)
                {
                    var chk = new CheckBox
                    {
                        AutoSize = true,
                        MaximumSize = new Size(flpAnswers.Width - 24, 0),
                        Text = a.Text,
                        Tag = a.AnswerId,
                        Checked = selected.Contains(a.AnswerId),
                        Margin = new Padding(8, 6, 8, 6)
                    };
                    chk.CheckedChanged += (s, e) =>
                    {
                        var id = (Guid)((CheckBox)s).Tag;
                        if (((CheckBox)s).Checked) selected.Add(id);
                        else selected.Remove(id);
                    };
                    flpAnswers.Controls.Add(chk);
                }
            }
            else
            {
                // 1 đáp án đúng -> RadioButton
                foreach (var a in q.Answers)
                {
                    var rdo = new RadioButton
                    {
                        AutoSize = true,
                        MaximumSize = new Size(flpAnswers.Width - 24, 0),
                        Text = a.Text,
                        Tag = a.AnswerId,
                        Checked = selected.Contains(a.AnswerId),
                        Margin = new Padding(8, 6, 8, 6)
                    };
                    rdo.CheckedChanged += (s, e) =>
                    {
                        if (((RadioButton)s).Checked)
                        {
                            selected.Clear();
                            selected.Add((Guid)((RadioButton)s).Tag);
                        }
                    };
                    flpAnswers.Controls.Add(rdo);
                }
            }

            flpAnswers.ResumeLayout();

            // cập nhật title form
            this.Text = $"Phòng thi - Câu {_index + 1}/{_questions.Count}";
        }

        private void lstQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstQuestions.SelectedIndex >= 0 && lstQuestions.SelectedIndex < _questions.Count)
            {
                _index = lstQuestions.SelectedIndex;
                RenderQuestion();
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (_index > 0)
            {
                _index--;
                lstQuestions.SelectedIndex = _index;
                RenderQuestion();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_index < _questions.Count - 1)
            {
                _index++;
                lstQuestions.SelectedIndex = _index;
                RenderQuestion();
            }
        }
        #endregion

        #region Timer & submit
        private void StartUiTimer()
        {
            _uiTimer?.Stop();
            _uiTimer = new Timer { Interval = 1000 };
            _uiTimer.Tick += async (s, e) =>
            {
                if (_remain.TotalSeconds > 0)
                {
                    _remain = _remain.Subtract(TimeSpan.FromSeconds(1));
                    lblTimer.Text = _remain.ToString(@"hh\:mm\:ss");
                }
                else
                {
                    _uiTimer.Stop();
                    await SubmitAsync("Hết thời gian!");
                }
            };
            _uiTimer.Start();
        }

        private int CalcRawScore()
        {
            int score = 0;
            foreach (var q in _questions)
            {
                HashSet<Guid> selected;
                _selectedByQuestion.TryGetValue(q.QuestionId, out selected);
                selected = selected ?? new HashSet<Guid>();

                var truth = new HashSet<Guid>(
                    q.Answers.Where(a => a.IsCorrect).Select(a => a.AnswerId));

                if (truth.SetEquals(selected)) score++;
            }
            return score;
        }

        private double CalcScore()
        {
            if (_questions.Count == 0) return 0;
            return Math.Round(10.0 * CalcRawScore() / _questions.Count, 2);
        }

        private async Task SaveAttemptAsync(Guid examId, Guid userId, double score, DateTime startAt, DateTime endAt)
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs)) return;

            const string sql = @"
                INSERT INTO attempts (id, user_id, exam_id, score, started_at, submitted_at)
                VALUES (@id, @uid, @eid, @score, @start, @submit);";

            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@eid", examId);
                    cmd.Parameters.AddWithValue("@score", score);
                    cmd.Parameters.AddWithValue("@start", startAt);
                    cmd.Parameters.AddWithValue("@submit", endAt);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task SubmitAsync(string reason)
        {
            _uiTimer?.Stop();

            var endAt = DateTime.Now;
            Score = CalcScore();
            await SaveAttemptAsync(_examId, _user.Id, Score, _startAt, endAt);

            var raw = CalcRawScore();
            MessageBox.Show(
                $"{reason}\nBạn đạt: {raw}/{_questions.Count} câu đúng\nĐiểm: {Score:0.##}",
                "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Bạn chắc chắn nộp bài?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            await SubmitAsync("Nộp bài");
        }
        #endregion
    }
}
