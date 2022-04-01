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
        SoilMethodPage soilMethodPage = new SoilMethodPage();
        AnchorsPage anchorsPage = new AnchorsPage();
        StrutPage strutsPage = new StrutPage();
        SurchargePage surchargePage = new SurchargePage();
        SystemLoadPage systemloadPage = new SystemLoadPage();
        public InputsMainPage()
        {
            InitializeComponent();
            StaticEvents.StageChangeEvent += StageChange;
            StaticEvents.SetStageEvent += SetStage;
        }
        
        private void StageChange(Stage stage)
        {
            
            StaticVariables.viewModel.stage = stage;
            switch (StaticVariables.viewModel.stage)
            {
                case Stage.Materials:
                    Main_pro.Content = materialsPage;
                    break;
                case Stage.WallProperties:
                    Main_pro.Content = wallProperties;
                    break;
                case Stage.ExDesign:
                    Main_pro.Content= exDesignPage;
                    break;
                case Stage.SoilMethod:
                    Main_pro.Content = soilMethodPage;
                    break;
                case Stage.Anchors:
                    Main_pro.Content = anchorsPage;
                    break;
                case Stage.Struts:
                    Main_pro.Content = strutsPage;
                    break;
                case Stage.Surcharge:
                    Main_pro.Content = surchargePage;
                    break;
                case Stage.SystemLoad:
                    Main_pro.Content = systemloadPage;
                    break;
                default:
                    Main_pro.Content = materialsPage;
                    break;
            }
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        private void SetStage(Stage stage)
        {
            StaticEvents.StageChangeEvent?.Invoke(stage);
            switch (StaticVariables.viewModel.stage)
            {
                case Stage.Materials:
                    MaterialsBttn.IsChecked = true;
                    break;
                case Stage.WallProperties:
                    WallPropertiesBttn.IsChecked = true;
                    break;
                case Stage.ExDesign:
                    ExDesignBttn.IsChecked = true;
                    break;
                case Stage.SoilMethod:
                    SoilMethodBttn.IsChecked= true;
                    break;
                case Stage.Anchors:
                    AnchorBttn.IsChecked = true;
                    break;
                case Stage.Struts:
                    StrutBttn.IsChecked = true;
                    break;
                case Stage.Surcharge:
                    SurchargeBttn.IsChecked = true;
                    break;
                case Stage.SystemLoad:
                    SystemLoadBttn.IsChecked = true;
                    break;
                default:
                    MaterialsBttn.IsChecked = true;
                    break;
            }
        }
        private void MaterialsBttn_Checked(object sender, RoutedEventArgs e)
        {
            StaticEvents.StageChangeEvent?.Invoke(Stage.Materials);
        }
        private void WallPropertiesBttn_Checked(object sender, RoutedEventArgs e)
        {
            StaticEvents.StageChangeEvent?.Invoke(Stage.WallProperties);
        }
        private void ExDesignBttn_Checked(object sender, RoutedEventArgs e)
        {
            StaticEvents.StageChangeEvent?.Invoke(Stage.ExDesign);
        }
        private void SolidMethodBttn_Checked(object sender, RoutedEventArgs e)
        {
            StaticEvents.StageChangeEvent?.Invoke(Stage.SoilMethod);
        }
        private void AnchorBttn_Checked(object sender, RoutedEventArgs e)
        {
            StaticEvents.StageChangeEvent?.Invoke(Stage.Anchors);
        }
        
        private void StrutBttn_Checked(object sender, RoutedEventArgs e)
        {
            StaticEvents.StageChangeEvent?.Invoke(Stage.Struts);
        }
        private void SurchargeBttn_Checked(object sender, RoutedEventArgs e)
        {
            StaticEvents.StageChangeEvent?.Invoke(Stage.Surcharge);
        }
        private void SystemLoadBttn_Checked(object sender, RoutedEventArgs e)
        {
            StaticEvents.StageChangeEvent?.Invoke(Stage.SystemLoad);
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
            UnitCombobox.SelectedIndex = StaticVariables.viewModel.UnitIndex;
            Concrete.ConcreteDataReader();
            Rebar.RebarDataReader();
            Steel.SteelDataReader();
            Pile.pileDiameterReader();
            Sheet.SheetDataReader();  
            Wire.WireDataReader();  
            SoilTexture.SoilTextureLibraryDataReader();
            SoilLibrary.SoilLibraryDataReader();
            StaticVariables.view3DPage.Refreshview();
            StaticEvents.SetStageEvent?.Invoke(StaticVariables.viewModel.stage);
        }

        
    }
}
