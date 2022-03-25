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
            Uri soilUri = new Uri(@"Textures/Soil/Kil.png", UriKind.Relative);
            GeometryDrawing wallGeometryDrawing = Wpf2Dutils.WallGeometryDrawing(center,wall_h,wall_t,Colors.Transparent);
                        
            

            //Point bottomSoilCenter = new Point(center.X-frontandbackCubeLength,center.Y+wall_h);
            //GeometryDrawing bottomSoilGeometry = Wpf2Dutils.SoilGeometryDrawing(bottomSoilCenter, bottomT_h, wall_t + frontandbackCubeLength * 2, Colors.Gray, soilUri, true);
            Point capBeamCenter = new Point(center.X + wall_t / 2 - capBeam_b / 2, center.Y - capBeam_h);
            switch (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    break;
                case WallType.ConcretePileWall:
                    
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

            double wallExtension = 0;
            double anchorDimensionExt1 = 0;
            //anchors
            foreach (var anchor in StaticVariables.viewModel.anchorDatas)
            {


                double soldierBeamH = 0;
                double soldierBeamW = 0;
                if (anchor.IsSoldierBeam)
                {
                    soldierBeamH = anchor.SoldierBeamHeight;
                    soldierBeamW = anchor.SoldierBeamwidth;
                    Point soldierBeamCenter = new Point(center.X - soldierBeamW, center.Y + anchor.AnchorDepth - soldierBeamH / 2);
                    GeometryDrawing soldierBeamGeometry = Wpf2Dutils.WallGeometryDrawing(soldierBeamCenter, soldierBeamH, soldierBeamW, Colors.Transparent);
                    mainDrawingGroup.Children.Add(soldierBeamGeometry);

                    if (wallExtension < anchor.SoldierBeamwidth) wallExtension = anchor.SoldierBeamwidth;
                }
                anchorDimensionExt1 += (soldierBeamW + StaticVariables.dimensionDiff + StaticVariables.dimensionExtension * 2 + StaticVariables.dimensionFontHeight);

                Point rotationCenter = new Point(center.X - soldierBeamW, center.Y + anchor.AnchorDepth);
                GeometryDrawing anchorGeometry = Wpf2Dutils.AnchorGeometryDrawing(rotationCenter, anchor.Inclination, wall_t, anchor.FreeLength, anchor.RootDiameter, anchor.RootLength, 0.2, soldierBeamW, Colors.DarkGray);
                mainDrawingGroup.Children.Add(anchorGeometry);
                double totalLength = Math.Cos((anchor.Inclination) * Math.PI / 180) * (anchor.FreeLength + anchor.RootLength);
                if (totalLength > backCubeLength) backCubeLength = totalLength;
                if(anchorDimensionExt1 > frontCubeLength) frontCubeLength = anchorDimensionExt1;

            }

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
                        new Point(frontSoilCenter.X + frontT_w_top_dis, frontSoilCenter.Y-frontT_h),
                        new Point(frontSoilCenter.X + frontCubeLength, frontSoilCenter.Y-frontT_h),
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

            Point backSoilCenter = new Point(center.X + wall_t, center.Y);
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
                                    new Point(backSoilCenter.X + backT_w_top_dis, backSoilCenter.Y-backT_h),
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

            
            
            switch (WpfUtils.GetGroundWaterType(StaticVariables.viewModel.WaterTypeIndex))
            {
                case GroundWaterType.none:
                    break;
                default:
                    GeometryDrawing frontWaterModel = Wpf2Dutils.LineGeometryDrawing(new Point(center.X-frontCubeLength, center.Y+excavationHeight+ groundW_h2),new Point(center.X , center.Y + excavationHeight + groundW_h2),Colors.Blue);
                    mainDrawingGroup.Children.Add(frontWaterModel);
                    GeometryDrawing backWaterModel = Wpf2Dutils.LineGeometryDrawing(new Point(center.X +wall_t, center.Y +  groundW_h1), new Point(center.X + wall_t + backCubeLength, center.Y +  groundW_h1), Colors.Blue);
                    mainDrawingGroup.Children.Add(backWaterModel);
                    break;
            }
            


            double totalHeight = 0;
            //soil boxes
            foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
            {
                if (totalHeight < wall_h + bottomT_h)
                {
                    Point soilLayerCenter = new Point(center.X + backCubeLength + wall_t + 7*StaticVariables.levelIconHeight, center.Y + totalHeight);
                    if (soilLayer.Soil != null)
                    {
                        double soilLayerheight = 0;
                        if (soilLayer == StaticVariables.viewModel.soilLayerDatas[0]&&StaticVariables.viewModel.GroundSurfaceTypeIndex!=0)
                        {
                            soilLayerheight = backT_h + soilLayer.LayerHeight;
                            soilLayerCenter = new Point(center.X + backCubeLength + wall_t + 7 * StaticVariables.levelIconHeight, center.Y + totalHeight-backT_h);
                        }
                        else
                        {
                            soilLayerheight = soilLayer.LayerHeight;
                        }
                        GeometryDrawing soilLayerGeometryDrawing = Wpf2Dutils.SoilGeometryDrawing(soilLayerCenter,
                        soilLayerheight,
                        soilLayerBoxW,
                        soilLayer.Soil.SoilColor,
                        soilLayer.Soil.SoilTexture.TextureUri,
                        soilLayer.Soil.isSoilTexture);
                        totalHeight += soilLayer.LayerHeight;
                        mainDrawingGroup.Children.Add(soilLayerGeometryDrawing);
                    }
                }
            }

            mainDrawingGroup.Children.Add(wallGeometryDrawing);
            mainDrawingGroup.Children.Add(backSoilGeometry);
            mainDrawingGroup.Children.Add(frontSoilGeometry);


            


            //levels
            GeometryDrawing topOfWallLevel = Wpf2Dutils.Level(new Point(center.X-frontCubeLength, center.Y), LevelDirection.Left, Colors.Red);
            mainDrawingGroup.Children.Add(topOfWallLevel);
            GeometryDrawing exLevel = Wpf2Dutils.Level(new Point(center.X - frontCubeLength, center.Y+excavationHeight), LevelDirection.Left, Colors.Red);
            mainDrawingGroup.Children.Add(exLevel);
            GeometryDrawing bottomOfWallLevel = Wpf2Dutils.Level(new Point(center.X - frontCubeLength, center.Y+wall_h), LevelDirection.Left, Colors.Red);
            mainDrawingGroup.Children.Add(bottomOfWallLevel);
            if(WpfUtils.GetGroundSurfaceType(StaticVariables.viewModel.GroundSurfaceTypeIndex)!=GroundSurfaceType.type1)
            {
                GeometryDrawing groundLevel = Wpf2Dutils.Level(new Point(center.X + backCubeLength + wall_t, center.Y - backT_h), LevelDirection.Right, Colors.Red);
                mainDrawingGroup.Children.Add(groundLevel);
            }
            


            switch (WpfUtils.GetGroundWaterType(StaticVariables.viewModel.WaterTypeIndex))
            {
                case GroundWaterType.none:
                    break;
                default:
                    GeometryDrawing exWaterLevel = Wpf2Dutils.Level(new Point(center.X - frontCubeLength, center.Y + excavationHeight+groundW_h2), LevelDirection.Left, Colors.Red);
                    mainDrawingGroup.Children.Add(exWaterLevel);
                    GeometryDrawing groundWaterLevel = Wpf2Dutils.Level(new Point(center.X + backCubeLength + wall_t, center.Y + groundW_h1), LevelDirection.Right, Colors.Red);
                    mainDrawingGroup.Children.Add(groundWaterLevel);
                    break;
            }

            


            //dimensions
            switch (StaticVariables.viewModel.stage)
            {
                case Stage.Materials:
                    break;
                case Stage.WallProperties:
                    switch (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex))
                    {
                        case WallType.ConcreteRectangleWall:
                            break;
                        case WallType.ConcretePileWall:                            
                            GeometryDrawing capBeamDimension = Wpf2Dutils.Dimension(capBeamCenter, new Point(capBeamCenter.X+capBeam_b,capBeamCenter.Y), Colors.DarkBlue, WpfUtils.GetDimension(capBeam_b).ToString() );
                            mainDrawingGroup.Children.Add(capBeamDimension);
                            GeometryDrawing capBeamDimension1 = Wpf2Dutils.Dimension(new Point(capBeamCenter.X + capBeam_b, capBeamCenter.Y), new Point(capBeamCenter.X + capBeam_b, capBeamCenter.Y+capBeam_h), Colors.DarkBlue, WpfUtils.GetDimension(capBeam_h).ToString() );
                            mainDrawingGroup.Children.Add(capBeamDimension1);
                            break;
                        case WallType.SteelSheetWall:
                            break;
                        default:
                            break;
                    }                    
                    GeometryDrawing wallDimensionDown = Wpf2Dutils.Dimension(new Point(center.X + wall_t, center.Y + wall_h), new Point(center.X, center.Y + wall_h), Colors.DarkBlue, WpfUtils.GetDimension(wall_t).ToString() );
                    mainDrawingGroup.Children.Add(wallDimensionDown);                                       
                    GeometryDrawing wallHeightLeft = Wpf2Dutils.Dimension(new Point(center.X - wallExtension, center.Y+wall_h),new Point(center.X - wallExtension, center.Y), Colors.DarkBlue, WpfUtils.GetDimension(wall_h).ToString() );
                    mainDrawingGroup.Children.Add(wallHeightLeft);
                    
                    break;
                case Stage.ExDesign:
                    GeometryDrawing exHeightLeft = Wpf2Dutils.Dimension(new Point(center.X-frontT_X1-frontT_X2, center.Y + excavationHeight), new Point(center.X - frontT_X1 - frontT_X2, center.Y), Colors.DarkBlue, WpfUtils.GetDimension(excavationHeight).ToString() );
                    mainDrawingGroup.Children.Add(exHeightLeft);
                    switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
                    {
                        case ExcavationType.none:
                            break;
                        case ExcavationType.type1:
                            GeometryDrawing _exZ = Wpf2Dutils.Dimension( new Point(center.X + wall_t, center.Y + excavationHeight-frontT_Z), new Point(center.X + wall_t, center.Y + excavationHeight), Colors.DarkBlue, WpfUtils.GetDimension(frontT_Z).ToString() );
                            mainDrawingGroup.Children.Add(_exZ);
                            GeometryDrawing _exX1 = Wpf2Dutils.Dimension(new Point(center.X - frontT_X2, center.Y + excavationHeight ), new Point(center.X - frontT_X1 - frontT_X2, center.Y + excavationHeight ), Colors.DarkBlue, WpfUtils.GetDimension(frontT_X1).ToString() );
                            mainDrawingGroup.Children.Add(_exX1);
                            GeometryDrawing _exX2 = Wpf2Dutils.Dimension(new Point(center.X, center.Y + excavationHeight ), new Point(center.X - frontT_X2, center.Y + excavationHeight ), Colors.DarkBlue, WpfUtils.GetDimension(frontT_X2).ToString() );
                            mainDrawingGroup.Children.Add(_exX2);
                            break;
                        case ExcavationType.type2:
                            GeometryDrawing exZ = Wpf2Dutils.Dimension(new Point(center.X +wall_t, center.Y + excavationHeight ), new Point(center.X +wall_t, center.Y + excavationHeight + frontT_Z), Colors.DarkBlue, WpfUtils.GetDimension(frontT_Z).ToString() );
                            mainDrawingGroup.Children.Add(exZ);
                            GeometryDrawing exX1 = Wpf2Dutils.Dimension(new Point(center.X -  frontT_X2, center.Y + excavationHeight+frontT_Z), new Point(center.X - frontT_X1 - frontT_X2, center.Y + excavationHeight + frontT_Z), Colors.DarkBlue, WpfUtils.GetDimension(frontT_X1).ToString() );
                            mainDrawingGroup.Children.Add(exX1);
                            GeometryDrawing exX2 = Wpf2Dutils.Dimension(new Point(center.X , center.Y + excavationHeight + frontT_Z), new Point(center.X - frontT_X2, center.Y + excavationHeight + frontT_Z), Colors.DarkBlue, WpfUtils.GetDimension(frontT_X2).ToString() );
                            mainDrawingGroup.Children.Add(exX2);
                            break;
                        default:
                            break;
                    }
                    switch (WpfUtils.GetGroundSurfaceType(StaticVariables.viewModel.GroundSurfaceTypeIndex))
                    {
                        case GroundSurfaceType.flat:
                            break;
                        case GroundSurfaceType.type1:
                            GeometryDrawing grA1 = Wpf2Dutils.Dimension(new Point(center.X +wall_t + backT_A1, center.Y ), new Point(center.X + wall_t, center.Y), Colors.DarkBlue, WpfUtils.GetDimension(backT_A1).ToString() );
                            mainDrawingGroup.Children.Add(grA1);
                            break;
                        case GroundSurfaceType.type2:
                            GeometryDrawing _grA1 = Wpf2Dutils.Dimension(new Point(center.X + wall_t + backT_A1, center.Y), new Point(center.X + wall_t, center.Y), Colors.DarkBlue, WpfUtils.GetDimension(backT_A1).ToString() );
                            mainDrawingGroup.Children.Add(_grA1);
                            GeometryDrawing grB = Wpf2Dutils.Dimension(new Point(center.X + wall_t + backT_A1 , center.Y - backT_h), new Point(center.X + wall_t + backT_A1 , center.Y), Colors.DarkBlue, WpfUtils.GetDimension(backT_B).ToString() );
                            mainDrawingGroup.Children.Add(grB);
                            break;
                        case GroundSurfaceType.type3:
                            GeometryDrawing _1grA1 = Wpf2Dutils.Dimension(new Point(center.X + wall_t + backT_A1, center.Y), new Point(center.X + wall_t, center.Y), Colors.DarkBlue, WpfUtils.GetDimension(backT_A1).ToString() );
                            mainDrawingGroup.Children.Add(_1grA1);
                            GeometryDrawing _grA2 = Wpf2Dutils.Dimension(new Point(center.X + wall_t + backT_A1 +backT_A2, center.Y), new Point(center.X + wall_t+backT_A1, center.Y), Colors.DarkBlue, WpfUtils.GetDimension(backT_A2).ToString() );
                            mainDrawingGroup.Children.Add(_grA2);
                            GeometryDrawing _grB = Wpf2Dutils.Dimension(new Point(center.X + wall_t + backT_A1 + backT_A2, center.Y -backT_h), new Point(center.X + wall_t + backT_A1 + backT_A2, center.Y ), Colors.DarkBlue, WpfUtils.GetDimension(backT_B).ToString() );
                            mainDrawingGroup.Children.Add(_grB);
                            break;
                        default:
                            break;
                    }

                    switch (WpfUtils.GetGroundWaterType(StaticVariables.viewModel.WaterTypeIndex))
                    {
                        case GroundWaterType.none:
                            break;
                        default:
                            GeometryDrawing frontWDimension = Wpf2Dutils.Dimension(new Point(center.X-frontT_X1-frontT_X2, center.Y+excavationHeight+groundW_h2), new Point(center.X - frontT_X1 - frontT_X2, center.Y + excavationHeight ),Colors.DarkBlue, WpfUtils.GetDimension(groundW_h2).ToString() );
                            mainDrawingGroup.Children.Add(frontWDimension);                           
                            GeometryDrawing backWDimension = Wpf2Dutils.Dimension(new Point(center.X + wall_t + backT_A1 + backT_A2, center.Y), new Point(center.X + wall_t + backT_A1 + backT_A2, center.Y+groundW_h1), Colors.DarkBlue, WpfUtils.GetDimension(groundW_h1).ToString() );
                            mainDrawingGroup.Children.Add(backWDimension);
                            break;
                    }
                    break;
                case Stage.SoilMethod:
                    totalHeight = 0;
                    foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                    {
                        if (totalHeight < wall_h + bottomT_h)
                        {
                            Point soilLayerDimensionCenter = new Point(center.X + backCubeLength + wall_t+soilLayerBoxW + 7 * StaticVariables.levelIconHeight, center.Y + totalHeight);
                            if (soilLayer.Soil != null)
                            {
                                double soilLayerheight = 0;
                                if (soilLayer == StaticVariables.viewModel.soilLayerDatas[0] && StaticVariables.viewModel.GroundSurfaceTypeIndex != 0)
                                {
                                    soilLayerheight = backT_h + soilLayer.LayerHeight;
                                    soilLayerDimensionCenter = new Point(center.X + backCubeLength + wall_t + soilLayerBoxW + 7 * StaticVariables.levelIconHeight, center.Y + totalHeight - backT_h);
                                }
                                else
                                {
                                    soilLayerheight = soilLayer.LayerHeight;
                                }
                                GeometryDrawing soilLayerDimension = Wpf2Dutils.Dimension(soilLayerDimensionCenter,new Point(soilLayerDimensionCenter.X,soilLayerDimensionCenter.Y+soilLayerheight),Colors.DarkBlue, WpfUtils.GetDimension(soilLayerheight).ToString() );
                                totalHeight += soilLayer.LayerHeight;
                                mainDrawingGroup.Children.Add(soilLayerDimension);
                            }
                        }
                    }
                    break;
                case Stage.Anchors:
                    //anchors
                    double anchorDimensionExt = 0;
                    foreach (var anchor in StaticVariables.viewModel.anchorDatas)
                    {
                        double soldierBeamH = 0;
                        double soldierBeamW = 0;
                        if (anchor.IsSoldierBeam)
                        {
                            soldierBeamH = anchor.SoldierBeamHeight;
                            soldierBeamW = anchor.SoldierBeamwidth;
                            Point soldierBeamCenter = new Point(center.X - soldierBeamW, center.Y + anchor.AnchorDepth + soldierBeamH / 2);
                            GeometryDrawing soldierBeamDimensionLeft = Wpf2Dutils.Dimension(soldierBeamCenter, new Point(soldierBeamCenter.X, soldierBeamCenter.Y - soldierBeamH), Colors.Blue);
                            mainDrawingGroup.Children.Add(soldierBeamDimensionLeft);
                            GeometryDrawing soldierBeamDimensionTop = Wpf2Dutils.Dimension(new Point(soldierBeamCenter.X + soldierBeamW, soldierBeamCenter.Y), new Point(soldierBeamCenter.X, soldierBeamCenter.Y), Colors.Blue);
                            mainDrawingGroup.Children.Add(soldierBeamDimensionTop);

                        }
                        anchorDimensionExt += (soldierBeamW + StaticVariables.dimensionDiff + StaticVariables.dimensionExtension*2 + StaticVariables.dimensionFontHeight);
                        Point rotationCenter = new Point(center.X , center.Y + anchor.AnchorDepth);
                        GeometryDrawing anchorDepthDimension = Wpf2Dutils.Dimension(new Point( rotationCenter.X - anchorDimensionExt,rotationCenter.Y), new Point(center.X - anchorDimensionExt, center.Y), Colors.Blue);
                        mainDrawingGroup.Children.Add(anchorDepthDimension);

                        
                    }
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
