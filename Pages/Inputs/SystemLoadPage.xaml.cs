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
    }
}
