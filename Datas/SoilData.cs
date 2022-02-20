using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ExDesign.Datas;

namespace ExDesign.Datas
{
    public class SoilData
    {
        public Guid ID { get; set; }
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
        public object Clone()
        {
            var clone = (SoilData)this.MemberwiseClone();
            return clone;
        }
    }
    
    public static class SoilLibrary
    {
        public static ObservableCollection<SoilData> SoilDataList = new ObservableCollection<SoilData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "SoilLibrary.gexdb";
        public static void SoilLibraryDataReader()
        {
            if (!File.Exists(Path))
            {
                SoilData soil1 = new SoilData() { ID = Guid.NewGuid(), isDefault = true, Name = "Kötü Derecelenmiş Gevşek Kum", NaturalUnitWeight = 18, SaturatedUnitWeight = 18.5, SoilStressStateIndex = 0, SoilStateKoIndex = 1, SoilFrictionAngle = 27, EffectiveCohesion = 0, UndrainedShearStrength = 0, WallSoilFrictionAngle = 18, WallSoilAdhesion = 0, PoissonRatio = 0.3, K0 = 0.55, Ocr = 1, OedometricModulus = 8, CohesionFactor = 1, YoungModulus = 5.9, isSoilTexture = true, SoilColor = Colors.Aqua, SoilTexture = SoilTexture.tempSoilTextureDataList[0], };
                SoilData soil2 = new SoilData() { ID = Guid.NewGuid(), isDefault = true, Name = "Kötü Derecelenmiş Orta Sıkı Kum", NaturalUnitWeight = 18.7, SaturatedUnitWeight = 19.2, SoilStressStateIndex = 0, SoilStateKoIndex = 1, SoilFrictionAngle = 31, EffectiveCohesion = 0, UndrainedShearStrength = 0, WallSoilFrictionAngle = 20.7, WallSoilAdhesion = 0, PoissonRatio = 0.3, K0 = 0.48, Ocr = 1, OedometricModulus = 20, CohesionFactor = 1, YoungModulus = 14.9, isSoilTexture = true, SoilColor = Colors.Aqua, SoilTexture = SoilTexture.tempSoilTextureDataList[0], };
                SoilData soil3 = new SoilData() { ID = Guid.NewGuid(), isDefault = true, Name = "Kötü Derecelenmiş Sıkı Kum", NaturalUnitWeight = 19.7, SaturatedUnitWeight = 20.2, SoilStressStateIndex = 0, SoilStateKoIndex = 1, SoilFrictionAngle = 34, EffectiveCohesion = 0, UndrainedShearStrength = 0, WallSoilFrictionAngle = 22.7, WallSoilAdhesion = 0, PoissonRatio = 0.3, K0 = 0.44, Ocr = 1, OedometricModulus = 45, CohesionFactor = 1, YoungModulus = 33.4, isSoilTexture = true, SoilColor = Colors.Aqua, SoilTexture = SoilTexture.tempSoilTextureDataList[0], };
                SoilData soil4 = new SoilData() { ID = Guid.NewGuid(), isDefault = true, Name = "İyi Derecelenmiş Gevşek Kum", NaturalUnitWeight = 18, SaturatedUnitWeight = 18.5, SoilStressStateIndex = 0, SoilStateKoIndex = 1, SoilFrictionAngle = 28, EffectiveCohesion = 0, UndrainedShearStrength = 0, WallSoilFrictionAngle = 18.7, WallSoilAdhesion = 0, PoissonRatio = 0.3, K0 = 0.53, Ocr = 1, OedometricModulus = 10, CohesionFactor = 1, YoungModulus = 7.4, isSoilTexture = true, SoilColor = Colors.Aqua, SoilTexture = SoilTexture.tempSoilTextureDataList[0], };
                SoilData soil5 = new SoilData() { ID = Guid.NewGuid(), isDefault = true, Name = "İyi Derecelenmiş Orta Sıkı Kum", NaturalUnitWeight = 19.2, SaturatedUnitWeight = 19.4, SoilStressStateIndex = 0, SoilStateKoIndex = 1, SoilFrictionAngle = 33, EffectiveCohesion = 0, UndrainedShearStrength = 0, WallSoilFrictionAngle = 22, WallSoilAdhesion = 0, PoissonRatio = 0.3, K0 = 0.46, Ocr = 1, OedometricModulus = 30, CohesionFactor = 1, YoungModulus = 22.3, isSoilTexture = true, SoilColor = Colors.Aqua, SoilTexture = SoilTexture.tempSoilTextureDataList[0], };
                SoilData soil6 = new SoilData() { ID = Guid.NewGuid(), isDefault = true, Name = "İyi Derecelenmiş Sıkı Kum", NaturalUnitWeight = 19.7, SaturatedUnitWeight = 20.2, SoilStressStateIndex = 0, SoilStateKoIndex = 1, SoilFrictionAngle = 36, EffectiveCohesion = 0, UndrainedShearStrength = 0, WallSoilFrictionAngle = 24, WallSoilAdhesion = 0, PoissonRatio = 0.3, K0 = 0.41, Ocr = 1, OedometricModulus = 65, CohesionFactor = 1, YoungModulus = 48.3, isSoilTexture = true, SoilColor = Colors.Aqua, SoilTexture = SoilTexture.tempSoilTextureDataList[0], };


                List<SoilData> tempList = new List<SoilData>();
                tempList.Add(soil1);
                tempList.Add(soil2);
                tempList.Add(soil3);
                tempList.Add(soil4);
                tempList.Add(soil5);
                tempList.Add(soil6);

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
