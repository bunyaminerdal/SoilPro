using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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
        public static LoadsAndFocesPage loadsAndFocesPage;
        public static Units CurrentUnit = Units.kg_cm;
        public static Units LastUnit = Units.kg_cm;
        public static string dimensionUnit { get { return CurrentUnit.ToString().Split('_')[1]; } }
        public static string areaUnit { get { return CurrentUnit.ToString().Split('_')[1]+ "²"; } }
        public static string volumeUnit { get { return CurrentUnit.ToString().Split('_')[1] + "³"; } }
        public static string inertiaUnit { get { return CurrentUnit.ToString().Split('_')[1] + "⁴"; } }
        public static string forceUnit { get { return CurrentUnit.ToString().Split('_')[0]; } }
        public static string StressUnit { get { return CurrentUnit.ToString().Split('_')[0]+"/" + CurrentUnit.ToString().Split('_')[1] + "²"; } }
        public static string SurfaceStressUnit { get { return CurrentUnit.ToString().Split('_')[0]+"/" + CurrentUnit.ToString().Split('_')[1] ; } }
        public static string DensityUnit { get { return CurrentUnit.ToString().Split('_')[0]+"/" + CurrentUnit.ToString().Split('_')[1] + "³"; } }
        public static string EIUnit { get { return CurrentUnit.ToString().Split('_')[0]+ CurrentUnit.ToString().Split('_')[1] + "²"; } }
        public static Dictionary<Units, double> UnitDimensionFactors = new Dictionary<Units, double> {{ Units.kg_mm, 1000 },{Units.kg_cm,100 },{ Units.kg_m,1},
                                                                                                        { Units.ton_mm, 1000 },{ Units.ton_cm, 100 },{ Units.ton_m, 1 },
                                                                                                        { Units.N_mm, 1000 },{ Units.N_cm, 100 },{ Units.N_m, 1 },
                                                                                                        { Units.kN_mm, 1000 },{ Units.kN_cm, 100 },{ Units.kN_m, 1 } };

        public static Dictionary<Units,double> UnitForceFactors = new Dictionary<Units,double> {{ Units.kg_mm, (1000*gravity_factor) },{Units.kg_cm,(1000*gravity_factor) },{ Units.kg_m,(1000*gravity_factor)},
                                                                                                        { Units.ton_mm, gravity_factor },{ Units.ton_cm, gravity_factor },{ Units.ton_m, gravity_factor },
                                                                                                        { Units.N_mm, 1000 },{ Units.N_cm, 1000 },{ Units.N_m, 1000 },
                                                                                                        { Units.kN_mm, 1 },{ Units.kN_cm, 1 },{ Units.kN_m, 1 } };
        public static int maxPileCount = 20;
        public static int maxAnchorCount = 20;
        public static double penThickness = 0.04;
        public static double dimensionPenThickness = 0.03;
        public static double LoadPenThickness = 0.05;
        public static double freeTextPenThickness = 0.03;
        public static double dimensionExtension = 0.1;
        public static double dimensionDiff = 0.2;
        public static Typeface typeface = new Typeface(new FontFamily(new Uri("pack://application:,,,/"), "/Resources/#Quicksand Light"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal);
        public static double dimensionFontHeight = 0.4;
        public static double freeTextFontHeight = 0.4;
        public static double levelFontHeight = 0.4;
        public static double soilLayerBoxWidth = 3;
        public static double levelIconHeight = 0.3;
        public static double wireScaleFactor = 4;
        public static double rootScaleFactor = 2;
        public static double strutLength = 3;
        public static double maxLoadHeight = 2;
        public static double minLoadHeight = 0.3;
        public static double maxPointLoadHeight = 3;
        public static double loadArrowScale = 0.2;
        public static bool IsDimensionShowed = false;
        public static double waterDensity = 9.807;
        public static bool isModelReadyToAnalys = false;
        public static bool isAnalysisDone = false;

        public static void SortAnchors(this ObservableCollection<AnchorData> collection)
        {
            List<AnchorData> sorted = collection.OrderBy(x => x.AnchorDepth).ToList();

            int ptr = 0;
            while (ptr < sorted.Count - 1)
            {
                if (!collection[ptr].Equals(sorted[ptr]))
                {
                    int idx = search(collection, ptr + 1, sorted[ptr]);
                    collection.Move(idx, ptr);
                }

                ptr++;
            }
        }
        public static void SortStruts(this ObservableCollection<StrutData> collection)
        {
            List<StrutData> sorted = collection.OrderBy(x => x.StrutDepth).ToList();

            int ptr = 0;
            while (ptr < sorted.Count - 1)
            {
                if (!collection[ptr].Equals(sorted[ptr]))
                {
                    int idx = search(collection, ptr + 1, sorted[ptr]);
                    collection.Move(idx, ptr);
                }

                ptr++;
            }
        }

        public static int search<T>(ObservableCollection<T> collection, int startIndex, T other)
        {
            for (int i = startIndex; i < collection.Count; i++)
            {
                if (other.Equals(collection[i]))
                    return i;
            }

            return -1; // decide how to handle error case
        }
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

    public enum Stage
    {
        Materials,
        WallProperties,
        ExDesign,
        SoilMethod,
        Anchors,
        Struts,
        Surcharge,
        SystemLoad,
        Analysis
    }
    public enum LevelDirection
    {
        Left,
        Right,
        Point
    }
    public enum SoilRockTypes
    {
        None,
        Soil,
        Rock,
    }
    public enum SoilTypes
    {
        None,
        Gravel,
        Sand,
        Silt,
        Clay,
    }
    public enum SoilDenseTypes
    {
        None,
        Loose,
        MediumDense,
        Dense,
    }
    public enum SoilStiffTypes
    {
        None,
        Soft,
        MediumStiff,
        Stiff,
    }
    public enum SiltTypes
    {
        None,
        NonPlastic,
        Plastic,
    }
    public enum RockTypes
    {
        None,
        Volcanic,
        Metamorphic,
        Sedimentary,
    }
    public enum RockSubTypes
    {
        None,
        Poor,
        Fair,
        Excellent,
    }
    public enum SoilState
    {
        Drained,
        UnDrained,
    }
    public enum AnalysMethod
    {
        ClassicLoading,
        EquivalentLinear,
        FHWA
    }
    public enum DrainedTheories
    {
        TBDY,
        MazindraniTheory,
        TheColoumbTheory,
        RankineTheory
    }
    public enum UnDrainedTheories
    {
        TBDY,
        MazindraniTheory,
        TheColoumbTheory,
        RankineTheory,
        TotalStress
    }

    public enum LoadType
    {
        SurfaceLoad,
        StripLoad,
        LineLoad,
        PointLoad,
        Back_WaterPressure,
        Front_WaterPressure,
        HydroStaticWaterPressure,
        Back_EffectiveStress,
        Front_EffectiveStress,
        Back_TotalStress,
        Front_TotalStress,
        SubgradeModulusofSoil,
        Front_Kactive,
        Front_Kpassive,
        Back_Kactive,
        Back_Kpassive,
    }
}
