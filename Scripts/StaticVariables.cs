using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoilPro.Scripts
{
    public static class StaticVariables
    {
        
        public static Units CurrentUnit = Units.kg_cm;
        public static Units LastUnit = Units.kg_cm;
        public static Dictionary<Units, double> UnitDimensionFactors = new Dictionary<Units, double> { {Units.kg_cm,1 },{ Units.kg_mm, 10 },{ Units.kg_m,0.01} };
    }
    public enum Units
    {
            kg_mm,
            kg_cm,
            kg_m
    }
}
