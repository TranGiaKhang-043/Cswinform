using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AILATRIEUPHU__MINH_QUAN_
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            // Dam bao nut quay lai chi xu ly mot lan.
            btnBack2.Click -= btnBack_Click;
            btnBack2.Click += btnBack_Click;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Dong trang tac gia; trang huong dan se dong theo de tro ve man hinh chinh.
            Close();
        }
    }
}
