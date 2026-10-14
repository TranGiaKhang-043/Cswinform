using System;
using System.Drawing;
using System.Windows.Forms;

namespace AILATRIEUPHU__MINH_QUAN_
{
    public partial class HuongDanForm : Form
    {
        public HuongDanForm()
        {
            InitializeComponent();
            AddAuthorNavigationButton();
        }

        private void AddAuthorNavigationButton()
        {
            int horizontalMargin = 17;
            int gap = 16;
            int buttonWidth = (ClientSize.Width - (horizontalMargin * 2) - gap) / 2;

            btnBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnBack.Location = new Point(horizontalMargin, btnBack.Top);
            btnBack.Size = new Size(buttonWidth, btnBack.Height);

            var btnAuthors = new Button
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.FromArgb(0, 119, 209),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.White,
                Location = new Point(horizontalMargin + buttonWidth + gap, btnBack.Top),
                Name = "btnAuthors",
                Size = new Size(buttonWidth, btnBack.Height),
                TabIndex = btnBack.TabIndex + 1,
                Text = "TÁC GIẢ",
                UseVisualStyleBackColor = false
            };
            btnAuthors.FlatAppearance.BorderSize = 0;
            btnAuthors.Click -= btnAuthors_Click;
            btnAuthors.Click += btnAuthors_Click;
            Controls.Add(btnAuthors);
        }

        private void btnAuthors_Click(object? sender, EventArgs e)
        {
            using (var authorForm = new Form2())
            {
                authorForm.StartPosition = FormStartPosition.CenterParent;
                authorForm.ShowDialog(this);
            }

            // Khi quay lai tu trang tac gia, dong huong dan de tro ve man hinh chinh.
            Close();
        }

        private void btnBack_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void HuongDanForm_Load(object? sender, EventArgs e)
        {
            // Khong thay doi giao dien tai day.
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
