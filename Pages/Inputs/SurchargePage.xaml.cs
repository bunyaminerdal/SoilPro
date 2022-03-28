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
    /// SurchargePage.xaml etkileşim mantığı
    /// </summary>
    public partial class SurchargePage : Page
    {
        public char separator = ',';
        public SurchargePage()
        {
            InitializeComponent();
        }
        private void UnitChange()
        {

            LoadGridInitialize();
            surfaceLoad_txtbox.Text = FindResource("Load").ToString() + " (" + StaticVariables.StressUnit + ")";
            stripdistance_txtbox.Text = FindResource("DistanceFromWall").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            Linedistance_txtbox.Text = FindResource("DistanceFromWall").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            pointdistance_txtbox.Text = FindResource("DistanceFromWall").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            stripLength_txtbox.Text = FindResource("StripLength").ToString() + " (" + StaticVariables.dimensionUnit + ")";
            stripStartLoad_txtbox.Text = FindResource("StartLoad").ToString() + " (" + StaticVariables.StressUnit + ")";
            stripEndLoad_txtbox.Text = FindResource("EndLoad").ToString() + " (" + StaticVariables.StressUnit + ")";
            LineLoad_txtbox.Text = FindResource("LineLoad").ToString() + " (" + StaticVariables.SurfaceStressUnit + ")";
            pointLoad_txtbox.Text = FindResource("PointLoad").ToString() + " (" + StaticVariables.forceUnit + ")";
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
                        
            StaticEvents.UnitChangeEvent += UnitChange;
            UnitChange();
            LoadGridInitialize();
        }
       
        

        
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
            StaticEvents.UnitChangeEvent -= UnitChange;
        }

        private void surfaceSurcharge_bttn_Click(object sender, RoutedEventArgs e)
        {
            SurfaceSurchargeData surfaceSurchargeData = new SurfaceSurchargeData() { SurchargeName="Surface Load 1",Load=10};
            StaticVariables.viewModel.surfaceSurchargeDatas.Add(surfaceSurchargeData);
            LoadGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void stripSurcharge_bttn_Click(object sender, RoutedEventArgs e)
        {
            StripLoadData stripLoadData = new StripLoadData() { SurchargeName="Strip Load 1",DistanceFromWall=1,StripLength=2,StartLoad=11,EndLoad=13};
            StaticVariables.viewModel.stripLoadDatas.Add(stripLoadData);
            LoadGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void LineSurcharge_bttn_Click(object sender, RoutedEventArgs e)
        {
            LineLoadData lineLoadData = new LineLoadData();
            StaticVariables.viewModel.LineLoadDatas.Add(lineLoadData);
            LoadGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void pointSurcharge_bttn_Click(object sender, RoutedEventArgs e)
        {
            PointLoadData pointLoadData = new PointLoadData();
            StaticVariables.viewModel.PointLoadDatas.Add(pointLoadData);
            LoadGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void LoadGridInitialize()
        {
            //surface Surcharge
            if (StaticVariables.viewModel.surfaceSurchargeDatas != null)
            {
                surfaceSurchargeGroupbox.Children.Clear();

                foreach (var surfaceLoad in StaticVariables.viewModel.surfaceSurchargeDatas)
                {
                    string surfaceLoadIndex = StaticVariables.viewModel.surfaceSurchargeDatas.IndexOf(surfaceLoad).ToString();
                    DockPanel dockPanel = new DockPanel();
                    dockPanel.Margin = new Thickness(2);
                    dockPanel.HorizontalAlignment = HorizontalAlignment.Left;
                    Button deleteSurfaceButton = new Button();
                    deleteSurfaceButton.Padding = new Thickness(2);
                    deleteSurfaceButton.Height = 27;
                    deleteSurfaceButton.Width = 27;
                    Image deletebuttonImage = new Image();
                    deletebuttonImage.Source = new BitmapImage(new Uri("/Textures/Icons/trash.png", UriKind.RelativeOrAbsolute));
                    deleteSurfaceButton.Content = deletebuttonImage;
                    deleteSurfaceButton.Name = "delete_" + surfaceLoadIndex;
                    deleteSurfaceButton.Click += DeleteSurfaceButton_Click;
                    TextBox textBox_no = new TextBox();
                    textBox_no.Width = 30;
                    textBox_no.Text = (int.Parse(surfaceLoadIndex) + 1).ToString();
                    textBox_no.VerticalContentAlignment = VerticalAlignment.Center;
                    textBox_no.HorizontalContentAlignment = HorizontalAlignment.Center;
                    textBox_no.IsEnabled = false;
                    TextBox textbox_SurfaceName = new TextBox();
                    textbox_SurfaceName.Width = 150;
                    textbox_SurfaceName.MaxLength = 50;
                    textbox_SurfaceName.Text = surfaceLoad.SurchargeName.ToString();
                    textbox_SurfaceName.VerticalContentAlignment = VerticalAlignment.Center;
                    textbox_SurfaceName.TextChanged += Textbox_SurfaceName_TextChanged;
                    textbox_SurfaceName.PreviewKeyDown += Textbox_SurfaceLoad_PreviewKeyDown;
                    textbox_SurfaceName.PreviewTextInput += Textbox_SurfaceLoad_PreviewTextInput;
                    textbox_SurfaceName.Name = "textboxsurfaceName_" + surfaceLoadIndex;
                    TextBox textbox_SurfaceLoad = new TextBox();
                    textbox_SurfaceLoad.Width = 100;
                    textbox_SurfaceLoad.Text = WpfUtils.GetStress(surfaceLoad.Load).ToString();
                    textbox_SurfaceLoad.VerticalContentAlignment = VerticalAlignment.Center;
                    textbox_SurfaceLoad.TextChanged += Textbox_SurfaceLoad_TextChanged;
                    textbox_SurfaceLoad.PreviewKeyDown += Textbox_SurfaceLoad_PreviewKeyDown;
                    textbox_SurfaceLoad.PreviewTextInput += Textbox_SurfaceLoad_PreviewTextInput;
                    textbox_SurfaceLoad.Name = "textboxsurfaceLoad_" + surfaceLoadIndex;

                    dockPanel.Children.Add(deleteSurfaceButton);
                    dockPanel.Children.Add(textBox_no);
                    dockPanel.Children.Add(textbox_SurfaceName);
                    dockPanel.Children.Add(textbox_SurfaceLoad);

                    surfaceSurchargeGroupbox.Children.Add(dockPanel);

                }

            }
            //strip Surcharge
            if (StaticVariables.viewModel.stripLoadDatas != null)
            {
                stripSurchargeGroupbox.Children.Clear();

                foreach (var stripLoad in StaticVariables.viewModel.stripLoadDatas)
                {
                    string stripLoadIndex = StaticVariables.viewModel.stripLoadDatas.IndexOf(stripLoad).ToString();
                    DockPanel dockPanel = new DockPanel();
                    dockPanel.Margin = new Thickness(2);
                    dockPanel.HorizontalAlignment = HorizontalAlignment.Left;
                    Button deleteStripButton = new Button();
                    deleteStripButton.Padding = new Thickness(2);
                    deleteStripButton.Height = 27;
                    deleteStripButton.Width = 27;
                    Image deletebuttonImage = new Image();
                    deletebuttonImage.Source = new BitmapImage(new Uri("/Textures/Icons/trash.png", UriKind.RelativeOrAbsolute));
                    deleteStripButton.Content = deletebuttonImage;
                    deleteStripButton.Name = "delete_" + stripLoadIndex;
                    deleteStripButton.Click += DeleteStripButton_Click;
                    TextBox textBox_no = new TextBox();
                    textBox_no.Width = 30;
                    textBox_no.Text = (int.Parse(stripLoadIndex) + 1).ToString();
                    textBox_no.VerticalContentAlignment = VerticalAlignment.Center;
                    textBox_no.HorizontalContentAlignment = HorizontalAlignment.Center;
                    textBox_no.IsEnabled = false;
                    TextBox textbox_StripName = new TextBox();
                    textbox_StripName.Width = 150;
                    textbox_StripName.MaxLength = 50;
                    textbox_StripName.Text = stripLoad.SurchargeName.ToString();
                    textbox_StripName.VerticalContentAlignment = VerticalAlignment.Center;
                    textbox_StripName.TextChanged += Textbox_SurfaceName_TextChanged;
                    textbox_StripName.PreviewKeyDown += Textbox_SurfaceLoad_PreviewKeyDown;
                    textbox_StripName.PreviewTextInput += Textbox_SurfaceLoad_PreviewTextInput;
                    textbox_StripName.Name = "textboxStripName_" + stripLoadIndex;
                    TextBox textbox_Distance = new TextBox();
                    textbox_Distance.Width = 100;
                    textbox_Distance.Text = WpfUtils.GetDimension(stripLoad.DistanceFromWall).ToString();
                    textbox_Distance.VerticalContentAlignment = VerticalAlignment.Center;
                    textbox_Distance.TextChanged += Textbox_Distance_TextChanged;
                    textbox_Distance.PreviewKeyDown += Textbox_SurfaceLoad_PreviewKeyDown;
                    textbox_Distance.PreviewTextInput += Textbox_SurfaceLoad_PreviewTextInput;
                    textbox_Distance.Name = "textboxdistance_" + stripLoadIndex;
                    TextBox textbox_Length = new TextBox();
                    textbox_Length.Width = 100;
                    textbox_Length.Text = WpfUtils.GetDimension(stripLoad.StripLength).ToString();
                    textbox_Length.VerticalContentAlignment = VerticalAlignment.Center;
                    textbox_Length.TextChanged += Textbox_Length_TextChanged;
                    textbox_Length.PreviewKeyDown += Textbox_SurfaceLoad_PreviewKeyDown;
                    textbox_Length.PreviewTextInput += Textbox_SurfaceLoad_PreviewTextInput;
                    textbox_Length.Name = "textboxLength_" + stripLoadIndex;
                    TextBox textbox_StartLoad = new TextBox();
                    textbox_StartLoad.Width = 100;
                    textbox_StartLoad.Text = WpfUtils.GetStress(stripLoad.StartLoad).ToString();
                    textbox_StartLoad.VerticalContentAlignment = VerticalAlignment.Center;
                    textbox_StartLoad.TextChanged += Textbox_StartLoad_TextChanged;
                    textbox_StartLoad.PreviewKeyDown += Textbox_SurfaceLoad_PreviewKeyDown;
                    textbox_StartLoad.PreviewTextInput += Textbox_SurfaceLoad_PreviewTextInput;
                    textbox_StartLoad.Name = "textboxstartLoad_" + stripLoadIndex;
                    TextBox textbox_EndLoad = new TextBox();
                    textbox_EndLoad.Width = 100;
                    textbox_EndLoad.Text = WpfUtils.GetStress(stripLoad.EndLoad).ToString();
                    textbox_EndLoad.VerticalContentAlignment = VerticalAlignment.Center;
                    textbox_EndLoad.TextChanged += Textbox_EndLoad_TextChanged;
                    textbox_EndLoad.PreviewKeyDown += Textbox_SurfaceLoad_PreviewKeyDown;
                    textbox_EndLoad.PreviewTextInput += Textbox_SurfaceLoad_PreviewTextInput;
                    textbox_EndLoad.Name = "textboxstartLoad_" + stripLoadIndex;
                    dockPanel.Children.Add(deleteStripButton);
                    dockPanel.Children.Add(textBox_no);
                    dockPanel.Children.Add(textbox_StripName);
                    dockPanel.Children.Add(textbox_Distance);
                    dockPanel.Children.Add(textbox_Length);
                    dockPanel.Children.Add(textbox_StartLoad);
                    dockPanel.Children.Add(textbox_EndLoad);

                    stripSurchargeGroupbox.Children.Add(dockPanel);

                }

            }
        }

        private void Textbox_EndLoad_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.stripLoadDatas[int.Parse(textBox.Name.Split('_')[1])].EndLoad = WpfUtils.GetValueStress(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void Textbox_StartLoad_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.stripLoadDatas[int.Parse(textBox.Name.Split('_')[1])].StartLoad = WpfUtils.GetValueStress(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void Textbox_Length_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.stripLoadDatas[int.Parse(textBox.Name.Split('_')[1])].StripLength = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void Textbox_Distance_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.stripLoadDatas[int.Parse(textBox.Name.Split('_')[1])].DistanceFromWall = WpfUtils.GetValueDimension(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void DeleteStripButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            StaticVariables.viewModel.stripLoadDatas.RemoveAt(int.Parse(buttonName.Split('_')[1]));
            LoadGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void Textbox_SurfaceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            StaticVariables.viewModel.surfaceSurchargeDatas[int.Parse(textBox.Name.Split('_')[1])].SurchargeName = textBox.Text;
        }

        private void Textbox_SurfaceLoad_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.surfaceSurchargeDatas[int.Parse(textBox.Name.Split('_')[1])].Load = WpfUtils.GetValueStress(result);
                StaticVariables.view3DPage.Refreshview();
                StaticVariables.SideviewPage.Refreshview();
            }
        }

        private void Textbox_SurfaceLoad_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Textbox_SurfaceLoad_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void DeleteSurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            StaticVariables.viewModel.surfaceSurchargeDatas.RemoveAt(int.Parse(buttonName.Split('_')[1]));
            LoadGridInitialize();
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        
    }
}
