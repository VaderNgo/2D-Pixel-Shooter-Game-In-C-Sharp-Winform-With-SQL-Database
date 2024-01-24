using _2DPixelShooterGame.GameScreenScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DPixelShooterGame.GameScripts
{
    public class MobWave
    {
        public List<Mob> mobs = new List<Mob>();
        private Random rdn = new Random();
        public bool isCleared = false;
        public MobWave()
        {
            GameUI.Instance.gameTimer.Elapsed += CheckDead;
        }
        public void CheckDead(object sender, ElapsedEventArgs e)
        {
            for (int i = mobs.Count - 1; i >= 0; i--)
            {
                if (mobs[i].IsMobDead())
                {
                    mobs[i].ClearData();
                    mobs.RemoveAt(i);
                }
            }
        }
        public void ClearMobs()
        {
            for (int i = mobs.Count - 1; i >= 0; i--)
            {
                mobs[i].ClearData();
                mobs.RemoveAt(i);
            }
            isCleared = true;
        }
        public void Add(int count, int Stage)
        {
            var gamemode = SettingsScreen.Instance.GameMode;
            for (int i = 0; i < count; i++)
            {
                mobs.Add(new NormalMob("Zombie", rdn.Next(0, 800), rdn.Next(100, 500), 20 + Stage, gamemode, gamemode, rdn.Next(10, 21)));
            }
            if (Stage == 10)
            {
                mobs.Add(new EliteMob("WizardSkeleton", rdn.Next(0, 800), rdn.Next(100, 500), 500, gamemode, 1 + gamemode, rdn.Next(50, 100)));
            }
        }
    }
}
