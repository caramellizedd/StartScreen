using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using ModernWpf.Media.Animation;
using System.Windows.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using System.Diagnostics.Metrics;
using System.Windows.Threading;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Security.RightsManagement;
using Microsoft.WindowsAPICodePack.Shell;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        public DispatcherTimer counter2 = new DispatcherTimer();
        DispatcherTimer counter = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            this.Opacity = 0;
            Topmost = true;
            this.ShowInTaskbar = false;
            counter.Tick += new EventHandler(windowAnim);
            counter.Interval = new TimeSpan(0, 0, 0, 0, 2);
            counter.Start();
            DispatcherTimer checkLauncher = new DispatcherTimer();
            checkLauncher.Tick += new EventHandler(launcherTick);
            checkLauncher.Interval = new TimeSpan(0, 0, 0, 0, 1);
            checkLauncher.Start();
            Loaded += MainWindow_Loaded;
            this.Background = SystemParameters.WindowGlassBrush;
            content.Navigate(new Home(), new EntranceNavigationTransitionInfo());
            if (Utils.getWallpaperPath().Contains("Transcoded"))
            {
                imageBackground.Stretch = Stretch.UniformToFill;
            }
            imageBackground.Source = Home.BitmapFromUri(new Uri(Utils.getWallpaperPath()));
            imageBackground.Opacity = 1;
            imageBackground.Effect = new BlurEffect { Radius = 24, RenderingBias = RenderingBias.Performance };
            counter2.Tick += new EventHandler(MainWindow.Instance.windowAnim2);
            counter2.Interval = new TimeSpan(0, 0, 0, 0, 2);
            var FOLDERID_AppsFolder = new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}");
            ShellObject appsFolder = (ShellObject)KnownFolderHelper.FromKnownFolderId(FOLDERID_AppsFolder);

            foreach (var app in (IKnownFolder)appsFolder)
            {
                // The friendly app name
                string name = app.Name;
                // The ParsingName property is the AppUserModelID
                string appUserModelID = app.ParsingName; // or app.Properties.System.AppUserModel.ID
                                                         // You can even get the Jumbo icon in one shot
                BitmapSource icon = app.Thumbnail.SmallBitmapSource;
                //appList.SortDescriptions.Add(new System.ComponentModel.SortDescription("NAME", System.ComponentModel.ListSortDirection.Ascending));
                appList.Add(new AppsIcons { Icon = icon, Name = name + "[" + appUserModelID});
                appListNameFriendly.Add(new AppsIcons { Icon = icon, Name = name });
            }
            
        }
        public List<AppsIcons> appList = new List<AppsIcons>();
        public List<AppsIcons> appListNameFriendly = new List<AppsIcons>();
        private void launcherTick(object? sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("ScreenLaunch");
            if (pname.Length >= 1)
            {
                this.Opacity = 0;
                this.Show();
                if (Utils.getWallpaperPath().Contains("Transcoded"))
                {
                    imageBackground.Stretch = Stretch.UniformToFill;
                }
                imageBackground.Source = Home.BitmapFromUri(new Uri(Utils.getWallpaperPath()));
                Home.beginTilesInit();
                counter.Start();
                foreach (Process p in pname)
                {
                    p.Kill();
                }
                startPressed = false;
            }
        }

        public void windowAnim2(object? sender, EventArgs e)
        {
            if (mainWindow.Opacity > 0.1)
            {
                mainWindow.Opacity -= 0.1;
            }
            else
            {
                counter2.Stop();
                //MainWindow.Instance.closeAnimDone = true;
                MainWindow.Instance.Hide();
                GC.Collect();
            }
        }

        private void windowAnim(object? sender, EventArgs e)
        {
            if (mainWindow.Opacity < 1)
                mainWindow.Opacity += 0.1;
            else
                counter.Stop();
        }

        public bool hidden = false;
        private HwndSource hwndSource;
        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return IntPtr.Zero;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }
        bool startPressed = false;
        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.LWin || e.Key == Key.RWin)
            {
                startPressed = true;
                Home.closeAppAnim();
            }
        }
        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void mainWindow_Deactivated(object sender, EventArgs e)
        {
            if (startPressed) return;
            Home.closeAppAnim();
        }
        public void HideWindow()
        {
            counter2.Start();
        }
    }
}
