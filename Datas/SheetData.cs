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
    public class SheetData
    {   

        public string Name { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public double Thickness { get; set; }
        public double Area { get; set; }
        public double Inertia { get; set; }
        public double Wely { get; set; }
        public double Wply { get; set; }
        public bool isDefault { get; set; }
        public int type { get; set; }

    }

    public static class Sheet
    {
        public static ObservableCollection<SheetData> SheetDataList = new ObservableCollection<SheetData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Sheet.gexdb";
        public static void SheetDataReader()
        {
            if (!File.Exists(Path))
            {
                SheetData sheet1 = new SheetData() { isDefault = true,type=0, Name = "AU14", Length=0.75, Height=0.204,Thickness = 0.01,Inertia= 0.0002868 ,Area= 0.0132 ,Wely= 0.001405 ,Wply= 0.001663 };

                List<SheetData> tempList = new List<SheetData>();
                tempList.Add(sheet1);

                string json = JsonConvert.SerializeObject(tempList.ToArray());

                //write string to file
                File.WriteAllText(Path, json);
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                SheetDataList = (ObservableCollection<SheetData>)serializer.Deserialize(file, typeof(ObservableCollection<SheetData>));
            }
        }
        public static void SheetSave()
        {

            string json = JsonConvert.SerializeObject(SheetDataList.ToArray());

            //write string to file
            File.WriteAllText(Path, json);
        }
        public static void GetSheetDataList(ComboBox combo)
        {
            combo.ItemsSource = SheetDataList;
            combo.DisplayMemberPath = "Name";
        }

    }
}
