using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using GameTimer = System.Timers.Timer;
namespace _2DPixelShooterGame.GameScripts
{
    public class GameUI
    {
        private GameUI() { }
        private static GameUI instance;
        private static readonly object _lock = new object();
        public static GameUI Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new GameUI();
                        }
                    }
                }
                instance.SetUp();
                return instance;
            }
        }


        public Panel gameUI_Panel = new Panel();
        public PictureBox gameCanvas = new PictureBox();
        public GameTimer gameTimer = new GameTimer();

        public bool isSetUp = false;

        public void SetUp()
        {
            if (!isSetUp)
            {
                SetGameTimer();
                SetGameUIPanel();
                SetGameCanvas();
                SetEvents();
                isSetUp = true;
            }
        }

        private void SetGameTimer()
        {
            gameTimer.Enabled = true;
            gameTimer.AutoReset = true;
            gameTimer.Interval = 16;
            gameTimer.SynchronizingObject = gameCanvas;
        }

        private void SetGameUIPanel()
        {
            gameUI_Panel.Dock = DockStyle.Fill;
            GameInit.Instance.gW.Controls.Add(gameUI_Panel);
        }
        private void SetGameCanvas()
        {
            gameCanvas.Size = GameInit.Instance.gW.Size;
            gameCanvas.BackgroundImage = AssetsLoader.Instance.UIMaps[GameManager.Instance.Map_Name];
            gameUI_Panel.Controls.Add(gameCanvas);
        }
        private void SetEvents()
        {
            gameTimer.Elapsed += GameTimer_Elapsed;
            gameCanvas.Paint += GameCanvas_Paint;
        }

        //Events
        public void FrameChangedUpdate(object sender, EventArgs e)
        {
            gameCanvas.Invalidate();
        }
        public void GameTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            gameCanvas.Invalidate();
        }
        public void GameCanvas_Paint(object sender, PaintEventArgs e)
        {
            var font = new Font(AssetsLoader.Instance.Fonts.Families[0], 12);
            var hpImg = AssetsLoader.Instance.UIPanels["HPPanel"];
            var ammoImg = AssetsLoader.Instance.UIPanels["AmmoPanel"];
            var moneyImg = AssetsLoader.Instance.UIPanels["MoneyPanel"];
            var stagePanel = AssetsLoader.Instance.UIIconStages[GameManager.Instance.Stage <= 10 ? GameManager.Instance.Stage.ToString() : "10"];
            e.Graphics.DrawImage(hpImg, 10, 10, hpImg.Width, hpImg.Height);
            e.Graphics.DrawString(GameManager.Instance.P.GetHP() > 0 ? GameManager.Instance.P.GetHP().ToString() : "0", font, new SolidBrush(Color.DarkRed), 65, 17);
            e.Graphics.DrawImage(moneyImg, 10 * 2 + hpImg.Width, 10, moneyImg.Width, moneyImg.Height);
            e.Graphics.DrawString(GameManager.Instance.P.GetGold().ToString(), font, new SolidBrush(Color.LightGoldenrodYellow), 65 + hpImg.Width, 17);
            e.Graphics.DrawImage(ammoImg, 10 * 3 + hpImg.Width * 2, 10, ammoImg.Width, ammoImg.Height);
            e.Graphics.DrawString(GameManager.Instance.P.GetAmmo().ToString(), font, new SolidBrush(Color.LightGoldenrodYellow), 80 + hpImg.Width * 2, 17);
            e.Graphics.DrawImage(stagePanel, 1024 - 100, 10, stagePanel.Width, stagePanel.Height);
        }

        public void DoClose()
        {
            gameUI_Panel.Hide();
        }
        public void DoOpen()
        {
            gameUI_Panel.Show();
        }
    }
}
