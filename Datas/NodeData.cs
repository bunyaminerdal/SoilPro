using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExDesign.Scripts;
using System.Diagnostics;

namespace ExDesign.Datas
{
    public class NodeData : IComparable<NodeData>
    {
        public static List<NodeData> Nodes = new List<NodeData>();
        public Guid ID { get; set; }
        public Point Location { get; set; }
        public Point DeformedLoc { get; set; }
        public bool isFrontSpringOn = true;
        public bool isBackSpringOn = false;
        public NodeData(Point _location)
        {
            ID = new Guid();
            Location = _location;
            Nodes.Add(this);
        }
        public List<Tuple<Load,double>> nodeForce =new List<Tuple<Load,double>>();
        public void AddForce(Tuple<Load,double,double> tuple)
        {
            switch (tuple.Item1.Type)
            {                
                case LoadType.StripLoad:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1,tuple.Item3));
                    break;
                case LoadType.LineLoad:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, tuple.Item3));
                    break;
                case LoadType.PointLoad:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, tuple.Item3));
                    break;
                case LoadType.HydroStaticWaterPressure:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1,  tuple.Item3));
                    break;
                case LoadType.Back_SubgradeModulusofSoil:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, tuple.Item3));
                    break;
                case LoadType.Front_SubgradeModulusofSoil:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, -1.0 * tuple.Item3));
                    break;
                case LoadType.Back_Active_Horizontal_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1,  tuple.Item3));
                    break;
                case LoadType.Back_Passive_Horizontal_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1,  tuple.Item3));
                    break;
                case LoadType.Back_Active_Vertical_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, tuple.Item3));
                    break;
                case LoadType.Back_Passive_Vertical_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1,  tuple.Item3));
                    break;
                case LoadType.Front_Active_Horizontal_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, -1.0 * tuple.Item3));
                    break;
                case LoadType.Front_Passive_Horizontal_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, -1.0 * tuple.Item3));
                    break;
                case LoadType.Front_Active_Vertical_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, -1.0 * tuple.Item3));
                    break;
                case LoadType.Front_Passive_Vertical_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, -1.0 * tuple.Item3));
                    break;
                case LoadType.Front_Rest_Horizontal_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, -1.0 * tuple.Item3));
                    break;
                case LoadType.Back_Rest_Horizontal_Force:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, tuple.Item3));
                    break;
                case LoadType.First_Displacement:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1,  tuple.Item3));
                    break;
                case LoadType.First_Rotation:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, tuple.Item3));
                    break;
                case LoadType.First_Iteration_Displacement:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1,  tuple.Item3));
                    break;
                case LoadType.First_Iteration_Rotation:
                    nodeForce.Add(new Tuple<Load, double>(tuple.Item1, tuple.Item3));
                    break;
                default:
                    break;
            }
        }
        public int CompareTo(NodeData? other)
        {
            if (this.Location.Y < other.Location.Y)
            {
                return 1;
            }
            else if (this.Location.Y > other.Location.Y)
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
