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
        public SoilData Soil { get; set; }
        
    }
}
