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
using ExDesign.Datas;

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
            fcktext_unit.Content = StaticVariables.StressUnit;
            fcttext_unit.Content = StaticVariables.StressUnit;
            Etext_unit.Content = StaticVariables.StressUnit;
            Gtext_unit.Content = StaticVariables.StressUnit;


            if(StaticVariables.viewModel.ConcreteIndex >= 0)
            {
                fcktext.Text = WpfUtils.GetStress(Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].fck).ToString();
                fcttext.Text = WpfUtils.GetStress(Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].fct).ToString();
                Etext.Text = WpfUtils.GetStress(Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].E).ToString();
                Gtext.Text = WpfUtils.GetStress(Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].G).ToString();
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            Concrete.GetConcreteDataList(concreteCombobox);
            if (StaticVariables.viewModel.ConcreteIndex > concreteCombobox.Items.Count - 1)
            {
                StaticVariables.viewModel.ConcreteIndex = 0;
            }
            concreteCombobox.SelectedIndex = StaticVariables.viewModel.ConcreteIndex;

            UnitChange();
            StaticEvents.UnitChangeEvent += UnitChange;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
            StaticEvents.UnitChangeEvent -= UnitChange;
        }

        private void concreteWindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.ConcreteWindow concreteWindow = new Windows.ConcreteWindow();
            concreteWindow.SelectConcrete(concreteCombobox);
            concreteWindow.ShowDialog();
        }

        private void concreteCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StaticVariables.viewModel.ConcreteIndex =((ComboBox)sender).SelectedIndex;
            UnitChange();
        }
    }
}
