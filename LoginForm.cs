using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using BCrypt.Net;

namespace GKOOP
{
    public partial class LoginForm : Form
    {
        public class UserDto
        {
            public Guid Id { get; }
            public string Username { get; }
            public string FullName { get; }
            public string Role { get; }

            public UserDto(Guid id, string username, string fullName, string role)
            {
                Id = id; Username = username; FullName = fullName; Role = role;
            }
        }
        public UserDto Result { get; private set; }
       

        public LoginForm() 
        { 
            InitializeComponent();
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !chkShow.Checked;
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var f = new RegisterForm())
            {
                if (f.ShowDialog(this) == DialogResult.OK && f.Result != null)
                {
                    
                    txtUser.Text = f.Result.Username;
                    txtPass.Focus();
                }
            }
        }

        private async void btnOK_Click(object sender, EventArgs e)
        {
            string u = txtUser.Text.Trim();
            string p = txtPass.Text;

            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu.");
                return;
            }

            btnOK.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                var connStr = System.Configuration.ConfigurationManager
                                 .ConnectionStrings["PgConn"].ConnectionString;

                const string sql = @"
            SELECT id, username, COALESCE(full_name, username) AS full_name,
                   role::text AS role_text, password_hash, is_active
            FROM users
            WHERE username = @u
            LIMIT 1;";

                using (var conn = new Npgsql.NpgsqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", u);

                        var rd = await cmd.ExecuteReaderAsync();
                        using (rd)
                        {
                            if (!await rd.ReadAsync())
                            {
                                MessageBox.Show("Sai tài khoản hoặc mật khẩu.");
                                return;
                            }

                            var id = rd.GetGuid(0);
                            var uname = rd.GetString(1);
                            var full = rd.GetString(2);
                            var role = rd.GetString(3);
                            var hash = rd.GetString(4);
                            var active = rd.GetBoolean(5);

                            if (!active)
                            {
                                MessageBox.Show("Tài khoản đã bị khóa.");
                                return;
                            }

                            // So sánh BCrypt (cần NuGet: BCrypt.Net-Next hoặc BCrypt.Net-StrongName)
                            if (!BCrypt.Net.BCrypt.Verify(p, hash))
                            {
                                MessageBox.Show("Sai tài khoản hoặc mật khẩu.");
                                return;
                            }

                            // Thành công: gán kết quả trả về cho Home
                            Result = new UserDto(id, uname, full, role);
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnOK.Enabled = true;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
