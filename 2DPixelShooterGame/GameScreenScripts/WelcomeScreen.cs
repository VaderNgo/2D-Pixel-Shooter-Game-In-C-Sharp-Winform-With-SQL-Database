using _2DPixelShooterGame.DatabaseScripts;
using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.GameScreenScripts
{
    public class WelcomeScreen
    {
        private WelcomeScreen() { }
        private static WelcomeScreen instance;
        private static readonly object _lock = new object();
        private Panel WelcomeScreenPanel;
        private Label GameTitle;
        private Label PlayNew;
        private Label LoadPlay;
        private Label Guide;
        private Label Settings;
        public bool isSetUp = false;
        public static WelcomeScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new WelcomeScreen();
                            instance.WelcomeScreenPanel = new Panel();
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
        public void SetUp()
        {
            SetWelcomeScreenPanel();
            SetWelcomeScreenUI();
            SetEvent();
            isSetUp = true;
        }
        private void SetWelcomeScreenPanel()
        {
            WelcomeScreenPanel.Dock = DockStyle.Fill;
            WelcomeScreenPanel.BackgroundImage = AssetsLoader.Instance.UIBG["GameBG"];
            WelcomeScreenPanel.BackgroundImageLayout = ImageLayout.Stretch;
            GameInit.Instance.gW.Controls.Add(WelcomeScreenPanel);
        }
        private void SetWelcomeScreenUI()
        {
            var titleFont = new Font(AssetsLoader.Instance.Fonts.Families[1], 30);
            GameTitle = CustomControls.Instance.CustomLabel("The Last Man", new Point(550, 50), Color.Red, titleFont, 1);

            var sectionFont = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            PlayNew = CustomControls.Instance.CustomLabel("Play New", new Point(50, 150), Color.White, sectionFont, 0);
            LoadPlay = CustomControls.Instance.CustomLabel("Load Play", new Point(50, 200), Color.White, sectionFont, 0);
            Guide = CustomControls.Instance.CustomLabel("Guide", new Point(50, 250), Color.White, sectionFont, 0);
            Settings = CustomControls.Instance.CustomLabel("Settings", new Point(50, 300), Color.White, sectionFont, 0);

            WelcomeScreenPanel.Controls.Add(GameTitle);
            WelcomeScreenPanel.Controls.Add(PlayNew);
            WelcomeScreenPanel.Controls.Add(LoadPlay);
            WelcomeScreenPanel.Controls.Add(Guide);
            WelcomeScreenPanel.Controls.Add(Settings);

            titleFont.Dispose();
            sectionFont.Dispose();
        }
        private void SetEvent()
        {
            Settings.MouseDown += Setting_MouseDown;
            LoadPlay.MouseDown += LoadPlay_MouseDown;
            PlayNew.MouseDown += PlayNew_MouseDown;
            Guide.MouseDown += Guide_MouseDown;
        }
        private void Setting_MouseDown(object sender, EventArgs e)
        {
            if (!SettingsScreen.Instance.isSetUp)
                SettingsScreen.Instance.SetUp();
            SettingsScreen.Instance.DoOpen();
        }
        private void LoadPlay_MouseDown(object sender, EventArgs e)
        {
            if (DatabaseController.Instance.sqlConnection == null)
            {
                if (!DatabaseConfiguration.Instance.isSetUp)
                    DatabaseConfiguration.Instance.SetUp();
                DatabaseConfiguration.Instance.DoOpen();
                return;
            }
            if (!LoadPlayScreen.Instance.isSetUp)
                LoadPlayScreen.Instance.SetUp();
            LoadPlayScreen.Instance.DoOpen();
        }
        private void PlayNew_MouseDown(object sender, EventArgs e)
        {
            if (DatabaseController.Instance.sqlConnection == null)
            {
                if (!DatabaseConfiguration.Instance.isSetUp)
                    DatabaseConfiguration.Instance.SetUp();
                DatabaseConfiguration.Instance.DoOpen();
                return;
            }
            if (!PlayNewScreen.Instance.isSetUp)
                PlayNewScreen.Instance.SetUp();
            PlayNewScreen.Instance.DoOpen();
        }
        private void Guide_MouseDown(object sender, EventArgs e)
        {
            if (!GuideScreen.Instance.isSetUp)
                GuideScreen.Instance.SetUp();
            GuideScreen.Instance.DoOpen();
        }
        public void DoClose()
        {
            WelcomeScreenPanel.Hide();
        }
        public void DoOpen()
        {
            WelcomeScreenPanel.Show();
        }
    }
}
