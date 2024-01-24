using _2DPixelShooterGame.GameScripts;
using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.GameScreenScripts
{
    public class PauseScreen
    {
        private PauseScreen() { }
        private static PauseScreen instance;
        private static readonly object _lock = new object();

        private Form PauseModal;
        private Form BackgroundModal;

        private Button BackToMenu_Button;
        private Button Resume_Button;

        public bool isSetUp = false;
        public static PauseScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new PauseScreen();
                            instance.PauseModal = new Form();
                            instance.SetUp();
                        }
                    }
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        private void SetUp()
        {
            if (!isSetUp)
            {
                SetPauseModal();
                SetBackgroundModal();
                SetPauseUI();
                SetEvents();
                isSetUp = true;
            }
        }
        private void SetPauseModal()
        {
            PauseModal.Text = "Pause";
            PauseModal.StartPosition = FormStartPosition.CenterScreen;
            PauseModal.MinimizeBox = false;
            PauseModal.MaximizeBox = false;
            PauseModal.ShowIcon = false;
            PauseModal.Size = new Size(400, 400);
            PauseModal.FormBorderStyle = FormBorderStyle.None;
            PauseModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            PauseModal.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetPauseUI()
        {
            var fontTitle = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 15);
            var Title_Label = CustomControls.Instance.CustomLabel("PAUSE", new Point(150, 20), Color.White, fontTitle, 1);

            Resume_Button = CustomControls.Instance.CustomBtn("Resume", new Point(150, 150), Color.Red, font);
            BackToMenu_Button = CustomControls.Instance.CustomBtn("Back To Menu", new Point(115, 250), Color.Red, font);
            PauseModal.Controls.Add(Title_Label);
            PauseModal.Controls.Add(Resume_Button);
            PauseModal.Controls.Add(BackToMenu_Button);
        }

        private void SetEvents()
        {
            BackToMenu_Button.Click += BackToMenu_Click;
            Resume_Button.Click += Resume_Click;
        }
        private void BackToMenu_Click(object sender, EventArgs e)
        {
            DoClose();
            GameUI.Instance.DoClose();
            GameManager.Instance.P.ClearData();
            GameManager.Instance.mobWave.ClearMobs();
            WelcomeScreen.Instance.DoOpen();
        }
        private void Resume_Click(object sender, EventArgs e)
        {
            DoClose();
            GameUI.Instance.gameTimer.Start();
        }
        public void UpdateUI()
        {
            PauseModal.Invalidate();
        }

        public void DoOpen()
        {
            BackgroundModal.Show();
            PauseModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            PauseModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && PauseModal.Visible)
                return true;
            return false;
        }
    }
}
