using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace GKOOP
{
    public class ExamEditorDialog : Form
    {
        // ==== public result ====
        public ExamDraft Draft { get; private set; }

        // ==== controls ====
        private TextBox txtName;
        private ComboBox cboSubject;   // hiển thị Subject Name, lưu SubjectId
        private NumericUpDown numDuration;
        private ListBox lstQuestions;
        private Button btnAddQ, btnEditQ, btnDelQ, btnOK, btnCancel;

        // giữ danh sách câu trong dialog
        private readonly List<QuestionDto> _questions = new List<QuestionDto>();

        public ExamEditorDialog() : this(new ExamDraft()) { }

        public ExamEditorDialog(ExamDraft init)
        {
            Draft = init ?? new ExamDraft();
            BuildUi();
            Load += ExamEditorDialog_Load;
        }

        private void BuildUi()
        {
            Text = "Thêm/Sửa bài thi";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(720, 520);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lblName = new Label { Text = "Tên bài thi", AutoSize = true, Location = new Point(16, 18) };
            txtName = new TextBox { Location = new Point(120, 14), Width = 560 };

            var lblSub = new Label { Text = "Môn", AutoSize = true, Location = new Point(16, 54) };
            cboSubject = new ComboBox
            {
                Location = new Point(120, 50),
                Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = "Text",
                ValueMember = "Value"
            };

            var lblDur = new Label { Text = "Thời lượng (phút)", AutoSize = true, Location = new Point(400, 54) };
            numDuration = new NumericUpDown
            {
                Location = new Point(520, 50),
                Minimum = 5,
                Maximum = 300,
                Value = 15,
                Width = 80
            };

            var grpQ = new GroupBox
            {
                Text = "Câu hỏi",
                Location = new Point(16, 90),
                Size = new Size(664, 360)
            };

            lstQuestions = new ListBox
            {
                Location = new Point(12, 24),
                Size = new Size(520, 320)
            };

            btnAddQ = new Button { Text = "Thêm câu", Location = new Point(540, 24), Size = new Size(104, 30) };
            btnEditQ = new Button { Text = "Sửa câu", Location = new Point(540, 64), Size = new Size(104, 30) };
            btnDelQ = new Button { Text = "Xóa câu", Location = new Point(540, 104), Size = new Size(104, 30) };

            grpQ.Controls.Add(lstQuestions);
            grpQ.Controls.Add(btnAddQ);
            grpQ.Controls.Add(btnEditQ);
            grpQ.Controls.Add(btnDelQ);

            btnOK = new Button { Text = "Lưu", DialogResult = DialogResult.None, Location = new Point(488, 464), Size = new Size(88, 30) };
            btnCancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Location = new Point(592, 464), Size = new Size(88, 30) };

            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblSub);
            Controls.Add(cboSubject);
            Controls.Add(lblDur);
            Controls.Add(numDuration);
            Controls.Add(grpQ);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);

            // events
            btnAddQ.Click += BtnAddQ_Click;
            btnEditQ.Click += BtnEditQ_Click;
            btnDelQ.Click += BtnDelQ_Click;
            btnOK.Click += BtnOK_Click;
        }

        private async void ExamEditorDialog_Load(object sender, EventArgs e)
        {
            // 1) load Subjects từ DB
            await LoadSubjectsAsync();

            // 2) nếu sửa -> đổ dữ liệu
            if (!string.IsNullOrWhiteSpace(Draft.Name))
                txtName.Text = Draft.Name;

            if (Draft.DurationMinutes > 0)
                numDuration.Value = Draft.DurationMinutes;

            if (Draft.SubjectId != Guid.Empty)
                cboSubject.SelectedValue = Draft.SubjectId;

            _questions.Clear();
            if (Draft.Questions != null) _questions.AddRange(Draft.Questions);
            RefreshQuestionList();
        }

        private async System.Threading.Tasks.Task LoadSubjectsAsync()
        {
            try
            {
                var cs = ConfigurationManager.ConnectionStrings["PgConn"].ConnectionString;
                const string sql = "SELECT id, name FROM subjects ORDER BY name;";

                var items = new List<_Item>();
                using (var conn = new NpgsqlConnection(cs))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            items.Add(new _Item
                            {
                                Value = rd.GetGuid(0),
                                Text = rd.GetString(1)
                            });
                        }
                    }
                }
                cboSubject.DataSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách môn: " + ex.Message);
            }
        }

        private class _Item
        {
            public Guid Value { get; set; }
            public string Text { get; set; }
        }

        private void RefreshQuestionList()
        {
            lstQuestions.Items.Clear();
            for (int i = 0; i < _questions.Count; i++)
            {
                var q = _questions[i];
                var text = (i + 1) + ". " + (q.Content.Length > 60 ? q.Content.Substring(0, 59) + "…" : q.Content);
                lstQuestions.Items.Add(text);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ExamEditorDialog
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "ExamEditorDialog";
            this.Load += new System.EventHandler(this.ExamEditorDialog_Load_1);
            this.ResumeLayout(false);

        }

        private void ExamEditorDialog_Load_1(object sender, EventArgs e)
        {

        }

        private void BtnAddQ_Click(object sender, EventArgs e)
        {
            using (var f = new QuestionEditorDialog())
            {
                if (f.ShowDialog(this) == DialogResult.OK && f.Result != null)
                {
                    _questions.Add(f.Result);
                    RefreshQuestionList();
                }
            }
        }

        private void BtnEditQ_Click(object sender, EventArgs e)
        {
            if (lstQuestions.SelectedIndex < 0) { MessageBox.Show("Chọn câu cần sửa."); return; }
            var current = _questions[lstQuestions.SelectedIndex];

            using (var f = new QuestionEditorDialog(current))
            {
                if (f.ShowDialog(this) == DialogResult.OK && f.Result != null)
                {
                    _questions[lstQuestions.SelectedIndex] = f.Result;
                    RefreshQuestionList();
                }
            }
        }

        private void BtnDelQ_Click(object sender, EventArgs e)
        {
            if (lstQuestions.SelectedIndex < 0) return;
            _questions.RemoveAt(lstQuestions.SelectedIndex);
            RefreshQuestionList();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Nhập tên bài thi.");
                txtName.Focus(); return;
            }
            if (cboSubject.SelectedItem == null)
            {
                MessageBox.Show("Chọn môn.");
                return;
            }
            if (_questions.Count == 0)
            {
                MessageBox.Show("Cần ít nhất 1 câu hỏi.");
                return;
            }

            Draft.Name = txtName.Text.Trim();
            Draft.SubjectId = ((_Item)cboSubject.SelectedItem).Value;
            Draft.DurationMinutes = (int)numDuration.Value;
            Draft.Questions = new List<QuestionDto>(_questions);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
