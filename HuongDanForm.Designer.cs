namespace AILATRIEUPHU__MINH_QUAN_
{
    partial class HuongDanForm
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
            btnExit = new Button();
            btnSound = new Button();
            btnAchievement = new Button();
            btnGuide = new Button();
            btnStart = new Button();
            lblTitle = new Label();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.BackColor = SystemColors.Highlight;
            btnExit.Font = new Font("Arial", 15F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnExit.ForeColor = Color.Yellow;
            btnExit.Location = new Point(625, 282);
            btnExit.Margin = new Padding(2, 2, 2, 2);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(44, 34);
            btnExit.TabIndex = 12;
            btnExit.Text = "➡️";
            btnExit.UseVisualStyleBackColor = false;
            // 
            // btnSound
            // 
            btnSound.BackColor = SystemColors.Highlight;
            btnSound.Font = new Font("Arial", 15F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnSound.ForeColor = Color.Yellow;
            btnSound.Location = new Point(-109, 282);
            btnSound.Margin = new Padding(2, 2, 2, 2);
            btnSound.Name = "btnSound";
            btnSound.Size = new Size(44, 34);
            btnSound.TabIndex = 11;
            btnSound.Text = "🔊";
            btnSound.UseVisualStyleBackColor = false;
            // 
            // btnAchievement
            // 
            btnAchievement.BackColor = SystemColors.Highlight;
            btnAchievement.Font = new Font("Arial", 15F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnAchievement.ForeColor = Color.Yellow;
            btnAchievement.Location = new Point(211, 233);
            btnAchievement.Margin = new Padding(2, 2, 2, 2);
            btnAchievement.Name = "btnAchievement";
            btnAchievement.Size = new Size(156, 34);
            btnAchievement.TabIndex = 10;
            btnAchievement.Text = "THÀNH TÍCH 🏆";
            btnAchievement.UseVisualStyleBackColor = false;
            btnAchievement.Click += btnAchievement_Click;
            // 
            // btnGuide
            // 
            btnGuide.BackColor = SystemColors.Highlight;
            btnGuide.Font = new Font("Arial", 15F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnGuide.ForeColor = Color.Yellow;
            btnGuide.Location = new Point(211, 282);
            btnGuide.Margin = new Padding(2, 2, 2, 2);
            btnGuide.Name = "btnGuide";
            btnGuide.Size = new Size(156, 34);
            btnGuide.TabIndex = 9;
            btnGuide.Text = "HƯỚNG DẪN 📖";
            btnGuide.UseVisualStyleBackColor = false;
            // 
            // btnStart
            // 
            btnStart.Anchor = AnchorStyles.None;
            btnStart.BackColor = SystemColors.Highlight;
            btnStart.Font = new Font("Arial", 15F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnStart.ForeColor = Color.Yellow;
            btnStart.Location = new Point(211, 186);
            btnStart.Margin = new Padding(2, 2, 2, 2);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(156, 34);
            btnStart.TabIndex = 8;
            btnStart.Text = "BẮT ĐẦU ▶";
            btnStart.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.Location = new Point(8, -46);
            lblTitle.Margin = new Padding(2, 0, 2, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(591, 37);
            lblTitle.TabIndex = 7;
            lblTitle.Text = "💰 BẠN CÓ MUỐN GIÀU SAU MỘT ĐÊM ? 💰";
            // 
            // HuongDanForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(561, 275);
            Controls.Add(btnExit);
            Controls.Add(btnSound);
            Controls.Add(btnAchievement);
            Controls.Add(btnGuide);
            Controls.Add(btnStart);
            Controls.Add(lblTitle);
            Margin = new Padding(2, 2, 2, 2);
            Name = "HuongDanForm";
            Text = "HuongDanForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnExit;
        private Button btnSound;
        private Button btnAchievement;
        private Button btnGuide;
        private Button btnStart;
        private Label lblTitle;
    }
}