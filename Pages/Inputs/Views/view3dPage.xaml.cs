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
        float scaleFactor = 1f;
        Point3D lookat = new Point3D(0, 0, 0);
        Point mouseFirstPos;
        bool isMouseWheelDown;
        float rotationAngleX;
        float rotationAngleY;
        Vector mousePosDiff;
        Point3D center3d = new Point3D(0, 0, 0);
        public View3dPage()
        {
            InitializeComponent();
            StartViewport3d();
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

            perspectiveCamera.Position = new Point3D(-WpfScene.sceneSize, WpfScene.sceneSize / 2, WpfScene.sceneSize);

            perspectiveCamera.LookDirection = new Vector3D(lookat.X - perspectiveCamera.Position.X,
                                                           lookat.Y - perspectiveCamera.Position.Y,
                                                           lookat.Z - perspectiveCamera.Position.Z);

            perspectiveCamera.FieldOfView = 60;

            return perspectiveCamera;
        }

        private void StartViewport3d()
        {
            
                viewport = viewport3d_main;

                WpfCylinder WpfCylinder = new WpfCylinder(
                    new Point3D(0, WpfScene.sceneSize / 4, WpfScene.sceneSize / 5),
                    40, WpfScene.sceneSize / 8,
                    WpfScene.sceneSize / 8,
                    WpfScene.sceneSize / 6);

                WpfCylinder WpfCylinder2 = new WpfCylinder(
                    new Point3D(-WpfScene.sceneSize / 2, WpfScene.sceneSize / 4, 0),
                    40, WpfScene.sceneSize / 8,
                    WpfScene.sceneSize / 8,
                    WpfScene.sceneSize / 6);


                WpfCircle circle = new WpfCircle(55,
                    new Point3D(-WpfScene.sceneSize / 2, WpfScene.sceneSize / 6, WpfScene.sceneSize / 2), WpfScene.sceneSize / 15);

                WpfCircle circle2 = new WpfCircle(55,
                    new Point3D(0, WpfScene.sceneSize / 6, WpfScene.sceneSize / 2), WpfScene.sceneSize / 15);


                GeometryModel3D WpfCylinderModel = WpfCylinder.CreateModel(Color.FromArgb(55, 255, 0, 0), true, true);

                GeometryModel3D WpfCylinderModel2 = WpfCylinder2.CreateModel(Colors.AliceBlue, true, true);

                GeometryModel3D circleModel = circle.createModel(Colors.Azure, false);
                GeometryModel3D circleModel2 = circle2.createModelTwoSided(Colors.Aqua, false);


                double floorThickness = WpfScene.sceneSize / 100;
                GeometryModel3D floorModel = WpfCube.CreateCubeModel(
                    new Point3D(-WpfScene.sceneSize / 2,
                                -floorThickness,
                                -WpfScene.sceneSize / 2),
                    WpfScene.sceneSize, floorThickness, WpfScene.sceneSize, Colors.Tan,true);

                groupScene = new Model3DGroup();

                groupScene.Children.Add(floorModel);

                groupScene.Children.Add(WpfCylinderModel);
                groupScene.Children.Add(WpfCylinderModel2);
                groupScene.Children.Add(circleModel);
                groupScene.Children.Add(circleModel2);


                groupScene.Children.Add(leftLight());
                groupScene.Children.Add(new AmbientLight(Colors.Gray));

                viewport.Camera = camera();

                ModelVisual3D visual = new ModelVisual3D();

                visual.Content = groupScene;

                viewport.Children.Add(visual);

                //turnModel(WpfCylinder.getCenter(), groupScene);
                //turnModel(WpfCylinder2.getCenter(), WpfCylinderModel2);

                //turnModel(circle.getCenter(), circleModel);
                //turnModel(circle2.getCenter(), circleModel2);
            

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
        }
            
        
        private void viewport3d_main_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            scaleFactor += 0.5f*e.Delta/1000;
            scaleFactor = Math.Clamp(scaleFactor, 0.3f, 3);
            ChangeModelTransform();
        }

        private void Position_Reset_bttn_Click(object sender, RoutedEventArgs e)
        {            
            scaleFactor = 1f;
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
                rotationAngleX += (float)mousePosDiff.X/5;
                rotationAngleY += (float)mousePosDiff.Y/5;
                ChangeModelTransform();
                mouseFirstPos = mousePos;
            }
            
        }

        private void viewport3d_main_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseWheelDown = false;
        }
    }
}
