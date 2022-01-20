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
using SoilPro.Scripts;
using System.Windows.Media.Animation;

namespace SoilPro.Pages.Inputs.Views
{
    /// <summary>
    /// view3dPage.xaml etkileşim mantığı
    /// </summary>
    public partial class View3dPage : Page
    {
        public  Viewport3D viewport;
        public Model3DGroup groupScene;
        double scaleFactor = 15;
        private double minScaleFactor = 8;
        private double maxScaleFactor = 30;
        Point3D lookat = new Point3D(0, 0, 0);
        Point mouseFirstPos;
        bool isMouseWheelDown;
        double rotationAngleX;
        double rotationAngleY;
        Vector mousePosDiff;
        Point3D center3d = new Point3D(0, 0, 0);
        double wall_t=0.5;
        double wall_h=12; 
        double wall_d=7;
        double frontandbackCubeLength = 15;
        double excavationHeight = 8;
        double frontT_Z = 2;
        double frontT_X1 = 2;
        double frontT_X2 = 1;
        double backT_Beta = 10;
        double backT_B = 1;
        double backT_A1 = 1;
        double backT_A2 = 2;
        double bottomT_h =3;
        double groundW_h1 = 5;
        double groundW_h2 = 2;
        

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

        private void StartViewport3d()
        {
                        
            Point3D wallCenter = new Point3D(center3d.X-wall_t, center3d.Y+wall_h/2, center3d.Z-wall_d/2);
            WpfCube wallCube = new WpfCube(wallCenter, wall_t, wall_h, wall_d);
            GeometryModel3D wallModel = WpfCube.CreateCubeModel(wallCube, Colors.DarkGray);
                       
            double backCube_w = frontandbackCubeLength;
            double backCube_h = wall_h;
            double backCube_d = wall_d;
            Point3D backCubeCenter = new Point3D(center3d.X, center3d.Y + wall_h / 2, center3d.Z-backCube_d/2);
            WpfCube backCube = new WpfCube(backCubeCenter, backCube_w, backCube_h, backCube_d);
            GeometryModel3D backCubeModel=WpfCube.CreateCubeModel(backCube, Color.FromArgb(100,200, 200, 200));

            double frontCube_w = frontandbackCubeLength;
            double frontCube_h = wall_h- excavationHeight;
            double frontCube_d = wall_d;
            Point3D frontCubeCenter = new Point3D(center3d.X - wall_t - frontCube_w, center3d.Y + wall_h / 2 - (backCube_h-frontCube_h), center3d.Z-frontCube_d/2);
            WpfCube frontCube = new WpfCube(frontCubeCenter, frontCube_w, frontCube_h, frontCube_d);
            GeometryModel3D frontCubeModel = WpfCube.CreateCubeModel(frontCube, Color.FromArgb(100, 200, 200, 200));

            double cylinder_h = 7;
            double cylinder_d = 0.15;
            double cylinder_loc = 1;
            Point3D cylinderCenter = new Point3D(center3d.X-wall_t-0.2,center3d.Y + wall_h / 2 - cylinder_loc,center3d.Z);
            WpfCylinder anchor = new WpfCylinder(cylinderCenter,30,cylinder_d,cylinder_d,cylinder_h);
            GeometryModel3D cylinderModel = anchor.CreateModel(Colors.Blue,true,true);
            
            double frontT_w_top_dis =0;
            double frontT_w_bottom_dis =0;
            double frontT_h =0;
            double frontT_d =0;
            double frontT_w_bottom  =0;
            double frontT_w_top  =0;
            Point3D TrapezoidCenter = new Point3D(frontCubeCenter.X + frontT_w_bottom_dis, frontCubeCenter.Y + frontT_h, frontCubeCenter.Z);
            Color frontT_color = Colors.Transparent;
            switch (StaticVariables.excavationType)
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
                    frontT_color = Color.FromArgb(100, 200, 200, 200);
                    break;
                case ExcavationType.type2:
                     frontT_w_top_dis = 0;
                     frontT_w_bottom_dis = 0;
                     frontT_h = frontT_Z;
                     frontT_d = wall_d;
                     frontT_w_bottom = frontCube_w - frontT_w_bottom_dis - frontT_X2;
                     frontT_w_top = frontCube_w - frontT_X1 - frontT_X2;
                    TrapezoidCenter = new Point3D(frontCubeCenter.X + frontT_w_bottom_dis, frontCubeCenter.Y + frontT_h, frontCubeCenter.Z);
                    frontT_color = Color.FromArgb(100, 200, 200, 200);
                    break;
                default:                     
                    break;
            }
            
            
            WpfTrapezoid frontT = new WpfTrapezoid(TrapezoidCenter, frontT_w_top, frontT_w_bottom, frontT_h, frontT_d, frontT_w_top_dis, frontT_w_bottom_dis);
            GeometryModel3D frontTmodel = WpfTrapezoid.CreateTrapezoidModel(frontT, frontT_color);

            double backT_w_top_dis = 0;
            double backT_w_bottom_dis = 0;
            double backT_h = 0;
            double backT_d = 0;
            double backT_w_bottom = 0;
            double backT_w_top = 0;

            
            Color backT_color = Colors.Transparent;
            switch (StaticVariables.groundSurfaceType)
            {
                case GroundSurfaceType.flat:
                    break;
                case GroundSurfaceType.type1:
                    backT_w_top_dis = backCube_w;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = Math.Sin(backT_Beta * Math.PI / 180) * (backCube_w - backT_A1);
                    backT_d = wall_d;
                    backT_w_bottom = backCube_w - backT_A1;
                    backT_w_top = 0;                    
                    backT_color = Color.FromArgb(100, 200, 200, 200);
                    break;
                case GroundSurfaceType.type2:
                    backT_w_top_dis = backT_A1;
                    backT_w_bottom_dis = 0;
                    backT_h = backT_B;
                    backT_d = wall_d;
                    backT_w_bottom = backCube_w ;
                    backT_w_top = backCube_w -backT_A1;
                    backT_color = Color.FromArgb(100, 200, 200, 200);
                    break;
                case GroundSurfaceType.type3:
                    backT_w_top_dis = backT_A1+backT_A2;
                    backT_w_bottom_dis = backT_A1;
                    backT_h = backT_B;
                    backT_d = wall_d;
                    backT_w_bottom = backCube_w- backT_A1;
                    backT_w_top = backCube_w - backT_A1 -backT_A2;
                    backT_color = Color.FromArgb(100, 200, 200, 200);
                    break;
                default:                     
                    break;
            }
            
            Point3D BackTCenter = new Point3D(backCubeCenter.X, backCubeCenter.Y + backT_h, backCubeCenter.Z);
            WpfTrapezoid backT = new WpfTrapezoid(BackTCenter, backT_w_top, backT_w_bottom, backT_h, backT_d, backT_w_top_dis, backT_w_bottom_dis);
            GeometryModel3D backTmodel = WpfTrapezoid.CreateTrapezoidModel(backT, backT_color);

            double backW_w = 0;
            //double backW_h = backCube_h + bottomT_h - groundW_h1;
            double backW_h =0;
            double backW_d = 0;
            double frontW_w = 0;
            //double frontW_h = frontCube_h + bottomT_h - groundW_h2;
            double frontW_h = 0;
            double frontW_d = 0;
            switch (StaticVariables.groundWaterType)
            {
                case GroundWaterType.none:                     
                    break;                
                default:
                     backW_w = frontandbackCubeLength + wall_t - 0.2;
                     backW_h = 0.1;
                     backW_d = wall_d - 0.1;
                     frontW_w = frontandbackCubeLength - 0.1;
                     frontW_h = 0.1;
                     frontW_d = wall_d - 0.1;
                    break;
            }
            
            Point3D backWCenter = new Point3D(center3d.X - wall_t + 0.1, center3d.Y - groundW_h1 + wall_h / 2-0.01, center3d.Z - backW_d / 2);
            WpfCube backW = new WpfCube(backWCenter, backW_w, backW_h, backW_d);
            GeometryModel3D backWmodel = WpfCube.CreateCubeModel(backW, Color.FromArgb(100, 0, 0, 255));                        
            Point3D frontWCenter = new Point3D(center3d.X - wall_t - frontW_w, center3d.Y - groundW_h2 + wall_h / 2 - 0.01 - (backCube_h - frontCube_h), center3d.Z - frontW_d / 2);
            WpfCube frontW = new WpfCube(frontWCenter, frontW_w, frontW_h, frontW_d);
            GeometryModel3D frontWmodel = WpfCube.CreateCubeModel(frontW, Color.FromArgb(100, 0, 0, 255));


            Point3D bottomTCenter = new Point3D(center3d.X-frontCube_w-wall_t, center3d.Y - wall_h / 2 , center3d.Z - wall_d / 2);
            WpfCube bottomT = new WpfCube(bottomTCenter, wall_t+frontCube_w+backCube_w, bottomT_h, wall_d);
            GeometryModel3D bottomTmodel = WpfCube.CreateCubeModel(bottomT, Color.FromArgb(100, 200, 200, 200));

            AxisAngleRotation3D rotationX = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 270);
            RotateTransform3D rotateTransformX = new RotateTransform3D(rotationX, cylinderCenter);
            AxisAngleRotation3D rotationY = new AxisAngleRotation3D(new Vector3D(0, 0, 1), -30);
            RotateTransform3D rotateTransformY = new RotateTransform3D(rotationY, cylinderCenter);
            Transform3DGroup myTransform3DGroup = new Transform3DGroup();
            myTransform3DGroup.Children.Add(rotateTransformX); 
            myTransform3DGroup.Children.Add(rotateTransformY); 
            cylinderModel.Transform = myTransform3DGroup;

            groupScene = new Model3DGroup();
            groupScene.Children.Add(wallModel);
            groupScene.Children.Add(cylinderModel);
            groupScene.Children.Add(frontWmodel);
            groupScene.Children.Add(frontTmodel);
            groupScene.Children.Add(frontCubeModel);
            groupScene.Children.Add(backWmodel);
            groupScene.Children.Add(backTmodel);
            groupScene.Children.Add(backCubeModel);
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
            scroll_3dview.Content = scaleFactor;
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
        public void ChangeWallHeight(double h)
        {
            wall_h = h;
            Refresh3Dview();
        }
        public void ChangeWallThickness(double d)
        {
            wall_t = d;
            Refresh3Dview();
        }
        public void ChangeexcavationHeight(double exHeight)
        {
            excavationHeight = exHeight;
            Refresh3Dview();
        }
        public void ChangeexcavationZ(double exZ)
        {
            frontT_Z = exZ;
            Refresh3Dview();
        }
        public void ChangeexcavationX1(double exX1)
        {
            frontT_X1 = exX1;
            Refresh3Dview();
        }
        public void ChangeexcavationX2(double exX2)
        {
            frontT_X2 = exX2;
            Refresh3Dview();
        }
        public void ChangeSurfaceBeta(double surfaceBeta)
        {
            backT_Beta = surfaceBeta;
            Refresh3Dview();
        }
        public void ChangeSurfaceB(double surfaceB)
        {
            backT_B = surfaceB;
            Refresh3Dview();
        }
        public void ChangeSurfaceA1(double surfaceA1)
        {
            backT_A1 = surfaceA1;
            Refresh3Dview();
        }
        public void ChangeSurfaceA2(double surfaceA2)
        {
            backT_A2 = surfaceA2;
            Refresh3Dview();
        }
        public void ChangeGroundWaterH1(double gw_h1)
        {
            groundW_h1 = gw_h1;
            Refresh3Dview();
        }
        public void ChangeGroundWaterH2(double gw_h2)
        {
            groundW_h2 = gw_h2;
            Refresh3Dview();
        }
        public double GetWallHeight()
        { return wall_h; }
        public double GetWallThickness()
        { return wall_t; }
        public double GetexcavationHeight()
        { return excavationHeight; }
        public double GetexcavationZ()
        { return frontT_Z; }
        public double GetexcavationX1()
        { return frontT_X1; }
        public double GetexcavationX2()
        { return frontT_X2; }
        public double GetSurfaceBeta()
        { return backT_Beta; }
        public double GetSurfaceB()
        { return backT_B; }
        public double GetSurfaceA1()
        { return backT_A1; }
        public double GetSurfaceA2()
        { return backT_A2; }
        public double GetGroundWaterH1()
        { return groundW_h1; }
        public double GetGroundWaterH2()
        { return groundW_h2; }
        public void Refresh3Dview()
        {
            groupScene.Children.Clear();
            StartViewport3d();
            ChangeModelTransform();
        }

        
    }
}
