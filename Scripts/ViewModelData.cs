using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ExDesign.Pages.Inputs.Views;
using Newtonsoft.Json;

namespace ExDesign.Scripts
{
    public  class ViewModelData
    {
       
        public Point3D center3d { get; set; }
        public double wall_t { get; set; }
        public double wall_h { get; set; }
        public double wall_d { get; set; }
        public double pile_s { get; set; }
        public double frontandbackCubeLength { get; set; }
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

        public void ChangeWallHeight(double h)
        {
            wall_h = h;
           StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeWallThickness(double d)
        {

            capBeam_b = (capBeam_b - wall_t) + d;
            wall_t = d;

            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeexcavationHeight(double exHeight)
        {
            excavationHeight = exHeight;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeexcavationZ(double exZ)
        {
            frontT_Z = exZ;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeexcavationX1(double exX1)
        {
            frontT_X1 = exX1;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeexcavationX2(double exX2)
        {
            frontT_X2 = exX2;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeSurfaceBeta(double surfaceBeta)
        {
            backT_Beta = surfaceBeta;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeSurfaceB(double surfaceB)
        {
            backT_B = surfaceB;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeSurfaceA1(double surfaceA1)
        {
            backT_A1 = surfaceA1;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeSurfaceA2(double surfaceA2)
        {
            backT_A2 = surfaceA2;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeGroundWaterH1(double gw_h1)
        {
            groundW_h1 = gw_h1;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeGroundWaterH2(double gw_h2)
        {
            groundW_h2 = gw_h2;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangePileSpace(double p_s)
        {
            pile_s = p_s;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeCapBeamH(double cb_h)
        {
            capBeam_h = cb_h;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeCapBeamB(double cb_b)
        {
            capBeam_b = cb_b;
            StaticVariables.view3DPage.Refresh3Dview();
        }
        public void ChangeUnitIndex(int unitIndex)
        {
            UnitIndex = unitIndex;
        }
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
    }
    public static class ViewModel
    {
        public static ViewModelData viewModelData = new ViewModelData();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Model.exdb";
        public static void RestartModel( )
        {
            if (!File.Exists(Path))
            {
                ViewModelData viewModel = new ViewModelData()
                {
                    backT_A1 = 2,
                    backT_A2 = 2,
                    center3d = new Point3D(0, 0, 0),
                    wall_t = 0.65,
                    wall_h = 12,
                    wall_d = 7,
                    pile_s = 0.80,
                    frontandbackCubeLength = 15,
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
                };
                
                string json = JsonConvert.SerializeObject(viewModel);

                //write string to file
                File.WriteAllText(Path, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                StaticVariables.viewModel = (ViewModelData)serializer.Deserialize(file, typeof(ViewModelData));
            }
        }
        public static void ModelSave()
        {
            viewModelData = StaticVariables.viewModel;
            string json = JsonConvert.SerializeObject(viewModelData);

            //write string to file
            File.WriteAllText(Path, json);
        }
        public static void ModelSaveAs(string path)
        {
            viewModelData = StaticVariables.viewModel;
            string json = JsonConvert.SerializeObject(viewModelData);
            StaticVariables.Path = path;
            //write string to file
            File.WriteAllText(path, json);
        }
        public static void OpenModel(string path)
        {

            if(File.Exists(path))
            using (StreamReader file = File.OpenText(path))
            {
                StaticVariables.Path = path;
                JsonSerializer serializer = new JsonSerializer();
                viewModelData = (ViewModelData)serializer.Deserialize(file, typeof(ViewModelData));
                    
                    
                }
        }
        
    }
}
