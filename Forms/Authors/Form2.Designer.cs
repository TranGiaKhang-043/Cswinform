namespace AILATRIEUPHU__MINH_QUAN_
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
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
            lblHowToPlayTitle = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            pnlHowToPlay = new Panel();
            btnBack2 = new Button();
            pnlHowToPlay.SuspendLayout();
            SuspendLayout();
            // 
            // lblHowToPlayTitle
            // 
            lblHowToPlayTitle.BackColor = Color.Transparent;
            lblHowToPlayTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHowToPlayTitle.ForeColor = Color.Gold;
            lblHowToPlayTitle.Location = new Point(345, 55);
            lblHowToPlayTitle.Margin = new Padding(4, 0, 4, 0);
            lblHowToPlayTitle.Name = "lblHowToPlayTitle";
            lblHowToPlayTitle.Size = new Size(571, 53);
            lblHowToPlayTitle.TabIndex = 0;
            lblHowToPlayTitle.Text = "NHÓM TÁC GIẢ · NOVATECH";
            lblHowToPlayTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 141);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(1189, 82);
            label1.TabIndex = 2;
            label1.Text = "01. Trần Minh Quân - 51.01.104.083";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(0, 262);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(1189, 82);
            label2.TabIndex = 3;
            label2.Text = "02. Trần Gia Khang - 51.01.104.043";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(0, 384);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(1189, 82);
            label3.TabIndex = 4;
            label3.Text = "03. Phan Đức Hoàng - 51.01.104.131";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(0, 515);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(1189, 82);
            label4.TabIndex = 5;
            label4.Text = "04. Lê Nhật Hào - 51.01.104.023";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            label4.Click += label4_Click;
            // 
            // pnlHowToPlay
            // 
            pnlHowToPlay.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlHowToPlay.BackColor = Color.FromArgb(66, 22, 106);
            pnlHowToPlay.Controls.Add(label4);
            pnlHowToPlay.Controls.Add(label3);
            pnlHowToPlay.Controls.Add(label2);
            pnlHowToPlay.Controls.Add(label1);
            pnlHowToPlay.Controls.Add(lblHowToPlayTitle);
            pnlHowToPlay.Location = new Point(13, 14);
            pnlHowToPlay.Margin = new Padding(4, 5, 4, 5);
            pnlHowToPlay.Name = "pnlHowToPlay";
            pnlHowToPlay.Size = new Size(1228, 776);
            pnlHowToPlay.TabIndex = 14;
            // 
            // btnBack2
            // 
            btnBack2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnBack2.BackColor = Color.FromArgb(0, 119, 209);
            btnBack2.Cursor = Cursors.Hand;
            btnBack2.FlatAppearance.BorderSize = 0;
            btnBack2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBack2.ForeColor = Color.White;
            btnBack2.Location = new Point(865, 800);
            btnBack2.Margin = new Padding(4, 5, 4, 5);
            btnBack2.Name = "btnBack2";
            btnBack2.Size = new Size(376, 93);
            btnBack2.TabIndex = 17;
            btnBack2.Text = "QUAY LẠI";
            btnBack2.UseVisualStyleBackColor = false;
            btnBack2.Click += btnBack_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(33, 11, 53);
            ClientSize = new Size(1254, 907);
            Controls.Add(btnBack2);
            Controls.Add(pnlHowToPlay);
            ForeColor = Color.FromArgb(33, 11, 53);
            KeyPreview = true;
            MinimumSize = new Size(1276, 963);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            pnlHowToPlay.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblHowToPlayTitle;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Panel pnlHowToPlay;
        private Button btnBack2;
    }
}