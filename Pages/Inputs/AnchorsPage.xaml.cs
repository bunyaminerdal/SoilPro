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

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;

            StaticEvents.UnitChangeEvent += UnitChange;

            AnchorsGridInitialize();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            StaticVariables.viewModel.anchorDatas.RemoveAt(int.Parse(buttonName.Split('_')[1]));
            AnchorsGridInitialize();
        }

        public void AnchorsGridInitialize()
        {
            if (StaticVariables.viewModel.anchorDatas == null) return;
            anchorsGroupbox.Children.Clear();
            foreach (var item in StaticVariables.viewModel.anchorDatas)
            {
                string anchorIndex = StaticVariables.viewModel.anchorDatas.IndexOf(item).ToString();
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
                textBox_no.Text = anchorIndex;
                textBox_no.VerticalContentAlignment = VerticalAlignment.Center;
                textBox_no.HorizontalContentAlignment = HorizontalAlignment.Center;
                textBox_no.IsEnabled = false;
                TextBox textbox_depth = new TextBox();
                textbox_depth.Width = 80;
                textbox_depth.Text = WpfUtils.GetDimension(item.AnchorDepth).ToString();
                textbox_depth.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_depth.TextChanged += Textbox_depth_TextChanged;
                textbox_depth.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_depth.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_depth.Name = "textboxdepth_" + anchorIndex;
                TextBox textbox_freeLength = new TextBox();
                textbox_freeLength.Width = 80;
                textbox_freeLength.Text = WpfUtils.GetDimension(item.FreeLength).ToString();
                textbox_freeLength.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_freeLength.TextChanged += Textbox_freeLength_TextChanged;
                textbox_freeLength.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_freeLength.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_freeLength.Name = "textboxFreeLength_" + anchorIndex;
                TextBox textbox_rootLength = new TextBox();
                textbox_rootLength.Width = 80;
                textbox_rootLength.Text = WpfUtils.GetDimension(item.RootLength).ToString();
                textbox_rootLength.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_rootLength.TextChanged += Textbox_rootLength_TextChanged;
                textbox_rootLength.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_rootLength.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_rootLength.Name = "textboxRootLength_" + anchorIndex;
                TextBox textbox_inclination = new TextBox();
                textbox_inclination.Width = 80;
                textbox_inclination.Text = item.Inclination.ToString();
                textbox_inclination.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_inclination.TextChanged += Textbox_inclination_TextChanged;
                textbox_inclination.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_inclination.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_inclination.Name = "textboxinclination_" + anchorIndex;
                TextBox textbox_spacing = new TextBox();
                textbox_spacing.Width = 80;
                textbox_spacing.Text = WpfUtils.GetDimension(item.Spacing).ToString();
                textbox_spacing.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_spacing.TextChanged += Textbox_spacing_TextChanged;
                textbox_spacing.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_spacing.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_spacing.Name = "textboxSpacing_" + anchorIndex;
                TextBox textbox_rootDiameter = new TextBox();
                textbox_rootDiameter.Width = 100;
                textbox_rootDiameter.Text = WpfUtils.GetDimension(item.RootDiameter).ToString();
                textbox_rootDiameter.VerticalContentAlignment = VerticalAlignment.Center;
                textbox_rootDiameter.TextChanged += Textbox_rootDiameter_TextChanged;
                textbox_rootDiameter.PreviewKeyDown += Textbox_depth_PreviewKeyDown;
                textbox_rootDiameter.PreviewTextInput += Textbox_depth_PreviewTextInput;
                textbox_rootDiameter.Name = "textboxRootDiameter_" + anchorIndex;
                ComboBox comboBox_numberofcable = new ComboBox();
                comboBox_numberofcable.Width = 100;
                comboBox_numberofcable.Items.Add("2");
                comboBox_numberofcable.Items.Add("3");
                comboBox_numberofcable.Items.Add("4");
                comboBox_numberofcable.Items.Add("5");
                comboBox_numberofcable.Items.Add("6");
                comboBox_numberofcable.Name = "comboNumberofCable_" + anchorIndex;
                comboBox_numberofcable.SelectedIndex = item.NumberofCable;
                comboBox_numberofcable.SelectionChanged += ComboBox_numberofcable_SelectionChanged;
                comboBox_numberofcable.VerticalContentAlignment = VerticalAlignment.Center;
                ComboBox comboBox_cableDiameter = new ComboBox();
                comboBox_cableDiameter.Width = 100;
                comboBox_cableDiameter.Items.Add("3/8 in");
                comboBox_cableDiameter.Items.Add("7/16 in");
                comboBox_cableDiameter.Items.Add("1/2 in");
                comboBox_cableDiameter.Items.Add("0.6 in");
                comboBox_cableDiameter.Name = "comboCableDiameter_" + anchorIndex;
                comboBox_cableDiameter.SelectedIndex = item.CableDiameter;
                comboBox_cableDiameter.SelectionChanged += ComboBox_cableDiameter_SelectionChanged;
                comboBox_cableDiameter.VerticalContentAlignment = VerticalAlignment.Center;

                dockPanel.Children.Add(deleteButton);
                dockPanel.Children.Add(textBox_no);
                dockPanel.Children.Add(textbox_depth);
                dockPanel.Children.Add(textbox_freeLength);
                dockPanel.Children.Add(textbox_rootLength);
                dockPanel.Children.Add(textbox_inclination);
                dockPanel.Children.Add(textbox_spacing);
                dockPanel.Children.Add(textbox_rootDiameter);
                dockPanel.Children.Add(comboBox_numberofcable);                
                dockPanel.Children.Add(comboBox_cableDiameter);                
                anchorsGroupbox.Children.Add(dockPanel);

                //refresh windows
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }
        private void ComboBox_numberofcable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comboname = ((ComboBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(comboname)].NumberofCable = (((ComboBox)sender).SelectedIndex);

            AnchorsGridInitialize();
        }
        private void ComboBox_cableDiameter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comboname = ((ComboBox)sender).Name.Split('_')[1];
            StaticVariables.viewModel.anchorDatas[int.Parse(comboname)].CableDiameter = (((ComboBox)sender).SelectedIndex);

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
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
            StaticEvents.UnitChangeEvent -= UnitChange;
        }

        private void addanchor_bttn_Click(object sender, RoutedEventArgs e)
        {
            AnchorData anchorData = new AnchorData { AnchorDepth = 3 };
            StaticVariables.viewModel.anchorDatas.Add(anchorData);
            AnchorsGridInitialize();
        }

        private void anchorsGridScrollBar_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            headerScrollBar?.ScrollToHorizontalOffset(anchorsGridScrollBar.HorizontalOffset);
        }
    }
}
