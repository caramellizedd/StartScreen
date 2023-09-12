using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        //public List<Button> Tiles = new List<Button>();
        public static Home Instance;
        //public List<tileData> tiles = new List<tileData>();
        TileBackend tile = new TileBackend();
        //public static Button desktopTile;
        
        public Home()
        {
            Instance = this;
            InitializeComponent();
            //desktopTile = DesktopTile;
            //Tiles.Add(DesktopTile);
            username.Content = Environment.UserName;
            // Profile Picture
            var image = new ImageBrush();
            image.ImageSource = Utils.GetUserimage();
            profilePicture.Fill = image;
            //DesktopTile.Background = SystemParameters.WindowGlassBrush;
            beginTilesInit();
            MainWindow.Instance.counter2.Tick += new EventHandler(MainWindow.Instance.windowAnim2);
            MainWindow.Instance.counter2.Interval = new TimeSpan(0, 0, 0, 0, 2);
        }
        public void beginTilesInit()
        {
            Logger.info("Initializing Tiles");
            tile.initDefaultTiles();
            //tiles.Add(new tileData { Size = tileSize.small });
            // Desktop Background Image
            Logger.info("Getting background image for desktop tile");
            var bgImageBrush = new ImageBrush(BitmapFromUri(new Uri(Utils.getWallpaperPath())));
            bgImageBrush.Stretch = Stretch.UniformToFill;
            //desktopTile.Background = bgImageBrush;
            //this.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0, 0, 0));
            foreach(TileBackend.tileData data in tile.data) 
            {
                Tile tile;
                if (data.name == "Desktop")
                {
                    tile = new Tile
                    {
                        Content = data.name,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Bottom,
                        Background = bgImageBrush
                    };
                    TileList.Items.Add(tile);
                    tile.Click += hideDesktopTile_Click;
                }
                else
                {

                }
            }
        }

        private void hideDesktopTile_Click(object sender, RoutedEventArgs e)
        {
            closeAppAnim();
        }

        public static ImageSource BitmapFromUri(Uri source)
        {
            if (source == null)
                return new BitmapImage(source);

            using (var fs = new FileStream(source.LocalPath, FileMode.Open))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = fs;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            closeAppAnim();
        }
        public static void closeAppAnim()
        {
            try
            {
                Logger.info("Hiding StartScreen!");
                MainWindow.Instance.alreadyShowing = false;
                Thread.Sleep(100);
                MainWindow.Instance.HideWindow();
            }
            catch
            {

            }
        }
        private void AllApps_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.content.Navigate(MainWindow.Instance.allApps, new EntranceNavigationTransitionInfo());
        }

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void Search_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void userButton_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "ms-settings:yourinfo",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void powerAction_Click(object sender, RoutedEventArgs e)
        {
            powerAction.ContextMenu.IsOpen = true;
        }

        private void powerOff_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("shutdown", "-s -hybrid -t 000");
        }

        private void reboot_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("shutdown", "-r -t 000");
        }

        [DllImport("user32.dll")]
        public static extern int PostMessage(int h,int m,int w,int l);
        private void standby_Click(object sender, RoutedEventArgs e)
        {
            PostMessage(-1, 0x0112, 0xF170, 2);
        }
        public void ExecuteAsAdmin(string fileName, string args)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }
    }
    
}
