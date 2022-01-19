using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoilPro.Pages.Inputs.Views;

namespace SoilPro.Scripts
{
    public static class StaticVariables
    {
        public static View3dPage view3DPage = new View3dPage();
        public static SideviewPage SideviewPage = new SideviewPage();
        public static Units CurrentUnit = Units.kg_cm;
        public static Units LastUnit = Units.kg_cm;
        public static Dictionary<Units, double> UnitDimensionFactors = new Dictionary<Units, double> { {Units.kg_cm,1 },{ Units.kg_mm, 10 },{ Units.kg_m,0.01}, { Units.kN_m, 0.01 } };

        public static ExcavationType excavationType = ExcavationType.none;
    }
    public enum Units
    {
            kg_mm,
            kg_cm,
            kg_m,
            kN_m
    }
    public enum ExcavationType
    {
        none,
        type1,
        type2,
    }
}
