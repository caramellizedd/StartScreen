using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for AllApps.xaml
    /// </summary>
    public partial class AllApps : Page
    {
        public AllApps()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.Instance.content.GoBack();
            MainWindow.Instance.imageBackground.Opacity = 1;
            MainWindow.Instance.imageBackground.Effect = new BlurEffect { Radius = 24, RenderingBias = RenderingBias.Performance };
        }
    }
}
