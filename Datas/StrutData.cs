using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExDesign.Datas
{
    public class StrutData
    {
        public double StrutDepth { get; set; }
        public double StrutLength { get; set; }
        public double StrutOuterDiameter { get; set; }
        public double StrutThickness { get; set; }
        public double StrutSpacing { get; set; }
        public bool IsCentralPlacement { get; set; }
        public bool IsSoldierBeam { get; set; }
        public double SoldierBeamHeight { get; set; }
        public double SoldierBeamwidth { get; set; }
    }
}
