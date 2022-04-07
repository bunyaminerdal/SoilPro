using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExDesign.Datas
{
    public class FrameData : IComparable<FrameData>
    {
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Frames.gexdb";

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
        public List<Tuple<Load,double,double>> startNodeLoadAndForce = new List<Tuple<Load, double, double>>();
        public List<Tuple<Load,double, double>> endNodeLoadAndForce = new List< Tuple<Load, double, double>>();
        public List<Tuple<Load, double, double,double>> startNodeActivePassiveCoef_S_P_N = new List<Tuple<Load, double, double,double>>();
        public List<Tuple<Load, double, double,double>> endNodeActivePassiveCoef_S_P_N = new List<Tuple<Load, double, double,double>>();

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
        public static void FrameSave()
        {

            string json = JsonConvert.SerializeObject(Frames.ToArray());

            //write string to file
            File.WriteAllText(Path, json);
        }
    }
}
