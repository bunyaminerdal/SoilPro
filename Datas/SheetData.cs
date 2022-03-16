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
        public object Clone()
        {
            var clone = (SheetData)this.MemberwiseClone();
            return clone;
        }

    }

    public static class Sheet
    {
        public static ObservableCollection<SheetData> SheetDataList = new ObservableCollection<SheetData>();
        public static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Sheet.gexdb";
        public static void SheetDataReader()
        {
            if (!File.Exists(Path))
            {
                SheetData sheet1 = new SheetData() { isDefault = true, type = 0, Name = "AU14", Length = 0.75, Height = 0.2040, Thickness = 0.0100, Inertia = 0.0002868, Area = 0.0132, Wely = 0.001405, Wply = 0.001663 };
                SheetData sheet2 = new SheetData() { isDefault = true, type = 0, Name = "AU16", Length = 0.75, Height = 0.2055, Thickness = 0.0115, Inertia = 0.0003285, Area = 0.0147, Wely = 0.001600, Wply = 0.001891 };
                SheetData sheet3 = new SheetData() { isDefault = true, type = 0, Name = "AU18", Length = 0.75, Height = 0.2205, Thickness = 0.0105, Inertia = 0.0003930, Area = 0.0150, Wely = 0.001780, Wply = 0.002082 };
                SheetData sheet4 = new SheetData() { isDefault = true, type = 0, Name = "AU20", Length = 0.75, Height = 0.2220, Thickness = 0.0120, Inertia = 0.0004444, Area = 0.0165, Wely = 0.002000, Wply = 0.002339 };
                SheetData sheet5 = new SheetData() { isDefault = true, type = 0, Name = "AU25", Length = 0.75, Height = 0.2250, Thickness = 0.0145, Inertia = 0.0005624, Area = 0.0188, Wely = 0.002500, Wply = 0.002866 };
                SheetData sheet6 = new SheetData() { isDefault = true, type = 0, Name = "GU14N", Length = 0.60, Height = 0.2100, Thickness = 0.0100, Inertia = 0.0002941, Area = 0.013600, Wely = 0.001400, Wply = 0.001685 };
                SheetData sheet7 = new SheetData() { isDefault = true, type = 0, Name = "GU16N", Length = 0.60, Height = 0.2150, Thickness = 0.0102, Inertia = 0.0003595, Area = 0.015420, Wely = 0.001670, Wply = 0.001988 };
                SheetData sheet8 = new SheetData() { isDefault = true, type = 0, Name = "GU18N", Length = 0.60, Height = 0.2150, Thickness = 0.0112, Inertia = 0.0003865, Area = 0.016330, Wely = 0.001800, Wply = 0.002134 };
                SheetData sheet9 = new SheetData() { isDefault = true, type = 0, Name = "GU20N", Length = 0.60, Height = 0.2150, Thickness = 0.0122, Inertia = 0.0004132, Area = 0.017230, Wely = 0.001920, Wply = 0.002280 };
                SheetData sheet10 = new SheetData() { isDefault = true, type = 0, Name = "GU22N", Length = 0.60, Height = 0.2250, Thickness = 0.0121, Inertia = 0.0004946, Area = 0.018290, Wely = 0.002200, Wply = 0.002580 };
                SheetData sheet11 = new SheetData() { isDefault = true, type = 0, Name = "GU28N", Length = 0.60, Height = 0.2270, Thickness = 0.0152, Inertia = 0.0006446, Area = 0.021600, Wely = 0.002840, Wply = 0.003269 };
                SheetData sheet12 = new SheetData() { isDefault = true, type = 0, Name = "GU32N", Length = 0.60, Height = 0.2260, Thickness = 0.0195, Inertia = 0.0007232, Area = 0.024230, Wely = 0.003200, Wply = 0.003687 };
                SheetData sheet13 = new SheetData() { isDefault = true, type = 0, Name = "AZ18-800", Length = 0.80, Height = 0.2245, Thickness = 0.0085, Inertia = 0.0004132, Area = 0.0129, Wely = 0.001840, Wply = 0.002135 };
                SheetData sheet14 = new SheetData() { isDefault = true, type = 0, Name = "AZ20- 800", Length = 0.80, Height = 0.2330, Thickness = 0.0095, Inertia = 0.0004505, Area = 0.0141, Wely = 0.002000, Wply = 0.002330 };
                SheetData sheet15 = new SheetData() { isDefault = true, type = 0, Name = "AZ22 - 800", Length = 0.80, Height = 0.2255, Thickness = 0.0105, Inertia = 0.0004879, Area = 0.0153, Wely = 0.002165, Wply = 0.002330 };
                SheetData sheet16 = new SheetData() { isDefault = true, type = 0, Name = "AZ23 - 800", Length = 0.80, Height = 0.2370, Thickness = 0.0115, Inertia = 0.0005526, Area = 0.0151, Wely = 0.002330, Wply = 0.002680 };
                SheetData sheet17 = new SheetData() { isDefault = true, type = 0, Name = "AZ25 - 800", Length = 0.80, Height = 0.2375, Thickness = 0.0125, Inertia = 0.0005941, Area = 0.0163, Wely = 0.002500, Wply = 0.002890 };
                SheetData sheet18 = new SheetData() { isDefault = true, type = 0, Name = "AZ27 - 800", Length = 0.80, Height = 0.2380, Thickness = 0.0135, Inertia = 0.0006357, Area = 0.0176, Wely = 0.002670, Wply = 0.003100 };
                SheetData sheet19 = new SheetData() { isDefault = true, type = 0, Name = "MMU5-1", Length = 0.60, Height = 0.0750, Thickness = 0.0095, Inertia = 0.0000390, Area = 0.01198, Wely = 0.000510, Wply = 0.000551 };
                SheetData sheet20 = new SheetData() { isDefault = true, type = 0, Name = "MMU5 - 2", Length = 0.60, Height = 0.0750, Thickness = 0.01, Inertia = 0.0000416, Area = 0.01261, Wely = 0.000540, Wply = 0.000580 };
                SheetData sheet21 = new SheetData() { isDefault = true, type = 0, Name = "MMU6 - 1", Length = 0.60, Height = 0.1400, Thickness = 0.0095, Inertia = 0.0000546, Area = 0.008960, Wely = 0.000640, Wply = 0.000727 };
                SheetData sheet22 = new SheetData() { isDefault = true, type = 0, Name = "MMU7 - 1", Length = 0.60, Height = 0.1500, Thickness = 0.0060, Inertia = 0.0001051, Area = 0.009342, Wely = 0.000690, Wply = 0.000789 };
                SheetData sheet23 = new SheetData() { isDefault = true, type = 0, Name = "MMU7 - 2", Length = 0.60, Height = 0.1700, Thickness = 0.0060, Inertia = 0.0001327, Area = 0.009810, Wely = 0.000745, Wply = 0.000870 };
                SheetData sheet24 = new SheetData() { isDefault = true, type = 0, Name = "MMU8 - 1", Length = 0.60, Height = 0.1700, Thickness = 0.0065, Inertia = 0.0001475, Area = 0.010334, Wely = 0.000780, Wply = 0.000918 };
                SheetData sheet25 = new SheetData() { isDefault = true, type = 0, Name = "MMU8 - 2", Length = 0.60, Height = 0.1625, Thickness = 0.0070, Inertia = 0.0001500, Area = 0.010998, Wely = 0.000825, Wply = 0.000960 };
                SheetData sheet26 = new SheetData() { isDefault = true, type = 0, Name = "MMU10 - 1", Length = 0.60, Height = 0.1850, Thickness = 0.0070, Inertia = 0.0001908, Area = 0.01440, Wely = 0.001060, Wply = 0.001216 };
                SheetData sheet27 = new SheetData() { isDefault = true, type = 0, Name = "MMU11 - 1", Length = 0.60, Height = 0.1900, Thickness = 0.0075, Inertia = 0.0001606, Area = 0.012340, Wely = 0.001170, Wply = 0.001343 };
                SheetData sheet28 = new SheetData() { isDefault = true, type = 0, Name = "MMU11 - 2", Length = 0.60, Height = 0.1800, Thickness = 0.0080, Inertia = 0.0001606, Area = 0.01316, Wely = 0.001110, Wply = 0.001282 };
                SheetData sheet29 = new SheetData() { isDefault = true, type = 0, Name = "MMU12 - 1", Length = 0.60, Height = 0.1900, Thickness = 0.0085, Inertia = 0.0002294, Area = 0.014030, Wely = 0.001200, Wply = 0.001405 };
                SheetData sheet30 = new SheetData() { isDefault = true, type = 0, Name = "MMU12 - 2", Length = 0.60, Height = 0.1550, Thickness = 0.0090, Inertia = 0.0002070, Area = 0.013760, Wely = 0.001200, Wply = 0.001330 };
                SheetData sheet31 = new SheetData() { isDefault = true, type = 0, Name = "MMU14 - 1", Length = 0.75, Height = 0.2225, Thickness = 0.0080, Inertia = 0.0003137, Area = 0.013200, Wely = 0.001410, Wply = 0.001623 };
                SheetData sheet32 = new SheetData() { isDefault = true, type = 0, Name = "MMU15 - 1", Length = 0.68, Height = 0.2100, Thickness = 0.0085, Inertia = 0.0003192, Area = 0.014200, Wely = 0.001250, Wply = 0.001729 };
                SheetData sheet33 = new SheetData() { isDefault = true, type = 0, Name = "MMU16 - 1", Length = 0.75, Height = 0.2200, Thickness = 0.0090, Inertia = 0.0003531, Area = 0.014640, Wely = 0.001605, Wply = 0.001833 };
                SheetData sheet34 = new SheetData() { isDefault = true, type = 0, Name = "MMU20 - 1", Length = 0.75, Height = 0.2300, Thickness = 0.01, Inertia = 0.0004612, Area = 0.01644, Wely = 0.002005, Wply = 0.002266 };
                SheetData sheet35 = new SheetData() { isDefault = true, type = 0, Name = "MMU28 - 1", Length = 0.60, Height = 0.2400, Thickness = 0.012, Inertia = 0.0006816, Area = 0.02160, Wely = 0.002840, Wply = 0.003223 };
                SheetData sheet36 = new SheetData() { isDefault = true, type = 0, Name = "MMU32 - 1", Length = 0.60, Height = 0.2260, Thickness = 0.014, Inertia = 0.0007232, Area = 0.02440, Wely = 0.003200, Wply = 0.003509 };
                SheetData sheet37 = new SheetData() { isDefault = true, type = 0, Name = "MMU46 - 1", Length = 0.75, Height = 0.3075, Thickness = 0.016, Inertia = 0.0014283, Area = 0.03020, Wely = 0.004645, Wply = 0.005306 };

                List<SheetData> tempList = new List<SheetData>();
                tempList.Add(sheet1);
                tempList.Add(sheet2);
                tempList.Add(sheet3);
                tempList.Add(sheet4);
                tempList.Add(sheet5);
                tempList.Add(sheet6);
                tempList.Add(sheet7);
                tempList.Add(sheet8);
                tempList.Add(sheet9);
                tempList.Add(sheet10);
                tempList.Add(sheet11);
                tempList.Add(sheet12);
                tempList.Add(sheet13);
                tempList.Add(sheet14);
                tempList.Add(sheet15);
                tempList.Add(sheet16);
                tempList.Add(sheet17);
                tempList.Add(sheet18);
                tempList.Add(sheet19);
                tempList.Add(sheet20);
                tempList.Add(sheet21);
                tempList.Add(sheet22);
                tempList.Add(sheet23);
                tempList.Add(sheet24);
                tempList.Add(sheet25);
                tempList.Add(sheet26);
                tempList.Add(sheet27);
                tempList.Add(sheet28);
                tempList.Add(sheet29);
                tempList.Add(sheet30);
                tempList.Add(sheet31);
                tempList.Add(sheet32);
                tempList.Add(sheet33);
                tempList.Add(sheet34);
                tempList.Add(sheet35);
                tempList.Add(sheet36);
                tempList.Add(sheet37);


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
