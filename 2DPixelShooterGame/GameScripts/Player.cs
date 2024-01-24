using _2DPixelShooterGame.DatabaseScripts;
using _2DPixelShooterGame.GameScreenScripts;
using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DPixelShooterGame.GameScripts
{
    public class Player
    {
        private string Player_ID;
        //Stats
        private int HP;
        private int Ammo;
        private int Gold;
        private int Speed = 2;
        //Coordination
        private Point playerXY = new Point(300, 300);
        //Frame Variables
        private int stateFrame = 0, totalFrame = 0;
        private float currentFrame = 0;
        private int countToDelay = 0;
        private int delayThreshold = 20;
        //Movement Properties
        private int directNum;
        private bool goUp, goDown, goLeft, goRight, goPressed;
        //Check
        private bool isAttacking = false;
        private bool isDead = false;
        private bool isHPZero = false;
        //Image
        private Image currentImage;
        private Dictionary<string, Image> playerImages = AssetsLoader.Instance.GOPlayer;
        private Dictionary<Keys, bool> checkKey = new Dictionary<Keys, bool>();
        //Weapon
        private string Weapon_ID;
        private string Weapon_Name;
        private Projectile projectile;

        private Player() { }
        private static Player instance;
        public static void InitInstance(string PID)
        {
            instance = new Player();
            GameManager.Instance.P = instance;
            instance.Player_ID = PID;
            instance.Weapon_ID = DatabaseController.Instance.GetCurrentWeaponID(instance.Player_ID);
            instance.Weapon_Name = DatabaseController.Instance.GetWeaponName(instance.Weapon_ID);
            instance.SetUpPlayer();
        }

        //Set Up Player
        private void SetUpPlayer()
        {
            InitPlayerData();
            SetPlayerIdleImage(0);
            SetEvents();
            InventoryScreen.Instance.ResetUI();
            UpgradeWeaponScreen.Instance.ResetUI();
        }
        private void InitPlayerData()
        {
            var PlayerInfo = DatabaseController.Instance.GetPlayerData(Player_ID);
            if (int.Parse(PlayerInfo[2]) <= 0)
            {
                DatabaseController.Instance.UpdatePlayerHP(Player_ID, 100);
                HP = 100;
            }
            else
            {
                HP = int.Parse(PlayerInfo[2]);
            }
            Ammo = int.Parse(PlayerInfo[3]);
            Gold = int.Parse(PlayerInfo[4]);
        }
        private void SetPlayerIdleImage(int direct)
        {
            this.currentImage = playerImages[ConvertIntToDirectString(direct) + "Idle"];
            this.directNum = direct;
            AnimatetCurrentImageFrame();
        }
        private void SetPlayerMoveImage(int direct)
        {
            this.currentImage = playerImages[ConvertIntToDirectString(direct) + "Walk"];
            this.directNum = direct;
            AnimatetCurrentImageFrame();
        }
        private void SetPlayerAttackImage(int direct, string Weapon_Name)
        {
            this.currentImage = playerImages[ConvertIntToDirectString(direct) + Weapon_Name];
            this.directNum = direct;
            AnimatetCurrentImageFrame();
            isAttacking = true;
        }
        private void SetPlayerDeadImage(int direct)
        {
            this.currentImage = playerImages[ConvertIntToDirectString(direct) + "Death"];
            this.directNum = direct;
            AnimatetCurrentImageFrame();
        }
        private void SetMovementProperties(int direct, bool pressed)
        {
            this.directNum = direct;
            switch (direct)
            {
                case 0: goDown = pressed; break;
                case 1: goUp = pressed; break;
                case 2: goRight = pressed; break;
                case 3: goLeft = pressed; break;

            }
            goPressed = pressed;
        }
        private void SetEvents()
        {
            GameInit.Instance.gW.KeyDown += Player_KeyDown;
            GameInit.Instance.gW.KeyUp += Player_KeyUp;
            GameUI.Instance.gameCanvas.Paint += Player_Paint;
            GameUI.Instance.gameTimer.Elapsed += Player_Update;
        }
        //Events
        private void Player_KeyDown(object sender, KeyEventArgs e)
        {
            bool pressed = true;
            var temp = e.KeyCode;
            if (isDead)
            {
                return;
            }
            if (temp == Keys.Left)
            {
                if (currentImage != playerImages["LeftWalk"])
                {
                    SetMovementProperties(3, pressed);
                    checkKey[temp] = true;
                    SetPlayerMoveImage(directNum);
                }
            }
            if (temp == Keys.Right)
            {
                if (currentImage != playerImages["RightWalk"])
                {
                    SetMovementProperties(2, pressed);
                    checkKey[temp] = true;
                    SetPlayerMoveImage(directNum);
                }
            }
            if (temp == Keys.Up)
            {
                if (currentImage != playerImages["BackWalk"])
                {
                    SetMovementProperties(1, pressed);
                    checkKey[temp] = true;
                    SetPlayerMoveImage(directNum);
                }
            }
            if (temp == Keys.Down)
            {
                if (currentImage != playerImages["FrontWalk"])
                {
                    SetMovementProperties(0, pressed);
                    checkKey[temp] = true;
                    SetPlayerMoveImage(directNum);
                }
            }
            if (temp == Keys.A)
            {
                if (currentImage != playerImages[ConvertIntToDirectString(directNum) + GetWeaponName()])
                {
                    SetPlayerAttackImage(directNum, GetWeaponName());
                    if (!GetWeaponName().Equals("Knife"))
                    {
                        if (Ammo > 0)
                        {
                            projectile = new Projectile(this);
                            DatabaseController.Instance.UpdatePlayerAmmo(Player_ID, -1);
                            UpdatePlayerStats();
                        }
                    }
                }
            }
            if (temp == Keys.U && !checkKey.ContainsValue(true))
            {
                if (!UpgradeWeaponScreen.Instance.isSetUp)
                    UpgradeWeaponScreen.Instance.SetUp();
                if (!UpgradeWeaponScreen.Instance.isOpenning())
                {
                    GameUI.Instance.gameTimer.Stop();
                    UpgradeWeaponScreen.Instance.DoOpen();
                }
            }
            if (temp == Keys.S && !checkKey.ContainsValue(true))
            {
                if (!ShopItemScreen.Instance.isSetUp)
                    ShopItemScreen.Instance.SetUp();
                if (!ShopItemScreen.Instance.isOpenning())
                {
                    GameUI.Instance.gameTimer.Stop();
                    ShopItemScreen.Instance.DoOpen();
                }
            }
            if (temp == Keys.I && !checkKey.ContainsValue(true))
            {
                if (!InventoryScreen.Instance.isSetUp)
                    InventoryScreen.Instance.SetUp();
                if (!InventoryScreen.Instance.isOpenning())
                {
                    GameUI.Instance.gameTimer.Stop();
                    InventoryScreen.Instance.DoOpen();
                }
            }
            if (temp == Keys.Escape && !checkKey.ContainsValue(true))
            {
                if (!PauseScreen.Instance.isOpenning())
                {
                    GameUI.Instance.gameTimer.Stop();
                    PauseScreen.Instance.DoOpen();
                }
            }
        }
        private void Player_KeyUp(object sender, KeyEventArgs e)
        {
            bool pressed = false;
            var temp = e.KeyCode;
            if (isDead)
            {
                return;
            }
            if (temp == Keys.Left)
            {
                SetMovementProperties(3, pressed);
                checkKey[temp] = false;
                if (!checkKey.ContainsValue(true))
                    SetPlayerIdleImage(directNum);
            }
            if (temp == Keys.Right)
            {
                SetMovementProperties(2, pressed);
                checkKey[temp] = false;
                if (!checkKey.ContainsValue(true))
                    SetPlayerIdleImage(directNum);
            }
            if (temp == Keys.Up)
            {
                SetMovementProperties(1, pressed);
                checkKey[temp] = false;
                if (!checkKey.ContainsValue(true))
                    SetPlayerIdleImage(directNum);
            }
            if (temp == Keys.Down)
            {
                SetMovementProperties(0, pressed);
                checkKey[temp] = false;
                if (!checkKey.ContainsValue(true))
                    SetPlayerIdleImage(directNum);
            }
            if (temp == Keys.A)
            {
                isAttacking = false;
                currentFrame = 0;
                if (currentImage != playerImages[ConvertIntToDirectString(directNum) + "Idle"])
                {
                    SetPlayerIdleImage(directNum);
                }
            }
        }
        private void Player_Paint(object sender, PaintEventArgs e)
        {
            if (!isDead)
            {
                e.Graphics.DrawImage(currentImage, playerXY.X, playerXY.Y, currentImage.Width, currentImage.Height);
                e.Graphics.DrawRectangle(new Pen(Color.SaddleBrown, 3), PlayerHPBar(0));
                e.Graphics.DrawRectangle(new Pen(Color.Green, 1), GetHitBox());
                e.Graphics.FillRectangle(new SolidBrush(Color.Green), PlayerHPBar(1));
            }
        }
        private void Player_Update(object sender, ElapsedEventArgs e)
        {
            UpdateImageAnimator();
            UpdatePlayerCoordination();
            CheckDead();
        }
        //UI
        private Rectangle PlayerHPBar(int type)
        {
            if (type == 1)
            {
                return new Rectangle(playerXY.X, playerXY.Y - currentImage.Height / 2, currentImage.Width - ((currentImage.Width - HP * currentImage.Width / 100)), 5);
            }
            else
            {
                return new Rectangle(playerXY.X, playerXY.Y - currentImage.Height / 2, currentImage.Width, 5);
            }
        }
        //Public Variables
        public int GetDirectNum() { return directNum; }
        public Rectangle GetHitBox()
        {
            if (!isDead)
            {
                return new Rectangle(playerXY.X, playerXY.Y, currentImage.Width, currentImage.Height);
            }
            return new Rectangle(0, 0, 0, 0);
        }
        public Point GetCoordination() { return playerXY; }
        public int GetHP() { return HP; }
        public int GetAmmo() { return Ammo; }
        public int GetGold() { return Gold; }
        public bool IsPlayerDead() { return isDead; }
        public string GetPlayerID() { return Player_ID; }
        public string GetPlayerName() { return DatabaseController.Instance.GetPlayerName(Player_ID); }
        public string GetPlayerCreatedAt() { return DatabaseController.Instance.GetPlayerCreateTime(Player_ID); }
        public string GetWeaponID() { return Weapon_ID; }
        public string GetWeaponName() { return Weapon_Name; }
        public int GetWeaponDamage() { return DatabaseController.Instance.GetWeaponDamage(Weapon_ID); }
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
        //Behaviours
        public void GainTemporarySpeed()
        {
            Speed++;
        }
        private void AnimatetCurrentImageFrame()
        {
            if (!isDead)
            {
                FrameDimension dimension = new FrameDimension(currentImage.FrameDimensionsList[0]);
                totalFrame = currentImage.GetFrameCount(dimension);
                ImageAnimator.Animate(currentImage, GameUI.Instance.FrameChangedUpdate);
            }
        }
        private void PlayerDead()
        {
            if (isHPZero)
            {
                if (isDead == false)
                {
                    FrameDimension dimensions = new FrameDimension(currentImage.FrameDimensionsList[0]);
                    totalFrame = currentImage.GetFrameCount(dimensions);
                    if (currentFrame < totalFrame)
                    {
                        currentImage.SelectActiveFrame(dimensions, (int)currentFrame);
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
                        if (countToDelay > delayThreshold)
                        {
                            isDead = true;
                            ClearData();
                            GameUI.Instance.gameTimer.Stop();
                            GameManager.Instance.mobWave.ClearMobs();
                            if (!FailScreen.Instance.isOpenning())
                            {
                                FailScreen.Instance.DoOpen();
                            }
                            countToDelay = 0;
                        }
                    }
                }
            }
        }
        public void ClearData()
        {
            GameInit.Instance.gW.KeyDown -= Player_KeyDown;
            GameInit.Instance.gW.KeyUp -= Player_KeyUp;
            GameUI.Instance.gameCanvas.Paint -= Player_Paint;
            GameUI.Instance.gameTimer.Elapsed -= Player_Update;
            //playerImages.Clear();
        }

        //Updating
        private void UpdateImageAnimator()
        {
            if (!isDead)
            {
                ImageAnimator.UpdateFrames(currentImage);
            }
        }
        private void UpdatePlayerCoordination()
        {
            if (!isHPZero)
            {
                if (goLeft)
                {
                    if (playerXY.X > 0)
                    {
                        playerXY.X -= Speed;
                    }
                }
                if (goRight)
                {
                    if (playerXY.X + currentImage.Width < 1024)
                    {
                        playerXY.X += Speed;
                    }
                }
                if (goUp)
                {
                    if (playerXY.Y > 0)
                    {
                        playerXY.Y -= Speed;
                    }
                }
                if (goDown)
                {
                    if (playerXY.Y + currentImage.Height < 600)
                    {
                        playerXY.Y += Speed;
                    }
                }
            }
        }
        public void UpdatePlayerStats()
        {
            var tb = DatabaseController.Instance.DataSet.Tables["PLAYERS"];
            tb.PrimaryKey = new DataColumn[] { tb.Columns["PLAYER_ID"] };
            DataRow row = tb.Rows.Find(Player_ID);
            if (row != null)
            {
                this.HP = (int)row["HP"];
                this.Gold = (int)row["Gold"];
                this.Ammo = (int)row["Ammo"];
            }
        }
        public void UpdatePlayerWeapon()
        {
            this.Weapon_ID = DatabaseController.Instance.GetCurrentWeaponID(Player_ID);
            this.Weapon_Name = DatabaseController.Instance.GetWeaponName(Weapon_ID);
        }
        public void CheckDead()
        {
            if (HP <= 0)
            {
                isHPZero = true;
                SetPlayerDeadImage(directNum);
                PlayerDead();
            }
        }
    }
}
