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
    /// AnchorsPage.xaml etkileşim mantığı
    /// </summary>
    public partial class AnchorsPage : Page
    {
        public char separator = ',';

        public AnchorsPage()
        {
            InitializeComponent();
        }

        private void UnitChange()
        {
            
            AnchorsGridInitialize();
            depth_txtbox.Text = FindResource("Depth").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            freeLength_txtbox.Text = FindResource("FreeLength").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            rootLength_txtbox.Text = FindResource("RootLength").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            Inclination_txtbox.Text = FindResource("Inclination").ToString() + " (°)";
            Spacing_txtbox.Text = FindResource("Spacing").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            RootDiameter_txtbox.Text = FindResource("RootDiameter").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            totalNominalArea_txtbox.Text = FindResource("TotalNominalArea").ToString() + " (" + StaticVariables.areaUnit + ")";
            BreakingStrength_txtbox.Text = FindResource("BreakingStrength").ToString() + " (" + StaticVariables.forceUnit + ")";
            rootModulus_txtbox.Text = FindResource("RootModulus").ToString() + " (" + StaticVariables.StressUnit + ")";
            skinfriction_txtbox.Text = FindResource("RootSoilSurfaceResistance").ToString() + " (" + StaticVariables.StressUnit + ")";
            prestressForce_txtbox.Text = FindResource("PreStressForce").ToString() + " (" + StaticVariables.forceUnit + ")";
            soldier2.Text = FindResource("BeamHeight").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            soldier3.Text = FindResource("BeamWidth").ToString() + " (" + StaticVariables.dimensionUnit + ")";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;

            if (StaticVariables.viewModel.useCableDiameterAndNumberForDesign == true)
            {                
                useCableData_checkbox.IsChecked = true;
            }
            else
            {
                useCableData_checkbox.IsChecked = false;
            }
            
            StaticEvents.UnitChangeEvent += UnitChange;
            UnitChange();
            AnchorsGridInitialize();
        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            StaticVariables.viewModel.anchorDatas.RemoveAt(int.Parse(buttonName.Split('_')[1]));
            AnchorsGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        public void AnchorsGridInitialize()
        {
            if (StaticVariables.viewModel.anchorDatas == null) return;
            anchorsGroupbox.Children.Clear();
            
            StaticVariables.SortAnchors(StaticVariables.viewModel.anchorDatas);

            foreach (var anchor in StaticVariables.viewModel.anchorDatas)
            {
                string anchorIndex = StaticVariables.viewModel.anchorDatas.IndexOf(anchor).ToString();
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
                deleteButton.Name = "delete_" + anchorIndex;
                deleteButton.Click += DeleteButton_Click;
                TextBox textBox_no = new TextBox();
                textBox_no.Width = 30;
                textBox_no.Text = (int.Parse(anchorIndex) + 1).ToString();
                textBox_no.VerticalContentAlignment = VerticalAlignment.Center;
                textBox_no.HorizontalContentAlignment = HorizontalAlignment.Center;
                textBox_no.IsEnabled = false;
                TextBox textbox_depth = new TextBox();
                textbox_depth.Width = 80;
                textbox_depth.Text = WpfUtils.GetDimension(anchor.AnchorDepth).ToString();
                textbox_depth.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_depth.TextChanged += Textbox_depth_TextChanged;
                textbox_depth.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_depth.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_depth.Name = "textboxdepth_" + anchorIndex;
                TextBox textbox_freeLength = new TextBox();
                textbox_freeLength.Width = 80;
                textbox_freeLength.Text = WpfUtils.GetDimension(anchor.FreeLength).ToString();
                textbox_freeLength.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_freeLength.TextChanged += Textbox_freeLength_TextChanged;
                textbox_freeLength.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_freeLength.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_freeLength.Name = "textboxFreeLength_" + anchorIndex;
                TextBox textbox_rootLength = new TextBox();
                textbox_rootLength.Width = 80;
                textbox_rootLength.Text = WpfUtils.GetDimension(anchor.RootLength).ToString();
                textbox_rootLength.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_rootLength.TextChanged += Textbox_rootLength_TextChanged;
                textbox_rootLength.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_rootLength.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_rootLength.Name = "textboxRootLength_" + anchorIndex;
                TextBox textbox_inclination = new TextBox();
                textbox_inclination.Width = 80;
                textbox_inclination.Text = anchor.Inclination.ToString();
                textbox_inclination.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_inclination.TextChanged += Textbox_inclination_TextChanged;
                textbox_inclination.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_inclination.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_inclination.Name = "textboxinclination_" + anchorIndex;
                DockPanel tempPanel2 = new DockPanel();
                tempPanel2.Width = 30;
                CheckBox checkBox_centralPlacement = new CheckBox();
                checkBox_centralPlacement.Width = 50;
                checkBox_centralPlacement.VerticalContentAlignment = VerticalAlignment.Center;
                if (anchor.IsCentralPlacement) checkBox_centralPlacement.IsChecked = true;
                checkBox_centralPlacement.Checked += CheckBox_centralPlacement_Checked;
                checkBox_centralPlacement.Unchecked += CheckBox_centralPlacement_Unchecked;
                checkBox_centralPlacement.Name = "checkboxCentralPlacement_" + anchorIndex;
                TextBox textbox_spacing = new TextBox();
                textbox_spacing.Width = 80;
                textbox_spacing.Text = WpfUtils.GetDimension(anchor.Spacing).ToString();
                textbox_spacing.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_spacing.TextChanged += Textbox_spacing_TextChanged;
                textbox_spacing.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_spacing.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_spacing.Name = "textboxSpacing_" + anchorIndex;
                TextBox textbox_rootDiameter = new TextBox();
                textbox_rootDiameter.Width = 100;
                textbox_rootDiameter.Text = WpfUtils.GetDimension(anchor.RootDiameter).ToString();
                textbox_rootDiameter.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_rootDiameter.TextChanged += Textbox_rootDiameter_TextChanged;
                textbox_rootDiameter.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_rootDiameter.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_rootDiameter.Name = "textboxRootDiameter_" + anchorIndex;
                ComboBox comboBox_numberofcable = new ComboBox();
                comboBox_numberofcable.Width = 105;
                comboBox_numberofcable.Items.Add("2");
                comboBox_numberofcable.Items.Add("3");
                comboBox_numberofcable.Items.Add("4");
                comboBox_numberofcable.Items.Add("5");
                comboBox_numberofcable.Items.Add("6");
                comboBox_numberofcable.Name = "comboNumberofCable_" + anchorIndex;
                comboBox_numberofcable.SelectedIndex = anchor.NumberofCable;
                if (!StaticVariables.viewModel.useCableDiameterAndNumberForDesign)
                {
                    comboBox_numberofcable.SelectedIndex = -1;
                    comboBox_numberofcable.IsEnabled = false;
                }
                comboBox_numberofcable.SelectionChanged += ComboBox_numberofcable_SelectionChanged;
                comboBox_numberofcable.VerticalContentAlignment = VerticalAlignment.Center;
                ComboBox comboBox_cableDiameter = new ComboBox();
                comboBox_cableDiameter.Width = 100;
                comboBox_cableDiameter.ItemsSource = Wire.WireDataList;
                comboBox_cableDiameter.DisplayMemberPath = "NominalDiameter";
                comboBox_cableDiameter.Name = "comboCableDiameter_" + anchorIndex;
                comboBox_cableDiameter.SelectedItem =WpfUtils.GetWireData( anchor.CableData.NominalDiameter);
                if (!StaticVariables.viewModel.useCableDiameterAndNumberForDesign)
                {
                    comboBox_cableDiameter.SelectedIndex = -1;
                    comboBox_cableDiameter.IsEnabled = false;
                }
                comboBox_cableDiameter.SelectionChanged += ComboBox_cableDiameter_SelectionChanged;
                comboBox_cableDiameter.VerticalContentAlignment = VerticalAlignment.Center;
                TextBox textbox_totalNominalArea = new TextBox();
                textbox_totalNominalArea.Width = 125;
                textbox_totalNominalArea.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetArea(anchor.TotalNominalArea));
                textbox_totalNominalArea.VerticalContentAlignment = VerticalAlignment.Center;
                if (!StaticVariables.viewModel.useCableDiameterAndNumberForDesign)
                {
                    textbox_totalNominalArea.IsEnabled = true;
                }
                else
                {
                    textbox_totalNominalArea.IsEnabled = false;
                }
                textbox_totalNominalArea.TextChanged += Textbox_totalnominalarea_TextChanged;
                textbox_totalNominalArea.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_totalNominalArea.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_totalNominalArea.Name = "textboxTotalNominalArea_" + anchorIndex;
                
                TextBox textbox_breakingStrength = new TextBox();
                textbox_breakingStrength.Width = 125;
                textbox_breakingStrength.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetForce(anchor.BreakingStrength));
                textbox_breakingStrength.VerticalContentAlignment = VerticalAlignment.Center;
                if (!StaticVariables.viewModel.useCableDiameterAndNumberForDesign)
                {
                    textbox_breakingStrength.IsEnabled = true;
                }
                else
                {
                    textbox_breakingStrength.IsEnabled = false;
                }
                textbox_breakingStrength.TextChanged += Textbox_breakingStrength_TextChanged;
                textbox_breakingStrength.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_breakingStrength.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_breakingStrength.Name = "textboxbreakingStrength_" + anchorIndex;
                
                TextBox textbox_rootModulus = new TextBox();
                textbox_rootModulus.Width = 125;
                textbox_rootModulus.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(anchor.RootModulus));
                textbox_rootModulus.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_rootModulus.TextChanged += Textbox_rootmodulus_TextChanged;
                textbox_rootModulus.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_rootModulus.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_rootModulus.Name = "textboxRootModulus_" + anchorIndex;
                TextBox textbox_rootsoilfriction = new TextBox();
                textbox_rootsoilfriction.Width = 125;
                textbox_rootsoilfriction.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(anchor.RootSoilFrictionResistance));
                textbox_rootsoilfriction.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_rootsoilfriction.TextChanged += Textbox_rootsoilfriction_TextChanged;
                textbox_rootsoilfriction.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_rootsoilfriction.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_rootsoilfriction.Name = "textboxRootSoilFriction_" + anchorIndex;
                DockPanel tempPanel1 = new DockPanel();
                tempPanel1.Width = 50;
                CheckBox checkBox_isPassiveAnchor = new CheckBox();
                checkBox_isPassiveAnchor.Width = 75;
                checkBox_isPassiveAnchor.VerticalContentAlignment = VerticalAlignment.Center;
                if (anchor.IsPassiveAnchor) checkBox_isPassiveAnchor.IsChecked = true;
                checkBox_isPassiveAnchor.Checked += CheckBox_passiveanchor_Checked;
                checkBox_isPassiveAnchor.Unchecked += CheckBox_passiveanchor_Unchecked;
                checkBox_isPassiveAnchor.Name = "checkboxPassiveAnchor_" + anchorIndex;
                TextBox textbox_preStressForce = new TextBox();
                textbox_preStressForce.Width = 100;
                textbox_preStressForce.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetForce(anchor.PreStressForce));
                textbox_preStressForce.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_preStressForce.TextChanged += Textbox_prestressforce_TextChanged;
                textbox_preStressForce.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_preStressForce.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_preStressForce.Name = "textboxPreStressForce_" + anchorIndex;
                if (anchor.IsPassiveAnchor) textbox_preStressForce.IsEnabled = false;
                DockPanel tempPanel = new DockPanel();
                tempPanel.Width = 40;
                CheckBox checkBox_soldierBeam = new CheckBox();
                checkBox_soldierBeam.Width = 60;
                checkBox_soldierBeam.VerticalContentAlignment = VerticalAlignment.Center;
                if (anchor.IsSoldierBeam) checkBox_soldierBeam.IsChecked = true;
                checkBox_soldierBeam.Checked += CheckBox_soldierBeam_Checked;
                checkBox_soldierBeam.Unchecked += CheckBox_soldierBeam_Unchecked;
                checkBox_soldierBeam.Name = "checkboxSoldierBeam_" + anchorIndex;
                TextBox textbox_beamHeight = new TextBox();
                textbox_beamHeight.Width = 100;
                textbox_beamHeight.Text = WpfUtils.GetDimension(anchor.SoldierBeamHeight).ToString();
                textbox_beamHeight.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_beamHeight.TextChanged += Textbox_beamheight_TextChanged;
                textbox_beamHeight.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_beamHeight.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_beamHeight.Name = "textboxBeamHeight_" + anchorIndex;
                if (!anchor.IsSoldierBeam) textbox_beamHeight.IsEnabled = false;
                TextBox textbox_beamWidth = new TextBox();
                textbox_beamWidth.Width = 100;
                textbox_beamWidth.Text = WpfUtils.GetDimension(anchor.SoldierBeamwidth).ToString();
                textbox_beamWidth.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_beamWidth.TextChanged += Textbox_beamwidth_TextChanged;
                textbox_beamWidth.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_beamWidth.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_beamWidth.Name = "textboxBeamWidth_" + anchorIndex;
                if (!anchor.IsSoldierBeam) textbox_beamWidth.IsEnabled = false;
                dockPanel.Children.Add(deleteButton);
                dockPanel.Children.Add(textBox_no);
                dockPanel.Children.Add(textbox_depth);
                dockPanel.Children.Add(textbox_freeLength);
                dockPanel.Children.Add(textbox_rootLength);
                dockPanel.Children.Add(textbox_inclination);
                dockPanel.Children.Add(tempPanel2);
                dockPanel.Children.Add(checkBox_centralPlacement);
                dockPanel.Children.Add(textbox_spacing);
                dockPanel.Children.Add(textbox_rootDiameter);
                dockPanel.Children.Add(comboBox_numberofcable);
                dockPanel.Children.Add(comboBox_cableDiameter);                
                dockPanel.Children.Add(textbox_totalNominalArea);
                dockPanel.Children.Add(textbox_breakingStrength);
                dockPanel.Children.Add(textbox_rootModulus);                
                dockPanel.Children.Add(textbox_rootsoilfriction);                
                dockPanel.Children.Add(tempPanel1);                
                dockPanel.Children.Add(checkBox_isPassiveAnchor);                
                dockPanel.Children.Add(textbox_preStressForce);                
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
                    anchor.IsSoldierBeam = false;
                    anchor.SoldierBeamHeight = 0;
                    anchor.SoldierBeamwidth = 0;
                }
                                
                anchorsGroupbox.Children.Add(dockPanel);

                ////refresh windows
                //StaticVariables.view3DPage.Refreshview();
                //StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void CheckBox_centralPlacement_Unchecked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(checkName)].IsCentralPlacement = false;

            AnchorsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void CheckBox_centralPlacement_Checked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(checkName)].IsCentralPlacement = true;

            AnchorsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void Textbox_depth_LostFocus(object sender, RoutedEventArgs e)
        {
            AnchorsGridInitialize();
        }

        private void CheckBox_soldierBeam_Unchecked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(checkName)].IsSoldierBeam = false;

            AnchorsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void CheckBox_soldierBeam_Checked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(checkName)].IsSoldierBeam = true;
            
            AnchorsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        private void CheckBox_passiveanchor_Unchecked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(checkName)].IsPassiveAnchor = false;

            AnchorsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void CheckBox_passiveanchor_Checked(object sender, RoutedEventArgs e)
        {
            string checkName = ((CheckBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(checkName)].IsPassiveAnchor = true;

            AnchorsGridInitialize();
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void TotalNominalAreaCalc(AnchorData anchor)
        {
            int count = 0;
            switch (anchor.NumberofCable)
            {
                case 0:
                    count = 2;
                    break;
                case 1:
                    count = 3;
                    break;
                case 2:
                    count = 4;
                    break;
                case 3:
                    count = 5;
                    break;
                case 4:
                    count = 6;
                    break;
                default:
                    count = 2;
                    break;
            }
            anchor.BreakingStrength = count*anchor.CableData.BreakingStrength;
            anchor.TotalNominalArea = count * anchor.CableData.NominalArea;
            //refresh windows
            StaticVariables.view3DPage.Refreshview();
        }

        private void ComboBox_numberofcable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comboname = ((ComboBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(comboname)].NumberofCable = (((ComboBox)sender).SelectedIndex);
            TotalNominalAreaCalc(StaticVariables.viewModel.anchorDatas[int.Parse(comboname)]);
            AnchorsGridInitialize();
        }
        private void ComboBox_cableDiameter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            string comboname = ((ComboBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(comboname)].CableData = ((WireData)((ComboBox)sender).SelectedItem);
            TotalNominalAreaCalc(StaticVariables.viewModel.anchorDatas[int.Parse(comboname)]);
            AnchorsGridInitialize();
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

        private void Textbox_depth_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].AnchorDepth = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void Textbox_freeLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].FreeLength = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void Textbox_rootLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].RootLength = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void Textbox_inclination_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].Inclination = result;
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void Textbox_spacing_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].Spacing = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void Textbox_rootDiameter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].RootDiameter = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void Textbox_totalnominalarea_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].TotalNominalArea = WpfUtils.GetValueArea(result);                
            }
        }
        private void Textbox_rootmodulus_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].RootModulus = WpfUtils.GetValueStress(result);
            }
        }
        private void Textbox_rootsoilfriction_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].RootSoilFrictionResistance = WpfUtils.GetValueStress(result);
            }
        }
        private void Textbox_prestressforce_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].PreStressForce = WpfUtils.GetValueForce(result);
            }
            //refresh windows
            StaticVariables.SideviewPage.Refreshview();
        }
        private void Textbox_breakingStrength_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].BreakingStrength = WpfUtils.GetForce(result);
            }
        }
        private void Textbox_beamheight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].SoldierBeamHeight = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void Textbox_beamwidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.anchorDatas[int.Parse(textBox.Name.Split('_')[1])].SoldierBeamwidth = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
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
            if(StaticVariables.viewModel.anchorDatas.Count > 0)
            {
                lastDepth = StaticVariables.viewModel.anchorDatas[StaticVariables.viewModel.anchorDatas.Count-1].AnchorDepth ;
                if (lastDepth >= StaticVariables.viewModel.excavationHeight) return;
                lastDepth += 2;
            }
            
            AnchorData anchorData = new AnchorData { AnchorDepth = lastDepth ,FreeLength=7,RootLength=10,RootDiameter=0.15,IsCentralPlacement=false,Spacing=2.4,Inclination=15,CableData=Wire.WireDataList[2], NumberofCable = 2,RootModulus=10000000,PreStressForce=300,IsSoldierBeam=true,SoldierBeamHeight=0.7,SoldierBeamwidth=0.4 ,RootSoilFrictionResistance=65};
            StaticVariables.viewModel.anchorDatas.Add(anchorData);
            TotalNominalAreaCalc(anchorData);
            AnchorsGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void anchorsGridScrollBar_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            headerScrollBar?.ScrollToHorizontalOffset(anchorsGridScrollBar.HorizontalOffset);
        }

        private void useCableData_checkbox_Checked(object sender, RoutedEventArgs e)
        {
           StaticVariables.viewModel.useCableDiameterAndNumberForDesign = true  ;
            foreach (var anchor in StaticVariables.viewModel.anchorDatas)
            {
                TotalNominalAreaCalc(anchor);
            }
           AnchorsGridInitialize();
           
        }

        private void useCableData_checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.useCableDiameterAndNumberForDesign = false;
            AnchorsGridInitialize();
        }
        
    }
    
}
