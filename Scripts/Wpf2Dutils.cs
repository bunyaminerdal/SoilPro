using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;


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
        public static GeometryDrawing AnchorGeometryDrawing(Point rotationCenter,double inclination,double wall_t,double freeL, double rootD, double rootL,double ex,double beamW, Color color)
        {
            
            LineGeometry lineGeometry = new LineGeometry(new Point(rotationCenter.X-ex,rotationCenter.Y), new Point(rotationCenter.X+beamW+wall_t+freeL,rotationCenter.Y));
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(rotationCenter.X+wall_t+beamW+freeL, rotationCenter.Y-rootD/2, Math.Clamp(rootL, 0, double.MaxValue), Math.Clamp(rootD, 0, double.MaxValue)));

            GeometryGroup geometryGroup = new GeometryGroup();
            RotateTransform rotateAnchor = new RotateTransform(inclination, rotationCenter.X, rotationCenter.Y);
            
            lineGeometry.Transform = rotateAnchor;            
            rectangleGeometry.Transform = rotateAnchor;
            geometryGroup.Children.Add(lineGeometry);
            geometryGroup.Children.Add(rectangleGeometry);
            
            GeometryDrawing wallDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(Brushes.Black, StaticVariables.penThickness),
                    geometryGroup
                    );
            return wallDrawing;
        }
        public static GeometryDrawing AnchorRootPartGeometryDrawing(Point rotationCenter, double inclination, double startL, double endL, double rootD, Color color)
        {

            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(rotationCenter.X + startL, rotationCenter.Y - rootD / 2, Math.Clamp(endL-startL, 0, double.MaxValue), Math.Clamp(rootD, 0, double.MaxValue)));

            GeometryGroup geometryGroup = new GeometryGroup();
            RotateTransform rotateAnchor = new RotateTransform(inclination, rotationCenter.X, rotationCenter.Y);

            rectangleGeometry.Transform = rotateAnchor;
            geometryGroup.Children.Add(rectangleGeometry);

            GeometryDrawing wallDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(Brushes.Black, StaticVariables.penThickness),
                    geometryGroup
                    );
            return wallDrawing;
        }
        public static GeometryDrawing StrutGeometryDrawing(Point rotationCenter, double strutL, double strutD, Color color)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(rotationCenter.X  -strutL, rotationCenter.Y-strutD/2,strutL,strutD) );

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
            if(text =="") text =Math.Round(_length,2).ToString();
            if(StaticVariables.IsDimensionShowed)
            {
                text = string.Join(text," ", StaticVariables.dimensionUnit);
            }
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
        /// <summary>
        /// 4 yöne text yazmak için
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static GeometryDrawing FreeTextDrawing(Point start, Point end, Color color, string text ,double ex=0)
        {
            double diff = StaticVariables.dimensionDiff;
            double xDiff = start.X - end.X;
            double yDiff = start.Y - end.Y;
            double angle = (Math.Atan2(yDiff, xDiff) * 180 / Math.PI) - 180;
            double _length = Point.Subtract(end, start).Length;
            
            
            // Create the initial formatted text string.
            FormattedText formattedText = new FormattedText(
                text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                StaticVariables.typeface,
                StaticVariables.freeTextFontHeight,
                Brushes.Black, VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip);

            // Set a maximum width and height. If the text overflows these values, an ellipsis "..." appears.
            formattedText.MaxTextWidth = 1000;
            formattedText.MaxTextHeight = 240;
            GeometryGroup geometryGroup = new GeometryGroup();
            var textgeometry = formattedText.BuildGeometry(new Point(start.X + (_length / 2) - formattedText.Width / 2, start.Y -ex - diff - formattedText.Height));

            RotateTransform rotateLine = new RotateTransform(angle, start.X, start.Y);
            if (angle < -90 && angle > -270)
            {
                angle += 180;
                textgeometry = formattedText.BuildGeometry(new Point(start.X - (_length / 2) - formattedText.Width / 2, start.Y +ex + diff));
            }
            else
            {
                textgeometry = formattedText.BuildGeometry(new Point(start.X + (_length / 2) - formattedText.Width / 2, start.Y -ex - diff - formattedText.Height));
            }
            RotateTransform rotateText = new RotateTransform(angle, start.X, start.Y);
            Debug.WriteLine(angle + " " + start.X  +" "+start.Y);
            if(angle < double.MaxValue) textgeometry.Transform = rotateText;

            
            geometryGroup.Children.Add(textgeometry);            

            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.freeTextPenThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }
        public static GeometryDrawing Level(Point start, LevelDirection direction , Color color)
        {
            double ex = StaticVariables.dimensionExtension;
            double ex_text = 0.5 * ex;
            double diff = StaticVariables.dimensionDiff;
            double levelIconH = StaticVariables.levelIconHeight;
            double levelLineL = 3 * levelIconH;
            double level = StaticVariables.viewModel.TopOfWallLevel-start.Y;
            
            GeometryGroup geometryGroup = new GeometryGroup();
            Point levelIconCenter = start;
            switch (direction)
            {
                case LevelDirection.Left:
                    LineGeometry lineGeometry = new LineGeometry(new Point(start.X-diff, start.Y), new Point(start.X-diff-levelLineL, start.Y));
                    geometryGroup.Children.Add(lineGeometry);
                    levelIconCenter = new Point(start.X-diff-2*levelIconH,start.Y);
                    break;
                case LevelDirection.Right:
                    LineGeometry lineGeometry1 = new LineGeometry(new Point(start.X + diff, start.Y), new Point(start.X + diff + levelLineL, start.Y));
                    geometryGroup.Children.Add(lineGeometry1);
                    levelIconCenter = new Point(start.X + diff + 2 * levelIconH, start.Y);
                    break;
                case LevelDirection.Point:
                    levelIconCenter = new Point(start.X , start.Y);
                    break;
                default:                    
                    break;
            }
            LineGeometry IconTop = new LineGeometry(new Point(levelIconCenter.X-levelIconH, levelIconCenter.Y-levelIconH), new Point(levelIconCenter.X+levelIconH, levelIconCenter.Y-levelIconH));
            LineGeometry IconLeft = new LineGeometry(new Point(levelIconCenter.X-levelIconH, levelIconCenter.Y-levelIconH), new Point(levelIconCenter.X, levelIconCenter.Y));
            LineGeometry IconRight = new LineGeometry(new Point(levelIconCenter.X+levelIconH, levelIconCenter.Y-levelIconH), new Point(levelIconCenter.X, levelIconCenter.Y));
            
            geometryGroup.Children.Add(IconTop);
            geometryGroup.Children.Add(IconLeft);
            geometryGroup.Children.Add(IconRight);
            string text = WpfUtils.LevelText(level);
            
            // Create the initial formatted text string.
            FormattedText formattedText = new FormattedText(
                text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                StaticVariables.typeface,
                StaticVariables.levelFontHeight,
                Brushes.Black, VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip);

            // Set a maximum width and height. If the text overflows these values, an ellipsis "..." appears.
            formattedText.MaxTextWidth = 1000;
            formattedText.MaxTextHeight = 240;
            var textgeometry = formattedText.BuildGeometry(new Point(levelIconCenter.X-formattedText.Width/2,levelIconCenter.Y-levelIconH-ex_text-formattedText.Height));
            
            geometryGroup.Children.Add(textgeometry);
            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.dimensionPenThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }
        public static GeometryDrawing PointLoadGeometryDrawing(Point center, double height, double distance, Color color)
        {
            if(height< StaticVariables.minLoadHeight) height = StaticVariables.minLoadHeight;
            double arrowScale = StaticVariables.loadArrowScale;
            LineGeometry lineGeometry = new LineGeometry(new Point(center.X+distance,center.Y), new Point(center.X+distance,center.Y-height));
            LineGeometry lineGeometryleft = new LineGeometry(new Point(center.X+distance,center.Y), new Point(center.X+distance-height*arrowScale/2,center.Y-height*arrowScale));
            LineGeometry lineGeometryright = new LineGeometry(new Point(center.X+distance,center.Y), new Point(center.X+distance+height*arrowScale/2,center.Y-height*arrowScale));

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(lineGeometry);
            geometryGroup.Children.Add(lineGeometryleft);
            geometryGroup.Children.Add(lineGeometryright);

            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.LoadPenThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }
        public static GeometryDrawing LineLoadGeometryDrawing(Point center, double height, double distance, Color color)
        {
            if (height < StaticVariables.minLoadHeight) height = StaticVariables.minLoadHeight;
            double arrowScale = StaticVariables.loadArrowScale;
            LineGeometry lineGeometry = new LineGeometry(new Point(center.X + distance, center.Y), new Point(center.X + distance, center.Y - height));
            LineGeometry lineGeometryleft = new LineGeometry(new Point(center.X + distance, center.Y), new Point(center.X + distance - height * arrowScale / 2, center.Y - height * arrowScale));
            LineGeometry lineGeometryright = new LineGeometry(new Point(center.X + distance, center.Y), new Point(center.X + distance + height * arrowScale / 2, center.Y - height * arrowScale));

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(lineGeometry);
            geometryGroup.Children.Add(lineGeometryleft);
            geometryGroup.Children.Add(lineGeometryright);

            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.LoadPenThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }
        public static GeometryDrawing StripLoadGeometryDrawing(Point center, double startHeight, double endHeight, double distance,double stripLength, Color color)
        {
            if (startHeight>0 && startHeight < StaticVariables.minLoadHeight) startHeight = StaticVariables.minLoadHeight;
            if (endHeight>0 && endHeight < StaticVariables.minLoadHeight) endHeight = StaticVariables.minLoadHeight;
            double arrowScale = StaticVariables.loadArrowScale;
            double arrowHeight = Math.Max(startHeight, endHeight)*arrowScale;
            double loadLineAmount = Math.Round( stripLength * 2,0);
            double lineStep = loadLineAmount > 0 ? stripLength / loadLineAmount : 1 ;

            GeometryGroup geometryGroup = new GeometryGroup();

            LineGeometry lineStartGeometry = new LineGeometry(new Point(center.X + distance, center.Y), new Point(center.X + distance, center.Y - startHeight));
            LineGeometry lineStartGeometryright = new LineGeometry(new Point(center.X + distance, center.Y), new Point(center.X + distance + arrowHeight / 2, center.Y - arrowHeight));
            geometryGroup.Children.Add(lineStartGeometry);
            if(startHeight > 0 )geometryGroup.Children.Add(lineStartGeometryright);            
            for (int i = 1; i < loadLineAmount; i++)
            {                
                LineGeometry lineGeometry = new LineGeometry(new Point(center.X + distance + i*lineStep, center.Y), new Point(center.X + distance + i * lineStep, center.Y - startHeight-(i*(lineStep/stripLength)*(endHeight-startHeight))));
                LineGeometry lineGeometryleft = new LineGeometry(new Point(center.X + distance + i * lineStep, center.Y), new Point(center.X + distance + i * lineStep - arrowHeight / 2, center.Y - arrowHeight));
                LineGeometry lineGeometryright = new LineGeometry(new Point(center.X + distance + i * lineStep, center.Y), new Point(center.X + distance + i * lineStep + arrowHeight / 2, center.Y - arrowHeight));
                geometryGroup.Children.Add(lineGeometry);
                geometryGroup.Children.Add(lineGeometryleft);
                geometryGroup.Children.Add(lineGeometryright);
            }
            LineGeometry lineEndGeometry = new LineGeometry(new Point(center.X + distance +stripLength, center.Y), new Point(center.X + distance + stripLength, center.Y - endHeight));
            LineGeometry lineEndGeometryleft = new LineGeometry(new Point(center.X + distance + stripLength, center.Y), new Point(center.X + distance + stripLength - arrowHeight / 2, center.Y - arrowHeight));
            geometryGroup.Children.Add(lineEndGeometry);
            if (endHeight > 0) geometryGroup.Children.Add(lineEndGeometryleft);
            LineGeometry lineTopGeometry = new LineGeometry(new Point(center.X + distance , center.Y-startHeight), new Point(center.X + distance + stripLength, center.Y - endHeight));
            geometryGroup.Children.Add(lineTopGeometry);
            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.LoadPenThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }
        public static GeometryDrawing SurchargeGeometryDrawing(Point center, double height,double backCubeLenght, Color color)
        {
            if (height > 0 && height < StaticVariables.minLoadHeight) height = StaticVariables.minLoadHeight;
            double arrowHeight = height * StaticVariables.loadArrowScale;
            double loadLineAmount = Math.Round( backCubeLenght * 2, 0);
            double lineStep = loadLineAmount > 0 ? backCubeLenght / loadLineAmount : 1;

            GeometryGroup geometryGroup = new GeometryGroup();

            LineGeometry lineStartGeometry = new LineGeometry(new Point(center.X , center.Y), new Point(center.X , center.Y - height));
            LineGeometry lineStartGeometryright = new LineGeometry(new Point(center.X , center.Y), new Point(center.X  + arrowHeight / 2, center.Y - arrowHeight));
            geometryGroup.Children.Add(lineStartGeometry);
            geometryGroup.Children.Add(lineStartGeometryright);
            for (int i = 1; i < loadLineAmount; i++)
            {
                LineGeometry lineGeometry = new LineGeometry(new Point(center.X  + i * lineStep, center.Y), new Point(center.X  + i * lineStep, center.Y - height ));
                LineGeometry lineGeometryleft = new LineGeometry(new Point(center.X  + i * lineStep, center.Y), new Point(center.X  + i * lineStep - arrowHeight / 2, center.Y - arrowHeight));
                LineGeometry lineGeometryright = new LineGeometry(new Point(center.X  + i * lineStep, center.Y), new Point(center.X  + i * lineStep + arrowHeight / 2, center.Y - arrowHeight));
                geometryGroup.Children.Add(lineGeometry);
                geometryGroup.Children.Add(lineGeometryleft);
                geometryGroup.Children.Add(lineGeometryright);
            }
            LineGeometry lineEndGeometry = new LineGeometry(new Point(center.X  + backCubeLenght, center.Y), new Point(center.X  + backCubeLenght, center.Y - height));
            LineGeometry lineEndGeometryleft = new LineGeometry(new Point(center.X  + backCubeLenght, center.Y), new Point(center.X  + backCubeLenght - arrowHeight / 2, center.Y - arrowHeight));
            geometryGroup.Children.Add(lineEndGeometry);
             geometryGroup.Children.Add(lineEndGeometryleft);
            LineGeometry lineTopGeometry = new LineGeometry(new Point(center.X , center.Y - height), new Point(center.X  + backCubeLenght, center.Y - height));
            geometryGroup.Children.Add(lineTopGeometry);


            GeometryDrawing lineDrawing =
            new GeometryDrawing(
                    new SolidColorBrush(color),
                    new Pen(new SolidColorBrush(color), StaticVariables.LoadPenThickness),
                    geometryGroup
                    );
            return lineDrawing;
        }
    }
}
