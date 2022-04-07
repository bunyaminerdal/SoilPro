using ExDesign.Scripts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// SystemLoadPage.xaml etkileşim mantığı
    /// </summary>
    public partial class SystemLoadPage : Page
    {
        public char separator = ',';

        public SystemLoadPage()
        {
            InitializeComponent();
        }
        private void UnitChange()
        {
            depthafteranchorstruttextbox_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            depthafteranchorstruttextbox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.depthAfterAnchorStrut));
            sds_textbox.Text = WpfUtils.ChangeDecimalOptions(StaticVariables.viewModel.SDSValue);
            kh_textbox.Text = WpfUtils.ChangeDecimalOptions(StaticVariables.viewModel.khValue);
            kv_textbox.Text = WpfUtils.ChangeDecimalOptions(StaticVariables.viewModel.kvValue);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            if (StaticVariables.viewModel.isEarthQuakeDesign == true)
            {
                earthquakeCheckbox.IsChecked = true;
                sdscheckbox.IsEnabled = true;
                if (StaticVariables.viewModel.isSDSValue == true)
                {
                    sdscheckbox.IsChecked = true;
                    sds_textbox.IsEnabled = true;
                    kh_textbox.IsEnabled = false;
                    kv_textbox.IsEnabled = false;
                }
                else
                {
                    sdscheckbox.IsChecked = false;
                    sds_textbox.IsEnabled = false;
                    kh_textbox.IsEnabled = true;
                    kv_textbox.IsEnabled = true;
                }
            }
            else
            {
                earthquakeCheckbox.IsChecked = false;
                sdscheckbox.IsEnabled = false ;
                sds_textbox.IsEnabled = false;
                kh_textbox.IsEnabled = false;
                kv_textbox.IsEnabled = false;
            }
            if (StaticVariables.viewModel.isStageConstruction == true)
            {
                stageconstructionCheckbox.IsChecked= true;
                depthafteranchorstruttextbox.IsEnabled = true;
            }
            else
            {
                stageconstructionCheckbox.IsChecked = false;
                depthafteranchorstruttextbox.IsEnabled = false;
            }
            switch (WpfUtils.GetAnalysMethodType(StaticVariables.viewModel.analysMethodIndex))
            {
                case AnalysMethod.ClassicLoading:
                    analysmethodCombobox.SelectedIndex = 0;
                    classicloadingInfo.Visibility = Visibility.Visible;
                    break;
                case AnalysMethod.EquivalentLinear:
                    analysmethodCombobox.SelectedIndex = 1;
                    equivalentLinearInfo.Visibility = Visibility.Visible;
                    break;
                case AnalysMethod.FHWA:
                    analysmethodCombobox.SelectedIndex = 2;
                    FHVAinfo.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.activeDrainedCoefficientIndex))
            {
                case DrainedTheories.TBDY:
                    ActiveDrained_combobox.SelectedIndex = 0;
                    break;
                case DrainedTheories.MazindraniTheory:
                    ActiveDrained_combobox.SelectedIndex = 1;
                    break;
                case DrainedTheories.TheColoumbTheory:
                    ActiveDrained_combobox.SelectedIndex = 2;
                    break;
                case DrainedTheories.RankineTheory:
                    ActiveDrained_combobox.SelectedIndex = 3;
                    break;
                default:
                    ActiveDrained_combobox.SelectedIndex = 0;
                    break;
            }
            switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.passiveDrainedCoefficientIndex))
            {
                case DrainedTheories.TBDY:
                    PassiveDrained_combobox.SelectedIndex = 0;
                    break;
                case DrainedTheories.MazindraniTheory:
                    PassiveDrained_combobox.SelectedIndex = 1;
                    break;
                case DrainedTheories.TheColoumbTheory:
                    PassiveDrained_combobox.SelectedIndex = 2;
                    break;
                case DrainedTheories.RankineTheory:
                    PassiveDrained_combobox.SelectedIndex = 3;
                    break;
                default:
                    PassiveDrained_combobox.SelectedIndex = 0;
                    break;
            }
            switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.activeUnDrainedCoefficientIndex))
            {
                case UnDrainedTheories.TBDY:
                    ActiveUndrained_combobox.SelectedIndex = 0;
                    break;
                case UnDrainedTheories.MazindraniTheory:
                    ActiveUndrained_combobox.SelectedIndex = 1;
                    break;
                case UnDrainedTheories.TheColoumbTheory:
                    ActiveUndrained_combobox.SelectedIndex = 2;
                    break;
                case UnDrainedTheories.RankineTheory:
                    ActiveUndrained_combobox.SelectedIndex = 3;
                    break;
                case UnDrainedTheories.TotalStress:
                    ActiveUndrained_combobox.SelectedIndex = 4;
                    break;
                default:
                    ActiveUndrained_combobox.SelectedIndex = 0;
                    break;
            }
            switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.passiveUnDrainedCoefficientIndex))
            {
                case UnDrainedTheories.TBDY:
                    PassiveUndrained_combobox.SelectedIndex = 0;
                    break;
                case UnDrainedTheories.MazindraniTheory:
                    PassiveUndrained_combobox.SelectedIndex = 1;
                    break;
                case UnDrainedTheories.TheColoumbTheory:
                    PassiveUndrained_combobox.SelectedIndex = 2;
                    break;
                case UnDrainedTheories.RankineTheory:
                    PassiveUndrained_combobox.SelectedIndex = 3;
                    break;
                case UnDrainedTheories.TotalStress:
                    PassiveUndrained_combobox.SelectedIndex = 4;
                    break;
                default:
                    PassiveUndrained_combobox.SelectedIndex = 0;
                    break;
            }
            StaticEvents.UnitChangeEvent += UnitChange;
            UnitChange();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
            StaticEvents.UnitChangeEvent -= UnitChange;
        }

        private void earthquakeCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.isEarthQuakeDesign = false;
            sdscheckbox.IsEnabled = false;
            sds_textbox.IsEnabled = false;
            kh_textbox.IsEnabled = false;
            kv_textbox.IsEnabled = false;
        }

        private void earthquakeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.isEarthQuakeDesign = true;
            sdscheckbox.IsEnabled = true;
            if (StaticVariables.viewModel.isSDSValue == true)
            {
                sdscheckbox.IsChecked = true;
                sds_textbox.IsEnabled = true;
                kh_textbox.IsEnabled = false;
                kv_textbox.IsEnabled = false;
            }
            else
            {
                sdscheckbox.IsChecked = false;
                sds_textbox.IsEnabled = false;
                kh_textbox.IsEnabled = true;
                kv_textbox.IsEnabled = true;
            }
        }

        private void sdscheckbox_Checked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.isSDSValue = true;
            StaticVariables.viewModel.khValue = StaticVariables.viewModel.SDSValue * 0.4;
            kh_textbox.Text = WpfUtils.ChangeDecimalOptions(StaticVariables.viewModel.khValue);
            StaticVariables.viewModel.kvValue = StaticVariables.viewModel.khValue * 0.5;
            kv_textbox.Text = WpfUtils.ChangeDecimalOptions(StaticVariables.viewModel.kvValue);
            sds_textbox.IsEnabled = true;
            kh_textbox.IsEnabled = false;
            kv_textbox.IsEnabled = false;
        }

        private void sdscheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.isSDSValue = false;
            sds_textbox.IsEnabled = false;
            kh_textbox.IsEnabled = true;
            kv_textbox.IsEnabled = true;
        }

        private void analysmethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (analysmethodCombobox.SelectedIndex >= 0) StaticVariables.viewModel.analysMethodIndex = analysmethodCombobox.SelectedIndex;
            switch (WpfUtils.GetAnalysMethodType(StaticVariables.viewModel.analysMethodIndex))
            {
                case AnalysMethod.ClassicLoading:
                    classicloadingInfo.Visibility = Visibility.Visible;
                    equivalentLinearInfo.Visibility = Visibility.Collapsed;
                    FHVAinfo.Visibility = Visibility.Collapsed;
                    break;
                case AnalysMethod.EquivalentLinear:
                    classicloadingInfo.Visibility = Visibility.Collapsed;
                    equivalentLinearInfo.Visibility = Visibility.Visible;
                    FHVAinfo.Visibility = Visibility.Collapsed;
                    break;
                case AnalysMethod.FHWA:
                    classicloadingInfo.Visibility = Visibility.Collapsed;
                    equivalentLinearInfo.Visibility = Visibility.Collapsed;
                    FHVAinfo.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void stageconstructionCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.isStageConstruction = true;
            depthafteranchorstruttextbox.IsEnabled = true;
        }

        private void stageconstructionCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.isStageConstruction = false;
            depthafteranchorstruttextbox.IsEnabled = false;
        }

        private void depthafteranchorstruttextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (separator == ',')
            {
                Regex regex = new Regex("^[,][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
                e.Handled = !regex.IsMatch(((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text));
            }
            else
            {
                Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
                e.Handled = !regex.IsMatch(((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text));
            }
        }

        private void depthafteranchorstruttextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                sideview_main.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                sideview_main.Focus();
            }
        }

        private void depthafteranchorstruttextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.DepthAfterControl(WpfUtils.GetValueDimension(result)))
                {
                    StaticVariables.viewModel.depthAfterAnchorStrut = WpfUtils.GetValueDimension(result);
                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.depthAfterAnchorStrut));
                }
            }
        }

        private void sds_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.SDSValue = result;
                if(sdscheckbox.IsChecked == true)
                {
                    StaticVariables.viewModel.khValue = result * 0.4;
                    kh_textbox.Text = WpfUtils.ChangeDecimalOptions(StaticVariables.viewModel.khValue);
                    StaticVariables.viewModel.kvValue = StaticVariables.viewModel.khValue * 0.5;
                    kv_textbox.Text = WpfUtils.ChangeDecimalOptions(StaticVariables.viewModel.kvValue);
                }
            }
        }

        private void kh_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.khValue = result;
            }
        }

        private void kv_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.kvValue = result;
            }
        }

        private void changeTheoryCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ActiveDrained_combobox.IsEnabled = true;
            ActiveUndrained_combobox.IsEnabled=true;
            PassiveDrained_combobox.IsEnabled = true;
            PassiveUndrained_combobox.IsEnabled = true;
        }

        private void changeTheoryCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            ActiveDrained_combobox.IsEnabled = false;
            ActiveUndrained_combobox.IsEnabled = false;
            PassiveDrained_combobox.IsEnabled = false;
            PassiveUndrained_combobox.IsEnabled = false;
        }

        private void ActiveDrained_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         if(ActiveDrained_combobox.SelectedIndex > 0)  StaticVariables.viewModel.activeDrainedCoefficientIndex = ActiveDrained_combobox.SelectedIndex;
        }

        private void ActiveUndrained_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActiveUndrained_combobox.SelectedIndex > 0) StaticVariables.viewModel.activeUnDrainedCoefficientIndex = ActiveUndrained_combobox.SelectedIndex;
        }

        private void PassiveDrained_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PassiveDrained_combobox.SelectedIndex > 0) StaticVariables.viewModel.passiveDrainedCoefficientIndex = PassiveDrained_combobox.SelectedIndex;
        }

        private void PassiveUndrained_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PassiveUndrained_combobox.SelectedIndex > 0) StaticVariables.viewModel.passiveUnDrainedCoefficientIndex = PassiveUndrained_combobox.SelectedIndex;
        }
    }
}
