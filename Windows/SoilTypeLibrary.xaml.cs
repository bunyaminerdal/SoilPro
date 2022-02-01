using ExDesign.Datas;
using ExDesign.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace ExDesign.Windows
{
    /// <summary>
    /// SoilTypeLibrary.xaml etkileşim mantığı
    /// </summary>
    public partial class SoilTypeLibrary : Window
    {
        private ObservableCollection<SoilData> tempSoilDataList = new ObservableCollection<SoilData>();
        public char separator = ',';
        public SoilTypeLibrary()
        {
            InitializeComponent();
        }

        private void soilname_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            wallsoiladhesionPanel.Visibility = Visibility.Collapsed;
            undrainedshearstrengthPanel.Visibility = Visibility.Collapsed;
            effectivecohesionPanel.Visibility = Visibility.Visible;
            soilfrictionanglePanel.Visibility = Visibility.Visible;
            wallsoilfrictionanglePanel.Visibility = Visibility.Visible;
            ocrPanel.Visibility = Visibility.Collapsed;
            koPanel.Visibility = Visibility.Collapsed;

            foreach (var soil in StaticVariables.viewModel.soilDatas)
            {
                tempSoilDataList.Add(soil);
            }
            UserSoilList.ItemsSource = tempSoilDataList;
            UserSoilList.DisplayMemberPath = "Name";
            UserSoilList.SelectionMode = SelectionMode.Single;
            naturalUnitWeight_unit.Content = StaticVariables.DensityUnit;
            saturatedUnitWeight_unit.Content = StaticVariables.DensityUnit;
            effectiveCohesion_unit.Content = StaticVariables.StressUnit;
            undrainedShearStrength_unit.Content = StaticVariables.StressUnit;
            wallSoilAdhesion_unit.Content = StaticVariables.StressUnit;
            eoed_unit.Content = StaticVariables.StressUnit;
            esprime_unit.Content = StaticVariables.StressUnit;
        }

        private void UserSoilList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            soilname.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            naturalUnitWeight.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            saturatedUnitWeight.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            soilFrictionAngle.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            effectiveCohesion.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            wallsoilfrictionangle.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            undrainedShearStrength.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            wallSoilAdhesion.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            poissonRatio.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            ko.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            ocr.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            eoed.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            ap.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;
            esprime.IsEnabled = !((SoilData)UserSoilList.SelectedItem).isDefault;

            //soilname.Text = ((SoilData)UserSoilList.SelectedItem).Name;
            //naturalUnitWeight.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(((SoilData)UserSoilList.SelectedItem).Height));
            //saturatedUnitWeight.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(((SoilData)UserSoilList.SelectedItem).Length));
            //soilFrictionAngle.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(((SoilData)UserSoilList.SelectedItem).Thickness));
            //effectiveCohesion.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetArea(((SoilData)UserSoilList.SelectedItem).Area));
            //wallsoilfrictionangle.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetInertia(((SoilData)UserSoilList.SelectedItem).Inertia));
            //undrainedShearStrength.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SoilData)UserSoilList.SelectedItem).Wely));
            //wallSoilAdhesion.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SoilData)UserSoilList.SelectedItem).Wply));
            //poissonRatio.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SoilData)UserSoilList.SelectedItem).Wply));
            //ko.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SoilData)UserSoilList.SelectedItem).Wply));
            //ocr.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SoilData)UserSoilList.SelectedItem).Wply));
            //eoed.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SoilData)UserSoilList.SelectedItem).Wply));
            //ap.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SoilData)UserSoilList.SelectedItem).Wply));
            //esprime.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SoilData)UserSoilList.SelectedItem).Wply));

        }

        private void soilstressstate_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wallsoiladhesionPanel == null) return;
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    wallsoiladhesionPanel.Visibility = Visibility.Collapsed;
                    undrainedshearstrengthPanel.Visibility = Visibility.Collapsed;
                    effectivecohesionPanel.Visibility = Visibility.Visible;
                    soilfrictionanglePanel.Visibility = Visibility.Visible;
                    wallsoilfrictionanglePanel.Visibility = Visibility.Visible;
                    break;
                case 1:
                    wallsoiladhesionPanel.Visibility = Visibility.Visible;
                    undrainedshearstrengthPanel.Visibility = Visibility.Visible;
                    effectivecohesionPanel.Visibility = Visibility.Collapsed;
                    soilfrictionanglePanel.Visibility = Visibility.Collapsed;
                    wallsoilfrictionanglePanel.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        

        private void soilstate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(koPanel==null) return;
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    ocrPanel.Visibility = Visibility.Collapsed;
                    koPanel.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    ocrPanel.Visibility = Visibility.Collapsed;
                    koPanel.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    ocrPanel.Visibility = Visibility.Visible;
                    koPanel.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    ocrPanel.Visibility = Visibility.Collapsed;
                    koPanel.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
