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
using ExDesign.Datas;
using System.Globalization;
using System.Diagnostics;

namespace ExDesign.Pages.Inputs.Views
{
    /// <summary>
    /// LoadsAndFocesPage.xaml etkileşim mantığı
    /// </summary>
    public partial class LoadsAndFocesPage : Page
    {
        Point center = new Point(0, 0);
        double wall_t = 0.65;
        double wall_h = 12;
        double frontCubeLength = 8;
        double backCubeLength = 12;
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
        bool isCapBeamBottom = true;
        double soilLayerBoxW = 5;
        Load showenLoad;
        bool isLoad = false;
        bool isForce = false;
        public LoadsAndFocesPage()
        {
            InitializeComponent();
            StartViewModel2D();
        }
        public void SetViewModel()
        {
            center = new Point(StaticVariables.viewModel.center3d.X, StaticVariables.viewModel.center3d.Y);
            wall_h = StaticVariables.viewModel.wall_h;
            wall_t = StaticVariables.viewModel.wall_t;
            frontCubeLength = StaticVariables.viewModel.frontCubeLength;
            backCubeLength = StaticVariables.viewModel.backCubeLength;
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
            isCapBeamBottom = StaticVariables.viewModel.isCapBeamBottom;
            soilLayerBoxW = StaticVariables.soilLayerBoxWidth;
        }
        private void StartViewModel2D()
        {
            SetViewModel();
            DrawingGroup mainDrawingGroup = new DrawingGroup();
            Point wallCenter = new Point(center.X, center.Y);
            GeometryDrawing wallGeometryDrawing = Wpf2Dutils.LineGeometryDrawing(wallCenter, new Point(wallCenter.X, wallCenter.Y + wall_h),Colors.Black);

            double frontT_w_top_dis = 0;
            double frontT_w_bottom_dis = 0;
            double frontT_h = 0;
            Point frontSoilCenter = new Point(center.X - frontCubeLength, center.Y + excavationHeight);
            //GeometryDrawing frontSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(frontSoilCenter, wall_h - excavationHeight, frontandbackCubeLength, frontT_h, frontT_w_top, frontT_w_bottom, frontT_w_top_dis, frontT_w_bottom_dis, Colors.Gray, soilUri, true);
            GeometryDrawing frontSoilGeometry = Wpf2Dutils.LineGeometryDrawing(frontSoilCenter,
                        new Point(frontSoilCenter.X + frontT_w_bottom_dis, frontSoilCenter.Y),
                        new Point(frontSoilCenter.X + frontT_w_top_dis, frontSoilCenter.Y - frontT_h),
                        new Point(frontSoilCenter.X + frontCubeLength, frontSoilCenter.Y - frontT_h),
                        Colors.Black);
            switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
            {
                case ExcavationType.none:
                    break;
                case ExcavationType.type1:
                    frontT_w_top_dis = frontCubeLength - frontT_X2;
                    frontT_w_bottom_dis = frontCubeLength - frontT_X1 - frontT_X2;
                    frontT_h = frontT_Z;
                    frontSoilCenter = new Point(center.X - frontCubeLength, center.Y + excavationHeight);
                    frontSoilGeometry = Wpf2Dutils.LineGeometryDrawing(frontSoilCenter,
                        new Point(frontSoilCenter.X + frontT_w_bottom_dis, frontSoilCenter.Y),
                        new Point(frontSoilCenter.X + frontT_w_top_dis, frontSoilCenter.Y - frontT_h),
                        new Point(frontSoilCenter.X + frontCubeLength, frontSoilCenter.Y - frontT_h),
                        Colors.Black);
                    break;
                case ExcavationType.type2:
                    frontT_w_top_dis = frontCubeLength - frontT_X2;
                    frontT_w_bottom_dis = frontCubeLength - frontT_X1 - frontT_X2;
                    frontT_h = frontT_Z;
                    frontSoilCenter = new Point(center.X - frontCubeLength, center.Y + excavationHeight);
                    frontSoilGeometry = Wpf2Dutils.LineGeometryDrawing(frontSoilCenter,
                        new Point(frontSoilCenter.X + frontT_w_bottom_dis, frontSoilCenter.Y),
                        new Point(frontSoilCenter.X + frontT_w_top_dis, frontSoilCenter.Y + frontT_h),
                        new Point(frontSoilCenter.X + frontCubeLength, frontSoilCenter.Y + frontT_h),
                        Colors.Black);
                    break;
                default:
                    break;
            }



            double backT_w_top_dis = 0;
            double backT_w_bottom_dis = 0;
            double backT_h = 0;

            Point backSoilCenter = new Point(center.X, center.Y);
            //GeometryDrawing backSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(backSoilCenter, wall_h +backT_h, frontandbackCubeLength, backT_h, backT_w_top, backT_w_bottom, backT_w_top_dis, backT_w_bottom_dis, Colors.Gray, soilUri, true);
            GeometryDrawing backSoilGeometry = Wpf2Dutils.LineGeometryDrawing(backSoilCenter,
                                    new Point(backSoilCenter.X + backT_w_bottom_dis, backSoilCenter.Y),
                                    new Point(backSoilCenter.X + backT_w_top_dis, backSoilCenter.Y - backT_h),
                                    new Point(backSoilCenter.X + backCubeLength, backSoilCenter.Y - backT_h),
                                    Colors.Black);

            switch (WpfUtils.GetGroundSurfaceType(StaticVariables.viewModel.GroundSurfaceTypeIndex))
            {
                case GroundSurfaceType.flat:
                    break;
                case GroundSurfaceType.type1:
                    backT_w_top_dis = backCubeLength;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = Math.Sin(backT_Beta * Math.PI / 180) * (backCubeLength - backT_A1);
                    backSoilGeometry = Wpf2Dutils.LineGeometryDrawing(backSoilCenter,
                                    new Point(backSoilCenter.X + backT_w_bottom_dis, backSoilCenter.Y),
                                    new Point(backSoilCenter.X + backT_w_top_dis, backSoilCenter.Y - backT_h),
                                    new Point(backSoilCenter.X + backCubeLength, backSoilCenter.Y - backT_h),
                                    Colors.Black);
                    break;
                case GroundSurfaceType.type2:
                    backT_w_top_dis = backT_A1;
                    backT_w_bottom_dis = 0;
                    backT_h = backT_B;
                    backSoilGeometry = Wpf2Dutils.LineGeometryDrawing(backSoilCenter,
                                    new Point(backSoilCenter.X + backT_w_bottom_dis, backSoilCenter.Y),
                                    new Point(backSoilCenter.X + backT_w_top_dis, backSoilCenter.Y - backT_h),
                                    new Point(backSoilCenter.X + backCubeLength, backSoilCenter.Y - backT_h),
                                    Colors.Black);
                    break;
                case GroundSurfaceType.type3:
                    backT_w_top_dis = backT_A1 + backT_A2;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = backT_B;
                    backSoilGeometry = Wpf2Dutils.LineGeometryDrawing(backSoilCenter,
                                    new Point(backSoilCenter.X + backT_w_bottom_dis, backSoilCenter.Y),
                                    new Point(backSoilCenter.X + backT_w_top_dis, backSoilCenter.Y - backT_h),
                                    new Point(backSoilCenter.X + backCubeLength, backSoilCenter.Y - backT_h),
                                    Colors.Black);
                    break;
                default:
                    break;
            }

            mainDrawingGroup.Children.Add(wallGeometryDrawing);
            mainDrawingGroup.Children.Add(backSoilGeometry);
            mainDrawingGroup.Children.Add(frontSoilGeometry);

            switch (WpfUtils.GetGroundWaterType(StaticVariables.viewModel.WaterTypeIndex))
            {
                case GroundWaterType.none:
                    break;
                default:
                    if (center.Y + excavationHeight + groundW_h2 - bottomT_h < wall_h)
                    {
                        GeometryDrawing frontWaterModel = Wpf2Dutils.LineGeometryDrawing(new Point(center.X - frontCubeLength, center.Y + excavationHeight + groundW_h2), new Point(center.X, center.Y + excavationHeight + groundW_h2), Colors.Blue);
                        mainDrawingGroup.Children.Add(frontWaterModel);
                    }
                    if (center.Y + groundW_h1 - bottomT_h < wall_h)
                    {
                        GeometryDrawing backWaterModel = Wpf2Dutils.LineGeometryDrawing(new Point(center.X, center.Y + groundW_h1), new Point(center.X + wall_t + backCubeLength, center.Y + groundW_h1), Colors.Blue);
                        mainDrawingGroup.Children.Add(backWaterModel);
                    }

                    break;
            }
            double loadScale = 0.1;
            double loadLimit = 3;
            if(StaticVariables.isAnalysisDone)
            {
                
                if(showenLoad!=null)
                {
                    if (isLoad)
                    {
                        double totalLoad = 0;
                        Point endFramePoint = new Point(0,0);
                        double endEndLoad = 0;
                        foreach (var frame in FrameData.Frames)
                        {

                            var endLoad = frame.endNodeLoadAndForce.Find(x => x.Item1.ID == showenLoad.ID);

                            if (endLoad.Item2 > totalLoad)
                            {
                                totalLoad = endLoad.Item3;
                                endFramePoint = frame.EndPoint;
                                endEndLoad = endLoad.Item3;
                            }

                        }
                        loadScale = loadLimit / totalLoad;
                        
                        foreach (var frame in FrameData.Frames)
                        {
                            var startLoad = frame.startNodeLoadAndForce.Find(x => x.Item1.ID == showenLoad.ID);
                            var endLoad = frame.endNodeLoadAndForce.Find(x => x.Item1.ID == showenLoad.ID);
                            
                                GeometryDrawing loadLineDrawing = Wpf2Dutils.LineGeometryDrawing(
                                new Point(frame.StartPoint.X + startLoad.Item3 * loadScale, frame.StartPoint.Y),
                                new Point(frame.EndPoint.X + endLoad.Item3 * loadScale, frame.EndPoint.Y),
                                Colors.Red);
                                mainDrawingGroup.Children.Add(loadLineDrawing);

                        }
                          //deneme texti
                            GeometryDrawing freeText = Wpf2Dutils.FreeTextDrawing(new Point(endFramePoint.X + endEndLoad * loadScale, endFramePoint.Y), new Point(endFramePoint.X + endEndLoad * loadScale + 0.5, endFramePoint.Y), Colors.Red, WpfUtils.ChangeDecimalOptions(endEndLoad));
                            mainDrawingGroup.Children.Add(freeText);

                    }
                    if (isForce)
                    {
                        double totalLoad = 0;
                        double totalLoad1 = 0;
                        double totalLoad2 = 0;
                        Point endFramePoint = new Point(0, 0);
                        double endEndLoad = 0;
                        Point endFramePoint1 = new Point(0, 0);
                        double endEndLoad1 = 0;
                        Point endFramePoint2 = new Point(0, 0);
                        double endEndLoad2 = 0;
                        foreach (var frame in FrameData.Frames)
                        {
                            var endLoad = frame.endNodeActivePassiveCoef_S_P_N.Find(x => x.Item1.ID == showenLoad.ID);
                            if (endLoad.Item2 > totalLoad)
                            {
                                totalLoad = endLoad.Item2;
                                endFramePoint = frame.EndPoint;
                                endEndLoad = endLoad.Item2;
                            }
                            if (endLoad.Item3 > totalLoad1)
                            {
                                totalLoad1 = endLoad.Item3;
                                endFramePoint1 = frame.EndPoint;
                                endEndLoad1 = endLoad.Item3;
                            }
                            if (endLoad.Item4 > totalLoad2)
                            {
                                totalLoad2 = endLoad.Item4;
                                endFramePoint2 = frame.EndPoint;
                                endEndLoad2 = endLoad.Item4;
                            }

                        }
                        loadScale = loadLimit / Math.Max( totalLoad,Math.Max( totalLoad1,totalLoad2));
                        foreach (var frame in FrameData.Frames)
                        {
                            var startLoad = frame.startNodeActivePassiveCoef_S_P_N.Find(x => x.Item1.ID == showenLoad.ID);
                            var endLoad = frame.endNodeActivePassiveCoef_S_P_N.Find(x => x.Item1.ID == showenLoad.ID);
                            GeometryDrawing loadLineDrawing = Wpf2Dutils.LineGeometryDrawing(
                                new Point(frame.StartPoint.X + startLoad.Item2 * loadScale, frame.StartPoint.Y),
                                new Point(frame.EndPoint.X + endLoad.Item2 * loadScale, frame.EndPoint.Y),
                                Colors.Red);
                            mainDrawingGroup.Children.Add(loadLineDrawing);
                            

                        }
                        foreach (var frame in FrameData.Frames)
                        {
                            var startLoad = frame.startNodeActivePassiveCoef_S_P_N.Find(x => x.Item1.ID == showenLoad.ID);
                            var endLoad = frame.endNodeActivePassiveCoef_S_P_N.Find(x => x.Item1.ID == showenLoad.ID);
                            GeometryDrawing loadLineDrawing = Wpf2Dutils.LineGeometryDrawing(
                                new Point(frame.StartPoint.X + startLoad.Item3 * loadScale, frame.StartPoint.Y),
                                new Point(frame.EndPoint.X + endLoad.Item3 * loadScale, frame.EndPoint.Y),
                                Colors.Green);
                            mainDrawingGroup.Children.Add(loadLineDrawing);

                        }
                        foreach (var frame in FrameData.Frames)
                        {
                            var startLoad = frame.startNodeActivePassiveCoef_S_P_N.Find(x => x.Item1.ID == showenLoad.ID);
                            var endLoad = frame.endNodeActivePassiveCoef_S_P_N.Find(x => x.Item1.ID == showenLoad.ID);
                            GeometryDrawing loadLineDrawing = Wpf2Dutils.LineGeometryDrawing(
                                new Point(frame.StartPoint.X + startLoad.Item4 * loadScale, frame.StartPoint.Y),
                                new Point(frame.EndPoint.X + endLoad.Item4 * loadScale, frame.EndPoint.Y),
                                Colors.Orange);
                            mainDrawingGroup.Children.Add(loadLineDrawing);

                        }
                        //deneme texti
                        GeometryDrawing freeText = Wpf2Dutils.FreeTextDrawing(new Point(endFramePoint.X + endEndLoad * loadScale +StaticVariables.freeTextFontHeight, endFramePoint.Y), new Point(endFramePoint.X + endEndLoad * loadScale + StaticVariables.freeTextFontHeight , endFramePoint.Y - 2), Colors.Red,"S = "+ WpfUtils.ChangeDecimalOptions(endEndLoad));
                        mainDrawingGroup.Children.Add(freeText);
                        //deneme texti
                        GeometryDrawing freeText1 = Wpf2Dutils.FreeTextDrawing(new Point(endFramePoint1.X + endEndLoad1 * loadScale + StaticVariables.freeTextFontHeight , endFramePoint1.Y), new Point(endFramePoint1.X + endEndLoad1 * loadScale + StaticVariables.freeTextFontHeight , endFramePoint1.Y - 2), Colors.Green, "P = " + WpfUtils.ChangeDecimalOptions(endEndLoad1));
                        mainDrawingGroup.Children.Add(freeText1);
                        //deneme texti
                        GeometryDrawing freeText2 = Wpf2Dutils.FreeTextDrawing(new Point(endFramePoint2.X + endEndLoad2 * loadScale + StaticVariables.freeTextFontHeight , endFramePoint2.Y), new Point(endFramePoint2.X + endEndLoad2 * loadScale + StaticVariables.freeTextFontHeight , endFramePoint2.Y - 2), Colors.Orange, "N = " + WpfUtils.ChangeDecimalOptions(endEndLoad2));
                        mainDrawingGroup.Children.Add(freeText2);
                    }
                }
            }

            loadsandforces_main.Source = new DrawingImage(mainDrawingGroup);
        }
        public void Refreshview()
        {
            StartViewModel2D();
        }
        public void ShowLoad(Load load)
        {
            isLoad = true;
            isForce = false;
            showenLoad = load;
            StartViewModel2D();
        }
        public void ShowForce(Load load)
        {
            isLoad = false;
            isForce =true;
            showenLoad = load;
            StartViewModel2D();
        }
    }
}
