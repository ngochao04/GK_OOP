using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace GKOOP
{
    public partial class RegisterForm : Form
    {
        public class CreatedUser
        {
            public Guid Id;
            public string Username;
            public string FullName;
            public string Role;
        }

        public CreatedUser Result { get; private set; }

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            bool show = this.chkShow.Checked;
            this.txtPass.UseSystemPasswordChar = !show;
            this.txtConfirm.UseSystemPasswordChar = !show;
        }

        private async void btnOK_Click(object sender, EventArgs e)
        {
            string u = txtUser.Text.Trim();
            string f = txtFull.Text.Trim();
            string p = txtPass.Text;
            string c = txtConfirm.Text;

            // validate
            if (u.Length < 4 || u.Length > 32) { MessageBox.Show("Tài khoản 4–32 ký tự."); return; }
            if (string.IsNullOrEmpty(f)) f = u;
            if (p.Length < 6 || p.Length > 50) { MessageBox.Show("Mật khẩu 6–50 ký tự."); return; }
            if (p != c) { MessageBox.Show("Nhập lại mật khẩu không khớp."); return; }

            btnOK.Enabled = false; Cursor = Cursors.WaitCursor;

            try
            {
                var connStr = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;

                // 1) kiểm tra trùng
                const string sqlCheck = "SELECT 1 FROM users WHERE username=@u LIMIT 1;";
                // 2) insert user
                const string sqlIns = @"
                    INSERT INTO users (id, username, password_hash, full_name, role, is_active, created_at)
                    VALUES (@id, @u, @h, @f, 'STUDENT', TRUE, now());";

                using (var conn = new NpgsqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    // check
                    using (var cmd = new NpgsqlCommand(sqlCheck, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", u);
                        var exists = await cmd.ExecuteScalarAsync();
                        if (exists != null)
                        {
                            MessageBox.Show("Tài khoản đã tồn tại.");
                            return;
                        }
                    }

                    // hash
                    string hash = BCrypt.Net.BCrypt.HashPassword(p, workFactor: 10);
                    Guid id = Guid.NewGuid();

                    // insert
                    using (var cmd2 = new NpgsqlCommand(sqlIns, conn))
                    {
                        cmd2.Parameters.AddWithValue("@id", id);
                        cmd2.Parameters.AddWithValue("@u", u);
                        cmd2.Parameters.AddWithValue("@h", hash);
                        cmd2.Parameters.AddWithValue("@f", f);
                        await cmd2.ExecuteNonQueryAsync();
                    }

                    this.Result = new CreatedUser { Id = id, Username = u, FullName = f, Role = "STUDENT" };
                }

                MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập ngay.");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng ký: " + ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnOK.Enabled = true;
            }
        }
    }
}
