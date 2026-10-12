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
        private readonly ScoreStorage storage = new ScoreStorage();

        public ThanhTichForm()
        {
            InitializeComponent();
        }

        private void ThanhTichForm_Shown(object? sender, EventArgs e)
        {
            if (!DesignMode && System.ComponentModel.LicenseManager.UsageMode !=
                System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadScores();
            }
        }

        private void dong_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void taiLai_Click(object? sender, EventArgs e)
        {
            LoadScores();
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

        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Giữ giao diện trong InitializeComponent để Visual Studio Designer đọc được.

        private void InitializeComponent()
        {
            DataGridViewCellStyle alternatingRowStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            bang = new DataGridView();
            cot0 = new DataGridViewTextBoxColumn();
            cot1 = new DataGridViewTextBoxColumn();
            cot2 = new DataGridViewTextBoxColumn();
            cot3 = new DataGridViewTextBoxColumn();
            cot4 = new DataGridViewTextBoxColumn();
            cot5 = new DataGridViewTextBoxColumn();
            cot6 = new DataGridViewTextBoxColumn();
            thongBao = new Label();
            buttons = new FlowLayoutPanel();
            dong = new Button();
            taiLai = new Button();
            tieuDe = new Label();
            ((System.ComponentModel.ISupportInitialize)bang).BeginInit();
            buttons.SuspendLayout();
            SuspendLayout();
            //
            // bang
            //
            bang.AllowUserToAddRows = false;
            bang.AllowUserToDeleteRows = false;
            bang.AllowUserToResizeRows = false;
            alternatingRowStyle.BackColor = Color.FromArgb(235, 239, 250);
            bang.AlternatingRowsDefaultCellStyle = alternatingRowStyle;
            bang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            bang.BackgroundColor = Color.FromArgb(20, 35, 80);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = Color.FromArgb(35, 55, 110);
            headerStyle.ForeColor = Color.Gold;
            headerStyle.SelectionBackColor = SystemColors.Highlight;
            headerStyle.SelectionForeColor = SystemColors.HighlightText;
            headerStyle.WrapMode = DataGridViewTriState.True;
            bang.ColumnHeadersDefaultCellStyle = headerStyle;
            bang.ColumnHeadersHeight = 40;
            bang.Columns.AddRange(new DataGridViewColumn[]
            {
                cot0, cot1, cot2, cot3, cot4, cot5, cot6
            });
            cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            cellStyle.BackColor = Color.White;
            cellStyle.ForeColor = Color.FromArgb(10, 20, 65);
            cellStyle.SelectionBackColor = Color.MediumPurple;
            cellStyle.SelectionForeColor = Color.White;
            cellStyle.WrapMode = DataGridViewTriState.False;
            bang.DefaultCellStyle = cellStyle;
            bang.Dock = DockStyle.Fill;
            bang.EnableHeadersVisualStyles = false;
            bang.Location = new Point(0, 65);
            bang.MultiSelect = false;
            bang.Name = "bang";
            bang.ReadOnly = true;
            bang.RowHeadersVisible = false;
            bang.RowTemplate.Height = 32;
            bang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            bang.Size = new Size(1000, 355);
            bang.TabIndex = 0;
            //
            // cot0
            //
            cot0.FillWeight = 45F;
            cot0.HeaderText = "Hạng";
            cot0.Name = "cot0";
            cot0.ReadOnly = true;
            cot0.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // cot1
            //
            cot1.FillWeight = 140F;
            cot1.HeaderText = "Người chơi";
            cot1.Name = "cot1";
            cot1.ReadOnly = true;
            cot1.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // cot2
            //
            cot2.FillWeight = 180F;
            cot2.HeaderText = "Chủ đề";
            cot2.Name = "cot2";
            cot2.ReadOnly = true;
            cot2.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // cot3
            //
            cot3.FillWeight = 70F;
            cot3.HeaderText = "Câu đúng";
            cot3.Name = "cot3";
            cot3.ReadOnly = true;
            cot3.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // cot4
            //
            cot4.FillWeight = 100F;
            cot4.HeaderText = "Tiền (USD)";
            cot4.Name = "cot4";
            cot4.ReadOnly = true;
            cot4.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // cot5
            //
            cot5.FillWeight = 75F;
            cot5.HeaderText = "Kết quả";
            cot5.Name = "cot5";
            cot5.ReadOnly = true;
            cot5.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // cot6
            //
            cot6.FillWeight = 135F;
            cot6.HeaderText = "Thời gian";
            cot6.Name = "cot6";
            cot6.ReadOnly = true;
            cot6.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // thongBao
            //
            thongBao.AutoEllipsis = true;
            thongBao.Dock = DockStyle.Bottom;
            thongBao.Location = new Point(0, 420);
            thongBao.Name = "thongBao";
            thongBao.Padding = new Padding(12, 5, 12, 0);
            thongBao.Size = new Size(1000, 55);
            thongBao.TabIndex = 1;
            thongBao.Text = "Chưa có thành tích. Hãy hoàn thành một lượt chơi!";
            //
            // buttons
            //
            buttons.Controls.Add(dong);
            buttons.Controls.Add(taiLai);
            buttons.Dock = DockStyle.Bottom;
            buttons.FlowDirection = FlowDirection.RightToLeft;
            buttons.Location = new Point(0, 475);
            buttons.Name = "buttons";
            buttons.Padding = new Padding(8);
            buttons.Size = new Size(1000, 55);
            buttons.TabIndex = 2;
            //
            // dong
            //
            dong.DialogResult = DialogResult.Cancel;
            dong.Location = new Point(879, 11);
            dong.Name = "dong";
            dong.Size = new Size(110, 35);
            dong.TabIndex = 0;
            dong.Text = "Đóng";
            dong.Click += dong_Click;
            //
            // taiLai
            //
            taiLai.Location = new Point(763, 11);
            taiLai.Name = "taiLai";
            taiLai.Size = new Size(110, 35);
            taiLai.TabIndex = 1;
            taiLai.Text = "Tải lại";
            taiLai.Click += taiLai_Click;
            //
            // tieuDe
            //
            tieuDe.Dock = DockStyle.Top;
            tieuDe.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            tieuDe.Location = new Point(0, 0);
            tieuDe.Name = "tieuDe";
            tieuDe.Size = new Size(1000, 65);
            tieuDe.TabIndex = 3;
            tieuDe.Text = "BẢNG THÀNH TÍCH";
            tieuDe.TextAlign = ContentAlignment.MiddleCenter;
            //
            // ThanhTichForm
            //
            BackColor = Color.FromArgb(10, 20, 65);
            CancelButton = dong;
            ClientSize = new Size(1000, 530);
            Controls.Add(bang);
            Controls.Add(thongBao);
            Controls.Add(buttons);
            Controls.Add(tieuDe);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.Gold;
            MinimumSize = new Size(850, 450);
            Name = "ThanhTichForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Bảng thành tích - Ai là triệu phú";
            Shown += ThanhTichForm_Shown;
            ((System.ComponentModel.ISupportInitialize)bang).EndInit();
            buttons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView bang = null!;
        private DataGridViewTextBoxColumn cot0 = null!;
        private DataGridViewTextBoxColumn cot1 = null!;
        private DataGridViewTextBoxColumn cot2 = null!;
        private DataGridViewTextBoxColumn cot3 = null!;
        private DataGridViewTextBoxColumn cot4 = null!;
        private DataGridViewTextBoxColumn cot5 = null!;
        private DataGridViewTextBoxColumn cot6 = null!;
        private Label thongBao = null!;
        private FlowLayoutPanel buttons = null!;
        private Button dong = null!;
        private Button taiLai = null!;
        private Label tieuDe = null!;
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
