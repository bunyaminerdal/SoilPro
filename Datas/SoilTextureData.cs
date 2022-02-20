using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace ExDesign.Datas
{
    public class SoilTextureData
    {
        public Guid ID { get; set; }
        public Uri TextureUri { get; set; }
        public string TextureName { get; set; }
        public int TextureIndex { get; set; }
        public object Clone()
        {
            var clone = (SoilTextureData)this.MemberwiseClone();
            return clone;
        }
    }

    public static class SoilTexture
    {
        public static ObservableCollection<SoilTextureData> tempSoilTextureDataList = new ObservableCollection<SoilTextureData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "SoilTextureLibrary.gexdb";
        public static void SoilTextureLibraryDataReader()
        {
            if (!File.Exists(Path))
            {

                SoilTextureData soilTextureData = new SoilTextureData { ID = Guid.NewGuid(), TextureName = "kil", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/Kil.png", UriKind.Absolute) };
                SoilTextureData soilTextureData1 = new SoilTextureData { ID = Guid.NewGuid(), TextureName = "kili kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/killikum.png", UriKind.Absolute) };
                SoilTextureData soilTextureData2 = new SoilTextureData { ID = Guid.NewGuid(), TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil.png", UriKind.Absolute) };
                SoilTextureData soilTextureData3 = new SoilTextureData { ID = Guid.NewGuid(), TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil1.png", UriKind.Absolute) };
                SoilTextureData soilTextureData4 = new SoilTextureData { ID = Guid.NewGuid(), TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil2.png", UriKind.Absolute) };
                SoilTextureData soilTextureData5 = new SoilTextureData { ID = Guid.NewGuid(), TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil2.png", UriKind.Absolute) };
                SoilTextureData soilTextureData6 = new SoilTextureData { ID = Guid.NewGuid(), TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil2.png", UriKind.Absolute) };
                List<SoilTextureData> tempList = new List<SoilTextureData>();
                tempList.Add(soilTextureData);
                tempList.Add(soilTextureData1);
                tempList.Add(soilTextureData2);
                tempList.Add(soilTextureData3);
                tempList.Add(soilTextureData4);
                tempList.Add(soilTextureData5);
                tempList.Add(soilTextureData6);

                string json = JsonConvert.SerializeObject(tempList.ToArray());

                //write string to file
                File.WriteAllText(Path, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                tempSoilTextureDataList = (ObservableCollection<SoilTextureData>)serializer.Deserialize(file, typeof(ObservableCollection<SoilTextureData>));
            }
        }
        
    }

}
