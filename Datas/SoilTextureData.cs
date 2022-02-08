using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace ExDesign.Datas
{
    public class SoilTextureData
    {
       public Uri TextureUri { get; set; }
       public string TextureName { get; set; }
        public int TextureIndex { get; set; }
    }

    public static class SoilTexture
    {
        public static ObservableCollection<SoilTextureData> tempSoilTextureDataList = new ObservableCollection<SoilTextureData>();
        public static void SetSoilTextures()
        {
            SoilTextureData soilTextureData = new SoilTextureData { TextureIndex = 0, TextureName = "kil", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/Kil.png", UriKind.Absolute) };
            tempSoilTextureDataList.Add(soilTextureData);
            SoilTextureData soilTextureData1 = new SoilTextureData { TextureIndex = 1, TextureName = "kili kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/killikum.png", UriKind.Absolute) };
            tempSoilTextureDataList.Add(soilTextureData1);
            SoilTextureData soilTextureData2 = new SoilTextureData { TextureIndex = 1, TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil.png", UriKind.Absolute) };
            tempSoilTextureDataList.Add(soilTextureData2);
            SoilTextureData soilTextureData3 = new SoilTextureData { TextureIndex = 1, TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil1.png", UriKind.Absolute) };
            tempSoilTextureDataList.Add(soilTextureData3);
            SoilTextureData soilTextureData4 = new SoilTextureData { TextureIndex = 1, TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil2.png", UriKind.Absolute) };
            tempSoilTextureDataList.Add(soilTextureData4);
            SoilTextureData soilTextureData5 = new SoilTextureData { TextureIndex = 1, TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil2.png", UriKind.Absolute) };
            tempSoilTextureDataList.Add(soilTextureData5);
            SoilTextureData soilTextureData6 = new SoilTextureData { TextureIndex = 1, TextureName = "kum", TextureUri = new Uri("pack://application:,,,/ExDesign;component/Textures/Soil/soil2.png", UriKind.Absolute) };
            tempSoilTextureDataList.Add(soilTextureData6);
        }
    }

}
