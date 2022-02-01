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
            //concretewall_height_unit.Content = StaticVariables.dimensionUnit;
            //concretewall_thickness_unit.Content = StaticVariables.dimensionUnit;
            //pilewall_height_unit.Content = StaticVariables.dimensionUnit;
            //pile_diameter_unit.Content = StaticVariables.dimensionUnit;
            //pile_space_unit.Content = StaticVariables.dimensionUnit;
            //beam_height_unit.Content = StaticVariables.dimensionUnit;
            //beam_width_unit.Content = StaticVariables.dimensionUnit;
            //sheetpilewall_height_unit.Content = StaticVariables.dimensionUnit;
            //areaText_unit.Content = StaticVariables.areaUnit + "/m";
            //inertiaText_unit.Content = StaticVariables.inertiaUnit + "/m";
            //EIText_unit.Content = StaticVariables.EIUnit + "/m";
            //EAText_unit.Content = StaticVariables.forceUnit + "/m";

            //concretewall_height.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()));
            //concretewall_thickness.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallThickness()));
            //pilewall_height.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()));
            //pile_diameter.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallThickness()));
            //pile_space.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetPileSpace()));
            //beam_height.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetCapBeamH()));
            //beam_width.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetCapBeamB()));
            //sheetpilewall_height.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()));
            //areaText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetArea(StaticVariables.viewModel.GetWallArea()));
            //inertiaText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetInertia(StaticVariables.viewModel.GetWallInertia()));
            //EIText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetMomentScaleDimension(StaticVariables.viewModel.GetWallEI()));
            //EAText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetForce(StaticVariables.viewModel.GetWallEA()));
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            soilModelCombobox.SelectedIndex = StaticVariables.viewModel.SoilModelIndex;
            
            StaticEvents.UnitChangeEvent += UnitChange;
            SoilData soilData1 = new SoilData { Name = "siltli kumlu balçık", NaturalUnitWeight = 13 };
            SoilData soilData2 = new SoilData { Name = "çakıllı makıllı sağlam gibi", NaturalUnitWeight = 11 };
            StaticVariables.viewModel.soilDatas.Add(soilData1);
            StaticVariables.viewModel.soilDatas.Add(soilData2);
            LayerGridInitialize();

        }
               
        private void LayerGridInitialize()
        {
            if(StaticVariables.viewModel.soilLayerDatas==null) return;
            soilLayerGroupbox.Children.Clear();
            
            foreach (var item in StaticVariables.viewModel.soilLayerDatas)
            {
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
                textBox_no.Text = layerIndex;
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
                TextBox textbox_layername = new TextBox();
                textbox_layername.Width = 200;
                textbox_layername.Text = item.Name;
                textbox_layername.Name ="textboxlayername_"+layerIndex;
                textbox_layername.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_layername.TextChanged += Textbox_layername_TextChanged;
                ComboBox comboBox = new ComboBox();
                comboBox.Width = 200;
                comboBox.ItemsSource = StaticVariables.viewModel.soilDatas;
                comboBox.DisplayMemberPath = "Name";
                comboBox.SelectedItem =WpfUtils.GetSoilData( item.soilIndex);
                comboBox.Name ="combo_"+ layerIndex;
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
                comboBox.VerticalContentAlignment = VerticalAlignment.Center;
                TextBox textbox_gama = new TextBox();
                textbox_gama.Width = 80;
                textbox_gama.Text = comboBox.SelectedIndex >=0 ? ((SoilData)comboBox.SelectedItem).NaturalUnitWeight.ToString():"" ;
                textbox_gama.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_gama.IsEnabled = false;
                TextBox textbox_gamasat = new TextBox();
                textbox_gamasat.Width = 80;
                textbox_gamasat.Text = comboBox.SelectedIndex >= 0 ? ((SoilData)comboBox.SelectedItem).SaturatedUnitWeight.ToString():"";
                textbox_gamasat.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_gamasat.IsEnabled = false;
                TextBox textbox_fi = new TextBox();
                textbox_fi.Width = 80;
                textbox_fi.Text = comboBox.SelectedIndex >= 0 ? ((SoilData)comboBox.SelectedItem).SoilFrictionAngle.ToString():"";
                textbox_fi.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_fi.IsEnabled = false;
                TextBox textbox_Cprime = new TextBox();
                textbox_Cprime.Width = 80;
                textbox_Cprime.Text = comboBox.SelectedIndex >= 0 ? ((SoilData)comboBox.SelectedItem).EffectiveCohesion.ToString():"";
                textbox_Cprime.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_Cprime.IsEnabled = false;
                TextBox textbox_Cu = new TextBox();
                textbox_Cu.Width = 80;
                textbox_Cu.Text = comboBox.SelectedIndex >= 0 ? ((SoilData)comboBox.SelectedItem).UndrainedShearStrength.ToString():"";
                textbox_Cu.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_Cu.IsEnabled = false;
                TextBox textbox_Poisson = new TextBox();
                textbox_Poisson.Width = 80;
                textbox_Poisson.Text = comboBox.SelectedIndex >= 0 ? ((SoilData)comboBox.SelectedItem).PoissonRatio.ToString():"";
                textbox_Poisson.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_Poisson.IsEnabled = false;
                dockPanel.Children.Add(deleteButton);
                dockPanel.Children.Add(textBox_no);
                dockPanel.Children.Add(textbox_height);
                dockPanel.Children.Add(textbox_layername);
                dockPanel.Children.Add(comboBox);
                dockPanel.Children.Add(textbox_gama);
                dockPanel.Children.Add(textbox_gamasat);
                dockPanel.Children.Add(textbox_fi);
                dockPanel.Children.Add(textbox_Cprime);
                dockPanel.Children.Add(textbox_Cu);
                dockPanel.Children.Add(textbox_Poisson);
                soilLayerGroupbox.Children.Add(dockPanel);
                
            }
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
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            StaticVariables.viewModel.soilLayerDatas.RemoveAt(int.Parse( buttonName.Split('_')[1]));
            LayerGridInitialize();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comboname = ((ComboBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.soilLayerDatas[int.Parse(comboname)].soilIndex =WpfUtils.GetSoilDataIndex( ((SoilData)((ComboBox)sender).SelectedItem));
            LayerGridInitialize();
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
            SoilLayerData soilLayerData1 = new SoilLayerData { LayerHeight=3,Name=FindResource("SoilLayer").ToString(),soilIndex=0 };
            StaticVariables.viewModel.soilLayerDatas.Add(soilLayerData1);
            LayerGridInitialize();
        }

        private void LayerGridScrollBar_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            headerScrollBar?.ScrollToHorizontalOffset(LayerGridScrollBar.HorizontalOffset );
        }

        private void soilTypeLibrary_Click(object sender, RoutedEventArgs e)
        {
            Windows.SoilTypeLibrary soilLibraryPage = new Windows.SoilTypeLibrary();
            soilLibraryPage.ShowDialog();
        }
    }
}
