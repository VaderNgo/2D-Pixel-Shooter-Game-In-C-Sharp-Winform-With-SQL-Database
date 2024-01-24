using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.OtherScripts
{
    public class CustomControls
    {
        private CustomControls() { }
        private static CustomControls instance;
        private static readonly object _lock = new object();
        public static CustomControls Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new CustomControls();

                        }
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }

        public Label CustomLabel(string Content, Point Coor, Color TextColor, Font TextFont, int type)
        {
            Label label = new Label();
            label.Text = Content;
            label.Location = Coor;
            label.ForeColor = TextColor;
            label.Font = TextFont;
            label.AutoSize = true;
            label.BackColor = System.Drawing.Color.Transparent;
            if (type == 0)
            {
                label.MouseEnter += (object o, EventArgs e) =>
                {
                    label.ForeColor = Color.Red;
                    label.Location = new Point(Coor.X + 20, Coor.Y);
                };

                label.MouseLeave += (object o, EventArgs e) =>
                {
                    label.ForeColor = TextColor;
                    label.Location = Coor;
                };
            }
            return label;
        }
        public Button CustomBtn(string Content, Point Coor, Color TextColor, Font TextFont, Image Icon = null, Image Background = null)
        {
            Button btn = new Button();
            btn.Text = Content;
            btn.Location = Coor;
            btn.ForeColor = TextColor;
            btn.Font = TextFont;
            btn.AutoSize = true;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            if (Background != null)
            {
                btn.BackColor = System.Drawing.Color.Transparent;
                btn.BackgroundImage = Background;
                btn.BackgroundImageLayout = ImageLayout.Stretch;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            }
            if (Icon != null)
            {
                btn.Image = Icon;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextAlign = ContentAlignment.MiddleLeft;

            }
            btn.EnabledChanged += (object sender, EventArgs e) =>
            {
                Button temp = (Button)sender;
                btn.ForeColor = TextColor;
            };
            return btn;
        }
        public PictureBox CustomPictureBox(Size size, Point point, Image img, ImageLayout imageLayout)
        {
            var pb = new PictureBox();
            pb.Size = size;
            pb.Location = point;
            pb.BackgroundImage = img;
            pb.BackgroundImageLayout = imageLayout;

            return pb;
        }
        public Form BackgroundModal()
        {
            Form form = new Form();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.None;
            form.BackColor = Color.Black;
            form.Opacity = 0.5d;
            form.Size = GameInit.Instance.gW.Size;
            form.Location = GameInit.Instance.gW.Location;
            form.ShowInTaskbar = false;
            return form;
        }
    }
}
