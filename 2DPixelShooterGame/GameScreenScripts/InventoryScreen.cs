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
    public class InventoryScreen
    {
        private InventoryScreen() { }
        private static InventoryScreen instance;
        private static readonly object _lock = new object();

        private Form InventoryModal;
        private Form BackgroundModal;

        private Panel Inventory_Panel;
        private PictureBox Exit;

        public bool isSetUp = false;
        public static InventoryScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new InventoryScreen();
                            instance.InventoryModal = new Form();
                            instance.Inventory_Panel = new Panel();
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
                SetInventoryModal();
                SetBackgroundModal();
                SetInventoryUI();
                SetEvents();
                isSetUp = true;
            }
        }
        private void SetInventoryModal()
        {
            InventoryModal.Text = "Inventory";
            InventoryModal.StartPosition = FormStartPosition.CenterScreen;
            InventoryModal.MinimizeBox = false;
            InventoryModal.MaximizeBox = false;
            InventoryModal.ShowIcon = false;
            InventoryModal.Size = new Size(491, 500);
            InventoryModal.FormBorderStyle = FormBorderStyle.None;
            InventoryModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            InventoryModal.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetInventoryUI()
        {
            Inventory_Panel.Name = "InventoryPanel";
            Inventory_Panel.Size = new Size(449, 370);
            Inventory_Panel.Location = new Point(20, 86);
            Inventory_Panel.BackColor = Color.Transparent;
            Inventory_Panel.AutoScroll = true;

            Exit = new PictureBox();
            Exit.Size = new Size(30, 30);
            Exit.Location = new Point(InventoryModal.Width - 60, 25);
            Exit.BackgroundImage = AssetsLoader.Instance.UIIcons["Exit Icon"];
            Exit.BackgroundImageLayout = ImageLayout.Stretch;

            var fontTitle = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var Title_Label = CustomControls.Instance.CustomLabel("INVENTORY", new Point(140, 25), Color.White, fontTitle, 1);
            InventoryModal.Controls.Add(Title_Label);
            InventoryModal.Controls.Add(Inventory_Panel);
            InventoryModal.Controls.Add(Exit);
        }
        private void SetEvents()
        {
            Exit.Click += Exit_Click;
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            DoClose();
            GameUI.Instance.gameTimer.Start();
        }

        public void UpdateUI()
        {
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 13);
            var font1 = new Font(AssetsLoader.Instance.Fonts.Families[1], 12);
            var tb = DatabaseController.Instance.DataSet.Tables["INVENTORY"];
            int i = 0;
            for (int j = 0; j < tb.Rows.Count; j++)
            {
                var PID = tb.Rows[j].ItemArray[0].ToString();
                if (PID.Equals(GameManager.Instance.P.GetPlayerID()))
                {
                    var IID = tb.Rows[j].ItemArray[1].ToString();
                    var QUANTITY = tb.Rows[j].ItemArray[2].ToString();
                    var Item_Name = DatabaseController.Instance.GetItemName(IID);
                    var Item_Info = DatabaseController.Instance.GetItemInfo(IID);
                    var padding = 100;
                    var border = new Panel();
                    var item_Pic = new PictureBox();
                    var item_Name_Label = CustomControls.Instance.CustomLabel(Item_Name, new Point(73, 3 + padding * i), Color.White, font, 1);
                    var item_Info_Label = CustomControls.Instance.CustomLabel(Item_Info, new Point(76, 25 + padding * i), Color.White, font1, 1);
                    var item_Quantity_Label = CustomControls.Instance.CustomLabel(QUANTITY, new Point(229, 25 + padding * i), Color.White, font1, 1);
                    item_Quantity_Label.Name = IID + "Label";
                    var UseBtn = new Button();
                    UseBtn.Text = "USE";
                    UseBtn.Name = IID;
                    UseBtn.Font = font;
                    UseBtn.Size = new Size(70, 25);
                    UseBtn.Location = new Point(350, 19 + padding * i);
                    UseBtn.Click += (object sender, EventArgs e) =>
                    {
                        if (int.Parse(item_Quantity_Label.Text) > 0)
                        {
                            Button temp = (Button)sender;
                            string Item_ID_To_Use = temp.Name;
                            switch (Item_ID_To_Use)
                            {
                                case "I01":
                                    DatabaseController.Instance.UpdatePlayerAmmo(PID, 5);
                                    item_Quantity_Label.Text = (int.Parse(item_Quantity_Label.Text) - 1).ToString();
                                    break;
                                case "I02":
                                    DatabaseController.Instance.UpdatePlayerAmmo(PID, 20);
                                    item_Quantity_Label.Text = (int.Parse(item_Quantity_Label.Text) - 1).ToString();
                                    break;
                                case "I03":
                                    GameManager.Instance.P.GainTemporarySpeed();
                                    item_Quantity_Label.Text = (int.Parse(item_Quantity_Label.Text) - 1).ToString();
                                    break;
                                case "I04":
                                    if (GameManager.Instance.P.GetHP() <= 90)
                                    {
                                        DatabaseController.Instance.UpdatePlayerHP(PID, 10);
                                        item_Quantity_Label.Text = (int.Parse(item_Quantity_Label.Text) - 1).ToString();
                                    }
                                    else
                                    {
                                        DatabaseController.Instance.UpdatePlayerHP(PID, 100 - GameManager.Instance.P.GetHP());
                                        item_Quantity_Label.Text = (int.Parse(item_Quantity_Label.Text) - 1).ToString();
                                    }
                                    break;
                            }
                            GameManager.Instance.P.UpdatePlayerStats();
                            DatabaseController.Instance.UpdateInventory(PID, IID, -1);
                        }
                        else
                        {
                            MessageBox.Show("Item has been used up");
                        }
                    };
                    border.Name = IID;
                    border.BackgroundImage = AssetsLoader.Instance.UIBorders["tile002"];
                    border.BackgroundImageLayout = ImageLayout.Stretch;
                    border.Size = new Size(64, 64);
                    border.Location = new Point(3, +padding * i);
                    item_Pic.Name = tb.Rows[i].ItemArray[0].ToString();
                    item_Pic.Size = new Size(32, 32);
                    item_Pic.Location = new Point(16, 16);
                    item_Pic.BackgroundImage = AssetsLoader.Instance.GOItem[Item_Name];
                    item_Pic.BackgroundImageLayout = ImageLayout.Stretch;
                    border.Controls.Add(item_Pic);
                    if (!this.Inventory_Panel.Controls.ContainsKey(border.Name))
                    {
                        this.Inventory_Panel.Controls.Add(border);
                        this.Inventory_Panel.Controls.Add(item_Name_Label);
                        this.Inventory_Panel.Controls.Add(item_Info_Label);
                        this.Inventory_Panel.Controls.Add(item_Quantity_Label);
                        this.Inventory_Panel.Controls.Add(UseBtn);
                    }
                    else
                    {
                        foreach (Control lb in Inventory_Panel.Controls)
                        {
                            if (lb.Name == IID + "Label")
                            {
                                lb.Text = QUANTITY;
                            }
                        }
                    }
                    i++;
                }
            }
            InventoryModal.Invalidate();
        }
        public void ResetUI()
        {
            while (Inventory_Panel.Controls.Count != 0)
            {
                foreach (Control c in Inventory_Panel.Controls)
                {
                    Inventory_Panel.Controls.Remove(c);
                    c.Dispose();
                    Inventory_Panel.Invalidate();
                }
            }
            UpdateUI();
        }

        public void DoOpen()
        {
            BackgroundModal.Show();
            InventoryModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            InventoryModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && InventoryModal.Visible)
                return true;
            return false;
        }
    }
}
