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

namespace SoilPro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        Page InputsMain_Page = new Pages.Inputs.InputsMainPage();
        Page OptionsMain_Page = new Pages.Options.OptionsMainPage(); 
        Page ReportMain_Page = new Pages.Reports.ReportMainPage();
        public MainWindow()
        {
            
            InitializeComponent();
            
            MainScreen.Content = InputsMain_Page;
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            MainScreen.Content = OptionsMain_Page;
        }

        private void MainPage_Click(object sender, RoutedEventArgs e)
        {
            MainScreen.Content = InputsMain_Page;
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            MainScreen.Content = ReportMain_Page;
        }
    }
}
