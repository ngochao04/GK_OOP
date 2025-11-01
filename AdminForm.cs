using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using Npgsql;
using System.Threading.Tasks;

namespace GKOOP
{
    public partial class AdminForm : Form
    {
        private readonly Home.CurrentUser _user;
        private TabControl tabs;
        private DataGridView grdUsers, grdSubjects, grdNews;
        private Button btnAddUser, btnEditUser, btnDelUser, btnReloadUser;
        private Button btnAddSubj, btnEditSubj, btnDelSubj, btnReloadSubj;

        private sealed class SubjectRow
        {
            public Guid Id { get; set; }
            public string Code { get; set; }   // giữ thuộc tính chỉ để bind rỗng (nếu lưới đã tạo cột)
            public string Name { get; set; }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {

        }

        private Button btnAddNews, btnEditNews, btnDelNews, btnReloadNews;

        public AdminForm(Home.CurrentUser user)
        {
            if (user == null || !string.Equals(user.Role, "ADMIN", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Chỉ ADMIN được phép vào khu vực này.");

            _user = user;
            InitializeComponent();
            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Bảng điều khiển Admin";
            Width = 1000; Height = 640; StartPosition = FormStartPosition.CenterParent;

            tabs = new TabControl { Dock = DockStyle.Fill };
            Controls.Add(tabs);

            // ===== Users =====
            var pageUsers = new TabPage("Người dùng");
            var panelUserTop = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
            btnAddUser = new Button { Text = "Thêm", Width = 90 };
            btnEditUser = new Button { Text = "Sửa", Width = 90 };
            btnDelUser = new Button { Text = "Xóa", Width = 90 };
            btnReloadUser = new Button { Text = "Tải lại", Width = 90 };
            panelUserTop.Controls.AddRange(new Control[] { btnAddUser, btnEditUser, btnDelUser, btnReloadUser });

            grdUsers = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = false, AllowUserToAddRows = false };
            grdUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Username", DataPropertyName = "Username", Width = 160 });
            grdUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Họ tên", DataPropertyName = "FullName", Width = 240 });
            grdUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Quyền", DataPropertyName = "Role", Width = 100 });
            grdUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tạo ngày", DataPropertyName = "CreatedAt", Width = 140 });

            pageUsers.Controls.Add(grdUsers);
            pageUsers.Controls.Add(panelUserTop);
            tabs.TabPages.Add(pageUsers);

            // ===== Subjects =====
            var pageSubj = new TabPage("Môn học");
            var panelSubjTop = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
            btnAddSubj = new Button { Text = "Thêm", Width = 90 };
            btnEditSubj = new Button { Text = "Sửa", Width = 90 };
            btnDelSubj = new Button { Text = "Xóa", Width = 90 };
            btnReloadSubj = new Button { Text = "Tải lại", Width = 90 };
            panelSubjTop.Controls.AddRange(new Control[] { btnAddSubj, btnEditSubj, btnDelSubj, btnReloadSubj });

            grdSubjects = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = false, AllowUserToAddRows = false };
            grdSubjects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã", DataPropertyName = "Code", Width = 100 });
            grdSubjects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên môn", DataPropertyName = "Name", Width = 280 });

            pageSubj.Controls.Add(grdSubjects);
            pageSubj.Controls.Add(panelSubjTop);
            tabs.TabPages.Add(pageSubj);

            // ===== News =====
            var pageNews = new TabPage("Thông báo");
            var panelNewsTop = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
            btnAddNews = new Button { Text = "Thêm", Width = 90 };
            btnEditNews = new Button { Text = "Sửa", Width = 90 };
            btnDelNews = new Button { Text = "Xóa", Width = 90 };
            btnReloadNews = new Button { Text = "Tải lại", Width = 90 };
            panelNewsTop.Controls.AddRange(new Control[] { btnAddNews, btnEditNews, btnDelNews, btnReloadNews });

            grdNews = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = false, AllowUserToAddRows = false };
            grdNews.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tiêu đề", DataPropertyName = "Title", Width = 320 });
            grdNews.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thời gian", DataPropertyName = "PublishedAt", Width = 160 });

            pageNews.Controls.Add(grdNews);
            pageNews.Controls.Add(panelNewsTop);
            tabs.TabPages.Add(pageNews);

            // events
            Shown += async (_, __) =>
            {
                await LoadUsersAsync();
                await LoadSubjectsAsync();
                await LoadNewsAsync();
            };

            btnReloadUser.Click += async (_, __) => await LoadUsersAsync();
            btnReloadSubj.Click += async (_, __) => await LoadSubjectsAsync();
            btnReloadNews.Click += async (_, __) => await LoadNewsAsync();

            // TODO: gắn handler btnAdd/Edit/Del để mở dialog thêm/sửa/xóa (phase sau)
        }

        // ====== Loaders ======
        private async Task LoadUsersAsync()
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
            const string sql = @"SELECT id, username, full_name, role, created_at FROM users ORDER BY created_at DESC;";
            var rows = new List<dynamic>();
            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var rd = await cmd.ExecuteReaderAsync())
                {
                    while (await rd.ReadAsync())
                    {
                        rows.Add(new
                        {
                            Id = rd.GetGuid(0),
                            Username = rd.GetString(1),
                            FullName = rd.IsDBNull(2) ? "" : rd.GetString(2),
                            Role = rd.GetString(3),
                            CreatedAt = rd.GetDateTime(4).ToString("dd/MM/yyyy HH:mm")
                        });
                    }
                }
            }
            grdUsers.DataSource = rows;
        }

        private async Task LoadSubjectsAsync()
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
            const string sql = @"SELECT id, name FROM subjects ORDER BY name;";

            var rows = new List<SubjectRow>();
            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var rd = await cmd.ExecuteReaderAsync())
                {
                    while (await rd.ReadAsync())
                    {
                        rows.Add(new SubjectRow
                        {
                            Id = rd.GetGuid(0),
                            // Nếu UI đang có cột Code, gán rỗng để khỏi lỗi binding:
                            Code = string.Empty,
                            Name = rd.GetString(1)
                        });
                    }
                }
            }
            grdSubjects.AutoGenerateColumns = false;
            grdSubjects.DataSource = rows;
        }

        private async Task LoadNewsAsync()
        {
            var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
            const string sql = @"SELECT id, title, published_at FROM news ORDER BY published_at DESC NULLS LAST;";
            var rows = new List<dynamic>();
            using (var conn = new NpgsqlConnection(cs))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var rd = await cmd.ExecuteReaderAsync())
                {
                    while (await rd.ReadAsync())
                    {
                        rows.Add(new
                        {
                            Id = rd.GetGuid(0),
                            Title = rd.GetString(1),
                            PublishedAt = rd.IsDBNull(2) ? "—" : rd.GetDateTime(2).ToString("dd/MM/yyyy HH:mm")
                        });
                    }
                }
            }
            grdNews.DataSource = rows;
        }
    }
}
