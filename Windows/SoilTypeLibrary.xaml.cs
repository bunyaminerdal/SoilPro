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
        private ObservableCollection<SoilData> tempSavedSoilDataList = new ObservableCollection<SoilData>();
        public char separator = ',';
        SoilMethodPage methodPage;
        public SoilData selectedSoilData = new SoilData();
        SoilRockTypes SoilRockType = SoilRockTypes.None;
        SoilTypes SoilType = SoilTypes.None;
        SiltTypes SiltType = SiltTypes.None;
        SoilDenseTypes SoilDenseType = SoilDenseTypes.None;
        SoilStiffTypes SoilStiffType = SoilStiffTypes.None;
        RockTypes RockType = RockTypes.None;
        RockSubTypes RockSubType = RockSubTypes.None;

        
        public void SetTexture(SoilTextureData soilTextureData)
        {
            selectedSoilData.SoilTexture = soilTextureData;
            SelectionChanged();
        }
        public SoilTypeLibrary()
        {
            InitializeComponent();
        }
              
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
                tempSoilDataList.Add((SoilData)soil.Clone());
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
            if (tempSoilDataList.Count <= 0)  tempSoilDataList.Add(new SoilData() { ID = Guid.NewGuid(), isDefault = false, Name = "new soil", isSoilTexture = false, SoilColor = Colors.AliceBlue, SoilTexture = SoilTexture.tempSoilTextureDataList[0] }); ;
                        
            UserSoilList.SelectedIndex = 0;

            foreach (var soil in SoilLibrary.SavedSoilDataList)
            {
                tempSavedSoilDataList.Add((SoilData)soil.Clone());
            }
            SavedSoilList.ItemsSource = tempSavedSoilDataList;
            SavedSoilList.DisplayMemberPath = "Name";
            SavedSoilList.SelectionMode = SelectionMode.Single;
            ListFocused(2);
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
            soilstate_combobox.IsEnabled = !(selectedSoilData).isDefault;
            soilstressstate_combobox.IsEnabled= !(selectedSoilData).isDefault;
            useColor.IsEnabled = !(selectedSoilData).isDefault;
            useTexture.IsEnabled = !(selectedSoilData).isDefault;
            Pickbttn.IsEnabled = !(selectedSoilData).isDefault;

            soilstressstate_combobox.SelectedIndex = (selectedSoilData).SoilStressStateIndex;
            soilstate_combobox.SelectedIndex = (selectedSoilData).SoilStateKoIndex;
            TextureDefinition();
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

        private void TextureDefinition()
        {
            if(selectedSoilData.SoilTexture==null) return;
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
                TextureImage.Stretch = Stretch.Fill;
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
            if(selectedSoilData==null) return;
            if (useTexture.IsChecked == true) selectedSoilData.isSoilTexture = true;
            TextureDefinition();
        }

        private void useColor_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedSoilData==null) return;
            if (useColor.IsChecked == true) selectedSoilData.isSoilTexture = false;
            TextureDefinition();
        }

        private void Pickbttn_Click(object sender, RoutedEventArgs e)
        {
            if(selectedSoilData.isDefault) return;
            if(selectedSoilData.isSoilTexture)
            {
                TextureWindow textureWindow = new TextureWindow();
                textureWindow.SetLibrary(this);
                textureWindow.ShowDialog();

            }
            else
            {
                System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedSoilData.SoilColor = Color.FromArgb(100, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                }
            }
            TextureDefinition();
            
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
            tempSoilDataList.Add(new SoilData() {ID=Guid.NewGuid(), isDefault = false, Name = "new soil",  isSoilTexture = false, SoilColor = Colors.AliceBlue ,SoilTexture=SoilTexture.tempSoilTextureDataList[0]}); ;
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
                ID = Guid.NewGuid(),
                isDefault = false,
                isUserDefined = true,
                Name = selectedSoilData.Name,
                SoilRockType = selectedSoilData.SoilRockType,
                SoilType = selectedSoilData.SoilType,
                SoilDenseType = selectedSoilData.SoilDenseType,
                SoilStiffType = selectedSoilData.SoilStiffType,
                SiltType = selectedSoilData.SiltType,
                RockSubType = selectedSoilData.RockSubType,
                RockType = selectedSoilData.RockType,
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
            ListFocused(2);
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
            SoilLibrary.SavedSoilDataList.Clear();
            foreach (var soil in tempSavedSoilDataList)
            {

                SoilLibrary.SavedSoilDataList.Add(soil);
            }
            SoilLibrary.SoilSave();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (methodPage != null) methodPage.LayerGridInitialize();
        }

        private void UserSoilList_GotFocus(object sender, RoutedEventArgs e)
        {
            if(UserSoilList.SelectedItem != null) selectedSoilData = (SoilData)UserSoilList.SelectedItem;
            ListFocused(2);
            SelectionChanged();
        }

        private void LibrarySoilList_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LibrarySoilList.SelectedItem != null) selectedSoilData = (SoilData)LibrarySoilList.SelectedItem;
            ListFocused(0);
            SelectionChanged();
        }

        private void soil_radiobutton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                
                if ( radioButton.IsChecked == true)
                {
                    radioButton.IsChecked = false;
                    FilterSystem(null, radioButton.Name);
                    e.Handled = true;
                }
               
            }
        }
        

        private void FilterSystem(bool? isChecked, string buttonname)
        {
            switch (buttonname)
            {
                case "soil_radiobutton":
                    if(isChecked == null)
                    {
                        soilRockNull();
                    }else if(isChecked==true)
                    {
                        soilTrue();
                    }                    
                    break;
                case "rock_radiobutton":
                    if (isChecked == null)
                    {
                        soilRockNull();
                    }
                    else if (isChecked == true)
                    {
                        rockTrue();
                    }
                    break;
                case "gravel_radiobutton":
                    if (isChecked == null)
                    {
                        gravelsandclaysiltNull();
                    }
                    else if (isChecked == true)
                    {
                        gravelTrue();
                    }
                    break;
                case "sand_radiobutton":
                    if (isChecked == null)
                    {
                        gravelsandclaysiltNull();
                    }
                    else if (isChecked == true)
                    {
                        sandTrue();
                    }
                    break;
                case "silt_radiobutton":
                    if (isChecked == null)
                    {
                        gravelsandclaysiltNull();
                    }
                    else if (isChecked == true)
                    {
                        siltTrue();
                    }
                    break;
                case "clay_radiobutton":
                    if (isChecked == null)
                    {
                        gravelsandclaysiltNull();
                    }
                    else if (isChecked == true)
                    {
                        clayTrue();
                    }
                    break;
                case "nonplastic_radiobutton":
                    if (isChecked == null)
                    {
                        plasticNull();
                    }
                    else if (isChecked == true)
                    {
                        nonplasticTrue();
                    }
                    break;
                case "plastic_radiobutton":
                    if (isChecked == null)
                    {
                        plasticNull();
                    }
                    else if (isChecked == true)
                    {
                        plasticTrue();
                    }
                    break;
                case "volcanic_radiobutton":
                    if (isChecked == null)
                    {
                        volcanicNull();
                    }
                    else if (isChecked == true)
                    {
                        volcanicTrue();
                    }
                    break;
                case "metamorphic_radiobutton":
                    if (isChecked == null)
                    {
                        volcanicNull();
                    }
                    else if (isChecked == true)
                    {
                        metamorphicTrue();
                    }
                    break;
                case "sedimantory_radiobutton":
                    if (isChecked == null)
                    {
                        volcanicNull();
                    }
                    else if (isChecked == true)
                    {
                        sedimantoryTrue();
                    }
                    break;
                case "loose_radiobutton":
                    if (isChecked == null)
                    {
                        denseNull();
                    }
                    else if (isChecked == true)
                    {
                        looseTrue();
                    }
                    break;
                case "dense_radiobutton":
                    if (isChecked == null)
                    {
                        denseNull();
                    }
                    else if (isChecked == true)
                    {
                        denseTrue();
                    }
                    break;
                case "mediumdense_radiobutton":
                    if (isChecked == null)
                    {
                        denseNull();
                    }
                    else if (isChecked == true)
                    {
                        mdenseTrue();
                    }
                    break;
                case "soft_radiobutton":
                    if (isChecked == null)
                    {
                        denseNull();
                    }
                    else if (isChecked == true)
                    {
                        softTrue();
                    }
                    break;
                case "mediumstiff_radiobutton":
                    if (isChecked == null)
                    {
                        denseNull();
                    }
                    else if (isChecked == true)
                    {
                        mstiffTrue();
                    }
                    break;
                case "stiff_radiobutton":
                    if (isChecked == null)
                    {
                        denseNull();
                    }
                    else if (isChecked == true)
                    {
                        stiffTrue();
                    }
                    break;
                case "poor_radiobutton":
                    if (isChecked == null)
                    {
                        poorNull();
                    }
                    else if (isChecked == true)
                    {
                        poorTrue();
                    }
                    break;
                case "fair_radiobutton":
                    if (isChecked == null)
                    {
                        poorNull();
                    }
                    else if (isChecked == true)
                    {
                        fairTrue();
                    }
                    break;
                case "excellent_radiobutton":
                    if (isChecked == null)
                    {
                        poorNull();
                    }
                    else if (isChecked == true)
                    {
                        excellentTrue();
                    }
                    break;
                default:
                    break;
            }
            
        }

        private void soil_radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                FilterSystem(radioButton.IsChecked, radioButton.Name);

            }
            
        }
        private void soilTrue()
        {
            soiltype_radiobutton_panel.Visibility = Visibility.Visible;
            rocktype_radiobutton_panel.Visibility = Visibility.Collapsed;
            soildense_radiobutton_panel.Visibility=Visibility.Collapsed;
            silttype_radiobutton_panel.Visibility =Visibility.Collapsed;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Collapsed;
            rocksubtype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll();
            SoilRockType = SoilRockTypes.Soil;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType 
            );
        }
        private void rockTrue()
        {
            rocktype_radiobutton_panel.Visibility = Visibility.Visible;
            soiltype_radiobutton_panel.Visibility = Visibility.Collapsed;
            soildense_radiobutton_panel.Visibility = Visibility.Collapsed;
            silttype_radiobutton_panel.Visibility = Visibility.Collapsed;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Collapsed;
            rocksubtype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll();
            SoilRockType = SoilRockTypes.Rock;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType
            );
        }
        private void soilRockNull()
        {
            rocktype_radiobutton_panel.Visibility = Visibility.Collapsed;
            soiltype_radiobutton_panel.Visibility = Visibility.Collapsed;
            soildense_radiobutton_panel.Visibility = Visibility.Collapsed;
            silttype_radiobutton_panel.Visibility = Visibility.Collapsed;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Collapsed;
            rocksubtype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll();
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList;
            
        }
        private void gravelTrue()
        {
            soildense_radiobutton_panel.Visibility = Visibility.Visible;
            silttype_radiobutton_panel.Visibility = Visibility.Collapsed;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll(gravel_radiobutton);
            SoilType = SoilTypes.Gravel;
            SiltType = SiltTypes.None;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType);
            
        }
        private void sandTrue()
        {
            soildense_radiobutton_panel.Visibility = Visibility.Visible;
            silttype_radiobutton_panel.Visibility = Visibility.Collapsed;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll(sand_radiobutton);
            SoilType = SoilTypes.Sand;
            SiltType = SiltTypes.None;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType);
        }
        private void siltTrue()
        {
            soildense_radiobutton_panel.Visibility = Visibility.Collapsed;
            silttype_radiobutton_panel.Visibility = Visibility.Visible;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll(silt_radiobutton);
            SoilType = SoilTypes.Silt;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType);
        }

        private void clayTrue()
        {
            soildense_radiobutton_panel.Visibility = Visibility.Collapsed;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Visible;
            silttype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll(clay_radiobutton);
            SoilType = SoilTypes.Clay;
            SiltType = SiltTypes.None;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType);

        }
        private void gravelsandclaysiltNull()
        {
            soildense_radiobutton_panel.Visibility = Visibility.Collapsed;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Collapsed;
            silttype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll();
            SoilType = SoilTypes.None;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType );
        }
        
       
        private void plasticTrue()
        {
            soildense_radiobutton_panel.Visibility = Visibility.Collapsed;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Visible;
            DeSelectAll(plastic_radiobutton);
            SiltType = SiltTypes.Plastic;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType && soil.SiltType == SiltType);
        }
        private void nonplasticTrue()
        {
            soildense_radiobutton_panel.Visibility = Visibility.Visible;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll(nonplastic_radiobutton);
            SiltType = SiltTypes.NonPlastic;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType && soil.SiltType == SiltType);
        }
        private void plasticNull()
        {
            soildense_radiobutton_panel.Visibility= Visibility.Collapsed;
            soilstifftype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll();
            silt_radiobutton.IsChecked = true;
            SiltType = SiltTypes.None;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType );
        }
        private void volcanicTrue()
        {
            rocksubtype_radiobutton_panel.Visibility = Visibility.Visible;
            DeSelectAll(volcanic_radiobutton);
            RockType = RockTypes.Volcanic;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.RockType == RockType);
        }
        private void metamorphicTrue()
        {
            rocksubtype_radiobutton_panel.Visibility = Visibility.Visible;
            DeSelectAll(metamorphic_radiobutton);
            RockType = RockTypes.Metamorphic;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.RockType == RockType);
        }
        private void sedimantoryTrue()
        {
            rocksubtype_radiobutton_panel.Visibility = Visibility.Visible;
            DeSelectAll(sedimantory_radiobutton);
            RockType = RockTypes.Sedimantory;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.RockType == RockType);
        }
        private void volcanicNull()
        {
            rocksubtype_radiobutton_panel.Visibility = Visibility.Collapsed;
            DeSelectAll();
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.RockType == RockType);
        }
        private void denseTrue()
        {
            DeselectDense(dense_radiobutton);
            SoilDenseType = SoilDenseTypes.Dense;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType && soil.SiltType == SiltType && soil.SoilDenseType == SoilDenseType);

        }
        private void mdenseTrue()
        {
            DeselectDense(mediumdense_radiobutton);
            SoilDenseType = SoilDenseTypes.MediumDense;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType && soil.SiltType == SiltType && soil.SoilDenseType == SoilDenseType);

        }
        private void looseTrue()
        {
            DeselectDense(loose_radiobutton);
            SoilDenseType = SoilDenseTypes.Loose;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType && soil.SiltType == SiltType && soil.SoilDenseType == SoilDenseType);

        }
        private void softTrue()
        {
            DeselectDense(soft_radiobutton);
            SoilStiffType = SoilStiffTypes.Soft;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType && soil.SiltType == SiltType && soil.SoilStiffType==SoilStiffType);
        }
        private void mstiffTrue()
        {
            DeselectDense(mediumstiff_radiobutton);
            SoilStiffType = SoilStiffTypes.MediumStiff;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType && soil.SiltType == SiltType && soil.SoilStiffType == SoilStiffType);

        }
        private void stiffTrue()
        {
            DeselectDense(stiff_radiobutton);
            SoilStiffType = SoilStiffTypes.Stiff;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType && soil.SiltType == SiltType && soil.SoilStiffType == SoilStiffType);

        }
        private void denseNull()
        {
            DeselectDense();
            SoilStiffType = SoilStiffTypes.None;
            SoilDenseType = SoilDenseTypes.None;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.SoilRockType == SoilRockType && soil.SoilType == SoilType && soil.SiltType == SiltType );
        }
        private void poorTrue()
        {
            DeselectPoor(poor_radiobutton);
            RockSubType = RockSubTypes.Poor;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.RockType == RockType && soil.RockSubType==RockSubType);
        }
        private void fairTrue()
        {
            DeselectPoor(fair_radiobutton);
            RockSubType = RockSubTypes.Fair;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.RockType == RockType && soil.RockSubType == RockSubType);
        }
        private void excellentTrue()
        {
            DeselectPoor(excellent_radiobutton);
            RockSubType = RockSubTypes.Excellent;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.RockType == RockType && soil.RockSubType == RockSubType);
        }
        private void poorNull()
        {
            DeselectPoor();
            RockSubType = RockSubTypes.None;
            LibrarySoilList.ItemsSource = SoilLibrary.SoilDataList.Where(soil =>
            soil.RockType == RockType);
        }

        private void DeSelectAll(RadioButton radioButton=null)
        {
            if (radioButton != volcanic_radiobutton) volcanic_radiobutton.IsChecked = false;
            if (radioButton != metamorphic_radiobutton) metamorphic_radiobutton.IsChecked = false;
            if (radioButton != sedimantory_radiobutton) sedimantory_radiobutton.IsChecked = false;
            if(radioButton != gravel_radiobutton) gravel_radiobutton.IsChecked = false;
            if (radioButton != sand_radiobutton) sand_radiobutton.IsChecked = false;
            if (radioButton != silt_radiobutton && radioButton != nonplastic_radiobutton && radioButton != plastic_radiobutton) silt_radiobutton.IsChecked = false;
            if (radioButton != clay_radiobutton) clay_radiobutton.IsChecked = false;
            if (radioButton != nonplastic_radiobutton) nonplastic_radiobutton.IsChecked = false;
            if (radioButton != plastic_radiobutton) plastic_radiobutton.IsChecked = false;
            if (radioButton != loose_radiobutton) loose_radiobutton.IsChecked = false;
            if (radioButton != mediumdense_radiobutton) mediumdense_radiobutton.IsChecked = false;
            if (radioButton != dense_radiobutton) dense_radiobutton.IsChecked = false;
            if (radioButton != soft_radiobutton) soft_radiobutton.IsChecked = false;
            if (radioButton != mediumstiff_radiobutton) mediumstiff_radiobutton.IsChecked= false;
            if (radioButton != stiff_radiobutton) stiff_radiobutton.IsChecked = false;
            if (radioButton != poor_radiobutton) poor_radiobutton.IsChecked = false;
            if (radioButton != fair_radiobutton) fair_radiobutton.IsChecked = false;
            if (radioButton != excellent_radiobutton) excellent_radiobutton.IsChecked = false;
        }
        private void DeselectDense(RadioButton radioButton = null)
        {
            if (radioButton != dense_radiobutton) dense_radiobutton.IsChecked = false;
            if (radioButton != loose_radiobutton) loose_radiobutton.IsChecked = false;
            if (radioButton != mediumdense_radiobutton) mediumdense_radiobutton.IsChecked = false;
            if (radioButton != soft_radiobutton) soft_radiobutton.IsChecked = false;
            if (radioButton != stiff_radiobutton) stiff_radiobutton.IsChecked = false;
            if (radioButton != mediumstiff_radiobutton) mediumstiff_radiobutton.IsChecked = false;
        }

        private void DeselectPoor(RadioButton radioButton = null)
        {
            if (radioButton != poor_radiobutton) poor_radiobutton.IsChecked = false;
            if (radioButton != fair_radiobutton) fair_radiobutton.IsChecked = false;
            if (radioButton != excellent_radiobutton) excellent_radiobutton.IsChecked = false;
        }

        private void saveto_button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSoilData.isDefault) return;
            SoilData soil1 = new SoilData()
            {
                ID = Guid.NewGuid(),
                isDefault = true,
                isUserDefined = true,
                Name = selectedSoilData.Name,
                SoilRockType = selectedSoilData.SoilRockType,
                SoilType = selectedSoilData.SoilType,
                SoilDenseType = selectedSoilData.SoilDenseType,
                SoilStiffType = selectedSoilData.SoilStiffType,
                SiltType = selectedSoilData.SiltType,
                RockSubType = selectedSoilData.RockSubType,
                RockType = selectedSoilData.RockType,
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
            tempSavedSoilDataList.Add(soil1);
            SavedSoilList.SelectedItem = soil1;
            ListFocused(1);
        }

        private void SavedSoilList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SavedSoilList.SelectedIndex < 0) return;
            selectedSoilData = ((SoilData)SavedSoilList.SelectedItem);
            SelectionChanged();
        }

        private void SavedSoilList_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SavedSoilList.SelectedItem != null) selectedSoilData = (SoilData)SavedSoilList.SelectedItem;
            ListFocused(1);
            SelectionChanged();
        }

        private void AddCopyFromSaved_Click(object sender, RoutedEventArgs e)
        {
            if (!selectedSoilData.isDefault) return;
            SoilData soil1 = new SoilData()
            {
                ID = Guid.NewGuid(),
                isDefault = false,
                isUserDefined = true,
                Name = selectedSoilData.Name,
                SoilRockType = selectedSoilData.SoilRockType,
                SoilType = selectedSoilData.SoilType,
                SoilDenseType = selectedSoilData.SoilDenseType,
                SoilStiffType = selectedSoilData.SoilStiffType,
                SiltType = selectedSoilData.SiltType,
                RockSubType = selectedSoilData.RockSubType,
                RockType = selectedSoilData.RockType,
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
            ListFocused(2);
        }
        private void ListFocused(int list)
        {
            AddCopyFromLibrary_button.IsEnabled = false;
            AddCopyFromSaved.IsEnabled = false;
            addnew_button.IsEnabled = false;
            delete_button.IsEnabled = false;
            saveto_button.IsEnabled = false;
            deletesaved_button.IsEnabled = false;
            if (list == 0)
            {
                AddCopyFromLibrary_button.IsEnabled = true;
                UserSoilList.SelectedIndex = -1;
                SavedSoilList.SelectedIndex = -1;
            }else if(list == 1)
            {
                AddCopyFromSaved.IsEnabled = true;
                deletesaved_button.IsEnabled = true;
                UserSoilList.SelectedIndex = -1;
                LibrarySoilList.SelectedIndex = -1;
            }
            else if(list == 2)
            {
                addnew_button.IsEnabled = true;
                delete_button.IsEnabled = true;
                saveto_button.IsEnabled = true;
                LibrarySoilList.SelectedIndex = -1;
                SavedSoilList.SelectedIndex = -1;
            }
        }

        private void deletesaved_button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSoilData.isDefault && selectedSoilData.isUserDefined)
            {
                tempSavedSoilDataList.Remove(selectedSoilData);
                SavedSoilList.SelectedIndex = 0;
            }
        }
    }
}
