using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ExDesign.Datas
{
    public class SoilData
    {
        public string Name { get; set; }
        public double NaturalUnitWeight { get; set; }
        public double SaturatedUnitWeight { get; set; }
        public int SoilStressStateIndex { get; set; }
        public int SoilStateKoIndex { get; set; }
        public double SoilFrictionAngle { get; set; }
        public double EffectiveCohesion { get; set; }
        public double UndrainedShearStrength { get; set; }
        public double WallSoilFrictionAngle { get; set; }
        public double WallSoilAdhesion { get; set; }
        public double PoissonRatio { get; set; }
        public double K0 { get; set; }
        public double Ocr { get; set; }
        public double OedometricModulus { get; set; }
        public double CohesionFactor { get; set; }
        public double YoungModulus { get; set; }
        public bool isDefault { get; set; }
        public bool isSoilTexture { get; set; }
        public SoilTextureData SoilTexture { get; set; }
        public Color SoilColor  { get; set; }
    }
    
    public static class SoilLibrary
    {
        public static ObservableCollection<SoilData> SoilDataList = new ObservableCollection<SoilData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "SoilLibrary.gexdb";
        public static void SoilLibraryDataReader()
        {
            if (!File.Exists(Path))
            {
                SoilTextureData soilTextureData = new SoilTextureData { TextureIndex = 0, TextureName = "kil", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/Kil.png", UriKind.Absolute) };
                SoilTextureData soilTextureData1 = new SoilTextureData { TextureIndex = 1, TextureName = "kili kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/killikum.png", UriKind.Absolute) };

                SoilData soil1 = new SoilData() {
                    isDefault = true,
                    Name = "gevşek Kil",
                    NaturalUnitWeight = 18,
                    SaturatedUnitWeight = 18.5,
                    SoilStressStateIndex = 0,
                    SoilStateKoIndex = 0,
                    SoilFrictionAngle = 27,
                    EffectiveCohesion = 10,
                    UndrainedShearStrength = 11,
                    WallSoilFrictionAngle = 18,
                    WallSoilAdhesion = 12,
                    PoissonRatio = 0.2,
                    K0 = 20,
                    Ocr = 30,
                    OedometricModulus = 15,
                    CohesionFactor = 16,
                    YoungModulus = 17,
                    isSoilTexture = true,
                    SoilColor = Colors.Aqua,
                    SoilTexture = soilTextureData,
                };
                SoilData soil2 = new SoilData()
                {
                    isDefault = true,
                    Name = "gevşek kumlu Kil",
                    NaturalUnitWeight = 19,
                    SaturatedUnitWeight = 19.5,
                    SoilStressStateIndex = 1,
                    SoilStateKoIndex = 1,
                    SoilFrictionAngle = 27,
                    EffectiveCohesion = 10,
                    UndrainedShearStrength = 11,
                    WallSoilFrictionAngle = 18,
                    WallSoilAdhesion = 12,
                    PoissonRatio = 0.2,
                    K0 = 20,
                    Ocr = 30,
                    OedometricModulus = 15,
                    CohesionFactor = 16,
                    YoungModulus = 17,
                    isSoilTexture = true,
                    SoilColor = Colors.Aqua,
                    SoilTexture = soilTextureData1,
                };
                List<SoilData> tempList = new List<SoilData>();
                tempList.Add(soil1);
                tempList.Add(soil2);

                string json = JsonConvert.SerializeObject(tempList.ToArray());

                //write string to file
                File.WriteAllText(Path, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                SoilDataList = (ObservableCollection<SoilData>)serializer.Deserialize(file, typeof(ObservableCollection<SoilData>));
            }
        }
       
        
    }
}
