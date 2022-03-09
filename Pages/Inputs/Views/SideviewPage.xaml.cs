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
        bool isCapBeamBottom = true;


        double soilLayerBoxW = 5;
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
            isCapBeamBottom = StaticVariables.viewModel.isCapBeamBottom;
        }

        private void StartViewModel2D()
        {
            SetViewModel();
            DrawingGroup mainDrawingGroup = new DrawingGroup();            
            Uri soilUri = new Uri(@"Textures/Soil/Kil.png", UriKind.Relative);
            GeometryDrawing wallGeometryDrawing = Wpf2Dutils.WallGeometryDrawing(center,wall_h,wall_t,Colors.Transparent);
                        
            double totalHeight = 0;
            //soil boxes
            foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
            {
                if(totalHeight<wall_h+ bottomT_h)
                {                    
                    Point soilLayerCenter = new Point(center.X + frontandbackCubeLength+wall_t, center.Y + totalHeight);
                    if (soilLayer.Soil != null)
                    {
                        GeometryDrawing soilLayerGeometryDrawing = Wpf2Dutils.SoilGeometryDrawing(soilLayerCenter,
                        soilLayer.LayerHeight,
                        soilLayerBoxW,
                        soilLayer.Soil.SoilColor,
                        soilLayer.Soil.SoilTexture.TextureUri,
                        soilLayer.Soil.isSoilTexture);
                        totalHeight += soilLayer.LayerHeight;
                        mainDrawingGroup.Children.Add(soilLayerGeometryDrawing);
                    }
                }
            }
           
            //Point bottomSoilCenter = new Point(center.X-frontandbackCubeLength,center.Y+wall_h);
            //GeometryDrawing bottomSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(bottomSoilCenter, bottomT_h, wall_t + frontandbackCubeLength * 2, Colors.Gray, soilUri, true);

            switch (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    break;
                case WallType.ConcretePileWall:
                    Point capBeamCenter = new Point(center.X + wall_t / 2 - capBeam_b / 2, center.Y - capBeam_h);
                    if (isCapBeamBottom)
                    {
                        capBeamCenter = new Point(center.X + wall_t / 2 - capBeam_b / 2, center.Y - capBeam_h);
                    }
                    else
                    {
                        capBeamCenter = new Point(center.X + wall_t / 2 - capBeam_b / 2, center.Y );
                    }                    
                    GeometryDrawing capBeamGeometryDrawing = Wpf2Dutils.WallGeometryDrawing(capBeamCenter, capBeam_h, capBeam_b, Colors.Transparent);
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
            Point frontSoilCenter = new Point(center.X - frontandbackCubeLength, center.Y + excavationHeight);
            //GeometryDrawing frontSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(frontSoilCenter, wall_h - excavationHeight, frontandbackCubeLength, frontT_h, frontT_w_top, frontT_w_bottom, frontT_w_top_dis, frontT_w_bottom_dis, Colors.Gray, soilUri, true);
            GeometryDrawing frontSoilGeometry = Wpf2Dutils.LineGeometryDrawing(frontSoilCenter,
                        new Point(frontSoilCenter.X + frontT_w_bottom_dis, frontSoilCenter.Y),
                        new Point(frontSoilCenter.X + frontT_w_top_dis, frontSoilCenter.Y - frontT_h),
                        new Point(frontSoilCenter.X + frontandbackCubeLength, frontSoilCenter.Y - frontT_h),
                        Colors.Black);
            switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
            {
                case ExcavationType.none:
                    break;
                case ExcavationType.type1:
                    frontT_w_top_dis = frontandbackCubeLength - frontT_X2;
                    frontT_w_bottom_dis = frontandbackCubeLength - frontT_X1 - frontT_X2;
                    frontT_h = frontT_Z;
                    frontSoilCenter = new Point(center.X - frontandbackCubeLength, center.Y + excavationHeight);
                    frontSoilGeometry = Wpf2Dutils.LineGeometryDrawing(frontSoilCenter,
                        new Point(frontSoilCenter.X + frontT_w_bottom_dis, frontSoilCenter.Y),
                        new Point(frontSoilCenter.X + frontT_w_top_dis, frontSoilCenter.Y-frontT_h),
                        new Point(frontSoilCenter.X + frontandbackCubeLength, frontSoilCenter.Y-frontT_h),
                        Colors.Black);
                    break;
                case ExcavationType.type2:
                    frontT_w_top_dis = frontandbackCubeLength - frontT_X2;
                    frontT_w_bottom_dis = frontandbackCubeLength - frontT_X1 - frontT_X2;
                    frontT_h = frontT_Z;
                    frontSoilCenter = new Point(center.X - frontandbackCubeLength, center.Y + excavationHeight);
                    frontSoilGeometry = Wpf2Dutils.LineGeometryDrawing(frontSoilCenter,
                        new Point(frontSoilCenter.X + frontT_w_bottom_dis, frontSoilCenter.Y),
                        new Point(frontSoilCenter.X + frontT_w_top_dis, frontSoilCenter.Y + frontT_h),
                        new Point(frontSoilCenter.X + frontandbackCubeLength, frontSoilCenter.Y + frontT_h),
                        Colors.Black); 
                    break;
                default:
                    break;
            }

            double backT_w_top_dis = 0;
            double backT_w_bottom_dis = 0;
            double backT_h = 0;

            Point backSoilCenter = new Point(center.X + wall_t, center.Y);
            //GeometryDrawing backSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(backSoilCenter, wall_h +backT_h, frontandbackCubeLength, backT_h, backT_w_top, backT_w_bottom, backT_w_top_dis, backT_w_bottom_dis, Colors.Gray, soilUri, true);
            GeometryDrawing backSoilGeometry = Wpf2Dutils.LineGeometryDrawing(backSoilCenter,
                                    new Point(backSoilCenter.X + backT_w_bottom_dis, backSoilCenter.Y),
                                    new Point(backSoilCenter.X + backT_w_top_dis, backSoilCenter.Y - backT_h),
                                    new Point(backSoilCenter.X + frontandbackCubeLength, backSoilCenter.Y - backT_h),
                                    Colors.Black);

            switch (WpfUtils.GetGroundSurfaceType(StaticVariables.viewModel.GroundSurfaceTypeIndex))
            {
                case GroundSurfaceType.flat:
                    break;
                case GroundSurfaceType.type1:
                    backT_w_top_dis = frontandbackCubeLength;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = Math.Sin(backT_Beta * Math.PI / 180) * (frontandbackCubeLength - backT_A1);
                    backSoilGeometry = Wpf2Dutils.LineGeometryDrawing(backSoilCenter,
                                    new Point(backSoilCenter.X + backT_w_bottom_dis, backSoilCenter.Y),
                                    new Point(backSoilCenter.X + backT_w_top_dis, backSoilCenter.Y - backT_h),
                                    new Point(backSoilCenter.X + frontandbackCubeLength, backSoilCenter.Y - backT_h),
                                    Colors.Black);
                    break;
                case GroundSurfaceType.type2:
                    backT_w_top_dis = backT_A1;
                    backT_w_bottom_dis = 0;
                    backT_h = backT_B;
                    backSoilGeometry = Wpf2Dutils.LineGeometryDrawing(backSoilCenter,
                                    new Point(backSoilCenter.X + backT_w_bottom_dis, backSoilCenter.Y),
                                    new Point(backSoilCenter.X + backT_w_top_dis, backSoilCenter.Y-backT_h),
                                    new Point(backSoilCenter.X + frontandbackCubeLength, backSoilCenter.Y - backT_h),
                                    Colors.Black); 
                    break;
                case GroundSurfaceType.type3:
                    backT_w_top_dis = backT_A1 + backT_A2;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = backT_B;
                    backSoilGeometry = Wpf2Dutils.LineGeometryDrawing(backSoilCenter,
                                    new Point(backSoilCenter.X + backT_w_bottom_dis, backSoilCenter.Y),
                                    new Point(backSoilCenter.X + backT_w_top_dis, backSoilCenter.Y - backT_h),
                                    new Point(backSoilCenter.X + frontandbackCubeLength, backSoilCenter.Y - backT_h),
                                    Colors.Black);
                    break;
                default:
                    break;
            }            
            
            mainDrawingGroup.Children.Add(wallGeometryDrawing);
            mainDrawingGroup.Children.Add(backSoilGeometry);
            mainDrawingGroup.Children.Add(frontSoilGeometry);
            //mainDrawingGroup.Children.Add(bottomSoilGeometry);

            //yazı kısmı sadece deneme için
            GeometryDrawing textdeneme = Wpf2Dutils.TextGeometryDrawing(center,"ali",Colors.Red);
            mainDrawingGroup.Children.Add(textdeneme);

            //dimensions
            switch (StaticVariables.viewModel.stage)
            {
                case Stage.Materials:
                    break;
                case Stage.WallProperties:                    
                    GeometryDrawing wallDimension = Wpf2Dutils.DimensionUp(center,wall_t,WpfUtils.GetDimension(wall_t).ToString()+" "+ StaticVariables.dimensionUnit, Colors.DarkBlue);
                    mainDrawingGroup.Children.Add(wallDimension);
                    break;
                case Stage.ExDesign:
                    break;
                case Stage.SoilMethod:
                    break;
                default:
                    break;
            }

            sideview_main.Source = new DrawingImage(mainDrawingGroup);

        }
         
        public void Refreshview()
        {
            StartViewModel2D();
        }
    }
}
