using System;
using System.Windows.Forms;

namespace AILATRIEUPHU__MINH_QUAN_
{
    public partial class HuongDanForm : Form
    {
        public HuongDanForm()
        {
            InitializeComponent();
        }

        private void buttonBack_Click(object? sender, EventArgs e)
        {
            // Đóng trang hướng dẫn để quay lại màn hình chính.
            Close();
        }

        private void buttonAuthor_Click(object? sender, EventArgs e)
        {
            // Mở trang tác giả; khi đóng trang này sẽ quay lại màn hình chính.
            using (var authorForm = new Form2())
            {
                authorForm.StartPosition = FormStartPosition.CenterParent;
                authorForm.ShowDialog(this);
            }

            Close();
        }

        private void HuongDanForm_Load(object? sender, EventArgs e)
        {
        }
    }
}
