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
    public partial class TeacherForm : Form
    {
        private readonly Home.CurrentUser _user;

        private DataGridView grd;
        private Button btnAdd, btnEdit, btnDel, btnReload;

        public TeacherForm(Home.CurrentUser user)
        {
            if (user == null || !string.Equals(user.Role, "TEACHER", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Chỉ giáo viên được phép vào khu vực này.");

            _user = user;
            InitializeComponent();
            BuildUi();
        }
        private async void TeacherForm_Load(object sender, EventArgs e)
        {
            
            await LoadExamsAsync();
        }


        private void BuildUi()
        {
            Text = "Quản lý bài thi";
            Width = 900; Height = 560;
            StartPosition = FormStartPosition.CenterParent;

            grd = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = false, AllowUserToAddRows = false };
            grd.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên đề", DataPropertyName = "Name", Width = 260 });
            grd.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Môn", DataPropertyName = "Subject", Width = 180 });
            grd.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số câu", DataPropertyName = "Total", Width = 70 });
            grd.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thời lượng", DataPropertyName = "Duration", Width = 90 });
            grd.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mở", DataPropertyName = "Start", Width = 120 });
            grd.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đóng", DataPropertyName = "End", Width = 120 });

            var panelTop = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 46, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
            btnAdd = new Button { Text = "Thêm", Width = 90 };
            btnEdit = new Button { Text = "Sửa", Width = 90 };
            btnDel = new Button { Text = "Xóa", Width = 90 };
            btnReload = new Button { Text = "Tải lại", Width = 90 };
            panelTop.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDel, btnReload });

            Controls.Add(grd);
            Controls.Add(panelTop);

            btnReload.Click += async (s, e) => await LoadExamsAsync();
            btnAdd.Click += async (s, e) => await AddExamAsync();
            btnEdit.Click += async (s, e) => await EditExamAsync();
            btnDel.Click += async (s, e) => await DeleteExamAsync();

            Shown += async (s, e) => await LoadExamsAsync();
        }

        private class Row
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Subject { get; set; }
            public int Total { get; set; }
            public string Duration { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
        }

        private async Task LoadExamsAsync()
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
            const string sql = @"
                SELECT e.id, e.name, s.name AS subject, e.total_questions, e.duration_minutes,
                       to_char(e.start_time, 'DD/MM HH24:MI') AS st,
                       to_char(e.end_time,   'DD/MM HH24:MI') AS et
                FROM exams e
                JOIN subjects s ON s.id = e.subject_id
                ORDER BY e.name;";

            var rows = new List<Row>();
            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var rd = await cmd.ExecuteReaderAsync())
                {
                    while (await rd.ReadAsync())
                    {
                        rows.Add(new Row
                        {
                            Id = rd.GetGuid(0),
                            Name = rd.GetString(1),
                            Subject = rd.GetString(2),
                            Total = rd.GetInt32(3),
                            Duration = rd.GetInt32(4) + "′",
                            Start = rd.IsDBNull(5) ? "—" : rd.GetString(5),
                            End = rd.IsDBNull(6) ? "—" : rd.GetString(6),
                        });
                    }
                }
            }
            grd.DataSource = rows;
        }

        // ==== ADD ====
        private async Task AddExamAsync()
        {
            // Editor để nhập thông tin và add câu hỏi
            using (var f = new ExamEditorDialog())
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;

                // f.Draft chứa exam + list câu hỏi/đáp án
                await CreateExamWithQuestionsAsync(f.Draft);
                await LoadExamsAsync();
                MessageBox.Show("Đã thêm bài thi.");
            }
        }

        // ==== EDIT ====
        private async Task EditExamAsync()
        {
            if (!(grd.CurrentRow?.DataBoundItem is Row row))
            { MessageBox.Show("Chọn 1 bài thi để sửa."); return; }

            // Load exam chi tiết + câu hỏi hiện có để đổ vào editor
            ExamDraft draft = await LoadExamDraftAsync(row.Id);

            using (var f = new ExamEditorDialog(draft))
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;
                await UpdateExamWithQuestionsAsync(row.Id, f.Draft);
                await LoadExamsAsync();
                MessageBox.Show("Đã cập nhật bài thi.");
            }
        }

        // ==== DELETE ====
        private async Task DeleteExamAsync()
        {
            if (!(grd.CurrentRow?.DataBoundItem is Row row))
            { MessageBox.Show("Chọn 1 bài thi để xóa."); return; }

            var confirm = MessageBox.Show($"Xóa bài thi: {row.Name} ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;

            // Yêu cầu: ON DELETE CASCADE trên exam_questions và attempts
            const string sql = "DELETE FROM exams WHERE id=@id;";
            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", row.Id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            await LoadExamsAsync();
            MessageBox.Show("Đã xóa.");
        }

        // ====== DB helpers ======

        // Tạo bài thi + câu hỏi/đáp án trong 1 transaction
        private async Task CreateExamWithQuestionsAsync(ExamDraft draft)
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();

                using (var tx = conn.BeginTransaction())   // dùng BeginTransaction() đồng bộ
                {
                    try
                    {
                        // 1) Lấy (hoặc tạo) topic mặc định cho subject
                        Guid topicId;
                        using (var cmdFindTopic = new NpgsqlCommand(
                            "SELECT id FROM topics WHERE subject_id = @sid ORDER BY name LIMIT 1;", conn, tx))
                        {
                            cmdFindTopic.Parameters.AddWithValue("@sid", draft.SubjectId);
                            var obj = await cmdFindTopic.ExecuteScalarAsync();
                            if (obj == null || obj == DBNull.Value)
                            {
                                topicId = Guid.NewGuid();
                                using (var cmdNewTopic = new NpgsqlCommand(
                                    "INSERT INTO topics(id, subject_id, name) VALUES(@id, @sid, 'Chung');", conn, tx))
                                {
                                    cmdNewTopic.Parameters.AddWithValue("@id", topicId);
                                    cmdNewTopic.Parameters.AddWithValue("@sid", draft.SubjectId);
                                    await cmdNewTopic.ExecuteNonQueryAsync();
                                }
                            }
                            else
                            {
                                topicId = (Guid)obj;
                            }
                        }

                        // 2) Thêm exam
                        var examId = Guid.NewGuid();
                        using (var cmdE = new NpgsqlCommand(@"
                        INSERT INTO exams
                            (id, subject_id, name, total_questions, level_mix, duration_minutes, start_time, end_time, created_by, created_at)
                        VALUES
                            (@id, @sid, @name, @total, '{}'::jsonb, @dur, NULL, NULL, @uid, now());", conn, tx))
                        {
                            cmdE.Parameters.AddWithValue("@id", examId);
                            cmdE.Parameters.AddWithValue("@sid", draft.SubjectId);
                            cmdE.Parameters.AddWithValue("@name", draft.Name);
                            cmdE.Parameters.AddWithValue("@total", draft.Questions.Count);
                            cmdE.Parameters.AddWithValue("@dur", draft.DurationMinutes);
                            cmdE.Parameters.AddWithValue("@uid", _user.Id);
                            await cmdE.ExecuteNonQueryAsync();
                        }

                        // 3) Thêm questions + answers + mapping exam_questions
                        int ord = 1;
                        foreach (var q in draft.Questions)
                        {
                            var qid = Guid.NewGuid();

                            using (var cmdQ = new NpgsqlCommand(@"
                            INSERT INTO questions
                                (id, topic_id, content, level, explanation, created_by, created_at)
                            VALUES
                                (@id, @topic, @content, 1, NULL, @uid, now());", conn, tx))
                            {
                                cmdQ.Parameters.AddWithValue("@id", qid);
                                cmdQ.Parameters.AddWithValue("@topic", topicId);        // đảm bảo có topic_id
                                cmdQ.Parameters.AddWithValue("@content", q.Content);
                                cmdQ.Parameters.AddWithValue("@uid", _user.Id);
                                await cmdQ.ExecuteNonQueryAsync();
                            }

                            foreach (var a in q.Answers)
                            {
                                var aid = Guid.NewGuid();
                                using (var cmdA = new NpgsqlCommand(@"
                                INSERT INTO answers (id, question_id, content, is_correct)
                                VALUES (@id, @qid, @content, @ok);", conn, tx))
                                {
                                    cmdA.Parameters.AddWithValue("@id", aid);
                                    cmdA.Parameters.AddWithValue("@qid", qid);
                                    cmdA.Parameters.AddWithValue("@content", a.Text);
                                    cmdA.Parameters.AddWithValue("@ok", a.IsCorrect);
                                    await cmdA.ExecuteNonQueryAsync();
                                }
                            }

                            using (var cmdEQ = new NpgsqlCommand(@"
                            INSERT INTO exam_questions (id, exam_id, question_id, order_no)
                            VALUES (@id, @eid, @qid, @ord);", conn, tx))
                            {
                                cmdEQ.Parameters.AddWithValue("@id", Guid.NewGuid());
                                cmdEQ.Parameters.AddWithValue("@eid", examId);
                                cmdEQ.Parameters.AddWithValue("@qid", qid);
                                cmdEQ.Parameters.AddWithValue("@ord", ord++);
                                await cmdEQ.ExecuteNonQueryAsync();
                            }
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        try { tx.Rollback(); } catch { /* ignore */ }
                        throw;
                    }
                }
            }
        }



        // Tải exam + câu hỏi để sửa
        private async Task<ExamDraft> LoadExamDraftAsync(Guid examId)
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
            var draft = new ExamDraft { Questions = new List<QuestionDto>() };

            // exam info
            const string sqlE = @"
                SELECT e.name, e.subject_id, e.duration_minutes
                FROM exams e WHERE e.id=@id;";
            // questions
            const string sqlQ = @"
                SELECT eq.order_no, q.id, q.content
                FROM exam_questions eq JOIN questions q ON q.id = eq.question_id
                WHERE eq.exam_id=@id ORDER BY eq.order_no;";
            // answers
            const string sqlA = @"SELECT a.id, a.question_id, a.content, a.is_correct
                                  FROM answers a WHERE a.question_id = @qid ORDER BY a.id;";

            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();

                using (var cmd = new NpgsqlCommand(sqlE, conn))
                {
                    cmd.Parameters.AddWithValue("@id", examId);
                    using (var rd = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (await rd.ReadAsync())
                        {
                            draft.Name = rd.GetString(0);
                            draft.SubjectId = rd.GetGuid(1);
                            draft.DurationMinutes = rd.GetInt32(2);
                        }
                    }
                }

                var temp = new List<QuestionDto>();
                using (var cmd = new NpgsqlCommand(sqlQ, conn))
                {
                    cmd.Parameters.AddWithValue("@id", examId);
                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            temp.Add(new QuestionDto
                            {
                                Id = rd.GetGuid(1),
                                Content = rd.GetString(2),
                                Answers = new List<AnswerDto>()
                            });
                        }
                    }
                }

                foreach (var q in temp)
                {
                    using (var cmdA = new NpgsqlCommand(sqlA, conn))
                    {
                        cmdA.Parameters.AddWithValue("@qid", q.Id);
                        using (var rd2 = await cmdA.ExecuteReaderAsync())
                        {
                            while (await rd2.ReadAsync())
                            {
                                q.Answers.Add(new AnswerDto
                                {
                                    Id = rd2.GetGuid(0),
                                    Text = rd2.GetString(2),
                                    IsCorrect = rd2.GetBoolean(3)
                                });
                            }
                        }
                    }
                }

                // Gán lại ID=Guid.Empty để khi lưu SỬA, mình xóa & chèn mới cho gọn (dựa trên logic bạn muốn)
                draft.Questions = temp.Select(q => new QuestionDto
                {
                    Id = Guid.Empty,
                    Content = q.Content,
                    Answers = q.Answers.Select(a => new AnswerDto { Id = Guid.Empty, Text = a.Text, IsCorrect = a.IsCorrect }).ToList()
                }).ToList();
            }

            return draft;
        }

        // Cập nhật:

        private async Task UpdateExamWithQuestionsAsync(Guid examId, ExamDraft draft)
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var tx = conn.BeginTransaction())
                {
                    // Xóa answers -> questions -> exam_questions cũ (các câu tạo riêng cho đề)
                    const string sqlDelAns = @"
                        DELETE FROM answers a WHERE a.question_id IN
                        (SELECT q.id FROM exam_questions eq JOIN questions q ON q.id = eq.question_id WHERE eq.exam_id=@eid);";
                    const string sqlDelQ = @"
                        DELETE FROM questions q WHERE q.id IN
                        (SELECT eq.question_id FROM exam_questions eq WHERE eq.exam_id=@eid);";
                    const string sqlDelEQ = @"DELETE FROM exam_questions WHERE exam_id=@eid;";

                    using (var cmd = new NpgsqlCommand(sqlDelAns, conn, tx)) { cmd.Parameters.AddWithValue("@eid", examId); await cmd.ExecuteNonQueryAsync(); }
                    using (var cmd = new NpgsqlCommand(sqlDelQ, conn, tx)) { cmd.Parameters.AddWithValue("@eid", examId); await cmd.ExecuteNonQueryAsync(); }
                    using (var cmd = new NpgsqlCommand(sqlDelEQ, conn, tx)) { cmd.Parameters.AddWithValue("@eid", examId); await cmd.ExecuteNonQueryAsync(); }

                    // Update exams info
                    const string sqlUpdateExam = @"
                        UPDATE exams SET subject_id=@sid, name=@name, duration_minutes=@dur, total_questions=@total
                        WHERE id=@id;";
                    using (var cmd = new NpgsqlCommand(sqlUpdateExam, conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@sid", draft.SubjectId);
                        cmd.Parameters.AddWithValue("@name", draft.Name);
                        cmd.Parameters.AddWithValue("@dur", draft.DurationMinutes);
                        cmd.Parameters.AddWithValue("@total", draft.Questions.Count);
                        cmd.Parameters.AddWithValue("@id", examId);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    // Ghi lại bộ câu hỏi như khi Create
                    int order = 1;
                    foreach (var q in draft.Questions)
                    {
                        var qid = Guid.NewGuid();
                        const string sqlQ = @"INSERT INTO questions (id,topic_id, content, level, explanation, created_by, created_at)
                                              VALUES (@id,@tid, @content, 1, NULL, @uid, now());";
                        using (var cmdQ = new NpgsqlCommand(sqlQ, conn, tx))
                        {
                            cmdQ.Parameters.AddWithValue("@id", qid);
                            cmdQ.Parameters.AddWithValue("@tid", q.TopicId);
                            cmdQ.Parameters.AddWithValue("@content", q.Content);
                            cmdQ.Parameters.AddWithValue("@uid", _user.Id);
                            await cmdQ.ExecuteNonQueryAsync();
                        }

                        foreach (var a in q.Answers)
                        {
                            var aid = Guid.NewGuid();
                            const string sqlA = @"INSERT INTO answers (id, question_id, content, is_correct)
                                                  VALUES (@id, @qid, @content, @correct);";
                            using (var cmdA = new NpgsqlCommand(sqlA, conn, tx))
                            {
                                cmdA.Parameters.AddWithValue("@id", aid);
                                cmdA.Parameters.AddWithValue("@qid", qid);
                                cmdA.Parameters.AddWithValue("@content", a.Text);
                                cmdA.Parameters.AddWithValue("@correct", a.IsCorrect);
                                await cmdA.ExecuteNonQueryAsync();
                            }
                        }

                        const string sqlEQ = @"INSERT INTO exam_questions (id, exam_id, question_id, order_no)
                                               VALUES (@id, @eid, @qid, @ord);";
                        using (var cmdEQ = new NpgsqlCommand(sqlEQ, conn, tx))
                        {
                            cmdEQ.Parameters.AddWithValue("@id", Guid.NewGuid());
                            cmdEQ.Parameters.AddWithValue("@eid", examId);
                            cmdEQ.Parameters.AddWithValue("@qid", qid);
                            cmdEQ.Parameters.AddWithValue("@ord", order++);
                            await cmdEQ.ExecuteNonQueryAsync();
                        }
                    }

                    await tx.CommitAsync();
                }
            }
        }
    }
}
