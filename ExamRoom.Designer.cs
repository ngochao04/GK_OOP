namespace GKOOP
{
    partial class ExamRoom
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            { components.Dispose(); }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tblRoot = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.split = new System.Windows.Forms.SplitContainer();
            this.lstQuestions = new System.Windows.Forms.ListBox();
            this.pnlQuestion = new System.Windows.Forms.Panel();
            this.flpAnswers = new System.Windows.Forms.FlowLayoutPanel();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.StatusStrip();
            this.sbInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tblRoot.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.pnlQuestion.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.status.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblRoot
            // 
            this.tblRoot.ColumnCount = 1;
            this.tblRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblRoot.Controls.Add(this.pnlHeader, 0, 0);
            this.tblRoot.Controls.Add(this.split, 0, 1);
            this.tblRoot.Controls.Add(this.pnlBottom, 0, 2);
            this.tblRoot.Controls.Add(this.status, 0, 3);
            this.tblRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblRoot.Location = new System.Drawing.Point(0, 0);
            this.tblRoot.Name = "tblRoot";
            this.tblRoot.RowCount = 4;
            this.tblRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tblRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tblRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblRoot.Size = new System.Drawing.Size(1161, 573);
            this.tblRoot.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblTimer);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(3, 3);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1155, 50);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTimer
            // 
            this.lblTimer.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTimer.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTimer.Location = new System.Drawing.Point(925, 0);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Padding = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblTimer.Size = new System.Drawing.Size(230, 50);
            this.lblTimer.TabIndex = 1;
            this.lblTimer.Text = "00:00:00";
            this.lblTimer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(650, 50);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Bài thi";
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Location = new System.Drawing.Point(3, 59);
            this.split.Name = "split";
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.lstQuestions);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.pnlQuestion);
            this.split.Size = new System.Drawing.Size(1155, 431);
            this.split.SplitterDistance = 260;
            this.split.TabIndex = 1;
            // 
            // lstQuestions
            // 
            this.lstQuestions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstQuestions.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lstQuestions.IntegralHeight = false;
            this.lstQuestions.ItemHeight = 25;
            this.lstQuestions.Location = new System.Drawing.Point(0, 0);
            this.lstQuestions.Name = "lstQuestions";
            this.lstQuestions.Size = new System.Drawing.Size(260, 431);
            this.lstQuestions.TabIndex = 0;
            this.lstQuestions.SelectedIndexChanged += new System.EventHandler(this.lstQuestions_SelectedIndexChanged);
            // 
            // pnlQuestion
            // 
            this.pnlQuestion.Controls.Add(this.flpAnswers);
            this.pnlQuestion.Controls.Add(this.lblQuestion);
            this.pnlQuestion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQuestion.Location = new System.Drawing.Point(0, 0);
            this.pnlQuestion.Name = "pnlQuestion";
            this.pnlQuestion.Padding = new System.Windows.Forms.Padding(8);
            this.pnlQuestion.Size = new System.Drawing.Size(891, 431);
            this.pnlQuestion.TabIndex = 0;
            // 
            // flpAnswers
            // 
            this.flpAnswers.AutoScroll = true;
            this.flpAnswers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpAnswers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpAnswers.Location = new System.Drawing.Point(8, 88);
            this.flpAnswers.Name = "flpAnswers";
            this.flpAnswers.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.flpAnswers.Size = new System.Drawing.Size(875, 335);
            this.flpAnswers.TabIndex = 1;
            this.flpAnswers.WrapContents = false;
            // 
            // lblQuestion
            // 
            this.lblQuestion.AutoEllipsis = true;
            this.lblQuestion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQuestion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblQuestion.Location = new System.Drawing.Point(8, 8);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblQuestion.Size = new System.Drawing.Size(875, 80);
            this.lblQuestion.TabIndex = 0;
            this.lblQuestion.Text = "Câu hỏi";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnSubmit);
            this.pnlBottom.Controls.Add(this.btnNext);
            this.pnlBottom.Controls.Add(this.btnPrev);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(3, 496);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(8);
            this.pnlBottom.Size = new System.Drawing.Size(1155, 50);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubmit.Location = new System.Drawing.Point(1025, 9);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(122, 32);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Nộp bài";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNext.Location = new System.Drawing.Point(124, 9);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(110, 32);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Sau ▶";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPrev.Location = new System.Drawing.Point(8, 9);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(110, 32);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "◀ Trước";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // status
            // 
            this.status.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbInfo});
            this.status.Location = new System.Drawing.Point(0, 549);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(1161, 24);
            this.status.TabIndex = 3;
            this.status.Text = "statusStrip1";
            // 
            // sbInfo
            // 
            this.sbInfo.Name = "sbInfo";
            this.sbInfo.Size = new System.Drawing.Size(50, 18);
            this.sbInfo.Text = "Ready";
            // 
            // ExamRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 573);
            this.Controls.Add(this.tblRoot);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Name = "ExamRoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Phòng thi";
            this.Load += new System.EventHandler(this.ExamRoom_Load);
            this.tblRoot.ResumeLayout(false);
            this.tblRoot.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.pnlQuestion.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblRoot;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.ListBox lstQuestions;
        private System.Windows.Forms.Panel pnlQuestion;
        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.FlowLayoutPanel flpAnswers;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel sbInfo;
    }
}
