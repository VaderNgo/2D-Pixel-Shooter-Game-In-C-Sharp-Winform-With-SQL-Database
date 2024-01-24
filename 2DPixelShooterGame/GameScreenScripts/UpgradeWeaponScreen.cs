using _2DPixelShooterGame.DatabaseScripts;
using _2DPixelShooterGame.GameScripts;
using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.GameScreenScripts
{
    public class UpgradeWeaponScreen
    {
        private UpgradeWeaponScreen() { }
        private static UpgradeWeaponScreen instance;
        private static readonly object _lock = new object();
        public bool isSetUp = false;


        private Form UpgradeWeaponModal;
        private Form BackgroundModal;

        //Controls
        private Label CurrentWeapon_Label;
        private PictureBox CurrentWeapon_Pic;
        private Panel CurrentWeapon_Panel;
        public Label CurrentWeapon_Name_Label;

        private Label NextWeapon_Label;
        private PictureBox NextWeapon_Pic;
        private Panel NextWeapon_Panel;
        public Label NextWeapon_Name_Label;

        private Label PriceToUpgrade_Label;
        private Label Price_Label;
        private PictureBox GoldIcon;

        private PictureBox Exit;
        private Button Upgrade_Button;
        //    
        private string currWeaponName;
        private string nextWeaponName;
        private string priceToUpgrade;
        public static UpgradeWeaponScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new UpgradeWeaponScreen();
                            instance.UpgradeWeaponModal = new Form();

                            instance.CurrentWeapon_Panel = new Panel();
                            instance.CurrentWeapon_Pic = new PictureBox();
                            instance.CurrentWeapon_Label = new Label();
                            instance.CurrentWeapon_Name_Label = new Label();

                            instance.NextWeapon_Panel = new Panel();
                            instance.NextWeapon_Pic = new PictureBox();
                            instance.NextWeapon_Label = new Label();
                            instance.NextWeapon_Name_Label = new Label();

                            instance.PriceToUpgrade_Label = new Label();
                            instance.GoldIcon = new PictureBox();
                            instance.Price_Label = new Label();
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
            if (!isSetUp)
            {
                SetUpgradeWeaponModal();
                SetBackgroundModal();
                SetUpgradeWeaponUI();
                SetEvents();
                isSetUp = true;
            }
        }
        private void SetUpgradeWeaponModal()
        {
            UpgradeWeaponModal.Text = "WEAPON UPGRADE";
            UpgradeWeaponModal.StartPosition = FormStartPosition.CenterScreen;
            UpgradeWeaponModal.MinimizeBox = false;
            UpgradeWeaponModal.MaximizeBox = false;
            UpgradeWeaponModal.ShowIcon = false;
            UpgradeWeaponModal.Size = new Size(400, 450);
            UpgradeWeaponModal.FormBorderStyle = FormBorderStyle.None;
            UpgradeWeaponModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            UpgradeWeaponModal.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetUpgradeWeaponUI()
        {
            var wName = GameManager.Instance.P.GetWeaponName();
            var wID = GameManager.Instance.P.GetWeaponID();

            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 13);
            var fontTitle = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font1 = new Font(AssetsLoader.Instance.Fonts.Families[1], 12, FontStyle.Italic | FontStyle.Bold);

            var title = CustomControls.Instance.CustomLabel("UPGRADE", new Point(120, 25), Color.White, fontTitle, 1);

            currWeaponName = wName;
            CurrentWeapon_Label = CustomControls.Instance.CustomLabel("Current Weapon:", new Point(20, 118), Color.White, font, 1);
            CurrentWeapon_Name_Label = CustomControls.Instance.CustomLabel(currWeaponName, new Point(193, 118), Color.OrangeRed, font1, 1);
            CurrentWeapon_Panel.Size = new Size(64, 64);
            CurrentWeapon_Panel.Location = new Point(285, 91);
            CurrentWeapon_Panel.BackgroundImage = AssetsLoader.Instance.UIBorders["tile038"];
            CurrentWeapon_Pic.Size = new Size(32, 32);
            CurrentWeapon_Pic.Location = new Point(16, 16);
            CurrentWeapon_Pic.BackColor = Color.Transparent;
            CurrentWeapon_Pic.BackgroundImage = AssetsLoader.Instance.GOWeapon[wName];
            CurrentWeapon_Pic.BackgroundImageLayout = ImageLayout.Stretch;

            nextWeaponName = DatabaseController.Instance.GetWeaponName(DatabaseController.Instance.GetNextWeaponID(wID));
            NextWeapon_Label = CustomControls.Instance.CustomLabel("Next Weapon:", new Point(20, 222), Color.White, font, 1);
            NextWeapon_Name_Label = CustomControls.Instance.CustomLabel(nextWeaponName, new Point(193, 222), Color.BlueViolet, font, 1);
            NextWeapon_Panel.Size = new Size(64, 64);
            NextWeapon_Panel.Location = new Point(285, 194);
            NextWeapon_Panel.BackgroundImage = AssetsLoader.Instance.UIBorders["tile038"];
            NextWeapon_Pic.Size = new Size(32, 32);
            NextWeapon_Pic.BackColor = Color.Transparent;
            NextWeapon_Pic.Location = new Point(16, 16);
            NextWeapon_Pic.BackgroundImage = AssetsLoader.Instance.GOWeapon[nextWeaponName];
            NextWeapon_Pic.BackgroundImageLayout = ImageLayout.Stretch;

            priceToUpgrade = DatabaseController.Instance.GetWeaponPrice(wID);
            PriceToUpgrade_Label = CustomControls.Instance.CustomLabel("Price to upgrade: ", new Point(20, 307), Color.White, font, 1);
            Price_Label = CustomControls.Instance.CustomLabel(priceToUpgrade, new Point(193, 307), Color.Gold, font, 1); ;

            GoldIcon.Size = new Size(32, 32);
            GoldIcon.Location = new Point(300, 293);
            GoldIcon.BackgroundImage = AssetsLoader.Instance.UIIcons["Money Icon"];
            GoldIcon.BackgroundImageLayout = ImageLayout.Stretch;
            GoldIcon.BackColor = Color.Transparent;

            Upgrade_Button = CustomControls.Instance.CustomBtn("Upgrade", new Point(141, 394), Color.Red, font);

            Exit = new PictureBox();
            Exit.Size = new Size(30, 30);
            Exit.Location = new Point(UpgradeWeaponModal.Width - 60, 25);
            Exit.BackgroundImage = AssetsLoader.Instance.UIIcons["Exit Icon"];
            Exit.BackgroundImageLayout = ImageLayout.Stretch;

            UpgradeWeaponModal.Controls.Add(title);
            UpgradeWeaponModal.Controls.Add(CurrentWeapon_Panel);
            UpgradeWeaponModal.Controls.Add(NextWeapon_Panel);
            UpgradeWeaponModal.Controls.Add(CurrentWeapon_Label);
            UpgradeWeaponModal.Controls.Add(CurrentWeapon_Name_Label);
            UpgradeWeaponModal.Controls.Add(NextWeapon_Label);
            UpgradeWeaponModal.Controls.Add(NextWeapon_Name_Label);
            UpgradeWeaponModal.Controls.Add(PriceToUpgrade_Label);
            UpgradeWeaponModal.Controls.Add(Price_Label);
            UpgradeWeaponModal.Controls.Add(GoldIcon);
            UpgradeWeaponModal.Controls.Add(Upgrade_Button);
            UpgradeWeaponModal.Controls.Add(Exit);

            CurrentWeapon_Panel.Controls.Add(CurrentWeapon_Pic);
            NextWeapon_Panel.Controls.Add(NextWeapon_Pic);
        }
        private void SetEvents()
        {
            Exit.Click += Exit_Click;
            Upgrade_Button.Click += Upgrade_Button_Click;
        }
        //Events
        private void Exit_Click(object sender, EventArgs e)
        {
            DoClose();
            GameUI.Instance.gameTimer.Start();
        }
        private void Upgrade_Button_Click(object sender, EventArgs e)
        {
            if (!nextWeaponName.Equals("-"))
            {
                if (GameManager.Instance.P.GetGold() >= int.Parse(priceToUpgrade))
                {
                    var PID = GameManager.Instance.P.GetPlayerID();
                    var currentWID = DatabaseController.Instance.GetCurrentWeaponID(PID);
                    DatabaseController.Instance.UpdatePlayerGold(PID, -int.Parse(priceToUpgrade));
                    DatabaseController.Instance.UpdateOwnData(PID, DatabaseController.Instance.GetNextWeaponID(currentWID));
                    GameManager.Instance.P.UpdatePlayerWeapon();
                    GameManager.Instance.P.UpdatePlayerStats();
                    UpdateUI();
                }
                else
                {
                    MessageBox.Show("Not enough gold to upgrade!");
                }
            }
            else
            {
                MessageBox.Show("Your weapon has reached maximum level!");
            }
        }
        //Updating
        public void UpdateUI()
        {
            var cWID = GameManager.Instance.P.GetWeaponID();
            var cWName = GameManager.Instance.P.GetWeaponName();
            var nWID = DatabaseController.Instance.GetNextWeaponID(cWID);
            var nWname = DatabaseController.Instance.GetWeaponName(nWID);
            currWeaponName = cWName;
            nextWeaponName = nWname;
            priceToUpgrade = DatabaseController.Instance.GetWeaponPrice(cWID);
            CurrentWeapon_Name_Label.Text = currWeaponName;
            NextWeapon_Name_Label.Text = nextWeaponName;
            Price_Label.Text = priceToUpgrade;
            CurrentWeapon_Pic.BackgroundImage = AssetsLoader.Instance.GOWeapon[currWeaponName];
            NextWeapon_Pic.BackgroundImage = AssetsLoader.Instance.GOWeapon[nextWeaponName];
        }

        public void ResetUI()
        {
            UpdateUI();
        }

        //Behaviours
        public void DoOpen()
        {
            BackgroundModal.Show();
            UpgradeWeaponModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            UpgradeWeaponModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && UpgradeWeaponModal.Visible)
                return true;
            return false;
        }
    }
}
