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
    public class PlayNewScreen
    {
        private PlayNewScreen() { }
        private static PlayNewScreen instance;
        private static readonly object _lock = new object();
        private Form PlayNewModal;
        private Form BackgroundModal;

        private Panel Player_Panel;
        private Label Player_Name_Label;
        private Label Input_Name_Label;
        public TextBox Player_Name_TextBox;
        private PictureBox Player_Canvas;

        private Label Map_Label;
        private ListView Map_ListView;
        private ImageList Map_ImageList;

        private Button Play_Button;
        private PictureBox Exit;

        public string Map_Selected_Name;
        public string Map_ID;
        public bool isSetUp = false;

        public static PlayNewScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new PlayNewScreen();

                        }
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }

        public void SetUp()
        {
            SetBackgroundModal();
            SetPlayNewModal();
            SetPlayNewUI();
            SetEvents();
            isSetUp = true;
        }
        private void SetPlayNewModal()
        {
            PlayNewModal = new Form();
            PlayNewModal.StartPosition = FormStartPosition.CenterScreen;
            PlayNewModal.MinimizeBox = false;
            PlayNewModal.MaximizeBox = false;
            PlayNewModal.ShowIcon = false;
            PlayNewModal.FormBorderStyle = FormBorderStyle.None;
            PlayNewModal.Size = new Size(800, 500);
            PlayNewModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            PlayNewModal.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetPlayerCreatePanel()
        {
            Player_Panel = new Panel();
            Player_Panel.Size = new Size(300, 280);
            Player_Panel.Location = new Point(50, 80);
            Player_Panel.BackColor = Color.Transparent;
            PlayNewModal.Controls.Add(Player_Panel);
        }
        private void SetPlayerCreateUI()
        {
            var fontT = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 13);

            Player_Name_TextBox = new TextBox();
            Player_Canvas = new PictureBox();


            Input_Name_Label = CustomControls.Instance.CustomLabel("Your character", new Point(100, 25), Color.White, fontT, 1);
            Player_Name_Label = CustomControls.Instance.CustomLabel("Your name", new Point(90, 90), Color.White, font, 1);
            Player_Name_TextBox.Location = new Point(60, 120);
            Player_Name_TextBox.Width = 200;
            Player_Name_TextBox.Font = font;
            Player_Canvas.Size = new Size(48, 48);
            Player_Canvas.Location = new Point(140, 40);

            PlayNewModal.Controls.Add(Input_Name_Label);

            Player_Panel.Controls.Add(Player_Canvas);
            Player_Panel.Controls.Add(Player_Name_Label);
            Player_Panel.Controls.Add(Player_Name_TextBox);
        }
        private void SetMapSelectUI()
        {
            var fontT = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 13);

            Map_ListView = new ListView();

            Map_Label = CustomControls.Instance.CustomLabel("Select Map", new Point(480, 25), Color.White, fontT, 1);
            Map_ListView.Size = new Size(350, 250);
            Map_ListView.Location = new Point(390, 100);

            Map_ImageList = new ImageList();
            var dt = DatabaseController.Instance.DataSet.Tables["MAPS"];
            Map_ImageList.ImageSize = new Size(300, 200);
            var mapImages = AssetsLoader.Instance.UIMaps;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!Map_ImageList.Images.ContainsKey(dt.Rows[i].ItemArray[0].ToString()))
                {
                    Map_ImageList.Images.Add(dt.Rows[i].ItemArray[0].ToString(), mapImages[dt.Rows[i].ItemArray[1].ToString()]);
                    var listItem = Map_ListView.Items.Add(dt.Rows[i].ItemArray[1].ToString());
                    listItem.ImageKey = dt.Rows[i].ItemArray[0].ToString();
                    listItem.Font = font;
                }
            }
            Map_ListView.LargeImageList = Map_ImageList;

            PlayNewModal.Controls.Add(Map_Label);
            PlayNewModal.Controls.Add(Map_ListView);
        }
        private void SetPlayNewUI()
        {
            SetPlayerCreatePanel();
            SetPlayerCreateUI();
            SetMapSelectUI();
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 17);
            Play_Button = CustomControls.Instance.CustomBtn("Play", new Point(333, 390), Color.Red, font);
            Play_Button.Width = 150;
            Exit = new PictureBox();
            Exit.Size = new Size(30, 30);
            Exit.Location = new Point(PlayNewModal.Width - 50, 20);
            Exit.BackgroundImage = AssetsLoader.Instance.UIIcons["Exit Icon"];
            Exit.BackgroundImageLayout = ImageLayout.Stretch;
            PlayNewModal.Controls.Add(Play_Button);
            PlayNewModal.Controls.Add(Exit);
        }
        private void SetEvents()
        {
            Exit.Click += Exit_Click;
            Map_ListView.SelectedIndexChanged += Map_ListView_SelectedIndexChanged;
            Player_Canvas.Paint += PlayerCanvas_Paint;
            Play_Button.Click += Play_Button_Click;
        }
        //Events
        private void Exit_Click(object sender, EventArgs e)
        {
            DoClose();
        }
        private void Map_ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isOpenning())
            {
                if (Map_ListView.SelectedItems.Count == 0)
                    return;
                Map_ID = Map_ListView.SelectedItems[0].ImageKey;
                Map_Selected_Name = Map_ListView.SelectedItems[0].Text;
            }

        }
        private void PlayerCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(AssetsLoader.Instance.GOPlayer["FrontIdle"], new Point(0, 0));
        }

        private void Play_Button_Click(object sender, EventArgs e)
        {
            if (isOpenning())
            {
                if (Player_Name_TextBox.Text != "" && Map_ListView.SelectedItems.Count != 0)
                {
                    DoClose();
                    WelcomeScreen.Instance.DoClose();
                    var PID = DatabaseController.Instance.InsertPlayerData(Player_Name_TextBox.Text);
                    GameManager.Instance.StartNewGame(PID);
                    Player_Name_TextBox.Clear();
                    Map_ListView.SelectedItems.Clear();
                }
            }
        }
        //Behaviours
        public void DoOpen()
        {
            BackgroundModal.Show();
            PlayNewModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            PlayNewModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && PlayNewModal.Visible)
                return true;
            return false;
        }
    }
}
