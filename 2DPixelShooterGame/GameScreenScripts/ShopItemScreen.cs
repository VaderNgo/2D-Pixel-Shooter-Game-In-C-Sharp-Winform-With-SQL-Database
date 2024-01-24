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
    public class ShopItemScreen
    {
        private ShopItemScreen() { }
        private static ShopItemScreen instance;
        private static readonly object _lock = new object();

        public bool isSetUp = false;

        private Form ShopItemModal;
        private Form BackgroundModal;

        private Panel ShopItem_Panel;
        private PictureBox Exit;
        public static ShopItemScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new ShopItemScreen();
                            instance.ShopItemModal = new Form();
                            instance.ShopItem_Panel = new Panel();
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
                SetShopItemModal();
                SetBackgroundModal();
                SetShopItemUI();
                SetEvents();
                isSetUp = true;
            }
        }
        private void SetShopItemModal()
        {
            ShopItemModal.Text = "SHOPITEM";
            ShopItemModal.StartPosition = FormStartPosition.CenterScreen;
            ShopItemModal.MinimizeBox = false;
            ShopItemModal.MaximizeBox = false;
            ShopItemModal.ShowIcon = false;
            ShopItemModal.Size = new Size(721, 500);
            ShopItemModal.FormBorderStyle = FormBorderStyle.None;
            ShopItemModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            ShopItemModal.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetShopItemUI()
        {
            var fontTitle = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 13);
            var font1 = new Font(AssetsLoader.Instance.Fonts.Families[1], 12);

            ShopItem_Panel.Name = "ShopPanel";
            ShopItem_Panel.Size = new Size(673, 370);
            ShopItem_Panel.Location = new Point(12, 86);
            ShopItem_Panel.BackColor = Color.Transparent;
            ShopItem_Panel.AutoScroll = true;
            var tb = DatabaseController.Instance.DataSet.Tables["ITEMS"];

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                var padding = 100;
                var border = new Panel();
                var item_Pic = new PictureBox();
                var item_Name = CustomControls.Instance.CustomLabel(tb.Rows[i].ItemArray[1].ToString(), new Point(73, 3 + padding * i), Color.White, font, 1);
                var item_Info = CustomControls.Instance.CustomLabel(tb.Rows[i].ItemArray[2].ToString(), new Point(76, 25 + padding * i), Color.White, font1, 1);
                var Price_Label = CustomControls.Instance.CustomLabel("Price: ", new Point(410, 21 + padding * i), Color.Gold, font1, 1);
                var price = CustomControls.Instance.CustomLabel("0", new Point(480, 21 + padding * i), Color.Gold, font1, 1);

                var quantity = new NumericUpDown();
                quantity.BackColor = Color.SandyBrown;
                quantity.Name = i.ToString();
                quantity.Location = new Point(279, 17 + padding * i);
                quantity.ValueChanged += (object sender, EventArgs e) => { price.Text = ((int)tb.Rows[int.Parse(quantity.Name)].ItemArray[3] * quantity.Value).ToString(); };
                var buyBTN = new Button();
                buyBTN.Text = "BUY";
                buyBTN.Size = new Size(100, 30);
                buyBTN.Font = font1;
                buyBTN.Location = new Point(520, 17 + padding * i);
                buyBTN.Click += (object sender, EventArgs e) =>
                {
                    var PID = GameManager.Instance.P.GetPlayerID();
                    if (quantity.Value > 0)
                    {
                        if (GameManager.Instance.P.GetGold() - int.Parse(price.Text) >= 0)
                        {
                            DatabaseController.Instance.UpdatePlayerGold(PID, -int.Parse(price.Text));
                            GameManager.Instance.P.UpdatePlayerStats();
                            DatabaseController.Instance.UpdateInventory(PID, item_Pic.Name, (int)quantity.Value);
                            MessageBox.Show("You bought " + quantity.Value.ToString() + " " + item_Pic.Name);
                            InventoryScreen.Instance.UpdateUI();
                        }
                        else
                        {
                            MessageBox.Show("Not enough gold to buy");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Choose the quality to buy");
                    }
                };
                border.BackgroundImage = AssetsLoader.Instance.UIBorders["tile002"];
                border.BackgroundImageLayout = ImageLayout.Stretch;
                border.Size = new Size(64, 64);
                border.Location = new Point(3, +padding * i);
                item_Pic.Name = tb.Rows[i].ItemArray[0].ToString();
                item_Pic.Size = new Size(32, 32);
                item_Pic.Location = new Point(16, 16);
                item_Pic.BackgroundImage = AssetsLoader.Instance.GOItem[tb.Rows[i].ItemArray[1].ToString()];
                item_Pic.BackgroundImageLayout = ImageLayout.Stretch;
                border.Controls.Add(item_Pic);
                ShopItem_Panel.Controls.Add(border);
                ShopItem_Panel.Controls.Add(item_Name);
                ShopItem_Panel.Controls.Add(item_Info);
                ShopItem_Panel.Controls.Add(Price_Label);
                ShopItem_Panel.Controls.Add(price);
                ShopItem_Panel.Controls.Add(quantity);
                ShopItem_Panel.Controls.Add(buyBTN);
            }

            var Title = CustomControls.Instance.CustomLabel("SHOP", new Point(300, 25), Color.White, fontTitle, 1);

            Exit = new PictureBox();
            Exit.Size = new Size(30, 30);
            Exit.Location = new Point(ShopItemModal.Width - 60, 25);
            Exit.BackgroundImage = AssetsLoader.Instance.UIIcons["Exit Icon"];
            Exit.BackgroundImageLayout = ImageLayout.Stretch;

            ShopItemModal.Controls.Add(Title);
            ShopItemModal.Controls.Add(ShopItem_Panel);
            ShopItemModal.Controls.Add(Exit);
        }
        private void SetEvents()
        {
            Exit.Click += Exit_Click;
        }
        //Events
        private void Exit_Click(object sender, EventArgs e)
        {
            DoClose();
            GameUI.Instance.gameTimer.Start();
        }
        //Behaviours
        public void DoOpen()
        {
            BackgroundModal.Show();
            ShopItemModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            ShopItemModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && ShopItemModal.Visible)
                return true;
            return false;
        }
    }
}
