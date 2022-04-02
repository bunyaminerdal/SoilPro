using ExDesign.Datas;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExDesign.Scripts
{
    internal class WpfUtils
    {
        public static bool DepthAfterControl( double newDepth)
        {            
            var tupleList = new List<(double depth, int type, AnchorData? anchor, StrutData? strut)>();
            tupleList.Add((StaticVariables.viewModel.TopOfWallLevel, 0, null, null));
            foreach (var anchor in StaticVariables.viewModel.anchorDatas)
            {
                
                    tupleList.Add((anchor.AnchorDepth, 1, anchor, null));
                
            }
            foreach (var strut in StaticVariables.viewModel.strutDatas)
            {
                tupleList.Add((strut.StrutDepth, 2, null, strut));
            }
            tupleList.Add((GetExHeight(), 3, null, null));

            tupleList.Sort();
            double minDepth = double.MaxValue;
            for (int i = 1; i < tupleList.Count; i++)
            {
                if(minDepth > tupleList[i].depth) minDepth = tupleList[i].depth;
            }
            if (minDepth == double.MaxValue) return false;
            if(minDepth < newDepth) return false;
            return true;
        }

        public static bool CheckFrontLength(double newL)
        {
            if(newL > StaticVariables.viewModel.frontCubeLength)
            {
                if (newL < StaticVariables.viewModel.backCubeLength)
                {
                    StaticVariables.viewModel.frontCubeLength = StaticVariables.viewModel.backCubeLength;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckBackLength(double newL)
        {
            if (newL > StaticVariables.viewModel.backCubeLength)
            {
                if (newL < StaticVariables.viewModel.wall_h)
                {
                    StaticVariables.viewModel.backCubeLength = StaticVariables.viewModel.wall_h;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckPileS(double newS)
        {
            if(newS < StaticVariables.viewModel.GetWallThickness()* 2/3)
            {
                return false;
            }
            return true;
        }
        public static double GetExHeight()
        {
            double xheight = StaticVariables.viewModel.TopOfWallLevel;
            switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
            {
                case ExcavationType.none:
                    xheight = StaticVariables.viewModel.excavationHeight;
                    break;
                case ExcavationType.type1:
                    xheight = StaticVariables.viewModel.excavationHeight - StaticVariables.viewModel.frontT_Z;
                    break;
                case ExcavationType.type2:
                    xheight = StaticVariables.viewModel.excavationHeight + StaticVariables.viewModel.frontT_Z;
                    break;
                default:
                    break;
            }
            return xheight;
        }
        public static bool CheckExHeight(double newExH)
        {
            switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
            {
                case ExcavationType.none:
                    if(newExH>=StaticVariables.viewModel.wall_h) return false;
                    break;
                case ExcavationType.type1:
                    if (newExH  >= StaticVariables.viewModel.wall_h) return false;
                    break;
                case ExcavationType.type2:
                    if (newExH + StaticVariables.viewModel.GetexcavationZ() >= StaticVariables.viewModel.wall_h) return false;
                    break;
                default:
                    break;
            }
            return true;
        }
        public static bool CheckExZ(double newExZ)
        {
            switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
            {
                case ExcavationType.none:
                    if (StaticVariables.viewModel.GetexcavationHeight() >= StaticVariables.viewModel.GetWallHeight()) return false;
                    break;
                case ExcavationType.type1:
                    if (StaticVariables.viewModel.GetexcavationHeight() - newExZ < 0) return false;
                    break;
                case ExcavationType.type2:
                    if (newExZ + StaticVariables.viewModel.GetexcavationHeight() >= StaticVariables.viewModel.GetWallHeight()) return false;
                    break;
                default:
                    break;
            }
            return true;
        }
        public static bool CheckWallH(double newWallH)
        {
            if(newWallH < 1) return false;
            switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
            {
                case ExcavationType.none:
                    if (StaticVariables.viewModel.GetexcavationHeight() >= newWallH)
                    {
                        StaticVariables.viewModel.ChangeexcavationHeight(StaticVariables.viewModel.GetexcavationHeight()-(StaticVariables.viewModel.GetWallHeight()-newWallH));
                        return true;
                    }
                    break;
                case ExcavationType.type1:
                    if (StaticVariables.viewModel.GetexcavationHeight() >= newWallH)
                    {
                        if(StaticVariables.viewModel.GetexcavationHeight() - (StaticVariables.viewModel.GetWallHeight() - newWallH) > StaticVariables.viewModel.GetexcavationZ())
                        {
                            StaticVariables.viewModel.ChangeexcavationHeight(StaticVariables.viewModel.GetexcavationHeight() - (StaticVariables.viewModel.GetWallHeight() - newWallH));
                            return true;
                        }
                        else
                        {
                            return false;
                        }                        
                    }
                    break;
                case ExcavationType.type2:
                    if (StaticVariables.viewModel.GetexcavationHeight() + StaticVariables.viewModel.GetexcavationZ() >= newWallH)
                    {
                        if (StaticVariables.viewModel.GetexcavationHeight() - (StaticVariables.viewModel.GetWallHeight() - newWallH) > StaticVariables.viewModel.GetexcavationZ())
                        {
                            StaticVariables.viewModel.ChangeexcavationHeight(StaticVariables.viewModel.GetexcavationHeight() - (StaticVariables.viewModel.GetWallHeight() - newWallH));
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
        public static double NewAnchorStrutPlacementHeight()
        {
            double height = 0;
            double xheight = GetExHeight();
            foreach (var anchor in StaticVariables.viewModel.anchorDatas)
            {
                if(anchor.AnchorDepth>height) height = anchor.AnchorDepth;
            }
            foreach (var strut in StaticVariables.viewModel.strutDatas)
            {
                if(strut.StrutDepth > height) height = strut.StrutDepth;
            }
                        
            if(xheight > height + 2.5)
            {
                return height+2;
            }
            else if(xheight > height + 2)
            {
                return height + 1.5;
            }else if(xheight > height + 1.5)
            {
                return height + 1;
            }
            return 0;            
        }
        public static bool StrutPlacementControl(StrutData strutPlaced,double newDepth)
        {
            double bottomofplacedanchor = newDepth + strutPlaced.SoldierBeamHeight / 2;
            double topofplacedanchor = newDepth - strutPlaced.SoldierBeamHeight / 2;

            var tupleList = new List<(double depth, int type, AnchorData? anchor, StrutData? strut)>();
            tupleList.Add((StaticVariables.viewModel.TopOfWallLevel, 0, null, null));
            foreach (var anchor in StaticVariables.viewModel.anchorDatas)
            {
                
                    tupleList.Add((anchor.AnchorDepth, 1, anchor, null));
                
            }
            foreach (var strut in StaticVariables.viewModel.strutDatas)
            {
                if (strutPlaced != strut)
                {
                    tupleList.Add((strut.StrutDepth, 2, null, strut));
                }
            }
            tupleList.Add((GetExHeight(), 3, null, null));

            tupleList.Sort();

            for (int i = 0; i < tupleList.Count - 1; i++)
            {
                if (newDepth == tupleList[i].depth)
                {
                    return false;
                }
                if (newDepth > GetExHeight())
                {
                    return false;
                }
                if (newDepth < StaticVariables.viewModel.TopOfWallLevel)
                {
                    return false;
                }
                if ( newDepth > tupleList[i].depth && newDepth < tupleList[i + 1].depth)
                {
                    if (tupleList[i].type == 0)
                    {
                        Debug.WriteLine("duvardan sonra");
                        if (topofplacedanchor < StaticVariables.viewModel.TopOfWallLevel)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i].type == 1)
                    {
                        Debug.WriteLine("anchordan sonra");
                        if (topofplacedanchor < tupleList[i].depth + tupleList[i].anchor.SoldierBeamHeight / 2)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i + 1].type == 1)
                    {
                        Debug.WriteLine("anchordan önce");
                        if (bottomofplacedanchor > tupleList[i + 1].depth - tupleList[i + 1].anchor.SoldierBeamHeight / 2)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i].type == 2)
                    {
                        Debug.WriteLine("struttan sonta");
                        if (topofplacedanchor < tupleList[i].depth + tupleList[i].strut.SoldierBeamHeight / 2)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i + 1].type == 2)
                    {
                        Debug.WriteLine("struttan önce");
                        if (bottomofplacedanchor > tupleList[i + 1].depth - tupleList[i + 1].strut.SoldierBeamHeight / 2)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i + 1].type == 3)
                    {
                        Debug.WriteLine("sona geldi");
                        if (bottomofplacedanchor > GetExHeight())
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        public static bool AnchorPlacementControl(AnchorData anchorPlaced,double newDepth)
        {            
            double bottomofplacedanchor = newDepth + anchorPlaced.SoldierBeamHeight / 2;
            double topofplacedanchor = newDepth - anchorPlaced.SoldierBeamHeight / 2;

            var tupleList = new List<(double depth,int type, AnchorData? anchor,StrutData? strut)>();
            tupleList.Add((StaticVariables.viewModel.TopOfWallLevel,0, null, null));
            foreach (var anchor in StaticVariables.viewModel.anchorDatas)
            {
                if(anchorPlaced != anchor)
                {
                    tupleList.Add((anchor.AnchorDepth,1,anchor,null));
                }
            }
            foreach (var strut in StaticVariables.viewModel.strutDatas)
            {
                tupleList.Add((strut.StrutDepth,2,null,strut));
            }
            tupleList.Add((GetExHeight(), 3, null, null));

            tupleList.Sort();

            for (int i = 0; i < tupleList.Count-1; i++)
            {
                if (newDepth == tupleList[i].depth)
                {
                    return false;
                }
                if(newDepth > GetExHeight())
                {
                    return false;
                }
                if(newDepth < StaticVariables.viewModel.TopOfWallLevel)
                {
                    return false;
                }
                if (newDepth > tupleList[i].depth && newDepth < tupleList[i+1].depth)
                {
                    if (tupleList[i].type == 0)
                    {
                        Debug.WriteLine("duvardan sonra");
                        if (topofplacedanchor < StaticVariables.viewModel.TopOfWallLevel)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i].type == 1)
                    {
                        Debug.WriteLine("anchordan sonra");
                        if (topofplacedanchor < tupleList[i].depth + tupleList[i].anchor.SoldierBeamHeight / 2)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i + 1].type == 1)
                    {
                        Debug.WriteLine("anchordan önce");
                        if (bottomofplacedanchor > tupleList[i + 1].depth - tupleList[i + 1].anchor.SoldierBeamHeight / 2)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i].type == 2)
                    {
                        Debug.WriteLine("struttan sonta");
                        if (topofplacedanchor < tupleList[i].depth + tupleList[i].strut.SoldierBeamHeight / 2)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i + 1].type == 2)
                    {
                        Debug.WriteLine("struttan önce");
                        if (bottomofplacedanchor > tupleList[i + 1].depth - tupleList[i + 1].strut.SoldierBeamHeight / 2)
                        {
                            return false;
                        }
                    }
                    if (tupleList[i + 1].type == 3)
                    {
                        Debug.WriteLine("sona geldi");
                        if (bottomofplacedanchor > GetExHeight())
                        {
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }
        static double one_rad_in_degrees = (double)57.0 + ((double)17.0 / (double)60.0) + ((double)44.6 / ((double)3600.0));
        public static string ChangeDecimalOptions(double value)
        {
            string textValue = "";
            
            if (value > 1)
            {
                textValue = Math.Round(value, 4).ToString();
                
            }
            else
            {
                textValue = Math.Round(value, 10).ToString();
            }
                
            return textValue;
        }
        public static Point3D RotatePointXY(Point3D p, Point3D rotation_point, double radians)
        {
            Point3D new_point = new Point3D(rotation_point.X, rotation_point.Y, rotation_point.Z);

            try
            {
                if (radians != 0)
                {
                    double ydiff = p.Y - rotation_point.Y;
                    double xdiff = p.X - rotation_point.X;

                    double xd = (xdiff * Math.Cos(radians)) - (ydiff * Math.Sin(radians));

                    double yd = (xdiff * Math.Sin(radians)) + (ydiff * Math.Cos(radians));

                    new_point.X += xd;
                    new_point.Y += yd;
                    new_point.Z = p.Z;
                }
                else
                {
                    new_point.X = p.X;
                    new_point.Y = p.Y;
                    new_point.Z = p.Z;
                }
            }
            catch
            {

            }

            return new_point;
        }

        public static Point3D RotatePointXZ(Point3D p, Point3D rotation_point, double radians)
        {
            Point3D new_point = new Point3D(rotation_point.X, rotation_point.Y, rotation_point.Z);

            try
            {
                if (radians != 0)
                {
                    double ydiff = p.Z - rotation_point.Z;
                    double xdiff = p.X - rotation_point.X;

                    double xd = (xdiff * Math.Cos(radians)) - (ydiff * Math.Sin(radians));

                    double yd = (xdiff * Math.Sin(radians)) + (ydiff * Math.Cos(radians));

                    new_point.X += xd;
                    new_point.Z += yd;
                    new_point.Y = p.Y;
                }
                else
                {
                    new_point.X = p.X;
                    new_point.Y = p.Y;
                    new_point.Z = p.Z;
                }
            }
            catch
            {

            }

            return new_point;
        }

        public static Point3D RotatePointZY(Point3D p, Point3D rotation_point, double radians)
        {
            Point3D new_point = new Point3D(rotation_point.X, rotation_point.Y, rotation_point.Z);

            try
            {
                if (radians != 0)
                {
                    double ydiff = p.Y - rotation_point.Y;
                    double xdiff = p.Z - rotation_point.Z;

                    double xd = (xdiff * Math.Cos(radians)) - (ydiff * Math.Sin(radians));
                    double yd = (xdiff * Math.Sin(radians)) + (ydiff * Math.Cos(radians));

                    new_point.Z += xd;
                    new_point.Y += yd;
                    new_point.X = p.X;
                }
                else
                {
                    new_point.X = p.X;
                    new_point.Y = p.Y;
                    new_point.Z = p.Z;
                }
            }
            catch
            {

            }

            return new_point;
        }


        public static double radians_from_degrees(double degrees)
        {
            return degrees / one_rad_in_degrees;
        }

        public static double degrees_from_radians(double radians)
        {
            return radians * one_rad_in_degrees;
        }

        //public static Point FindIntersection(Point anchorRootStart,Point anchorRootEnd,Point layerStart,Point layerEnd)
        //{
        //    double A = layerEnd.Y - anchorRootEnd.Y;
        //    double B = anchor layerEnd.X 
        //    return new Point();
        //}
        public static double GetDimension(double value)
        {
            double newValue;
            newValue =value * StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit];
            return newValue;
        }
        public static double GetValueDimension(double value)
        {
            double newValue;
            newValue = value / StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit];
            return newValue;
        }
        public static double GetArea(double value)
        {
            double newValue;
            newValue = value * Math.Pow( StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit],2);
            return newValue;
        }
        public static double GetValueArea(double value)
        {
            double newValue;
            newValue = value / Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit],2);
            return newValue;
        }
        public static double GetVolume(double value)
        {
            double newValue;
            newValue = value * Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 3);
            return newValue;
        }
        public static double GetValueVolume(double value)
        {
            double newValue;
            newValue = value / Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 3);
            return newValue;
        }
        public static double GetInertia(double value)
        {
            double newValue;
            newValue = value * Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 4);
            return newValue;
        }
        public static double GetValueInertia(double value)
        {
            double newValue;
            newValue = value / Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 4);
            return newValue;
        }
        public static double GetForce(double value)
        {
            double newValue;
            newValue = value * StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit];
            return newValue;
        }
        public static double GetValueForce(double value)
        {
            double newValue;
            newValue = value / StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit];
            return newValue;
        }
        public static double GetMomentScaleDimension(double value)
        {
            double newValue;
            newValue = value *StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] * Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 2);
            return newValue;
        }
        public static double GetValueMomentScaleDimension(double value)
        {
            double newValue;
            newValue = value / StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] / Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 2);
            return newValue;
        }
        public static double GetMoment(double value)
        {
            double newValue;
            newValue = value * StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] * StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit];
            return newValue;
        }
        public static double GetValueMoment(double value)
        {
            double newValue;
            newValue = value / StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] / StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit];
            return newValue;
        }
        public static double GetStress(double value)
        {
            double newValue;
            newValue = value * StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] / Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 2);
            return newValue;
        }
        public static double GetValueStress(double value)
        {
            double newValue;
            newValue = value / StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] * Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 2);
            return newValue;
        }
        public static double GetSurfaceStress(double value) //kN/m
        {
            double newValue;
            newValue = value * StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] / StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit];
            return newValue;
        }
        public static double GetValueSurfaceStress(double value) //kN/m
        {
            double newValue;
            newValue = value / StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] * StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit];
            return newValue;
        }
        public static double GetDensity(double value)
        {
            double newValue;
            newValue = value * StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] / Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 3);
            return newValue;
        }
        public static double GetValueDensity(double value)
        {
            double newValue;
            newValue = value / StaticVariables.UnitForceFactors[StaticVariables.CurrentUnit] * Math.Pow(StaticVariables.UnitDimensionFactors[StaticVariables.CurrentUnit], 3);
            return newValue;
        }

        public static void OpenWindow(Window window)
        {
            window.Show();
            foreach (Window win in App.Current.Windows)
            {
                if (win != window) win.Close();
                
            }
            App.Current.MainWindow = window;

        }
        public static WallType GetWallType(int index)
        {
            switch (index)
            {
                case 0:
                    return WallType.ConcreteRectangleWall;
                case 1:
                    return WallType.ConcretePileWall;
                case 2:
                    return WallType.SteelSheetWall;
                default:
                    return WallType.ConcreteRectangleWall;
            }
        }
        public static int GetWallTypeIndex(WallType wallType)
        {
            switch (wallType)
            {
                case WallType.ConcreteRectangleWall:
                    return 0;
                case WallType.ConcretePileWall:
                    return 1;
                case WallType.SteelSheetWall:
                    return 2;
                default:
                    return 0;
            }


        }

        public static ExcavationType GetExcavationType(int index)
        {
            switch (index)
            {
                case 0:
                    return ExcavationType.none;
                case 1:
                    return ExcavationType.type1;
                case 2:
                    return ExcavationType.type2;                
                default:
                    return ExcavationType.none;
            }
        }

        public static int GetExcavationTypeIndex(ExcavationType excavationType)
        {
            switch (excavationType)
            {
                case ExcavationType.none:
                    return 0;
                case ExcavationType.type1:
                    return 1;
                case ExcavationType.type2:
                    return 2;
                default:
                    return 0;
            }
        }
        public static GroundSurfaceType GetGroundSurfaceType(int index)
        {
            switch (index)
            {
                case 0:
                    return GroundSurfaceType.flat;
                case 1:
                    return GroundSurfaceType.type1;
                case 2:
                    return GroundSurfaceType.type2;
                case 3:
                    return GroundSurfaceType.type3;
                default:
                    return GroundSurfaceType.flat;
            }
        }
        public static int GetGroundSurfaceTypeIndex(GroundSurfaceType groundSurfaceType)
        {
            switch (groundSurfaceType)
            {
                case GroundSurfaceType.flat:
                    return 0;
                case GroundSurfaceType.type1:
                    return 1;
                case GroundSurfaceType.type2:
                    return 2;
                case GroundSurfaceType.type3:
                    return 3;
                default:
                    return 0;
            }
        }
        public static GroundWaterType GetGroundWaterType(int index)
        {
            switch (index)
            {
                case 0:
                    return GroundWaterType.none;
                case 1:
                    return GroundWaterType.type1;
                case 2:
                    return GroundWaterType.type2;
                case 3:
                    return GroundWaterType.type3;
                default:
                    return GroundWaterType.none;
            }
        }
        public static int GetGroundWaterTypeIndex(GroundWaterType groundWaterType)
        {
            switch (groundWaterType)
            {
                case GroundWaterType.none:
                    return 0;
                case GroundWaterType.type1:
                    return 1;
                case GroundWaterType.type2:
                    return 2;
                case GroundWaterType.type3:
                    return 3;
                default:
                    return 0;
            }
        }
        public static SoilModelType GetSoilModelType(int index)
        {
            switch (index)
            {
                case 0:
                    return SoilModelType.Schmitt_Model;
                case 1:
                    return SoilModelType.Chadeisson_Model;
                case 2:
                    return SoilModelType.Vesic_Model;                
                default:
                    return SoilModelType.Schmitt_Model;
            }
        }
        public static int GetSoilModelIndex(SoilModelType soilModel)
        {
            switch (soilModel)
            {
                case SoilModelType.Schmitt_Model:
                    return 0;
                case SoilModelType.Chadeisson_Model:
                    return 1;
                case SoilModelType.Vesic_Model:
                    return 2;
                default:
                    return 0;
            }
        }
        public static AnalysMethod GetAnalysMethodType(int index)
        {
            switch (index)
            {
                case 0:
                    return AnalysMethod.ClassicLoading;
                case 1:
                    return AnalysMethod.EquivalentLinear;
                case 2:
                    return AnalysMethod.FHWA;
                default:
                    return AnalysMethod.ClassicLoading;
            }
        }
        public static int GetAnalysMethodIndex(AnalysMethod analysmethod)
        {
            switch (analysmethod)
            {
                case AnalysMethod.ClassicLoading:
                    return 0;
                case AnalysMethod.EquivalentLinear:
                    return 1;
                case AnalysMethod.FHWA:
                    return 2;
                default:
                    return 0;
            }
        }

        public static SoilData GetSoilData(Guid guid)
        {
            foreach (var soil in StaticVariables.viewModel.soilDatas)
            {
                if(guid == soil.ID)
                {
                    return soil;
                }
            }
            return null;
        }
        public static WireData GetWireData(string name)
        {
            foreach (var wire in Wire.WireDataList)
            {
                if (name == wire.NominalDiameter)
                {
                    return wire;
                }
            }
            return null;
        }
        public static SoilState GetSoilState(int index)
        {
            switch (index)
            {
                case 0:
                    return SoilState.Drained;
                    
                case 1:
                    return SoilState.UnDrained;
                default:
                    return SoilState.Drained;
            }
        }
        
        public static String LevelText(double level)
        {
            char separator = ',';
            char tempSeparator = ',';
            string text = "";
            if(level >= 0)
            {             
                text += "+";
            }
            level = Math.Round(level, 2);
            text+=level.ToString();
            separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (separator == ',')
            {
                tempSeparator = ',';
            }
            else
            {
                tempSeparator = '.';
            }
            string[] splittedText = text.Split(tempSeparator);
            if(splittedText.Length>1)
            {
                if(splittedText[1].Length == 1 )
                {
                    text += "0";
                }
            }
            else
            {
                text += separator.ToString() + "00";
            }

            return text;
        }
    }
}
