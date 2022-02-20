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
    public class PileData:ICloneable
    {
        
        public string Name { get; set; }
        public double t { get; set; }
        public bool isDefault { get; set; }
        public object Clone()
        {
            var clone = (PileData)this.MemberwiseClone();
            return clone;
        }
    }

    public static class Pile
    {
        public static ObservableCollection<PileData> PileDiameterDataList = new ObservableCollection<PileData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString()+"Pile.gexdb";
        public static void pileDiameterReader()
        {
            if(!File.Exists(Path))
            {
                PileData pile1 = new PileData() { isDefault = true,Name= "ф40", t=0.40};
                PileData pile2 = new PileData() { isDefault = true,Name= "ф65", t=0.65};
                PileData pile3 = new PileData() { isDefault = true,Name= "ф80", t=0.80};
                List<PileData> tempList = new List<PileData>();
                tempList.Add(pile1);
                tempList.Add(pile2);
                tempList.Add(pile3);
                string json = JsonConvert.SerializeObject(tempList.ToArray());

                //write string to file
                File.WriteAllText(Path, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                PileDiameterDataList= (ObservableCollection<PileData>)serializer.Deserialize(file, typeof(ObservableCollection<PileData>));
            }
        }
        public static void pileDiameterSave()
        {

            string json = JsonConvert.SerializeObject(PileDiameterDataList.ToArray());

            //write string to file
            File.WriteAllText(Path, json);
        }
        public static void GetPileDiameterDataList(ComboBox combo)
        {            
            combo.ItemsSource = PileDiameterDataList;
            combo.DisplayMemberPath = "Name";            
        }
        
    }

    
}
