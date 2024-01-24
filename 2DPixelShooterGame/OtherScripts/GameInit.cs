using _2DPixelShooterGame.GameScreenScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.OtherScripts
{
    public class GameInit
    {
        private GameInit() { }
        private static GameInit instance;
        private static readonly object _lock = new object();
        public Form gW;
        public static GameInit Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new GameInit();
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

        public void SetUp()
        {
            SetGameWindow();
            SetGameWindowUI();
        }
        public void SetGameWindow()
        {
            gW = new Form();
            gW.Name = "GameWindow";
            gW.Text = "2DPixelShooterGame";
            gW.Size = new Size(1024, 600);
            gW.StartPosition = FormStartPosition.CenterScreen;
            gW.MaximizeBox = false;
            gW.ShowIcon = false;
            SoundPlayer soundPlayer = new SoundPlayer(AssetsLoader.Instance.Audio["BG Audio"]);
            soundPlayer.PlayLooping();
        }
        public void SetGameWindowUI()
        {
            if (!WelcomeScreen.Instance.isSetUp)
                WelcomeScreen.Instance.SetUp();
        }
    }
}
