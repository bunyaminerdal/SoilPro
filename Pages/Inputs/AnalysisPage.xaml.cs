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
            Analysis.WallPartization();
            Analysis.SurchargeToFrameNodes();
            Analysis.WaterLoadToFrameNodes();
            Analysis.EffectiveStressToFrameNodes();
            Analysis.ActivePassiveCoefToFrameNodes();
            StaticVariables.isAnalysisDone = true;
            LoadsAndForcesPre();
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
                    case LoadType.WaterLoad:
                        listitem.Item1.Name =FindResource("HydroStaticWaterPressure").ToString();
                        break;
                    case LoadType.EffectiveStress:
                        listitem.Item1.Name = FindResource("EffectiveStress").ToString();
                        break;
                    case LoadType.SoilSpringCoef:
                        listitem.Item1.Name = FindResource("SubgradeModulusofSoil").ToString();
                        break;
                    default:
                        break;
                }
            }
            var loadandForceDic = FrameData.Frames[0].startNodeLoadAndForce;
            loads_combobox.ItemsSource = loadandForceDic;
            loads_combobox.DisplayMemberPath = "Item1.Name";
            forces_combobox.ItemsSource = loadandForceDic;
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
                var dic = (Tuple<Load, double, double>)comboBox.SelectedItem;
                StaticVariables.loadsAndFocesPage.ShowForce(dic.Item1);
            }
        }
    }
}
