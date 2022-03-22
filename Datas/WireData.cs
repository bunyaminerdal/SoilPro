using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExDesign.Datas
{
    public class WireData
    {
        public string NominalDiameter { get; set; }
        public double BreakingStrength { get; set; }
        public double NominalArea { get; set; }
        public bool isDefault { get; set; }
    }
    public static class Wire
    {
        public static ObservableCollection<WireData> WireDataList = new ObservableCollection<WireData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Wire.gexdb";
        public static void WireDataReader()
        {
            if (!File.Exists(Path))
            {
                WireData wire1 = new WireData() { isDefault = true, NominalDiameter = "3/8 in", NominalArea = 0.00005484,BreakingStrength = 102.3 };
                WireData wire2 = new WireData() { isDefault = true, NominalDiameter = "7/16 in", NominalArea = 0.00007419,BreakingStrength = 137.9 };
                WireData wire3 = new WireData() { isDefault = true, NominalDiameter = "1/2 in", NominalArea = 0.00009871,BreakingStrength = 183.7 };
                WireData wire4 = new WireData() { isDefault = true, NominalDiameter = "0.60 in", NominalArea = 0.000140,BreakingStrength = 260.7 };
                
                List<WireData> tempList = new List<WireData>();
                tempList.Add(wire1);
                tempList.Add(wire2);
                tempList.Add(wire3);
                tempList.Add(wire4);
                
                string json = JsonConvert.SerializeObject(tempList.ToArray());

                //write string to file
                File.WriteAllText(Path, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                WireDataList = (ObservableCollection<WireData>)serializer.Deserialize(file, typeof(ObservableCollection<WireData>));
            }
        }
        public static void pileDiameterSave()
        {

            string json = JsonConvert.SerializeObject(WireDataList.ToArray());

            //write string to file
            File.WriteAllText(Path, json);
        }
        

    }
}
