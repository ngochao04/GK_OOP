namespace GKOOP
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>I
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.tblRoot = new System.Windows.Forms.TableLayoutPanel();
            this.grpSummary = new System.Windows.Forms.GroupBox();
            this.tblRightKpi = new System.Windows.Forms.TableLayoutPanel();
            this.lblOpenExams = new System.Windows.Forms.Label();
            this.lblBestScore = new System.Windows.Forms.Label();
            this.lblMyRank = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpUser = new System.Windows.Forms.GroupBox();
            this.tblLeftInfo = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblConn = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabExams = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStartExam = new System.Windows.Forms.Button();
            this.grdExams = new System.Windows.Forms.DataGridView();
            this.colExamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWindow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.grdHistory = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabNews = new System.Windows.Forms.TabPage();
            this.lstNews = new System.Windows.Forms.ListView();
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sbDB = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbClock = new System.Windows.Forms.ToolStripStatusLabel();
            this.tblRoot.SuspendLayout();
            this.grpSummary.SuspendLayout();
            this.tblRightKpi.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.grpUser.SuspendLayout();
            this.tblLeftInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tabExams.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExams)).BeginInit();
            this.tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            this.tabNews.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblRoot
            // 
            this.tblRoot.AutoSize = true;
            this.tblRoot.ColumnCount = 3;
            this.tblRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tblRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tblRoot.Controls.Add(this.grpSummary, 2, 1);
            this.tblRoot.Controls.Add(this.pnlHeader, 0, 0);
            this.tblRoot.Controls.Add(this.grpUser, 0, 1);
            this.tblRoot.Controls.Add(this.tabMain, 1, 1);
            this.tblRoot.Controls.Add(this.statusStrip1, 0, 2);
            this.tblRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblRoot.Location = new System.Drawing.Point(0, 0);
            this.tblRoot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tblRoot.Name = "tblRoot";
            this.tblRoot.Padding = new System.Windows.Forms.Padding(8, 16, 0, 0);
            this.tblRoot.RowCount = 3;
            this.tblRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tblRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tblRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblRoot.Size = new System.Drawing.Size(1164, 672);
            this.tblRoot.TabIndex = 0;
            this.tblRoot.Paint += new System.Windows.Forms.PaintEventHandler(this.tblRoot_Paint);
            // 
            // grpSummary
            // 
            this.grpSummary.Controls.Add(this.tblRightKpi);
            this.grpSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSummary.Location = new System.Drawing.Point(867, 83);
            this.grpSummary.Name = "grpSummary";
            this.grpSummary.Size = new System.Drawing.Size(294, 560);
            this.grpSummary.TabIndex = 4;
            this.grpSummary.TabStop = false;
            this.grpSummary.Text = "Tổng Quan";
            // 
            // tblRightKpi
            // 
            this.tblRightKpi.ColumnCount = 2;
            this.tblRightKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRightKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRightKpi.Controls.Add(this.lblOpenExams, 0, 0);
            this.tblRightKpi.Controls.Add(this.lblBestScore, 1, 0);
            this.tblRightKpi.Controls.Add(this.lblMyRank, 0, 1);
            this.tblRightKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblRightKpi.Location = new System.Drawing.Point(3, 18);
            this.tblRightKpi.Name = "tblRightKpi";
            this.tblRightKpi.Padding = new System.Windows.Forms.Padding(8);
            this.tblRightKpi.RowCount = 6;
            this.tblRightKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRightKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRightKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblRightKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblRightKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblRightKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblRightKpi.Size = new System.Drawing.Size(288, 539);
            this.tblRightKpi.TabIndex = 0;
            // 
            // lblOpenExams
            // 
            this.lblOpenExams.AutoSize = true;
            this.lblOpenExams.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOpenExams.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOpenExams.Location = new System.Drawing.Point(11, 8);
            this.lblOpenExams.Name = "lblOpenExams";
            this.lblOpenExams.Padding = new System.Windows.Forms.Padding(12);
            this.lblOpenExams.Size = new System.Drawing.Size(130, 58);
            this.lblOpenExams.TabIndex = 0;
            this.lblOpenExams.Text = "Bài Thi Đang Mở";
            this.lblOpenExams.Click += new System.EventHandler(this.lblOpenExams_Click);
            // 
            // lblBestScore
            // 
            this.lblBestScore.AutoSize = true;
            this.lblBestScore.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBestScore.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBestScore.Location = new System.Drawing.Point(147, 8);
            this.lblBestScore.Name = "lblBestScore";
            this.lblBestScore.Padding = new System.Windows.Forms.Padding(12);
            this.lblBestScore.Size = new System.Drawing.Size(130, 42);
            this.lblBestScore.TabIndex = 1;
            this.lblBestScore.Text = "Điểm Cao Nhất";
            // 
            // lblMyRank
            // 
            this.lblMyRank.AutoSize = true;
            this.lblMyRank.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMyRank.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMyRank.Location = new System.Drawing.Point(11, 229);
            this.lblMyRank.Name = "lblMyRank";
            this.lblMyRank.Padding = new System.Windows.Forms.Padding(12);
            this.lblMyRank.Size = new System.Drawing.Size(130, 42);
            this.lblMyRank.TabIndex = 2;
            this.lblMyRank.Text = "Xếp Hạng";
            // 
            // pnlHeader
            // 
            this.tblRoot.SetColumnSpan(this.pnlHeader, 3);
            this.pnlHeader.Controls.Add(this.btnTest);
            this.pnlHeader.Controls.Add(this.btnLogin);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(11, 18);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1150, 60);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // btnTest
            // 
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTest.Location = new System.Drawing.Point(934, 0);
            this.btnTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(96, 60);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "TestDB";
            this.btnTest.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLogin.Location = new System.Drawing.Point(1030, 0);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(120, 60);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Đăng Nhập";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(416, 38);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HỆ THỐNG THI TRẮC NGHIỆM";
            // 
            // grpUser
            // 
            this.grpUser.Controls.Add(this.tblLeftInfo);
            this.grpUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpUser.Location = new System.Drawing.Point(11, 82);
            this.grpUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpUser.Name = "grpUser";
            this.grpUser.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpUser.Size = new System.Drawing.Size(254, 562);
            this.grpUser.TabIndex = 1;
            this.grpUser.TabStop = false;
            this.grpUser.Text = "Người Dùng";
            this.grpUser.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // tblLeftInfo
            // 
            this.tblLeftInfo.ColumnCount = 1;
            this.tblLeftInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLeftInfo.Controls.Add(this.pictureBox1, 0, 1);
            this.tblLeftInfo.Controls.Add(this.lblFullName, 0, 3);
            this.tblLeftInfo.Controls.Add(this.lblRole, 0, 4);
            this.tblLeftInfo.Controls.Add(this.lblConn, 0, 5);
            this.tblLeftInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLeftInfo.Location = new System.Drawing.Point(3, 17);
            this.tblLeftInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tblLeftInfo.Name = "tblLeftInfo";
            this.tblLeftInfo.RowCount = 6;
            this.tblLeftInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tblLeftInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tblLeftInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tblLeftInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLeftInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLeftInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLeftInfo.Size = new System.Drawing.Size(248, 543);
            this.tblLeftInfo.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(224, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.Location = new System.Drawing.Point(3, 116);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(65, 25);
            this.lblFullName.TabIndex = 1;
            this.lblFullName.Text = "Khách";
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(3, 141);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(109, 16);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "Chưa Đăng Nhập";
            // 
            // lblConn
            // 
            this.lblConn.AutoSize = true;
            this.lblConn.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblConn.Location = new System.Drawing.Point(3, 157);
            this.lblConn.Name = "lblConn";
            this.lblConn.Size = new System.Drawing.Size(80, 16);
            this.lblConn.TabIndex = 3;
            this.lblConn.Text = "DB: Unkown";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabExams);
            this.tabMain.Controls.Add(this.tabHistory);
            this.tabMain.Controls.Add(this.tabNews);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(271, 83);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(590, 560);
            this.tabMain.TabIndex = 2;
            // 
            // tabExams
            // 
            this.tabExams.Controls.Add(this.panel1);
            this.tabExams.Controls.Add(this.grdExams);
            this.tabExams.Location = new System.Drawing.Point(4, 25);
            this.tabExams.Name = "tabExams";
            this.tabExams.Padding = new System.Windows.Forms.Padding(3);
            this.tabExams.Size = new System.Drawing.Size(582, 531);
            this.tabExams.TabIndex = 0;
            this.tabExams.Text = "Thi Hiện Có";
            this.tabExams.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStartExam);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 482);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8);
            this.panel1.Size = new System.Drawing.Size(576, 46);
            this.panel1.TabIndex = 1;
            // 
            // btnStartExam
            // 
            this.btnStartExam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartExam.Location = new System.Drawing.Point(456, 11);
            this.btnStartExam.Name = "btnStartExam";
            this.btnStartExam.Size = new System.Drawing.Size(75, 23);
            this.btnStartExam.TabIndex = 0;
            this.btnStartExam.Text = "Bắt Đầu";
            this.btnStartExam.UseVisualStyleBackColor = true;
            // 
            // grdExams
            // 
            this.grdExams.AllowUserToAddRows = false;
            this.grdExams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdExams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colExamName,
            this.colSubject,
            this.colTotal,
            this.colDuration,
            this.colWindow});
            this.grdExams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExams.Location = new System.Drawing.Point(3, 3);
            this.grdExams.Name = "grdExams";
            this.grdExams.ReadOnly = true;
            this.grdExams.RowHeadersWidth = 51;
            this.grdExams.RowTemplate.Height = 24;
            this.grdExams.Size = new System.Drawing.Size(576, 525);
            this.grdExams.TabIndex = 0;
            // 
            // colExamName
            // 
            this.colExamName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colExamName.DataPropertyName = "Name";
            this.colExamName.HeaderText = "Bài Thi";
            this.colExamName.MinimumWidth = 6;
            this.colExamName.Name = "colExamName";
            this.colExamName.ReadOnly = true;
            // 
            // colSubject
            // 
            this.colSubject.DataPropertyName = "Subject";
            this.colSubject.HeaderText = "Môn";
            this.colSubject.MinimumWidth = 6;
            this.colSubject.Name = "colSubject";
            this.colSubject.ReadOnly = true;
            this.colSubject.Width = 120;
            // 
            // colTotal
            // 
            this.colTotal.DataPropertyName = "TotalQuestions";
            this.colTotal.HeaderText = "Câu Hỏi";
            this.colTotal.MinimumWidth = 6;
            this.colTotal.Name = "colTotal";
            this.colTotal.ReadOnly = true;
            this.colTotal.Width = 80;
            // 
            // colDuration
            // 
            this.colDuration.DataPropertyName = "Duration";
            this.colDuration.HeaderText = "Thời Lượng";
            this.colDuration.MinimumWidth = 6;
            this.colDuration.Name = "colDuration";
            this.colDuration.ReadOnly = true;
            this.colDuration.Width = 90;
            // 
            // colWindow
            // 
            this.colWindow.DataPropertyName = "Window";
            this.colWindow.HeaderText = "Thời Gian Mở";
            this.colWindow.MinimumWidth = 6;
            this.colWindow.Name = "colWindow";
            this.colWindow.ReadOnly = true;
            this.colWindow.Width = 125;
            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(this.grdHistory);
            this.tabHistory.Location = new System.Drawing.Point(4, 25);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabHistory.Size = new System.Drawing.Size(582, 531);
            this.tabHistory.TabIndex = 1;
            this.tabHistory.Text = "Lịch Sử";
            this.tabHistory.UseVisualStyleBackColor = true;
            // 
            // grdHistory
            // 
            this.grdHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.colScore,
            this.colStartAt,
            this.colEndAt});
            this.grdHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdHistory.Location = new System.Drawing.Point(3, 3);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.ReadOnly = true;
            this.grdHistory.RowHeadersWidth = 51;
            this.grdHistory.RowTemplate.Height = 24;
            this.grdHistory.Size = new System.Drawing.Size(576, 525);
            this.grdHistory.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "Bài Thi";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 240;
            // 
            // colScore
            // 
            this.colScore.DataPropertyName = "Score";
            this.colScore.HeaderText = "Điểm";
            this.colScore.MinimumWidth = 6;
            this.colScore.Name = "colScore";
            this.colScore.ReadOnly = true;
            this.colScore.Width = 80;
            // 
            // colStartAt
            // 
            this.colStartAt.DataPropertyName = "StartAt";
            this.colStartAt.HeaderText = "Bắt Đầu";
            this.colStartAt.MinimumWidth = 6;
            this.colStartAt.Name = "colStartAt";
            this.colStartAt.ReadOnly = true;
            this.colStartAt.Width = 140;
            // 
            // colEndAt
            // 
            this.colEndAt.DataPropertyName = "EndAt";
            this.colEndAt.HeaderText = "Kết thúc";
            this.colEndAt.MinimumWidth = 6;
            this.colEndAt.Name = "colEndAt";
            this.colEndAt.ReadOnly = true;
            this.colEndAt.Width = 140;
            // 
            // tabNews
            // 
            this.tabNews.Controls.Add(this.lstNews);
            this.tabNews.Location = new System.Drawing.Point(4, 25);
            this.tabNews.Name = "tabNews";
            this.tabNews.Padding = new System.Windows.Forms.Padding(3);
            this.tabNews.Size = new System.Drawing.Size(582, 531);
            this.tabNews.TabIndex = 2;
            this.tabNews.Text = "Thông Báo";
            this.tabNews.UseVisualStyleBackColor = true;
            // 
            // lstNews
            // 
            this.lstNews.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title,
            this.Time});
            this.lstNews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNews.FullRowSelect = true;
            this.lstNews.HideSelection = false;
            this.lstNews.Location = new System.Drawing.Point(3, 3);
            this.lstNews.Name = "lstNews";
            this.lstNews.Size = new System.Drawing.Size(576, 525);
            this.lstNews.TabIndex = 0;
            this.lstNews.UseCompatibleStateImageBehavior = false;
            this.lstNews.View = System.Windows.Forms.View.Details;
            // 
            // Title
            // 
            this.Title.Text = "Tiêu Đề";
            this.Title.Width = 300;
            // 
            // Time
            // 
            this.Time.Text = "Thời Gian";
            this.Time.Width = 160;
            // 
            // statusStrip1
            // 
            this.tblRoot.SetColumnSpan(this.statusStrip1, 3);
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbDB,
            this.toolStripStatusLabel1,
            this.sbUser,
            this.toolStripStatusLabel2,
            this.sbClock});
            this.statusStrip1.Location = new System.Drawing.Point(8, 646);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1156, 26);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // sbDB
            // 
            this.sbDB.Name = "sbDB";
            this.sbDB.Size = new System.Drawing.Size(29, 20);
            this.sbDB.Text = "DB";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(13, 20);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // sbUser
            // 
            this.sbUser.Name = "sbUser";
            this.sbUser.Size = new System.Drawing.Size(41, 20);
            this.sbUser.Text = "User:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(13, 20);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // sbClock
            // 
            this.sbClock.Name = "sbClock";
            this.sbClock.Size = new System.Drawing.Size(51, 20);
            this.sbClock.Text = "--:--:--";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 672);
            this.Controls.Add(this.tblRoot);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Home";
            this.Text = "Ứng Dụng Thi Trắc Nghiệm";
            this.Load += new System.EventHandler(this.Home_Load);
            this.tblRoot.ResumeLayout(false);
            this.tblRoot.PerformLayout();
            this.grpSummary.ResumeLayout(false);
            this.tblRightKpi.ResumeLayout(false);
            this.tblRightKpi.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.grpUser.ResumeLayout(false);
            this.tblLeftInfo.ResumeLayout(false);
            this.tblLeftInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabExams.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExams)).EndInit();
            this.tabHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            this.tabNews.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblRoot;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.GroupBox grpUser;
        private System.Windows.Forms.TableLayoutPanel tblLeftInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblConn;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabExams;
        private System.Windows.Forms.DataGridView grdExams;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.TabPage tabNews;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStartExam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWindow;
        private System.Windows.Forms.DataGridView grdHistory;
        private System.Windows.Forms.ListView lstNews;
        private System.Windows.Forms.GroupBox grpSummary;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.TableLayoutPanel tblRightKpi;
        private System.Windows.Forms.Label lblOpenExams;
        public System.Windows.Forms.Label lblBestScore;
        private System.Windows.Forms.Label lblMyRank;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel sbDB;
        private System.Windows.Forms.ToolStripStatusLabel sbUser;
        private System.Windows.Forms.ToolStripStatusLabel sbClock;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndAt;

    }
}

