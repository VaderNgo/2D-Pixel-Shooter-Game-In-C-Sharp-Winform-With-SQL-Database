using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPixelShooterGame.OtherScripts
{
    public class AssetsLoader
    {
        private AssetsLoader() { }
        private static AssetsLoader instance;
        private readonly static object _lock = new object();
        public static AssetsLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new AssetsLoader();
                            instance.LoadAssets();
                        }
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }
        public Dictionary<string, Image> UIMaps = new Dictionary<string, Image>();
        public Dictionary<string, Image> UIIcons = new Dictionary<string, Image>();
        public Dictionary<string, Image> UIBorders = new Dictionary<string, Image>();
        public Dictionary<string, Image> UIBG = new Dictionary<string, Image>();
        public Dictionary<string, Image> UIPanels = new Dictionary<string, Image>();
        public Dictionary<string, Image> UIIconStages = new Dictionary<string, Image>();

        public Dictionary<string, Image> GOPlayer = new Dictionary<string, Image>();
        public Dictionary<string, Image> GOProjectile = new Dictionary<string, Image>();
        public Dictionary<string, Image> GOWeapon = new Dictionary<string, Image>();
        public Dictionary<string, Image> GOItem = new Dictionary<string, Image>();

        public Dictionary<string, string> Audio = new Dictionary<string, string>();

        public PrivateFontCollection Fonts = new PrivateFontCollection();
        private void LoadAssets()
        {
            LoadUIMaps();
            LoadUIBG();
            LoadUIBorders();
            LoadUIIcons();
            LoadUIPanels();
            LoadUIIconStages();
            LoadGOPlayer();
            LoadGOWeapons();
            LoadGOItems();
            LoadGOProjectiles();
            LoadAudio();
            LoadFonts();
        }
        private void LoadUIMaps()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\UI\\Maps", "*.png");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                UIMaps.Add(fileName, fileImage);
            }
        }
        private void LoadUIPanels()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\UI\\Panels", "*.png");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                UIPanels.Add(fileName, fileImage);
            }

        }
        private void LoadUIIcons()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\UI\\Icons", "*.png");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                UIIcons.Add(fileName, fileImage);
            }
        }
        private void LoadUIBG()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\UI\\BG", "*.png");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                UIBG.Add(fileName, fileImage);
            }
        }
        private void LoadUIBorders()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\UI\\Borders", "*.png");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                UIBorders.Add(fileName, fileImage);
            }
        }

        private void LoadUIIconStages()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\UI\\Icons\\Stages", "*.png");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                UIIconStages.Add(fileName, fileImage);
            }
        }

        private void LoadGOPlayer()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\GameObject\\Player", "*.gif");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                GOPlayer.Add(fileName, fileImage);
            }
        }

        private void LoadGOProjectiles()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\GameObject\\Projectiles", "*.png");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                GOProjectile.Add(fileName, fileImage);
            }
        }
        private void LoadGOWeapons()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\GameObject\\Weapons", "*.png");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                GOWeapon.Add(fileName, fileImage);
            }
        }
        private void LoadGOItems()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\GameObject\\Items", "*.png");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                GOItem.Add(fileName, fileImage);
            }
        }

        public Dictionary<string, Image> LoadGOMob(string Name)
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\GameObject\\Mobs\\" + Name, "*.gif");
            var stores = new Dictionary<string, Image>();
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var fileImage = Image.FromFile(file);
                stores[fileName] = fileImage;
            }
            return stores;
        }
        private void LoadFonts()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\Fonts", "*.ttf");
            foreach (var file in files)
            {
                Fonts.AddFontFile(file);
            }
        }

        public void LoadAudio()
        {
            var files = Directory.GetFiles("..\\..\\..\\Assets\\Audio", "*.wav");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                Audio.Add(fileName, Path.GetFullPath(file));
            }
        }
    }
}
