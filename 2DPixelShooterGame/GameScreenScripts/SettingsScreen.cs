using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.GameScreenScripts
{
    public class SettingsScreen
    {
        private SettingsScreen() { }
        private static SettingsScreen instance;
        private static readonly object _lock = new object();
        private Form SettingModal;
        private Form BackgroundModal;

        private Panel Settings_Panel;
        private Label Easy;
        private Label Medium;
        private Label Hard;
        private PictureBox Exit;

        public int GameMode = 1;
        public bool isSetUp = false;
        public static SettingsScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SettingsScreen();
                        }
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }
        //Set Up
        public void SetUp()
        {
            isSetUp = true;
            SetSettingsModal();
            SetBackgroundModal();
            SetSettingsPanel();
            SetSettingsUI();
            SetEvents();
        }
        private void SetSettingsModal()
        {
            SettingModal = new Form();
            SettingModal.Size = new Size(400, 400);
            SettingModal.MinimizeBox = false;
            SettingModal.MaximizeBox = false;
            SettingModal.ShowIcon = false;
            SettingModal.StartPosition = FormStartPosition.CenterScreen;
            SettingModal.FormBorderStyle = FormBorderStyle.None;
            SettingModal.AutoSize = true;
        }
        private void SetSettingsPanel()
        {
            Settings_Panel = new Panel();
            Settings_Panel.Dock = DockStyle.Fill;
            Settings_Panel.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            Settings_Panel.BackgroundImageLayout = ImageLayout.Stretch;
            SettingModal.Controls.Add(Settings_Panel);
        }
        private void SetSettingsUI()
        {
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font1 = new Font(AssetsLoader.Instance.Fonts.Families[1], 17);

            var Title = CustomControls.Instance.CustomLabel("GAME MODE", new Point(100, 20), Color.White, font, 1);
            Easy = CustomControls.Instance.CustomLabel("Easy", new Point(10, 100), Color.White, font1, 1);
            Medium = CustomControls.Instance.CustomLabel("Medium", new Point(10, 180), Color.White, font1, 1);
            Hard = CustomControls.Instance.CustomLabel("Hard", new Point(10, 260), Color.White, font1, 1);

            Exit = new PictureBox();
            Exit.Size = new Size(30, 30);
            Exit.Location = new Point(SettingModal.Width - 50, 20);
            Exit.BackgroundImage = AssetsLoader.Instance.UIIcons["Exit Icon"];
            Exit.BackgroundImageLayout = ImageLayout.Stretch;

            SettingModal.Controls.Add(Settings_Panel);
            Settings_Panel.Controls.Add(Exit);
            Settings_Panel.Controls.Add(Title);
            Settings_Panel.Controls.Add(Easy);
            Settings_Panel.Controls.Add(Medium);
            Settings_Panel.Controls.Add(Hard);

            font.Dispose();
            font1.Dispose();
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetEvents()
        {
            Exit.Click += Exit_Click;
            Easy.MouseDown += Easy_MouseDown;
            Medium.MouseDown += Medium_MouseDown;
            Hard.MouseDown += Hard_MouseDown;
            SettingModal.Invalidated += SettingsModal_Invalidated;
        }
        //Events
        private void Exit_Click(object sender, EventArgs e)
        {
            DoClose();
        }
        private void Easy_MouseDown(object sender, EventArgs e)
        {
            this.GameMode = 1;
            SettingModal.Invalidate();
        }
        private void Medium_MouseDown(object sender, MouseEventArgs e)
        {
            this.GameMode = 2;
            SettingModal.Invalidate();
        }
        private void Hard_MouseDown(object sender, MouseEventArgs e)
        {
            this.GameMode = 3;
            SettingModal.Invalidate();
        }
        private void SettingsModal_Invalidated(object sender, InvalidateEventArgs e)
        {
            Easy.ForeColor = GameMode == 1 ? Color.IndianRed : Color.White;
            Easy.Location = new Point(GameMode == 1 ? 50 + 30 : 50, Easy.Location.Y);
            Medium.ForeColor = GameMode == 2 ? Color.IndianRed : Color.White;
            Medium.Location = new Point(GameMode == 2 ? 50 + 30 : 50, Medium.Location.Y);
            Hard.ForeColor = GameMode == 3 ? Color.IndianRed : Color.White;
            Hard.Location = new Point(GameMode == 3 ? 50 + 30 : 50, Hard.Location.Y);
        }
        //Behaviours
        public void DoOpen()
        {
            BackgroundModal.Show();
            SettingModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            SettingModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && SettingModal.Visible)
                return true;
            return false;
        }
    }
}
