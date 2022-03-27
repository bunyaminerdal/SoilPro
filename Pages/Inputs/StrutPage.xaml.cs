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
    /// StrutPage.xaml etkileşim mantığı
    /// </summary>
    public partial class StrutPage : Page
    {
        public char separator = ',';

        public StrutPage()
        {
            InitializeComponent();
        }
        private void UnitChange()
        {

            StrutsGridInitialize();
            depth_txtbox.Text = FindResource("Depth").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            strutLength_txtbox.Text = FindResource("StrutLength").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            strutdiameter_txtbox.Text = FindResource("StrutDiameter").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            Spacing_txtbox.Text = FindResource("Spacing").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            strutthickness_txtbox.Text = FindResource("StrutThickness").ToString() + " (" + StaticVariables.dimensionUnit + ")";            
            soldier2.Text = FindResource("BeamHeight").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            soldier3.Text = FindResource("BeamWidth").ToString() + " (" + StaticVariables.dimensionUnit + ")";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            if (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex) == WallType.SteelSheetWall) addanchor_bttn.IsEnabled = false;
            StaticEvents.UnitChangeEvent += UnitChange;
            UnitChange();
            StrutsGridInitialize();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            StaticVariables.viewModel.strutDatas.RemoveAt(int.Parse(buttonName.Split('_')[1]));
            StrutsGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void StrutsGridInitialize()
        {            

            if (StaticVariables.viewModel.strutDatas == null) return;
            strutssGroupbox.Children.Clear();

            StaticVariables.SortStruts(StaticVariables.viewModel.strutDatas);

            foreach (var strut in StaticVariables.viewModel.strutDatas)
            {
                string strutIndex = StaticVariables.viewModel.strutDatas.IndexOf(strut).ToString();
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
                deleteButton.Name = "delete_" + strutIndex;
                deleteButton.Click += DeleteButton_Click;
                TextBox textBox_no = new TextBox();
                textBox_no.Width = 30;
                textBox_no.Text = (int.Parse(strutIndex) + 1).ToString();
                textBox_no.VerticalContentAlignment = VerticalAlignment.Center;
                textBox_no.HorizontalContentAlignment = HorizontalAlignment.Center;
                textBox_no.IsEnabled = false;
                TextBox textbox_depth = new TextBox();
                textbox_depth.Width = 100;
                textbox_depth.Text = WpfUtils.GetDimension(strut.StrutDepth).ToString();
                textbox_depth.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_depth.TextChanged += Textbox_depth_TextChanged;
                textbox_depth.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_depth.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_depth.Name = "textboxdepth_" + strutIndex;
                TextBox textbox_strutLength = new TextBox();
                textbox_strutLength.Width = 120;
                textbox_strutLength.Text = WpfUtils.GetDimension(strut.StrutLength).ToString();
                textbox_strutLength.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_strutLength.TextChanged += Textbox_freeLength_TextChanged;
                textbox_strutLength.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_strutLength.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_strutLength.Name = "textboxstrutLength_" + strutIndex;
                TextBox textbox_strutDiameter = new TextBox();
                textbox_strutDiameter.Width = 120;
                textbox_strutDiameter.Text = WpfUtils.GetDimension(strut.StrutOuterDiameter).ToString();
                textbox_strutDiameter.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_strutDiameter.TextChanged += Textbox_strutDiameter_TextChanged;
                textbox_strutDiameter.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_strutDiameter.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_strutDiameter.Name = "textboxStrutDiameter_" + strutIndex;
                TextBox textbox_strutThickness = new TextBox();
                textbox_strutThickness.Width = 120;
                textbox_strutThickness.Text = WpfUtils.GetDimension(strut.StrutThickness).ToString();
                textbox_strutThickness.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_strutThickness.TextChanged += Textbox_strutThickness_TextChanged;
                textbox_strutThickness.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_strutThickness.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_strutThickness.Name = "textboxThickness_" + strutIndex;
                DockPanel tempPanel1 = new DockPanel();
                tempPanel1.Width = 50;
                CheckBox checkBox_isCentralPlacement = new CheckBox();
                checkBox_isCentralPlacement.Width = 75;
                checkBox_isCentralPlacement.VerticalContentAlignment = VerticalAlignment.Center;
                if (strut.IsCentralPlacement) checkBox_isCentralPlacement.IsChecked = true;
                checkBox_isCentralPlacement.Checked += CheckBox_isCentralPlacement_Checked;
                checkBox_isCentralPlacement.Unchecked += CheckBox_isCentralPlacement_Unchecked;
                checkBox_isCentralPlacement.Name = "checkboxCentralPlacement_" + strutIndex;
                TextBox textbox_spacing = new TextBox();
                textbox_spacing.Width = 100;
                textbox_spacing.Text = WpfUtils.GetDimension(strut.StrutSpacing).ToString();
                textbox_spacing.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_spacing.TextChanged += Textbox_spacing_TextChanged;
                textbox_spacing.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_spacing.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_spacing.Name = "textboxSpacing_" + strutIndex;
                DockPanel tempPanel = new DockPanel();
                tempPanel.Width = 40;
                CheckBox checkBox_soldierBeam = new CheckBox();
                checkBox_soldierBeam.Width = 60;
                checkBox_soldierBeam.VerticalContentAlignment = VerticalAlignment.Center;
                if (strut.IsSoldierBeam) checkBox_soldierBeam.IsChecked = true;
                checkBox_soldierBeam.Checked += CheckBox_soldierBeam_Checked;
                checkBox_soldierBeam.Unchecked += CheckBox_soldierBeam_Unchecked;
                checkBox_soldierBeam.Name = "checkboxSoldierBeam_" + strutIndex;
                TextBox textbox_beamHeight = new TextBox();
                textbox_beamHeight.Width = 100;
                textbox_beamHeight.Text = WpfUtils.GetDimension(strut.SoldierBeamHeight).ToString();
                textbox_beamHeight.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_beamHeight.TextChanged += Textbox_beamheight_TextChanged;
                textbox_beamHeight.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_beamHeight.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_beamHeight.Name = "textboxBeamHeight_" + strutIndex;
                if (!strut.IsSoldierBeam) textbox_beamHeight.IsEnabled = false;
                TextBox textbox_beamWidth = new TextBox();
                textbox_beamWidth.Width = 100;
                textbox_beamWidth.Text = WpfUtils.GetDimension(strut.SoldierBeamwidth).ToString();
                textbox_beamWidth.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_beamWidth.TextChanged += Textbox_beamwidth_TextChanged;
                textbox_beamWidth.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_beamWidth.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_beamWidth.Name = "textboxBeamWidth_" + strutIndex;
                if (!strut.IsSoldierBeam) textbox_beamWidth.IsEnabled = false;
                dockPanel.Children.Add(deleteButton);
                dockPanel.Children.Add(textBox_no);
                dockPanel.Children.Add(textbox_depth);
                dockPanel.Children.Add(textbox_strutLength);
                dockPanel.Children.Add(textbox_strutDiameter);
                dockPanel.Children.Add(textbox_strutThickness);                
                dockPanel.Children.Add(tempPanel1);
                dockPanel.Children.Add(checkBox_isCentralPlacement);
                dockPanel.Children.Add(textbox_spacing);                
                if (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex) != WallType.SteelSheetWall)
                {
                    dockPanel.Children.Add(tempPanel);
                    dockPanel.Children.Add(checkBox_soldierBeam);
                    dockPanel.Children.Add(textbox_beamHeight);
                    dockPanel.Children.Add(textbox_beamWidth);
                }
                else
                {
                    soldier1.Visibility = Visibility.Hidden;
                    soldier2.Visibility = Visibility.Hidden;
                    soldier3.Visibility = Visibility.Hidden;                    
                }

                strutssGroupbox.Children.Add(dockPanel);

                ////refresh windows
                //StaticVariables.view3DPage.Refreshview();
                //StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void Textbox_beamheight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.strutDatas[int.Parse(textBox.Name.Split('_')[1])].SoldierBeamHeight = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void Textbox_beamwidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.strutDatas[int.Parse(textBox.Name.Split('_')[1])].SoldierBeamwidth = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void CheckBox_soldierBeam_Unchecked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.strutDatas[int.Parse(checkName)].IsSoldierBeam = false;

            StrutsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void CheckBox_soldierBeam_Checked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.strutDatas[int.Parse(checkName)].IsSoldierBeam = true;

            StrutsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        private void Textbox_spacing_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.strutDatas[int.Parse(textBox.Name.Split('_')[1])].StrutSpacing = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
            }
        }

        private void CheckBox_isCentralPlacement_Unchecked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.strutDatas[int.Parse(checkName)].IsCentralPlacement = false;

            StrutsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void CheckBox_isCentralPlacement_Checked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.strutDatas[int.Parse(checkName)].IsCentralPlacement = true;

            StrutsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void Textbox_strutThickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.strutDatas[int.Parse(textBox.Name.Split('_')[1])].StrutThickness = WpfUtils.GetValueDimension(result);
                StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void Textbox_strutDiameter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.strutDatas[int.Parse(textBox.Name.Split('_')[1])].StrutOuterDiameter = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void Textbox_freeLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.strutDatas[int.Parse(textBox.Name.Split('_')[1])].StrutLength = WpfUtils.GetValueDimension(result);
                
            }
        }

        private void Textbox_depth_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.strutDatas[int.Parse(textBox.Name.Split('_')[1])].StrutDepth = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void Textbox_depth_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void Textbox_depth_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
            StaticEvents.UnitChangeEvent -= UnitChange;
        }
        private void addanchor_bttn_Click(object sender, RoutedEventArgs e)
        {

            double lastDepth = 2;
            if (StaticVariables.viewModel.strutDatas.Count > 0)
            {
                lastDepth = StaticVariables.viewModel.strutDatas[StaticVariables.viewModel.strutDatas.Count - 1].StrutDepth;
                if (lastDepth >= StaticVariables.viewModel.excavationHeight) return;
                lastDepth += 2;
            }

            StrutData strutData = new StrutData {StrutDepth =lastDepth,StrutLength =14,StrutOuterDiameter=0.812,StrutThickness=0.016,StrutSpacing=4.5,IsCentralPlacement=true, IsSoldierBeam = true, SoldierBeamHeight = 1.1, SoldierBeamwidth = 0.5 };
            StaticVariables.viewModel.strutDatas.Add(strutData);
            StrutsGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        private void anchorsGridScrollBar_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            headerScrollBar?.ScrollToHorizontalOffset(strutsGridScrollBar.HorizontalOffset);
        }
    }
}
