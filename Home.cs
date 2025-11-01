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
using NpgsqlTypes;

namespace GKOOP
{
    public partial class Home : Form
    {
        private Button btnManageExams;
        private Button btnAdmin;

        public class CurrentUser
        {
            public Guid Id; public string Username; public string FullName; public string Role;
        }
        private CurrentUser _user;

        public Home()
        {
            InitializeComponent();

            // Nút vào khu giáo viên
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
                    await LoadHistoryAsync();
                    await UpdateRankForSelectedExamAsync();
                    await LoadLeaderboardForSelectedExamAsync();
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
            btnAdmin = new Button
            {
                Name = "btnAdmin",
                Text = "Quản trị",
                Dock = DockStyle.Right,
                Width = 100,
                Visible = false
            };
            pnlHeader.Controls.Add(btnAdmin);

            btnAdmin.Click += (_, __) =>
            {
                using (var f = new AdminDashboardForm(_user))
                    f.ShowDialog(this);
            };

            pictureBox1.Cursor = Cursors.Hand;
        }

        #region Avatar
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

                        pictureBox1.Image = (obj == null || obj == DBNull.Value)
                            ? SystemIcons.Information.ToBitmap()
                            : BytesToImage((byte[])obj);
                    }
                }
            }
            catch
            {
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

                byte[] data = File.ReadAllBytes(ofd.FileName);

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
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }
        #endregion

        private void WireEvents()
        {
            btnLogin.Click += (s, e) => DoLogin();
            btnStartExam.Click += (s, e) => StartSelectedExam();

            grdExams.SelectionChanged += async (s, e) =>
            {
                await UpdateRankForSelectedExamAsync();
                await LoadLeaderboardForSelectedExamAsync();
            };
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
            await LoadHistoryAsync();
            await UpdateRankForSelectedExamAsync();
            await LoadLeaderboardForSelectedExamAsync();
            await LoadNewsListAsync();
            UpdateKpis();
        }

        private static Image BytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            using (var ms = new MemoryStream(bytes))
                return Image.FromStream(ms);
        }

        #region Misc empty handlers (VS designer)
        private void pnlHeader_Paint(object sender, PaintEventArgs e) { }
        private void tblRoot_Paint(object sender, PaintEventArgs e) { }
        private void grpUser_Enter(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void lblOpenExams_Click(object sender, EventArgs e) { }
        private void btnSetting_Click(object sender, EventArgs e) { }
        private void btnSetting_Click_1(object sender, EventArgs e) { }
        #endregion

        #region DB utilities
      
        #endregion

        #region Auth state
        private void ApplyAuthState()
        {
            if (_user == null)
            {
                lblFullName.Text = "Khách"; lblRole.Text = "Chưa đăng nhập";
                sbUser.Text = "User: Khách";
                btnLogin.Text = "Đăng nhập…";
                pictureBox1.Image = SystemIcons.Information.ToBitmap();
                btnStartExam.Enabled = false;
                if (btnManageExams != null) btnManageExams.Visible = false;
                if (btnAdmin != null) btnAdmin.Visible = false;
                // Ẩn bảng điểm nếu chưa đăng nhập / không phải GV
                if (grpLeaderboard != null) grpLeaderboard.Visible = false;
                if (grdLeaderboard != null) grdLeaderboard.DataSource = null;

                return;
            }

            lblFullName.Text = _user.FullName;
            lblRole.Text = _user.Role;
            sbUser.Text = $"User: {_user.Username}";
            btnLogin.Text = "Đăng xuất";
            btnStartExam.Enabled = string.Equals(_user.Role, "STUDENT", StringComparison.OrdinalIgnoreCase);

            var isTeacher = _user.Role.Equals("TEACHER", StringComparison.OrdinalIgnoreCase);
            btnManageExams.Visible = isTeacher;
            var isAdmin = _user != null && _user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);
            if (btnAdmin != null) btnAdmin.Visible = isAdmin;

            // Bảng điểm chỉ dành cho GV ad
            bool canSeeLeaderboard = isTeacher || isAdmin;
            grpLeaderboard.Visible = canSeeLeaderboard;
            if (!canSeeLeaderboard && grdLeaderboard != null) grdLeaderboard.DataSource = null;
        }

        private async void DoLogin()
        {
            if (_user != null)
            {
                _user = null;
                ApplyAuthState();
                lblMyRank.Text = "Xếp hạng\n—";
                await LoadHistoryAsync();
                UpdateKpis();
                await LoadNewsListAsync();
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

                    await LoadAvatarAsync(_user.Id);
                    await LoadExamsAsync();
                    await LoadHistoryAsync();
                    await UpdateRankForSelectedExamAsync();
                    await LoadLeaderboardForSelectedExamAsync();
                    await LoadNewsListAsync();
                    UpdateKpis();
                }
            }
        }
        #endregion

        #region Load exams & history
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
        // Thông Báo
        private sealed class NewsListItem
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public DateTime? PublishedAt { get; set; }
        }

        private async Task LoadNewsListAsync()
        {
            lstNews.BeginUpdate();
            lstNews.Items.Clear();

            var cs = ConfigurationManager.ConnectionStrings["PgConn"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
            {
                lstNews.EndUpdate();
                return;
            }

            const string sql = @"
        SELECT id, title, published_at
        FROM news
        ORDER BY published_at DESC NULLS LAST, id DESC;";

            var rows = new List<NewsListItem>();
            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var rd = await cmd.ExecuteReaderAsync())
                {
                    while (await rd.ReadAsync())
                    {
                        rows.Add(new NewsListItem
                        {
                            Id = rd.GetGuid(0),
                            Title = rd.GetString(1),
                            PublishedAt = rd.IsDBNull(2) ? (DateTime?)null : rd.GetDateTime(2)
                        });
                    }
                }
            }

            foreach (var r in rows)
            {
                var timeText = r.PublishedAt.HasValue
                    ? r.PublishedAt.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
                    : "—";

                var item = new ListViewItem(r.Title);
                item.SubItems.Add(timeText);
                item.Tag = r.Id; // nếu sau này cần mở chi tiết
                lstNews.Items.Add(item);
            }

            // auto-size cột tiêu đề vừa khít (tuỳ chọn)
            if (lstNews.Columns.Count >= 2)
            {
                lstNews.Columns[0].Width = -2; // AutoSize
                lstNews.Columns[1].Width = 160;
            }

            lstNews.EndUpdate();
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

            grdHistory.AutoGenerateColumns = false;
            grdHistory.DataSource = rows;
        }
        #endregion

        #region Start exam
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

            using (var f = new ExamRoom(row.Id, _user))
            {
                var dlg = f.ShowDialog(this);
                if (dlg == DialogResult.OK)
                {
                    await LoadHistoryAsync();
                    UpdateKpis();
                    await UpdateRankForSelectedExamAsync();
                    await LoadLeaderboardForSelectedExamAsync();
                }
            }
        }
        #endregion

        #region KPI + Rank + Leaderboard
        private void UpdateKpis()
        {
            // số bài thi mở
            var openExams = grdExams.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
            lblOpenExams.Text = $"Bài thi đang mở\n{openExams}";

            // --- Điểm cao nhất ---
            double best = 0;
            if (grdHistory.DataSource is IEnumerable<HistoryRow> history)
            {
                best = history
                    .Where(h => h.Score > 0)
                    .Select(h => h.Score)
                    .DefaultIfEmpty(0)
                    .Max();
            }

            lblBestScore.Text = $"Điểm cao nhất\n{(best > 0 ? best.ToString("0.##") : "—")}";

            // Xếp hạng giữ nguyên
            if (!lblMyRank.Text.StartsWith("Xếp hạng"))
                lblMyRank.Text = "Xếp hạng\n—";
        }


        // 1) Lấy hạng của user trong 1 đề thi
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

        // 2) Update label KPI “Xếp hạng” theo đề đang chọn
        private async Task UpdateRankForSelectedExamAsync()
        {
            lblMyRank.Text = "Xếp hạng\n—";
            if (_user == null) return; // chưa đăng nhập

            if (grdExams.CurrentRow?.DataBoundItem is ExamRow row)
            {
                var (rank, total, _) = await GetRankForExamAsync(row.Id, _user.Id);
                lblMyRank.Text = rank.HasValue
                    ? $"Xếp hạng\n{rank}/{total}"
                    : "Xếp hạng\nChưa thi";
            }
        }

        // 3) Bảng điểm cho giáo viên
        private class LeaderboardRow
        {
            public int Rnk { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public double Score { get; set; }
            public string StartAt { get; set; } // dùng làm 'Thời gian' hiển thị
            public string EndAt { get; set; }   // để trống cho khớp cột
        }

        private async Task LoadLeaderboardForSelectedExamAsync()
        {
            if (grpLeaderboard == null || grdLeaderboard == null) return;

            // Chỉ GV nhìn thấy
            bool canSeeLeaderboard = _user != null &&
        (_user.Role.Equals("TEACHER", StringComparison.OrdinalIgnoreCase) ||
         _user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase));

            grpLeaderboard.Visible = canSeeLeaderboard;
            if (!canSeeLeaderboard) { grdLeaderboard.DataSource = null; return; }

            if (!(grdExams.CurrentRow?.DataBoundItem is ExamRow row))
            { grdLeaderboard.DataSource = null; return; }

            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
            const string sql = @"
WITH best AS (
    SELECT user_id, MAX(score) AS best_score
    FROM attempts
    WHERE exam_id = @eid
    GROUP BY user_id
),
last_time AS (
    SELECT a.user_id, MAX(a.submitted_at) AS last_submit
    FROM attempts a
    JOIN best b ON b.user_id = a.user_id AND b.best_score = a.score
    WHERE a.exam_id = @eid
    GROUP BY a.user_id
)
SELECT DENSE_RANK() OVER (ORDER BY b.best_score DESC) AS rnk,
       u.username, u.full_name,
       b.best_score,
       to_char(l.last_submit, 'DD/MM HH24:MI') AS last_submit
FROM best b
JOIN users u ON u.id = b.user_id
LEFT JOIN last_time l ON l.user_id = b.user_id
ORDER BY rnk, u.username;";

            var rows = new List<LeaderboardRow>();
            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@eid", row.Id);
                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            rows.Add(new LeaderboardRow
                            {
                                Rnk = rd.GetInt32(0),
                                Username = rd.IsDBNull(1) ? "" : rd.GetString(1),
                                Name = rd.IsDBNull(2) ? "" : rd.GetString(2),
                                Score = rd.IsDBNull(3) ? 0 : rd.GetDouble(3),
                                StartAt = rd.IsDBNull(4) ? "—" : rd.GetString(4),
                                EndAt = "" // cột trống cho khớp schema grid
                            });
                        }
                    }
                }
            }

            grdLeaderboard.AutoGenerateColumns = false;
            grdLeaderboard.DataSource = rows;
        }
        #endregion

        private void grdExams_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
