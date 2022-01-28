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
    public class RebarData
    {
        public string Name { get; set; }
        public double fyk { get; set; }
        public double E { get; set; }
        public bool isDefault { get; set; }
    }
    
    public static class Rebar   
    {
        public static ObservableCollection<RebarData> RebarDataList = new ObservableCollection<RebarData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Rebar.gexdb";
        public static void RebarDataReader()
        {
            if (!File.Exists(Path))
            {
                RebarData rebar1 = new RebarData() { isDefault = true, Name = "B420C", fyk = 420000, E = 200000000 };
                List<RebarData> tempList = new List<RebarData>();
                tempList.Add(rebar1);

                string json = JsonConvert.SerializeObject(tempList.ToArray());

                //write string to file
                File.WriteAllText(Path, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                RebarDataList = (ObservableCollection<RebarData>)serializer.Deserialize(file, typeof(ObservableCollection<RebarData>));
            }
        }
        public static void RebarSave()
        {

            string json = JsonConvert.SerializeObject(RebarDataList.ToArray());

            //write string to file
            File.WriteAllText(Path, json);
        }
        public static void GetRebarDataList(ComboBox combo)
        {
            combo.ItemsSource = RebarDataList;
            combo.DisplayMemberPath = "Name";
        }

    }
}
