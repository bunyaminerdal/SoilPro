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

namespace ExDesign.Pages.Inputs
{
    /// <summary>
    /// AnalysisPage.xaml etkileşim mantığı
    /// </summary>
    public partial class AnalysisPage : Page
    {
        public AnalysisPage()
        {
            InitializeComponent();
        }

        private void analysis_button_Click(object sender, RoutedEventArgs e)
        {
            double exH_waterH2 =StaticVariables.viewModel.GetexcavationHeight() + (StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH2() : double.MaxValue);
            double exH_calc = WpfUtils.GetExHeightForCalculation();
            Analysis.StageCalculation(exH_waterH2,exH_calc);           
            
            LoadsAndForcesPre();
            //FrameData.FrameSave();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.loadsAndFocesPage;
        }
        private void LoadsAndForcesPre()
        {
            if (!StaticVariables.isAnalysisDone) return;
            foreach (var listitem in FrameData.Frames[0].startNodeLoadAndForce)
            {
                switch (listitem.Item1.Type)
                {
                    case LoadType.SurfaceLoad:
                        break;
                    case LoadType.StripLoad:
                        break;
                    case LoadType.LineLoad:
                        break;
                    case LoadType.PointLoad:
                        break;
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
                    case LoadType.SubgradeModulusofSoil:
                        listitem.Item1.Name = FindResource("SubgradeModulusofSoil").ToString();
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
        }

        private void loads_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if(comboBox.SelectedItem != null)
            {
                var dic =(Tuple<Load,double,double>) comboBox.SelectedItem;
                StaticVariables.loadsAndFocesPage.ShowLoad(dic.Item1);
            }
        }

        private void forces_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                var dic = (Tuple<Load, double, double,double>)comboBox.SelectedItem;
                StaticVariables.loadsAndFocesPage.ShowForce(dic.Item1);
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
        }
    }
}
