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

namespace ExDesign
{
    /// <summary>
    /// OpeningWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class OpeningWindow : Window
    {
        Pages.Opening.OpeningMainPage OpeningPage = new Pages.Opening.OpeningMainPage();
        public OpeningWindow()
        {
            InitializeComponent();
            OpeningScreen.Content = OpeningPage;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void OpeningNew()
        {
            OpeningPage.NewProjectOpening();
        }
    }
}
