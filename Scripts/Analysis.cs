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

            double waterH1 = StaticVariables.viewModel.GetGroundWaterH1();
            double waterH2 = StaticVariables.viewModel.GetGroundWaterH2();

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
        public static void WaterLoadToFrameNodes()
        {
            double exH = WpfUtils.GetExHeightForCalculation();
            double exH_waterType3 = StaticVariables.viewModel.GetexcavationHeight();
            double _waterDensity = StaticVariables.waterDensity;
            double waterH1 = StaticVariables.viewModel.GetGroundWaterH1();
            double waterH2 = StaticVariables.viewModel.GetGroundWaterH2();
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
                        WaterLoadData waterLoadData = new WaterLoadData() { Type = LoadType.WaterLoad };
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
                        WaterLoadData waterLoadData = new WaterLoadData() { Type = LoadType.WaterLoad };
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
                        WaterLoadData waterLoadData = new WaterLoadData() { Type = LoadType.WaterLoad };
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
        public static void EffectiveStressToFrameNodes()
        {
            double exH = WpfUtils.GetExHeightForCalculation();
            double wallH = StaticVariables.viewModel.GetWallHeight();
            double _waterDensity = StaticVariables.waterDensity;
            double waterH1 = StaticVariables.viewModel.GetGroundWaterH1();
            double waterH2 = StaticVariables.viewModel.GetGroundWaterH2();

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
                
                EffectiveStress effectiveStress = new EffectiveStress() { Type = LoadType.EffectiveStress };
                double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load,double,double>(effectiveStress,startLoad,startNodeForce));
                double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add( new Tuple<Load,double, double>(effectiveStress,endLoad, endNodeForce));
            }
        }
        public static void ActivePassiveCoefToFrameNodes()
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

                        SoilSpringCoef soilSpringCoef = new SoilSpringCoef() { Type =LoadType.SoilSpringCoef };
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

                        SoilSpringCoef soilSpringCoef = new SoilSpringCoef() { Type = LoadType.SoilSpringCoef };
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

                        SoilSpringCoef soilSpringCoef = new SoilSpringCoef() { Type = LoadType.SoilSpringCoef };
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
    }
}
