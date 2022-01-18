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
        double scaleFactor = 0.2f;
        private double minScaleFactor = 0.1f;
        private double maxScaleFactor = 1f;
        Point3D lookat = new Point3D(0, 0, 0);
        Point mouseFirstPos;
        bool isMouseWheelDown;
        double rotationAngleX;
        double rotationAngleY;
        Vector mousePosDiff;
        Point3D center3d = new Point3D(0, 0, 0);
        double wall_t=50;
        double wall_h=1200; 
        double wall_d=700;
        double frontandbackCubeLength = 1500;
        double excavationHeight = 800;
        double frontT_Z = 200;
        double frontT_X1 = 200;
        double frontT_X2 = 100;

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
            //center3d = new Point3D(center3d.X - wall_w,center3d.Y,center3d.Z -wall_d/2);
            
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

            double cylinder_h = 700;
            double cylinder_d = 15;
            double cylinder_loc = 100;
            Point3D cylinderCenter = new Point3D(center3d.X-wall_t-20,center3d.Y + wall_h / 2 - cylinder_loc,center3d.Z);
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
            groupScene.Children.Add(frontTmodel);
            groupScene.Children.Add(frontCubeModel);
            groupScene.Children.Add(backCubeModel);
                

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
            scaleFactor += 0.25f*e.Delta/1000;
            scaleFactor = Math.Clamp(scaleFactor, minScaleFactor, maxScaleFactor);
            ChangeModelTransform();
        }

        private void Position_Reset_bttn_Click(object sender, RoutedEventArgs e)
        {            
            scaleFactor = 0.2f;
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
            groupScene.Children.Clear();
            StartViewport3d();
            ChangeModelTransform();
        }
        public void ChangeWallThickness(double d)
        {
            wall_t = d;
            groupScene.Children.Clear();
            StartViewport3d();
            ChangeModelTransform();
        }
        public void ChangeexcavationHeight(double exHeight)
        {
            excavationHeight = exHeight;
            groupScene.Children.Clear();
            StartViewport3d();
            ChangeModelTransform();
        }public void ChangeexcavationZ(double exZ)
        {
            frontT_Z = exZ;
            groupScene.Children.Clear();
            StartViewport3d();
            ChangeModelTransform();
        }public void ChangeexcavationX1(double exX1)
        {
            frontT_X1 = exX1;
            groupScene.Children.Clear();
            StartViewport3d();
            ChangeModelTransform();
        }public void ChangeexcavationX2(double exX2)
        {
            frontT_X2 = exX2;
            groupScene.Children.Clear();
            StartViewport3d();
            ChangeModelTransform();
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

        public void ChangeExcavationType()
        {
            groupScene.Children.Clear();
            StartViewport3d();
            ChangeModelTransform();
        }
    }
}
