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
using System.Windows.Media.Animation;
using ExDesign.Datas;

namespace ExDesign.Pages.Inputs.Views
{
    /// <summary>
    /// view3dPage.xaml etkileşim mantığı
    /// </summary>
    public partial class View3dPage : Page
    {
        public Viewport3D viewport;
        public Model3DGroup groupScene;
        double scaleFactor = 15;
        private double minScaleFactor = 8;
        private double maxScaleFactor = 50;
        Point3D lookat = new Point3D(0, 0, 0);
        Point mouseFirstPos;
        bool isMouseWheelDown;
        double rotationAngleX;
        double rotationAngleY;
        Vector mousePosDiff;
        Point3D center3d = new Point3D(0, 0, 0);
        double wall_t=0.65;
        double wall_h=12; 
        double wall_d=7;
        double pile_s = 0.80;
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
        double bottomT_h =3;
        double groundW_h1 = 5;
        double groundW_h2 = 2;
        double capBeam_h = 0.8;
        double capBeam_b = 0.65;
        int sheetIndex = 0;
        bool isCapBeamBottom = true;

        public View3dPage()
        {
            InitializeComponent();
            viewport = viewport3d_main;
            StartViewport3d();
            ChangeModelTransform();
        }
        
        public DirectionalLight positionLight(Point3D position)
        {
            DirectionalLight directionalLight = new DirectionalLight();
            directionalLight.Color = Colors.Gray;
            directionalLight.Direction = new Point3D(0, 0, 0) - position;
            return directionalLight;
        }

        public DirectionalLight leftLight()
        {
            return positionLight(new Point3D(-WpfScene.sceneSize, WpfScene.sceneSize / 2, 0.0));
        }

        public PerspectiveCamera camera()
        {
            PerspectiveCamera perspectiveCamera = new PerspectiveCamera();

            perspectiveCamera.Position = new Point3D(-WpfScene.sceneSize/2, WpfScene.sceneSize / 2, WpfScene.sceneSize);

            perspectiveCamera.LookDirection = new Vector3D(lookat.X - perspectiveCamera.Position.X,
                                                           lookat.Y - perspectiveCamera.Position.Y,
                                                           lookat.Z - perspectiveCamera.Position.Z);
            //perspectiveCamera.Width = WpfScene.sceneSize;
            perspectiveCamera.FieldOfView = 60;

            return perspectiveCamera;
        }
        public void SetViewModel()
        {
            center3d = StaticVariables.viewModel.center3d;
            wall_d = StaticVariables.viewModel.wall_d;
            wall_h = StaticVariables.viewModel.wall_h;
            wall_t = StaticVariables.viewModel.wall_t;
            pile_s = StaticVariables.viewModel.pile_s;
            frontCubeLength = StaticVariables.viewModel.CubeLength;
            backCubeLength = StaticVariables.viewModel.CubeLength;
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
        private void StartViewport3d()
        {
            //başta anchorage ve duvar olmalı döngünün içinde yapınca işler karıştı
            SetViewModel();
            double centerY = wall_h / 2 + bottomT_h / 2;            
            groupScene = new Model3DGroup();
            Uri soilUri = new Uri(@"Textures/Soil/Kil.png", UriKind.Relative); 
            Uri soilUri1 = new Uri(@"Textures/Soil/killikum.png", UriKind.Relative);
            
           

            //create wall model
            Point3D wallCenter = new Point3D(center3d.X - wall_t, center3d.Y + centerY, center3d.Z - wall_d / 2);
            switch (WpfUtils.GetWallType( StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    WpfCube wallCube = new WpfCube(wallCenter, wall_t, wall_h, wall_d);
                    GeometryModel3D wallModel = WpfCube.CreateCubeModel(wallCube, Colors.DarkGray,false,soilUri);
                    groupScene.Children.Add(wallModel);
                    break;
                case WallType.ConcretePileWall:
                    double spaceCount =Math.Clamp( Math.Round((wall_d - wall_t) / pile_s,MidpointRounding.ToNegativeInfinity),1,StaticVariables.maxPileCount);
                    double pile_h = wall_h;
                    double pile_d = wall_t / 2;
                    double pile_start = (wall_d - (spaceCount * pile_s)) / 2;
                    for (int i = 0; i < spaceCount+1; i++)
                    {
                        
                        Point3D pileCenter = new Point3D(center3d.X -wall_t/2, center3d.Y + centerY, center3d.Z-wall_d/2+pile_start +i*pile_s);
                        WpfCylinder pile = new WpfCylinder(pileCenter, 30, pile_d, pile_d, pile_h);
                        GeometryModel3D pileModel = pile.CreateModel(Colors.DarkGray, true, true);
                        AxisAngleRotation3D pileRotY = new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90);
                        RotateTransform3D PileTransform3d = new RotateTransform3D(pileRotY, pileCenter);
                        Transform3DGroup pileTransform3DGroup = new Transform3DGroup();
                        pileTransform3DGroup.Children.Add(PileTransform3d);
                        pileModel.Transform = pileTransform3DGroup;
                        groupScene.Children.Add(pileModel);
                        Point3D capbeamCenter;
                        if (isCapBeamBottom)
                        {
                            capbeamCenter = new Point3D(center3d.X - wall_t / 2 - capBeam_b / 2, center3d.Y + centerY + capBeam_h, center3d.Z - wall_d / 2);

                        }else
                        {
                            capbeamCenter = new Point3D(center3d.X - wall_t / 2 - capBeam_b / 2, center3d.Y + centerY , center3d.Z - wall_d / 2);

                        }
                        WpfCube capbeam = new WpfCube(capbeamCenter, capBeam_b, capBeam_h, wall_d);
                        GeometryModel3D capbeamModel = WpfCube.CreateCubeModel(capbeam, Colors.DarkGray,false,soilUri);
                        groupScene.Children.Add(capbeamModel);
                    }
                    break;
                case WallType.SteelSheetWall:
                    if (Sheet.SheetDataList.Count <= 0) break;
                    double sheetH = Sheet.SheetDataList[sheetIndex].Height;
                    double sheetL = Sheet.SheetDataList[sheetIndex].Length;
                    double sheetT = Sheet.SheetDataList[sheetIndex].Thickness;
                    wall_t = 2 * sheetH;
                    double sheetL1 = sheetH / 2; //TODO: bu kısmı kafadan attım. değitirilmesi lazım
                    double sheetspaceCount = Math.Clamp(Math.Round(wall_d / sheetL, MidpointRounding.ToNegativeInfinity), 1, StaticVariables.maxPileCount);

                    double sheetpile_start =(wall_d - (sheetspaceCount * sheetL)) / 2;
                    for (int i = 0; i < sheetspaceCount ; i++)
                    {
                        Math.DivRem(i, 2, out int result);
                        double thicknesChanger = wall_t * result;
                        Point3D sheetCoreCenter = new Point3D(center3d.X-wall_t+thicknesChanger, center3d.Y + centerY, center3d.Z + wall_d / 2 - sheetpile_start - i * sheetL);
                        WpfTrapezoid sheetCore = new WpfTrapezoid(sheetCoreCenter, sheetL-2*sheetL1 , sheetL-2*sheetL1 , sheetT, wall_h, 0, 0);
                        GeometryModel3D sheetCoreModel = WpfTrapezoid.CreateTrapezoidModel(sheetCore, Colors.DarkGray,false,soilUri);
                        AxisAngleRotation3D pileRotY = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90);
                        RotateTransform3D PileTransform3d = new RotateTransform3D(pileRotY, sheetCoreCenter); 
                        AxisAngleRotation3D pileRotX = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90);
                        RotateTransform3D PileTransform3dX = new RotateTransform3D(pileRotX, sheetCoreCenter);
                        Transform3DGroup pileTransform3DGroup = new Transform3DGroup();
                        pileTransform3DGroup.Children.Add(PileTransform3d);
                        pileTransform3DGroup.Children.Add(PileTransform3dX);
                        sheetCoreModel.Transform = pileTransform3DGroup;
                        groupScene.Children.Add(sheetCoreModel);

                        double wingLength = Math.Sqrt(Math.Pow(sheetL1, 2) + Math.Pow(sheetH, 2));
                        double angleLeft = Math.Cos(sheetH / sheetL1)*180/Math.PI;
                        double thicknesschangerleft = wall_t / 2 * result;
                        Point3D sheetleftCenter = new Point3D(center3d.X - wall_t + thicknesschangerleft, center3d.Y + centerY, center3d.Z + (wall_d / 2) - sheetpile_start + sheetT / 2 - result*(sheetL-sheetL1) - i * sheetL);
                        WpfTrapezoid sheetleft = new WpfTrapezoid(sheetleftCenter, wingLength, wingLength, sheetT, wall_h, 0, 0);
                        GeometryModel3D sheetLeftModel = WpfTrapezoid.CreateTrapezoidModel(sheetleft,Colors.DarkGray, false, soilUri);
                        AxisAngleRotation3D pileRotYLeft = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90);
                        RotateTransform3D PileTransform3dLeft = new RotateTransform3D(pileRotYLeft, sheetleftCenter);
                        AxisAngleRotation3D pileRotXLeft = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angleLeft);
                        RotateTransform3D PileTransform3dXLeft = new RotateTransform3D(pileRotXLeft, sheetleftCenter);
                        Transform3DGroup pileTransform3DGroupLeft = new Transform3DGroup();
                        pileTransform3DGroupLeft.Children.Add(PileTransform3dLeft);
                        pileTransform3DGroupLeft.Children.Add(PileTransform3dXLeft);
                        sheetLeftModel.Transform = pileTransform3DGroupLeft;
                        groupScene.Children.Add(sheetLeftModel);

                        double angleRight = Math.Cos(sheetH / sheetL1) * 180 / Math.PI;
                        double thicknesschangerRight = wall_t / 2 * result;
                        Point3D sheetRightCenter = new Point3D(center3d.X - wall_t + thicknesschangerRight, center3d.Y + centerY, center3d.Z + wall_d / 2 - sheetpile_start + sheetT / 2 - sheetL + 2 * sheetL1 + result * (sheetL - sheetL1) - i * sheetL);
                        WpfTrapezoid sheetRight = new WpfTrapezoid(sheetRightCenter, wingLength, wingLength, sheetT, wall_h, 0, 0);
                        GeometryModel3D sheetRightModel = WpfTrapezoid.CreateTrapezoidModel(sheetRight, Colors.DarkGray, false, soilUri);
                        AxisAngleRotation3D pileRotYRight = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90);
                        RotateTransform3D PileTransform3dRight = new RotateTransform3D(pileRotYRight, sheetRightCenter);
                        AxisAngleRotation3D pileRotXRight = new AxisAngleRotation3D(new Vector3D(0, 1, 0), -angleRight);
                        RotateTransform3D PileTransform3dXRight = new RotateTransform3D(pileRotXRight, sheetRightCenter);
                        Transform3DGroup pileTransform3DGroupRight = new Transform3DGroup();
                        pileTransform3DGroupRight.Children.Add(PileTransform3dRight);
                        pileTransform3DGroupRight.Children.Add(PileTransform3dXRight);
                        sheetRightModel.Transform = pileTransform3DGroupRight;
                        groupScene.Children.Add(sheetRightModel);
                    }
                   
                    break;
                default:
                    break;
            }
            
            //anchorage
            double anchor_free_L = 7;
            double anchor_wire_d = 0.15;
            double anchor_loc = 1;
            double beam_w = 1;
            double beam_h = 1;
            double anchor_angle = 15;
            double anchor_root_d = 3;
            double anchor_root_L = 3;
            double numofanchor = 3;
            foreach (var anchor in StaticVariables.viewModel.anchorDatas)
            {
                anchor_wire_d = StaticVariables.wireScaleFactor * Math.Sqrt(anchor.TotalNominalArea / Math.PI);
                if (wall_d > anchor.Spacing)
                {
                    if(anchor.IsCentralPlacement)
                    {
                        numofanchor = anchor.Spacing > 0 ? Math.Round(((wall_d / 2)  - anchor_wire_d) / anchor.Spacing, 0, MidpointRounding.ToPositiveInfinity) : 1;
                    }
                    else
                    {
                        numofanchor = anchor.Spacing > 0 ? Math.Round(((wall_d / 2) - (anchor.Spacing / 2) - anchor_wire_d / 2) / anchor.Spacing, 0, MidpointRounding.ToPositiveInfinity) : 1;
                    }
                }
                else
                {
                    numofanchor = 1;
                }

                
                anchor_loc = anchor.AnchorDepth;
                beam_w = anchor.IsSoldierBeam ? anchor.SoldierBeamwidth : 0;
                beam_h = anchor.IsSoldierBeam ? anchor.SoldierBeamHeight : 0;
                anchor_free_L = anchor.FreeLength + wall_t + 0.2 + beam_w;
                anchor_angle = anchor.Inclination;
                anchor_root_d = anchor.RootDiameter/2;
                anchor_root_L = anchor.RootLength;
                //anchor_wire_d =anchor_root_d <= 0.3 ? 0.05 : 0.7;
                
                double totalLength = anchor_free_L + anchor_root_L;
                if(totalLength > backCubeLength) backCubeLength = totalLength;
                for (int i = 0; i < numofanchor; i++)
                {
                    Point3D cylinderCenter = new Point3D(center3d.X - wall_t - 0.2 - beam_w, center3d.Y + centerY - anchor_loc, center3d.Z + i * anchor.Spacing + (anchor.IsCentralPlacement ? 0 : anchor.Spacing / 2));
                    WpfCylinder cylinder = new WpfCylinder(cylinderCenter, 10, anchor_wire_d, anchor_wire_d, anchor_free_L);
                    GeometryModel3D cylinderModel = cylinder.CreateModel(Colors.Gray, true, true);
                    AxisAngleRotation3D rotationX = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 270);
                    RotateTransform3D rotateTransformX = new RotateTransform3D(rotationX, cylinderCenter);
                    AxisAngleRotation3D rotationY = new AxisAngleRotation3D(new Vector3D(0, 0, 1), -anchor_angle);
                    RotateTransform3D rotateTransformY = new RotateTransform3D(rotationY, cylinderCenter);
                    Transform3DGroup myTransform3DGroup = new Transform3DGroup();
                    

                    Point3D cylinderCenter1 = new Point3D(center3d.X - wall_t - 0.2 - beam_w, center3d.Y + centerY - anchor_loc, center3d.Z - anchor_free_L + i * anchor.Spacing + (anchor.IsCentralPlacement ? 0 : anchor.Spacing / 2));
                    WpfCylinder cylinder1 = new WpfCylinder(cylinderCenter1, 10, anchor_root_d, anchor_root_d, anchor_root_L);
                    GeometryModel3D cylinderModel1 = cylinder1.CreateModel(Colors.DarkGray, true, true);
                    myTransform3DGroup.Children.Add(rotateTransformX);
                    myTransform3DGroup.Children.Add(rotateTransformY);
                    cylinderModel.Transform = myTransform3DGroup;
                    cylinderModel1.Transform = myTransform3DGroup;

                    groupScene.Children.Add(cylinderModel);
                    groupScene.Children.Add(cylinderModel1);
                }
                for (int i = 0; i < numofanchor; i++)
                {

                    Point3D cylinderCenter2 = new Point3D(center3d.X - wall_t - 0.2 - beam_w, center3d.Y + centerY - anchor_loc, center3d.Z - i * anchor.Spacing - (anchor.IsCentralPlacement ? 0 : anchor.Spacing / 2));
                    WpfCylinder cylinder = new WpfCylinder(cylinderCenter2, 10, anchor_wire_d, anchor_wire_d, anchor_free_L);
                    GeometryModel3D cylinderModel = cylinder.CreateModel(Colors.Gray, true, true);
                    AxisAngleRotation3D rotationX1 = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 270);
                    RotateTransform3D rotateTransformX1 = new RotateTransform3D(rotationX1, cylinderCenter2);
                    AxisAngleRotation3D rotationY1 = new AxisAngleRotation3D(new Vector3D(0, 0, 1), -anchor_angle);
                    RotateTransform3D rotateTransformY1 = new RotateTransform3D(rotationY1, cylinderCenter2);
                    Transform3DGroup myTransform3DGroup1 = new Transform3DGroup();


                    Point3D cylinderCenter3 = new Point3D(center3d.X - wall_t - 0.2 - beam_w, center3d.Y + centerY - anchor_loc, center3d.Z - anchor_free_L - i * anchor.Spacing - (anchor.IsCentralPlacement ? 0 : anchor.Spacing / 2));
                    WpfCylinder cylinder1 = new WpfCylinder(cylinderCenter3, 10, anchor_root_d, anchor_root_d, anchor_root_L);
                    GeometryModel3D cylinderModel1 = cylinder1.CreateModel(Colors.DarkGray, true, true);
                    myTransform3DGroup1.Children.Add(rotateTransformX1);
                    myTransform3DGroup1.Children.Add(rotateTransformY1);
                    cylinderModel.Transform = myTransform3DGroup1;
                    cylinderModel1.Transform = myTransform3DGroup1;

                    groupScene.Children.Add(cylinderModel);

                    groupScene.Children.Add(cylinderModel1);
                }
                if (anchor.IsSoldierBeam)
                {
                    Point3D beamCenter = new Point3D(center3d.X - wall_t - beam_w, center3d.Y + centerY - anchor_loc + beam_h / 2, center3d.Z - wall_d / 2);
                    WpfCube bemaCube = new WpfCube(beamCenter, beam_w, beam_h, wall_d);
                    GeometryModel3D beamModel = WpfCube.CreateCubeModel(bemaCube, Colors.DarkGray, false, soilUri);
                    groupScene.Children.Add(beamModel);
                }
            }


            double numofStrut = 3;
            double strut_D = 0;
            double strut_T = 0.01;
            double strut_L = 10;
            double strutbeam_w = 1;
            double strutbeam_h = 1;
            double strut_loc = 1;

            //Struts
            foreach (var strut in StaticVariables.viewModel.strutDatas)
            {
                strut_D = strut.StrutOuterDiameter/2;
                if (wall_d > strut.StrutSpacing)
                {
                    if(strut.IsCentralPlacement)
                    {
                        numofStrut = strut.StrutSpacing > 0 ? Math.Round(((wall_d / 2)  - strut_D) / strut.StrutSpacing, 0, MidpointRounding.ToPositiveInfinity) : 1;
                    }
                    else { 
                        numofStrut = strut.StrutSpacing > 0 ? Math.Round(((wall_d / 2) - (strut.StrutSpacing / 2) - strut_D ) / strut.StrutSpacing, 0, MidpointRounding.ToPositiveInfinity) : 1;
                    }
                }
                else
                {
                    numofStrut = 1;
                }
                strut_loc = strut.StrutDepth;
                strutbeam_w = strut.IsSoldierBeam ? strut.SoldierBeamwidth : 0;
                strutbeam_h = strut.IsSoldierBeam ? strut.SoldierBeamHeight : 0;
                strut_T = strut.StrutThickness;
                strut_L = StaticVariables.strutLength;
                for (int i = 0; i < numofStrut; i++)
                {
                    Point3D strutCenter = new Point3D(center3d.X - wall_t - strutbeam_w - strut_L, center3d.Y + centerY - strut_loc , center3d.Z - i * strut.StrutSpacing - (strut.IsCentralPlacement? 0: strut.StrutSpacing / 2));
                    WpfCylinder strutGeometry = new WpfCylinder(strutCenter, 15, strut_D, strut_D, strut_L);
                    GeometryModel3D strutModel = strutGeometry.CreateModel(Colors.MistyRose, true, true);
                    AxisAngleRotation3D strutrotationX = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 270);
                    RotateTransform3D strutrotateTransformX = new RotateTransform3D(strutrotationX, strutCenter);                    
                    Transform3DGroup strutmyTransform3DGroup = new Transform3DGroup();
                    strutmyTransform3DGroup.Children.Add(strutrotateTransformX);
                    strutModel.Transform = strutmyTransform3DGroup;
                    groupScene.Children.Add(strutModel);
                }
                for (int i = 0; i < numofStrut; i++)
                {
                    Point3D strutCenter = new Point3D(center3d.X - wall_t - strutbeam_w - strut_L, center3d.Y + centerY - strut_loc, center3d.Z + i * strut.StrutSpacing + (strut.IsCentralPlacement ? 0 : strut.StrutSpacing / 2));
                    WpfCylinder strutGeometry = new WpfCylinder(strutCenter, 15, strut_D, strut_D, strut_L);
                    GeometryModel3D strutModel = strutGeometry.CreateModel(Colors.MistyRose, true, true);
                    AxisAngleRotation3D strutrotationX = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 270);
                    RotateTransform3D strutrotateTransformX = new RotateTransform3D(strutrotationX, strutCenter);
                    Transform3DGroup strutmyTransform3DGroup = new Transform3DGroup();
                    strutmyTransform3DGroup.Children.Add(strutrotateTransformX);
                    strutModel.Transform = strutmyTransform3DGroup;
                    groupScene.Children.Add(strutModel);
                }
                if (strut.IsSoldierBeam)
                {
                    Point3D beamCenter = new Point3D(center3d.X - wall_t - strutbeam_w, center3d.Y + centerY - strut_loc + strutbeam_h / 2, center3d.Z - wall_d / 2);
                    WpfCube bemaCube = new WpfCube(beamCenter, strutbeam_w, strutbeam_h, wall_d);
                    GeometryModel3D beamModel = WpfCube.CreateCubeModel(bemaCube, Colors.DarkGray, false, soilUri);
                    groupScene.Children.Add(beamModel);
                }
            }

            //create backT model
            double backW_w = 0;
            double backW_h = 0;
            double backW_d = 0;
            double frontW_w = 0;
            double frontW_h = 0;
            double frontW_d = 0;
            switch (WpfUtils.GetGroundWaterType(StaticVariables.viewModel.WaterTypeIndex))
            {
                case GroundWaterType.none:
                    break;
                default:
                    backW_w = backCubeLength - 0.2;
                    backW_h = 0.1;
                    backW_d = wall_d - 0.1;
                    frontW_w = frontCubeLength - 0.1;
                    frontW_h = 0.1;
                    frontW_d = wall_d - 0.1;
                    break;
            }
            double backT_w_top_dis = 0;
            double backT_w_bottom_dis = 0;
            double backT_h = 0;
            double backT_d = 0;
            double backT_w_bottom = 0;
            double backT_w_top = 0;
            Color backT_color = Color.FromArgb(100, 200, 200, 200);
            switch (WpfUtils.GetGroundSurfaceType(StaticVariables.viewModel.GroundSurfaceTypeIndex))
            {
                case GroundSurfaceType.flat:
                    break;
                case GroundSurfaceType.type1:
                    backT_w_top_dis = backCubeLength;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = Math.Sin(backT_Beta * Math.PI / 180) * (backCubeLength - backT_A1);
                    backT_d = wall_d;
                    backT_w_bottom = backCubeLength - backT_A1;
                    backT_w_top = 0;
                    
                    break;
                case GroundSurfaceType.type2:
                    backT_w_top_dis = backT_A1;
                    backT_w_bottom_dis = 0;
                    backT_h = backT_B;
                    backT_d = wall_d;
                    backT_w_bottom = backCubeLength;
                    backT_w_top = backCubeLength - backT_A1;
                    
                    break;
                case GroundSurfaceType.type3:
                    backT_w_top_dis = backT_A1 + backT_A2;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = backT_B;
                    backT_d = wall_d;
                    backT_w_bottom = backCubeLength - backT_A1;
                    backT_w_top = backCubeLength - backT_A1 - backT_A2;
                    
                    break;
                default:
                    break;
            }
            bool isbackTTextured = false;
            Uri bacTTextureUri = null;
            if (StaticVariables.viewModel.soilLayerDatas.Count > 0)
            {
                if(StaticVariables.viewModel.soilLayerDatas[0].Soil != null)
                {
                    backT_color = StaticVariables.viewModel.soilLayerDatas[0].Soil.SoilColor;
                    isbackTTextured = StaticVariables.viewModel.soilLayerDatas[0].Soil.isSoilTexture;
                    bacTTextureUri = StaticVariables.viewModel.soilLayerDatas[0].Soil.SoilTexture.TextureUri;

                }
            }
            Point3D BackTCenter = new Point3D(center3d.X, center3d.Y + centerY + backT_h, center3d.Z - wall_d / 2);
            WpfTrapezoid backT = new WpfTrapezoid(BackTCenter, backT_w_top, backT_w_bottom, backT_h, backT_d, backT_w_top_dis, backT_w_bottom_dis);
            GeometryModel3D backTmodel = WpfTrapezoid.CreateTrapezoidModel(backT, backT_color, isbackTTextured, bacTTextureUri);
            //create backWater model
            Point3D backWCenter = new Point3D(center3d.X + 0.1, center3d.Y - groundW_h1 + centerY - 0.01, center3d.Z - backW_d / 2);
            WpfCube backW = new WpfCube(backWCenter, backW_w, backW_h, backW_d);
            GeometryModel3D backWmodel = WpfCube.CreateCubeModel(backW, Color.FromArgb(150, 0, 0, 255), false, soilUri1);

            groupScene.Children.Add(backWmodel);
            groupScene.Children.Add(backTmodel);

            //create backcubemodel
            double soiltotalHeight = 0;
            foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
            {
                if(soilLayer != null)
                {
                    soiltotalHeight += soilLayer.LayerHeight;
                    if (soiltotalHeight <= wall_h)
                    {
                        double soilbackCube_w = backCubeLength;
                        double soilbackCube_h = soilLayer.LayerHeight;
                        double soilbackCube_d = wall_d;
                        Point3D soilbackCubeCenter = new Point3D(center3d.X, center3d.Y + centerY - soiltotalHeight + soilLayer.LayerHeight, center3d.Z - soilbackCube_d / 2);
                        WpfCube soilbackCube = new WpfCube(soilbackCubeCenter, soilbackCube_w, soilbackCube_h, soilbackCube_d);
                        if(soilLayer.Soil != null)
                        {
                            GeometryModel3D soilbackCubeModel = WpfCube.CreateCubeModel(soilbackCube, soilLayer.Soil.SoilColor, soilLayer.Soil.isSoilTexture, soilLayer.Soil.SoilTexture.TextureUri);
                            groupScene.Children.Add(soilbackCubeModel);
                        }
                        else
                        {
                            GeometryModel3D soilbackCubeModel = WpfCube.CreateCubeModel(soilbackCube, Color.FromArgb(100, 200, 200, 200), false, soilUri);
                            groupScene.Children.Add(soilbackCubeModel);
                        }
                        

                    }
                }
                
            }
            if (soiltotalHeight< wall_h)
            {
                double backCube_w = backCubeLength;
                double backCube_h = wall_h-soiltotalHeight;
                double backCube_d = wall_d;
                Point3D backCubeCenter = new Point3D(center3d.X, center3d.Y + centerY-soiltotalHeight, center3d.Z - backCube_d / 2);
                WpfCube backCube = new WpfCube(backCubeCenter, backCube_w, backCube_h, backCube_d);
                GeometryModel3D backCubeModel = WpfCube.CreateCubeModel(backCube, Color.FromArgb(100, 200, 200, 200), false, soilUri);
                groupScene.Children.Add(backCubeModel);

            }



            double frontCube_w = frontCubeLength;
            double frontCube_h = Math.Clamp(wall_h - excavationHeight, 0, double.MaxValue);
            double frontCube_d = wall_d;
            Point3D frontCubeCenter = new Point3D(center3d.X - wall_t - frontCube_w, center3d.Y + centerY - (wall_h - frontCube_h), center3d.Z - frontCube_d / 2);

            double frontT_w_top_dis =0;
            double frontT_w_bottom_dis =0;
            double frontT_h =0;
            double frontT_d =0;
            double frontT_w_bottom  =0;
            double frontT_w_top  =0;
            Point3D TrapezoidCenter = new Point3D(frontCubeCenter.X + frontT_w_bottom_dis, frontCubeCenter.Y + frontT_h, frontCubeCenter.Z);
            Color frontT_color = Color.FromArgb(100, 200, 200, 200);
            switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
            {
                case ExcavationType.none:                     
                    break;
                case ExcavationType.type1:
                     frontT_w_top_dis = frontCube_w-frontT_X2;
                     frontT_w_bottom_dis = frontCube_w  - frontT_X1 -frontT_X2;
                     frontT_h = frontT_Z;
                     frontT_d = wall_d;
                     frontT_w_bottom = frontT_X1 + frontT_X2;
                     frontT_w_top =  frontT_X2;
                    TrapezoidCenter = new Point3D(frontCubeCenter.X , frontCubeCenter.Y + frontT_h, frontCubeCenter.Z);
                    
                    break;
                case ExcavationType.type2:
                    frontCube_h = Math.Clamp(wall_h - excavationHeight-frontT_Z, 0, double.MaxValue);
                    frontCubeCenter = new Point3D(center3d.X - wall_t - frontCube_w, center3d.Y + centerY - (wall_h - frontCube_h), center3d.Z - frontCube_d / 2);
                    frontT_w_top_dis = 0;
                     frontT_w_bottom_dis = 0;
                     frontT_h = frontT_Z;
                     frontT_d = wall_d;
                     frontT_w_bottom = frontCube_w - frontT_w_bottom_dis - frontT_X2;
                     frontT_w_top = frontCube_w - frontT_X1 - frontT_X2;
                    TrapezoidCenter = new Point3D(frontCubeCenter.X + frontT_w_bottom_dis, frontCubeCenter.Y + frontT_h, frontCubeCenter.Z);
                    break;
                default:                     
                    break;
            }
            
            WpfCube frontCube = new WpfCube(frontCubeCenter, frontCube_w, frontCube_h, frontCube_d);
            GeometryModel3D frontCubeModel = WpfCube.CreateCubeModel(frontCube, Color.FromArgb(100, 200, 200, 200), false, soilUri);


            WpfTrapezoid frontT = new WpfTrapezoid(TrapezoidCenter, frontT_w_top, frontT_w_bottom, frontT_h, frontT_d, frontT_w_top_dis, frontT_w_bottom_dis);
            GeometryModel3D frontTmodel = WpfTrapezoid.CreateTrapezoidModel(frontT, frontT_color,false,soilUri);
                        
                                 
            Point3D frontWCenter = new Point3D(center3d.X - wall_t - frontW_w, center3d.Y +centerY- groundW_h2 - 0.01 - excavationHeight, center3d.Z - frontW_d / 2);
            WpfCube frontW = new WpfCube(frontWCenter, frontW_w, frontW_h, frontW_d);
            GeometryModel3D frontWmodel = WpfCube.CreateCubeModel(frontW, Color.FromArgb(150, 0, 0, 255), false, soilUri);

            Point3D bottomTCenter = new Point3D(center3d.X-frontCube_w-wall_t, center3d.Y - centerY +bottomT_h , center3d.Z - wall_d / 2);
            WpfCube bottomT = new WpfCube(bottomTCenter, wall_t+(frontCubeLength+backCubeLength), bottomT_h, wall_d);
            GeometryModel3D bottomTmodel = WpfCube.CreateCubeModel(bottomT, Color.FromArgb(100, 200, 200, 200), false, soilUri);


            
            groupScene.Children.Add(frontWmodel);
            groupScene.Children.Add(frontTmodel);
            groupScene.Children.Add(frontCubeModel);
            

            //groupScene.Children.Add(backCubeModel);
            groupScene.Children.Add(bottomTmodel);
                            

            groupScene.Children.Add(leftLight());
            groupScene.Children.Add(new AmbientLight(Colors.Gray));

            viewport.Camera = camera();

                ModelVisual3D visual = new ModelVisual3D();

                visual.Content = groupScene;

                viewport.Children.Add(visual);

            //ScreenSortingHelper.AlphaSort(
            //        camera().Position,
            //        groupScene.Children,
            //        groupScene.Transform
            //    );
        }
        public void ChangeModelTransform()
        {
            AxisAngleRotation3D rotationX = new AxisAngleRotation3D(new Vector3D(0, 1, 0), rotationAngleX);

            RotateTransform3D rotateTransformX = new RotateTransform3D(rotationX, center3d);

            AxisAngleRotation3D rotationY = new AxisAngleRotation3D(new Vector3D(1, 0, 1), rotationAngleY);

            RotateTransform3D rotateTransformY = new RotateTransform3D(rotationY, center3d);

            Transform3DGroup myTransform3DGroup = new Transform3DGroup();

            myTransform3DGroup.Children.Add(rotateTransformX);
            myTransform3DGroup.Children.Add(rotateTransformY);

            Vector3D scaleVector = new Vector3D(scaleFactor, scaleFactor, scaleFactor);
            ScaleTransform3D scaleTransform3D = new ScaleTransform3D(scaleVector, center3d);
            myTransform3DGroup.Children.Add(scaleTransform3D);

            groupScene.Transform = myTransform3DGroup;
            //ScreenSortingHelper.AlphaSort(
            //        camera().Position,
            //        groupScene.Children,
            //        groupScene.Transform
            //    );
            //scroll_3dview.Content = scaleFactor;
        }
            
        
        private void viewport3d_main_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            scaleFactor += e.Delta/100;
            scaleFactor = Math.Clamp(scaleFactor, minScaleFactor, maxScaleFactor);
            ChangeModelTransform();
        }

        private void Position_Reset_bttn_Click(object sender, RoutedEventArgs e)
        {            
            scaleFactor = 15;
            rotationAngleX = 0;
            rotationAngleY = 0; 
            ChangeModelTransform();            
        }

        private void viewport3d_main_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseFirstPos = e.GetPosition(this);
            if (e.MiddleButton == MouseButtonState.Pressed) isMouseWheelDown = true;

        }

        private void viewport3d_main_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            if (e.MiddleButton == MouseButtonState.Released) isMouseWheelDown = false;
        }

        private void viewport3d_main_MouseMove(object sender, MouseEventArgs e)
        {
            
            if(isMouseWheelDown)
            {
                var mousePos = e.GetPosition(this);
                mousePosDiff = mousePos - mouseFirstPos;
                rotationAngleX += (double)mousePosDiff.X/5;
                rotationAngleY += (double)mousePosDiff.Y/5;
                ChangeModelTransform();
                mouseFirstPos = mousePos;
            }
            
        }

        private void viewport3d_main_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseWheelDown = false;
        }
        
        public void Refreshview()       
        {
            groupScene.Children.Clear();
            StartViewport3d();
            ChangeModelTransform();
        }
                
    }
}
