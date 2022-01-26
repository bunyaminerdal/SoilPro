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
    public class ConcreteData
    {
        public string Name { get; set; }
        public double fck { get; set; }
        public double fct { get; set; }
        public double E { get; set; }
        public double G { get; set; }
        public bool isDefault { get; set; }
    }
    
    public static class Concrete
    {
        public static ObservableCollection<ConcreteData> ConcreteDataList = new ObservableCollection<ConcreteData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Concrete.gexdb";
        public static void ConcreteDataReader()
        {
            if (!File.Exists(Path))
            {
                ConcreteData concrete1 = new ConcreteData() { isDefault = true, Name = "C20", fck = 20000, fct=1570,E=28534000,G=11414000 };

                List<ConcreteData> tempList = new List<ConcreteData>();
                tempList.Add(concrete1);

                string json = JsonConvert.SerializeObject(tempList.ToArray());

                //write string to file
                File.WriteAllText(Path, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                ConcreteDataList = (ObservableCollection<ConcreteData>)serializer.Deserialize(file, typeof(ObservableCollection<ConcreteData>));
            }
        }
        public static void ConcreteSave()
        {

            string json = JsonConvert.SerializeObject(ConcreteDataList.ToArray());

            //write string to file
            File.WriteAllText(Path, json);
        }
        public static void GetConcreteDataList(ComboBox combo)
        {
            combo.ItemsSource = ConcreteDataList;
            combo.DisplayMemberPath = "Name";
        }

    }
}
