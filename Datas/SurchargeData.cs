using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExDesign.Datas
{
    public class SurfaceSurchargeData : Load
    {
        public string SurchargeName { get; set; }
        public double Load { get; set; }
    }
    public class StripLoadData : Load
    {
        public string SurchargeName { get; set; }        
        public double DistanceFromWall { get; set; }
        public double StripLength { get; set; }
        public double StartLoad { get; set; }
        public double EndLoad { get; set; }
    }
    public class PointLoadData : Load
    {
        public string SurchargeName { get; set; }
        public double DistanceFromWall { get; set; }
        public double Load { get; set; }
    }
    public class LineLoadData : Load
    {
        public string SurchargeName { get; set; }
        public double DistanceFromWall { get; set; }
        public double Load { get; set; }
    }
    public class Load
    {

    }
}
