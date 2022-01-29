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
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExDesign.Scripts;
using System.Globalization;

namespace ExDesign.Pages.Inputs.Views
{
    /// <summary>
    /// SideviewPage.xaml etkileşim mantığı
    /// </summary>
    public partial class SideviewPage : Page
    {
        double scaleFactor = 15;
        double minScaleFactor = 8;
        double maxScaleFactor = 50;
        Point center = new Point(0, 0);
        double wall_t = 0.65;
        double wall_h = 12;
        double wall_d = 7;
        double pile_s = 0.80;
        double frontandbackCubeLength = 15;
        double excavationHeight = 8;
        double frontT_Z = 2;
        double frontT_X1 = 2;
        double frontT_X2 = 2;
        double backT_Beta = 10;
        double backT_B = 1;
        double backT_A1 = 2;
        double backT_A2 = 2;
        double bottomT_h = 3;
        double groundW_h1 = 5;
        double groundW_h2 = 2;
        double capBeam_h = 0.8;
        double capBeam_b = 0.65;
        int sheetIndex = 0;
        public SideviewPage()
        {
            InitializeComponent();
            StartViewModel2D();
        }
        public void SetViewModel()
        {
            center = new Point( StaticVariables.viewModel.center3d.X,StaticVariables.viewModel.center3d.Y);
            wall_h = StaticVariables.viewModel.wall_h;
            wall_t = StaticVariables.viewModel.wall_t;
            pile_s = StaticVariables.viewModel.pile_s;
            frontandbackCubeLength = StaticVariables.viewModel.frontandbackCubeLength;
            excavationHeight = StaticVariables.viewModel.excavationHeight;
            frontT_Z = StaticVariables.viewModel.frontT_Z;
            frontT_X1 = StaticVariables.viewModel.frontT_X1;
            frontT_X2 = StaticVariables.viewModel.frontT_X2;
            backT_Beta = StaticVariables.viewModel.backT_Beta;
            backT_B = StaticVariables.viewModel.backT_B;
            backT_A1 = StaticVariables.viewModel.backT_A1;
            backT_A2 = StaticVariables.viewModel.backT_A2;
            bottomT_h = StaticVariables.viewModel.bottomT_h;
            groundW_h1 = StaticVariables.viewModel.groundW_h1;
            groundW_h2 = StaticVariables.viewModel.groundW_h2;
            capBeam_b = StaticVariables.viewModel.capBeam_b;
            capBeam_h = StaticVariables.viewModel.capBeam_h;
            sheetIndex = StaticVariables.viewModel.SheetIndex;
        }

        private void StartViewModel2D()
        {
            SetViewModel();
            DrawingGroup mainDrawingGroup = new DrawingGroup();            
            Uri soilUri = new Uri(@"Textures/Soil/soil2.png", UriKind.Relative);
            GeometryDrawing wallGeometryDrawing = Wpf2Dutils.WallGeometryDrawing(center,wall_h,wall_t,Colors.DarkGray);
            Point bottomSoilCenter = new Point(center.X-frontandbackCubeLength,center.Y+wall_h);
            GeometryDrawing bottomSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(bottomSoilCenter, bottomT_h, wall_t + frontandbackCubeLength * 2, Colors.Gray, soilUri, true);

            switch (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    break;
                case WallType.ConcretePileWall:
                    Point capBeamCenter = new Point(center.X+wall_t/2-capBeam_b/2 , center.Y-capBeam_h );
                    GeometryDrawing capBeamGeometryDrawing = Wpf2Dutils.WallGeometryDrawing(capBeamCenter, capBeam_h, capBeam_b, Colors.DarkGray);
                    mainDrawingGroup.Children.Add(capBeamGeometryDrawing);
                    break;
                case WallType.SteelSheetWall:
                    break;
                default:
                    break;
            }
            double frontT_w_top_dis = 0;
            double frontT_w_bottom_dis = 0;
            double frontT_h = 0;
            double frontT_w_bottom = 0;
            double frontT_w_top = 0;
            Point frontSoilCenter = new Point(center.X - frontandbackCubeLength, center.Y + excavationHeight);
            GeometryDrawing frontSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(frontSoilCenter, wall_h - excavationHeight, frontandbackCubeLength, frontT_h, frontT_w_top, frontT_w_bottom, frontT_w_top_dis, frontT_w_bottom_dis, Colors.Gray, soilUri, true);

            switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
            {
                case ExcavationType.none:
                    break;
                case ExcavationType.type1:
                    frontT_w_top_dis = frontandbackCubeLength - frontT_X2;
                    frontT_w_bottom_dis = frontandbackCubeLength - frontT_X1 - frontT_X2;
                    frontT_h = frontT_Z;
                    frontT_w_bottom = frontT_X1 + frontT_X2;
                    frontT_w_top = frontT_X2;
                    frontSoilCenter = new Point(center.X - frontandbackCubeLength, center.Y + excavationHeight-frontT_h);
                    frontSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(frontSoilCenter, wall_h - excavationHeight, frontandbackCubeLength, frontT_h, frontT_w_top,frontT_w_bottom, frontT_w_top_dis,frontT_w_bottom_dis, Colors.Gray, soilUri, true);
                    break;
                case ExcavationType.type2:
                    frontT_w_top_dis = 0;
                    frontT_w_bottom_dis = 0;
                    frontT_h = frontT_Z;
                    frontT_w_bottom = frontandbackCubeLength - frontT_w_bottom_dis - frontT_X2;
                    frontT_w_top = frontandbackCubeLength - frontT_X1 - frontT_X2;
                    frontSoilCenter = new Point(center.X - frontandbackCubeLength, center.Y + excavationHeight );
                    frontSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(frontSoilCenter, wall_h - excavationHeight-frontT_h, frontandbackCubeLength, frontT_h, frontT_w_top, frontT_w_bottom, frontT_w_top_dis, frontT_w_bottom_dis, Colors.Gray, soilUri, true);
                    break;
                default:
                    break;
            }

            double backT_w_top_dis = 0;
            double backT_w_bottom_dis = 0;
            double backT_h = 0;
            double backT_w_bottom = 0;
            double backT_w_top = 0;

            Point backSoilCenter = new Point(center.X + wall_t, center.Y);
            GeometryDrawing backSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(backSoilCenter, wall_h +backT_h, frontandbackCubeLength, backT_h, backT_w_top, backT_w_bottom, backT_w_top_dis, backT_w_bottom_dis, Colors.Gray, soilUri, true);

            switch (WpfUtils.GetGroundSurfaceType(StaticVariables.viewModel.GroundSurfaceTypeIndex))
            {
                case GroundSurfaceType.flat:
                    break;
                case GroundSurfaceType.type1:
                    backT_w_top_dis = frontandbackCubeLength;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = Math.Sin(backT_Beta * Math.PI / 180) * (frontandbackCubeLength - backT_A1);
                    backT_w_bottom = frontandbackCubeLength - backT_A1;
                    backT_w_top = 0;
                    backSoilCenter = new Point(center.X + wall_t, center.Y-backT_h);
                    backSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(backSoilCenter, wall_h , frontandbackCubeLength, backT_h, backT_w_top, backT_w_bottom, backT_w_top_dis, backT_w_bottom_dis, Colors.Gray, soilUri, true);
                    break;
                case GroundSurfaceType.type2:
                    backT_w_top_dis = backT_A1;
                    backT_w_bottom_dis = 0;
                    backT_h = backT_B;
                    backT_w_bottom = frontandbackCubeLength;
                    backT_w_top = frontandbackCubeLength - backT_A1;
                    backSoilCenter = new Point(center.X + wall_t, center.Y-backT_h);
                    backSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(backSoilCenter, wall_h , frontandbackCubeLength, backT_h, backT_w_top, backT_w_bottom, backT_w_top_dis, backT_w_bottom_dis, Colors.Gray, soilUri, true);
                    break;
                case GroundSurfaceType.type3:
                    backT_w_top_dis = backT_A1 + backT_A2;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = backT_B;
                    backT_w_bottom = frontandbackCubeLength - backT_A1;
                    backT_w_top = frontandbackCubeLength - backT_A1 - backT_A2;
                    backSoilCenter = new Point(center.X + wall_t, center.Y-backT_h);
                    backSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(backSoilCenter, wall_h , frontandbackCubeLength, backT_h, backT_w_top, backT_w_bottom, backT_w_top_dis, backT_w_bottom_dis, Colors.Gray, soilUri, true);
                    break;
                default:
                    break;
            }            
            
            mainDrawingGroup.Children.Add(wallGeometryDrawing);
            mainDrawingGroup.Children.Add(backSoilGeometry);
            mainDrawingGroup.Children.Add(frontSoilGeometry);
            mainDrawingGroup.Children.Add(bottomSoilGeometry);

            //yazı kısmı sadece deneme için
            GeometryDrawing textdeneme = Wpf2Dutils.TextGeometryDrawing(center,"ali",Colors.Red);
            mainDrawingGroup.Children.Add(textdeneme);



            sideview_main.Source = new DrawingImage(mainDrawingGroup);

        }
         
        public void Refresh3Dview()
        {
            StartViewModel2D();
        }
    }
}
