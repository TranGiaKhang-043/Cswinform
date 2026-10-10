using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AiLaTrieuPhu;

namespace AILATRIEUPHU__MINH_QUAN_
{
    public partial class frmMain : Form
    {
        // Mỗi chủ đề giữ một bộ luật để tránh lặp câu khi quay về rồi chơi tiếp.
        private readonly Dictionary<QuestionTopic, GameRules> _gamesByTopic =
            new Dictionary<QuestionTopic, GameRules>();
        private QuestionTopic _lastTopic = QuestionTopic.Programming;
        private bool _openingGame;

        // =========================================
        // VỊ TRÍ BAN ĐẦU
        // =========================================

        private Point startPos;
        private Point guidePos;
        private Point soundPos;
        private Point achievementPos;
        private Point exitPos;

        // =========================================
        // VỊ TRÍ TIÊU ĐỀ
        // =========================================

        private Point titleOriginalPos;

        // =========================================
        // MÀU GỐC CỦA BUTTON
        // =========================================

        private Color startBackColor;
        private Color startForeColor;

        private Color guideBackColor;
        private Color guideForeColor;

        private Color soundBackColor;
        private Color soundForeColor;

        private Color achievementBackColor;
        private Color achievementForeColor;

        private Color exitBackColor;
        private Color exitForeColor;


        // =========================================
        // CONSTRUCTOR
        // =========================================

        public frmMain()
        {
            InitializeComponent();

            // Nối nút thoát còn thiếu trong bản màn hình chính.
            btnExit.Click += btnExit_Click;

            // Ba chức năng này chờ phần của Khang và Hoàng để ghép tiếp.
            // Tạm khóa để người chơi không bấm vào nút chưa có xử lý.
            btnGuide.Enabled = false;
            btnSound.Enabled = false;
            btnAchievement.Enabled = false;

            // =====================================
            // LƯU VỊ TRÍ BAN ĐẦU
            // =====================================

            startPos = btnStart.Location;
            guidePos = btnGuide.Location;
            soundPos = btnSound.Location;
            achievementPos = btnAchievement.Location;
            exitPos = btnExit.Location;

            // =====================================
            // LƯU VỊ TRÍ TIÊU ĐỀ
            // =====================================

            titleOriginalPos = lblTitle.Location;

            // =====================================
            // LƯU MÀU GỐC
            // =====================================

            startBackColor = btnStart.BackColor;
            startForeColor = btnStart.ForeColor;

            guideBackColor = btnGuide.BackColor;
            guideForeColor = btnGuide.ForeColor;

            soundBackColor = btnSound.BackColor;
            soundForeColor = btnSound.ForeColor;

            achievementBackColor = btnAchievement.BackColor;
            achievementForeColor = btnAchievement.ForeColor;

            exitBackColor = btnExit.BackColor;
            exitForeColor = btnExit.ForeColor;

            // =====================================
            // THÊM HIỆU ỨNG CHO BUTTON
            // =====================================

            AddButtonEffect(btnStart);
            AddButtonEffect(btnGuide);
            AddButtonEffect(btnSound);
            AddButtonEffect(btnAchievement);
            AddButtonEffect(btnExit);

            // =====================================
            // HIỆU ỨNG TIÊU ĐỀ
            // =====================================

            lblTitle.MouseEnter += lblTitle_MouseEnter;
            lblTitle.MouseLeave += lblTitle_MouseLeave;
        }


        // =========================================
        // CÀI HIỆU ỨNG BUTTON
        // =========================================

        private void AddButtonEffect(Button button)
        {
            button.Cursor = Cursors.Hand;

            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;
            button.MouseDown += Button_MouseDown;
            button.MouseUp += Button_MouseUp;
        }


        // =========================================
        // RÊ CHUỘT VÀO BUTTON
        // =========================================

        private void Button_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;

            // Luôn tính từ vị trí gốc để nút không bị lệch sau nhiều lượt rê chuột.
            ResetButtonPosition(button);
            button.Top -= 2;

            // =====================================
            // ĐỔI NỀN SANG MÀU TÍM
            // =====================================

            button.BackColor = Color.MediumPurple;

            // =====================================
            // MÀU CHỮ GIỮ NGUYÊN
            // =====================================
        }


        // =========================================
        // RỜI CHUỘT RA KHỎI BUTTON
        // =========================================

        private void Button_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;

            // Trở về vị trí ban đầu
            ResetButtonPosition(button);

            // Trở về màu ban đầu
            ResetButtonColor(button);
        }


        // =========================================
        // NHẤN CHUỘT
        // =========================================

        private void Button_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (sender is not Button button) return;

            // Khi nhấn, đưa nút về vị trí gốc tạo cảm giác lún xuống.
            ResetButtonPosition(button);
        }


        // =========================================
        // THẢ CHUỘT
        // =========================================

        private void Button_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (sender is not Button button) return;
            ResetButtonPosition(button);

            // Nếu chuột vẫn nằm trên button
            if (button.ClientRectangle.Contains(
                button.PointToClient(Cursor.Position)))
            {
                // Trở lại vị trí hover
                button.Top -= 2;
            }
        }


        // =========================================
        // RESET VỊ TRÍ BUTTON
        // =========================================

        private void ResetButtonPosition(Button button)
        {
            if (button == btnStart)
            {
                button.Location = startPos;
            }
            else if (button == btnGuide)
            {
                button.Location = guidePos;
            }
            else if (button == btnSound)
            {
                button.Location = soundPos;
            }
            else if (button == btnAchievement)
            {
                button.Location = achievementPos;
            }
            else if (button == btnExit)
            {
                button.Location = exitPos;
            }
        }


        // =========================================
        // RESET MÀU BUTTON
        // =========================================

        private void ResetButtonColor(Button button)
        {
            if (button == btnStart)
            {
                button.BackColor = startBackColor;
                button.ForeColor = startForeColor;
            }
            else if (button == btnGuide)
            {
                button.BackColor = guideBackColor;
                button.ForeColor = guideForeColor;
            }
            else if (button == btnSound)
            {
                button.BackColor = soundBackColor;
                button.ForeColor = soundForeColor;
            }
            else if (button == btnAchievement)
            {
                button.BackColor = achievementBackColor;
                button.ForeColor = achievementForeColor;
            }
            else if (button == btnExit)
            {
                button.BackColor = exitBackColor;
                button.ForeColor = exitForeColor;
            }
        }


        // =========================================
        // TIÊU ĐỀ - RÊ CHUỘT VÀO
        // =========================================

        private void lblTitle_MouseEnter(object? sender, EventArgs e)
        {
            lblTitle.Location = new Point(
                titleOriginalPos.X,
                titleOriginalPos.Y - 2
            );
        }


        // =========================================
        // TIÊU ĐỀ - RỜI CHUỘT
        // =========================================

        private void lblTitle_MouseLeave(object? sender, EventArgs e)
        {
            lblTitle.Location = titleOriginalPos;
        }


        // =========================================
        // CÁC EVENT CÓ SẴN
        // =========================================

        private void lblTitle_Click(object sender, EventArgs e)
        {
        }

        private void btnStart_Click(object? sender, EventArgs e)
        {
            // Chặn bấm liên tiếp để không mở hai cửa sổ chơi cùng lúc.
            if (_openingGame) return;

            _openingGame = true;
            btnStart.Enabled = false;

            try
            {
                // Tạo xong cửa sổ chơi mới ẩn màn hình chính.
                // Dùng chung bộ luật giúp ván mới nhớ câu đã dùng ở ván trước.
                using (var gameForm = new AILATRIEUPHU.GameForm(_lastTopic, _gamesByTopic))
                {
                    Hide();
                    gameForm.ShowDialog(this);
                    _lastTopic = gameForm.SelectedTopic;
                }
            }
            finally
            {
                // Dù đóng bằng dấu X hay Alt+F4, màn hình chính đều hiện lại.
                _openingGame = false;
                if (!IsDisposed)
                {
                    Show();
                    btnStart.Enabled = true;
                    ResetButtonPosition(btnStart);
                    ResetButtonColor(btnStart);
                    Activate();
                }
            }
        }

        private void btnGuide_Click(object sender, EventArgs e)
        {
        }

        private void btnSound_Click(object sender, EventArgs e)
        {
        }

        private void btnAchievement_Click(object sender, EventArgs e)
        {
        }

        private void btnExit_Click(object? sender, EventArgs e)
        {
            // Đây là cửa sổ chính nên đóng nó sẽ kết thúc ứng dụng.
            Close();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
