using ExDesign.Scripts;
using ExDesign.Datas;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ExDesign.Pages.Inputs
{
    /// <summary>
    /// SoilMethodPage.xaml etkileşim mantığı
    /// </summary>
    public partial class SoilMethodPage : Page
    {
        public char separator = ',';

        public SoilMethodPage()
        {
            InitializeComponent();
        }
        private void UnitChange()
        {
            LayerGridInitialize();
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            soilModelCombobox.SelectedIndex = StaticVariables.viewModel.SoilModelIndex;
            
            StaticEvents.UnitChangeEvent += UnitChange;

            LayerGridInitialize();

        }
               
        public void LayerGridInitialize()
        {
            if(StaticVariables.viewModel.soilLayerDatas==null) return;
            soilLayerGroupbox.Children.Clear();
            foreach (var item in StaticVariables.viewModel.soilLayerDatas)
            {
                StaticVariables.viewModel.soilLayerDatas[StaticVariables.viewModel.soilLayerDatas.IndexOf(item)].Soil = item.Soil!=null? WpfUtils.GetSoilData(item.Soil.ID):null;
                string layerIndex = StaticVariables.viewModel.soilLayerDatas.IndexOf(item).ToString();
                DockPanel dockPanel = new DockPanel();
                dockPanel.Margin = new Thickness(2);
                dockPanel.HorizontalAlignment = HorizontalAlignment.Left;
                Button deleteButton = new Button();
                deleteButton.Padding = new Thickness(2);
                deleteButton.Height = 27;
                deleteButton.Width = 27;
                Image deletebuttonImage = new Image();
                deletebuttonImage.Source = new BitmapImage(new Uri("/Textures/Icons/trash.png", UriKind.RelativeOrAbsolute));
                deleteButton.Content = deletebuttonImage;
                deleteButton.Name ="delete_"+layerIndex;
                deleteButton.Click += DeleteButton_Click;
                TextBox textBox_no = new TextBox();
                textBox_no.Width = 30;
                textBox_no.Text = (int.Parse( layerIndex)+1).ToString();
                textBox_no.VerticalContentAlignment = VerticalAlignment.Center;
                textBox_no.HorizontalContentAlignment = HorizontalAlignment.Center;
                textBox_no.IsEnabled = false;
                TextBox textbox_height = new TextBox();
                textbox_height.Width = 80;
                textbox_height.Text =WpfUtils.GetDimension(item.LayerHeight).ToString();
                textbox_height.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_height.TextChanged += Textbox_height_TextChanged;
                textbox_height.PreviewKeyDown += Textbox_height_PreviewKeyDown;
                textbox_height.PreviewTextInput += Textbox_height_PreviewTextInput;
                textbox_height.Name = "textboxheight_" + layerIndex;
                Image textureImage = new Image();
                textureImage.Height = 27;
                textureImage.Width = 55;
                if (item.Soil != null) textureImage = TextureImage(WpfUtils.GetSoilData(item.Soil.ID));
                ComboBox comboBox = new ComboBox();
                comboBox.Width = 300;
                comboBox.ItemsSource = StaticVariables.viewModel.soilDatas;
                comboBox.DisplayMemberPath = "Name";                
                comboBox.Name ="combo_"+ layerIndex;
               if(item.Soil!=null) comboBox.SelectedItem = WpfUtils.GetSoilData(item.Soil.ID);
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
                comboBox.VerticalContentAlignment = VerticalAlignment.Center;
                TextBox textbox_gama = new TextBox();
                textbox_gama.Width = 80;
                textbox_gama.Text = comboBox.SelectedIndex >=0 ? WpfUtils.ChangeDecimalOptions(WpfUtils.GetDensity(((SoilData)comboBox.SelectedItem).NaturalUnitWeight)) : "" ;
                textbox_gama.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_gama.IsEnabled = false;
                TextBox textbox_gamasat = new TextBox();
                textbox_gamasat.Width = 80;
                textbox_gamasat.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(WpfUtils.GetDensity(((SoilData)comboBox.SelectedItem).SaturatedUnitWeight)) : "";
                textbox_gamasat.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_gamasat.IsEnabled = false;
                TextBox textbox_fi = new TextBox();
                textbox_fi.Width = 80;
                textbox_fi.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_fi.IsEnabled = false;
                TextBox textbox_Cprime = new TextBox();
                textbox_Cprime.Width = 80;
                textbox_Cprime.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_Cprime.IsEnabled = false;
                TextBox textbox_Cu = new TextBox();
                textbox_Cu.Width = 80;
                if (comboBox.SelectedIndex >= 0)
                {
                    switch (WpfUtils.GetSoilState(((SoilData)comboBox.SelectedItem).SoilStressStateIndex))
                    {
                        case SoilState.Drained:
                            textbox_Cu.Text = " - ";
                            textbox_fi.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(((SoilData)comboBox.SelectedItem).SoilFrictionAngle) : "";
                            textbox_Cprime.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((SoilData)comboBox.SelectedItem).EffectiveCohesion)) : "";
                            break;
                        case SoilState.UnDrained:
                            textbox_Cu.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((SoilData)comboBox.SelectedItem).UndrainedShearStrength)) : "";
                            textbox_fi.Text = "0";
                            textbox_Cprime.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((SoilData)comboBox.SelectedItem).WallSoilAdhesion)) : "";
                            break;
                        default:
                            break;
                    }
                }
                textbox_Cu.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_Cu.IsEnabled = false;
                TextBox textbox_gamad = new TextBox();
                textbox_gamad.Width = 80;
                textbox_gamad.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(((SoilData)comboBox.SelectedItem).WallSoilFrictionAngle) : "";
                textbox_gamad.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_gamad.IsEnabled = false;
                TextBox textbox_Poisson = new TextBox();
                textbox_Poisson.Width = 80;
                textbox_Poisson.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(((SoilData)comboBox.SelectedItem).PoissonRatio) : "";
                textbox_Poisson.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_Poisson.IsEnabled = false;
                TextBox textbox_SoilState = new TextBox();
                textbox_SoilState.Width = 80;
                textbox_SoilState.Text = comboBox.SelectedIndex >= 0 ? FindResource(WpfUtils.GetSoilState(((SoilData)comboBox.SelectedItem).SoilStressStateIndex).ToString()).ToString() : "";
                textbox_SoilState.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_SoilState.IsEnabled = false;
                TextBox textbox_OCR = new TextBox();
                textbox_OCR.Width = 80;
                if(comboBox.SelectedIndex >= 0)
                {
                    switch (((SoilData)comboBox.SelectedItem).SoilStateKoIndex)
                    {
                        case 2:
                            textbox_OCR.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(((SoilData)comboBox.SelectedItem).Ocr) : "";
                            break;
                        default:
                            textbox_OCR.Text = "1";
                            break;
                    }
                }
                
                textbox_OCR.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_OCR.IsEnabled = false;
                TextBox textbox_K0 = new TextBox();
                textbox_K0.Width = 80;
                textbox_K0.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(((SoilData)comboBox.SelectedItem).K0) : "";
                textbox_K0.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_K0.IsEnabled = false;
                TextBox textbox_model_parameter = new TextBox();
                textbox_model_parameter.Width = 80;
                switch (WpfUtils.GetSoilModelType(StaticVariables.viewModel.SoilModelIndex))
                {
                    case SoilModelType.Schmitt_Model:
                        textbox_model_parameter.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((SoilData)comboBox.SelectedItem).OedometricModulus)) : "";
                        selected_parameter.Text = "Eoed";
                        break;
                    case SoilModelType.Chadeisson_Model:
                        textbox_model_parameter.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(((SoilData)comboBox.SelectedItem).CohesionFactor) : "";
                        selected_parameter.Text = "Ap";
                        break;
                    case SoilModelType.Vesic_Model:
                        textbox_model_parameter.Text = comboBox.SelectedIndex >= 0 ? WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((SoilData)comboBox.SelectedItem).YoungModulus)) : "";
                        selected_parameter.Text = "Es'";
                        break;
                    default:
                        break;
                }
                textbox_model_parameter.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_model_parameter.IsEnabled = false;
                dockPanel.Children.Add(deleteButton);
                dockPanel.Children.Add(textBox_no);
                dockPanel.Children.Add(textbox_height);
                //dockPanel.Children.Add(textbox_layername);
                dockPanel.Children.Add(textureImage);
                dockPanel.Children.Add(comboBox);
                dockPanel.Children.Add(textbox_gama);
                dockPanel.Children.Add(textbox_gamasat);
                dockPanel.Children.Add(textbox_fi);
                dockPanel.Children.Add(textbox_Cprime);
                dockPanel.Children.Add(textbox_Cu);  
                dockPanel.Children.Add(textbox_gamad);
                dockPanel.Children.Add(textbox_Poisson);
                dockPanel.Children.Add(textbox_SoilState);
                dockPanel.Children.Add(textbox_OCR);
                dockPanel.Children.Add(textbox_K0);
                dockPanel.Children.Add(textbox_model_parameter);
                soilLayerGroupbox.Children.Add(dockPanel);

                ////refresh windows
                //StaticVariables.view3DPage.Refreshview();
                //StaticVariables.SideviewPage.Refreshview();
            }
        }
        private Image TextureImage(SoilData soil)
        {
            Image image = new Image();
            image.Width = 55;
            image.Height = 27;
            if (soil.isSoilTexture)
            {
                image.Source = new BitmapImage(soil.SoilTexture.TextureUri);
                image.Stretch = Stretch.Fill;
            }
            else
            {
                image.Source = new DrawingImage(
                    new GeometryDrawing(
                        new SolidColorBrush(soil.SoilColor),
                        new Pen(new SolidColorBrush(Color.FromRgb(171,173,179)),
                        1),
                        new RectangleGeometry(
                            new Rect(
                                new Size(55, 27)
                                )
                            )
                        )
                    );
                image.Stretch = Stretch.Fill;
            }
            return image;
        }

        private void Textbox_layername_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            StaticVariables.viewModel.soilLayerDatas[int.Parse(textBox.Name.Split('_')[1])].Name = textBox.Text;
        }
        private void Textbox_height_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void Textbox_height_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Textbox_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.soilLayerDatas[int.Parse(textBox.Name.Split('_')[1])].LayerHeight = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            StaticVariables.viewModel.soilLayerDatas.RemoveAt(int.Parse( buttonName.Split('_')[1]));
            LayerGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comboname = ((ComboBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.soilLayerDatas[int.Parse(comboname)].Soil = ((SoilData)((ComboBox)sender).SelectedItem);
            
            LayerGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
            StaticEvents.UnitChangeEvent -= UnitChange;
        }

        private void soilModelCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StaticVariables.viewModel.SoilModelIndex = soilModelCombobox.SelectedIndex;
            switch (WpfUtils.GetSoilModelType(StaticVariables.viewModel.SoilModelIndex))
            {
                case SoilModelType.Schmitt_Model:
                    SchmittInfo.Visibility = Visibility.Visible;
                    ChadeissonInfo.Visibility = Visibility.Hidden;
                    VesicInfo.Visibility = Visibility.Hidden;
                    break;
                case SoilModelType.Chadeisson_Model:
                    SchmittInfo.Visibility = Visibility.Hidden;
                    ChadeissonInfo.Visibility = Visibility.Visible;
                    VesicInfo.Visibility = Visibility.Hidden;
                    break;
                case SoilModelType.Vesic_Model:
                    SchmittInfo.Visibility = Visibility.Hidden;
                    ChadeissonInfo.Visibility = Visibility.Hidden;
                    VesicInfo.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            UnitChange();
        }

        private void addsoillayer_bttn_Click(object sender, RoutedEventArgs e)
        {
            SoilLayerData soilLayerData1 = new SoilLayerData { LayerHeight=3,Name=FindResource("SoilLayer").ToString() };
            StaticVariables.viewModel.soilLayerDatas.Add(soilLayerData1);
            LayerGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void LayerGridScrollBar_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            headerScrollBar?.ScrollToHorizontalOffset(LayerGridScrollBar.HorizontalOffset );
        }

        private void soilTypeLibrary_Click(object sender, RoutedEventArgs e)
        {
            Windows.SoilTypeLibrary soilLibraryPage = new Windows.SoilTypeLibrary();
            soilLibraryPage.SetMethodPage(this);
            soilLibraryPage.ShowDialog();
        }
    }
}
