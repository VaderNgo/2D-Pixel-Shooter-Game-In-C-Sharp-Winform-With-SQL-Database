using _2DPixelShooterGame.DatabaseScripts;
using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DPixelShooterGame.GameScripts
{
    public class Projectile
    {
        private int Speed = 3;
        private dynamic shooterObject;
        private int directNum;
        private Point projectileXY = new Point(0, 0);


        private bool isDispose = false;
        private bool isHitMob = false;
        private bool isHitPlayer = false;

        private Image currentImage;
        private Dictionary<string, Image> projectileImages = AssetsLoader.Instance.GOProjectile;

        public Projectile(dynamic sO)
        {
            shooterObject = sO;
            if (!isDispose)
            {
                SetUp();
            }
            SetEvents();
        }
        private void SetUp()
        {

            SetProjectileImage();
            SetProjectileCoordination();
        }
        private void SetProjectileImage()
        {
            this.directNum = shooterObject.GetDirectNum();
            currentImage = projectileImages[ConvertIntToDirectString(directNum) + shooterObject.GetWeaponName()];

        }
        private void SetProjectileCoordination()
        {
            switch (shooterObject.GetDirectNum())
            {
                case 0:
                    {
                        projectileXY.X = shooterObject.GetCoordination().X + currentImage.Width / 2;
                        projectileXY.Y = shooterObject.GetCoordination().Y + currentImage.Height * 3 / 2;
                        break;
                    }
                case 1:
                    {
                        projectileXY.X = shooterObject.GetCoordination().X + currentImage.Width / 2;
                        projectileXY.Y = shooterObject.GetCoordination().Y - currentImage.Height * 3 / 2;
                        break;
                    }
                case 2:
                    {
                        projectileXY.X = shooterObject.GetCoordination().X + currentImage.Width * 2;
                        projectileXY.Y = shooterObject.GetCoordination().Y + currentImage.Height / 2;
                        break;
                    }
                case 3:
                    {
                        projectileXY.X = shooterObject.GetCoordination().X - currentImage.Width * 3 / 2;
                        projectileXY.Y = shooterObject.GetCoordination().Y + currentImage.Height / 2;
                        break;
                    }
            }
        }
        private void SetEvents()
        {
            GameUI.Instance.gameTimer.Elapsed += Projectile_Update;
            GameUI.Instance.gameCanvas.Paint += Projectile_Paint;
        }
        //Events
        private void Projectile_Paint(object sender, PaintEventArgs e)
        {
            if (!isDispose)
            {
                e.Graphics.DrawImage(currentImage, projectileXY.X, projectileXY.Y);
            }
        }
        private void Projectile_Update(object sender, ElapsedEventArgs e)
        {
            if (!isDispose)
            {
                DetectCollision();
            }
        }
        //Convert
        private string ConvertIntToDirectString(int direct)
        {
            switch (direct)
            {
                case 0:
                    return "Front";
                case 1:
                    return "Back";
                case 2:
                    return "Right";
                case 3:
                    return "Left";
                default:
                    return "Unknow";
            }
        }
        //Public Variable
        public Rectangle GetHitBox()
        {
            if (!isDispose)
            {
                return new Rectangle(projectileXY.X, projectileXY.Y, currentImage.Width, currentImage.Height);
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }

        //Behaviours
        private void DetectCollision()
        {
            foreach (var m in GameManager.Instance.mobWave.mobs)
            {
                if (GetHitBox().IntersectsWith(m.GetHitBox()) && !isHitMob && !m.IsMobDead())
                {
                    if (!isDispose)
                    {
                        isDispose = true;
                        ClearData();
                    }
                    isHitMob = true;
                    var dmg = -GameManager.Instance.P.GetWeaponDamage();
                    m.GetDamage(dmg);
                }
            }
            if (shooterObject?.GetType().ToString() != "Player")
            {
                if (GetHitBox().IntersectsWith(GameManager.Instance.P.GetHitBox()) && !GameManager.Instance.P.IsPlayerDead() && !isHitPlayer)
                {
                    if (!isDispose)
                    {
                        isDispose = true;
                        ClearData();
                    }
                    isHitPlayer = true;
                    var PID = GameManager.Instance.P.GetPlayerID();
                    if (GameManager.Instance.P.GetHP() >= 10)
                    {
                        DatabaseController.Instance.UpdatePlayerHP(PID, -10);
                    }
                    else
                    {
                        DatabaseController.Instance.UpdatePlayerHP(PID, -GameManager.Instance.P.GetHP());
                    }
                    GameManager.Instance.P.UpdatePlayerStats();
                }
            }
            if (!isHitPlayer && !isHitMob)
            {
                ProjectileMove();
            }
            //if (VictoryScreen.Instance.isOpenning())
            //{
            //    isDispose = true;
            //    ClearData();
            //}
        }
        private void ProjectileMove()
        {
            if (projectileXY.Y < 0 || projectileXY.Y > 800 ||
                    projectileXY.X < 0 || projectileXY.X > 1200
                   )
            {
                if (!isDispose)
                {
                    isDispose = true;
                    ClearData();
                }
            }
            switch (this.directNum)
            {
                case 0: projectileXY.Y += Speed; break;
                case 1: projectileXY.Y -= Speed; break;
                case 2: projectileXY.X += Speed; break;
                case 3: projectileXY.X -= Speed; break;
            }
        }
        private void ClearData()
        {
            GameUI.Instance.gameTimer.Elapsed -= Projectile_Update;
            GameUI.Instance.gameCanvas.Paint -= Projectile_Paint;
            //currentImage.Dispose();
            //projectileImages.Clear();

        }
    }
}
