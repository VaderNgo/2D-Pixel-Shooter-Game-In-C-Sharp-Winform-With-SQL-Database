using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.GameScripts
{
    public class NormalMob : Mob
    {
        public NormalMob(string name, int x, int y, int hp, int strength, int speed, int gold)
        {
            this.Name = name;
            this.goldBonus = gold;
            SetUpStats(x, y, hp, strength, speed);
            SetUp();
        }
    }
}
