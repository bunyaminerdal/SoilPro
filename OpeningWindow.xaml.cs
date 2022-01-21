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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SoilPro
{
    /// <summary>
    /// OpeningWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class OpeningWindow : Window
    {
        Page OpeningPage = new Pages.Opening.OpeningMainPage();
        public OpeningWindow()
        {
            InitializeComponent();
            OpeningScreen.Content = OpeningPage;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (App.Current.Windows.Count<2)
            {
                App.Current.MainWindow.Close();
            }
        }
    }
}
