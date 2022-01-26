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
    }
    
    public static class Steel
        
    {
        public static ObservableCollection<SteelData> SteelDataList = new ObservableCollection<SteelData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Steel.gexdb";
        public static void SteelDataReader()
        {
            if (!File.Exists(Path))
            {
                SteelData steel1 = new SteelData() { isDefault = true, Name = "S235", E=1,fy = 1};
                List<SteelData> tempList = new List<SteelData>();
                tempList.Add(steel1);

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
