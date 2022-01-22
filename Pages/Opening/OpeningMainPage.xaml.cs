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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExDesign.Pages.Opening
{
    /// <summary>
    /// OpeningMainPage.xaml etkileşim mantığı
    /// </summary>
    public partial class OpeningMainPage : Page
    {
        Page newsPage = new Pages.Opening.newsPage();
        Page newProjectPage = new Pages.Opening.NewProjectPage();
        public OpeningMainPage()
        {
            InitializeComponent();
            OpeningScreen.Content = newsPage;
        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            OpeningScreen.Content = newProjectPage;
        }
    }
}
