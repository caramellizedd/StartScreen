﻿using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for AllApps.xaml
    /// </summary>
    public partial class AllApps : Page
    {
        List<string> appTag = new List<string>();
        public AllApps()
        {
            InitializeComponent();
            listBox.Loaded += ListBox_Loaded;
            listBox.Unloaded += ListBox_Unloaded;
            listBox.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            listBox.ItemsSource = MainWindow.Instance.appListNameFriendly;
            foreach (AppsIcons obj in MainWindow.Instance.appList)
            {
                Logger.info("Adding " + obj.Name + " to tag list");
                appTag.Add(obj.Name);
            }
            Logger.info("[AllApps] Menu Executed!");
        }

        private void ListBox_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow.Instance.content.GoBack();
            //MainWindow.Instance.imageBackground.Opacity = 1;
            //MainWindow.Instance.imageBackground.Effect = new BlurEffect { Radius = 24, RenderingBias = RenderingBias.Performance };
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (String obj in appTag)
            {
                String[] temp = obj.Split('[');
                AppsIcons? temp2 = listBox.SelectedItem as AppsIcons;
                //Button temp3 = AppsIcons
                //Logger.info(temp2.Name);
                if (temp2 == null) return;
                if (temp[0].Equals(temp2.Name))
                {
                    Logger.info("Starting selected app");
                    Process.Start("explorer.exe", @" shell:appsFolder\" + temp[1]);
                    Home.closeAppAnim();
                }
            }
            listBox.UnselectAll();
            MainWindow.Instance.content.GoBack();
        }

        private void listBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void listBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void listBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
    public class AppsIcons
    {
        public BitmapSource Icon { get; set; }
        public string Name { get; set; }
    }
    public enum SortGroup
    {
        Symbol = 0,
        A, B, C, D, E, F, G, H, I, J, K,
        L, M, N, O, P, Q, R, S, T, U, V,
        W, X, Y, Z
    }

}
