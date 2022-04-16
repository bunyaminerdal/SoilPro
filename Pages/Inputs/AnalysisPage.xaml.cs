using System;
using System.Collections.Generic;
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
using ExDesign.Scripts;
using ExDesign.Datas;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ExDesign.Pages.Inputs
{
    /// <summary>
    /// AnalysisPage.xaml etkileşim mantığı
    /// </summary>
    public partial class AnalysisPage : Page
    {
        public char separator = ',';

        bool isShowValues = true;
        bool isBackForceStartWithK0 = false;
        bool isSpringOpenWithK0 = false;
        int iterationCount = 0;
        public AnalysisPage()
        {
            InitializeComponent();
        }

        private void analysis_button_Click(object sender, RoutedEventArgs e)
        {
            double exH_waterH2 =StaticVariables.viewModel.GetexcavationHeight() + (StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH2() : double.MaxValue);
            double exH_calc = WpfUtils.GetExHeightForCalculation();
            Analysis.StageCalculation(exH_waterH2,exH_calc,isBackForceStartWithK0, isSpringOpenWithK0, iterationCount);
            
            LoadsAndForcesPre();
            //FrameData.FrameSave();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.loadsAndFocesPage;
            showValues.IsChecked = true;
            iterationCount_tb.Text = iterationCount.ToString();
        }
        private void LoadsAndForcesPre()
        {
            if (!StaticVariables.isAnalysisDone) return;
            foreach (var listitem in FrameData.Frames[0].startNodeLoadAndForce)
            {
                switch (listitem.Item1.Type)
                {                    
                    case LoadType.Back_WaterPressure:
                        listitem.Item1.Name = FindResource("BackWaterPressure").ToString();
                        break;
                    case LoadType.Front_WaterPressure:
                        listitem.Item1.Name = FindResource("FrontWaterPressure").ToString();
                        break;
                    case LoadType.HydroStaticWaterPressure:
                        listitem.Item1.Name =FindResource("HydroStaticWaterPressure").ToString();
                        break;
                    case LoadType.Back_TotalStress:
                        listitem.Item1.Name = FindResource("BackTotalStress").ToString();
                        break;
                    case LoadType.Front_TotalStress:
                        listitem.Item1.Name = FindResource("FrontTotalStress").ToString();
                        break;
                    case LoadType.Back_EffectiveStress:
                        listitem.Item1.Name = FindResource("BackEffectiveStress").ToString();
                        break;
                    case LoadType.Front_EffectiveStress:
                        listitem.Item1.Name = FindResource("FrontEffectiveStress").ToString();
                        break;
                    case LoadType.Back_SubgradeModulusofSoil:
                        listitem.Item1.Name = FindResource("BackSubgradeModulusofSoil").ToString();
                        break;
                    case LoadType.Front_SubgradeModulusofSoil:
                        listitem.Item1.Name = FindResource("FrontSubgradeModulusofSoil").ToString();
                        break;
                    case LoadType.Back_Active_Horizontal_Force:
                        listitem.Item1.Name = FindResource("BackActiveHorizontalForce").ToString();
                        break;
                    case LoadType.Back_Passive_Horizontal_Force:
                        listitem.Item1.Name = FindResource("BackPassiveHorizontalForce").ToString();
                        break;
                    case LoadType.Back_Active_Vertical_Force:
                        listitem.Item1.Name = FindResource("BackActiveVerticalForce").ToString();
                        break;
                    case LoadType.Back_Passive_Vertical_Force:
                        listitem.Item1.Name = FindResource("BackPassiveVerticalForce").ToString();
                        break;
                    case LoadType.Front_Active_Horizontal_Force:
                        listitem.Item1.Name = FindResource("FrontActiveHorizontalForce").ToString();
                        break;
                    case LoadType.Front_Passive_Horizontal_Force:
                        listitem.Item1.Name = FindResource("FrontPassiveHorizontalForce").ToString();
                        break;
                    case LoadType.Front_Active_Vertical_Force:
                        listitem.Item1.Name = FindResource("FrontActiveVerticalForce").ToString();
                        break;
                    case LoadType.Front_Passive_Vertical_Force:
                        listitem.Item1.Name = FindResource("FrontPassiveVerticalForce").ToString();
                        break;
                    case LoadType.Front_Rest_Horizontal_Force:
                        listitem.Item1.Name = FindResource("FrontRestHorizontalForce").ToString();
                        break;
                    case LoadType.Back_Rest_Horizontal_Force:
                        listitem.Item1.Name = FindResource("BackRestHorizontalForce").ToString();
                        break;
                    default:
                        break;
                }
            }
            foreach (var listitem in FrameData.Frames[0].startNodeActivePassiveCoef_S_P_N)
            {
                switch (listitem.Item1.Type)
                {                    
                    case LoadType.Front_Kactive:
                        listitem.Item1.Name = FindResource("FrontKactive").ToString();
                        break;
                    case LoadType.Back_Kactive:
                        listitem.Item1.Name = FindResource("BackKactive").ToString();
                        break;
                    case LoadType.Front_Kpassive:
                        listitem.Item1.Name = FindResource("FrontKpassive").ToString();
                        break;
                    case LoadType.Back_Kpassive:
                        listitem.Item1.Name = FindResource("BackKpassive").ToString();
                        break;
                    case LoadType.Front_Krest:
                        listitem.Item1.Name = FindResource("FrontKrest").ToString();
                        break;
                    case LoadType.Back_Krest:
                        listitem.Item1.Name = FindResource("BackKrest").ToString();
                        break;
                    default:
                        break;
                }
            }
            int rotationCount = 0;
            int displacementCount = 0;
            int backforceCount = 0;
            int frontforceCount = 0;
            int totalforceCount = 0;
            int springCount = 0;
            foreach (var listitem in NodeData.Nodes[0].nodeForce)
            {                
                switch (listitem.Item1.Type)
                {
                    case LoadType.HydroStaticWaterPressure:
                        listitem.Item1.Name = FindResource("HydroStaticWaterPressure").ToString();
                        break;
                    case LoadType.Back_SubgradeModulusofSoil:
                        listitem.Item1.Name = FindResource("BackSubgradeModulusofSoil").ToString();
                        break;
                    case LoadType.Front_SubgradeModulusofSoil:
                        listitem.Item1.Name = FindResource("FrontSubgradeModulusofSoil").ToString();
                        break;
                    case LoadType.Analys_SubgradeModulusofSoil:
                        springCount =springCount + 1;
                        listitem.Item1.Name = FindResource("AnalysSubgradeModulusofSoil").ToString()+"_"+springCount.ToString();
                        break;                    
                    case LoadType.Back_Active_Horizontal_Force:
                        listitem.Item1.Name = FindResource("BackActiveHorizontalForce").ToString();
                        break;
                    case LoadType.Back_Passive_Horizontal_Force:
                        listitem.Item1.Name = FindResource("BackPassiveHorizontalForce").ToString();
                        break;
                    case LoadType.Back_Active_Vertical_Force:
                        listitem.Item1.Name = FindResource("BackActiveVerticalForce").ToString();
                        break;
                    case LoadType.Back_Passive_Vertical_Force:
                        listitem.Item1.Name = FindResource("BackPassiveVerticalForce").ToString();
                        break;
                    case LoadType.Front_Active_Horizontal_Force:
                        listitem.Item1.Name = FindResource("FrontActiveHorizontalForce").ToString();
                        break;
                    case LoadType.Front_Passive_Horizontal_Force:
                        listitem.Item1.Name = FindResource("FrontPassiveHorizontalForce").ToString();
                        break;
                    case LoadType.Front_Active_Vertical_Force:
                        listitem.Item1.Name = FindResource("FrontActiveVerticalForce").ToString();
                        break;
                    case LoadType.Front_Passive_Vertical_Force:
                        listitem.Item1.Name = FindResource("FrontPassiveVerticalForce").ToString();
                        break;
                    case LoadType.Front_Rest_Horizontal_Force:
                        listitem.Item1.Name = FindResource("FrontRestHorizontalForce").ToString();
                        break;
                    case LoadType.Back_Rest_Horizontal_Force:
                        listitem.Item1.Name = FindResource("BackRestHorizontalForce").ToString();
                        break;
                    case LoadType.First_Total_Force:
                        totalforceCount++;
                        listitem.Item1.Name = FindResource("FirstTotalForce").ToString()+"_" + totalforceCount.ToString();
                        break;                    
                    case LoadType.Back_First_Total_Force:
                        backforceCount++;
                        listitem.Item1.Name = FindResource("BackFirstTotalForce").ToString() + "_" + backforceCount.ToString();
                        break;
                    case LoadType.Front_First_Total_Force:
                        frontforceCount++;
                        listitem.Item1.Name = FindResource("FrontFirstTotalForce").ToString() + "_" + frontforceCount.ToString();
                        break;                    
                    case LoadType.First_Displacement:
                        displacementCount++;
                        listitem.Item1.Name = FindResource("FirstDisplacement").ToString() + "_" + displacementCount.ToString();
                        break;
                    case LoadType.First_Rotation:
                        rotationCount++;
                        listitem.Item1.Name = FindResource("FirstRotation").ToString() + "_" + rotationCount.ToString();
                        break;                    
                    default:
                        break;
                }
            }

            var loadandForceDic = FrameData.Frames[0].startNodeLoadAndForce;            
            loads_combobox.ItemsSource = loadandForceDic;
            loads_combobox.DisplayMemberPath = "Item1.Name";

            var loadandForceDic1 = FrameData.Frames[0].startNodeActivePassiveCoef_S_P_N;
            forces_combobox.ItemsSource = loadandForceDic1;
            forces_combobox.DisplayMemberPath = "Item1.Name";

            var loadandForceDic2 = NodeData.Nodes[0].nodeForce;
            nodeforces_combobox.ItemsSource = loadandForceDic2;
            nodeforces_combobox.DisplayMemberPath = "Item1.Name";
        }

        private void loads_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if(comboBox.SelectedItem != null)
            {
                var dic =(Tuple<Load,double,double>) comboBox.SelectedItem;
                StaticVariables.loadsAndFocesPage.ShowLoad(dic.Item1);
                if (isShowValues) textBlockFillerLoad(dic.Item1);
            }
        }

        private void forces_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                var dic = (Tuple<Load, double, double,double>)comboBox.SelectedItem;
                StaticVariables.loadsAndFocesPage.ShowForce(dic.Item1);
                if (isShowValues) textBlockFillerCoef(dic.Item1);

            }
        }
        private void nodeforces_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                var dic = (Tuple<Load, double>)comboBox.SelectedItem;
                StaticVariables.loadsAndFocesPage.ShowNodeForce(dic.Item1);
                if(isShowValues) textBlockFillerNodeForce(dic.Item1);

            }
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
        }

        private void textBlockFillerLoad(Load load)
        {
            textblock_start.Text = "";
            textblock_end.Text = "";
            foreach (var frame in FrameData.Frames)
            {
                var startLoad = frame.startNodeLoadAndForce.Find(x => x.Item1.ID == load.ID);
                textblock_start.Text +=  startLoad.Item2.ToString() + "\n" ;
                var endLoad = frame.endNodeLoadAndForce.Find(x => x.Item1.ID == load.ID);
                textblock_end.Text +=  endLoad.Item2.ToString()+ "\n" ;

            }
        }
        private void textBlockFillerCoef(Load load)
        {
            textblock_start.Text = "";
            textblock_end.Text = "";
            foreach (var frame in FrameData.Frames)
            {
                var startLoad = frame.startNodeActivePassiveCoef_S_P_N.Find(x => x.Item1.ID == load.ID);
                textblock_start.Text +=  startLoad.Item2.ToString() + "\n";
                var endLoad = frame.endNodeActivePassiveCoef_S_P_N.Find(x => x.Item1.ID == load.ID);
                textblock_end.Text +=  endLoad.Item2.ToString() + "\n";

            }
        }
        private void textBlockFillerNodeForce(Load load)
        {
            textblock_start.Text = "";
            textblock_end.Text = "";
            foreach (var node in NodeData.Nodes)
            {
                var startLoad = node.nodeForce.Find(x => x.Item1.ID == load.ID);
                textblock_start.Text +=  startLoad.Item2.ToString() + "\n";
            }
        }

        private void showValues_Unchecked(object sender, RoutedEventArgs e)
        {
            textblock_start.Visibility = Visibility.Collapsed;
            textblock_end.Visibility = Visibility.Collapsed;
        }

        private void showValues_Checked(object sender, RoutedEventArgs e)
        {
            textblock_start.Visibility = Visibility.Visible;
            textblock_end.Visibility = Visibility.Visible;
        }

        private void isPlussedSprings_cb_Checked(object sender, RoutedEventArgs e)
        {
            isSpringOpenWithK0 = true;
        }

        private void isPlussedSprings_cb_Unchecked(object sender, RoutedEventArgs e)
        {
            isSpringOpenWithK0 = false;
        }

        private void isUsedFirstForces_cb_Checked(object sender, RoutedEventArgs e)
        {
            isBackForceStartWithK0 = true;
        }

        private void isUsedFirstForces_cb_Unchecked(object sender, RoutedEventArgs e)
        {
            isBackForceStartWithK0 = false;
        }

        private void iterationCount_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (int.TryParse(textBox.Text, out int result))
            {
                iterationCount = result;
            }

        }

        private void iterationCount_tb_TextInput(object sender, TextCompositionEventArgs e)
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

        private void iterationCount_tb_PreviewKeyDown(object sender, KeyEventArgs e)
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
    }
}
