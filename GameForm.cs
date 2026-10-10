using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using AiLaTrieuPhu;

namespace AILATRIEUPHU
{
    public partial class GameForm : Form
    {
        #region Trạng thái và cấu hình

        // Form chỉ quản lý hiển thị và lượt trợ giúp.
        // Chấm đáp án, tiến độ, tiền thưởng vẫn do GameRules quyết định.
        private GameRules _game;
        private QuestionTopic _selectedTopic;
        // Màn hình chính giữ bộ luật của từng chủ đề suốt lần mở ứng dụng.
        // Nhờ vậy, đóng cửa sổ chơi rồi mở lại vẫn nhớ câu của ván trước.
        private readonly IDictionary<QuestionTopic, GameRules> _gamesByTopic;

        // Cho màn hình chính nhớ chủ đề người chơi vừa chọn.
        public QuestionTopic SelectedTopic => _selectedTopic;
        private readonly Random _random = new Random();
        private readonly Panel[] _answerPanels;
        private readonly ToolTip _toolTip;
        private readonly ContextMenuStrip _topicMenu;

        private readonly Dictionary<Panel, AnswerUiState> _answerStates =
            new Dictionary<Panel, AnswerUiState>();
        private readonly HashSet<Panel> _hoveredPanels = new HashSet<Panel>();

        private bool _usedFiftyFifty;
        private bool _usedAudience;
        private bool _usedPhone;
        private bool _interactionBusy;
        private bool _isResultPopupVisible;
        private ResultPopupAction _resultPopupAction;

        private static readonly CultureInfo MoneyCulture =
            CultureInfo.GetCultureInfo("en-US");
        private static readonly Color SuccessColor = Color.FromArgb(70, 230, 135);
        private static readonly Color FailureColor = Color.FromArgb(255, 85, 95);
        private static readonly Color GoldColor = Color.FromArgb(245, 200, 70);
        private Color _resultAccentColor = Color.FromArgb(80, 160, 255);

        private enum AnswerUiState
        {
            Normal,
            Correct,
            Incorrect,
            Disabled
        }

        private enum ResultPopupAction
        {
            Continue,
            Replay
        }

        #endregion

        #region Khởi tạo và đăng ký sự kiện

        // Giữ cách mở Form hiện tại của Program.cs và trình thiết kế.
        public GameForm() : this(QuestionTopic.Programming)
        {
        }

        // Cửa sổ chọn chủ đề riêng, nếu có, có thể truyền chủ đề vào đây.
        public GameForm(QuestionTopic topic)
            : this(topic, new Dictionary<QuestionTopic, GameRules>())
        {
        }

        // Nhận bộ luật dùng chung từ màn hình chính; không đưa luật chơi vào nút bấm.
        public GameForm(QuestionTopic topic,
            IDictionary<QuestionTopic, GameRules> gamesByTopic)
        {
            _gamesByTopic = gamesByTopic ??
                throw new ArgumentNullException(nameof(gamesByTopic));
            _selectedTopic = topic;
            _game = GetOrStartGame(topic);

            InitializeComponent();
            DoubleBuffered = true;

            _answerPanels = new[] { btnAnswerA, btnAnswerB, btnAnswerC, btnAnswerD };

            // Designer sẽ tự giải phóng các thành phần này cùng Form.
            components ??= new Container();
            _toolTip = new ToolTip(components)
            {
                AutoPopDelay = 20000,
                InitialDelay = 400,
                ReshowDelay = 100
            };
            _topicMenu = new ContextMenuStrip(components);

            SetupGameUI();
            SetupResultPopupUI();
            SetupTopicMenu();
            SetupGameEvents();
            ResetLifelines();
            LoadQuestionToUI();
        }

        // Mỗi thao tác chính dùng một hàm xử lý có tên, dễ tìm và sửa.
        private void SetupGameEvents()
        {
            foreach (Panel panel in _answerPanels)
            {
                panel.Click += AnswerPanel_Click;
            }

            btnFiftyFifty.Click -= btnFiftyFifty_Click;
            btnFiftyFifty.Click += btnFiftyFifty_Click;
            btnAudience.Click -= btnAudience_Click;
            btnAudience.Click += btnAudience_Click;
            btnPhone.Click -= btnPhone_Click;
            btnPhone.Click += btnPhone_Click;
            btnStop.Click -= btnStop_Click;
            btnStop.Click += btnStop_Click;
            btnTopic.Click += TopicPanel_Click;
            btnClose.Click += ClosePanel_Click;
            pnlResultAction.Click += ResultAction_Click;

            // Bỏ liên kết rỗng mà Designer đã tạo cho nhãn này,
            // sau đó đăng ký đúng một lần hành động thực tế.
            lblResultAction.Click -= lblResultAction_Click;
            lblResultAction.Click += ResultAction_Click;
            Resize += FormResized;
        }

        private void ClosePanel_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void FormResized(object? sender, EventArgs e)
        {
            CenterResultPopup();
        }

        #endregion

        #region Chủ đề và vòng đời ván chơi

        // Dùng lại bộ luật nếu chủ đề đã được chơi để giữ cơ chế tránh lặp câu.
        // Lần đầu chọn chủ đề, hàm khởi tạo GameRules tự bắt đầu ván đầu tiên.
        private GameRules GetOrStartGame(QuestionTopic topic)
        {
            if (_gamesByTopic.TryGetValue(topic, out GameRules? game))
            {
                game.StartNewGame();
                return game;
            }

            game = new GameRules(new QuestionBank(topic));
            _gamesByTopic.Add(topic, game);
            return game;
        }

        private void SetupTopicMenu()
        {
            foreach (QuestionTopic topic in QuestionBank.GetAvailableTopics())
            {
                ToolStripMenuItem item = new ToolStripMenuItem(
                    QuestionBank.GetTopicName(topic))
                {
                    Tag = topic,
                    Checked = topic == _selectedTopic
                };
                item.Click += TopicItem_Click;
                _topicMenu.Items.Add(item);
            }

            _topicMenu.Opening += (sender, e) => e.Cancel = !CanChangeTopic();
            btnTopic.ContextMenuStrip = _topicMenu;
        }

        // Chỉ đổi chủ đề trước khi trả lời hoặc dùng trợ giúp ở đầu ván.
        // Tránh đổi chủ đề giữa ván để xóa tiến độ hay khôi phục trợ giúp.
        private bool CanChangeTopic()
        {
            return !_interactionBusy && !_isResultPopupVisible &&
                _game.CanAnswer && _game.CurrentQuestionNumber == 1 &&
                _game.CorrectCount == 0 &&
                !_usedFiftyFifty && !_usedAudience && !_usedPhone;
        }

        private void TopicPanel_Click(object? sender, EventArgs e)
        {
            if (CanChangeTopic())
            {
                _topicMenu.Show(btnTopic, new Point(0, btnTopic.Height));
            }
        }

        private void TopicItem_Click(object? sender, EventArgs e)
        {
            if (!CanChangeTopic() || sender is not ToolStripMenuItem item ||
                item.Tag is not QuestionTopic topic || topic == _selectedTopic)
            {
                return;
            }

            // Chuẩn bị ván mới trước khi thay đối tượng đang dùng.
            GameRules newGame = GetOrStartGame(topic);
            _game = newGame;
            _selectedTopic = topic;
            ResetLifelines();
            LoadQuestionToUI();
        }

        private void StartNewGameFromUI()
        {
            // Dùng lại GameRules để giữ danh sách mã của ván trước.
            _game.StartNewGame();
            HideResultPopup();
            ResetLifelines();
            LoadQuestionToUI();
        }

        private void ResetLifelines()
        {
            _usedFiftyFifty = false;
            _usedAudience = false;
            _usedPhone = false;
            SetPanelText(btnFiftyFifty, "50:50");
            SetPanelText(btnAudience, "Khán giả");
            SetPanelText(btnPhone, "Gọi điện");
        }

        private void LoadQuestionToUI()
        {
            Question question = _game.CurrentQuestion;
            string[] options = question.Options;

            SetPanelText(btnQuestionNumber,
                $"CÂU {_game.CurrentQuestionNumber}/{_game.PrizeLadder.Count}");
            SetPanelText(btnTopic,
                "CHỦ ĐỀ: " + QuestionBank.GetTopicName(_selectedTopic) + " ▾");
            SetPanelText(pnlQuestion, question.Text);
            _toolTip.SetToolTip(btnQuestionNumber, "Độ khó: " + GetDifficultyText(question.Difficulty));

            // Mỗi câu mới phải khôi phục cả chữ, màu và các lựa chọn bị 50:50 loại.
            for (int i = 0; i < _answerPanels.Length; i++)
            {
                Panel panel = _answerPanels[i];
                _answerStates[panel] = AnswerUiState.Normal;
                _hoveredPanels.Remove(panel);
                SetPanelText(panel, options[i]);
            }

            foreach (ToolStripMenuItem item in _topicMenu.Items)
            {
                item.Checked = item.Tag is QuestionTopic topic && topic == _selectedTopic;
            }

            RefreshMoneyDisplay();
            RefreshGameControls();
        }

        // Một chỗ duy nhất quyết định trạng thái bật/tắt của các nút.
        private void RefreshGameControls()
        {
            bool available = !_interactionBusy && !_isResultPopupVisible;
            bool canAnswer = available && _game.CanAnswer;

            foreach (Panel panel in _answerPanels)
            {
                SetPanelEnabled(panel,
                    canAnswer && _answerStates[panel] != AnswerUiState.Disabled);
            }

            SetPanelEnabled(btnStop, available && _game.CanStop);
            SetPanelEnabled(btnFiftyFifty, canAnswer && !_usedFiftyFifty);
            SetPanelEnabled(btnAudience, canAnswer && !_usedAudience);
            SetPanelEnabled(btnPhone, canAnswer && !_usedPhone);
            SetPanelEnabled(btnTopic, CanChangeTopic());
        }

        private void RefreshMoneyDisplay()
        {
            // Khi đã kết thúc, hiển thị tiền thực nhận, không giữ số tiền trước khi thua.
            int money = _game.FinalMoney ?? _game.CurrentMoney;
            SetPanelText(lblMoney, FormatMoney(money));
        }

        private bool CanUseLifeline(bool alreadyUsed)
        {
            return !alreadyUsed && !_interactionBusy &&
                !_isResultPopupVisible && _game.CanAnswer;
        }

        #endregion

        #region Trả lời và dừng chơi

        private void AnswerPanel_Click(object? sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                SubmitAnswerFromUI(Array.IndexOf(_answerPanels, panel));
            }
        }

        private void SubmitAnswerFromUI(int answerIndex)
        {
            if (_interactionBusy || _isResultPopupVisible || !_game.CanAnswer ||
                answerIndex < 0 || answerIndex >= _answerPanels.Length ||
                _answerStates[_answerPanels[answerIndex]] == AnswerUiState.Disabled)
            {
                return;
            }

            _interactionBusy = true;
            RefreshGameControls();

            try
            {
                Question question = _game.CurrentQuestion;
                AnswerResult result = _game.SubmitAnswer(question.Id, answerIndex);
                if (result == AnswerResult.Invalid)
                {
                    return;
                }

                RefreshMoneyDisplay();
                if (result == AnswerResult.Correct)
                {
                    SetAnswerState(answerIndex, AnswerUiState.Correct);

                    if (_game.State == GameState.Won)
                    {
                        ShowResultPopup("CHÚC MỪNG!",
                            $"Bạn đã chinh phục toàn bộ {_game.PrizeLadder.Count} câu hỏi!\n\n" +
                            $"Tiền thưởng: {FormatMoney(_game.FinalMoney ?? 0)}",
                            GoldColor, "CHƠI LẠI", ResultPopupAction.Replay);
                    }
                    else
                    {
                        ShowResultPopup("CHÍNH XÁC!",
                            "Bạn đã trả lời đúng!\n\n" +
                            $"Tiền hiện tại: {FormatMoney(_game.CurrentMoney)}",
                            SuccessColor, "TIẾP TỤC", ResultPopupAction.Continue);
                    }
                }
                else
                {
                    SetAnswerState(answerIndex, AnswerUiState.Incorrect);
                    SetAnswerState(question.CorrectAnswerIndex, AnswerUiState.Correct);

                    ShowResultPopup("RẤT TIẾC!",
                        "Bạn đã trả lời sai.\n\n" +
                        $"Đáp án đúng: {GetAnswerLetter(question.CorrectAnswerIndex)}\n" +
                        $"Tiền nhận được: {FormatMoney(_game.FinalMoney ?? 0)}",
                        FailureColor, "CHƠI LẠI", ResultPopupAction.Replay);
                }
            }
            finally
            {
                _interactionBusy = false;
                RefreshGameControls();
            }
        }

        private void btnStop_Click(object? sender, EventArgs e)
        {
            if (_interactionBusy || _isResultPopupVisible || !_game.CanStop)
            {
                return;
            }

            _interactionBusy = true;
            RefreshGameControls();

            try
            {
                DialogResult result = MessageBox.Show(this,
                    "Bạn có chắc muốn dừng cuộc chơi?",
                    "Dừng cuộc chơi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes && _game.StopGame(_game.CurrentQuestion.Id))
                {
                    RefreshMoneyDisplay();
                    ShowResultPopup("DỪNG CUỘC CHƠI",
                        "Bạn đã quyết định dừng cuộc chơi.\n\n" +
                        $"Tiền nhận được: {FormatMoney(_game.FinalMoney ?? 0)}",
                        GoldColor, "CHƠI LẠI", ResultPopupAction.Replay);
                }
            }
            finally
            {
                _interactionBusy = false;
                RefreshGameControls();
            }
        }

        #endregion

        #region Ba quyền trợ giúp

        private void btnFiftyFifty_Click(object? sender, EventArgs e)
        {
            if (!CanUseLifeline(_usedFiftyFifty))
            {
                return;
            }

            int[] hiddenIndexes = _game.GetTwoWrongAnswerIndexes();
            if (hiddenIndexes.Length != 2)
            {
                return;
            }

            _usedFiftyFifty = true;
            foreach (int index in hiddenIndexes)
            {
                SetAnswerState(index, AnswerUiState.Disabled);
                SetPanelText(_answerPanels[index], string.Empty);
            }

            SetPanelText(btnFiftyFifty, "50:50 ✓");
            RefreshGameControls();
        }

        private void btnAudience_Click(object? sender, EventArgs e)
        {
            if (!CanUseLifeline(_usedAudience))
            {
                return;
            }

            int? correctIndex = _game.GetCorrectAnswerIndex();
            if (!correctIndex.HasValue)
            {
                return;
            }

            int[] values = CreateAudienceVotes(correctIndex.Value);
            _usedAudience = true;
            SetPanelText(btnAudience, "Khán giả ✓");

            ShowLifelineMessage("Hỏi ý kiến khán giả",
                "Kết quả khán giả:\n\n" +
                $"A: {values[0]}%\nB: {values[1]}%\n" +
                $"C: {values[2]}%\nD: {values[3]}%");
        }

        // Đây là kết quả khán giả mô phỏng, không phải bình chọn trực tuyến.
        // Tổng luôn bằng 100%; đáp án đã bị 50:50 loại luôn nhận 0%.
        private int[] CreateAudienceVotes(int correctIndex)
        {
            int[] values = new int[4];
            List<int> remainingWrongIndexes = new List<int>();

            for (int i = 0; i < _answerPanels.Length; i++)
            {
                if (i != correctIndex &&
                    _answerStates[_answerPanels[i]] != AnswerUiState.Disabled)
                {
                    remainingWrongIndexes.Add(i);
                }
            }

            if (remainingWrongIndexes.Count == 0)
            {
                values[correctIndex] = 100;
                return values;
            }

            values[correctIndex] = _random.Next(55, 76);
            int remaining = 100 - values[correctIndex];

            // Trộn thứ tự phân chia để không luôn ưu tiên cùng một chữ cái.
            for (int i = remainingWrongIndexes.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                int temporary = remainingWrongIndexes[i];
                remainingWrongIndexes[i] = remainingWrongIndexes[j];
                remainingWrongIndexes[j] = temporary;
            }

            for (int i = 0; i < remainingWrongIndexes.Count; i++)
            {
                int share = i == remainingWrongIndexes.Count - 1
                    ? remaining
                    : _random.Next(remaining + 1);
                values[remainingWrongIndexes[i]] = share;
                remaining -= share;
            }

            return values;
        }

        private void btnPhone_Click(object? sender, EventArgs e)
        {
            if (!CanUseLifeline(_usedPhone))
            {
                return;
            }

            int? correctIndex = _game.GetCorrectAnswerIndex();
            if (!correctIndex.HasValue)
            {
                return;
            }

            _usedPhone = true;
            SetPanelText(btnPhone, "Gọi điện ✓");
            ShowLifelineMessage("Gọi điện cho người thân",
                "Người thân nói:\n\n" +
                $"\"Tôi khá chắc đáp án là {GetAnswerLetter(correctIndex.Value)}.\"");
        }

        // Gắn hộp thông báo với Form chủ và khóa thao tác trong lúc hộp đang mở.
        private void ShowLifelineMessage(string title, string message)
        {
            _interactionBusy = true;
            RefreshGameControls();
            try
            {
                MessageBox.Show(this, message, title,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _interactionBusy = false;
                RefreshGameControls();
            }
        }

        #endregion

        #region Bảng kết quả và nút tiếp tục

        private void ShowResultPopup(string title, string message, Color accentColor,
            string actionText, ResultPopupAction action)
        {
            _resultAccentColor = accentColor;
            _resultPopupAction = action;
            _isResultPopupVisible = true;

            lblResultTitle.Text = title;
            lblResultTitle.ForeColor = accentColor;
            lblResultMessage.Text = message;
            lblResultAction.Text = actionText;
            _toolTip.SetToolTip(lblResultMessage, message);

            CenterResultPopup();
            pnlResult.Visible = true;
            pnlResult.BringToFront();
            pnlResult.Invalidate();
            pnlResultAction.Invalidate();
            RefreshGameControls();
        }

        private void HideResultPopup()
        {
            _isResultPopupVisible = false;
            pnlResult.Visible = false;
        }

        private void ResultAction_Click(object? sender, EventArgs e)
        {
            if (!_isResultPopupVisible || _interactionBusy)
            {
                return;
            }

            _interactionBusy = true;
            try
            {
                if (_resultPopupAction == ResultPopupAction.Continue)
                {
                    // Chỉ ẩn bảng khi chuyển câu thành công, tránh để ván bị kẹt.
                    if (_game.MoveToNextQuestion())
                    {
                        HideResultPopup();
                        LoadQuestionToUI();
                    }
                }
                else
                {
                    StartNewGameFromUI();
                }
            }
            finally
            {
                _interactionBusy = false;
                RefreshGameControls();
            }
        }

        private void CenterResultPopup()
        {
            pnlResult.Location = new Point(
                Math.Max(0, (ClientSize.Width - pnlResult.Width) / 2),
                Math.Max(0, (ClientSize.Height - pnlResult.Height) / 2));
        }

        #endregion

        #region Tiện ích hiển thị

        private static string FormatMoney(int money)
        {
            return money.ToString("C0", MoneyCulture);
        }

        private static string GetAnswerLetter(int index)
        {
            return index >= 0 && index < 4 ? ((char)('A' + index)).ToString() : "?";
        }

        private static string GetDifficultyText(QuestionDifficulty difficulty)
        {
            return difficulty switch
            {
                QuestionDifficulty.Easy => "Dễ",
                QuestionDifficulty.Medium => "Trung bình",
                QuestionDifficulty.Hard => "Khó",
                _ => "Không xác định"
            };
        }

        private void SetPanelText(Panel panel, string text)
        {
            panel.Tag = text;
            panel.AccessibleName = text;
            _toolTip.SetToolTip(panel, text);
            panel.Invalidate();
        }

        private void SetPanelEnabled(Panel panel, bool enabled)
        {
            panel.Enabled = enabled;
            if (!enabled)
            {
                _hoveredPanels.Remove(panel);
            }
            panel.Invalidate();
        }

        private void SetAnswerState(int index, AnswerUiState state)
        {
            Panel panel = _answerPanels[index];
            _answerStates[panel] = state;
            _hoveredPanels.Remove(panel);
            panel.Invalidate();
        }

        #endregion

        #region Bố cục và vẽ giao diện

        private void SetupGameUI()
        {
            StyleTopPanel(btnQuestionNumber, "CÂU 1/10");

            // SPEC hiện tại chưa có luật hết giờ. Không hiển thị số 30 giả.
            StyleTopPanel(btnTimer, "—");
            _toolTip.SetToolTip(btnTimer, "Ván hiện tại không giới hạn thời gian.");

            // Tận dụng khoảng trống có sẵn để tên chủ đề dễ đọc hơn.
            int topicWidth = btnFiftyFifty.Left - btnTopic.Left - 12;
            if (topicWidth > btnTopic.Width)
            {
                btnTopic.Width = topicWidth;
            }

            StyleTopPanel(btnTopic, "CHỦ ĐỀ ▾", true);
            StyleTopPanel(btnFiftyFifty, "50:50", true);
            StyleTopPanel(btnAudience, "Khán giả", true);
            StyleTopPanel(btnPhone, "Gọi điện", true);
            StyleTopPanel(btnStop, "Dừng cuộc chơi", true, true);
            StyleClosePanel(btnClose);
            StyleMoneyPanel(lblMoney);
            StyleQuestionPanel(pnlQuestion);

            for (int i = 0; i < _answerPanels.Length; i++)
            {
                StyleAnswerPanel(_answerPanels[i], GetAnswerLetter(i));
            }
        }

        private void SetupResultPopupUI()
        {
            pnlResult.BackColor = Color.Transparent;
            pnlResultAction.BackColor = Color.Transparent;
            pnlResultAction.Cursor = Cursors.Hand;
            pnlResult.Visible = false;

            lblResultTitle.BackColor = Color.Transparent;
            lblResultTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblResultMessage.BackColor = Color.Transparent;
            lblResultMessage.ForeColor = Color.White;
            lblResultMessage.TextAlign = ContentAlignment.MiddleCenter;
            lblResultAction.BackColor = Color.Transparent;
            lblResultAction.ForeColor = Color.White;
            lblResultAction.Cursor = Cursors.Hand;
            lblResultAction.TextAlign = ContentAlignment.MiddleCenter;
            pnlResultAction.Left = Math.Max(0, (pnlResult.Width - pnlResultAction.Width) / 2);

            pnlResult.Paint += (sender, e) =>
            {
                if (!TryGetPaintRectangle(pnlResult, out Rectangle rect)) return;
                DrawSurface(e.Graphics, rect, false, 20,
                    Color.FromArgb(7, 15, 45), Color.FromArgb(7, 15, 45),
                    _resultAccentColor, 2.2f);
            };

            pnlResultAction.Paint += (sender, e) =>
            {
                if (!TryGetPaintRectangle(pnlResultAction, out Rectangle rect)) return;
                DrawSurface(e.Graphics, rect, false, 14,
                    Color.FromArgb(25, 45, 135), Color.FromArgb(8, 18, 70),
                    _resultAccentColor, 1.5f);
            };

            TrackShape(pnlResult, false, 20);
            TrackShape(pnlResultAction, false, 14);
            CenterResultPopup();
        }

        // Các hàm tạo kiểu chỉ đăng ký một lần trong hàm khởi tạo.
        // Thay nội dung khi chơi bằng SetPanelText, không đăng ký lại Paint.
        private void StyleTopPanel(Panel panel, string text,
            bool clickable = false, bool goldBorder = false)
        {
            PreparePanel(panel, clickable);
            SetPanelText(panel, text);
            panel.Paint += (sender, e) =>
            {
                if (!TryGetPaintRectangle(panel, out Rectangle rect)) return;
                bool hovered = panel.Enabled && _hoveredPanels.Contains(panel);
                DrawSurface(e.Graphics, rect, false, 10,
                    hovered ? Color.FromArgb(30, 50, 105) : Color.FromArgb(10, 18, 50),
                    hovered ? Color.FromArgb(18, 32, 80) : Color.FromArgb(6, 12, 38),
                    goldBorder ? Color.FromArgb(190, 150, 55) : Color.FromArgb(70, 100, 170),
                    1.2f);

                Color color = !panel.Enabled ? Color.FromArgb(145, 150, 165) :
                    goldBorder ? Color.FromArgb(245, 230, 190) : Color.White;
                DrawFittedText(e.Graphics, panel.Tag?.ToString() ?? string.Empty,
                    Rectangle.Inflate(rect, -3, -1), color, 8.5f, 7f, false, true);
            };
            TrackShape(panel, false, 10);
        }

        private void StyleAnswerPanel(Panel panel, string letter)
        {
            PreparePanel(panel, true);
            _answerStates[panel] = AnswerUiState.Normal;

            panel.Paint += (sender, e) =>
            {
                if (!TryGetPaintRectangle(panel, out Rectangle rect)) return;
                AnswerUiState state = _answerStates[panel];
                bool hovered = panel.Enabled && _hoveredPanels.Contains(panel);
                GetAnswerColors(state, hovered, out Color top, out Color bottom, out Color border);
                DrawSurface(e.Graphics, rect, true, 22, top, bottom, border,
                    hovered && state == AnswerUiState.Normal ? 2f : 1.4f);

                Rectangle badge = new Rectangle(22, (panel.Height - 32) / 2, 32, 32);
                using SolidBrush badgeBrush = new SolidBrush(Color.FromArgb(8, 18, 70));
                using Pen badgePen = new Pen(Color.FromArgb(90, 120, 230), 1.2f);
                e.Graphics.FillEllipse(badgeBrush, badge);
                e.Graphics.DrawEllipse(badgePen, badge);
                DrawFittedText(e.Graphics, letter, badge, Color.White, 10f, 8f, false, true);

                Rectangle textArea = new Rectangle(66, 4,
                    Math.Max(1, panel.Width - 90), Math.Max(1, panel.Height - 8));
                DrawFittedText(e.Graphics, panel.Tag?.ToString() ?? string.Empty,
                    textArea, Color.White, 11f, 8f, true, false);
            };
            TrackShape(panel, true, 22);
        }

        private static void GetAnswerColors(AnswerUiState state, bool hovered,
            out Color top, out Color bottom, out Color border)
        {
            switch (state)
            {
                case AnswerUiState.Correct:
                    top = Color.FromArgb(35, 180, 90);
                    bottom = Color.FromArgb(10, 90, 50);
                    border = Color.FromArgb(100, 255, 160);
                    break;
                case AnswerUiState.Incorrect:
                    top = Color.FromArgb(200, 50, 60);
                    bottom = Color.FromArgb(100, 15, 25);
                    border = Color.FromArgb(255, 110, 120);
                    break;
                case AnswerUiState.Disabled:
                    top = Color.FromArgb(35, 38, 55);
                    bottom = Color.FromArgb(18, 20, 35);
                    border = Color.FromArgb(60, 65, 80);
                    break;
                default:
                    top = hovered ? Color.FromArgb(45, 70, 225) : Color.FromArgb(29, 45, 165);
                    bottom = hovered ? Color.FromArgb(17, 35, 145) : Color.FromArgb(8, 17, 88);
                    border = hovered ? Color.FromArgb(100, 160, 255) : Color.FromArgb(55, 90, 220);
                    break;
            }
        }

        private void StyleMoneyPanel(Panel panel)
        {
            PreparePanel(panel, false);
            panel.Paint += (sender, e) =>
            {
                if (!TryGetPaintRectangle(panel, out Rectangle rect)) return;
                DrawSurface(e.Graphics, rect, false, 12,
                    Color.FromArgb(50, 39, 15), Color.FromArgb(18, 17, 38),
                    Color.FromArgb(220, 175, 55), 1.5f);
                DrawFittedText(e.Graphics, panel.Tag?.ToString() ?? string.Empty,
                    Rectangle.Inflate(rect, -5, -2), Color.Gold, 11f, 8f, false, true);
            };
            TrackShape(panel, false, 12);
        }

        private void StyleQuestionPanel(Panel panel)
        {
            PreparePanel(panel, false);
            panel.Paint += (sender, e) =>
            {
                if (!TryGetPaintRectangle(panel, out Rectangle rect)) return;
                DrawSurface(e.Graphics, rect, true, 28,
                    Color.FromArgb(31, 48, 180), Color.FromArgb(9, 18, 86),
                    Color.FromArgb(55, 90, 230), 2f);
                Rectangle textArea = new Rectangle(42, 5,
                    Math.Max(1, panel.Width - 84), Math.Max(1, panel.Height - 10));
                DrawFittedText(e.Graphics, panel.Tag?.ToString() ?? string.Empty,
                    textArea, Color.White, 15f, 8f, true, true);
            };
            TrackShape(panel, true, 28);
        }

        private void StyleClosePanel(Panel panel)
        {
            PreparePanel(panel, true);
            panel.AccessibleName = "Đóng trò chơi";
            _toolTip.SetToolTip(panel, "Đóng trò chơi");
            panel.Paint += (sender, e) =>
            {
                if (!TryGetPaintRectangle(panel, out Rectangle rect)) return;
                bool hovered = _hoveredPanels.Contains(panel);
                Color background = hovered ? Color.FromArgb(135, 35, 45) : Color.FromArgb(10, 18, 45);
                Color border = hovered ? Color.FromArgb(240, 90, 100) : Color.FromArgb(70, 85, 135);
                DrawSurface(e.Graphics, rect, false, 8, background, background, border, 1f);
                DrawFittedText(e.Graphics, "×", rect, Color.White, 11f, 8f, false, true);
            };
            TrackShape(panel, false, 8);
        }

        private void PreparePanel(Panel panel, bool clickable)
        {
            panel.BackColor = Color.Transparent;
            panel.Cursor = clickable ? Cursors.Hand : Cursors.Default;
            if (!clickable) return;

            panel.MouseEnter += (sender, e) =>
            {
                if (panel.Enabled)
                {
                    _hoveredPanels.Add(panel);
                    panel.Invalidate();
                }
            };
            panel.MouseLeave += (sender, e) =>
            {
                _hoveredPanels.Remove(panel);
                panel.Invalidate();
            };
        }

        // Xuống dòng và thu nhỏ vừa đủ để đọc câu/đáp án dài.
        // NoPrefix bảo đảm ký tự & trong câu hỏi lập trình không bị ẩn.
        private static void DrawFittedText(Graphics graphics, string text, Rectangle area,
            Color color, float maximumSize, float minimumSize, bool wrap, bool centered)
        {
            if (area.Width <= 0 || area.Height <= 0 || string.IsNullOrEmpty(text)) return;

            TextFormatFlags measureFlags = TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding |
                (wrap ? TextFormatFlags.WordBreak : TextFormatFlags.SingleLine);
            TextFormatFlags drawFlags = measureFlags | TextFormatFlags.EndEllipsis |
                (centered ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Left);
            if (!wrap)
            {
                drawFlags |= TextFormatFlags.VerticalCenter;
            }

            for (float size = maximumSize; size >= minimumSize; size -= 0.5f)
            {
                using Font font = new Font("Segoe UI", size, FontStyle.Bold);
                Size measured = TextRenderer.MeasureText(graphics, text, font,
                    new Size(area.Width, int.MaxValue), measureFlags);
                if ((measured.Width <= area.Width && measured.Height <= area.Height) ||
                    size <= minimumSize)
                {
                    // Tự căn giữa theo chiều dọc khi có nhiều dòng.
                    Rectangle drawArea = area;
                    if (wrap && measured.Height < area.Height)
                    {
                        drawArea.Y += (area.Height - measured.Height) / 2;
                        drawArea.Height = measured.Height;
                    }
                    TextRenderer.DrawText(graphics, text, font, drawArea, color, drawFlags);
                    return;
                }
            }
        }

        #endregion

        #region Hình học và tài nguyên vẽ

        // Không tạo cọ chuyển màu với hình chữ nhật rỗng khi cửa sổ co nhỏ.
        private static bool TryGetPaintRectangle(Panel panel, out Rectangle rect)
        {
            rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            return rect.Width > 0 && rect.Height > 0;
        }

        private static void DrawSurface(Graphics graphics, Rectangle rect, bool hexagon,
            int cornerSize, Color top, Color bottom, Color border, float borderWidth)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using GraphicsPath path = hexagon
                ? CreateHexagonPath(rect, cornerSize)
                : CreateRoundedRectangle(rect, cornerSize);
            using LinearGradientBrush brush = new LinearGradientBrush(rect, top, bottom, 90f);
            using Pen pen = new Pen(border, borderWidth);
            graphics.FillPath(brush, path);
            graphics.DrawPath(pen, path);
        }

        private static GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (rect.Width <= 0 || rect.Height <= 0) return path;

            int diameter = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));
            if (diameter < 2)
            {
                path.AddRectangle(rect);
                return path;
            }

            path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateHexagonPath(Rectangle rect, int cut)
        {
            GraphicsPath path = new GraphicsPath();
            if (rect.Width <= 0 || rect.Height <= 0) return path;

            cut = Math.Max(0, Math.Min(cut, rect.Width / 3));
            path.AddPolygon(new[]
            {
                new Point(rect.Left + cut, rect.Top),
                new Point(rect.Right - cut, rect.Top),
                new Point(rect.Right, rect.Top + rect.Height / 2),
                new Point(rect.Right - cut, rect.Bottom),
                new Point(rect.Left + cut, rect.Bottom),
                new Point(rect.Left, rect.Top + rect.Height / 2)
            });
            path.CloseFigure();
            return path;
        }

        private static void TrackShape(Panel panel, bool hexagon, int cornerSize)
        {
            panel.Resize += (sender, e) => ApplyPanelRegion(panel, hexagon, cornerSize);
            ApplyPanelRegion(panel, hexagon, cornerSize);
        }

        // Giải phóng vùng vẽ cũ để không tích lũy tài nguyên khi đổi kích thước.
        private static void ApplyPanelRegion(Panel panel, bool hexagon, int cornerSize)
        {
            if (panel.Width <= 0 || panel.Height <= 0) return;

            Rectangle rect = new Rectangle(Point.Empty, panel.Size);
            using GraphicsPath path = hexagon
                ? CreateHexagonPath(rect, cornerSize)
                : CreateRoundedRectangle(rect, cornerSize);
            Region? oldRegion = panel.Region;
            panel.Region = new Region(path);
            oldRegion?.Dispose();
            panel.Invalidate();
        }

        #endregion

        #region Các liên kết cũ của Designer

        // Giữ các tên này để GameForm.Designer.cs hiện tại vẫn mở và biên dịch được.
        // Phần vẽ và hành động thật đã được đăng ký trong các hàm Setup phía trên.
        private void pnlTop_Paint(object? sender, PaintEventArgs e) { }
        private void panel2_Paint(object? sender, PaintEventArgs e) { }
        private void Form1_Load(object? sender, EventArgs e) { }
        private void pnlResult_Paint(object? sender, PaintEventArgs e) { }
        private void panel1_Paint(object? sender, PaintEventArgs e) { }
        private void label1_Click(object? sender, EventArgs e) { }
        private void lblResultAction_Click(object? sender, EventArgs e) { }
        private void pnlResult_Paint_1(object? sender, PaintEventArgs e) { }

        #endregion
    }
}
