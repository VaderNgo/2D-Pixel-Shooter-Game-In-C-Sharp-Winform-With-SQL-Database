using _2DPixelShooterGame.DatabaseScripts;
using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DPixelShooterGame.GameScripts
{
    public abstract class Mob
    {
        protected string Name;
        protected int HP;
        protected int HPMAX;
        protected int Speed;
        protected int Strength;
        protected int directNum;
        protected Point mobXY;

        protected int totalFrame = 0;
        protected int currentFrame = 0;
        protected int countToDelay = 0;
        protected int delayThreshold = 30;

        protected bool goUp, goDown, goLeft, goRight;
        protected bool isDead = false;
        protected bool isHPZero = false;
        protected bool isAttacking = false;

        protected int getDamage;
        protected int goldBonus;

        protected Image currentImage;
        protected Dictionary<string, Image> mobImages;
        //Set Up
        public void SetUp()
        {
            mobImages = AssetsLoader.Instance.LoadGOMob(Name);
            SetMobMoveImage(0);
            SetEvents();
        }
        public virtual void SetUpStats(int x, int y, int hp, int strength, int speed)
        {
            HP = HPMAX = hp;
            Strength = strength;
            Speed = speed;
            this.mobXY.X = x;
            this.mobXY.Y = y;
        }
        private void SetMobMoveImage(int direct)
        {
            this.currentImage = mobImages[ConvertIntToDirectString(direct) + "Walk"];
            this.directNum = direct;
            AnimateCurrentImageFrame();
        }
        private void SetMobAttackImage(int direct)
        {
            this.currentImage = mobImages[ConvertIntToDirectString(direct) + "Attack"];
            this.directNum = direct;
            AnimateCurrentImageFrame();
            isAttacking = true;
        }
        private void SetMobDeadImage(int direct)
        {
            this.currentImage = mobImages[ConvertIntToDirectString(direct) + "Death"];
            this.directNum = direct;
            AnimateCurrentImageFrame();
        }
        private void SetEvents()
        {
            GameUI.Instance.gameCanvas.Paint += Mob_Paint;
            GameUI.Instance.gameTimer.Elapsed += Mob_Update;
            GameInit.Instance.gW.KeyDown += Mob_KeyDown;
        }
        //Events
        private void Mob_Paint(object sender, PaintEventArgs e)
        {
            if (!isDead)
            {
                e.Graphics.DrawImage(currentImage, mobXY.X, mobXY.Y, currentImage.Width, currentImage.Height);
                e.Graphics.DrawRectangle(new Pen(Color.Red, 1), GetHitBox());
                e.Graphics.DrawRectangle(new Pen(Color.PaleVioletRed, 3), MobHPBar(0));
                e.Graphics.FillRectangle(new SolidBrush(Color.Red), MobHPBar(1));
            }
        }
        private void Mob_Update(object sender, ElapsedEventArgs e)
        {
            if (!isDead)
            {
                UpdateImageAnimator();
                UpdateMobCoordination();
                CheckDead();
            }
        }
        private void Mob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && GameManager.Instance.P.GetWeaponName().Equals("Knife"))
            {
                if (GetHitBox().IntersectsWith(GameManager.Instance.P.GetHitBox()))
                {
                    var dmg = -GameManager.Instance.P.GetWeaponDamage();
                    GetDamage(dmg);
                }
            }
        }
        //UI
        private Rectangle MobHPBar(int type)
        {
            if (type == 1)
            {
                return new Rectangle(mobXY.X, mobXY.Y - currentImage.Height / 2, currentImage.Width - ((currentImage.Width - HP * currentImage.Width / HPMAX)), 5);
            }
            else
            {
                return new Rectangle(mobXY.X, mobXY.Y - currentImage.Height / 2, currentImage.Width, 5);
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

        //Public Variables
        public Point GetCoordination() { return mobXY; }
        public int GetDirectNum() { return directNum; }
        public Rectangle GetHitBox()
        {
            if (!isHPZero)
            {
                return new Rectangle(mobXY.X, mobXY.Y, currentImage.Width, currentImage.Height);
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }
        public bool IsMobDead() { return isDead; }
        //Behaviours
        private void AnimateCurrentImageFrame()
        {
            if (!isDead)
            {
                FrameDimension dimension = new FrameDimension(currentImage.FrameDimensionsList[0]);
                totalFrame = currentImage.GetFrameCount(dimension);
                ImageAnimator.Animate(currentImage, GameUI.Instance.FrameChangedUpdate);
            }
            else
            {
                ImageAnimator.StopAnimate(currentImage, GameUI.Instance.FrameChangedUpdate);
            }
        }
        public virtual void MobMove()
        {
            if (goDown && currentImage != mobImages["FrontWalk"])
            {
                SetMobMoveImage(0);
            }
            if (goUp && currentImage != mobImages["BackWalk"])
            {
                SetMobMoveImage(1);
            }
            if (goLeft && currentImage != mobImages["LeftWalk"])
            {
                SetMobMoveImage(3);
            }
            if (goRight && currentImage != mobImages["RightWalk"])
            {
                SetMobMoveImage(2);
            }
        }
        public virtual void MobAttack()
        {
            if (isAttacking && currentImage != mobImages[ConvertIntToDirectString(directNum) + "Attack"])
                SetMobAttackImage(directNum);
        }
        public void MobDead()
        {
            if (isHPZero)
            {
                if (isDead == false)
                {
                    FrameDimension dimensions = new FrameDimension(currentImage.FrameDimensionsList[0]);
                    totalFrame = currentImage.GetFrameCount(dimensions);
                    if (currentFrame < totalFrame)
                    {
                        currentImage.SelectActiveFrame(dimensions, currentFrame);
                        countToDelay++;
                        if (countToDelay > 5)
                        {
                            currentFrame += 1;
                            countToDelay = 0;
                        }
                    }
                    if (currentFrame == totalFrame)
                    {
                        countToDelay++;
                        if (countToDelay > 2)
                        {
                            isDead = true;
                            DatabaseController.Instance.UpdatePlayerGold(GameManager.Instance.P.GetPlayerID(), goldBonus);
                            GameManager.Instance.P.UpdatePlayerStats();
                            countToDelay = 0;
                        }
                    }
                }
            }
        }
        public virtual void DetectPlayer()
        {
            if (!isDead && !isHPZero)
            {
                if (GetHitBox().IntersectsWith(GameManager.Instance.P.GetHitBox()))
                {
                    isAttacking = true;
                }
                else
                {
                    isAttacking = false;
                }
            }
        }
        public virtual void MoveToPlayer()
        {
            if (!isDead && !isHPZero)
            {
                if (!isAttacking)
                {
                    if (mobXY.X - currentImage.Width / 3 > GameManager.Instance.P.GetCoordination().X)
                    {
                        mobXY.X -= Speed;
                        goLeft = true;
                        goRight = false;
                        goDown = false;
                        goUp = false;
                    }
                    if (mobXY.X + currentImage.Width / 3 < GameManager.Instance.P.GetCoordination().X)
                    {
                        mobXY.X += Speed;
                        goRight = true;
                        goLeft = false;
                        goDown = false;
                        goUp = false;
                    }
                    if (mobXY.Y + currentImage.Height / 3 < GameManager.Instance.P.GetCoordination().Y)
                    {
                        mobXY.Y += Speed;
                        goDown = true;
                        goUp = false;
                        goLeft = false;
                        goRight = false;
                    }
                    if (mobXY.Y - currentImage.Height / 3 > GameManager.Instance.P.GetCoordination().Y)
                    {
                        mobXY.Y -= Speed;
                        goUp = true;
                        goDown = false;
                        goLeft = false;
                        goRight = false;
                    }
                    MobMove();
                }
            }
        }
        public virtual void AttackPlayer()
        {
            if (!isHPZero && !isDead)
            {
                if (isAttacking)
                {
                    countToDelay++;
                    if (countToDelay >= delayThreshold)
                    {
                        MobAttack();
                        DatabaseController.Instance.UpdatePlayerHP(GameManager.Instance.P.GetPlayerID(), -Strength);
                        GameManager.Instance.P.UpdatePlayerStats();
                        countToDelay = 0;
                    }
                }
            }
        }
        public void GetDamage(int damage)
        {
            if (!isDead && !isHPZero)
            {
                if (HP + damage > 0)
                    HP += damage;
                else
                {
                    HP = 0;
                    isHPZero = true;
                }
            }
        }
        public virtual void ClearData()
        {
            GameUI.Instance.gameCanvas.Paint -= Mob_Paint;
            GameUI.Instance.gameTimer.Elapsed -= Mob_Update;
            GameInit.Instance.gW.KeyDown -= Mob_KeyDown;
            mobImages.Clear();
            currentImage.Dispose();
        }
        //Updating
        public virtual void UpdateImageAnimator()
        {
            if (!isDead)
            {
                if (!GameManager.Instance.P.IsPlayerDead())
                {
                    ImageAnimator.UpdateFrames(currentImage);
                }
            }
        }
        public virtual void UpdateMobCoordination()
        {
            if (GameManager.Instance.P.IsPlayerDead())
            {
                return;
            }
            if (!isHPZero && !isDead)
            {
                MoveToPlayer();
                DetectPlayer();
                AttackPlayer();
            }
        }
        public void CheckDead()
        {
            if (isHPZero && !isDead)
            {
                SetMobDeadImage(directNum);
                MobDead();
            }
        }
    }
}
