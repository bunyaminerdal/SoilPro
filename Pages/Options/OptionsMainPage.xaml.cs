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

namespace ExDesign.Pages.Options
{
    /// <summary>
    /// OptionsMainPage.xaml etkileşim mantığı
    /// </summary>
    public partial class OptionsMainPage : Page
    {
        Page GeneralOptions_Page = new Pages.Options.GeneralOptions();
        Page RegionOptions_Page = new Pages.Options.RegionOptions();
        public OptionsMainPage()
        {
            InitializeComponent();
            Options_main.Content = GeneralOptions_Page;
            GeneralOptionsRadioBttn.IsChecked = true;  

        }

        private void General_Clicked(object sender, RoutedEventArgs e)
        {
            Options_main.Content = GeneralOptions_Page;

        }

        private void Region_Clicked(object sender, RoutedEventArgs e)
        {
            Options_main.Content = RegionOptions_Page;

        }
    }
}
