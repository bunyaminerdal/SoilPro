using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExDesign.Datas
{
    public class SoilLayerData
    {
        public double LayerHeight { get; set; }
        public string Name { get; set; }
        
        public SoilData SoilType { get; set; }
        public double GamaSat { get; set; }
        public double Fi { get; set; }
        public double Cprime { get; set; }
        public double Cu { get; set; }
        public double Poisson { get; set; }
        public double Eoed { get; set; }
        public double Ap { get; set; }
        public double Esprime { get; set; }
        public double Gama { get; set; }
    }
}
