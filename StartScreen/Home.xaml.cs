using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
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

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public List<Button> Tiles = new List<Button>();
        public static Home Instance;
        public static Button desktopTile;
        public Home()
        {
            Instance = this;
            InitializeComponent();
            desktopTile = DesktopTile;
            Tiles.Add(DesktopTile);
            username.Content = Environment.UserName;
            // Profile Picture
            var image = new ImageBrush();
            image.ImageSource = Utils.GetUserimage();
            profilePicture.Fill = image;
            DesktopTile.Background = SystemParameters.WindowGlassBrush;
            beginTilesInit();
            MainWindow.Instance.counter2.Tick += new EventHandler(MainWindow.Instance.windowAnim2);
            MainWindow.Instance.counter2.Interval = new TimeSpan(0, 0, 0, 0, 2);
        }
        public static void beginTilesInit()
        {
            // Desktop Background Image
            
            var bgImageBrush = new ImageBrush(BitmapFromUri(new Uri(Utils.getWallpaperPath())));
            bgImageBrush.Stretch = Stretch.UniformToFill;
            desktopTile.Background = bgImageBrush;
            //this.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0, 0, 0));
            
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
                Thread.Sleep(100);
                MainWindow.Instance.HideWindow();
            }
            catch
            {

            }
        }
        private void AllApps_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.content.Navigate(new AllApps(), new EntranceNavigationTransitionInfo());
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
    }
    public class tileData
    {
        public int Size { get; set; }
        public int name { get; set; }
        public int programPath { get; set; }
        public int tilePos { get; set; }
    }
}
