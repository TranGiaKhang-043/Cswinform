using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AiLaTrieuPhu;

namespace AILATRIEUPHU__MINH_QUAN_
{
    public class ThanhTichForm : Form
    {
        private readonly DataGridView bang = new DataGridView();
        private readonly Label thongBao = new Label();
        private readonly ScoreStorage storage = new ScoreStorage();

        public ThanhTichForm()
        {
            Text = "Bảng thành tích - Ai là triệu phú";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1000, 530);
            MinimumSize = new Size(850, 450);
            Font = new Font("Segoe UI", 10);
            BackColor = Color.FromArgb(10, 20, 65);
            ForeColor = Color.Gold;

            Label tieuDe = new Label { Text = "BẢNG THÀNH TÍCH", Dock = DockStyle.Top,
                Height = 65, TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 22, FontStyle.Bold) };
            FlowLayoutPanel buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom,
                Height = 55, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
            Button dong = new Button { Text = "Đóng", Width = 110, Height = 35,
                DialogResult = DialogResult.Cancel };
            Button taiLai = new Button { Text = "Tải lại", Width = 110, Height = 35 };
            dong.Click += (s, e) => Close();
            taiLai.Click += (s, e) => LoadScores();
            buttons.Controls.Add(dong);
            buttons.Controls.Add(taiLai);
            CancelButton = dong;
            thongBao.Dock = DockStyle.Bottom;
            thongBao.Height = 55;
            thongBao.Padding = new Padding(12, 5, 12, 0);
            thongBao.AutoEllipsis = true;

            bang.Dock = DockStyle.Fill;
            bang.ReadOnly = true;
            bang.AllowUserToAddRows = false;
            bang.AllowUserToDeleteRows = false;
            bang.AllowUserToResizeRows = false;
            bang.RowHeadersVisible = false;
            bang.MultiSelect = false;
            bang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            bang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            bang.BackgroundColor = Color.FromArgb(20, 35, 80);
            bang.DefaultCellStyle.BackColor = Color.White;
            bang.DefaultCellStyle.ForeColor = Color.FromArgb(10, 20, 65);
            bang.DefaultCellStyle.SelectionBackColor = Color.MediumPurple;
            bang.DefaultCellStyle.SelectionForeColor = Color.White;
            bang.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 239, 250);
            bang.EnableHeadersVisualStyles = false;
            bang.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 55, 110);
            bang.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gold;
            bang.ColumnHeadersHeight = 40;
            bang.RowTemplate.Height = 32;
            string[] headers = { "Hạng", "Người chơi", "Chủ đề", "Câu đúng", "Tiền (USD)", "Kết quả", "Thời gian" };
            int[] widths = { 45, 140, 180, 70, 100, 75, 135 };
            for (int i = 0; i < headers.Length; i++)
            {
                int index = bang.Columns.Add("cot" + i, headers[i]);
                bang.Columns[index].FillWeight = widths[i];
                bang.Columns[index].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            Controls.Add(bang);
            Controls.Add(thongBao);
            Controls.Add(buttons);
            Controls.Add(tieuDe);
            Shown += (s, e) => LoadScores();
        }

        private void LoadScores()
        {
            bang.Rows.Clear();
            if (!storage.TryRead(out List<Score> scores, out string error))
            {
                thongBao.Text = "Không đọc được thành tích. Kiểm tra file rồi bấm Tải lại.";
                MessageBox.Show(this, error, "Thành tích", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            for (int i = 0; i < scores.Count; i++)
            {
                Score score = scores[i];
                bang.Rows.Add(i + 1, score.PlayerName, score.Topic, score.CorrectCount + "/10",
                    score.Money.ToString("N0", CultureInfo.GetCultureInfo("en-US")),
                    score.Result, score.PlayedAt.ToString("dd/MM/yyyy HH:mm"));
            }
            thongBao.Text = scores.Count == 0 ? "Chưa có thành tích. Hãy hoàn thành một lượt chơi!" :
                "Có " + scores.Count + " lượt chơi. Xếp theo tiền, số câu đúng, rồi thời gian mới nhất.";
            bang.ClearSelection();
        }
    }

    // Hộp nhập tên dùng trước khi mở màn hình chơi.
    public class TenNguoiChoiForm : Form
    {
        private readonly TextBox ten = new TextBox();
        public string PlayerName => ten.Text.Trim();

        public TenNguoiChoiForm(string tenCu)
        {
            Text = "Tên người chơi";
            ClientSize = new Size(400, 165);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 11);
            Label label = new Label { Text = "Nhập tên để lưu thành tích (tối đa 40 ký tự):",
                Location = new Point(20, 15), Size = new Size(365, 30) };
            ten.Location = new Point(20, 50);
            ten.Size = new Size(355, 30);
            ten.MaxLength = 40;
            ten.Text = tenCu;
            Button batDau = new Button { Text = "Bắt đầu", Location = new Point(145, 105), Size = new Size(110, 35) };
            Button huy = new Button { Text = "Hủy", Location = new Point(265, 105), Size = new Size(110, 35), DialogResult = DialogResult.Cancel };
            batDau.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(PlayerName))
                {
                    MessageBox.Show(this, "Bạn cần nhập tên người chơi.", "Thiếu tên");
                    ten.Focus();
                    return;
                }
                DialogResult = DialogResult.OK;
            };
            Controls.AddRange(new Control[] { label, ten, batDau, huy });
            AcceptButton = batDau;
            CancelButton = huy;
        }
    }
}
