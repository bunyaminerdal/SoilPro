using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ExDesign.Pages.Inputs.Views;
using ExDesign.Scripts;
using Newtonsoft.Json;

namespace ExDesign.Datas
{
    public  class ViewModelData
    {
        public string SaveDate { get; set; }
        public Point3D center3d { get; set; }
        public double wall_t { get; set; }
        public double wall_h { get; set; }
        public double wall_d { get; set; }
        public double pile_s { get; set; }
        public double frontCubeLength { get; set; }
        public double backCubeLength { get; set; }
        public double CubeLength { get; set; }
        public double TopOfWallLevel { get; set; }
        public double excavationHeight { get; set; }
        public double frontT_Z { get; set; }
        public double frontT_X1 { get; set; }
        public double frontT_X2 { get; set; }
        public double backT_Beta { get; set; }
        public double backT_B { get; set; }
        public double backT_A1 { get; set; }
        public double backT_A2 { get; set; }
        public double bottomT_h { get; set; }
        public double groundW_h1 { get; set; }
        public double groundW_h2 { get; set; }
        public double capBeam_h { get; set; }
        public double capBeam_b { get; set; }        
        public int UnitIndex { get; set; }
        public int WallTypeIndex { get; set; }
        public string Path { get; set; }
        public string ProjectName { get; set; }      
        public int ConcreteIndex { get; set; }
        public int RebarIndex { get; set; }
        public int SteelIndex { get; set; }
        public int PileIndex { get; set; }
        public int SheetIndex { get; set; }
        public int ExcavationTypeIndex { get; set; }
        public int GroundSurfaceTypeIndex { get; set; }
        public int WaterTypeIndex { get; set; } 
        public double WallArea { get; set; } 
        public double WallInertia { get; set; } 
        public double WallE { get; set; } 
        public double WallEI { get; set; } 
        public double WallEA { get; set; } 
        public int SoilModelIndex { get; set; }
        public bool useCableDiameterAndNumberForDesign { get; set; }
        public ObservableCollection<SoilLayerData> soilLayerDatas { get; set; }
        public ObservableCollection<AnchorData> anchorDatas { get; set; }
        public ObservableCollection<StrutData> strutDatas { get; set; }
        public ObservableCollection<SoilData> soilDatas { get; set; }
        public ObservableCollection<SoilTextureData> soilTextures { get; set; }
        public Stage stage { get; set; }
        public bool isCapBeamBottom { get; set; }

        public void ChangeWallHeight(double h)
        {
            wall_h = h;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeWallThickness(double d)
        {

            capBeam_b = (capBeam_b - wall_t) + d;
            wall_t = d;

            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeexcavationHeight(double exHeight)
        {
            excavationHeight = exHeight;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeexcavationZ(double exZ)
        {
            frontT_Z = exZ;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeexcavationX1(double exX1)
        {
            frontT_X1 = exX1;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeexcavationX2(double exX2)
        {
            frontT_X2 = exX2;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeSurfaceBeta(double surfaceBeta)
        {
            backT_Beta = surfaceBeta;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeSurfaceB(double surfaceB)
        {
            backT_B = surfaceB;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeSurfaceA1(double surfaceA1)
        {
            backT_A1 = surfaceA1;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeSurfaceA2(double surfaceA2)
        {
            backT_A2 = surfaceA2;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeGroundWaterH1(double gw_h1)
        {
            groundW_h1 = gw_h1;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeGroundWaterH2(double gw_h2)
        {
            groundW_h2 = gw_h2;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangePileSpace(double p_s)
        {
            pile_s = p_s;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeCapBeamH(double cb_h)
        {
            capBeam_h = cb_h;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeCapBeamB(double cb_b)
        {
            capBeam_b = cb_b;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeTopOfWallLevel(double towLevel)
        {
            TopOfWallLevel = towLevel;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }
        public void ChangeUnitIndex(int unitIndex)
        {
            UnitIndex = unitIndex;
        }
        public void ChangeWallProperties()
        {
            double Area = 0;
            switch (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    Area = wall_t;
                    break;
                case WallType.ConcretePileWall:
                    if (Pile.PileDiameterDataList.Count > 0) Area = Math.Pow(Pile.PileDiameterDataList[StaticVariables.viewModel.PileIndex].t, 2) * Math.PI / (4 * pile_s);
                    break;
                case WallType.SteelSheetWall:
                    if (Sheet.SheetDataList.Count > 0) Area = Sheet.SheetDataList[StaticVariables.viewModel.SheetIndex].Area;
                    break;
                default:
                    Area = wall_t;
                    break;
            }
            WallArea = Area;

            double Inertia = 0;
            switch (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    Inertia = Math.Pow(wall_t, 3) / 12;
                    break;
                case WallType.ConcretePileWall:
                    if (Pile.PileDiameterDataList.Count > 0) Inertia = Math.Pow(Pile.PileDiameterDataList[StaticVariables.viewModel.PileIndex].t, 4) * Math.PI / (64 * pile_s);
                    break;
                case WallType.SteelSheetWall:
                    if (Sheet.SheetDataList.Count > 0) Inertia = Sheet.SheetDataList[StaticVariables.viewModel.SheetIndex].Inertia;
                    break;
                default:
                    Inertia = Math.Pow(wall_t, 3) / 12;
                    break;
            }
            WallInertia = Inertia;
            double E = 0;
            switch (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    if (Concrete.ConcreteDataList.Count > 0) E = Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].E ;
                    break;
                case WallType.ConcretePileWall:
                    if (Concrete.ConcreteDataList.Count > 0) E = Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].E ;
                    break;
                case WallType.SteelSheetWall:
                    if (Steel.SteelDataList.Count > 0) E = Steel.SteelDataList[StaticVariables.viewModel.SteelIndex].E;
                    break;
                default:
                    if (Concrete.ConcreteDataList.Count > 0) E = Concrete.ConcreteDataList[StaticVariables.viewModel.ConcreteIndex].E;
                    break;
            }
            WallE = E;

            double EI = 0;
            switch (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    if (Concrete.ConcreteDataList.Count > 0)  EI = WallE*WallInertia;
                    break;
                case WallType.ConcretePileWall:
                    if (Concrete.ConcreteDataList.Count > 0) EI = WallE * WallInertia;
                    break;
                case WallType.SteelSheetWall:
                    if (Sheet.SheetDataList.Count > 0) EI = WallE * WallInertia;
                    break;
                default:
                    if (Concrete.ConcreteDataList.Count > 0) EI = WallE * WallInertia;
                    break;
            }
            WallEI = EI;

            double EA = 0;
            switch (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    if (Concrete.ConcreteDataList.Count > 0) EA = WallE * WallArea;
                    break;
                case WallType.ConcretePileWall:
                    if (Concrete.ConcreteDataList.Count > 0) EA = WallE * WallArea;
                    break;
                case WallType.SteelSheetWall:
                    if (Sheet.SheetDataList.Count > 0) EA = WallE * WallArea;
                    break;
                default:
                    if (Concrete.ConcreteDataList.Count > 0) EA = WallE * WallArea;
                    break;
            }
            WallEA = EA;
        }
        public double GetWallArea() { return WallArea; }
        public double GetWallInertia() { return WallInertia; }
        public double GetWallE() { return WallE; }
        public double GetWallEI() { return WallEI; }
        public double GetWallEA() { return WallEA; }
        public double GetWallHeight() { return wall_h; }
        public double GetWallThickness() { return wall_t; }
        public double GetexcavationHeight() { return excavationHeight; }
        public double GetexcavationZ() { return frontT_Z; }
        public double GetexcavationX1() { return frontT_X1; }
        public double GetexcavationX2() { return frontT_X2; }
        public double GetSurfaceBeta() { return backT_Beta; }
        public double GetSurfaceB() { return backT_B; }
        public double GetSurfaceA1() { return backT_A1; }
        public double GetSurfaceA2() { return backT_A2; }
        public double GetGroundWaterH1() { return groundW_h1; }
        public double GetGroundWaterH2() { return groundW_h2; }
        public double GetPileSpace() { return pile_s; }
        public double GetCapBeamH() { return capBeam_h; }
        public double GetCapBeamB() { return capBeam_b; }
        public int GetUnitIndex() { return UnitIndex; }
        public double GetTopOfWallLevel() { return TopOfWallLevel; }
    }
    public static class ViewModel
    {
        public static ViewModelData viewModelData = new ViewModelData();
        public static string tempPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Model.exdb";
        public static void RestartModel( )
        {
            if (!File.Exists(tempPath))
            {
                ViewModelData viewModel = new ViewModelData()
                {
                    backT_A1 = 2,
                    backT_A2 = 2,
                    center3d = new Point3D(0, 0, 0),
                    wall_t = 0.65,
                    wall_h = 12,
                    wall_d = 11.5,
                    pile_s = 0.80,
                    frontCubeLength = 8,
                    backCubeLength = 12,
                    CubeLength = 15,
                    TopOfWallLevel = 0,
                    excavationHeight = 8,
                    frontT_Z = 2,
                    frontT_X1 = 2,
                    frontT_X2 = 2,
                    backT_Beta = 10,
                    backT_B = 1,
                    bottomT_h = 3,
                    groundW_h1 = 5,
                    groundW_h2 = 2,
                    capBeam_h = 0.8,
                    capBeam_b = 0.65,
                    UnitIndex = 11,
                    WallTypeIndex = 1,
                    PileIndex = 1,
                    ConcreteIndex = 0,
                    RebarIndex = 0,
                    SteelIndex = 0,
                    SheetIndex = 0,
                    ExcavationTypeIndex = 0,
                    GroundSurfaceTypeIndex = 0,
                    WaterTypeIndex = 0,
                    WallArea = 1,
                    WallInertia = 1,
                    WallE = 1,
                    WallEA = 2,
                    WallEI = 3,
                    SoilModelIndex = 0,
                    Path = "Untitled",
                    ProjectName = "Untitled",
                    SaveDate = "0",
                    useCableDiameterAndNumberForDesign = true,
                    isCapBeamBottom = true,
                    soilDatas = new ObservableCollection<SoilData>(),
                    soilLayerDatas = new ObservableCollection<SoilLayerData>(),
                    anchorDatas = new ObservableCollection<AnchorData>(),
                    strutDatas = new ObservableCollection<StrutData>(),
                    soilTextures = new ObservableCollection<SoilTextureData>(),

                    stage = Stage.Materials,
                    
                };
                
                string json = JsonConvert.SerializeObject(viewModel);

                //write string to file
                File.WriteAllText(tempPath, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(tempPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                StaticVariables.viewModel = (ViewModelData)serializer.Deserialize(file, typeof(ViewModelData));
            }
        }
        public static void ModelSave()
        {
            StaticVariables.viewModel.SaveDate = DateTime.Now.ToString();
            viewModelData = StaticVariables.viewModel;
            string json = JsonConvert.SerializeObject(viewModelData);

            //write string to file
            File.WriteAllText(StaticVariables.viewModel.Path, json);
            if (ProgramModel.CheckPath(StaticVariables.viewModel.Path)) ProgramModel.ModelSave(StaticVariables.viewModel.Path);
        }
        public static void ModelSaveAs(string path,string projectName)
        {
            StaticVariables.viewModel.SaveDate = DateTime.Now.ToString();
            StaticVariables.viewModel.Path = path;
            StaticVariables.viewModel.ProjectName = projectName;
            viewModelData = StaticVariables.viewModel;
            string json = JsonConvert.SerializeObject(viewModelData);
            
            //write string to file
            File.WriteAllText(path, json);
            if (ProgramModel.CheckPath(path)) ProgramModel.ModelSave(path);
        }
        public static void OpenModel(string path)
        {

            if(File.Exists(path))
            using (StreamReader file = File.OpenText(path))
            {
                
                JsonSerializer serializer = new JsonSerializer();
                StaticVariables.viewModel = (ViewModelData)serializer.Deserialize(file, typeof(ViewModelData));
                if(StaticVariables.viewModel!=null) StaticVariables.viewModel.Path = path;
                if (ProgramModel.CheckPath(path)) ProgramModel.ModelSave(path);
            }
        }
        
    }
}
