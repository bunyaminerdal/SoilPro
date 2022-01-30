using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExDesign.Datas
{
    public class SoilData
    {
        public string Name { get; set; }
        public double Gama { get; set; }
        public double GamaSat { get; set; }
        public double Fi { get; set; }
        public double Cprime { get; set; }
        public double Cu { get; set; }
        public double Poisson { get; set; }
        public double Eoed { get; set; }
        public double Ap { get; set; }
        public double Esprime { get; set; }


    }
    public static class Soil

    {
        public static ObservableCollection<SoilData> SoilDataList = new ObservableCollection<SoilData>();
    }
}
