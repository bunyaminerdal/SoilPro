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
        public View3dPage()
        {
            InitializeComponent();
            StartViewport3d();

        }
     
        Point3D lookat = new Point3D(0, 0, 0);

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
            
                Viewport3D viewport = viewport3d_main;

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

                GeometryModel3D circleModel = circle.createModel(Colors.Aqua, false);
                GeometryModel3D circleModel2 = circle2.createModelTwoSided(Colors.Aqua, false);


                double floorThickness = WpfScene.sceneSize / 100;
                GeometryModel3D floorModel = WpfCube.CreateCubeModel(
                    new Point3D(-WpfScene.sceneSize / 2,
                                -floorThickness,
                                -WpfScene.sceneSize / 2),
                    WpfScene.sceneSize, floorThickness, WpfScene.sceneSize, Colors.Tan);

                Model3DGroup groupScene = new Model3DGroup();

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

                turnModel(WpfCylinder.getCenter(), WpfCylinderModel);
                turnModel(WpfCylinder2.getCenter(), WpfCylinderModel2);

                turnModel(circle.getCenter(), circleModel);
                turnModel(circle2.getCenter(), circleModel2);
            

        }
        public void turnModel(Point3D center, GeometryModel3D model)
        {
            Vector3D vector = new Vector3D(0, 1, 0);

            AxisAngleRotation3D rotation = new AxisAngleRotation3D(vector, 0.0);

            RotateTransform3D rotateTransform = new RotateTransform3D(rotation, center);

            model.Transform = rotateTransform;

        }



               
    }
}
