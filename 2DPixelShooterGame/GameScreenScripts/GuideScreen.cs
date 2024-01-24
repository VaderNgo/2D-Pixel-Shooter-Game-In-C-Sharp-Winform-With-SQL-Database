using _2DPixelShooterGame.OtherScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.GameScreenScripts
{
    public class GuideScreen
    {
        private GuideScreen() { }
        private static GuideScreen instance;
        private static readonly object _lock = new object();
        private Form GuideModal;
        private Form BackgroundModal;

        private Panel Guide_Panel;
        private Label PlayMode_Label;
        private Label Button_Label;
        private Label Control_Label;


        private PictureBox Exit;

        public bool isSetUp = false;
        public static GuideScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new GuideScreen();
                        }
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }
        //Set Up
        public void SetUp()
        {
            isSetUp = true;
            SetGuideModal();
            SetBackgroundModal();
            SetGuidePanel();
            SetGuideUI();
            SetPlayModeContent();
            SetEvents();

        }
        private void SetGuideModal()
        {
            GuideModal = new Form();
            GuideModal.Size = new Size(700, 500);
            GuideModal.MinimizeBox = false;
            GuideModal.MaximizeBox = false;
            GuideModal.ShowIcon = false;
            GuideModal.StartPosition = FormStartPosition.CenterScreen;
            GuideModal.FormBorderStyle = FormBorderStyle.None;
            GuideModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            GuideModal.BackgroundImageLayout = ImageLayout.Stretch;
            GuideModal.AutoSize = true;
        }
        private void SetGuidePanel()
        {
            Guide_Panel = new Panel();
            Guide_Panel.Size = new Size(475, 321);
            Guide_Panel.Location = new Point(184, 101);
            Guide_Panel.BackColor = Color.Transparent;
            GuideModal.Controls.Add(Guide_Panel);
        }
        private void SetGuideUI()
        {
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 23);
            var font1 = new Font(AssetsLoader.Instance.Fonts.Families[1], 17);

            var Title = CustomControls.Instance.CustomLabel("GUIDE", new Point(289, 27), Color.White, font, 1);

            Exit = new PictureBox();
            Exit.Size = new Size(30, 30);
            Exit.Location = new Point(GuideModal.Width - 60, 35);
            Exit.BackgroundImage = AssetsLoader.Instance.UIIcons["Exit Icon"];
            Exit.BackgroundImageLayout = ImageLayout.Stretch;


            PlayMode_Label = CustomControls.Instance.CustomLabel("-> PlayMode", new Point(22, 131), Color.White, font1, 0);
            Button_Label = CustomControls.Instance.CustomLabel("-> Button", new Point(22, 215), Color.White, font1, 0);
            Control_Label = CustomControls.Instance.CustomLabel("-> Control", new Point(22, 299), Color.White, font1, 0);


            GuideModal.Controls.Add(Title);
            GuideModal.Controls.Add(Exit);
            GuideModal.Controls.Add(PlayMode_Label);
            GuideModal.Controls.Add(Button_Label);
            GuideModal.Controls.Add(Control_Label);

        }

        private void SetPlayModeContent()
        {
            ResetGuidePanelUI();
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 20);
            var font1 = new Font(AssetsLoader.Instance.Fonts.Families[1], 15);
            var content1 = CustomControls.Instance.CustomLabel("Play New: ", new Point(20, 30), Color.Red, font, 1);
            var content2 = CustomControls.Instance.CustomLabel("Choose PlayNew to create new game\n", new Point(30, 80), Color.White, font1, 1);
            var content3 = CustomControls.Instance.CustomLabel("Load Play: ", new Point(20, 160), Color.Red, font, 1);
            var content4 = CustomControls.Instance.CustomLabel("Choose LoadPlay to play again \n" +
            "which game you failed", new Point(30, 210), Color.White, font1, 1);

            Guide_Panel.Controls.Add(content1);
            Guide_Panel.Controls.Add(content2);
            Guide_Panel.Controls.Add(content3);
            Guide_Panel.Controls.Add(content4);

        }

        private void SetButtonContent()
        {
            ResetGuidePanelUI();
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 17);
            var img1 = AssetsLoader.Instance.UIIcons["A Button Icon"];
            var img2 = AssetsLoader.Instance.UIIcons["S Button Icon"];
            var img3 = AssetsLoader.Instance.UIIcons["U Button Icon"];
            var img4 = AssetsLoader.Instance.UIIcons["I Button Icon"];
            var img5 = AssetsLoader.Instance.UIIcons["ESC Button Icon"];
            var pb1 = CustomControls.Instance.CustomPictureBox(new Size(32, 32), new Point(41, 30), img1, ImageLayout.Stretch);
            var pb2 = CustomControls.Instance.CustomPictureBox(new Size(32, 32), new Point(41, 80), img2, ImageLayout.Stretch);
            var pb3 = CustomControls.Instance.CustomPictureBox(new Size(32, 32), new Point(41, 130), img3, ImageLayout.Stretch);
            var pb4 = CustomControls.Instance.CustomPictureBox(new Size(32, 32), new Point(41, 180), img4, ImageLayout.Stretch);
            var pb5 = CustomControls.Instance.CustomPictureBox(new Size(32, 32), new Point(41, 230), img5, ImageLayout.Stretch);
            var content1 = CustomControls.Instance.CustomLabel("Attack", new Point(105, 30), Color.White, font, 1);
            var content2 = CustomControls.Instance.CustomLabel("Open Shop Modal", new Point(105, 80), Color.White, font, 1);
            var content3 = CustomControls.Instance.CustomLabel("Open Weapon Upgrade Modal", new Point(105, 130), Color.White, font, 1);
            var content4 = CustomControls.Instance.CustomLabel("Open Inventory Modal", new Point(105, 180), Color.White, font, 1);
            var content5 = CustomControls.Instance.CustomLabel("Pause the game", new Point(105, 230), Color.White, font, 1);


            Guide_Panel.Controls.Add(pb1);
            Guide_Panel.Controls.Add(pb2);
            Guide_Panel.Controls.Add(pb3);
            Guide_Panel.Controls.Add(pb4);
            Guide_Panel.Controls.Add(pb5);
            Guide_Panel.Controls.Add(content1);
            Guide_Panel.Controls.Add(content2);
            Guide_Panel.Controls.Add(content3);
            Guide_Panel.Controls.Add(content4);
            Guide_Panel.Controls.Add(content5);
        }


        private void SetControlContent()
        {
            ResetGuidePanelUI();
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 17);
            var img1 = AssetsLoader.Instance.UIIcons["ArrowDown Button Icon"];
            var img2 = AssetsLoader.Instance.UIIcons["ArrowLeft Button Icon"];
            var img3 = AssetsLoader.Instance.UIIcons["ArrowRight Button Icon"];
            var img4 = AssetsLoader.Instance.UIIcons["ArrowUp Button Icon"];
            var pb1 = CustomControls.Instance.CustomPictureBox(new Size(32, 32), new Point(41, 30), img1, ImageLayout.Stretch);
            var pb2 = CustomControls.Instance.CustomPictureBox(new Size(32, 32), new Point(41, 80), img2, ImageLayout.Stretch);
            var pb3 = CustomControls.Instance.CustomPictureBox(new Size(32, 32), new Point(41, 130), img3, ImageLayout.Stretch);
            var pb4 = CustomControls.Instance.CustomPictureBox(new Size(32, 32), new Point(41, 180), img4, ImageLayout.Stretch);
            var content1 = CustomControls.Instance.CustomLabel("Move down", new Point(105, 30), Color.White, font, 1);
            var content2 = CustomControls.Instance.CustomLabel("Move to left", new Point(105, 80), Color.White, font, 1);
            var content3 = CustomControls.Instance.CustomLabel("Move to right", new Point(105, 130), Color.White, font, 1);
            var content4 = CustomControls.Instance.CustomLabel("Move up", new Point(105, 180), Color.White, font, 1);
            Guide_Panel.Controls.Add(pb1);
            Guide_Panel.Controls.Add(pb2);
            Guide_Panel.Controls.Add(pb3);
            Guide_Panel.Controls.Add(pb4);
            Guide_Panel.Controls.Add(content1);
            Guide_Panel.Controls.Add(content2);
            Guide_Panel.Controls.Add(content3);
            Guide_Panel.Controls.Add(content4);

        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetEvents()
        {
            Exit.Click += Exit_Click;
            PlayMode_Label.Click += PlayMode_Click;
            Button_Label.Click += Button_Click;
            Control_Label.Click += Control_Click;
        }
        //Events
        private void Exit_Click(object sender, EventArgs e)
        {
            DoClose();
        }

        private void PlayMode_Click(object sender, EventArgs e)
        {
            SetPlayModeContent();
        }
        private void Button_Click(object sender, EventArgs e)
        {
            SetButtonContent();
        }
        private void Control_Click(object sender, EventArgs e)
        {
            SetControlContent();
        }
        //Behaviours
        private void ResetGuidePanelUI()
        {
            while (Guide_Panel.Controls.Count != 0)
            {
                foreach (Control c in Guide_Panel.Controls)
                {
                    Guide_Panel.Controls.Remove(c);
                    c.Dispose();
                    Guide_Panel.Invalidate();
                }
            }
        }
        public void DoOpen()
        {
            BackgroundModal.Show();
            GuideModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            GuideModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && GuideModal.Visible)
                return true;
            return false;
        }
    }
}
