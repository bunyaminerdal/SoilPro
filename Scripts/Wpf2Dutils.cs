using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExDesign.Scripts
{
    public static class Wpf2Dutils
    {

        public static GeometryDrawing LineGeometryDrawing(Point start, Point end, Color color)
        {
            LineGeometry lineGeometry = new LineGeometry(start, end);

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(lineGeometry);

            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.penThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }

        public static GeometryDrawing LineGeometryDrawing(Point start, Point point1, Point end, Color color)
        {
            LineGeometry lineGeometry = new LineGeometry(start, point1);
            LineGeometry lineGeometry1 = new LineGeometry(point1, end);

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(lineGeometry);
            geometryGroup.Children.Add(lineGeometry1);

            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.penThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }
        public static GeometryDrawing LineGeometryDrawing(Point start, Point point1, Point point2, Point end, Color color)
        {
            LineGeometry lineGeometry = new LineGeometry(start, point1);
            LineGeometry lineGeometry1 = new LineGeometry(point1, point2);
            LineGeometry lineGeometry2 = new LineGeometry(point2, end);

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(lineGeometry);
            geometryGroup.Children.Add(lineGeometry1);
            geometryGroup.Children.Add(lineGeometry2);

            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.penThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }
        public static GeometryDrawing WallGeometryDrawing(Point center, double H, double W, Color color)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(center.X, center.Y, Math.Clamp(W, 0, double.MaxValue), Math.Clamp(H, 0, double.MaxValue)));

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(rectangleGeometry);

            GeometryDrawing wallDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(Brushes.Black, StaticVariables.penThickness),
                    geometryGroup
                    );
            return wallDrawing;
        }
        public static GeometryDrawing SoilGeometryDrawing(Point center, double H, double W, Color color, Uri texture, bool textured)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(center.X, center.Y, Math.Clamp(W, 0, double.MaxValue), Math.Clamp(H, 0, double.MaxValue)));

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(rectangleGeometry);
            Brush brush = new SolidColorBrush(color);
            if (textured)
            {
                var imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(texture);
                imageBrush.Stretch = Stretch.UniformToFill;
                brush = imageBrush;
            }

            GeometryDrawing soilDrawing =
            new GeometryDrawing(
                    brush,
                    new Pen(Brushes.Black, StaticVariables.penThickness),
                    geometryGroup
                    );
            return soilDrawing;
        }
        public static GeometryDrawing SoilGeometryDrawing(Point center, double H, double W, double Z, double W_top, double W_bot, double X_top, double X_bot, Color color, Uri texture, bool textured)
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(center.X, center.Y + H + Z);
            LineSegment myLineSegment = new LineSegment();
            myLineSegment.Point = new Point(center.X, center.Y + Z);
            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = new Point(center.X + X_bot, center.Y + Z);
            LineSegment lineSegment1 = new LineSegment();
            lineSegment1.Point = new Point(center.X + X_top, center.Y);
            LineSegment lineSegment2 = new LineSegment();
            lineSegment2.Point = new Point(center.X + X_top + W_top, center.Y);
            LineSegment lineSegment3 = new LineSegment();
            lineSegment3.Point = new Point(center.X + X_bot + W_bot, center.Y + Z);
            LineSegment lineSegment4 = new LineSegment();
            lineSegment4.Point = new Point(center.X + W, center.Y + Z);
            LineSegment lineSegment5 = new LineSegment();
            lineSegment5.Point = new Point(center.X + W, center.Y + H + Z);

            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
            myPathSegmentCollection.Add(myLineSegment);
            myPathSegmentCollection.Add(lineSegment);
            myPathSegmentCollection.Add(lineSegment1);
            myPathSegmentCollection.Add(lineSegment2);
            myPathSegmentCollection.Add(lineSegment3);
            myPathSegmentCollection.Add(lineSegment4);
            myPathSegmentCollection.Add(lineSegment5);

            myPathFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(myPathFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            Brush brush = new SolidColorBrush(color);
            if (textured)
            {
                var imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(texture);
                imageBrush.Stretch = Stretch.None;
                brush = imageBrush;
            }
            GeometryDrawing geometryDrawing = new GeometryDrawing(brush, new Pen(brush, StaticVariables.penThickness), myPathGeometry);
            return geometryDrawing;
        }
        
        /// <summary>
        /// 4 yöne ölçü atmak için. eğer ölçü texti belliyse son parametre olarak girilebilir, yoksa kendi hesaplayacak.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static GeometryDrawing Dimension(Point start, Point end, Color color, string text = "")
        {
            double ex = StaticVariables.dimensionExtension;
            double ex_text = 2.2 * ex;
            double diff = StaticVariables.dimensionDiff;
            double xDiff = start.X - end.X;
            double yDiff = start.Y - end.Y;
            double angle =  (Math.Atan2(yDiff, xDiff) * 180 / Math.PI) - 180;
            double _length = Point.Subtract(end, start).Length;
            LineGeometry lineGeometry = new LineGeometry(new Point(start.X - ex, start.Y - ex - diff), new Point(start.X + _length + ex, start.Y - ex - diff));
            LineGeometry lineGeometryleft = new LineGeometry(new Point(start.X, start.Y - diff), new Point(start.X, start.Y - 2 * ex - diff));
            LineGeometry lineGeometryright = new LineGeometry(new Point(start.X + _length, start.Y - diff), new Point(start.X + _length, start.Y - 2 * ex - diff));
            if(text =="") text =string.Join( Math.Round(_length,2).ToString()," ",StaticVariables.dimensionUnit);
            // Create the initial formatted text string.
            FormattedText formattedText = new FormattedText(
                text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                StaticVariables.typeface,
                StaticVariables.dimensionFontHeight,
                Brushes.Black, VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip);

            // Set a maximum width and height. If the text overflows these values, an ellipsis "..." appears.
            formattedText.MaxTextWidth = 1000;
            formattedText.MaxTextHeight = 240;
            GeometryGroup geometryGroup = new GeometryGroup();
            var textgeometry = formattedText.BuildGeometry(new Point(start.X + (_length / 2) - formattedText.Width / 2, start.Y - ex_text - diff - formattedText.Height));

            RotateTransform rotateLine = new RotateTransform(angle , start.X, start.Y);
            Debug.WriteLine(angle.ToString());
            if (angle < -90 && angle > -270)
            {
                angle  += 180;
                textgeometry = formattedText.BuildGeometry(new Point(start.X - (_length / 2) - formattedText.Width / 2, start.Y + ex_text + diff ));
            }
            else
            {
                textgeometry = formattedText.BuildGeometry(new Point(start.X + (_length / 2) - formattedText.Width / 2, start.Y - ex_text - diff - formattedText.Height));
            }
            RotateTransform rotateText = new RotateTransform(angle , start.X, start.Y);
            textgeometry.Transform = rotateText;
            lineGeometry.Transform = rotateLine;
            lineGeometryleft.Transform = rotateLine;
            lineGeometryright.Transform = rotateLine;
            geometryGroup.Children.Add(textgeometry);
            geometryGroup.Children.Add(lineGeometry);
            geometryGroup.Children.Add(lineGeometryleft);
            geometryGroup.Children.Add(lineGeometryright);

            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.dimensionPenThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }
    }
}
