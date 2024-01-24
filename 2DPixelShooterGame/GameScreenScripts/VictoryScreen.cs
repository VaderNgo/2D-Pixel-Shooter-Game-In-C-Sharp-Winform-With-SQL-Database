using _2DPixelShooterGame.GameScripts;
using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.GameScreenScripts
{
    public class VictoryScreen
    {
        private VictoryScreen() { }
        private static VictoryScreen instance;
        private static readonly object _lock = new object();

        private Form VictoryModal;
        private Form BackgroundModal;

        private PictureBox StarPic;
        private Label Content_Label;
        private Button BackToMenu_Button;

        public bool isSetUp = false;
        public static VictoryScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new VictoryScreen();
                            instance.VictoryModal = new Form();
                            instance.StarPic = new PictureBox();
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
                SetVictoryModal();
                SetBackgroundModal();
                SetVictoryUI();
                SetEvents();
                isSetUp = true;
            }
        }
        private void SetVictoryModal()
        {
            VictoryModal.Text = "Victory";
            VictoryModal.StartPosition = FormStartPosition.CenterScreen;
            VictoryModal.MinimizeBox = false;
            VictoryModal.MaximizeBox = false;
            VictoryModal.ShowIcon = false;
            VictoryModal.Size = new Size(400, 400);
            VictoryModal.FormBorderStyle = FormBorderStyle.None;
            VictoryModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            VictoryModal.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetVictoryUI()
        {
            var fontTitle = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 13);
            var Title_Label = CustomControls.Instance.CustomLabel("VICTORY", new Point(135, 20), Color.White, fontTitle, 1);

            StarPic.Size = new Size(64, 64);
            StarPic.Location = new Point(166, 76);
            StarPic.BackColor = Color.Transparent;
            StarPic.BackgroundImage = AssetsLoader.Instance.UIIcons["Star Icon"];
            StarPic.BackgroundImageLayout = ImageLayout.Stretch;

            Content_Label = CustomControls.Instance.CustomLabel(
                "Congratulation player: " + GameManager.Instance.P.GetPlayerName() + "\n" +
                "Map: " + GameManager.Instance.Map_Name + "\n" +
                "Start Time: \n" + GameManager.Instance.P.GetPlayerCreatedAt() + "\n" +
                "Finish Time: \n" + DateTime.Now.ToString(),
                new Point(80, 158),
                Color.White,
                font,
                1);
            BackToMenu_Button = CustomControls.Instance.CustomBtn("Back To Menu", new Point(125, 310), Color.Red, font);
            VictoryModal.Controls.Add(Title_Label);
            VictoryModal.Controls.Add(Content_Label);
            VictoryModal.Controls.Add(StarPic);
            VictoryModal.Controls.Add(BackToMenu_Button);
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


        public void UpdateUI()
        {
            Content_Label.Text = "Congratulation player: " + GameManager.Instance.P.GetPlayerName() + "\n" +
                "Map: " + GameManager.Instance.Map_Name + "\n" +
                "Start Time: \n" + "\t\t\t" + GameManager.Instance.P.GetPlayerCreatedAt() + "\n" +
                "Finish Time: \n" + "\t\t\t" + DateTime.Now.ToString();
            VictoryModal.Invalidate();
        }

        public void DoOpen()
        {
            BackgroundModal.Show();
            VictoryModal.Show();
            GameUI.Instance.gameTimer.Stop();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            VictoryModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && VictoryModal.Visible)
                return true;
            return false;
        }
    }
}
