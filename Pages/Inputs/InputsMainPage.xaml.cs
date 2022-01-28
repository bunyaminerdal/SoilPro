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
using ExDesign.Datas;
using ExDesign.Scripts;

namespace ExDesign.Pages.Inputs
{
    /// <summary>
    /// InputsMainPage.xaml etkileşim mantığı
    /// </summary>
    public partial class InputsMainPage : Page
    {
        MaterialsPage materialsPage = new MaterialsPage();
        WallProperties wallProperties   =new WallProperties();
        ExDesignPage exDesignPage = new ExDesignPage();
        public InputsMainPage()
        {
            InitializeComponent();            
            materialsPage.SetViews();
            MaterialsBttn.IsChecked = true;
        }
        

        private void MaterialsBttn_Checked(object sender, RoutedEventArgs e)
        {
            Main_pro.Content = materialsPage;
        }
        private void WallPropertiesBttn_Checked(object sender, RoutedEventArgs e)
        {
            Main_pro.Content = wallProperties;
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
                case 3:
                    StaticVariables.CurrentUnit = Units.ton_mm;
                    break;
                case 4:
                    StaticVariables.CurrentUnit = Units.ton_cm;
                    break;
                case 5:
                    StaticVariables.CurrentUnit = Units.ton_m;
                    break;
                case 6:
                    StaticVariables.CurrentUnit = Units.N_mm;
                    break;
                case 7:
                    StaticVariables.CurrentUnit = Units.N_cm;
                    break;
                case 8:
                    StaticVariables.CurrentUnit = Units.N_m;
                    break;
                case 9:
                    StaticVariables.CurrentUnit = Units.kN_mm;
                    break;
                case 10:
                    StaticVariables.CurrentUnit = Units.kN_cm;
                    break;
                case 11:
                    StaticVariables.CurrentUnit = Units.kN_m;
                    break;

                default:
                    StaticVariables.CurrentUnit = Units.kN_m;
                    break;
            }
            StaticVariables.viewModel.UnitIndex = ((ComboBox)sender).SelectedIndex;
            StaticEvents.UnitChangeEvent?.Invoke();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Main_pro.Content = materialsPage;
            UnitCombobox.SelectedIndex = StaticVariables.viewModel.UnitIndex;
            Concrete.ConcreteDataReader();
            Rebar.RebarDataReader();
            Steel.SteelDataReader();
            Pile.pileDiameterReader();
            Sheet.SheetDataReader();            
            StaticVariables.view3DPage.Refresh3Dview();
        }
                
    }
}
