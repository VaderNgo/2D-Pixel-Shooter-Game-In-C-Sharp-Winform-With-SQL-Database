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
    public class LoadPlayScreen
    {
        private LoadPlayScreen() { }
        private static LoadPlayScreen instance;
        private static readonly object _lock = new object();

        private Form LoadPlayModal;
        private Form BackgroundModal;
        private DataGridView PlayHistoryGridView;
        private Button PlayAgain_Button;
        private PictureBox Exit;

        private DataGridViewRow SelectedRow;

        public bool isSetUp = false;
        public static LoadPlayScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new LoadPlayScreen();

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
            SetLoadPlayModal();
            SetBackgroundModal();
            SetLoadPlayUI();
            SetEvents();
            isSetUp = true;
        }
        private void SetLoadPlayModal()
        {
            LoadPlayModal = new Form();
            LoadPlayModal.Size = new Size(800, 400);
            LoadPlayModal.StartPosition = FormStartPosition.CenterScreen;
            LoadPlayModal.FormBorderStyle = FormBorderStyle.None;
            LoadPlayModal.MinimizeBox = false;
            LoadPlayModal.MaximizeBox = false;
            LoadPlayModal.ShowIcon = false;
            LoadPlayModal.BackgroundImage = AssetsLoader.Instance.UIBG["PanelBG"];
            LoadPlayModal.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void SetBackgroundModal()
        {
            BackgroundModal = CustomControls.Instance.BackgroundModal();
        }
        private void SetLoadPlayUI()
        {
            var font = new Font(AssetsLoader.Instance.Fonts.Families[1], 15);
            var font1 = new Font(AssetsLoader.Instance.Fonts.Families[0], 7);

            DatabaseController.Instance.GetAllTablesData();
            PlayHistoryGridView = new DataGridView();
            PlayHistoryGridView.Size = new Size(750, 250);
            PlayHistoryGridView.Location = new Point(20, 70);
            PlayHistoryGridView.Font = font1;
            PlayHistoryGridView.AutoGenerateColumns = true;
            PlayHistoryGridView.RowHeadersVisible = false;
            var tb = DatabaseController.Instance.DataSet.Tables["PLAY"];
            PlayHistoryGridView.DataSource = tb;
            PlayHistoryGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
            PlayHistoryGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PlayHistoryGridView.MultiSelect = false;
            PlayHistoryGridView.ReadOnly = true;

            PlayAgain_Button = CustomControls.Instance.CustomBtn("Play Again", new Point(340, 330), Color.Red, font);

            Exit = new PictureBox();
            Exit.Size = new Size(30, 30);
            Exit.Location = new Point(LoadPlayModal.Width - 70, 20);
            Exit.BackgroundImage = AssetsLoader.Instance.UIIcons["Exit Icon"];
            Exit.BackgroundImageLayout = ImageLayout.Stretch;

            LoadPlayModal.Controls.Add(PlayAgain_Button);
            LoadPlayModal.Controls.Add(PlayHistoryGridView);
            LoadPlayModal.Controls.Add(Exit);
        }
        private void SetEvents()
        {
            PlayAgain_Button.Click += PlayAgain_Button_Click;
            PlayHistoryGridView.SelectionChanged += PlayHistoryGridView_SelectionChanged;
            PlayHistoryGridView.RowPrePaint += PlayHistoryGridView_RowPrePaint;
            Exit.Click += Exit_Click;
        }
        //Events
        private void PlayHistoryGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (PlayHistoryGridView.DataSource != null)
            {
                var rowsCount = this.PlayHistoryGridView.SelectedRows.Count;
                if (rowsCount == 0)
                    return;
                else
                {
                    SelectedRow = this.PlayHistoryGridView.SelectedRows[0];
                }
            }
        }
        private void PlayHistoryGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < PlayHistoryGridView.RowCount - 1)
            {
                if (Convert.ToInt32(PlayHistoryGridView.Rows[e.RowIndex].Cells[6].Value) == 1)
                {
                    PlayHistoryGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    PlayHistoryGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightSalmon;
                }
            }
        }
        private void PlayAgain_Button_Click(object sender, EventArgs e)
        {
            DoPlayAgain();
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            DoClose();
        }
        //Behaviours
        private void DoPlayAgain()
        {
            var row = this.SelectedRow;
            if (row != null)
            {
                if (!row.Cells[6].Value.ToString().Equals("1"))
                {

                    DoClose();
                    WelcomeScreen.Instance.DoClose();

                    GameManager.Instance.PlayGameAgain(
                        row.Cells[2].Value.ToString(),
                        row.Cells[3].Value.ToString(),
                        Convert.ToInt32(row.Cells[4].Value),
                        Convert.ToInt32(row.Cells[5].Value),
                        char.Parse(row.Cells[6].Value.ToString()),
                        Convert.ToInt32(row.Cells[7].Value) + 1
                        );
                }
                else
                {
                    MessageBox.Show("This play is won and completed");
                }
            }
        }
        public void DoOpen()
        {
            BackgroundModal.Show();
            LoadPlayModal.Show();
        }
        public void DoClose()
        {
            BackgroundModal.Hide();
            LoadPlayModal.Hide();
        }
        public bool isOpenning()
        {
            if (BackgroundModal.Visible && LoadPlayModal.Visible)
                return true;
            return false;
        }
    }
}
