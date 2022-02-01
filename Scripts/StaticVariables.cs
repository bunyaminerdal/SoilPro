using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExDesign.Datas;
using ExDesign.Pages.Inputs.Views;

namespace ExDesign.Scripts
{
    public static class StaticVariables
    {
        private static double gravity_factor = 1 / 9.80665;
        public static ViewModelData viewModel = new ViewModelData();
        public static View3dPage view3DPage;
        public static SideviewPage SideviewPage;
        public static Units CurrentUnit = Units.kg_cm;
        public static Units LastUnit = Units.kg_cm;
        public static string dimensionUnit { get { return CurrentUnit.ToString().Split('_')[1]; } }
        public static string areaUnit { get { return CurrentUnit.ToString().Split('_')[1]+"2"; } }
        public static string volumeUnit { get { return CurrentUnit.ToString().Split('_')[1] + "3"; } }
        public static string inertiaUnit { get { return CurrentUnit.ToString().Split('_')[1] + "4"; } }
        public static string forceUnit { get { return CurrentUnit.ToString().Split('_')[0]; } }
        public static string StressUnit { get { return CurrentUnit.ToString().Split('_')[0]+"/" + CurrentUnit.ToString().Split('_')[1] + "2"; } }
        public static string DensityUnit { get { return CurrentUnit.ToString().Split('_')[0]+"/" + CurrentUnit.ToString().Split('_')[1] + "3"; } }
        public static string EIUnit { get { return CurrentUnit.ToString().Split('_')[0]+ CurrentUnit.ToString().Split('_')[1] + "2"; } }
        public static Dictionary<Units, double> UnitDimensionFactors = new Dictionary<Units, double> {{ Units.kg_mm, 1000 },{Units.kg_cm,100 },{ Units.kg_m,1},
                                                                                                        { Units.ton_mm, 1000 },{ Units.ton_cm, 100 },{ Units.ton_m, 1 },
                                                                                                        { Units.N_mm, 1000 },{ Units.N_cm, 100 },{ Units.N_m, 1 },
                                                                                                        { Units.kN_mm, 1000 },{ Units.kN_cm, 100 },{ Units.kN_m, 1 } };

        public static Dictionary<Units,double> UnitForceFactors = new Dictionary<Units,double> {{ Units.kg_mm, (1000/gravity_factor) },{Units.kg_cm,(1000/gravity_factor) },{ Units.kg_m,(1000/gravity_factor)},
                                                                                                        { Units.ton_mm, gravity_factor },{ Units.ton_cm, gravity_factor },{ Units.ton_m, gravity_factor },
                                                                                                        { Units.N_mm, 1000 },{ Units.N_cm, 1000 },{ Units.N_m, 1000 },
                                                                                                        { Units.kN_mm, 1 },{ Units.kN_cm, 1 },{ Units.kN_m, 1 } };
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

    public enum SoilModelType
    {
        Schmitt_Model,
        Chadeisson_Model,
        Vesic_Model

    }
}
