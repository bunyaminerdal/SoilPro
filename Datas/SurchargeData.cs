using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExDesign.Datas
{
    public class SurfaceSurchargeData
    {
        public string SurchargeName { get; set; }
        public double Load { get; set; }
    }
    public class StripLoadData
    {
        public string SurchargeName { get; set; }        
        public double DistanceFromWall { get; set; }
        public double StripLength { get; set; }
        public double StartLoad { get; set; }
        public double EndLoad { get; set; }
    }
    public class PointLoadData
    {
        public string SurchargeName { get; set; }
        public double DistanceFromWall { get; set; }
        public double Load { get; set; }
    }
    public class LineLoadData
    {
        public string SurchargeName { get; set; }
        public double DistanceFromWall { get; set; }
        public double Load { get; set; }
    }
}
