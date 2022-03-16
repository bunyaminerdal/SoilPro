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
    public class ConcreteData : ICloneable
    {
        public string Name { get; set; }
        public double fck { get; set; }
        public double fct { get; set; }
        public double E { get; set; }
        public double G { get; set; }
        public bool isDefault { get; set; }
        

        public object Clone()
        {
            var clone = (ConcreteData)this.MemberwiseClone();
            return clone;
        }
    }
    
    public static class Concrete
    {
        public static ObservableCollection<ConcreteData> ConcreteDataList = new ObservableCollection<ConcreteData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Concrete.gexdb";
        public static void ConcreteDataReader()
        {
            if (!File.Exists(Path))
            {
                ConcreteData concrete1 = new ConcreteData() { isDefault = true, Name = "C20", fck = 20000, fct = 1600, E = 28000000, G = 11200000 };
                ConcreteData concrete2 = new ConcreteData() { isDefault = true, Name = "C25", fck = 25000, fct = 1800, E = 30000000, G = 12000000 };
                ConcreteData concrete3 = new ConcreteData() { isDefault = true, Name = "C30", fck = 30000, fct = 1900, E = 32000000, G = 12800000 };
                ConcreteData concrete4 = new ConcreteData() { isDefault = true, Name = "C35", fck = 35000, fct = 2100, E = 33000000, G = 13200000 };
                ConcreteData concrete5 = new ConcreteData() { isDefault = true, Name = "C40", fck = 40000, fct = 2200, E = 34000000, G = 13600000 };
                ConcreteData concrete6 = new ConcreteData() { isDefault = true, Name = "C45", fck = 45000, fct = 2300, E = 36000000, G = 14400000 };
                ConcreteData concrete7 = new ConcreteData() { isDefault = true, Name = "C50", fck = 50000, fct = 2500, E = 37000000, G = 14800000 };

                List<ConcreteData> tempList = new List<ConcreteData>();
                tempList.Add(concrete1);
                tempList.Add(concrete2);
                tempList.Add(concrete3);
                tempList.Add(concrete4);
                tempList.Add(concrete5);
                tempList.Add(concrete6);
                tempList.Add(concrete7);

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
