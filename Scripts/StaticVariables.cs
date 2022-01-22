using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExDesign.Pages.Inputs.Views;

namespace ExDesign.Scripts
{
    public static class StaticVariables
    {
        public static View3dPage view3DPage;
        public static SideviewPage SideviewPage = new SideviewPage();
        public static Units CurrentUnit = Units.kg_cm;
        public static Units LastUnit = Units.kg_cm;
        public static int UnitIndex = 11;
        public static Dictionary<Units, double> UnitDimensionFactors = new Dictionary<Units, double> {{ Units.kg_mm, 1000 },{Units.kg_cm,100 },{ Units.kg_m,1},
                                                                                                        { Units.ton_mm, 1000 },{ Units.ton_cm, 100 },{ Units.ton_m, 1 },
                                                                                                        { Units.N_mm, 1000 },{ Units.N_cm, 100 },{ Units.N_m, 1 },
                                                                                                        { Units.kN_mm, 1000 },{ Units.kN_cm, 100 },{ Units.kN_m, 1 } };

        public static WallType wallType = WallType.ConcreteRectangleWall;
        public static ExcavationType excavationType = ExcavationType.none;
        public static GroundSurfaceType groundSurfaceType = GroundSurfaceType.flat;
        public static GroundWaterType groundWaterType = GroundWaterType.none;

        public static double wall_d = 11;
        public static int maxPileCount = 20;
    }
    public enum WallType
    {
        ConcreteRectangleWall,
        ConcretePileWall,
        SteelSheetWall

    }
    public enum Units
    {
            kg_mm,
            kg_cm,
            kg_m,
            ton_mm,
            ton_cm,
            ton_m,
            N_mm,
            N_cm,
            N_m,
            kN_mm,
            kN_cm,
            kN_m,

    }
    public enum ExcavationType
    {
        none,
        type1,
        type2,
    }
    public enum GroundSurfaceType
    {
        flat,
        type1,
        type2,
        type3
    }
    public enum GroundWaterType
    {
        none,
        type1,
        type2,
        type3
        
    }
}
