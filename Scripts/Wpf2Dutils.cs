using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace ExDesign.Scripts
{
    public static class Wpf2Dutils
    {
        public static GeometryDrawing WallGeometryDrawing(Point center,double H,double W,Color color)
        {           
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(center.X,center.Y, Math.Clamp(W, 0, double.MaxValue), Math.Clamp(H, 0, double.MaxValue)));

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(rectangleGeometry);

            GeometryDrawing wallDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(Brushes.Black, 0.03),
                    geometryGroup
                    );
            return wallDrawing;
        }
        public static GeometryDrawing SoilGeometryDrawing(Point center, double H, double W, Color color, Uri texture, bool textured)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(center.X, center.Y,Math.Clamp( W,0,double.MaxValue),Math.Clamp( H,0,double.MaxValue)));

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(rectangleGeometry);
            Brush brush = new SolidColorBrush(color);
            if (textured)
            {
                var imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(texture);
                imageBrush.Stretch = Stretch.None;
                brush = imageBrush;
            }

            GeometryDrawing soilDrawing =
            new GeometryDrawing(
                    brush,
                    new Pen(brush, 0.03),
                    geometryGroup
                    );
            return soilDrawing;
        }
        public static GeometryDrawing SoilGeometryDrawing(Point center, double H, double W, double Z, double W_top,double W_bot, double X_top, double X_bot, Color color, Uri texture, bool textured)
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(center.X, center.Y+H+Z);
            LineSegment myLineSegment = new LineSegment();
            myLineSegment.Point = new Point(center.X, center.Y+Z);
            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = new Point(center.X + X_bot, center.Y + Z);
            LineSegment lineSegment1 = new LineSegment();
            lineSegment1.Point = new Point(center.X + X_top, center.Y );
            LineSegment lineSegment2 = new LineSegment();
            lineSegment2.Point = new Point(center.X + X_top+W_top, center.Y);
            LineSegment lineSegment3 = new LineSegment();
            lineSegment3.Point = new Point(center.X + X_bot+ W_bot, center.Y +Z);
            LineSegment lineSegment4 = new LineSegment();
            lineSegment4.Point = new Point(center.X + W, center.Y +Z);
            LineSegment lineSegment5 = new LineSegment();
            lineSegment5.Point = new Point(center.X + W, center.Y + H+Z);

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
            GeometryDrawing geometryDrawing = new GeometryDrawing(brush,new Pen(brush,0.01),myPathGeometry);
            return geometryDrawing;
        }
        public static GeometryDrawing TextGeometryDrawing(Point center, string testString, Color color)
        {

            // Create the initial formatted text string.
            FormattedText formattedText = new FormattedText(
                testString,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                1,
                Brushes.Black);

            // Set a maximum width and height. If the text overflows these values, an ellipsis "..." appears.
            formattedText.MaxTextWidth = 300;
            formattedText.MaxTextHeight = 240;

            
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(formattedText.BuildGeometry(center));

            GeometryDrawing wallDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(Brushes.Black, 0.001),
                    geometryGroup
                    );
            return wallDrawing;
        }
    }
}
