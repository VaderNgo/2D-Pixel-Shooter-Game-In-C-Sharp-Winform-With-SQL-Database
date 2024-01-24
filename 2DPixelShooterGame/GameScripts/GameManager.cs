using _2DPixelShooterGame.DatabaseScripts;
using _2DPixelShooterGame.GameScreenScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DPixelShooterGame.GameScripts
{
    public class GameManager
    {
        private GameManager() { }
        private static GameManager instance;
        private static readonly object _lock = new object();


        public string Player_ID;
        public string Map_ID;

        public string Map_Name;
        public int Stage;
        public int Difficulty;
        public char isVictory;
        public int Attempt;

        public Player P;
        public MobWave mobWave;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new GameManager();
                        }
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }

        public void StartNewGame(string PID)
        {
            this.Player_ID = PID;
            this.Map_Name = PlayNewScreen.Instance.Map_Selected_Name;
            this.Map_ID = PlayNewScreen.Instance.Map_ID;
            this.Stage = 1;
            this.Attempt = 1;
            this.isVictory = '0';
            this.Difficulty = SettingsScreen.Instance.GameMode;
            DatabaseController.Instance.UpdatePlayData(
                Player_ID,
                Map_ID,
                Stage,
                Difficulty,
                isVictory,
                Attempt
                );
            DatabaseController.Instance.UpdateOwnData(Player_ID, "W01");
            UpdateGameUI();
            Player.InitInstance(Player_ID);
            SetSpawner();
        }
        public void PlayGameAgain(string PID, string MID, int STAGE, int DIFFICULTY, char ISVICTORY, int ATTEMPT)
        {
            this.Player_ID = PID;
            this.Map_ID = MID;
            this.Map_Name = DatabaseController.Instance.GetMapName(Map_ID);
            this.Stage = STAGE;
            this.Difficulty = DIFFICULTY;
            this.isVictory = ISVICTORY;
            this.Attempt = ATTEMPT + 1;
            DatabaseController.Instance.UpdatePlayAttempt(PID, MID, ATTEMPT);
            UpdateGameUI();
            Player.InitInstance(Player_ID);
            SetSpawner();
        }
        public void SetSpawner()
        {
            GameUI.Instance.gameTimer.Elapsed += Spawner_Update;
        }
        public void CreateMob()
        {
            if (!P.IsPlayerDead())
            {
                if (mobWave == null)
                {
                    mobWave = new MobWave();
                    mobWave.Add(5 + Stage + SettingsScreen.Instance.GameMode, Stage);
                }
                else if (!mobWave.isCleared)
                {
                    if (mobWave.mobs.Count == 0 && Stage < 10)
                    {
                        Stage++;
                        mobWave.Add(5 + Stage + SettingsScreen.Instance.GameMode, Stage);
                        DatabaseController.Instance.UpdatePlayStage(Player_ID, Map_ID, Stage);
                    }
                    else if (mobWave.mobs.Count == 0 && Stage == 10)
                    {
                        DatabaseController.Instance.UpdatePlayIsVictory(Player_ID, Map_ID, '1');
                        Stage++;
                        if (!VictoryScreen.Instance.isOpenning())
                            VictoryScreen.Instance.DoOpen();
                    }
                }
                else if (mobWave.isCleared)
                {
                    mobWave.Add(5 + Stage + SettingsScreen.Instance.GameMode, Stage);
                    mobWave.isCleared = false;
                    if (mobWave.mobs.Count == 0 && Stage < 10)
                    {
                        Stage++;
                        mobWave.Add(5 + Stage + SettingsScreen.Instance.GameMode, Stage);
                        DatabaseController.Instance.UpdatePlayStage(Player_ID, Map_ID, Stage);
                    }
                    else if (mobWave.mobs.Count == 0 && Stage == 10)
                    {
                        DatabaseController.Instance.UpdatePlayIsVictory(Player_ID, Map_ID, '1');
                        Stage++;
                        if (!VictoryScreen.Instance.isOpenning())
                            VictoryScreen.Instance.DoOpen();
                    }
                }
            }
        }

        private void Spawner_Update(object sender, ElapsedEventArgs e)
        {
            CreateMob();
        }

        private void UpdateGameUI()
        {
            GameUI.Instance.DoOpen();
            GameUI.Instance.gameTimer.Start();
        }
    }
}
