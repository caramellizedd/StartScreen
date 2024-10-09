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
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Drawing;
using static StartScreen.pinvoke;
using System.Windows.Forms;

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
        
        bool initialized = false;
        // Use the user wallpaper as the background if true
        // If false it blurs the content behind the start screen (STATIC)
        bool userBackgroundEnabled = true;
        
        public AllApps allApps;
        public Home homeScreen;
        
        public MainWindow()
        {
            this.Hide();
            alreadyShowing = true;
            InitializeComponent();
            Logger.info("Object initialized!");
            Instance = this;
            Logger.info("Instance has been set to \"This\"!");
            //this.Opacity = 0;
            
            Logger.info("Opacity has been set to 0");
            // Set window to topmost
            // Prevents stuff from covering the startscreen.
            Topmost = true;
            Logger.info("Window has been set to Top-Most");
            this.ShowInTaskbar = false;
            Logger.info("Successfully hidden from taskbar");
            if (!initialized)
            {
                Logger.info("windowAnim timer has initialized and started");
                DispatcherTimer checkLauncher = new DispatcherTimer();
                checkLauncher.Tick += new EventHandler(launcherTick);
                checkLauncher.Interval = new TimeSpan(0, 0, 0, 0, 1);
                checkLauncher.Start();
                Logger.info("Launcher Checker has been started");
                Loaded += MainWindow_Loaded;
                this.Background = SystemParameters.WindowGlassBrush;
                Logger.info("Background has been set to Accent Color");
                homeScreen = new Home();
                content.Navigate(homeScreen, new EntranceNavigationTransitionInfo());
                new Thread(() =>
                {
                    if(userBackgroundEnabled)
                    {
                        Logger.info("Getting user background");
                        // Background Logic
                        MemoryStream ms = new MemoryStream();
                        Logger.info("ms Instance: " + ms.ToString());
                        // Save to a memory stream...
                        Utils.getDesktopWallpaper().Save(ms, ImageFormat.Bmp);
                        Logger.info("Desktop Wallpaper saved as BMP in Memory");
                        ms.Seek(0, SeekOrigin.Begin);
                        Logger.info("Resetted Memory Stream Seek Distance to 0");
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        Logger.info("BitmapImage has been initialized");
                        bi.StreamSource = ms;
                        bi.EndInit();
                        bi.Freeze();
                        Logger.info("BitmapImage has been frozen");
                        this.Dispatcher.Invoke(() =>
                        {
                            Logger.info("Setting imageBackground as User Background");
                            //imageBackground.Source = bi;
                            this.Background = new ImageBrush(bi);
                        });
                    }
                    else
                    {
                        Logger.info("Capturing the desktop as background...");
                        // Background Logic
                        MemoryStream ms = new MemoryStream();
                        Logger.info("ms Instance: " + ms.ToString());
                        // Save to a memory stream...
                        Utils.getDesktop().Save(ms, ImageFormat.Bmp);
                        Logger.info("Desktop Wallpaper saved as BMP in Memory");
                        ms.Seek(0, SeekOrigin.Begin);
                        Logger.info("Resetted Memory Stream Seek Distance to 0");
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        Logger.info("BitmapImage has been initialized");
                        bi.StreamSource = ms;
                        bi.EndInit();
                        bi.Freeze();
                        Logger.info("BitmapImage has been frozen");
                        this.Dispatcher.Invoke(() =>
                        {
                            Logger.info("Setting imageBackground as User Background");
                            //imageBackground.Source = bi;
                            //imageBackground.Stretch = Stretch.None;
                            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0,0,0));
                        });
                    }
                    
                }).Start();
                
                //imageBackground.Opacity = 1;
                Logger.info("Background Opacity has been set to 1");
                //imageBackground.Effect = new BlurEffect { Radius = 30, RenderingBias = RenderingBias.Performance };
                Logger.info("Background Blur effect has been added");
                var FOLDERID_AppsFolder = new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}");
                ShellObject appsFolder = (ShellObject)KnownFolderHelper.FromKnownFolderId(FOLDERID_AppsFolder);
                Logger.info("Listing All Apps");
                foreach (var app in (IKnownFolder)appsFolder)
                {
                    
                    Logger.info("Name : " + app.Name + " | ID : " + app.ParsingName);
                    // The friendly app name
                    string name = app.Name;
                    // The ParsingName property is the AppUserModelID
                    string appUserModelID = app.ParsingName; // or app.Properties.System.AppUserModel.ID
                                                             // You can even get the Jumbo icon in one shot
                    BitmapSource icon = app.Thumbnail.SmallBitmapSource;
                    //appList.SortDescriptions.Add(new System.ComponentModel.SortDescription("NAME", System.ComponentModel.ListSortDirection.Ascending));
                    appList.Add(new AppsIcons { Icon = icon, Name = name + "[" + appUserModelID });
                    appListNameFriendly.Add(new AppsIcons { Icon = icon, Name = name });
                    Logger.info("Added Successfully");
                }
                initialized = true;
                allApps = new AllApps();
                
            }
            this.Show();
        }
        public List<AppsIcons> appList = new List<AppsIcons>();
        public List<AppsIcons> appListNameFriendly = new List<AppsIcons>();
        public bool alreadyShowing = false;
        private void launcherTick(object? sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("ScreenLaunch");
            if (pname.Length >= 1)
            {
                if (alreadyShowing)
                {
                    Home.closeAppAnim();
                    foreach (Process p in pname)
                    {
                        p.Kill();
                    }
                    return;
                }
                //this.Opacity = 0;
                this.Show();
                //Home.beginTilesInit();
                counter.Start();
                foreach (Process p in pname)
                {
                    p.Kill();
                }
                new Thread(() =>
                {
                    if (userBackgroundEnabled)
                    {
                        Logger.info("Getting user background");
                        // Background Logic
                        MemoryStream ms = new MemoryStream();
                        Logger.info("ms Instance: " + ms.ToString());
                        // Save to a memory stream...
                        Utils.getDesktopWallpaper().Save(ms, ImageFormat.Bmp);
                        Logger.info("Desktop Wallpaper saved as BMP in Memory");
                        ms.Seek(0, SeekOrigin.Begin);
                        Logger.info("Resetted Memory Stream Seek Distance to 0");
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        Logger.info("BitmapImage has been initialized");
                        bi.StreamSource = ms;
                        bi.EndInit();
                        bi.Freeze();
                        Logger.info("BitmapImage has been frozen");
                        this.Dispatcher.Invoke(() =>
                        {
                            Logger.info("Setting imageBackground as User Background");
                            //imageBackground.Source = bi;
                        });
                    }
                    else
                    {
                        Logger.info("Getting user background");
                        // Background Logic
                        MemoryStream ms = new MemoryStream();
                        Logger.info("ms Instance: " + ms.ToString());
                        // Save to a memory stream...
                        Utils.getDesktop().Save(ms, ImageFormat.Bmp);
                        Logger.info("Desktop Wallpaper saved as BMP in Memory");
                        ms.Seek(0, SeekOrigin.Begin);
                        Logger.info("Resetted Memory Stream Seek Distance to 0");
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        Logger.info("BitmapImage has been initialized");
                        bi.StreamSource = ms;
                        bi.EndInit();
                        bi.Freeze();
                        Logger.info("BitmapImage has been frozen");
                        this.Dispatcher.Invoke(() =>
                        {
                            Logger.info("Setting imageBackground as User Background");
                            //imageBackground.Source = bi;
                            //imageBackground.Stretch = Stretch.None;
                            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                        });
                    }

                }).Start();
                homeScreen.beginTilesInit();
                startPressed = false;
                alreadyShowing = true;
            }
        }

        public bool hidden = false;
        private HwndSource hwndSource;
        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return IntPtr.Zero;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!userBackgroundEnabled)
            {
                Utils.EnableBlur(new WindowInteropHelper(this).Handle);
            }
            hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            Logger.info("Hooking the start button and other start menu hotkeys...");
            //hwndSource.AddHook(new HwndSourceHook(WndProc));
            hook.HookedKeys.Add(Keys.LWin);
            hook.HookedKeys.Add(Keys.RWin);
            hook.HookedKeys.Add(Keys.Escape);
            hook.HookedKeys.Add(Keys.R);
            hook.KeyDown += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);
            hook.KeyUp += new System.Windows.Forms.KeyEventHandler(hook_KeyUp);
            hookProc = hook.hookProc;
            hook.hook(this.hookProc);
        }
        globalKeyboardHook.keyboardHookProc hookProc;
        private void hook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            bool isModifierKeysPressed = Keyboard.Modifiers == ModifierKeys.Control;
            if (e.KeyCode == System.Windows.Forms.Keys.LWin || e.KeyCode == Keys.RWin)
            {
                if (Keyboard.IsKeyDown(System.Windows.Input.Key.R))
                {
                    System.Windows.MessageBox.Show("lol");
                    return;
                }
                if (alreadyShowing) { HideWindow(); }
                else { this.Show(); alreadyShowing = true; }
                e.Handled = true;
            }
            else if (isModifierKeysPressed && e.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                if (alreadyShowing) { HideWindow(); }
                else { this.Show(); alreadyShowing = true; }
                e.Handled = true;
            }
        }

        private void hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            
            bool isModifierKeysPressed = Keyboard.Modifiers == ModifierKeys.Control;
            if (e.KeyCode == System.Windows.Forms.Keys.LWin || e.KeyCode == System.Windows.Forms.Keys.RWin)
            {
                if (Keyboard.IsKeyDown(System.Windows.Input.Key.R))
                {
                    System.Windows.MessageBox.Show("lol");
                    return;
                }
                e.Handled = true;
            }
            else if (isModifierKeysPressed && e.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                e.Handled = true;
            }
        }

        globalKeyboardHook hook = new globalKeyboardHook();
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
            hook.unhook();
        }

        private void mainWindow_Deactivated(object sender, EventArgs e)
        {
            if (startPressed) return;
            Home.closeAppAnim();
        }
        public void HideWindow()
        {
            alreadyShowing = false;
            this.Hide();
            //counter2.Start();
        }
    }
}
