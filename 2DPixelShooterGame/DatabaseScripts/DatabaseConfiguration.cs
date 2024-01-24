using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.DatabaseScripts
{
    public class DatabaseConfiguration
    {
        private DatabaseConfiguration() { }
        private static DatabaseConfiguration instance;
        private static readonly object _lock = new object();

        Form DBConfigurationModal;
        Form BackgroundModal;

        private TextBox Server_TextBox;
        private TextBox Database_TextBox;
        private CheckBox Trusted_Connection_CheckBox;
        private TextBox UserID_TextBox;
        private TextBox Password_TextBox;
        private Button Connect_Button;

        private Label Server_Lable;
        private Label Trusted_Lable;
        private Label Database_Lable;
        private Label UserID_Lable;
        private Label Password_Lable;

        private PictureBox Exit;

        public bool isSetUp = false;
        public static DatabaseConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new DatabaseConfiguration();

                        }
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }

        public void SetUp()
        {
            isSetUp = true;
            SetBackgroundModal();
            SetDBConfigurationModal();
            SetDBConfiUI();
            SetEvents();
        }
        private void SetDBConfigurationModal()
        {
            DBConfigurationModal = new Form();
            DBConfigurationModal.Size = new Size(500, 420);
            DBConfigurationModal.MinimizeBox = false;
            DBConfigurationModal.MaximizeBox = false;
            DBConfigurationModal.ShowIcon = false;
            DBConfigurationModal.StartPosition = FormStartPosition.CenterScreen;
            DBConfigurationModal.FormBorderStyle = FormBorderStyle.None;
            DBConfigurationModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            DBConfigurationModal.AutoSize = true;
            DBConfigurationModal.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetDBConfiUI()
        {
            var fontTitle = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 15);
            var font1 = new Font(AssetsLoader.Instance.Fonts.Families[1], 10);

            var Title = CustomControls.Instance.CustomLabel("CONNECT TO DATABASE", new Point(50, 22), Color.White, fontTitle, 1);

            Server_Lable = CustomControls.Instance.CustomLabel("Server", new Point(100, 90), Color.White, font, 1);
            Server_TextBox = new TextBox();
            Server_TextBox.Location = new Point(250, 90);
            Server_TextBox.Width = 150;
            Server_TextBox.Font = font1;
            Server_TextBox.Text = ".\\SQLEXPRESS";

            Database_Lable = CustomControls.Instance.CustomLabel("Database", new Point(100, 140), Color.White, font, 1);
            Database_TextBox = new TextBox();
            Database_TextBox.Location = new Point(250, 140);
            Database_TextBox.Width = 150;
            Database_TextBox.Text = "GAME";
            Database_TextBox.Font = font1;

            Trusted_Lable = CustomControls.Instance.CustomLabel("Trusted Connection", new Point(100, 190), Color.White, font, 1);
            Trusted_Connection_CheckBox = new CheckBox();
            Trusted_Connection_CheckBox.Location = new Point(320, 190);
            Trusted_Connection_CheckBox.BackColor = Color.Transparent;

            UserID_Lable = CustomControls.Instance.CustomLabel("UserID", new Point(100, 240), Color.White, font, 1);
            UserID_TextBox = new TextBox();
            UserID_TextBox.Location = new Point(250, 240);
            UserID_TextBox.Width = 150;
            UserID_TextBox.Font = font1;

            Password_Lable = CustomControls.Instance.CustomLabel("Password", new Point(100, 290), Color.White, font, 1);
            Password_TextBox = new TextBox();
            Password_TextBox.Location = new Point(250, 290);
            Password_TextBox.Width = 150;
            Password_TextBox.Font = font1;

            Connect_Button = CustomControls.Instance.CustomBtn("Connect", new Point(195, 350), Color.Red, font);

            Exit = new PictureBox();
            Exit.Size = new Size(30, 30);
            Exit.Location = new Point(DBConfigurationModal.Width - 50, 20);
            Exit.BackgroundImage = AssetsLoader.Instance.UIIcons["Exit Icon"];
            Exit.BackgroundImageLayout = ImageLayout.Stretch;

            DBConfigurationModal.Controls.Add(Title);
            DBConfigurationModal.Controls.Add(Server_TextBox);
            DBConfigurationModal.Controls.Add(Server_Lable);
            DBConfigurationModal.Controls.Add(Database_TextBox);
            DBConfigurationModal.Controls.Add(Database_Lable);
            DBConfigurationModal.Controls.Add(Trusted_Connection_CheckBox);
            DBConfigurationModal.Controls.Add(Trusted_Lable);
            DBConfigurationModal.Controls.Add(UserID_Lable);
            DBConfigurationModal.Controls.Add(UserID_TextBox);
            DBConfigurationModal.Controls.Add(Password_Lable);
            DBConfigurationModal.Controls.Add(Password_TextBox);
            DBConfigurationModal.Controls.Add(Connect_Button);
            DBConfigurationModal.Controls.Add(Exit);
        }
        private void SetEvents()
        {
            Trusted_Connection_CheckBox.CheckedChanged += Trusted_Connection_CheckChanged;
            Connect_Button.Click += Connect_Button_Click;
            Exit.Click += Exit_Click;
        }
        //Events
        private void Trusted_Connection_CheckChanged(object sender, EventArgs e)
        {
            if (Trusted_Connection_CheckBox.Checked)
            {
                UserID_TextBox.Hide();
                UserID_Lable.Hide();
                Password_TextBox.Hide();
                Password_Lable.Hide();
            }
            else
            {
                UserID_TextBox.Show();
                UserID_Lable.Show();
                Password_TextBox.Show();
                Password_Lable.Show();
            }
        }
        private void Connect_Button_Click(object sender, EventArgs e)
        {
            DatabaseController.Instance.DoConnect(
                Server_TextBox.Text,
                Database_TextBox.Text,
                Trusted_Connection_CheckBox.Checked == true ? "true" : "false",
                UserID_TextBox.Text,
                Password_TextBox.Text
                );
            DoClose();
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            DoClose();
        }
        //Behaviours
        public void DoOpen()
        {
            BackgroundModal.Show();
            DBConfigurationModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            DBConfigurationModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && DBConfigurationModal.Visible)
                return true;
            return false;
        }
    }
}
