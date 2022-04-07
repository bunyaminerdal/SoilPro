using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExDesign.Datas;

namespace ExDesign.Scripts
{
    public static class Analysis
    {
        public static void WallPartization()
        {
            FrameData.Frames.Clear();
            double wallHeight = StaticVariables.viewModel.wall_h;

            double waterH1 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH1() : double.MaxValue;
            double waterH2 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH2() : double.MaxValue;

            double wallPartStep = wallHeight / (wallHeight * 10);
            int wallPartCount =Convert.ToInt32(Math.Round( (wallHeight/wallPartStep),0,MidpointRounding.ToNegativeInfinity));
            for (int i = 0; i < wallPartCount; i++)
            {
                FrameData frame = new FrameData(new Point(0,Math.Round( i * wallPartStep,6)),new Point(0,Math.Round((i+1)*wallPartStep,6)));
            }
            for (int i = 0;i < FrameData.Frames.Count;i++)
            {
                foreach (var anchor in StaticVariables.viewModel.anchorDatas)
                {                    
                    if (anchor.AnchorDepth > FrameData.Frames[i].StartPoint.Y && anchor.AnchorDepth < FrameData.Frames[i].EndPoint.Y)
                    {
                        FrameData frameUp = new FrameData(FrameData.Frames[i].StartPoint, new Point(0, anchor.AnchorDepth));
                        FrameData frameDown = new FrameData(new Point(0, anchor.AnchorDepth), FrameData.Frames[i].EndPoint);
                        FrameData.Frames.Remove(FrameData.Frames[i]);
                    }
                }
                foreach (var strut in StaticVariables.viewModel.strutDatas)
                {                    
                    if (strut.StrutDepth > FrameData.Frames[i].StartPoint.Y && strut.StrutDepth < FrameData.Frames[i].EndPoint.Y)
                    {
                        FrameData frameUp = new FrameData(FrameData.Frames[i].StartPoint, new Point(0, strut.StrutDepth));
                        FrameData frameDown = new FrameData(new Point(0, strut.StrutDepth), FrameData.Frames[i].EndPoint);
                        FrameData.Frames.Remove(FrameData.Frames[i]);
                    }
                }
                double _layerH = 0;
                foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                {
                    _layerH += soilLayer.LayerHeight;                    
                    if (_layerH > FrameData.Frames[i].StartPoint.Y && _layerH < FrameData.Frames[i].EndPoint.Y)
                    {
                        FrameData frameUp = new FrameData(FrameData.Frames[i].StartPoint, new Point(0, _layerH));
                        FrameData frameDown = new FrameData(new Point(0, _layerH), FrameData.Frames[i].EndPoint);
                        FrameData.Frames.Remove(FrameData.Frames[i]);
                    }
                }
                if (WpfUtils.GetExHeightForCalculation() > FrameData.Frames[i].StartPoint.Y && WpfUtils.GetExHeightForCalculation() < FrameData.Frames[i].EndPoint.Y)
                {
                    FrameData frameUp = new FrameData(FrameData.Frames[i].StartPoint, new Point(0, WpfUtils.GetExHeightForCalculation()));
                    FrameData frameDown = new FrameData(new Point(0, WpfUtils.GetExHeightForCalculation()), FrameData.Frames[i].EndPoint);
                    FrameData.Frames.Remove(FrameData.Frames[i]);
                }
                if (waterH1 < wallHeight)
                {
                    if (waterH1 > FrameData.Frames[i].StartPoint.Y && waterH1 < FrameData.Frames[i].EndPoint.Y)
                    {
                        FrameData frameUp = new FrameData(FrameData.Frames[i].StartPoint, new Point(0, waterH1));
                        FrameData frameDown = new FrameData(new Point(0, waterH1), FrameData.Frames[i].EndPoint);
                        FrameData.Frames.Remove(FrameData.Frames[i]);
                    }
                }
                if(waterH2 < wallHeight- WpfUtils.GetExHeightForCalculation())
                {
                    if (waterH2 > FrameData.Frames[i].StartPoint.Y && waterH2 < FrameData.Frames[i].EndPoint.Y)
                    {
                        FrameData frameUp = new FrameData(FrameData.Frames[i].StartPoint, new Point(0, waterH2));
                        FrameData frameDown = new FrameData(new Point(0, waterH2), FrameData.Frames[i].EndPoint);
                        FrameData.Frames.Remove(FrameData.Frames[i]);
                    }
                }
            }

            FrameData.Frames.Sort();
            FrameData.Frames.Reverse();
        }
        public static void SurchargeToFrameNodes()
        {
            double exH = WpfUtils.GetExHeightForCalculation();
            //point loaddan gelen nokta force ları
            foreach (var pointLoad in StaticVariables.viewModel.PointLoadDatas)
            {
                double mValue = pointLoad.DistanceFromWall / exH;
                foreach (var frame in FrameData.Frames)
                {                    
                    double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                    double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                    double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));
                    
                    double startN = startLength/ exH;
                    double endN = endLength / exH;
                    
                    double startLoad = 0;
                    double endLoad = 0;
                    if(mValue > 0.4)
                    {
                        startLoad = (1.77 * pointLoad.Load / Math.Pow(exH, 2)) * (Math.Pow(startN, 2)* Math.Pow(mValue,2) / Math.Pow(Math.Pow(startN, 2) + Math.Pow(mValue,2), 3));
                        endLoad = (1.77 * pointLoad.Load / Math.Pow(exH, 2)) * (Math.Pow(endN, 2)* Math.Pow(mValue,2) / Math.Pow(Math.Pow(endN, 2) + Math.Pow(mValue,2), 3));
                    }
                    else
                    {
                        startLoad = (0.28*pointLoad.Load/Math.Pow(exH, 2))*(Math.Pow(startN,2)/Math.Pow(Math.Pow(startN,2)+0.16,3));
                        endLoad = (0.28*pointLoad.Load/Math.Pow(exH, 2))*(Math.Pow(endN,2)/Math.Pow(Math.Pow(endN,2)+0.16,3));
                    }
                    double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                    frame.startNodeLoadAndForce.Add( new Tuple<Load,double, double>(pointLoad, startLoad, startNodeForce));
                    double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                    frame.endNodeLoadAndForce.Add( new Tuple<Load,double, double>(pointLoad, endLoad, endNodeForce));
                }
            }            
            //Line loaddan gelen nokta forceları
            foreach (var lineLoad in StaticVariables.viewModel.LineLoadDatas)
            {
                double mValue = lineLoad.DistanceFromWall / exH;
                foreach (var frame in FrameData.Frames)
                {
                    double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                    double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                    double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                    double startN = startLength / exH;
                    double endN = endLength / exH;

                    double startLoad = 0;
                    double endLoad = 0;
                    if (mValue > 0.4)
                    {
                        startLoad = (1.28 * lineLoad.Load / exH) * (startN*Math.Pow(mValue,2) / Math.Pow(Math.Pow(startN, 2) + Math.Pow(mValue, 2), 2));
                        endLoad = (1.28 * lineLoad.Load / exH) * (endN*Math.Pow(mValue, 2) / Math.Pow(Math.Pow(endN, 2) + Math.Pow(mValue, 2), 2));
                    }
                    else
                    {
                        startLoad = (0.203 * lineLoad.Load / exH )* (startN / Math.Pow(Math.Pow(startN, 2) + 0.16, 2));
                        endLoad = (0.203 * lineLoad.Load / exH )* (endN / Math.Pow(Math.Pow(endN, 2) + 0.16, 2));
                    }
                    double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                    frame.startNodeLoadAndForce.Add( new Tuple<Load,double, double>(lineLoad,startLoad, startNodeForce));
                    double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                    frame.endNodeLoadAndForce.Add( new Tuple<Load,double, double>(lineLoad,endLoad, endNodeForce));
                }
            }
            //strip loaddan gelen nokta forceları
            foreach (var stripLoad in StaticVariables.viewModel.stripLoadDatas)
            {
                double startLoc = stripLoad.DistanceFromWall;
                double endLoc = stripLoad.DistanceFromWall+stripLoad.StripLength;
                double midLoc =startLoc + (endLoc-startLoc)/2;
                double fi = 0 ;
                double Ka = 0 ;
                if(StaticVariables.viewModel.soilLayerDatas.Count > 0)
                {
                    if(StaticVariables.viewModel.soilLayerDatas[0].Soil != null)
                    {
                        fi = StaticVariables.viewModel.soilLayerDatas[0].Soil.SoilFrictionAngle;
                        
                    }
                }
                Ka = Math.Pow(Math.Tan((45-fi/2)*Math.PI/180), 2);
                
                foreach (var frame in FrameData.Frames)
                {
                    double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                    double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                    double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));
                    
                    double alfaStart = Math.Atan(midLoc / startLength);
                    double alfaEnd = Math.Atan(midLoc / endLength);
                    double betaStart = Math.Atan(endLoc/startLength)-Math.Atan(startLoc/startLength);
                    double betaEnd = Math.Atan(endLoc/endLength)-Math.Atan(startLoc/endLength);
                    if (startLength == 0)
                    {
                        alfaStart = 0;
                        betaStart = 0;
                    }
                    double startLoad = 0;
                    double endLoad = 0;
                    
                    startLoad = (2 * stripLoad.StartLoad / exH) * (betaStart - (Math.Sin(betaStart) * Math.Cos(2 * alfaStart)));
                    endLoad = (2 * stripLoad.StartLoad / exH) * (betaEnd - (Math.Sin(betaEnd) * Math.Cos(2 * alfaEnd)));
                    if (Ka * stripLoad.StartLoad < startLoad)
                    {
                        startLoad = Ka * stripLoad.StartLoad;
                    }
                    if(Ka * stripLoad.StartLoad < endLoad)
                    {
                        endLoad = Ka * stripLoad.StartLoad;
                        if (startLength == 0)
                        {
                            startLoad = endLoad;
                        }
                    }
                    
                    double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                    frame.startNodeLoadAndForce.Add( new Tuple<Load,double, double>(stripLoad,startLoad, startNodeForce));
                    double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                    frame.endNodeLoadAndForce.Add( new Tuple<Load,double, double>(stripLoad,endLoad, endNodeForce));
                }
            }
        }
        public static void HydroStaticWaterPressureToFrameNodes()
        {
            double exH = WpfUtils.GetExHeightForCalculation();
            double exH_waterType3 = StaticVariables.viewModel.GetexcavationHeight();
            double _waterDensity = StaticVariables.waterDensity;
            double waterH1 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH1() : double.MaxValue;
            double waterH2 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH2() : double.MaxValue;

            switch (WpfUtils.GetGroundWaterType(StaticVariables.viewModel.WaterTypeIndex))
            {
                case GroundWaterType.none:
                    break;
                case GroundWaterType.type1:
                    // water load type1
                    foreach (var frame in FrameData.Frames)
                    {
                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                        double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                        double startLoad = 0;
                        double endLoad = 0;
                        if (waterH1 < startLength)
                        {
                            startLoad = (startLength - waterH1) * _waterDensity;
                        }
                        if (waterH1 < endLength)
                        {
                            endLoad = (endLength - waterH1) * _waterDensity;
                        }                        
                        WaterLoadData waterLoadData = new WaterLoadData() { Type = LoadType.HydroStaticWaterPressure };
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add( new Tuple<Load,double, double>(waterLoadData,startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add( new Tuple<Load,double, double>(waterLoadData,endLoad, endNodeForce));
                    }
                    break;
                case GroundWaterType.type2:
                    // water load type2
                    foreach (var frame in FrameData.Frames)
                    {
                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                        double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));
                        double startfrontLength = 0;
                        double endfrontLength = 0;
                        if (startLength > exH_waterType3 + waterH2)
                        {
                            startfrontLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(exH_waterType3 + waterH2 - frame.StartPoint.Y, 2)));
                        }
                        if(endLength > exH_waterType3 + waterH2)
                        {
                            endfrontLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(exH_waterType3 + waterH2 - frame.EndPoint.Y, 2)));
                        }
                        double startLoad = 0;
                        double endLoad = 0;
                        if (waterH1 < startLength)
                        {
                            startLoad = (startLength - waterH1 - startfrontLength) * _waterDensity;
                        }
                        if (waterH1 < endLength)
                        {
                            endLoad = (endLength - waterH1 - endfrontLength) * _waterDensity;
                        }
                        WaterLoadData waterLoadData = new WaterLoadData() { Type = LoadType.HydroStaticWaterPressure };
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add( new Tuple<Load,double, double>(waterLoadData,startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add( new Tuple<Load,double, double>(waterLoadData,endLoad, endNodeForce));
                    }
                    break;
                case GroundWaterType.type3:
                    // water load type2
                    double scaleWaterLoad = (exH_waterType3 + waterH2 - waterH1) / (StaticVariables.viewModel.wall_h - exH_waterType3 - waterH2);
                    foreach (var frame in FrameData.Frames)
                    {
                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                        double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));
                        double startfrontLength = 0;
                        double endfrontLength = 0;

                        if (startLength > exH_waterType3 + waterH2)
                        {
                            startfrontLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(exH_waterType3 + waterH2 - frame.StartPoint.Y, 2)));
                        }
                        if(endLength > exH_waterType3 + waterH2)
                        {
                            endfrontLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(exH_waterType3 + waterH2 - frame.EndPoint.Y, 2)));
                        }

                        double startLoad = 0;
                        double endLoad = 0;
                        if (waterH1 < startLength)
                        {
                            startLoad = (startLength - waterH1 - startfrontLength) * _waterDensity - (startfrontLength * _waterDensity * scaleWaterLoad);

                        }
                        if (waterH1 < endLength)
                        {
                            endLoad = (endLength - waterH1 - endfrontLength) * _waterDensity - (endfrontLength * _waterDensity * scaleWaterLoad);
                        }
                        WaterLoadData waterLoadData = new WaterLoadData() { Type = LoadType.HydroStaticWaterPressure };
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add( new Tuple<Load,double, double>(waterLoadData,startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add( new Tuple<Load,double, double>(waterLoadData,endLoad, endNodeForce));
                        
                    }
                    break;
                default:
                    break;
            }
        }
        public static void BackEffectiveStressToFrameNodes()
        {
            double wallH = StaticVariables.viewModel.GetWallHeight();
            double _waterDensity = StaticVariables.waterDensity;
            double waterH1 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH1() : double.MaxValue;

            double startLoad = 0;
            double endLoad = 0;
            foreach (var frame in FrameData.Frames)
            {
                double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));
                                
                double soilLayerHeight = 0;
                SoilData lastSoil = null;
                foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                {
                    soilLayerHeight += soilLayer.LayerHeight;
                    if(startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight < startLength)
                    {
                        if (soilLayer.Soil != null)
                        {
                            
                            if (startLength <= waterH1)
                            {
                                startLoad += (frameLength*soilLayer.Soil.NaturalUnitWeight);
                            }
                            else
                            {
                                startLoad += (frameLength*(soilLayer.Soil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                    if (endLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight < endLength)
                    {
                        if (soilLayer.Soil != null)
                        {
                            if (endLength <= waterH1)
                            {
                                endLoad += (frameLength * soilLayer.Soil.NaturalUnitWeight);
                            }
                            else
                            {
                                endLoad +=  (frameLength * (soilLayer.Soil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                    if (soilLayer.Soil != null)
                    {
                        lastSoil = soilLayer.Soil;
                    }
                }

                //duvardan küçükse
                if(soilLayerHeight < wallH)
                {
                    if (startLength <= wallH && soilLayerHeight < startLength)
                    {
                        if (lastSoil != null)
                        {
                            if (startLength <= waterH1)
                            {
                                startLoad += (frameLength * lastSoil.NaturalUnitWeight);
                            }
                            else
                            {
                                startLoad += (frameLength * (lastSoil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                    if (endLength <= wallH && soilLayerHeight < endLength)
                    {
                        if (lastSoil != null)
                        {

                            if (endLength <= waterH1)
                            {
                                endLoad += (frameLength * lastSoil.NaturalUnitWeight);
                            }
                            else
                            {
                                endLoad += (frameLength * (lastSoil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                }
                
                EffectiveStress effectiveStress = new EffectiveStress() { Type = LoadType.Back_EffectiveStress };
                double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load,double,double>(effectiveStress,startLoad,startNodeForce));
                double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add( new Tuple<Load,double, double>(effectiveStress,endLoad, endNodeForce));
            }
        }
        public static void FrontEffectiveStressToFrameNodes()
        {
            double exH = WpfUtils.GetExHeightForCalculation();
            double wallH = StaticVariables.viewModel.GetWallHeight();
            double _waterDensity = StaticVariables.waterDensity;
            double waterH2 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH2() : double.MaxValue;

            double startLoad = 0;
            double endLoad = 0;
            foreach (var frame in FrameData.Frames)
            {
                double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                double soilLayerHeight = 0;
                SoilData lastSoil = null;
                foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                {
                    soilLayerHeight += soilLayer.LayerHeight;
                    if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight < startLength && exH < startLength)
                    {
                        if (soilLayer.Soil != null)
                        {

                            if (startLength <= waterH2)
                            {
                                startLoad += (frameLength * soilLayer.Soil.NaturalUnitWeight);
                            }
                            else
                            {
                                startLoad += (frameLength * (soilLayer.Soil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                    if (endLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight < endLength && exH < endLength)
                    {
                        if (soilLayer.Soil != null)
                        {
                            if (endLength <= waterH2)
                            {
                                endLoad += (frameLength * soilLayer.Soil.NaturalUnitWeight);
                            }
                            else
                            {
                                endLoad += (frameLength * (soilLayer.Soil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                    if (soilLayer.Soil != null)
                    {
                        lastSoil = soilLayer.Soil;
                    }
                }

                //duvardan küçükse
                if (soilLayerHeight < wallH)
                {
                    if (startLength <= wallH && soilLayerHeight < startLength && exH < startLength)
                    {
                        if (lastSoil != null)
                        {
                            if (startLength <= waterH2)
                            {
                                startLoad += (frameLength * lastSoil.NaturalUnitWeight);
                            }
                            else
                            {
                                startLoad += (frameLength * (lastSoil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                    if (endLength <= wallH && soilLayerHeight < endLength && exH < endLength)
                    {
                        if (lastSoil != null)
                        {

                            if (endLength <= waterH2)
                            {
                                endLoad += (frameLength * lastSoil.NaturalUnitWeight);
                            }
                            else
                            {
                                endLoad += (frameLength * (lastSoil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                }

                EffectiveStress effectiveStress = new EffectiveStress() { Type = LoadType.Front_EffectiveStress };
                double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(effectiveStress, startLoad, startNodeForce));
                double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(effectiveStress, endLoad, endNodeForce));
            }
        }
        public static void SubgradeModulusofSoilToFrameNodes()
        {
            double wallH = StaticVariables.viewModel.GetWallHeight();
            double wallt = StaticVariables.viewModel.GetWallThickness();
            double wallEI = StaticVariables.viewModel.GetWallEI();
            double wallE = StaticVariables.viewModel.GetWallE();
            double EoedPow = 4.0 / 3.0;
            double EIPow = 1.0 / 3.0;
            double Pow3 = 1.0 / 5.0;
            double Pow4 = 1.0 / 12.0;
            switch (WpfUtils.GetSoilModelType(StaticVariables.viewModel.SoilModelIndex))
            {
                case SoilModelType.Schmitt_Model:
                    
                    //Kh for schmitt model
                    foreach (var frame in FrameData.Frames)
                    {
                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));

                        double startLoad = 0;
                        double endLoad = 0;
                        double soilLayerHeight = 0;
                        SoilData lastSoil = null;
                        foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                        {
                            soilLayerHeight += soilLayer.LayerHeight;
                            if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength)
                            {
                                if (soilLayer.Soil != null)
                                {
                                    startLoad = 2.1 * ((Math.Pow(soilLayer.Soil.OedometricModulus, EoedPow) / Math.Pow(wallEI,EIPow)));
                                    endLoad = 2.1 * (Math.Pow(soilLayer.Soil.OedometricModulus, EoedPow) / Math.Pow(wallEI, EIPow));

                                }
                            }
                            
                            if (soilLayer.Soil != null)
                            {
                                lastSoil = soilLayer.Soil;
                            }
                        }

                        //duvardan küçükse
                        if (soilLayerHeight < wallH)
                        {
                            if (startLength <= wallH && soilLayerHeight < startLength)
                            {
                                if (lastSoil != null)
                                {
                                    startLoad = 2.1 * (Math.Pow(lastSoil.OedometricModulus, EoedPow) / Math.Pow(wallEI, EIPow));
                                    endLoad = 2.1 * (Math.Pow(lastSoil.OedometricModulus, EoedPow) / Math.Pow(wallEI, EIPow));

                                }
                            }
                            
                        }

                        SubgradeModulusofSoil soilSpringCoef = new SubgradeModulusofSoil() { Type =LoadType.SubgradeModulusofSoil };
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(soilSpringCoef, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(soilSpringCoef, endLoad, endNodeForce));
                    }

                    break;
                case SoilModelType.Chadeisson_Model:

                    //Kh for Chadeisson model
                    foreach (var frame in FrameData.Frames)
                    {

                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));

                        double startLoad = 0;
                        double endLoad = 0;
                        double soilLayerHeight = 0;
                        SoilData lastSoil = null;
                        foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                        {
                            soilLayerHeight += soilLayer.LayerHeight;
                            if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength)
                            {
                                if (soilLayer.Soil != null)
                                {
                                    double fi = soilLayer.Soil.SoilFrictionAngle * Math.PI / 180;
                                    double Delta = soilLayer.Soil.WallSoilFrictionAngle * Math.PI / 180;
                                    double CPrime = soilLayer.Soil.EffectiveCohesion;
                                    double Ap = soilLayer.Soil.CohesionFactor;
                                    double K0 = soilLayer.Soil.K0;
                                    double Gama = soilLayer.Soil.NaturalUnitWeight;
                                    double alfa = 0.0 * Math.PI / 180;
                                    double beta = 0.0 * Math.PI / 180;
                                    double Kp =Math.Pow( Math.Cos(fi + alfa),2)/(
                                        Math.Pow( Math.Cos(alfa),2) * Math.Cos(Delta-alfa)*
                                        Math.Pow( 1 - Math.Sqrt(Math.Sin(fi+Delta)*Math.Sin(fi+beta)/
                                        (Math.Cos(Delta-alfa)*Math.Cos(beta-alfa))),2)
                                        ) ;
                                    startLoad = Math.Pow(20 * wallEI * (Math.Pow(Kp * Gama * (1 - (K0 / Kp)) / 0.015, 4)), Pow3) + (Ap * CPrime * Math.Tanh(CPrime / 30) / 0.015);
                                    endLoad = Math.Pow(20 * wallEI * (Math.Pow(Kp * Gama * (1 - (K0 / Kp)) / 0.015, 4)), Pow3) + (Ap * CPrime * Math.Tanh(CPrime / 30) / 0.015);

                                }
                            }

                            if (soilLayer.Soil != null)
                            {
                                lastSoil = soilLayer.Soil;
                            }
                        }

                        //duvardan küçükse
                        if (soilLayerHeight < wallH)
                        {
                            if (startLength <= wallH && soilLayerHeight < startLength)
                            {
                                if (lastSoil != null)
                                {
                                    double fi = lastSoil.SoilFrictionAngle * Math.PI / 180;
                                    double Delta = lastSoil.WallSoilFrictionAngle * Math.PI / 180;
                                    double CPrime = lastSoil.EffectiveCohesion;
                                    double Ap = lastSoil.CohesionFactor;
                                    double K0 = lastSoil.K0;
                                    double Gama = lastSoil.NaturalUnitWeight;
                                    double alfa = 0.0 * Math.PI / 180;
                                    double beta = 0.0 * Math.PI / 180;
                                    double Kp = Math.Pow(Math.Cos(fi + alfa), 2) / (
                                        Math.Pow(Math.Cos(alfa), 2) * Math.Cos(Delta - alfa) *
                                        Math.Pow(1 - Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi + beta) /
                                        (Math.Cos(Delta - alfa) * Math.Cos(beta - alfa))), 2)
                                        );
                                    startLoad = Math.Pow(20 * wallEI * (Math.Pow(Kp * Gama * (1 - (K0 / Kp)) / 0.015, 4)), Pow3) + (Ap * CPrime * Math.Tanh(CPrime / 30) / 0.015);
                                    endLoad = Math.Pow(20 * wallEI * (Math.Pow(Kp * Gama * (1 - (K0 / Kp)) / 0.015, 4)), Pow3) + (Ap * CPrime * Math.Tanh(CPrime / 30) / 0.015);

                                }
                            }

                        }

                        SubgradeModulusofSoil soilSpringCoef = new SubgradeModulusofSoil() { Type = LoadType.SubgradeModulusofSoil };
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(soilSpringCoef, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(soilSpringCoef, endLoad, endNodeForce));
                    }
                    break;
                case SoilModelType.Vesic_Model:
                    //Kh for vesic model
                    foreach (var frame in FrameData.Frames)
                    {

                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));

                        double startLoad = 0;
                        double endLoad = 0;
                        double soilLayerHeight = 0;
                        SoilData lastSoil = null;
                        foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                        {
                            soilLayerHeight += soilLayer.LayerHeight;
                            if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength)
                            {
                                if (soilLayer.Soil != null)
                                {
                                    double Es = soilLayer.Soil.YoungModulus;
                                    double poisson = soilLayer.Soil.PoissonRatio ;
                                    startLoad = (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4.0) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));
                                    endLoad = (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4.0) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));
                                }
                            }

                            if (soilLayer.Soil != null)
                            {
                                lastSoil = soilLayer.Soil;
                            }
                        }

                        //duvardan küçükse
                        if (soilLayerHeight < wallH)
                        {
                            if (startLength <= wallH && soilLayerHeight < startLength)
                            {
                                if (lastSoil != null)
                                {
                                    double Es = lastSoil.YoungModulus;
                                    double poisson = lastSoil.PoissonRatio;
                                    startLoad = (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));
                                    endLoad = (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));

                                }
                            }
                        }

                        SubgradeModulusofSoil soilSpringCoef = new SubgradeModulusofSoil() { Type = LoadType.SubgradeModulusofSoil };
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(soilSpringCoef, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(soilSpringCoef, endLoad, endNodeForce));
                    }
                    break;
                default:
                    break;
            }
            
        }
        public static void BackActivePassiveCoefToFrameNodes()
        {
            double wallH = StaticVariables.viewModel.GetWallHeight();
            double waterH1 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH1() : double.MaxValue;
            double ksi = 90 * Math.PI / 180;
            double gamaw = StaticVariables.waterDensity;
            double beta_back = WpfUtils.GetGroundSurfaceType( StaticVariables.viewModel.GroundSurfaceTypeIndex) == GroundSurfaceType.type1? StaticVariables.viewModel.backT_Beta * Math.PI / 180 : 0;

            foreach (var frame in FrameData.Frames)
            {
                double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));

                double Ka_P = 0;
                double Ka_N = 0;
                double Ka_S = 0;
                double Kp_P = 0;
                double Kp_N = 0;
                double Kp_S = 0;
                double soilLayerHeight = 0;
                SoilData lastSoil = null;
                //ka
                foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                {
                    soilLayerHeight += soilLayer.LayerHeight;
                    if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength)
                    {

                        Back_TBDY_Theory_Active(waterH1, ksi, gamaw, beta_back, startLength, ref Ka_P, ref Ka_N, ref Ka_S, soilLayer.Soil);
                        Back_TBDY_Theory_Passive(waterH1, ksi, gamaw, beta_back, startLength, ref Kp_P, ref Kp_N, ref Kp_S, soilLayer.Soil);

                    }

                    if (soilLayer.Soil != null)
                    {
                        lastSoil = soilLayer.Soil;
                    }
                }

                //duvardan küçükse
                if (soilLayerHeight < wallH)
                {
                    if (startLength <= wallH && soilLayerHeight < startLength)
                    {
                        Back_TBDY_Theory_Active(waterH1, ksi, gamaw, beta_back, startLength, ref Ka_P, ref Ka_N, ref Ka_S, lastSoil);
                        Back_TBDY_Theory_Passive(waterH1, ksi, gamaw, beta_back, startLength, ref Kp_P, ref Kp_N, ref Kp_S, lastSoil);

                    }
                }

                Ka_Kp Back_Kactive = new Ka_Kp() { Type = LoadType.Back_Kactive };                
                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double,double>(Back_Kactive, Ka_S, Ka_P,Ka_N));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double,double>(Back_Kactive, Ka_S, Ka_P, Ka_N));

                Ka_Kp Back_Kpassive = new Ka_Kp() { Type = LoadType.Back_Kpassive };
                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Back_Kpassive, Kp_S, Kp_P, Kp_N));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Back_Kpassive, Kp_S, Kp_P, Kp_N));
            }
        }


        public static void FrontActivePassiveCoefToFrameNodes()
        {
            double exH = WpfUtils.GetExHeightForCalculation();
            double wallH = StaticVariables.viewModel.GetWallHeight();
            double waterH2 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH2() : double.MaxValue;
            double ksi = 90 * Math.PI / 180;
            double gamaw = StaticVariables.waterDensity;
            double beta_front = 0 * Math.PI / 180;

            foreach (var frame in FrameData.Frames)
            {
                double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));

                double Ka_P = 0;
                double Ka_N = 0;
                double Ka_S = 0;
                double Kp_P = 0;
                double Kp_N = 0;
                double Kp_S = 0;
                double soilLayerHeight = 0;
                SoilData lastSoil = null;
                //ka
                foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                {
                    soilLayerHeight += soilLayer.LayerHeight;
                    if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength && exH <= startLength)
                    {
                        if (soilLayer.Soil != null)
                        {
                            double fi = soilLayer.Soil.SoilFrictionAngle * Math.PI / 180;
                            double Delta = soilLayer.Soil.WallSoilFrictionAngle * Math.PI / 180;
                            double tetaS = 0 * Math.PI / 180;
                            double tetaP = 0 * Math.PI / 180;
                            double tetaN = 0 * Math.PI / 180;
                            double gama = soilLayer.Soil.NaturalUnitWeight;
                            double gamad = soilLayer.Soil.SaturatedUnitWeight;

                            if (beta_front > fi - tetaS)
                            {
                                Ka_S = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaS - Delta));
                            }
                            else
                            {
                                Ka_S = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) /
                                    (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) *
                                    Math.Sin(ksi - tetaS - Delta) *
                                    Math.Pow(1 + Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi - beta_front - tetaS) / (Math.Sin(ksi - tetaS - Delta) * Math.Sin(ksi + beta_front))), 2.0));
                            }
                            Kp_S = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaS) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_front - tetaS) / (Math.Sin(ksi + tetaS) * Math.Sin(ksi + beta_front))), 2));

                            if (StaticVariables.viewModel.isEarthQuakeDesign) //dynamic ka
                            {
                                double khValue = StaticVariables.viewModel.khValue;
                                double kvValue = StaticVariables.viewModel.kvValue;

                                if (startLength < waterH2) //su olmayan kısım
                                {
                                    tetaN = Math.Atan(khValue / (1.0 - kvValue));

                                    tetaP = Math.Atan(khValue / (1.0 + kvValue));

                                }
                                else //su olan kısım
                                {
                                    if (WpfUtils.GetSoilState(soilLayer.Soil.SoilStressStateIndex) == SoilState.Drained) //su olan kısım drained
                                    {
                                        tetaN = Math.Atan((gamad / (gamad - gamaw)) * (khValue / (1.0 - kvValue)));

                                        tetaP = Math.Atan((gamad / (gamad - gamaw)) * (khValue / (1.0 + kvValue)));

                                    }
                                    else //su olan kısım undrained
                                    {
                                        tetaN = Math.Atan((gama / (gamad - gamaw)) * (khValue / (1.0 - kvValue)));

                                        tetaP = Math.Atan((gama / (gamad - gamaw)) * (khValue / (1.0 + kvValue)));

                                    }
                                }
                                if (beta_front > fi - tetaN)
                                {
                                    Ka_N = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaN - Delta));
                                }
                                else
                                {
                                    Ka_N = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) /
                                        (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                                        Math.Sin(ksi - tetaN - Delta) *
                                        Math.Pow(1 + Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi - beta_front - tetaN) / (Math.Sin(ksi - tetaN - Delta) * Math.Sin(ksi + beta_front))), 2.0));
                                }
                                Kp_N = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                                    Math.Sin(ksi + tetaN) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_front - tetaN) / (Math.Sin(ksi + tetaN) * Math.Sin(ksi + beta_front))), 2));
                                if (beta_front > fi - tetaP)
                                {
                                    Ka_P = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaP - Delta));
                                }
                                else
                                {
                                    Ka_P = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) /
                                        (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) *
                                        Math.Sin(ksi - tetaP - Delta) *
                                        Math.Pow(1 + Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi - beta_front - tetaP) / (Math.Sin(ksi - tetaP - Delta) * Math.Sin(ksi + beta_front))), 2.0));
                                }
                                Kp_P = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaP) *
                                    Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_front - tetaP) / (Math.Sin(ksi + tetaP) * Math.Sin(ksi + beta_front))), 2));

                            }

                        }
                    }

                    if (soilLayer.Soil != null)
                    {
                        lastSoil = soilLayer.Soil;
                    }
                }

                //duvardan küçükse
                if (soilLayerHeight < wallH)
                {
                    if (startLength <= wallH && soilLayerHeight < startLength && exH <= startLength)
                    {
                        if (lastSoil != null)
                        {

                            double fi = lastSoil.SoilFrictionAngle * Math.PI / 180;
                            double Delta = lastSoil.WallSoilFrictionAngle * Math.PI / 180;
                            double tetaS = 0 * Math.PI / 180;
                            double tetaP = 0 * Math.PI / 180;
                            double tetaN = 0 * Math.PI / 180;
                            double gama = lastSoil.NaturalUnitWeight;
                            double gamad = lastSoil.SaturatedUnitWeight;

                            if (beta_front > fi - tetaS)
                            {
                                Ka_S = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaS - Delta));
                            }
                            else
                            {
                                Ka_S = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) /
                                    (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) *
                                    Math.Sin(ksi - tetaS - Delta) *
                                    Math.Pow(1 + Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi - beta_front - tetaS) / (Math.Sin(ksi - tetaS - Delta) * Math.Sin(ksi + beta_front))), 2.0));
                            }
                            Kp_S = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaS) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_front - tetaS) / (Math.Sin(ksi + tetaS) * Math.Sin(ksi + beta_front))), 2));

                            if (StaticVariables.viewModel.isEarthQuakeDesign) //dynamic ka
                            {
                                double khValue = StaticVariables.viewModel.khValue;
                                double kvValue = StaticVariables.viewModel.kvValue;

                                if (startLength < waterH2) //su olmayan kısım
                                {
                                    tetaN = Math.Atan(khValue / (1.0 - kvValue));

                                    tetaP = Math.Atan(khValue / (1.0 + kvValue));

                                }
                                else //su olan kısım
                                {
                                    if (WpfUtils.GetSoilState(lastSoil.SoilStressStateIndex) == SoilState.Drained) //su olan kısım drained
                                    {
                                        tetaN = Math.Atan((gamad / (gamad - gamaw)) * (khValue / (1.0 - kvValue)));

                                        tetaP = Math.Atan((gamad / (gamad - gamaw)) * (khValue / (1.0 + kvValue)));

                                    }
                                    else //su olan kısım undrained
                                    {
                                        tetaN = Math.Atan((gama / (gamad - gamaw)) * (khValue / (1.0 - kvValue)));

                                        tetaP = Math.Atan((gama / (gamad - gamaw)) * (khValue / (1.0 + kvValue)));

                                    }
                                }
                                if (beta_front > fi - tetaN)
                                {
                                    Ka_N = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaN - Delta));
                                }
                                else
                                {
                                    Ka_N = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) /
                                        (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                                        Math.Sin(ksi - tetaN - Delta) *
                                        Math.Pow(1 + Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi - beta_front - tetaN) / (Math.Sin(ksi - tetaN - Delta) * Math.Sin(ksi + beta_front))), 2.0));
                                }
                                Kp_N = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                                    Math.Sin(ksi + tetaN) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_front - tetaN) / (Math.Sin(ksi + tetaN) * Math.Sin(ksi + beta_front))), 2));
                                if (beta_front > fi - tetaP)
                                {
                                    Ka_P = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaP - Delta));
                                }
                                else
                                {
                                    Ka_P = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) /
                                        (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) *
                                        Math.Sin(ksi - tetaP - Delta) *
                                        Math.Pow(1 + Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi - beta_front - tetaP) / (Math.Sin(ksi - tetaP - Delta) * Math.Sin(ksi + beta_front))), 2.0));
                                }
                                Kp_P = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaP) *
                                    Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_front - tetaP) / (Math.Sin(ksi + tetaP) * Math.Sin(ksi + beta_front))), 2));

                            }

                        }
                    }

                }

                Ka_Kp Front_Kactive = new Ka_Kp() { Type = LoadType.Front_Kactive };
                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Kactive, Ka_S, Ka_P, Ka_N));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Kactive, Ka_S, Ka_P, Ka_N));

                Ka_Kp Front_Kpassive = new Ka_Kp() { Type = LoadType.Front_Kpassive };
                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Kpassive, Kp_S, Kp_P, Kp_N));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Kpassive, Kp_S, Kp_P, Kp_N));

            }
        }


        private static void Back_TBDY_Theory_Active(double waterH1, double ksi, double gamaw, double beta_back, double startLength, ref double Ka_P, ref double Ka_N, ref double Ka_S, SoilData soil)
        {
            if (soil != null)
            {

                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double Delta = soil.WallSoilFrictionAngle * Math.PI / 180;
                double tetaS = 0 * Math.PI / 180;
                double tetaP = 0 * Math.PI / 180;
                double tetaN = 0 * Math.PI / 180;
                double gama = soil.NaturalUnitWeight;
                double gamad = soil.SaturatedUnitWeight;

                if (beta_back > fi - tetaS)
                {
                    Ka_S = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaS - Delta));
                }
                else
                {
                    Ka_S = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) /
                        (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) *
                        Math.Sin(ksi - tetaS - Delta) *
                        Math.Pow(1 + Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi - beta_back - tetaS) / (Math.Sin(ksi - tetaS - Delta) * Math.Sin(ksi + beta_back))), 2.0));
                }

                if (StaticVariables.viewModel.isEarthQuakeDesign) //dynamic ka
                {
                    double khValue = StaticVariables.viewModel.khValue;
                    double kvValue = StaticVariables.viewModel.kvValue;

                    if (startLength < waterH1) //su olmayan kısım
                    {
                        tetaN = Math.Atan(khValue / (1.0 - kvValue));

                        tetaP = Math.Atan(khValue / (1.0 + kvValue));

                    }
                    else //su olan kısım
                    {
                        if (WpfUtils.GetSoilState(soil.SoilStressStateIndex) == SoilState.Drained) //su olan kısım drained
                        {
                            tetaN = Math.Atan((gamad / (gamad - gamaw)) * (khValue / (1.0 - kvValue)));

                            tetaP = Math.Atan((gamad / (gamad - gamaw)) * (khValue / (1.0 + kvValue)));

                        }
                        else //su olan kısım undrained
                        {
                            tetaN = Math.Atan((gama / (gamad - gamaw)) * (khValue / (1.0 - kvValue)));

                            tetaP = Math.Atan((gama / (gamad - gamaw)) * (khValue / (1.0 + kvValue)));

                        }
                    }
                    if (beta_back > fi - tetaN)
                    {
                        Ka_N = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaN - Delta));
                    }
                    else
                    {
                        Ka_N = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) /
                            (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                            Math.Sin(ksi - tetaN - Delta) *
                            Math.Pow(1 + Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi - beta_back - tetaN) / (Math.Sin(ksi - tetaN - Delta) * Math.Sin(ksi + beta_back))), 2.0));
                    }
                    if (beta_back > fi - tetaP)
                    {
                        Ka_P = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaP - Delta));
                    }
                    else
                    {
                        Ka_P = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) /
                            (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) *
                            Math.Sin(ksi - tetaP - Delta) *
                            Math.Pow(1 + Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi - beta_back - tetaP) / (Math.Sin(ksi - tetaP - Delta) * Math.Sin(ksi + beta_back))), 2.0));
                    }
                    
                }

            }
        }
        private static void Back_TBDY_Theory_Passive(double waterH1, double ksi, double gamaw, double beta_back, double startLength, ref double Kp_P, ref double Kp_N, ref double Kp_S, SoilData soil)
        {
            if (soil != null)
            {

                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double tetaS = 0 * Math.PI / 180;
                double tetaP = 0 * Math.PI / 180;
                double tetaN = 0 * Math.PI / 180;
                double gama = soil.NaturalUnitWeight;
                double gamad = soil.SaturatedUnitWeight;

                
                Kp_S = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaS) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_back - tetaS) / (Math.Sin(ksi + tetaS) * Math.Sin(ksi + beta_back))), 2));

                if (StaticVariables.viewModel.isEarthQuakeDesign) //dynamic ka
                {
                    double khValue = StaticVariables.viewModel.khValue;
                    double kvValue = StaticVariables.viewModel.kvValue;

                    if (startLength < waterH1) //su olmayan kısım
                    {
                        tetaN = Math.Atan(khValue / (1.0 - kvValue));

                        tetaP = Math.Atan(khValue / (1.0 + kvValue));

                    }
                    else //su olan kısım
                    {
                        if (WpfUtils.GetSoilState(soil.SoilStressStateIndex) == SoilState.Drained) //su olan kısım drained
                        {
                            tetaN = Math.Atan((gamad / (gamad - gamaw)) * (khValue / (1.0 - kvValue)));

                            tetaP = Math.Atan((gamad / (gamad - gamaw)) * (khValue / (1.0 + kvValue)));

                        }
                        else //su olan kısım undrained
                        {
                            tetaN = Math.Atan((gama / (gamad - gamaw)) * (khValue / (1.0 - kvValue)));

                            tetaP = Math.Atan((gama / (gamad - gamaw)) * (khValue / (1.0 + kvValue)));

                        }
                    }
                    
                    Kp_N = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                        Math.Sin(ksi + tetaN) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_back - tetaN) / (Math.Sin(ksi + tetaN) * Math.Sin(ksi + beta_back))), 2));
                    
                    Kp_P = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaP) *
                        Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_back - tetaP) / (Math.Sin(ksi + tetaP) * Math.Sin(ksi + beta_back))), 2));

                }

            }
        }

    }
}
