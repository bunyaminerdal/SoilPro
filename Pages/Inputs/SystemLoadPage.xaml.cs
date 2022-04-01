using ExDesign.Scripts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ExDesign.Pages.Inputs
{
    /// <summary>
    /// SystemLoadPage.xaml etkileşim mantığı
    /// </summary>
    public partial class SystemLoadPage : Page
    {
        public char separator = ',';

        public SystemLoadPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
        }
    }
}
