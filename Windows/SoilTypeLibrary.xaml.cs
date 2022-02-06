using ExDesign.Datas;
using ExDesign.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using ExDesign.Pages.Inputs;

namespace ExDesign.Windows
{
    /// <summary>
    /// SoilTypeLibrary.xaml etkileşim mantığı
    /// </summary>
    public partial class SoilTypeLibrary : Window
    {
        private ObservableCollection<SoilData> tempSoilDataList = new ObservableCollection<SoilData>();
        public char separator = ',';
        SoilMethodPage methodPage;
        
        public SoilTypeLibrary()
        {
            InitializeComponent();
        }
        private SoilData selectedSoilData = new SoilData();
              
        public void SetMethodPage( SoilMethodPage soilMethodPage)
        {
            methodPage = soilMethodPage;
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
            if (tempSoilDataList.Count > 0)
            {
                UserSoilList.SelectedItem = tempSoilDataList[0];
            }
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList;
            LibrarySoilList.DisplayMemberPath = "Name";
            LibrarySoilList.SelectionMode = SelectionMode.Single;
            if(tempSoilDataList.Count<=0) tempSoilDataList.Add(new SoilData() { isDefault = false, Name = "new soil", isSoilTexture = true, SoilColor = Colors.AliceBlue, SoilTexture = SoilTexture.tempSoilTextureDataList[0] }); ;
            UserSoilList.SelectedIndex = 0;
        }

        private void UserSoilList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserSoilList.SelectedIndex < 0) return;
            selectedSoilData = ((SoilData)UserSoilList.SelectedItem);
            SelectionChanged();
        }
        private void LibrarySoilList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LibrarySoilList.SelectedIndex < 0) return;
            selectedSoilData = ((SoilData)LibrarySoilList.SelectedItem);
            SelectionChanged();
        }

        private void SelectionChanged()
        {

            if (selectedSoilData == null) return;
            soilname.IsEnabled = !(selectedSoilData).isDefault;
            naturalUnitWeight.IsEnabled = !(selectedSoilData).isDefault;
            saturatedUnitWeight.IsEnabled = !(selectedSoilData).isDefault;
            soilFrictionAngle.IsEnabled = !(selectedSoilData).isDefault;
            effectiveCohesion.IsEnabled = !(selectedSoilData).isDefault;
            wallsoilfrictionangle.IsEnabled = !(selectedSoilData).isDefault;
            undrainedShearStrength.IsEnabled = !(selectedSoilData).isDefault;
            wallSoilAdhesion.IsEnabled = !(selectedSoilData).isDefault;
            poissonRatio.IsEnabled = !(selectedSoilData).isDefault;
            K0.IsEnabled = !(selectedSoilData).isDefault;
            ocr.IsEnabled = !(selectedSoilData).isDefault;
            OedometricModulus.IsEnabled = !(selectedSoilData).isDefault;
            CohesionFactor.IsEnabled = !(selectedSoilData).isDefault;
            YoungModulus.IsEnabled = !(selectedSoilData).isDefault;

            soilstressstate_combobox.SelectedIndex = (selectedSoilData).SoilStressStateIndex;
            soilstate_combobox.SelectedIndex = (selectedSoilData).SoilStateKoIndex;
            TextureDefination();
            soilname.Text = (selectedSoilData).Name;
            naturalUnitWeight.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDensity((selectedSoilData).NaturalUnitWeight));
            saturatedUnitWeight.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDensity((selectedSoilData).SaturatedUnitWeight));
            soilFrictionAngle.Text = WpfUtils.ChangeDecimalOptions((selectedSoilData).SoilFrictionAngle);
            effectiveCohesion.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress((selectedSoilData).EffectiveCohesion));
            wallsoilfrictionangle.Text = WpfUtils.ChangeDecimalOptions((selectedSoilData).WallSoilFrictionAngle);
            undrainedShearStrength.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress((selectedSoilData).UndrainedShearStrength));
            wallSoilAdhesion.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress((selectedSoilData).WallSoilAdhesion));
            poissonRatio.Text = WpfUtils.ChangeDecimalOptions((selectedSoilData).PoissonRatio);
            K0.Text = WpfUtils.ChangeDecimalOptions((selectedSoilData).K0);
            ocr.Text = WpfUtils.ChangeDecimalOptions((selectedSoilData).Ocr);
            OedometricModulus.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress((selectedSoilData).OedometricModulus));
            CohesionFactor.Text = WpfUtils.ChangeDecimalOptions((selectedSoilData).CohesionFactor);
            YoungModulus.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress((selectedSoilData).YoungModulus));

        }

        private void TextureDefination()
        {
            if(UserSoilList.SelectedItem==null) return;
            if ((selectedSoilData).isSoilTexture)
            {
                useTexture.IsChecked = true;
                TextureImage.Source = new BitmapImage((selectedSoilData).SoilTexture.TextureUri);
                TextureImage.Stretch = Stretch.Fill;
            }
            else
            {
                useColor.IsChecked = true;
                TextureImage.Source = new DrawingImage(
                    new GeometryDrawing(
                        new SolidColorBrush((selectedSoilData).SoilColor),
                        new Pen(new SolidColorBrush((selectedSoilData).SoilColor),
                        0),
                        new RectangleGeometry(
                            new Rect(
                                new Size(50, 50)
                                )
                            )
                        )
                    );
            }
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

        private void useTexture_Checked(object sender, RoutedEventArgs e)
        {
            if(selectedSoilData.isDefault) return;
            if (useTexture.IsChecked == true) selectedSoilData.isSoilTexture = true;
            TextureDefination();
        }

        private void useColor_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedSoilData.isDefault) return;
            if (useColor.IsChecked == true) selectedSoilData.isSoilTexture = false;
            TextureDefination();
        }

        private void Pickbttn_Click(object sender, RoutedEventArgs e)
        {
            if(selectedSoilData.isDefault) return;
            if(selectedSoilData.isSoilTexture)
            {
                MessageBox.Show("not implemented yet!");
            }
            else
            {
                System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedSoilData.SoilColor = Color.FromArgb(50, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                }
            }
            TextureDefination();
            
        }
        private void soilname_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            selectedSoilData.Name = textBox.Text;
            UserSoilList.Items.Refresh();
        }
       

        private void naturalUnitWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.NaturalUnitWeight = WpfUtils.GetValueDensity(result);
            }
        }

        private void naturalUnitWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void naturalUnitWeight_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void saturatedUnitWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.SaturatedUnitWeight = WpfUtils.GetValueDensity(result);
            }
        }

        private void soilFrictionAngle_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.SoilFrictionAngle = result;
            }
        }

        private void effectiveCohesion_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.EffectiveCohesion = WpfUtils.GetValueStress(result);
            }
        }

        private void wallsoilfrictionangle_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.WallSoilFrictionAngle = result;
            }
        }

        private void undrainedShearStrength_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.UndrainedShearStrength = WpfUtils.GetValueStress(result);
            }
        }

        private void wallSoilAdhesion_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.WallSoilAdhesion = WpfUtils.GetValueStress(result);
            }
        }

        private void poissonRatio_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.PoissonRatio = result;
            }
        }

        private void K0_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.K0 = result;
            }
        }

        private void ocr_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.Ocr = result;
            }
        }

        private void OedometricModulus_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.OedometricModulus = WpfUtils.GetValueStress(result);
            }
        }

        private void CohesionFactor_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.CohesionFactor = result;
            }
        }

        private void YoungModulus_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                selectedSoilData.YoungModulus = WpfUtils.GetValueStress(result);
            }
        }

        private void addnew_button_Click(object sender, RoutedEventArgs e)
        {
            tempSoilDataList.Add(new SoilData() { isDefault = false, Name = "new soil",  isSoilTexture = true, SoilColor = Colors.AliceBlue ,SoilTexture=SoilTexture.tempSoilTextureDataList[0]}); ;
            UserSoilList.SelectedIndex = tempSoilDataList.Count - 1;
            selectedSoilData = tempSoilDataList[tempSoilDataList.Count-1];

        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (!selectedSoilData.isDefault)
            {
                tempSoilDataList.Remove(selectedSoilData);
                UserSoilList.SelectedIndex = 0;
            }
        }

        private void AddCopyFromLibrary_button_Click(object sender, RoutedEventArgs e)
        {
            if (!selectedSoilData.isDefault) return;
            SoilData soil1 = new SoilData()
            {
                isDefault = false,
                Name = selectedSoilData.Name,
                NaturalUnitWeight = selectedSoilData.NaturalUnitWeight,
                SaturatedUnitWeight = selectedSoilData.SaturatedUnitWeight,
                SoilStressStateIndex = selectedSoilData.SoilStressStateIndex,
                SoilStateKoIndex = selectedSoilData.SoilStateKoIndex,
                SoilFrictionAngle = selectedSoilData.SoilFrictionAngle,
                EffectiveCohesion = selectedSoilData.EffectiveCohesion,
                UndrainedShearStrength = selectedSoilData.UndrainedShearStrength,
                WallSoilFrictionAngle = selectedSoilData.WallSoilFrictionAngle,
                WallSoilAdhesion = selectedSoilData.WallSoilAdhesion,
                PoissonRatio = selectedSoilData.PoissonRatio,
                K0 = selectedSoilData.K0,
                Ocr = selectedSoilData.Ocr,
                OedometricModulus = selectedSoilData.OedometricModulus,
                CohesionFactor = selectedSoilData.CohesionFactor,
                YoungModulus = selectedSoilData.YoungModulus,
                isSoilTexture = selectedSoilData.isSoilTexture,
                SoilColor = selectedSoilData.SoilColor,
                SoilTexture = selectedSoilData.SoilTexture,
            };
            tempSoilDataList.Add(soil1);
            UserSoilList.SelectedItem = soil1;
        }

        private void save_close_button_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            this.Close();
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SaveChanges()
        {            
           StaticVariables.viewModel.soilDatas = tempSoilDataList;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (methodPage != null) methodPage.LayerGridInitialize();
        }
    }
}
