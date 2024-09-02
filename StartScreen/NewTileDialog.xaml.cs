using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for NewTileDialog.xaml
    /// </summary>
    public partial class NewTileDialog : ContentDialog
    {
        public NewTileDialog()
        {
            InitializeComponent();
            foreach (var item in Enum.GetValues(typeof(TileBackend.tileSize)))
            {
                TileSizeComboBox.Items.Add(item);
            }
        }

        private void addTile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.homeScreen.tile.addTile(this.tileName.Text, this.programPath.Text, TileBackend.tileSize.Wide);
            MainWindow.Instance.homeScreen.beginTilesInit();
            this.Hide();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
