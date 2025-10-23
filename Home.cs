using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using NpgsqlTypes;



namespace GKOOP
{
    public partial class Home : Form
    {
        private Button btnManageExams;
        private Button btnChangeAvatar;
        public class CurrentUser
        {
            public Guid Id; public string Username; public string FullName; public string Role;
        }
        private CurrentUser _user;
        public Home()
        {
            
            InitializeComponent();
            
            btnManageExams = new Button
            {
                Name = "btnManageExams",
                Text = "Quản lý bài thi",
                Dock = DockStyle.Right,
                Width = 120,
                Visible = false
            };
            pnlHeader.Controls.Add(btnManageExams);


            btnManageExams.Click += async (_, __) =>
            {
                using (var f = new TeacherForm(_user))
                {
                    f.ShowDialog(this);
                    await LoadExamsAsync();
                    UpdateKpis();
                }
            };
            BuildHeaderExtras();
            WireEvents();
            StartUiClock();
            ApplyAuthState();
        }
        private void BuildHeaderExtras()
        {
            btnChangeAvatar = new Button
            {
                Name = "btnChangeAvatar",
                Text = "Đổi ảnh",
                Width = 90,
                Dock = DockStyle.Right,
                Visible = false   
            };

           
            pnlHeader.Controls.Add(btnChangeAvatar);


            pictureBox1.Cursor = Cursors.Hand;
        }
        private async Task LoadAvatarAsync(Guid userId)
        {
            try
            {
                var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
                using (var conn = new NpgsqlConnection(cs))
                {
                    await conn.OpenAsync();
                    const string sql = "SELECT avatar FROM users WHERE id=@id;";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        var obj = await cmd.ExecuteScalarAsync();

                        if (obj == null || obj == DBNull.Value)
                        {
                            // ảnh mặc định
                            pictureBox1.Image = SystemIcons.Information.ToBitmap();
                        }
                        else
                        {
                            pictureBox1.Image = BytesToImage((byte[])obj);
                        }
                    }
                }
            }
            catch
            {
                // nếu lỗi, dùng ảnh mặc định để UI không trống
                pictureBox1.Image = SystemIcons.Information.ToBitmap();
            }
        }
        private async Task ChangeAvatarAsync()
        {
            if (_user == null) return;

            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh đại diện";
                ofd.Filter = "Ảnh|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                ofd.Multiselect = false;

                if (ofd.ShowDialog(this) != DialogResult.OK) return;

                // đọc file
                byte[] data = ImageFileToBytes(ofd.FileName);

                // lưu DB
                var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
                using (var conn = new NpgsqlConnection(cs))
                {
                    await conn.OpenAsync();
                    const string sql = "UPDATE users SET avatar=@a WHERE id=@id;";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        var p = cmd.Parameters.Add("@a", NpgsqlDbType.Bytea);
                        p.Value = data;
                        cmd.Parameters.AddWithValue("@id", _user.Id);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // hiển thị lại ngay
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }


        private void WireEvents()
        {
            btnTest.Click += async (s, e) => await TestPgConnectionAsync();
            btnLogin.Click += (s, e) => DoLogin();
            btnStartExam.Click += (s, e) => StartSelectedExam();

            grdExams.SelectionChanged += async (s, e) =>
            await UpdateRankForSelectedExamAsync();
            btnChangeAvatar.Click += async (s, e) => await ChangeAvatarAsync();
            pictureBox1.Click += async (s, e) => await ChangeAvatarAsync();
        }
        private void StartUiClock()
        {
            var t = new System.Windows.Forms.Timer { Interval = 1000 };
            t.Tick += (_, __) => sbClock.Text = DateTime.Now.ToString("HH:mm:ss");
            t.Start();
        }
        private async void Home_Load(object sender, EventArgs e)
        {
            
            await LoadExamsAsync();
            await UpdateRankForSelectedExamAsync();
            UpdateKpis();
        }
        private static Image BytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        private static byte[] ImageFileToBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tblRoot_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grpUser_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
  

        private async Task TestPgConnectionAsync()
        {
            var connStr = ConfigurationManager.ConnectionStrings["PgConn"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("Không thấy chuỗi kết nối 'PgConn' trong App.config", "Lỗi");
                return;
            }

            btnTest.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15)))
                using (var conn = new NpgsqlConnection(connStr))
                {
                    await conn.OpenAsync(cts.Token);

                    using (var cmd = new NpgsqlCommand("select current_database(), current_user, version()", conn))
                    using (var rd = await cmd.ExecuteReaderAsync(cts.Token))
                    {
                        if (await rd.ReadAsync(cts.Token))
                        {
                            var db = rd.GetString(0);
                            var user = rd.GetString(1);
                            var ver = rd.GetString(2);

                            MessageBox.Show($"Kết nối OK!\nDB: {db}\nUser: {user}\n{ver}", "PostgreSQL");
                        
                            sbDB.Text = "DB: Online";
                            lblConn.Text = $"DB: {db} (as {user})";
                            lblConn.ForeColor = System.Drawing.Color.ForestGreen;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối FAIL:\n" + ex.Message, "PostgreSQL");
                sbDB.Text = "DB: Offline";
                lblConn.Text = "DB: Offline";
                lblConn.ForeColor = System.Drawing.Color.Firebrick;
            }
            finally
            {
                Cursor = Cursors.Default;
                btnTest.Enabled = true;
            }
        }
        private void ApplyAuthState()
        {
            if (_user == null)
            {
                lblFullName.Text = "Khách"; lblRole.Text = "Chưa đăng nhập";
                sbUser.Text = "User: Khách";
                btnLogin.Text = "Đăng nhập…";
                btnChangeAvatar.Visible = false;
                pictureBox1.Image = SystemIcons.Information.ToBitmap();
                btnStartExam.Enabled = false;
                if (btnManageExams != null)   
                    btnManageExams.Visible = false;
                return;
            }
            else
            {
                lblFullName.Text = _user.FullName;
                lblRole.Text = _user.Role;
                sbUser.Text = $"User: {_user.Username}";
                btnLogin.Text = "Đăng xuất";
                btnChangeAvatar.Visible = true;
                btnStartExam.Enabled = string.Equals(_user.Role, "STUDENT", StringComparison.OrdinalIgnoreCase);
                btnTest.Visible = !_user.Role.Equals("STUDENT", StringComparison.OrdinalIgnoreCase);

                var isTeacher = _user.Role.Equals("TEACHER", StringComparison.OrdinalIgnoreCase);
                btnManageExams.Visible = isTeacher;
            }
            
        }
        private async void DoLogin()
        {
            if (_user != null)
            {
                _user = null;
                ApplyAuthState();
                lblMyRank.Text = "Xếp hạng\n—";
                return;
            }

            using (var f = new LoginForm())
            {
                if (f.ShowDialog(this) == DialogResult.OK && f.Result != null)
                {
                    _user = new CurrentUser
                    {
                        Id = f.Result.Id,
                        Username = f.Result.Username,
                        FullName = f.Result.FullName,
                        Role = f.Result.Role
                    };
                    ApplyAuthState();
                    await LoadExamsAsync();
                    await LoadAvatarAsync(_user.Id);
                    await UpdateRankForSelectedExamAsync();
                }
            }

        }

        private class ExamRow
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Subject { get; set; }
            public int TotalQuestions { get; set; }
            public string Duration { get; set; }
            public string Window { get; set; }
        }

        private async Task LoadExamsAsync()
        {
            var connStr = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
            const string sql = @"
    SELECT e.id,
           e.name AS ""Name"",
           s.name AS ""Subject"",
           e.total_questions AS ""TotalQuestions"",
           (e.duration_minutes || '''') AS ""Duration"",
           COALESCE(to_char(e.start_time,'DD/MM HH24:MI'),'—') || ' – ' ||
           COALESCE(to_char(e.end_time  ,'DD/MM HH24:MI'),'—') AS ""Window""
    FROM exams e
    JOIN subjects s ON s.id = e.subject_id
    WHERE (e.start_time IS NULL OR now() >= e.start_time)
      AND (e.end_time   IS NULL OR now() <= e.end_time)
    ORDER BY e.start_time NULLS LAST, e.name;";

            using (var conn = new NpgsqlConnection(connStr))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var rd = await cmd.ExecuteReaderAsync())
                {
                    var rows = new List<ExamRow>();
                    while (await rd.ReadAsync())
                    {
                        rows.Add(new ExamRow
                        {
                            Id = rd.GetGuid(0),
                            Name = rd.GetString(1),
                            Subject = rd.GetString(2),
                            TotalQuestions = rd.GetInt32(3),
                            Duration = rd.GetString(4),
                            Window = rd.GetString(5)
                        });
                    }

                    grdExams.AutoGenerateColumns = false;
                    grdExams.DataSource = rows;
                }
            }
        }
        private class HistoryRow
        {
            public string Name { get; set; }
            public double Score { get; set; }
            public DateTime StartAt { get; set; }
            public DateTime EndAt { get; set; }
        }

        private async Task LoadHistoryAsync()
        {
            if (_user == null) { grdHistory.DataSource = null; return; }

            var cs = ConfigurationManager.ConnectionStrings["PgConn"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs)) return;

            const string sql = @"
        SELECT e.name,
               a.score,
               a.started_at,
               a.submitted_at
        FROM attempts a
        JOIN exams e ON e.id = a.exam_id
        WHERE a.user_id = @uid
        ORDER BY a.submitted_at DESC NULLS LAST, a.started_at DESC;";

            var rows = new List<HistoryRow>();
            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", _user.Id);
                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            rows.Add(new HistoryRow
                            {
                                Name = rd.GetString(0),
                                Score = rd.IsDBNull(1) ? 0 : rd.GetDouble(1),
                                StartAt = rd.IsDBNull(2) ? DateTime.MinValue : rd.GetDateTime(2),
                                EndAt = rd.IsDBNull(3) ? DateTime.MinValue : rd.GetDateTime(3)
                            });
                        }
                    }
                }
            }

            grdHistory.AutoGenerateColumns = false;   // bạn đã cấu hình cột sẵn
            grdHistory.DataSource = rows;             // cột "Điểm" nhớ đặt Name = "colScore"
        }


        private async void StartSelectedExam()
        {

            if (_user == null || !_user.Role.Equals("STUDENT", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Bạn cần đăng nhập bằng tài khoản Học viên để bắt đầu thi.");
                return;
            }
            if (!(grdExams.CurrentRow?.DataBoundItem is ExamRow row))
            {
                MessageBox.Show("Chọn một bài thi trước.");
                return;
            }
            var examId = row.Id; 
            MessageBox.Show($"Bắt đầu: {row.Name}\nExamId: {examId}");

            using (var f = new ExamRoom(row.Id, _user))
            {
                var dlg = f.ShowDialog(this);
                if (dlg == DialogResult.OK)
                {
                    await LoadHistoryAsync();   
                    UpdateKpis();
                    await UpdateRankForSelectedExamAsync();
                }
            }

        }


        private void UpdateKpis()
        {
           
            var openExams = grdExams.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
            lblOpenExams.Text = $"Bài thi đang mở\n{openExams}";

           
            double best = 0;
            var scoreCol = grdHistory.Columns["colScore"];
            if (scoreCol != null)
            {
                best = grdHistory.Rows.Cast<DataGridViewRow>()
                    .Where(r => !r.IsNewRow && r.Cells[scoreCol.Index].Value != null)
                    .Select(r =>
                    {
                        var s = r.Cells[scoreCol.Index].Value.ToString();
                        return double.TryParse(s, out var v) ? v : 0.0;
                    })
                    .DefaultIfEmpty(0.0)
                    .Max();
            }
            lblBestScore.Text = $"Điểm cao nhất\n{(best > 0 ? best.ToString("0.##") : "—")}";

            
            lblMyRank.Text = "Xếp hạng\n—";
        }


        private void lblOpenExams_Click(object sender, EventArgs e)
        {

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {

        }

        private void btnSetting_Click_1(object sender, EventArgs e)
        {

        }
        // 1) Tính hạng của user trong 1 đề thi
        private async Task<(int? rank, int total, double? myScore)>
            GetRankForExamAsync(Guid examId, Guid userId)
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
            const string sql = @"
WITH best AS (
  SELECT user_id, MAX(score) AS best_score
  FROM attempts
  WHERE exam_id = @eid
  GROUP BY user_id
),
ranked AS (
  SELECT user_id, best_score,
         DENSE_RANK() OVER (ORDER BY best_score DESC) AS rnk,
         COUNT(*)     OVER ()                          AS total
  FROM best
)
SELECT rnk, total, best_score
FROM ranked
WHERE user_id = @uid;";

            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@eid", examId);
                    cmd.Parameters.AddWithValue("@uid", userId);

                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        if (await rd.ReadAsync())
                            return (rd.GetInt32(0), rd.GetInt32(1), rd.GetDouble(2));
                    }
                }
            }
            return (null, 0, null);
        }

        // 2) Cập nhật label KPI “Xếp hạng” theo đề đang chọn
        private async Task UpdateRankForSelectedExamAsync()
        {
            // mặc định
            lblMyRank.Text = "Xếp hạng\n—";
            if (_user == null) return; // chưa đăng nhập

            if (grdExams.CurrentRow?.DataBoundItem is ExamRow row)
            {
                var (rank, total, myScore) =
                    await GetRankForExamAsync(row.Id, _user.Id);

                lblMyRank.Text = rank.HasValue
                    ? $"Xếp hạng\n{rank}/{total}"
                    : "Xếp hạng\nChưa thi";
            }
        }

    }
}
