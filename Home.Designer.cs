using System;
using System.Windows.Forms;
using System.Drawing;

namespace GKOOP
{
    partial class Home
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));

            // ====== ROOT LAYOUT ======
            this.tblRoot = new TableLayoutPanel();
            this.tblRoot.ColumnCount = 3;
            this.tblRoot.RowCount    = 3;
            this.tblRoot.Dock        = DockStyle.Fill;
            this.tblRoot.BackColor   = SystemColors.Control;
            this.tblRoot.Padding     = new Padding(8,16,0,0);

            // Cột: trái 260px | giữa co giãn | phải 320px (rộng hơn để đủ KPI + bảng điểm)
            this.tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260F));
            this.tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent , 100F));
            this.tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
            // Hàng: header 64px | thân co giãn | status 26px
            this.tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            this.tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent , 100F));
            this.tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));

            // ====== HEADER ======
            this.pnlHeader = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8,6,8,6) };
            this.lblTitle  = new Label { Dock = DockStyle.Left, AutoSize = true,
                Font = new Font("Segoe UI", 16.2F, FontStyle.Bold),
                Text = "HỆ THỐNG THI TRẮC NGHIỆM" };

            // Nhóm nút bên phải: không chồng nhau
            this.headerActions = new FlowLayoutPanel {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0), Margin = new Padding(0)
            };
            this.btnLogin        = new Button { Text = "Đăng nhập", AutoSize = true, Margin = new Padding(8,0,0,0) };
            this.btnChangeAvatar = new Button { Text = "Đổi ảnh",     AutoSize = true, Margin = new Padding(8,0,0,0), Visible = false };

            this.headerActions.Controls.Add(this.btnLogin);
            this.headerActions.Controls.Add(this.btnChangeAvatar);

            this.pnlHeader.Controls.Add(this.headerActions);
            this.pnlHeader.Controls.Add(this.lblTitle);

            // ====== USER (TRÁI) ======
            this.grpUser = new GroupBox { Dock = DockStyle.Fill, Text = "Người Dùng" };
            this.tblLeftInfo = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6 };
            this.tblLeftInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tblLeftInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            this.tblLeftInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            this.tblLeftInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            this.tblLeftInfo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.tblLeftInfo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.tblLeftInfo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            this.pictureBox1 = new PictureBox {
                Dock = DockStyle.Fill,
                Image = ((Image)(resources.GetObject("pictureBox1.Image"))),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            this.lblFullName = new Label { AutoSize = true, Font = new Font("Segoe UI", 10.8F, FontStyle.Bold), Text = "Khách" };
            this.lblRole     = new Label { AutoSize = true, Text = "Chưa đăng nhập" };
            this.lblConn     = new Label { AutoSize = true, ForeColor = SystemColors.GrayText, Text = "DB: Unknown" };

            this.tblLeftInfo.Controls.Add(new Panel(), 0, 0);
            this.tblLeftInfo.Controls.Add(this.pictureBox1, 0, 1);
            this.tblLeftInfo.Controls.Add(new Panel(), 0, 2);
            this.tblLeftInfo.Controls.Add(this.lblFullName, 0, 3);
            this.tblLeftInfo.Controls.Add(this.lblRole,     0, 4);
            this.tblLeftInfo.Controls.Add(this.lblConn,     0, 5);
            this.grpUser.Controls.Add(this.tblLeftInfo);

            // ====== CENTER TABS ======
            this.tabMain = new TabControl { Dock = DockStyle.Fill };
            this.tabExams   = new TabPage("Thi Hiện Có");
            this.tabHistory = new TabPage("Lịch Sử");
            this.tabNews    = new TabPage("Thông Báo");
            this.tabMain.TabPages.AddRange(new[] { this.tabExams, this.tabHistory, this.tabNews });

            // ---- Exams tab: panel dưới + grid fill (thứ tự thêm để dock chuẩn) ----
            this.panel1 = new Panel { Dock = DockStyle.Bottom, Height = 46, Padding = new Padding(8) };
            this.btnStartExam = new Button { Anchor = AnchorStyles.Bottom | AnchorStyles.Right, Text = "Bắt đầu", Size = new Size(90,27) };
            this.panel1.Controls.Add(this.btnStartExam);

            this.grdExams = new DataGridView {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, RowHeadersWidth = 25,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            };
            // Cột bài thi
            this.colExamName = new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Bài Thi", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 100 };
            this.colSubject  = new DataGridViewTextBoxColumn { DataPropertyName = "Subject", HeaderText = "Môn", Width = 140 };
            this.colTotal    = new DataGridViewTextBoxColumn { DataPropertyName = "TotalQuestions", HeaderText = "Câu Hỏi", Width = 80 };
            this.colDuration = new DataGridViewTextBoxColumn { DataPropertyName = "Duration", HeaderText = "Thời Lượng", Width = 100 };
            this.grdExams.Columns.AddRange(this.colExamName, this.colSubject, this.colTotal, this.colDuration);

            // Thêm theo thứ tự: Panel dưới trước, rồi Grid fill
            this.tabExams.Controls.Add(this.grdExams);
            this.tabExams.Controls.Add(this.panel1);

            // ---- History tab ----
            this.grdHistory = new DataGridView {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, RowHeadersWidth = 25
            };
            this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Bài Thi", Width = 240 };
            this.colScore   = new DataGridViewTextBoxColumn { DataPropertyName = "Score", HeaderText = "Điểm", Width = 80 };
            this.colStartAt = new DataGridViewTextBoxColumn { DataPropertyName = "StartAt", HeaderText = "Bắt đầu", Width = 140 };
            this.colEndAt   = new DataGridViewTextBoxColumn { DataPropertyName = "EndAt", HeaderText = "Kết thúc", Width = 140 };
            this.grdHistory.Columns.AddRange(this.dataGridViewTextBoxColumn1, this.colScore, this.colStartAt, this.colEndAt);
            this.tabHistory.Controls.Add(this.grdHistory);

            // ---- News tab ----
            this.lstNews = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true };
            this.Title = new ColumnHeader { Text = "Tiêu đề", Width = 360 };
            this.Time  = new ColumnHeader { Text = "Thời gian", Width = 180 };
            this.lstNews.Columns.AddRange(new[] { this.Title, this.Time });
            this.tabNews.Controls.Add(this.lstNews);

            // ====== SUMMARY (PHẢI) ======
            this.grpSummary = new GroupBox { Dock = DockStyle.Fill, Text = "Tổng Quan" };
            this.tblRight   = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(6) };
            this.tblRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F));
            this.tblRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            this.tblRight.RowStyles.Add(new RowStyle(SizeType.Percent , 100F));
            this.grpSummary.Controls.Add(this.tblRight);

            // KPI
            this.tblKpi = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, Padding = new Padding(6) };
            this.tblKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            this.tblKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            this.tblKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            this.lblOpenExams = new Label { AutoSize = true, Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(12), Text = "Bài thi đang mở\r\n—" };
            this.lblMyRank    = new Label { AutoSize = true, Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(12), Text = "Xếp hạng\r\n—" };
            this.lblBestScore = new Label { AutoSize = true, Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(12), Text = "Điểm cao nhất\r\n—" };
            this.tblKpi.Controls.Add(this.lblOpenExams, 0, 0);
            this.tblKpi.Controls.Add(this.lblMyRank   , 1, 0);
            this.tblKpi.Controls.Add(this.lblBestScore, 2, 0);

            // Leaderboard
            this.grpLeaderboard = new GroupBox { Dock = DockStyle.Fill, Text = "Bảng điểm (GV)" };
            this.grdLeaderboard = new DataGridView {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false
            };
            // Cột bảng điểm (khởi tạo 1 lần ở designer)
            this.grdLeaderboard.AutoGenerateColumns = false;
            this.grdLeaderboard.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Rnk", HeaderText = "STT", Width = 50 });
            this.grdLeaderboard.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Username", HeaderText = "Mã SV", Width = 110 });
            this.grdLeaderboard.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Họ tên", Width = 160 });
            this.grdLeaderboard.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Score", HeaderText = "Điểm", Width = 70 });
            this.grdLeaderboard.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "StartAt", HeaderText = "Bắt đầu", Width = 110 });
            this.grdLeaderboard.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EndAt", HeaderText = "Kết thúc", Width = 110 });
            this.grpLeaderboard.Controls.Add(this.grdLeaderboard);

            this.tblRight.Controls.Add(this.tblKpi, 0, 0);
            this.tblRight.Controls.Add(this.grpLeaderboard, 0, 1);

            // ====== STATUS BAR ======
            this.statusStrip1 = new StatusStrip { SizingGrip = false };
            this.sbDB    = new ToolStripStatusLabel("DB");
            this.toolStripStatusLabel1 = new ToolStripStatusLabel("|");
            this.sbUser  = new ToolStripStatusLabel("User:");
            this.toolStripStatusLabel2 = new ToolStripStatusLabel("|");
            this.sbClock = new ToolStripStatusLabel("--:--:--");
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.sbDB, this.toolStripStatusLabel1, this.sbUser, this.toolStripStatusLabel2, this.sbClock });

            // ====== THÊM VÀO ROOT ======
            this.tblRoot.Controls.Add(this.pnlHeader , 0, 0);
            this.tblRoot.SetColumnSpan(this.pnlHeader, 3);    // header phủ 3 cột

            this.tblRoot.Controls.Add(this.grpUser   , 0, 1);
            this.tblRoot.Controls.Add(this.tabMain   , 1, 1);
            this.tblRoot.Controls.Add(this.grpSummary, 2, 1);

            this.tblRoot.Controls.Add(this.statusStrip1, 0, 2);
            this.tblRoot.SetColumnSpan(this.statusStrip1, 3);

            // ====== FORM ======
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize  = new Size(1464, 662);
            this.MinimumSize = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Ứng Dụng Thi Trắc Nghiệm";
            this.Controls.Add(this.tblRoot);

            // ====== SỰ KIỆN ======
            this.Load += new EventHandler(this.Home_Load);
            this.lblOpenExams.Click += new EventHandler(this.lblOpenExams_Click);
            this.grdExams.CellContentClick += new DataGridViewCellEventHandler(this.grdExams_CellContentClick);
        }
        #endregion

        // ===== Fields =====
        private TableLayoutPanel tblRoot;

        private Panel pnlHeader;
        private Label lblTitle;
        private FlowLayoutPanel headerActions; // NEW
        private Button btnLogin;
        private Button btnChangeAvatar;        // NEW

        private GroupBox grpUser;
        private TableLayoutPanel tblLeftInfo;
        private PictureBox pictureBox1;
        private Label lblFullName;
        private Label lblRole;
        private Label lblConn;

        private TabControl tabMain;
        private TabPage tabExams;
        private TabPage tabHistory;
        private TabPage tabNews;

        private Panel panel1;
        private Button btnStartExam;

        private DataGridView grdExams;
        private DataGridViewTextBoxColumn colExamName;
        private DataGridViewTextBoxColumn colSubject;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colDuration;
        private DataGridViewTextBoxColumn colWindow;

        private DataGridView grdHistory;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn colScore;
        private DataGridViewTextBoxColumn colStartAt;
        private DataGridViewTextBoxColumn colEndAt;

        private ListView lstNews;
        private ColumnHeader Title;
        private ColumnHeader Time;

        private GroupBox grpSummary;
        private TableLayoutPanel tblRight;
        private TableLayoutPanel tblKpi;
        private Label lblOpenExams;
        public  Label lblBestScore;
        private Label lblMyRank;

        private GroupBox grdLeaderboardContainer; // (không dùng)
        private GroupBox grpLeaderboard;
        private DataGridView grdLeaderboard;

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel sbDB;
        private ToolStripStatusLabel sbUser;
        private ToolStripStatusLabel sbClock;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
    }
}
