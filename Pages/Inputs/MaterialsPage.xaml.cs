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
        public MaterialsPage()
        {
            InitializeComponent();
        }

        public void SetViews()
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
        }
        private void UnitChange()
        {
            fcktext_unit.Content = StaticVariables.StressUnit;
            fcttext_unit.Content = StaticVariables.StressUnit;
            Etext_unit.Content = StaticVariables.StressUnit;
            Gtext_unit.Content = StaticVariables.StressUnit;
            fyktext_unit.Content = StaticVariables.StressUnit;
            rebarEtext_unit.Content = StaticVariables.StressUnit;
            fytext_unit.Content = StaticVariables.StressUnit;
            steelEtext_unit.Content = StaticVariables.StressUnit;

            if(StaticVariables.viewModel.ConcreteIndex >= 0 && Concrete.ConcreteDataList.Count>0)
            {
                fcktext.Text =WpfUtils.ChangeDecimalOptions( WpfUtils.GetStress(Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].fck));
                fcttext.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].fct));
                Etext.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].E));
                Gtext.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].G));
            }
            if(StaticVariables.viewModel.RebarIndex >= 0 && Rebar.RebarDataList.Count>0)
            {
                fyktext.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(Rebar.RebarDataList[StaticVariables.viewModel.RebarIndex].fyk));
                rebarEtext.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(Rebar.RebarDataList[StaticVariables.viewModel.RebarIndex].E));
            }
            if (StaticVariables.viewModel.SteelIndex >= 0 && Steel.SteelDataList.Count>0)
            {
                fytext.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(Steel.SteelDataList[StaticVariables.viewModel.SteelIndex].fy));
                steelEtext.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(Steel.SteelDataList[StaticVariables.viewModel.SteelIndex].E));
            }
            StaticVariables.viewModel.ChangeWallProperties();
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
            Rebar.GetRebarDataList(rebarCombobox);
            if (StaticVariables.viewModel.RebarIndex > rebarCombobox.Items.Count - 1)
            {
                StaticVariables.viewModel.RebarIndex = 0;
            }
            rebarCombobox.SelectedIndex = StaticVariables.viewModel.RebarIndex;

            Steel.GetSteelDataList(steelCombobox);
            if (StaticVariables.viewModel.SteelIndex > steelCombobox.Items.Count - 1)
            {
                StaticVariables.viewModel.SteelIndex = 0;
            }
            steelCombobox.SelectedIndex = StaticVariables.viewModel.SteelIndex;

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
            StaticVariables.viewModel.ConcreteIndex = ((ComboBox)sender).SelectedIndex>=0? ((ComboBox)sender).SelectedIndex:StaticVariables.viewModel.ConcreteIndex;
            UnitChange();
        }

        private void rebarCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StaticVariables.viewModel.RebarIndex =((ComboBox)sender).SelectedIndex>=0? ((ComboBox)sender).SelectedIndex:StaticVariables.viewModel.RebarIndex;
            UnitChange();
        }

        private void rebarWindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.RebarWindow rebarWindow = new Windows.RebarWindow();
            rebarWindow.SelectRebar(rebarCombobox);
            rebarWindow.ShowDialog();
        }

        private void steelCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StaticVariables.viewModel.SteelIndex = ((ComboBox)sender).SelectedIndex>=0? ((ComboBox)sender).SelectedIndex:StaticVariables.viewModel.SteelIndex;
            UnitChange();
        }

        private void steelWindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.SteelWindow steelWindow = new Windows.SteelWindow();
            steelWindow.SelectSteel(steelCombobox);
            steelWindow.ShowDialog();
        }
    }
}
