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

        private void btnBack_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void HuongDanForm_Load(object? sender, EventArgs e)
        {
            // Khong thay doi giao dien tai day.
        }
    }
}
