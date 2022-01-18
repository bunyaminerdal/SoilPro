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
using SoilPro.Scripts;

namespace SoilPro.Pages.Inputs
{
    /// <summary>
    /// InputsMainPage.xaml etkileşim mantığı
    /// </summary>
    public partial class InputsMainPage : Page
    {
        
        WallTypePage wallTypePage = new WallTypePage();
        MaterialsPage materialsPage = new MaterialsPage();
        ExDesignPage exDesignPage = new ExDesignPage( );
        
        public InputsMainPage()
        {
            InitializeComponent();
            walltypeBttn.IsChecked = true;
            Main_pro.Content = wallTypePage;

        }

        private void walltypeBttn_Checked(object sender, RoutedEventArgs e)
        {
            Main_pro.Content = wallTypePage;
        }

        private void MaterialsBttn_Checked(object sender, RoutedEventArgs e)
        {
            Main_pro.Content = materialsPage;
        }
        private void ExDesignBttn_Checked(object sender, RoutedEventArgs e)
        {
            Main_pro.Content = exDesignPage;

        }
        private void UnitCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    StaticVariables.CurrentUnit = Units.kg_mm;
                    break;
                case 1:
                    StaticVariables.CurrentUnit = Units.kg_cm;
                    break;
                case 2:
                    StaticVariables.CurrentUnit = Units.kg_m;
                    break;
                default:
                    break;
            }
            
            StaticEvents.UnitChangeEvent?.Invoke();
        }

       
    }
}
