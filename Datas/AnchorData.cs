using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExDesign.Datas
{
    public class AnchorData
    {
        public double AnchorDepth { get; set; }
        public double FreeLength { get; set; }
        public double RootLength { get; set; }
        public double Inclination { get; set; }
        public double Spacing { get; set; }
        public double RootDiameter { get; set; }
        public int NumberofCable { get; set; }
        public WireData CableData { get; set; }
        public double TotalNominalArea { get; set; }
        public double RootModulus { get; set; }
        public bool IsPassiveAnchor { get; set; }
        public double PreStressForce { get; set; }
        public double BreakingStrength { get; set; }
        public bool IsSoldierBeam { get; set; }
        public double SoldierBeamHeight { get; set; }
        public double SoldierBeamwidth { get; set; }
    }
}
