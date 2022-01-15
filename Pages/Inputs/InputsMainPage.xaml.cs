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

namespace SoilPro.Pages.Inputs
{
    /// <summary>
    /// InputsMainPage.xaml etkileşim mantığı
    /// </summary>
    public partial class InputsMainPage : Page
    {
        
        public Page view3dScreenPage = new Views.View3dPage();
        public Page sideviewPage = new Views.SideviewPage();
        WallTypePage wallTypePage = new WallTypePage();
        MaterialsPage materialsPage = new MaterialsPage();
        public InputsMainPage()
        {
            InitializeComponent();
            walltypeBttn.IsChecked = true;
            Main_pro.Content = wallTypePage;
            materialsPage.SetViewPages(view3dScreenPage,sideviewPage);
        }

        private void walltypeBttn_Checked(object sender, RoutedEventArgs e)
        {
            Main_pro.Content = wallTypePage;
        }

        private void MaterialsBttn_Checked(object sender, RoutedEventArgs e)
        {
            Main_pro.Content = materialsPage;
        }
    }
}
