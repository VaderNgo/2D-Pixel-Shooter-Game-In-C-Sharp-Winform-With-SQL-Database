using _2DPixelShooterGame.GameScripts;
using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.GameScreenScripts
{
    public class FailScreen
    {
        private FailScreen() { }
        private static FailScreen instance;
        private static readonly object _lock = new object();

        private Form FailModal;
        private Form BackgroundModal;

        private PictureBox FailPic;
        private Label Content_Label;
        private Button BackToMenu_Button;

        public bool isSetUp = false;
        public static FailScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new FailScreen();
                            instance.FailModal = new Form();
                            instance.FailPic = new PictureBox();
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
                SetFailModal();
                SetBackgroundModal();
                SetFailUI();
                SetEvents();
                isSetUp = true;
            }
        }
        private void SetFailModal()
        {
            FailModal.Text = "Fail";
            FailModal.StartPosition = FormStartPosition.CenterScreen;
            FailModal.MinimizeBox = false;
            FailModal.MaximizeBox = false;
            FailModal.ShowIcon = false;
            FailModal.Size = new Size(400, 400);
            FailModal.FormBorderStyle = FormBorderStyle.None;
            FailModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            FailModal.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetFailUI()
        {
            var fontTitle = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 15);
            var Title_Label = CustomControls.Instance.CustomLabel("FAIL", new Point(160, 20), Color.White, fontTitle, 1);

            FailPic.Size = new Size(64, 64);
            FailPic.Location = new Point(166, 76);
            FailPic.BackColor = Color.Transparent;
            FailPic.BackgroundImage = AssetsLoader.Instance.GOPlayer["FrontDeath"];
            FailPic.BackgroundImageLayout = ImageLayout.Stretch;

            Content_Label = CustomControls.Instance.CustomLabel(
                "      Don't give up\n" +
                "You can play again !",
                new Point(80, 158),
                Color.White,
                font,
                1);
            BackToMenu_Button = CustomControls.Instance.CustomBtn("Back To Menu", new Point(125, 310), Color.Red, font);
            FailModal.Controls.Add(Title_Label);
            FailModal.Controls.Add(Content_Label);
            FailModal.Controls.Add(FailPic);
            FailModal.Controls.Add(BackToMenu_Button);
        }

        private void SetEvents()
        {
            BackToMenu_Button.Click += BackToMenu_Click;
        }
        private void BackToMenu_Click(object sender, EventArgs e)
        {
            DoClose();
            GameUI.Instance.DoClose();
            GameManager.Instance.P.ClearData();
            WelcomeScreen.Instance.DoOpen();
        }
        public void DoOpen()
        {
            BackgroundModal.Show();
            FailModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            FailModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && FailModal.Visible)
                return true;
            return false;
        }
    }
}
