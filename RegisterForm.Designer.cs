using System.Windows.Forms;

namespace GKOOP
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel tlp;
        private Label lblTitle, lblUser, lblFull, lblPass, lblConfirm, lblHint;
        private TextBox txtUser, txtFull, txtPass, txtConfirm;
        private CheckBox chkShow;
        private FlowLayoutPanel pnlButtons;
        private Button btnOK, btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tlp = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblFull = new System.Windows.Forms.Label();
            this.txtFull = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.chkShow = new System.Windows.Forms.CheckBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tlp.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp
            // 
            this.tlp.ColumnCount = 2;
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp.Controls.Add(this.lblTitle, 0, 0);
            this.tlp.Controls.Add(this.lblUser, 0, 1);
            this.tlp.Controls.Add(this.txtUser, 1, 1);
            this.tlp.Controls.Add(this.lblFull, 0, 2);
            this.tlp.Controls.Add(this.txtFull, 1, 2);
            this.tlp.Controls.Add(this.lblPass, 0, 3);
            this.tlp.Controls.Add(this.txtPass, 1, 3);
            this.tlp.Controls.Add(this.lblConfirm, 0, 4);
            this.tlp.Controls.Add(this.txtConfirm, 1, 4);
            this.tlp.Controls.Add(this.chkShow, 0, 5);
            this.tlp.Controls.Add(this.lblHint, 0, 6);
            this.tlp.Controls.Add(this.pnlButtons, 0, 7);
            this.tlp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp.Location = new System.Drawing.Point(0, 0);
            this.tlp.Name = "tlp";
            this.tlp.Padding = new System.Windows.Forms.Padding(12);
            this.tlp.RowCount = 8;
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp.Size = new System.Drawing.Size(420, 280);
            this.tlp.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.tlp.SetColumnSpan(this.lblTitle, 2);
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(189, 23);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Tạo tài khoản học sinh";
            // 
            // lblUser
            // 
            this.lblUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUser.Location = new System.Drawing.Point(15, 40);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(114, 32);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "Tài khoản";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUser
            // 
            this.txtUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUser.Location = new System.Drawing.Point(135, 43);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(270, 22);
            this.txtUser.TabIndex = 0;
            // 
            // lblFull
            // 
            this.lblFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFull.Location = new System.Drawing.Point(15, 72);
            this.lblFull.Name = "lblFull";
            this.lblFull.Size = new System.Drawing.Size(114, 32);
            this.lblFull.TabIndex = 2;
            this.lblFull.Text = "Họ tên";
            this.lblFull.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFull
            // 
            this.txtFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFull.Location = new System.Drawing.Point(135, 75);
            this.txtFull.Name = "txtFull";
            this.txtFull.Size = new System.Drawing.Size(270, 22);
            this.txtFull.TabIndex = 1;
            // 
            // lblPass
            // 
            this.lblPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPass.Location = new System.Drawing.Point(15, 104);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(114, 32);
            this.lblPass.TabIndex = 3;
            this.lblPass.Text = "Mật khẩu";
            this.lblPass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPass
            // 
            this.txtPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPass.Location = new System.Drawing.Point(135, 107);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(270, 22);
            this.txtPass.TabIndex = 2;
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // lblConfirm
            // 
            this.lblConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConfirm.Location = new System.Drawing.Point(15, 136);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(114, 32);
            this.lblConfirm.TabIndex = 4;
            this.lblConfirm.Text = "Nhập lại mật khẩu";
            this.lblConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtConfirm
            // 
            this.txtConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConfirm.Location = new System.Drawing.Point(135, 139);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(270, 22);
            this.txtConfirm.TabIndex = 3;
            this.txtConfirm.UseSystemPasswordChar = true;
            // 
            // chkShow
            // 
            this.chkShow.AutoSize = true;
            this.tlp.SetColumnSpan(this.chkShow, 2);
            this.chkShow.Location = new System.Drawing.Point(15, 171);
            this.chkShow.Name = "chkShow";
            this.chkShow.Size = new System.Drawing.Size(114, 20);
            this.chkShow.TabIndex = 5;
            this.chkShow.Text = "Hiện mật khẩu";
            this.chkShow.CheckedChanged += new System.EventHandler(this.chkShow_CheckedChanged);
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.tlp.SetColumnSpan(this.lblHint, 2);
            this.lblHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblHint.Location = new System.Drawing.Point(15, 196);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(303, 16);
            this.lblHint.TabIndex = 6;
            this.lblHint.Text = "Yêu cầu: username 4–32 ký tự, mật khẩu 6–50 ký tự.";
            // 
            // pnlButtons
            // 
            this.tlp.SetColumnSpan(this.pnlButtons, 2);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlButtons.Location = new System.Drawing.Point(15, 227);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(390, 38);
            this.pnlButtons.TabIndex = 7;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(287, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Đăng ký";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(191, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Hủy";
            // 
            // RegisterForm
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(420, 280);
            this.Controls.Add(this.tlp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đăng ký tài khoản";
            this.tlp.ResumeLayout(false);
            this.tlp.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
