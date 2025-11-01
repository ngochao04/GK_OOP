using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace GKOOP
{
    public sealed class AdminDashboardForm : Form
    {
        private readonly Home.CurrentUser _user;

        // UI
        private TabControl tabs;
        private DataGridView grdUsers, grdSubjects, grdNews;
        private Button btnAddSubj, btnEditSubj, btnDelSubj, btnReloadSubj;
        private Button btnReloadUser;

        // News buttons
        private Button btnAddNews, btnEditNews, btnDelNews, btnReloadNews;

        public AdminDashboardForm(Home.CurrentUser user)
        {
            if (user == null || !string.Equals(user.Role, "ADMIN", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Chỉ ADMIN được phép vào khu vực này.");

            _user = user;

            BuildUi();
            WireEvents();
        }

        #region UI
        private void BuildUi()
        {
            Text = "Bảng điều khiển Admin";
            Width = 1000;
            Height = 640;
            StartPosition = FormStartPosition.CenterParent;

            tabs = new TabControl { Dock = DockStyle.Fill };
            Controls.Add(tabs);

            // ===== Tab Users =====
            var pageUsers = new TabPage("Người dùng");
            var pnlUserTop = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 44,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(8)
            };
            btnReloadUser = new Button { Text = "Tải lại", Width = 90 };
            pnlUserTop.Controls.Add(btnReloadUser);

            grdUsers = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false
            };
            grdUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Username",
                DataPropertyName = "Username",
                Width = 160
            });
            grdUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Họ tên",
                DataPropertyName = "FullName",
                Width = 240
            });
            grdUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quyền",
                DataPropertyName = "Role",
                Width = 100
            });
            grdUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tạo ngày",
                DataPropertyName = "CreatedAt",
                Width = 140
            });

            pageUsers.Controls.Add(grdUsers);
            pageUsers.Controls.Add(pnlUserTop);
            tabs.TabPages.Add(pageUsers);

            // ===== Tab Subjects =====
            var pageSubj = new TabPage("Môn học");
            var pnlSubjTop = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 44,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(8)
            };
            btnReloadSubj = new Button { Text = "Tải lại", Width = 90 };
            btnDelSubj = new Button { Text = "Xóa", Width = 90 };
            btnEditSubj = new Button { Text = "Sửa", Width = 90 };
            btnAddSubj = new Button { Text = "Thêm", Width = 90 };
            pnlSubjTop.Controls.AddRange(new Control[] { btnReloadSubj, btnDelSubj, btnEditSubj, btnAddSubj });

            grdSubjects = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            grdSubjects.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên môn",
                DataPropertyName = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            pageSubj.Controls.Add(grdSubjects);
            pageSubj.Controls.Add(pnlSubjTop);
            tabs.TabPages.Add(pageSubj);

            // ===== Tab News (CRUD) =====
            var pageNews = new TabPage("Thông báo");
            var pnlNewsTop = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 44,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(8)
            };
            btnReloadNews = new Button { Text = "Tải lại", Width = 90 };
            btnDelNews = new Button { Text = "Xóa", Width = 90 };
            btnEditNews = new Button { Text = "Sửa", Width = 90 };
            btnAddNews = new Button { Text = "Thêm", Width = 90 };
            pnlNewsTop.Controls.AddRange(new Control[] { btnReloadNews, btnDelNews, btnEditNews, btnAddNews });

            grdNews = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            grdNews.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tiêu đề",
                DataPropertyName = "Title",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            grdNews.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Thời gian",
                DataPropertyName = "PublishedAt",
                Width = 160
            });

            pageNews.Controls.Add(grdNews);
            pageNews.Controls.Add(pnlNewsTop);
            tabs.TabPages.Add(pageNews);

            // Tải dữ liệu lần đầu
            Shown += async (_, __) =>
            {
                await LoadUsersAsync();
                await LoadSubjectsAsync();
                await LoadNewsAsync();
            };
        }

        private void WireEvents()
        {
            // Users
            btnReloadUser.Click += async (_, __) => await LoadUsersAsync();

            // Subjects
            btnReloadSubj.Click += async (_, __) => await LoadSubjectsAsync();
            btnAddSubj.Click += async (_, __) => await AddSubjectAsync();
            btnEditSubj.Click += async (_, __) => await EditSubjectAsync();
            btnDelSubj.Click += async (_, __) => await DeleteSubjectAsync();
            grdSubjects.CellDoubleClick += async (_, __) => await EditSubjectAsync();

            // News
            btnReloadNews.Click += async (_, __) => await LoadNewsAsync();
            btnAddNews.Click += async (_, __) => await AddNewsAsync();
            btnEditNews.Click += async (_, __) => await EditNewsAsync();
            btnDelNews.Click += async (_, __) => await DeleteNewsAsync();
            grdNews.CellDoubleClick += async (_, __) => await EditNewsAsync();
        }
        #endregion

        #region DTO
        private sealed class SubjectRow
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        private sealed class NewsRow
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public DateTime? PublishedAtRaw { get; set; }

            public string PublishedAt => PublishedAtRaw.HasValue
                ? PublishedAtRaw.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
                : "—";
        }
        #endregion

        #region ConnStr
        private static string CS
        {
            get
            {
                string cs = ConfigurationManager.ConnectionStrings["PgConn"]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(cs))
                    throw new InvalidOperationException("Chưa cấu hình chuỗi kết nối 'PgConn' trong app.config.");
                return cs;
            }
        }
        #endregion

        #region Loaders
        private async Task LoadUsersAsync()
        {
            const string sql = @"SELECT id, username, full_name, role, created_at
                                 FROM users
                                 ORDER BY created_at DESC;";
            var rows = new List<object>();

            using (NpgsqlConnection conn = new NpgsqlConnection(CS))
            {
                await conn.OpenAsync();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                using (NpgsqlDataReader rd = await cmd.ExecuteReaderAsync())
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
            const string sql = @"SELECT id, name
                                 FROM subjects
                                 ORDER BY name;";
            var rows = new List<SubjectRow>();

            using (NpgsqlConnection conn = new NpgsqlConnection(CS))
            {
                await conn.OpenAsync();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                using (NpgsqlDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (await rd.ReadAsync())
                    {
                        rows.Add(new SubjectRow
                        {
                            Id = rd.GetGuid(0),
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
            // BẢNG news: id, title, published_at
            const string sql = @"SELECT id, title, published_at
                                 FROM news
                                 ORDER BY published_at DESC NULLS LAST, id DESC;";
            var rows = new List<NewsRow>();

            using (NpgsqlConnection conn = new NpgsqlConnection(CS))
            {
                await conn.OpenAsync();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                using (NpgsqlDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (await rd.ReadAsync())
                    {
                        rows.Add(new NewsRow
                        {
                            Id = rd.GetGuid(0),
                            Title = rd.GetString(1),
                            PublishedAtRaw = rd.IsDBNull(2) ? (DateTime?)null : rd.GetDateTime(2)
                        });
                    }
                }
            }

            grdNews.AutoGenerateColumns = false;
            grdNews.DataSource = rows;
        }
        #endregion

        #region Helpers
        private SubjectRow GetSelectedSubject()
        {
            if (grdSubjects.CurrentRow == null) return null;
            return grdSubjects.CurrentRow.DataBoundItem as SubjectRow;
        }

        private NewsRow GetSelectedNews()
        {
            if (grdNews.CurrentRow == null) return null;
            return grdNews.CurrentRow.DataBoundItem as NewsRow;
        }
        #endregion

        #region CRUD: Subjects
        private async Task AddSubjectAsync()
        {
            using (SubjectEditDialog d = new SubjectEditDialog("Thêm môn học"))
            {
                if (d.ShowDialog(this) != DialogResult.OK) return;

                if (string.IsNullOrWhiteSpace(d.SubjectName))
                {
                    MessageBox.Show("Tên môn không được trống.");
                    return;
                }

                const string sql = "INSERT INTO subjects(id, name) VALUES(@id, @name);";

                using (NpgsqlConnection conn = new NpgsqlConnection(CS))
                {
                    await conn.OpenAsync();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@name", d.SubjectName.Trim());
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await LoadSubjectsAsync();
            }
        }

        private async Task EditSubjectAsync()
        {
            SubjectRow cur = GetSelectedSubject();
            if (cur == null)
            {
                MessageBox.Show("Chọn một môn để sửa.");
                return;
            }

            using (SubjectEditDialog d = new SubjectEditDialog("Sửa môn học", cur.Name))
            {
                if (d.ShowDialog(this) != DialogResult.OK) return;

                if (string.IsNullOrWhiteSpace(d.SubjectName))
                {
                    MessageBox.Show("Tên môn không được trống.");
                    return;
                }

                const string sql = "UPDATE subjects SET name=@name WHERE id=@id;";

                using (NpgsqlConnection conn = new NpgsqlConnection(CS))
                {
                    await conn.OpenAsync();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", cur.Id);
                        cmd.Parameters.AddWithValue("@name", d.SubjectName.Trim());
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await LoadSubjectsAsync();
            }
        }

        private async Task DeleteSubjectAsync()
        {
            SubjectRow cur = GetSelectedSubject();
            if (cur == null)
            {
                MessageBox.Show("Chọn một môn để xóa.");
                return;
            }

            DialogResult confirm = MessageBox.Show($"Xóa môn: {cur.Name} ?", "Xác nhận",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            const string sql = "DELETE FROM subjects WHERE id=@id;";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(CS))
                {
                    await conn.OpenAsync();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", cur.Id);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await LoadSubjectsAsync();
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                MessageBox.Show("Không thể xóa vì đang được tham chiếu (ví dụ: bài thi đang dùng môn này).");
            }
        }
        #endregion

        #region CRUD: News (id, title, published_at)
        private async Task AddNewsAsync()
        {
            using (NewsEditForm d = new NewsEditForm())
            {
                if (d.ShowDialog(this) != DialogResult.OK) return;

                if (string.IsNullOrWhiteSpace(d.TitleText))
                {
                    MessageBox.Show("Tiêu đề không được trống.");
                    return;
                }

                const string sql = @"INSERT INTO news(id, title, published_at)
                                     VALUES(@id, @t, @p);";

                using (NpgsqlConnection conn = new NpgsqlConnection(CS))
                {
                    await conn.OpenAsync();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@t", d.TitleText.Trim());
                        if (d.PublishedAt.HasValue)
                            cmd.Parameters.AddWithValue("@p", d.PublishedAt.Value.ToUniversalTime());
                        else
                            cmd.Parameters.AddWithValue("@p", DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await LoadNewsAsync();
            }
        }

        private async Task EditNewsAsync()
        {
            NewsRow cur = GetSelectedNews();
            if (cur == null)
            {
                MessageBox.Show("Chọn một thông báo để sửa.");
                return;
            }

            string title = null;
            DateTime? pub = null;

            const string get = @"SELECT title, published_at FROM news WHERE id=@id;";
            using (NpgsqlConnection conn = new NpgsqlConnection(CS))
            {
                await conn.OpenAsync();
                using (NpgsqlCommand cmd = new NpgsqlCommand(get, conn))
                {
                    cmd.Parameters.AddWithValue("@id", cur.Id);
                    using (NpgsqlDataReader rd = await cmd.ExecuteReaderAsync())
                    {
                        if (!await rd.ReadAsync())
                        {
                            MessageBox.Show("Không tìm thấy bản ghi.");
                            return;
                        }
                        title = rd.GetString(0);
                        pub = rd.IsDBNull(1) ? (DateTime?)null : rd.GetDateTime(1).ToLocalTime();
                    }
                }
            }

            using (NewsEditForm d = new NewsEditForm(title, pub))
            {
                if (d.ShowDialog(this) != DialogResult.OK) return;

                if (string.IsNullOrWhiteSpace(d.TitleText))
                {
                    MessageBox.Show("Tiêu đề không được trống.");
                    return;
                }

                const string upd = @"UPDATE news
                                     SET title=@t, published_at=@p
                                     WHERE id=@id;";

                using (NpgsqlConnection conn = new NpgsqlConnection(CS))
                {
                    await conn.OpenAsync();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(upd, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", cur.Id);
                        cmd.Parameters.AddWithValue("@t", d.TitleText.Trim());
                        if (d.PublishedAt.HasValue)
                            cmd.Parameters.AddWithValue("@p", d.PublishedAt.Value.ToUniversalTime());
                        else
                            cmd.Parameters.AddWithValue("@p", DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await LoadNewsAsync();
            }
        }

        private async Task DeleteNewsAsync()
        {
            NewsRow cur = GetSelectedNews();
            if (cur == null)
            {
                MessageBox.Show("Chọn một thông báo để xóa.");
                return;
            }

            DialogResult confirm = MessageBox.Show($"Xóa thông báo:\n“{cur.Title}” ?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            const string sql = "DELETE FROM news WHERE id=@id;";

            using (NpgsqlConnection conn = new NpgsqlConnection(CS))
            {
                await conn.OpenAsync();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", cur.Id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            await LoadNewsAsync();
        }
        #endregion

        #region Dialogs
        private sealed class SubjectEditDialog : Form
        {
            private TextBox txtName;
            private Button btnOk, btnCancel;
            public string SubjectName { get { return txtName.Text; } }

            public SubjectEditDialog(string title, string init = "")
            {
                Text = title;
                Width = 420;
                Height = 160;
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;

                Label lbl = new Label { Text = "Tên môn:", Left = 12, Top = 18, AutoSize = true };
                txtName = new TextBox { Left = 80, Top = 14, Width = 300, Text = init ?? "" };

                btnOk = new Button { Text = "OK", Left = 226, Width = 72, Top = 60, DialogResult = DialogResult.OK };
                btnCancel = new Button { Text = "Hủy", Left = 308, Width = 72, Top = 60, DialogResult = DialogResult.Cancel };

                Controls.AddRange(new Control[] { lbl, txtName, btnOk, btnCancel });
                AcceptButton = btnOk;
                CancelButton = btnCancel;
            }
        }

        private sealed class NewsEditForm : Form
        {
            private TextBox txtTitle;
            private DateTimePicker dtpPublished;
            private Button btnOk, btnCancel;

            public string TitleText { get { return txtTitle.Text; } }
            public DateTime? PublishedAt
            {
                get { return dtpPublished.Checked ? (DateTime?)dtpPublished.Value : null; }
            }

            public NewsEditForm(string title = "", DateTime? publishedAt = null)
            {
                Text = string.IsNullOrEmpty(title) ? "Thêm thông báo" : "Sửa thông báo";
                Width = 540; Height = 200;
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false; MinimizeBox = false;

                Label lblT = new Label { Text = "Tiêu đề:", Left = 12, Top = 18, AutoSize = true };
                txtTitle = new TextBox { Left = 80, Top = 14, Width = 420, Text = title ?? "" };

                Label lblP = new Label { Text = "Thời gian đăng:", Left = 12, Top = 54, AutoSize = true };
                dtpPublished = new DateTimePicker
                {
                    Left = 110,
                    Top = 50,
                    Width = 200,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy HH:mm",
                    ShowCheckBox = true
                };
                if (publishedAt.HasValue)
                {
                    dtpPublished.Value = publishedAt.Value;
                    dtpPublished.Checked = true;
                }
                else
                {
                    dtpPublished.Value = DateTime.Now;
                    dtpPublished.Checked = false;
                }

                btnOk = new Button { Text = "OK", Left = 338, Width = 72, Top = 100, DialogResult = DialogResult.OK };
                btnCancel = new Button { Text = "Hủy", Left = 420, Width = 72, Top = 100, DialogResult = DialogResult.Cancel };

                Controls.AddRange(new Control[] { lblT, txtTitle, lblP, dtpPublished, btnOk, btnCancel });

                AcceptButton = btnOk;
                CancelButton = btnCancel;
            }
        }
        #endregion
    }
}
