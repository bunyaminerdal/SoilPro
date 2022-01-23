using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace ExDesign.Scripts
{
    internal class WpfUtils
    {
        static double one_rad_in_degrees = (double)57.0 + ((double)17.0 / (double)60.0) + ((double)44.6 / ((double)3600.0));

        public static Point3D RotatePointXY(Point3D p, Point3D rotation_point, double radians)
        {
            Point3D new_point = new Point3D(rotation_point.X, rotation_point.Y, rotation_point.Z);

            try
            {
                if (radians != 0)
                {
                    double ydiff = p.Y - rotation_point.Y;
                    double xdiff = p.X - rotation_point.X;

                    double xd = (xdiff * Math.Cos(radians)) - (ydiff * Math.Sin(radians));

                    double yd = (xdiff * Math.Sin(radians)) + (ydiff * Math.Cos(radians));

                    new_point.X += xd;
                    new_point.Y += yd;
                    new_point.Z = p.Z;
                }
                else
                {
                    new_point.X = p.X;
                    new_point.Y = p.Y;
                    new_point.Z = p.Z;
                }
            }
            catch
            {

            }

            return new_point;
        }

        public static Point3D RotatePointXZ(Point3D p, Point3D rotation_point, double radians)
        {
            Point3D new_point = new Point3D(rotation_point.X, rotation_point.Y, rotation_point.Z);

            try
            {
                if (radians != 0)
                {
                    double ydiff = p.Z - rotation_point.Z;
                    double xdiff = p.X - rotation_point.X;

                    double xd = (xdiff * Math.Cos(radians)) - (ydiff * Math.Sin(radians));

                    double yd = (xdiff * Math.Sin(radians)) + (ydiff * Math.Cos(radians));

                    new_point.X += xd;
                    new_point.Z += yd;
                    new_point.Y = p.Y;
                }
                else
                {
                    new_point.X = p.X;
                    new_point.Y = p.Y;
                    new_point.Z = p.Z;
                }
            }
            catch
            {

            }

            return new_point;
        }

        public static Point3D RotatePointZY(Point3D p, Point3D rotation_point, double radians)
        {
            Point3D new_point = new Point3D(rotation_point.X, rotation_point.Y, rotation_point.Z);

            try
            {
                if (radians != 0)
                {
                    double ydiff = p.Y - rotation_point.Y;
                    double xdiff = p.Z - rotation_point.Z;

                    double xd = (xdiff * Math.Cos(radians)) - (ydiff * Math.Sin(radians));
                    double yd = (xdiff * Math.Sin(radians)) + (ydiff * Math.Cos(radians));

                    new_point.Z += xd;
                    new_point.Y += yd;
                    new_point.X = p.X;
                }
                else
                {
                    new_point.X = p.X;
                    new_point.Y = p.Y;
                    new_point.Z = p.Z;
                }
            }
            catch
            {

            }

            return new_point;
        }


        public static double radians_from_degrees(double degrees)
        {
            return degrees / one_rad_in_degrees;
        }

        public static double degrees_from_radians(double radians)
        {
            return radians * one_rad_in_degrees;
        }

        
        public static double GetDimension(double value)
        {
            double newValue;
            newValue =Math.Round( value * StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit],10);
            return newValue;
        }
        public static double GetValue(double value)
        {
            double newValue;
            newValue =Math.Round( value / StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit],10);
            return newValue;
        }

        public static void OpenWindow(Window window)
        {
            window.Show();
            foreach (Window win in App.Current.Windows)
            {
                if (win != window) win.Close();
                
            }
            App.Current.MainWindow = window;

        }
        public static WallType GetWallType(int index)
        {
            switch (index)
            {
                case 0:
                    return WallType.ConcreteRectangleWall;
                case 1:
                    return WallType.ConcretePileWall;
                case 2:
                    return WallType.SteelSheetWall;
                default:
                    return WallType.ConcreteRectangleWall;
            }
        }
        public static int GetWallTypeIndex(WallType wallType)
        {
            switch (wallType)
            {
                case WallType.ConcreteRectangleWall:
                    return 0;
                case WallType.ConcretePileWall:
                    return 1;
                case WallType.SteelSheetWall:
                    return 2;
                default:
                    return 0;
            }


        }

    }
}
