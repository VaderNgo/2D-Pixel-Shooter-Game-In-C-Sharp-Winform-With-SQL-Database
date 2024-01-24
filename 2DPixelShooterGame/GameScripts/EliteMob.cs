using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.GameScripts
{
    public class EliteMob : Mob
    {
        private Projectile projectile;
        public EliteMob(string name, int x, int y, int hp, int strength, int speed, int gold)
        {
            this.Name = name;
            this.goldBonus = gold;
            SetUpStats(x, y, hp, strength, speed);
            SetUp();
        }
        public Rectangle AttackRangeX()
        {
            if (!isHPZero)
            {
                return new Rectangle(0, mobXY.Y + 5, 1024, 50);
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }
        public Rectangle AttackRangeY()
        {
            if (!isHPZero)
            {
                return new Rectangle(mobXY.X + 5, 0, 50, 600);
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }
        public Rectangle DodgeBox()
        {
            if (!isHPZero)
            {
                return new Rectangle(mobXY.X - currentImage.Width * 2, mobXY.Y - currentImage.Height * 2, currentImage.Width * 5, currentImage.Height * 5);
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }
        public override void DetectPlayer()
        {
            if (AttackRangeY().IntersectsWith(GameManager.Instance.P.GetHitBox()))
            {
                if (directNum != 0 || directNum != 1)
                {
                    directNum = mobXY.Y > GameManager.Instance.P.GetCoordination().Y ? 1 : 0;
                }
                isAttacking = true;
            }
            else if (AttackRangeX().IntersectsWith(GameManager.Instance.P.GetHitBox()))
            {
                if (directNum != 2 || directNum != 3)
                {
                    directNum = mobXY.X > GameManager.Instance.P.GetCoordination().X ? 3 : 2;
                }
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }
        }
        public override void MoveToPlayer()
        {
            if (!isAttacking)
            {
                if (!DodgeBox().IntersectsWith(GameManager.Instance.P.GetHitBox()))
                {
                    if (!AttackRangeX().IntersectsWith(GameManager.Instance.P.GetHitBox()) && GameManager.Instance.P.GetCoordination().Y > mobXY.Y)
                    {
                        directNum = 0;
                        mobXY.Y += Speed;
                        goDown = true;
                        goUp = false;
                        goLeft = false;
                        goRight = false;
                    }
                    if (!AttackRangeY().IntersectsWith(GameManager.Instance.P.GetHitBox()) && GameManager.Instance.P.GetCoordination().X > mobXY.X)
                    {
                        directNum = 2;
                        mobXY.X += Speed;
                        goRight = true;
                        goLeft = false;
                        goDown = false;
                        goUp = false;
                    }
                    if (!AttackRangeX().IntersectsWith(GameManager.Instance.P.GetHitBox()) && GameManager.Instance.P.GetCoordination().Y < mobXY.Y)
                    {
                        directNum = 1;
                        mobXY.Y -= Speed;
                        goUp = true;
                        goDown = false;
                        goLeft = false;
                        goRight = false;
                    }
                    if (!AttackRangeY().IntersectsWith(GameManager.Instance.P.GetHitBox()) && GameManager.Instance.P.GetCoordination().X < mobXY.X)
                    {
                        directNum = 3;
                        mobXY.X -= Speed;
                        goLeft = true;
                        goRight = false;
                        goDown = false;
                        goUp = false;
                    }
                }
                if (DodgeBox().IntersectsWith(GameManager.Instance.P.GetHitBox()) && GameManager.Instance.P.GetCoordination().Y < mobXY.Y)
                {
                    mobXY.Y += Speed;
                    directNum = 0;
                    goDown = true;
                    goUp = false;
                    goLeft = false;
                    goRight = false;
                }
                if (DodgeBox().IntersectsWith(GameManager.Instance.P.GetHitBox()) && GameManager.Instance.P.GetCoordination().Y > mobXY.Y)
                {
                    mobXY.Y -= Speed;
                    directNum = 1;
                    goUp = true;
                    goDown = false;
                    goLeft = false;
                    goRight = false;
                }


                if (DodgeBox().IntersectsWith(GameManager.Instance.P.GetHitBox()) && GameManager.Instance.P.GetCoordination().X < mobXY.X)
                {
                    mobXY.X += Speed;
                    directNum = 2;
                    goRight = true;
                    goLeft = false;
                    goDown = false;
                    goUp = false;
                }
                MobMove();
            }
        }
        public override void MobAttack()
        {
            base.MobAttack();
            if (isAttacking)
            {
                projectile = new Projectile(this);
            }
        }

        public override void AttackPlayer()
        {
            if (!isHPZero && !isDead)
            {
                if (isAttacking)
                {
                    countToDelay++;
                    if (countToDelay >= delayThreshold)
                    {
                        MobAttack();
                        countToDelay = 0;
                    }
                }
            }
        }
        public string GetWeaponName()
        {
            return "Wand";
        }
    }
}
