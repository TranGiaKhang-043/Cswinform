
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WMPLib;
using AiLaTrieuPhu;

namespace AILATRIEUPHU__MINH_QUAN_
{
    public partial class frmMain : Form
    {
        #region Bien va trang thai

        private readonly Dictionary<QuestionTopic, GameRules> _gamesByTopic =
            new Dictionary<QuestionTopic, GameRules>();

        private QuestionTopic _lastTopic = QuestionTopic.Programming;

        private bool _openingGame;
        private bool _soundEnabled = true;
        private string _playerName = string.Empty;

        private WindowsMediaPlayer? _musicPlayer;

        // Vi tri ban dau cua cac nut.
        private Point startPos;
        private Point guidePos;
        private Point soundPos;
        private Point achievementPos;
        private Point exitPos;

        // Vi tri ban dau cua tieu de.
        private Point titleOriginalPos;

        // Mau ban dau cua cac nut.
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

        #endregion

        #region Khoi tao form

        public frmMain()
        {
            InitializeComponent();

            // Dam bao moi nut chi goi mot handler Click tuong ung.
            // -= truoc khi += giup tranh mo hai cua so neu Designer da noi su kien.
            btnExit.Click -= btnExit_Click;
            btnExit.Click += btnExit_Click;

            // btnGuide.Click da duoc noi trong Form1.Designer.cs.

            // Bat cac chuc nang da duoc tich hop.
            btnGuide.Enabled = true;
            btnAchievement.Enabled = true;
            btnSound.Enabled = true;

            // Designer chua noi Click cho nut Thanh tich.
            btnAchievement.Click -= btnAchievement_Click;
            btnAchievement.Click += btnAchievement_Click;

            // Luu vi tri ban dau.
            startPos = btnStart.Location;
            guidePos = btnGuide.Location;
            soundPos = btnSound.Location;
            achievementPos = btnAchievement.Location;
            exitPos = btnExit.Location;

            titleOriginalPos = lblTitle.Location;

            // Luu mau ban dau.
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

            // Hieu ung cho cac nut.
            AddButtonEffect(btnStart);
            AddButtonEffect(btnGuide);
            AddButtonEffect(btnSound);
            AddButtonEffect(btnAchievement);
            AddButtonEffect(btnExit);

            // Hieu ung tieu de.
            lblTitle.MouseEnter += lblTitle_MouseEnter;
            lblTitle.MouseLeave += lblTitle_MouseLeave;

            // Giai phong trinh phat khi dong form.
            FormClosed += frmMain_FormClosed;
        }

        #endregion

        #region Nhac nen

        private void InitializeBackgroundMusic()
        {
            string musicPath = Path.Combine(
                Application.StartupPath,
                "Assets",
                "Backgroundmusic.mp3"
            );

            if (!File.Exists(musicPath))
            {
                MessageBox.Show(
                    "Khong tim thay file nhac nen:\n" + musicPath +
                    "\n\nHay kiem tra thu muc Assets va Copy to Output Directory.",
                    "Loi nhac nen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            try
            {
                // Tranh khoi tao nhieu trinh phat.
                if (_musicPlayer != null)
                    return;

                _musicPlayer = new WindowsMediaPlayer();

                _musicPlayer.settings.volume = 30;
                _musicPlayer.settings.setMode("loop", true);
                _musicPlayer.settings.mute = !_soundEnabled;

                _musicPlayer.URL = musicPath;
                _musicPlayer.controls.play();
            }
            catch (Exception ex)
            {
                // Neu khoi tao that bai, giai phong doi tuong neu co.
                try
                {
                    _musicPlayer?.close();
                }
                catch
                {
                    // Bo qua loi trong qua trinh don dep.
                }

                _musicPlayer = null;

                MessageBox.Show(
                    "Khong the phat nhac nen:\n" + ex.Message,
                    "Loi am thanh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnSound_Click(object sender, EventArgs e)
        {
            // Dao trang thai am thanh.
            _soundEnabled = !_soundEnabled;

            // Cap nhat bieu tuong.
            btnSound.Text = _soundEnabled ? "🔊" : "🔇";

            btnSound.AccessibleDescription = _soundEnabled
                ? "Am thanh dang bat"
                : "Am thanh dang tat";

            // Bat/tat tieng nhac nen thuc te.
            if (_musicPlayer != null)
            {
                try
                {
                    _musicPlayer.settings.mute = !_soundEnabled;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Khong the thay doi trang thai am thanh:\n" +
                        ex.Message,
                        "Loi am thanh",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        private void frmMain_FormClosed(
            object? sender,
            FormClosedEventArgs e)
        {
            if (_musicPlayer != null)
            {
                try
                {
                    _musicPlayer.controls.stop();
                    _musicPlayer.close();
                }
                catch
                {
                    // Bo qua loi neu trinh phat da dong.
                }
                finally
                {
                    _musicPlayer = null;
                }
            }
        }

        #endregion

        #region Hieu ung nut

        private void AddButtonEffect(Button button)
        {
            button.Cursor = Cursors.Hand;

            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;
            button.MouseDown += Button_MouseDown;
            button.MouseUp += Button_MouseUp;
        }

        private void Button_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is not Button button)
                return;

            ResetButtonPosition(button);
            button.Top -= 2;
            button.BackColor = Color.MediumPurple;
        }

        private void Button_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is not Button button)
                return;

            ResetButtonPosition(button);
            ResetButtonColor(button);
        }

        private void Button_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (sender is not Button button)
                return;

            ResetButtonPosition(button);
        }

        private void Button_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (sender is not Button button)
                return;

            ResetButtonPosition(button);

            if (button.ClientRectangle.Contains(
                button.PointToClient(Cursor.Position)))
            {
                button.Top -= 2;
            }
        }

        private void ResetButtonPosition(Button button)
        {
            if (button == btnStart)
                button.Location = startPos;
            else if (button == btnGuide)
                button.Location = guidePos;
            else if (button == btnSound)
                button.Location = soundPos;
            else if (button == btnAchievement)
                button.Location = achievementPos;
            else if (button == btnExit)
                button.Location = exitPos;
        }

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

        #endregion

        #region Hieu ung tieu de

        private void lblTitle_MouseEnter(object? sender, EventArgs e)
        {
            lblTitle.Location = new Point(
                titleOriginalPos.X,
                titleOriginalPos.Y - 2
            );
        }

        private void lblTitle_MouseLeave(object? sender, EventArgs e)
        {
            lblTitle.Location = titleOriginalPos;
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            // Chua can xu ly.
        }

        #endregion

        #region Xu ly nut chinh

        private void btnStart_Click(object? sender, EventArgs e)
        {
            // Chan nhan lien tiep de khong mo hai form cung luc.
            if (_openingGame)
                return;

            _openingGame = true;
            btnStart.Enabled = false;

            try
            {
                // Nhap ten truoc khi bat dau de co the luu thanh tich.
                using (var nameForm = new TenNguoiChoiForm(_playerName))
                {
                    if (nameForm.ShowDialog(this) != DialogResult.OK)
                        return;

                    _playerName = nameForm.PlayerName;
                }

                // Truyen ten nguoi choi sang GameForm.
                using (var gameForm =
                    new AILATRIEUPHU.GameForm(_lastTopic, _gamesByTopic))
                {
                    gameForm.PlayerName = _playerName;

                    Hide();
                    gameForm.ShowDialog(this);

                    _lastTopic = gameForm.SelectedTopic;
                }
            }
            finally
            {
                // Luon khoi phuc man hinh chinh sau khi dong hoac huy.
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

        private void btnGuide_Click(object? sender, EventArgs e)
        {
            // Mo form huong dan nhu mot hop thoai.
            // Khi HuongDanForm dong, nguoi choi tro lai frmMain hien tai.
            using (HuongDanForm form = new HuongDanForm())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
        }
        private void btnAchievement_Click(object? sender, EventArgs e)
        {
            using (var form = new ThanhTichForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnExit_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitializeBackgroundMusic();
        }

        #endregion
    }
}
