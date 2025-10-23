using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GKOOP
{
    public class QuestionEditorDialog : Form
    {
        public QuestionDto Result { get; private set; }

        private TextBox txtContent;
        private TextBox[] txtAns = new TextBox[4];
        private RadioButton[] rdoCorrect = new RadioButton[4];
        private Button btnOK, btnCancel;

        public QuestionEditorDialog() : this(null) { }

        public QuestionEditorDialog(QuestionDto init)
        {
            BuildUi();

            if (init != null)
            {
                txtContent.Text = init.Content ?? "";
                for (int i = 0; i < 4 && i < init.Answers.Count; i++)
                {
                    txtAns[i].Text = init.Answers[i].Text ?? "";
                    rdoCorrect[i].Checked = init.Answers[i].IsCorrect;
                }
            }
        }

        private void BuildUi()
        {
            Text = "Câu hỏi";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(620, 360);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lblQ = new Label { Text = "Nội dung câu hỏi:", AutoSize = true, Location = new Point(12, 12) };
            txtContent = new TextBox { Location = new Point(12, 32), Size = new Size(592, 60), Multiline = true, ScrollBars = ScrollBars.Vertical };

            Controls.Add(lblQ);
            Controls.Add(txtContent);

            for (int i = 0; i < 4; i++)
            {
                rdoCorrect[i] = new RadioButton { Location = new Point(16, 110 + i * 40), AutoSize = true };
                txtAns[i] = new TextBox { Location = new Point(44, 106 + i * 40), Width = 560 };
                Controls.Add(rdoCorrect[i]);
                Controls.Add(txtAns[i]);
            }

            var lblHint = new Label { Text = "Chọn 1 đáp án đúng", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(12, 274) };
            Controls.Add(lblHint);

            btnOK = new Button { Text = "OK", Location = new Point(416, 300), Size = new Size(86, 28) };
            btnCancel = new Button { Text = "Hủy", Location = new Point(518, 300), Size = new Size(86, 28), DialogResult = DialogResult.Cancel };
            Controls.Add(btnOK);
            Controls.Add(btnCancel);

            btnOK.Click += BtnOK_Click;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // QuestionEditorDialog
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "QuestionEditorDialog";
            this.Load += new System.EventHandler(this.QuestionEditorDialog_Load);
            this.ResumeLayout(false);

        }

        private void QuestionEditorDialog_Load(object sender, EventArgs e)
        {

        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtContent.Text))
            {
                MessageBox.Show("Nhập nội dung câu hỏi."); return;
            }

            var answers = new List<AnswerDto>();
            for (int i = 0; i < 4; i++)
            {
                if (string.IsNullOrWhiteSpace(txtAns[i].Text))
                {
                    MessageBox.Show("Nhập đủ 4 đáp án."); return;
                }
                answers.Add(new AnswerDto
                {
                    Id = Guid.Empty,
                    Text = txtAns[i].Text.Trim(),
                    IsCorrect = rdoCorrect[i].Checked
                });
            }

            if (!answers.Any(a => a.IsCorrect))
            {
                MessageBox.Show("Hãy chọn 1 đáp án đúng."); return;
            }

            Result = new QuestionDto
            {
                Id = Guid.Empty,
                Content = txtContent.Text.Trim(),
                Answers = answers
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
