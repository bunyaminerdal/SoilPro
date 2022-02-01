using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExDesign.Datas
{
    public class SoilData
    {
        public string Name { get; set; }
        public double NaturalUnitWeight { get; set; }
        public double SaturatedUnitWeight { get; set; }
        public int SoilStressStateIndex { get; set; }
        public int SoilStateKoIndex { get; set; }
        public double SoilFrictionAngle { get; set; }
        public double EffectiveCohesion { get; set; }
        public double UndrainedShearStrength { get; set; }
        public double WallSoilFrictionAngle { get; set; }
        public double WallSoilAdhesion { get; set; }
        public double PoissonRatio { get; set; }
        public double K0 { get; set; }
        public double Ocr { get; set; }
        public double OedometricModulus { get; set; }
        public double CohesionFactor { get; set; }
        public double YoungModulus { get; set; }
        public bool isDefault { get; set; }
        public int ImageIndex { get; set; }
        public Color SoilColor  { get; set; }
    }
    
}
