using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExDesign.Datas
{
    public class FrameData : IComparable<FrameData>
    {
        public static List<FrameData> Frames = new List<FrameData>();
        public Guid ID { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Point DeformedStartPoint { get; set; }
        public Point DeformedEndPoint { get; set; }
        public FrameData(Point _startPoint,Point _endPoint)
        {
            ID = new Guid();
            EndPoint = DeformedEndPoint = _endPoint;
            StartPoint = DeformedStartPoint = _startPoint;
            Frames.Add(this);
        }
        public Dictionary<Load,Tuple<double,double>> startNodeLoadAndForce = new Dictionary<Load,Tuple<double, double>>();
        public Dictionary<Load,Tuple<double, double>> endNodeLoadAndForce = new Dictionary<Load, Tuple<double, double>>();

        
        public int CompareTo(FrameData? other)
        {
            if (this.StartPoint.Y < other.StartPoint.Y)
            {
                return 1;
            }
            else if (this.StartPoint.Y > other.StartPoint.Y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        
    }
}
