using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExDesign.Datas
{
    public class SteelData
    {
        public string Name { get; set; }
        public double fy { get; set; }
        public double E { get; set; }
        public bool isDefault { get; set; }
        public object Clone()
        {
            var clone = (SteelData)this.MemberwiseClone();
            return clone;
        }
    }
    
    public static class Steel
        
    {
        public static ObservableCollection<SteelData> SteelDataList = new ObservableCollection<SteelData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Steel.gexdb";
        public static void SteelDataReader()
        {
            if (!File.Exists(Path))
            {
                SteelData steel1 = new SteelData() { isDefault = true, Name = "S235H", fy = 235000, E = 200000000 };
                SteelData steel2 = new SteelData() { isDefault = true, Name = "S275H", fy = 275000, E = 200000000 };
                SteelData steel3 = new SteelData() { isDefault = true, Name = "S355H", fy = 355000, E = 200000000 };

                List<SteelData> tempList = new List<SteelData>();
                tempList.Add(steel1);
                tempList.Add(steel2);
                tempList.Add(steel3);

                string json = JsonConvert.SerializeObject(tempList.ToArray());

                //write string to file
                File.WriteAllText(Path, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                SteelDataList = (ObservableCollection<SteelData>)serializer.Deserialize(file, typeof(ObservableCollection<SteelData>));
            }
        }
        public static void SteelSave()
        {

            string json = JsonConvert.SerializeObject(SteelDataList.ToArray());

            //write string to file
            File.WriteAllText(Path, json);
        }
        public static void GetSteelDataList(ComboBox combo)
        {
            combo.ItemsSource = SteelDataList;
            combo.DisplayMemberPath = "Name";
        }

    }
}
