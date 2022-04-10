using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExDesign.Scripts;

namespace ExDesign.Datas
{
    public class SurfaceSurchargeData : Load
    {
        public string Name { get; set; }
        public double Load { get; set; }
        public LoadType Type { get; set; }
        public Guid ID { get; set; }
    }
    public class StripLoadData : Load
    {
        public string Name { get; set; }        
        public double DistanceFromWall { get; set; }
        public double StripLength { get; set; }
        public double StartLoad { get; set; }
        public double EndLoad { get; set; }
        public LoadType Type { get; set; }
        public Guid ID { get; set; }

    }
    public class PointLoadData : Load
    {
        public string Name { get; set; }
        public double DistanceFromWall { get; set; }
        public double Load { get; set; }
        public LoadType Type { get; set; }
        public Guid ID { get; set; }

    }
    public class LineLoadData : Load
    {
        public string Name { get; set; }
        public double DistanceFromWall { get; set; }
        public double Load { get; set; }
        public LoadType Type { get; set; }
        public Guid ID { get; set; }
    }
    public interface Load
    {
        public LoadType Type { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }
    }
    public class WaterLoadData: Load
    {
        public LoadType Type { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }

    }
    public class EffectiveStress : Load
    {
        public LoadType Type { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }
    }
    public class SubgradeModulusofSoil : Load
    {
        public LoadType Type { get; set;}
        public string Name { get; set; }
        public Guid ID { get; set; }
    }
    public class Ka_Kp : Load
    {
        public LoadType Type { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }
    }
    public class Force : Load
    {
        public LoadType Type { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }
    }
    
}
