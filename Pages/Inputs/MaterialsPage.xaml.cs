using ExDesign.Scripts;
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

namespace ExDesign.Pages.Inputs
{
    /// <summary>
    /// MaterialsPage.xaml etkileşim mantığı
    /// </summary>
    public partial class MaterialsPage : Page
    {
        public char separator = ',';
        public MaterialsPage()
        {
            InitializeComponent();
        }

        public void Set3dView(Views.View3dPage view)
        {
            view3d_main.Content = StaticVariables.view3DPage;
        }
        private void UnitChange()
        {
            //concretewall_height_unit.Content = StaticVariables.dimensionUnit;
            //concretewall_thickness_unit.Content = StaticVariables.dimensionUnit;
            //pilewall_height_unit.Content = StaticVariables.dimensionUnit;
            //pile_diameter_unit.Content = StaticVariables.dimensionUnit;
            //pile_space_unit.Content = StaticVariables.dimensionUnit;
            //beam_height_unit.Content = StaticVariables.dimensionUnit;
            //beam_width_unit.Content = StaticVariables.dimensionUnit;

            //sheetpilewall_height_unit.Content = StaticVariables.dimensionUnit;

            //concretewall_height.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()).ToString();
            //concretewall_thickness.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetWallThickness()).ToString();
            //pilewall_height.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()).ToString();
            //pile_diameter.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetWallThickness()).ToString();
            //pile_space.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetPileSpace()).ToString();
            //beam_height.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetCapBeamH()).ToString();
            //beam_width.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetCapBeamB()).ToString();
            //sheetpilewall_height.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()).ToString();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            UnitChange();
            StaticEvents.UnitChangeEvent += UnitChange;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
            StaticEvents.UnitChangeEvent -= UnitChange;
        }
    }
}
