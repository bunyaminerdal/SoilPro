using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace ExDesign.Scripts
{
    internal class WpfCylinder
    {
        private WpfCircle front;

        private WpfCircle back;

        private int nSides;

        private double frontRadius;

        private double backRadius;

        private double length;

        private Point3D center;

        private Point3D backcenter;

        public Point3D getCenter()
        {
            return center;
        }

        public WpfCylinder(Point3D Center, int NSides, double FrontRadius, double BackRadius,
            double Length)
        {
            center = Center;
            nSides = NSides;
            frontRadius = FrontRadius;
            backRadius = BackRadius;
            length = Length;

            front = new WpfCircle(nSides, center, frontRadius);

            backcenter = new Point3D(center.X, center.Y, center.Z - length);

            back = new WpfCircle(nSides, backcenter, backRadius);
        }

        public WpfCylinder(Point3D Center, int NSides, double FrontRadius, double BackRadius,
                            double Length, Point3D rotation_point, double radians)
        {
            center = Center;
            nSides = NSides;
            frontRadius = FrontRadius;
            backRadius = BackRadius;
            length = Length;

            front = new WpfCircle(nSides, center, frontRadius);
            backcenter = new Point3D(center.X, center.Y, center.Z - length);
            back = new WpfCircle(nSides, backcenter, backRadius);

            RotateZY(rotation_point, radians);
        }

        public void RotateZY(Point3D rotation_point, double radians)
        {
            front.RotateZY(rotation_point, radians);
            back.RotateZY(rotation_point, radians);
            backcenter = WpfUtils.RotatePointZY(backcenter, rotation_point, radians);
        }

        public void RotateXZ(Point3D rotation_point, double radians)
        {
            front.RotateXZ(rotation_point, radians);
            back.RotateXZ(rotation_point, radians);
            backcenter = WpfUtils.RotatePointXZ(backcenter, rotation_point, radians);
        }

        public void RotateXY(Point3D rotation_point, double radians)
        {
            front.RotateXY(rotation_point, radians);
            back.RotateXY(rotation_point, radians);
            backcenter = WpfUtils.RotatePointXY(backcenter, rotation_point, radians);
        }

        public void addToMesh(MeshGeometry3D mesh)
        {
            addToMesh(mesh, false, false);
        }

        public void addToMesh(MeshGeometry3D mesh, bool encloseTop, bool combineVertices)
        {
            if (front.getPoints().Count > 2)
            {
                List<Point3D> frontPoints = new List<Point3D>();
                foreach (Point3D p in front.getPoints())
                {
                    frontPoints.Add(p);
                }
                frontPoints.Add(front.getPoints()[0]);

                List<Point3D> backPoints = new List<Point3D>();
                foreach (Point3D p in back.getPoints())
                {
                    backPoints.Add(p);
                }
                backPoints.Add(back.getPoints()[0]);

                for (int i = 1; i < frontPoints.Count; i++)
                {
                    WpfTriangle.addTriangleToMesh(frontPoints[i - 1], backPoints[i - 1], frontPoints[i], mesh, combineVertices);
                    WpfTriangle.addTriangleToMesh(frontPoints[i], backPoints[i - 1], backPoints[i], mesh, combineVertices);
                }

                if (encloseTop)
                {
                    front.addToMesh(mesh, false);
                    back.addToMesh(mesh, false);
                }
            }
        }

        public GeometryModel3D CreateModel(Color color)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addToMesh(mesh);

            Material material = new DiffuseMaterial(new SolidColorBrush(color));

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            return model;
        }

        public GeometryModel3D CreateModel(Color color, bool enclose, bool combine)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addToMesh(mesh, enclose, combine);


            //Image image = new Image();
            //image.Source = new BitmapImage(new Uri(@"../../../Texture/texturedeneme.png", UriKind.Relative));

            //Material material = new DiffuseMaterial(new ImageBrush(image.Source));

            Material material = new DiffuseMaterial(new SolidColorBrush(color));

            
            GeometryModel3D model = new GeometryModel3D(mesh, material);

            model.BackMaterial = material;

            return model;
        }

    }
}
