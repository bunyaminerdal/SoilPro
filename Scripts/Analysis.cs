﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExDesign.Datas;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ExDesign.Scripts
{
    public static class Analysis
    {
        public static SoilLayerData fixedSoilLayer;
        public static void StageCalculation(double exH_waterH2, double exH_calc,bool isBackForceStartWithK0, bool isSpringsOpenWithK0, int iterationCount)
        {
            SoilLayerFixing();
            WallPartization(exH_waterH2, exH_calc);
            SurchargeToFrameNodes(exH_calc);
            BackWaterPressureToFrameNodes();
            FrontWaterPressureToFrameNodes(exH_waterH2, exH_calc);
            HydroStaticWaterPressureToFrameNodes(exH_waterH2, exH_calc);
            BackEffectiveStressToFrameNodes();
            FrontEffectiveStressToFrameNodes(exH_waterH2, exH_calc);
            BackSubgradeModulusofSoilToFrameNodes();
            FrontSubgradeModulusofSoilToFrameNodes(exH_calc);
            BackActivePassiveCoefToFrameNodes();
            FrontActivePassiveCoefToFrameNodes(exH_waterH2, exH_calc);
            FrameToNodeForce();
            Force displacement = new Force();
            Force rotation = new Force();
            Force springs = new Force() { ID = NodeData.Nodes[0].nodeForce.Find(x => x.Item1.Type == LoadType.Front_SubgradeModulusofSoil).Item1.ID };
            Force backForce = new Force() { ID = NodeData.Nodes[0].nodeForce.Find(x => x.Item1.Type == LoadType.Back_Rest_Horizontal_Force).Item1.ID };
            Force frontForce = new Force() { ID = NodeData.Nodes[0].nodeForce.Find(x => x.Item1.Type == LoadType.Front_Rest_Horizontal_Force).Item1.ID };
            Force TotalForce = new Force();
            for (int i = 0; i < iterationCount+1; i++)
            {
                
               Tuple<Force,Force,Force> forceTuple = IterationForceFrameToNodeForce(displacement.ID, backForce.ID, frontForce.ID, isBackForceStartWithK0, isSpringsOpenWithK0);
                backForce = forceTuple.Item1;
                frontForce = forceTuple.Item2;
                TotalForce = forceTuple.Item3;
                if (i < 1)
                {
                    springs = new Force() { ID = NodeData.Nodes[0].nodeForce.Find(x => x.Item1.Type == LoadType.Front_SubgradeModulusofSoil).Item1.ID };
                }
                else
                {
                    Force springFixed = IterationSpringFixing();
                    springs = springFixed;
                }
                

                Tuple<Force,Force> displacementTuple = IterationMatrixAnalys(TotalForce.ID, springs.ID);
                rotation = displacementTuple.Item1;
                displacement = displacementTuple.Item2;
                
            }
            
            if (fixedSoilLayer != null)
            {
                StaticVariables.viewModel.soilLayerDatas.Remove(fixedSoilLayer);
            }
            StaticVariables.isAnalysisDone = true;
        }
        public static void SoilLayerFixing()
        {
            double wallH = StaticVariables.viewModel.GetWallHeight();
            double soilLayerHeight = 0;
            SoilData lastSoil = null;
            //ka
            foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
            {
                soilLayerHeight += soilLayer.LayerHeight;
                if (soilLayer.Soil != null)
                {
                    lastSoil = soilLayer.Soil;
                }
            }
            if (soilLayerHeight < wallH)
            {
                fixedSoilLayer = new SoilLayerData() { Name = "fixedLayer", LayerHeight = 1000, Soil = lastSoil };
                StaticVariables.viewModel.soilLayerDatas.Add(fixedSoilLayer);
            }
        }
        public static void WallPartization(double exH_waterH2, double exH_calc)
        {
            FrameData.Frames.Clear();
            double wallHeight = StaticVariables.viewModel.wall_h;

            double waterH1 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH1() : double.MaxValue;

            double wallPartStep = wallHeight / (wallHeight * 10);
            int wallPartCount = Convert.ToInt32(Math.Round((wallHeight / wallPartStep), 0, MidpointRounding.ToNegativeInfinity));
            for (int i = 0; i < wallPartCount; i++)
            {
                FrameData frame = new FrameData(new Point(0, Math.Round(i * wallPartStep, 6)), new Point(0, Math.Round((i + 1) * wallPartStep, 6)));
            }
            for (int i = 0; i < FrameData.Frames.Count; i++)
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
                if (exH_calc > FrameData.Frames[i].StartPoint.Y && exH_calc < FrameData.Frames[i].EndPoint.Y)
                {
                    FrameData frameUp = new FrameData(FrameData.Frames[i].StartPoint, new Point(0, exH_calc));
                    FrameData frameDown = new FrameData(new Point(0, exH_calc), FrameData.Frames[i].EndPoint);
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
                if (exH_waterH2 < wallHeight)
                {
                    if (exH_waterH2 > FrameData.Frames[i].StartPoint.Y && exH_waterH2 < FrameData.Frames[i].EndPoint.Y)
                    {
                        FrameData frameUp = new FrameData(FrameData.Frames[i].StartPoint, new Point(0, exH_waterH2));
                        FrameData frameDown = new FrameData(new Point(0, exH_waterH2), FrameData.Frames[i].EndPoint);
                        FrameData.Frames.Remove(FrameData.Frames[i]);
                    }
                }
            }

            FrameData.Frames.Sort();
            FrameData.Frames.Reverse();
        }
        public static void SurchargeToFrameNodes(double exH_calc)
        {
            //surface loaddan gelen nokta force ları
            foreach (var surface in StaticVariables.viewModel.surfaceSurchargeDatas)
            {
                foreach (var frame in FrameData.Frames)
                {
                    double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                    double startLoad = surface.Load;
                    double endLoad = surface.Load;

                    double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                    frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(surface, startLoad, startNodeForce));
                    double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                    frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(surface, endLoad, endNodeForce));
                }
            }
            //point loaddan gelen nokta force ları
            foreach (var pointLoad in StaticVariables.viewModel.PointLoadDatas)
            {
                double mValue = pointLoad.DistanceFromWall / exH_calc;
                foreach (var frame in FrameData.Frames)
                {
                    double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                    double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                    double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                    double startN = startLength / exH_calc;
                    double endN = endLength / exH_calc;

                    double startLoad = 0;
                    double endLoad = 0;
                    if (mValue > 0.4)
                    {
                        startLoad = (1.77 * pointLoad.Load / Math.Pow(exH_calc, 2)) * (Math.Pow(startN, 2) * Math.Pow(mValue, 2) / Math.Pow(Math.Pow(startN, 2) + Math.Pow(mValue, 2), 3));
                        endLoad = (1.77 * pointLoad.Load / Math.Pow(exH_calc, 2)) * (Math.Pow(endN, 2) * Math.Pow(mValue, 2) / Math.Pow(Math.Pow(endN, 2) + Math.Pow(mValue, 2), 3));
                    }
                    else
                    {
                        startLoad = (0.28 * pointLoad.Load / Math.Pow(exH_calc, 2)) * (Math.Pow(startN, 2) / Math.Pow(Math.Pow(startN, 2) + 0.16, 3));
                        endLoad = (0.28 * pointLoad.Load / Math.Pow(exH_calc, 2)) * (Math.Pow(endN, 2) / Math.Pow(Math.Pow(endN, 2) + 0.16, 3));
                    }
                    double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                    frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(pointLoad, startLoad, startNodeForce));
                    double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                    frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(pointLoad, endLoad, endNodeForce));
                }
            }
            //Line loaddan gelen nokta forceları
            foreach (var lineLoad in StaticVariables.viewModel.LineLoadDatas)
            {
                double mValue = lineLoad.DistanceFromWall / exH_calc;
                foreach (var frame in FrameData.Frames)
                {
                    double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                    double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                    double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                    double startN = startLength / exH_calc;
                    double endN = endLength / exH_calc;

                    double startLoad = 0;
                    double endLoad = 0;
                    if (mValue > 0.4)
                    {
                        startLoad = (1.28 * lineLoad.Load / exH_calc) * (startN * Math.Pow(mValue, 2) / Math.Pow(Math.Pow(startN, 2) + Math.Pow(mValue, 2), 2));
                        endLoad = (1.28 * lineLoad.Load / exH_calc) * (endN * Math.Pow(mValue, 2) / Math.Pow(Math.Pow(endN, 2) + Math.Pow(mValue, 2), 2));
                    }
                    else
                    {
                        startLoad = (0.203 * lineLoad.Load / exH_calc) * (startN / Math.Pow(Math.Pow(startN, 2) + 0.16, 2));
                        endLoad = (0.203 * lineLoad.Load / exH_calc) * (endN / Math.Pow(Math.Pow(endN, 2) + 0.16, 2));
                    }
                    double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                    frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(lineLoad, startLoad, startNodeForce));
                    double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                    frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(lineLoad, endLoad, endNodeForce));
                }
            }
            //strip loaddan gelen nokta forceları
            foreach (var stripLoad in StaticVariables.viewModel.stripLoadDatas)
            {
                double startLoc = stripLoad.DistanceFromWall;
                double endLoc = stripLoad.DistanceFromWall + stripLoad.StripLength;
                double midLoc = startLoc + (endLoc - startLoc) / 2;
                double fi = 0;
                double Ka = 0;
                if (StaticVariables.viewModel.soilLayerDatas.Count > 0)
                {
                    if (StaticVariables.viewModel.soilLayerDatas[0].Soil != null)
                    {
                        fi = StaticVariables.viewModel.soilLayerDatas[0].Soil.SoilFrictionAngle;

                    }
                }
                Ka = Math.Pow(Math.Tan((45 - fi / 2) * Math.PI / 180), 2);

                foreach (var frame in FrameData.Frames)
                {
                    double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                    double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                    double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                    double alfaStart = Math.Atan(midLoc / startLength);
                    double alfaEnd = Math.Atan(midLoc / endLength);
                    double betaStart = Math.Atan(endLoc / startLength) - Math.Atan(startLoc / startLength);
                    double betaEnd = Math.Atan(endLoc / endLength) - Math.Atan(startLoc / endLength);
                    if (startLength == 0)
                    {
                        alfaStart = 0;
                        betaStart = 0;
                    }
                    double startLoad = 0;
                    double endLoad = 0;

                    startLoad = (2 * stripLoad.StartLoad / exH_calc) * (betaStart - (Math.Sin(betaStart) * Math.Cos(2 * alfaStart)));
                    endLoad = (2 * stripLoad.StartLoad / exH_calc) * (betaEnd - (Math.Sin(betaEnd) * Math.Cos(2 * alfaEnd)));
                    if (Ka * stripLoad.StartLoad < startLoad)
                    {
                        startLoad = Ka * stripLoad.StartLoad;
                    }
                    if (Ka * stripLoad.StartLoad < endLoad)
                    {
                        endLoad = Ka * stripLoad.StartLoad;
                        if (startLength == 0)
                        {
                            startLoad = endLoad;
                        }
                    }

                    double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                    frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(stripLoad, startLoad, startNodeForce));
                    double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                    frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(stripLoad, endLoad, endNodeForce));
                }
            }
        }
        public static void BackWaterPressureToFrameNodes()
        {
            double _waterDensity = StaticVariables.waterDensity;
            double waterH1 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH1() : double.MaxValue;
            WaterLoadData waterLoadData = new WaterLoadData() { ID = Guid.NewGuid(), Type = LoadType.Back_WaterPressure };
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
                double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, startLoad, startNodeForce));
                double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, endLoad, endNodeForce));
            }
        }
        public static void FrontWaterPressureToFrameNodes(double exH_waterH2, double exH_calc)
        {
            double _waterDensity = StaticVariables.waterDensity;
            WaterLoadData waterLoadData = new WaterLoadData() { ID = Guid.NewGuid(), Type = LoadType.Front_WaterPressure };
            // water load type1
            foreach (var frame in FrameData.Frames)
            {
                double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                double startLoad = 0;
                double endLoad = 0;
                if (exH_waterH2 < startLength)
                {
                    startLoad = (startLength - (exH_waterH2)) * _waterDensity;
                }
                if (exH_waterH2 < endLength)
                {
                    endLoad = (endLength - (exH_waterH2)) * _waterDensity;
                }

                double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, startLoad, startNodeForce));
                double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, endLoad, endNodeForce));
            }
        }
        public static void HydroStaticWaterPressureToFrameNodes(double exH_waterH2, double exH_calc)
        {
            double _waterDensity = StaticVariables.waterDensity;
            double waterH1 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH1() : double.MaxValue;
            WaterLoadData waterLoadData = new WaterLoadData() { ID = Guid.NewGuid(), Type = LoadType.HydroStaticWaterPressure };
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

                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, endLoad, endNodeForce));
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
                        if (startLength > exH_waterH2)
                        {
                            startfrontLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(exH_waterH2 - frame.StartPoint.Y, 2)));
                        }
                        if (endLength > exH_waterH2)
                        {
                            endfrontLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(exH_waterH2 - frame.EndPoint.Y, 2)));
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
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, endLoad, endNodeForce));
                    }
                    break;
                case GroundWaterType.type3:
                    // water load type2
                    double scaleWaterLoad = (exH_waterH2 - waterH1) / (StaticVariables.viewModel.wall_h - exH_waterH2);
                    foreach (var frame in FrameData.Frames)
                    {
                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                        double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));
                        double startfrontLength = 0;
                        double endfrontLength = 0;

                        if (startLength > exH_waterH2)
                        {
                            startfrontLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(exH_waterH2 - frame.StartPoint.Y, 2)));
                        }
                        if (endLength > exH_waterH2)
                        {
                            endfrontLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(exH_waterH2 - frame.EndPoint.Y, 2)));
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
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(waterLoadData, endLoad, endNodeForce));

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
            EffectiveStress effectiveStress = new EffectiveStress() { ID = Guid.NewGuid(), Type = LoadType.Back_EffectiveStress };
            EffectiveStress totalStress = new EffectiveStress() { ID = Guid.NewGuid(), Type = LoadType.Back_TotalStress };
            double startLoad = 0;
            double endLoad = 0;
            foreach (var surfaceLoad in StaticVariables.viewModel.surfaceSurchargeDatas)
            {
                startLoad += surfaceLoad.Load;
                endLoad += surfaceLoad.Load;
            }
            foreach (var frame in FrameData.Frames)
            {
                double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                double soilLayerHeight = 0;
                //SoilData lastSoil = null;
                foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                {
                    soilLayerHeight += soilLayer.LayerHeight;
                    if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight < startLength)
                    {
                        if (soilLayer.Soil != null)
                        {

                            if (startLength <= waterH1)
                            {
                                startLoad += (frameLength * soilLayer.Soil.NaturalUnitWeight);
                            }
                            else
                            {
                                startLoad += (frameLength * (soilLayer.Soil.SaturatedUnitWeight - _waterDensity));
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
                                endLoad += (frameLength * (soilLayer.Soil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                    //if (soilLayer.Soil != null)
                    //{
                    //    lastSoil = soilLayer.Soil;
                    //}
                }

                ////duvardan küçükse
                //if(soilLayerHeight < wallH)
                //{
                //    if (startLength <= wallH && soilLayerHeight < startLength)
                //    {
                //        if (lastSoil != null)
                //        {
                //            if (startLength <= waterH1)
                //            {
                //                startLoad += (frameLength * lastSoil.NaturalUnitWeight);
                //            }
                //            else
                //            {
                //                startLoad += (frameLength * (lastSoil.SaturatedUnitWeight - _waterDensity));
                //            }
                //        }
                //    }
                //    if (endLength <= wallH && soilLayerHeight < endLength)
                //    {
                //        if (lastSoil != null)
                //        {

                //            if (endLength <= waterH1)
                //            {
                //                endLoad += (frameLength * lastSoil.NaturalUnitWeight);
                //            }
                //            else
                //            {
                //                endLoad += (frameLength * (lastSoil.SaturatedUnitWeight - _waterDensity));
                //            }
                //        }
                //    }
                //}


                double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(effectiveStress, startLoad, startNodeForce));
                double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(effectiveStress, endLoad, endNodeForce));

                //total Stress
                double totalStartLoad = startLoad + (frame.startNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Back_WaterPressure) != null ? frame.startNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Back_WaterPressure).Item2 : 0);
                double totalEndLoad = endLoad + (frame.endNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Back_WaterPressure) != null ? frame.endNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Back_WaterPressure).Item2 : 0);
                double totalStartNodeForce = ((((totalStartLoad + totalEndLoad) / 2) + totalStartLoad) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(totalStress, totalStartLoad, totalStartNodeForce));
                double totalEndNodeForce = ((((totalStartLoad + totalEndLoad) / 2) + totalEndLoad) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(totalStress, totalEndLoad, totalEndNodeForce));
            }
        }
        public static void FrontEffectiveStressToFrameNodes(double exH_waterH2, double exH_calc)
        {
            double wallH = StaticVariables.viewModel.GetWallHeight();
            double _waterDensity = StaticVariables.waterDensity;
            EffectiveStress effectiveStress = new EffectiveStress() { ID = Guid.NewGuid(), Type = LoadType.Front_EffectiveStress };
            EffectiveStress totalStress = new EffectiveStress() { ID = Guid.NewGuid(), Type = LoadType.Front_TotalStress };

            double startLoad = 0;
            double endLoad = 0;
            foreach (var frame in FrameData.Frames)
            {
                double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                double soilLayerHeight = 0;
                //SoilData lastSoil = null;
                foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                {
                    soilLayerHeight += soilLayer.LayerHeight;
                    if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight < startLength && exH_calc < startLength)
                    {
                        if (soilLayer.Soil != null)
                        {

                            if (startLength <= exH_waterH2)
                            {
                                startLoad += (frameLength * soilLayer.Soil.NaturalUnitWeight);
                            }
                            else
                            {
                                startLoad += (frameLength * (soilLayer.Soil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                    if (endLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight < endLength && exH_calc < endLength)
                    {
                        if (soilLayer.Soil != null)
                        {
                            if (endLength <= exH_waterH2)
                            {
                                endLoad += (frameLength * soilLayer.Soil.NaturalUnitWeight);
                            }
                            else
                            {
                                endLoad += (frameLength * (soilLayer.Soil.SaturatedUnitWeight - _waterDensity));
                            }
                        }
                    }
                    //if (soilLayer.Soil != null)
                    //{
                    //    lastSoil = soilLayer.Soil;
                    //}
                }

                ////duvardan küçükse
                //if (soilLayerHeight < wallH)
                //{
                //    if (startLength <= wallH && soilLayerHeight < startLength && exH_calc < startLength)
                //    {
                //        if (lastSoil != null)
                //        {
                //            if (startLength <= exH_waterH2)
                //            {
                //                startLoad += (frameLength * lastSoil.NaturalUnitWeight);
                //            }
                //            else
                //            {
                //                startLoad += (frameLength * (lastSoil.SaturatedUnitWeight - _waterDensity));
                //            }
                //        }
                //    }
                //    if (endLength <= wallH && soilLayerHeight < endLength && exH_calc < endLength)
                //    {
                //        if (lastSoil != null)
                //        {

                //            if (endLength <= exH_waterH2)
                //            {
                //                endLoad += (frameLength * lastSoil.NaturalUnitWeight);
                //            }
                //            else
                //            {
                //                endLoad += (frameLength * (lastSoil.SaturatedUnitWeight - _waterDensity));
                //            }
                //        }
                //    }
                //}

                double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(effectiveStress, startLoad, startNodeForce));
                double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(effectiveStress, endLoad, endNodeForce));
                //total Stress
                double totalStartLoad = startLoad + (frame.startNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Front_WaterPressure) != null ? frame.startNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Front_WaterPressure).Item2 : 0);
                double totalEndLoad = endLoad + (frame.endNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Front_WaterPressure) != null ? frame.endNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Front_WaterPressure).Item2 : 0);
                double totalStartNodeForce = ((((totalStartLoad + totalEndLoad) / 2) + totalStartLoad) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(totalStress, totalStartLoad, totalStartNodeForce));
                double totalEndNodeForce = ((((totalStartLoad + totalEndLoad) / 2) + totalEndLoad) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(totalStress, totalEndLoad, totalEndNodeForce));
            }
        }
        public static void BackSubgradeModulusofSoilToFrameNodes()
        {
            SubgradeModulusofSoil Back_subgrademodulus = new SubgradeModulusofSoil() { ID = Guid.NewGuid(), Type = LoadType.Back_SubgradeModulusofSoil };

            double wallH = StaticVariables.viewModel.GetWallHeight();
            double wallt = StaticVariables.viewModel.GetWallThickness();
            double wallEI = StaticVariables.viewModel.GetWallEI();
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
                        foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                        {
                            soilLayerHeight += soilLayer.LayerHeight;
                            if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength)
                            {
                                if (soilLayer.Soil != null)
                                {
                                    startLoad = 2.1 * ((Math.Pow(soilLayer.Soil.OedometricModulus, EoedPow) / Math.Pow(wallEI, EIPow)));
                                    endLoad = 2.1 * (Math.Pow(soilLayer.Soil.OedometricModulus, EoedPow) / Math.Pow(wallEI, EIPow));

                                }
                            }
                        }

                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_subgrademodulus, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_subgrademodulus, endLoad, endNodeForce));
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

                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_subgrademodulus, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_subgrademodulus, endLoad, endNodeForce));
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
                        foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                        {
                            soilLayerHeight += soilLayer.LayerHeight;
                            if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength)
                            {
                                if (soilLayer.Soil != null)
                                {
                                    double Es = soilLayer.Soil.YoungModulus;
                                    double poisson = soilLayer.Soil.PoissonRatio;
                                    startLoad = (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4.0) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));
                                    endLoad = (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4.0) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));
                                }
                            }
                        }
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_subgrademodulus, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_subgrademodulus, endLoad, endNodeForce));
                    }
                    break;
                default:
                    break;
            }

        }
        public static void FrontSubgradeModulusofSoilToFrameNodes(double exH_calc)
        {
            SubgradeModulusofSoil Front_subgrademodulus = new SubgradeModulusofSoil() { ID = Guid.NewGuid(), Type = LoadType.Front_SubgradeModulusofSoil };

            double wallH = StaticVariables.viewModel.GetWallHeight();
            double wallt = StaticVariables.viewModel.GetWallThickness();
            double wallEI = StaticVariables.viewModel.GetWallEI();
            double EoedPow = 4.0 / 3.0;
            double EIPow = 1.0 / 3.0;
            double Pow3 = 1.0 / 5.0;
            double Pow4 = 1.0 / 12.0;
            switch (WpfUtils.GetSoilModelType(StaticVariables.viewModel.SoilModelIndex))
            {
                case SoilModelType.Schmitt_Model:
                    int decreaseCount = 6;
                    int limitofDecreasing = 0;
                    //Kh for schmitt model
                    foreach (var frame in FrameData.Frames)
                    {
                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));

                        double startLoad = 0;
                        double endLoad = 0;
                        double soilLayerHeight = 0;
                        foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                        {
                            soilLayerHeight += soilLayer.LayerHeight;
                            if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength && exH_calc <= startLength)
                            {
                                if (soilLayer.Soil != null)
                                {
                                    decreaseCount= decreaseCount -1;
                                    if(decreaseCount > limitofDecreasing)
                                    {
                                        startLoad = (1 - decreaseCount * 0.1) * 2.1 * ((Math.Pow(soilLayer.Soil.OedometricModulus, EoedPow) / Math.Pow(wallEI, EIPow)));
                                        endLoad = (1 - decreaseCount * 0.1) * 2.1 * (Math.Pow(soilLayer.Soil.OedometricModulus, EoedPow) / Math.Pow(wallEI, EIPow));
                                    }
                                    else
                                    {
                                        startLoad =  2.1 * ((Math.Pow(soilLayer.Soil.OedometricModulus, EoedPow) / Math.Pow(wallEI, EIPow)));
                                        endLoad =  2.1 * (Math.Pow(soilLayer.Soil.OedometricModulus, EoedPow) / Math.Pow(wallEI, EIPow));
                                    }

                                }
                            }
                        }

                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_subgrademodulus, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_subgrademodulus, endLoad, endNodeForce));
                    }

                    break;
                case SoilModelType.Chadeisson_Model:
                    decreaseCount = 6;
                    limitofDecreasing = 0;
                    //Kh for Chadeisson model
                    foreach (var frame in FrameData.Frames)
                    {

                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));

                        double startLoad = 0;
                        double endLoad = 0;
                        double soilLayerHeight = 0;
                        foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                        {
                            soilLayerHeight += soilLayer.LayerHeight;
                            if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength && exH_calc <= startLength)
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
                                    double Kp = Math.Pow(Math.Cos(fi + alfa), 2) / (
                                        Math.Pow(Math.Cos(alfa), 2) * Math.Cos(Delta - alfa) *
                                        Math.Pow(1 - Math.Sqrt(Math.Sin(fi + Delta) * Math.Sin(fi + beta) /
                                        (Math.Cos(Delta - alfa) * Math.Cos(beta - alfa))), 2)
                                        );
                                    decreaseCount = decreaseCount - 1;
                                    if (decreaseCount > limitofDecreasing)
                                    {
                                        startLoad = (1 - decreaseCount * 0.1) * Math.Pow(20 * wallEI * (Math.Pow(Kp * Gama * (1 - (K0 / Kp)) / 0.015, 4)), Pow3) + (Ap * CPrime * Math.Tanh(CPrime / 30) / 0.015);
                                        endLoad = (1 - decreaseCount * 0.1) * Math.Pow(20 * wallEI * (Math.Pow(Kp * Gama * (1 - (K0 / Kp)) / 0.015, 4)), Pow3) + (Ap * CPrime * Math.Tanh(CPrime / 30) / 0.015);
                                    }
                                    else
                                    {
                                        startLoad =  Math.Pow(20 * wallEI * (Math.Pow(Kp * Gama * (1 - (K0 / Kp)) / 0.015, 4)), Pow3) + (Ap * CPrime * Math.Tanh(CPrime / 30) / 0.015);
                                        endLoad =  Math.Pow(20 * wallEI * (Math.Pow(Kp * Gama * (1 - (K0 / Kp)) / 0.015, 4)), Pow3) + (Ap * CPrime * Math.Tanh(CPrime / 30) / 0.015);

                                    }
                                }
                            }
                        }

                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_subgrademodulus, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_subgrademodulus, endLoad, endNodeForce));
                    }
                    break;
                case SoilModelType.Vesic_Model:
                    decreaseCount = 6;
                    limitofDecreasing = 0;
                    //Kh for vesic model
                    foreach (var frame in FrameData.Frames)
                    {

                        double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                        double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));

                        double startLoad = 0;
                        double endLoad = 0;
                        double soilLayerHeight = 0;
                        foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                        {
                            soilLayerHeight += soilLayer.LayerHeight;
                            if (startLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength && exH_calc <= startLength)
                            {
                                if (soilLayer.Soil != null)
                                {
                                    double Es = soilLayer.Soil.YoungModulus;
                                    double poisson = soilLayer.Soil.PoissonRatio;
                                    decreaseCount = decreaseCount - 1;
                                    if (decreaseCount > limitofDecreasing)
                                    {
                                        startLoad = (1 - decreaseCount * 0.1) * (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4.0) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));
                                        endLoad = (1 - decreaseCount * 0.1) * (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4.0) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));
                                    }
                                    else
                                    {
                                        startLoad =  (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4.0) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));
                                        endLoad =  (0.65 / wallt) * Math.Pow(Es * Math.Pow(wallt, 4.0) / wallEI, Pow4) * Es / (1.0 - Math.Pow(poisson, 2));

                                    }
                                }
                            }
                        }
                        double startNodeForce = ((((startLoad + endLoad) / 2) + startLoad) / 2) * (frameLength / 2);
                        frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_subgrademodulus, startLoad, startNodeForce));
                        double endNodeForce = ((((startLoad + endLoad) / 2) + endLoad) / 2) * (frameLength / 2);
                        frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_subgrademodulus, endLoad, endNodeForce));
                    }
                    break;
                default:
                    break;
            }

        }
        public static void BackActivePassiveCoefToFrameNodes()
        {
            Ka_Kp Back_Kactive = new Ka_Kp() { ID = Guid.NewGuid(), Type = LoadType.Back_Kactive };
            Ka_Kp Back_Kpassive = new Ka_Kp() { ID = Guid.NewGuid(), Type = LoadType.Back_Kpassive };
            Ka_Kp Back_Krest = new Ka_Kp() { ID = Guid.NewGuid(), Type = LoadType.Back_Krest };
            Force Back_Active_Vertical_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Back_Active_Vertical_Force };
            Force Back_Passive_Vertical_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Back_Passive_Vertical_Force };
            Force Back_Active_Horizontal_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Back_Active_Horizontal_Force };
            Force Back_Passive_Horizontal_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Back_Passive_Horizontal_Force };
            Force Back_Rest_Horizontal_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Back_Rest_Horizontal_Force };

            double wallH = StaticVariables.viewModel.GetWallHeight();
            double waterH1 = StaticVariables.viewModel.WaterTypeIndex > 0 ? StaticVariables.viewModel.GetGroundWaterH1() : double.MaxValue;
            double ksi = 90 * Math.PI / 180;
            double gamaw = StaticVariables.waterDensity;
            double beta_back = WpfUtils.GetGroundSurfaceType(StaticVariables.viewModel.GroundSurfaceTypeIndex) == GroundSurfaceType.type1 ? StaticVariables.viewModel.backT_Beta * Math.PI / 180 : 0 * Math.PI / 180;

            foreach (var frame in FrameData.Frames)
            {
                double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                double Ka_P_start = 0;
                double Ka_N_start = 0;
                double Ka_S_start = 0;
                double Kp_P_start = 0;
                double Kp_N_start = 0;
                double Kp_S_start = 0;
                double Ka_P_end = 0;
                double Ka_N_end = 0;
                double Ka_S_end = 0;
                double Kp_P_end = 0;
                double Kp_N_end = 0;
                double Kp_S_end = 0;
                double K0_S_start = 0;
                double K0_S_end = 0;
                double Active_Vertical_Force_start = 0;
                double Active_Vertical_Force_end = 0;
                double Active_Horizontal_Force_start = 0;
                double Active_Horizontal_Force_end = 0;
                double Passive_Vertical_Force_start = 0;
                double Passive_Vertical_Force_end = 0;
                double Passive_Horizontal_Force_start = 0;
                double Passive_Horizontal_Force_end = 0;

                double Rest_Horizontal_Force_start = 0;
                double Rest_Horizontal_Force_end = 0;
                double soilLayerHeight = 0;

                //SoilData lastSoil = null;
                //ka
                foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                {
                    soilLayerHeight += soilLayer.LayerHeight;
                    if (soilLayer.Soil != null)
                    {
                        double delta = soilLayer.Soil.WallSoilFrictionAngle * Math.PI / 180;
                        double K0 = soilLayer.Soil.K0;
                        double fi = soilLayer.Soil.SoilFrictionAngle * Math.PI / 180;
                        if (startLength < soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength)
                        {

                            if (WpfUtils.GetSoilState(soilLayer.Soil.SoilStressStateIndex) == SoilState.Drained)
                            {
                                double stress = frame.startNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Back_EffectiveStress).Item2;
                                double cPrime = soilLayer.Soil.EffectiveCohesion;
                                Front_Rest_Start(fi, beta_back, K0, stress, ref K0_S_start, ref Rest_Horizontal_Force_start);
                                switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.activeDrainedCoefficientIndex))
                                {
                                    case DrainedTheories.TBDY:
                                        Back_TBDY_Theory_Active_Start(waterH1, delta, stress, cPrime, ksi, gamaw, beta_back, startLength, ref Ka_P_start, ref Ka_N_start, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Active_Start(0, delta, stress, frameLength, startLength, beta_back, cPrime, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Active_Start(delta, stress, cPrime, beta_back, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Active_Start(delta, stress, cPrime, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                                switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.passiveDrainedCoefficientIndex))
                                {
                                    case DrainedTheories.TBDY:
                                        Back_TBDY_Theory_Passive_Start(waterH1, delta, stress, cPrime, ksi, gamaw, beta_back, startLength, ref Kp_P_start, ref Kp_N_start, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Passive_Start(0, delta, stress, frameLength, startLength, beta_back, cPrime, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Passive_Start(delta, stress, cPrime, beta_back, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Passive_Start(delta, stress, cPrime, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                double stress = frame.startNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Back_TotalStress).Item2;
                                double cPrime = soilLayer.Soil.UndrainedShearStrength;
                                Front_Rest_Start(fi, beta_back, K0, stress, ref K0_S_start, ref Rest_Horizontal_Force_start);
                                switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.activeUnDrainedCoefficientIndex))
                                {
                                    case UnDrainedTheories.TBDY:
                                        Back_TBDY_Theory_Active_Start(waterH1, delta, stress, cPrime, ksi, gamaw, beta_back, startLength, ref Ka_P_start, ref Ka_N_start, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Active_Start(0, delta, stress, frameLength, startLength, beta_back, cPrime, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Active_Start(delta, stress, cPrime, beta_back, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Active_Start(delta, stress, cPrime, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TotalStress:
                                        Back_TotalStress_Theory_Active_Start(stress, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                                switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.passiveUnDrainedCoefficientIndex))
                                {
                                    case UnDrainedTheories.TBDY:
                                        Back_TBDY_Theory_Passive_Start(waterH1, delta, stress, cPrime, ksi, gamaw, beta_back, startLength, ref Kp_P_start, ref Kp_N_start, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Passive_Start(0, delta, stress, frameLength, startLength, beta_back, cPrime, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Passive_Start(delta, stress, cPrime, beta_back, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Passive_Start(delta, stress, cPrime, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TotalStress:
                                        Back_TotalStress_Theory_Passive_Start(stress, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }
                        if (endLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight < endLength)
                        {
                            if (WpfUtils.GetSoilState(soilLayer.Soil.SoilStressStateIndex) == SoilState.Drained)
                            {
                                double stress = frame.endNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Back_EffectiveStress).Item2;
                                double cPrime = soilLayer.Soil.EffectiveCohesion;
                                Front_Rest_End(fi, beta_back, K0, stress, ref K0_S_end, ref Rest_Horizontal_Force_end);
                                switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.activeDrainedCoefficientIndex))
                                {
                                    case DrainedTheories.TBDY:
                                        Back_TBDY_Theory_Active_End(waterH1, delta, stress, cPrime, ksi, gamaw, beta_back, endLength, ref Ka_P_end, ref Ka_N_end, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Active_End(0, delta, stress, endLength, beta_back, cPrime, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Active_End(delta, stress, cPrime, beta_back, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Active_End(delta, stress, cPrime, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                                switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.passiveDrainedCoefficientIndex))
                                {
                                    case DrainedTheories.TBDY:
                                        Back_TBDY_Theory_Passive_End(waterH1, delta, stress, cPrime, ksi, gamaw, beta_back, endLength, ref Kp_P_end, ref Kp_N_end, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Passive_End(0, delta, stress, endLength, beta_back, cPrime, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Passive_End(delta, stress, cPrime, beta_back, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Passive_End(delta, stress, cPrime, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                double stress = frame.endNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Back_TotalStress).Item2;
                                double cPrime = soilLayer.Soil.UndrainedShearStrength;
                                Front_Rest_End(fi, beta_back, K0, stress, ref K0_S_end, ref Rest_Horizontal_Force_end);
                                switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.activeUnDrainedCoefficientIndex))
                                {
                                    case UnDrainedTheories.TBDY:
                                        Back_TBDY_Theory_Active_End(waterH1, delta, stress, cPrime, ksi, gamaw, beta_back, endLength, ref Ka_P_end, ref Ka_N_end, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Active_End(0, delta, stress, endLength, beta_back, cPrime, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Active_End(delta, stress, cPrime, beta_back, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Active_End(delta, stress, cPrime, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TotalStress:
                                        Back_TotalStress_Theory_Active_End(stress, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                                switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.passiveUnDrainedCoefficientIndex))
                                {
                                    case UnDrainedTheories.TBDY:
                                        Back_TBDY_Theory_Passive_End(waterH1, delta, stress, cPrime, ksi, gamaw, beta_back, endLength, ref Kp_P_end, ref Kp_N_end, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);

                                        break;
                                    case UnDrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Passive_End(0, delta, stress, endLength, beta_back, cPrime, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);

                                        break;
                                    case UnDrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Passive_End(delta, stress, cPrime, beta_back, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);

                                        break;
                                    case UnDrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Passive_End(delta, stress, cPrime, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TotalStress:
                                        Back_TotalStress_Theory_Passive_End(stress, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        //lastSoil = soilLayer.Soil;
                    }
                }

                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Back_Kactive, Ka_S_start, Ka_P_start, Ka_N_start));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Back_Kactive, Ka_S_end, Ka_P_end, Ka_N_end));

                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Back_Kpassive, Kp_S_start, Kp_P_start, Kp_N_start));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Back_Kpassive, Kp_S_end, Kp_P_end, Kp_N_end));

                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Back_Krest, K0_S_start, 0, 0));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Back_Krest, K0_S_end, 0, 0));

                double Active_vertical_startNodeForce = ((((Active_Vertical_Force_start + Active_Vertical_Force_end) / 2) + Active_Vertical_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Active_Vertical_Force, Active_Vertical_Force_start, Active_vertical_startNodeForce));
                double Active_vertical_endNodeForce = ((((Active_Vertical_Force_start + Active_Vertical_Force_end) / 2) + Active_Vertical_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Active_Vertical_Force, Active_Vertical_Force_end, Active_vertical_endNodeForce));

                double Passive_Vertical_startNodeForce = ((((Passive_Vertical_Force_start + Passive_Vertical_Force_end) / 2) + Passive_Vertical_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Passive_Vertical_Force, Passive_Vertical_Force_start, Passive_Vertical_startNodeForce));
                double Passive_Vertical_endNodeForce = ((((Passive_Vertical_Force_start + Passive_Vertical_Force_end) / 2) + Passive_Vertical_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Passive_Vertical_Force, Passive_Vertical_Force_end, Passive_Vertical_endNodeForce));

                double Active_horizontal_startNodeForce = ((((Active_Horizontal_Force_start + Active_Horizontal_Force_end) / 2) + Active_Horizontal_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Active_Horizontal_Force, Active_Horizontal_Force_start, Active_horizontal_startNodeForce));
                double Active_horizontal_endNodeForce = ((((Active_Horizontal_Force_start + Active_Horizontal_Force_end) / 2) + Active_Horizontal_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Active_Horizontal_Force, Active_Horizontal_Force_end, Active_horizontal_endNodeForce));

                double Passive_Horizontal_startNodeForce = ((((Passive_Horizontal_Force_start + Passive_Horizontal_Force_end) / 2) + Passive_Horizontal_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Passive_Horizontal_Force, Passive_Horizontal_Force_start, Passive_Horizontal_startNodeForce));
                double Passive_Horizontal_endNodeForce = ((((Passive_Horizontal_Force_start + Passive_Horizontal_Force_end) / 2) + Passive_Horizontal_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Passive_Horizontal_Force, Passive_Horizontal_Force_end, Passive_Horizontal_endNodeForce));

                double Rest_horizontal_startNodeForce = ((((Rest_Horizontal_Force_start + Rest_Horizontal_Force_end) / 2) + Rest_Horizontal_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Rest_Horizontal_Force, Rest_Horizontal_Force_start, Rest_horizontal_startNodeForce));
                double Rest_horizontal_endNodeForce = ((((Rest_Horizontal_Force_start + Rest_Horizontal_Force_end) / 2) + Rest_Horizontal_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Back_Rest_Horizontal_Force, Rest_Horizontal_Force_end, Rest_horizontal_endNodeForce));


            }
        }
        public static void FrontActivePassiveCoefToFrameNodes(double exH_waterH2, double exH_calc)
        {
            Ka_Kp Front_Kactive = new Ka_Kp() { ID = Guid.NewGuid(), Type = LoadType.Front_Kactive };
            Ka_Kp Front_Kpassive = new Ka_Kp() { ID = Guid.NewGuid(), Type = LoadType.Front_Kpassive };
            Ka_Kp Front_Krest = new Ka_Kp() { ID = Guid.NewGuid(), Type = LoadType.Front_Krest };

            Force Front_Active_Vertical_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Front_Active_Vertical_Force };
            Force Front_Passive_Vertical_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Front_Passive_Vertical_Force };
            Force Front_Active_Horizontal_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Front_Active_Horizontal_Force };
            Force Front_Passive_Horizontal_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Front_Passive_Horizontal_Force };
            Force Front_Rest_Horizontal_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Front_Rest_Horizontal_Force };

            double wallH = StaticVariables.viewModel.GetWallHeight();
            double ksi = 90 * Math.PI / 180;
            double gamaw = StaticVariables.waterDensity;
            double beta_front = 0 * Math.PI / 180;

            foreach (var frame in FrameData.Frames)
            {
                double frameLength = Math.Sqrt((Math.Pow(frame.StartPoint.X - frame.EndPoint.X, 2) + Math.Pow(frame.StartPoint.Y - frame.EndPoint.Y, 2)));
                double startLength = Math.Sqrt((Math.Pow(0 - frame.StartPoint.X, 2) + Math.Pow(0 - frame.StartPoint.Y, 2)));
                double endLength = Math.Sqrt((Math.Pow(0 - frame.EndPoint.X, 2) + Math.Pow(0 - frame.EndPoint.Y, 2)));

                double Ka_P_start = 0;
                double Ka_N_start = 0;
                double Ka_S_start = 0;
                double Kp_P_start = 0;
                double Kp_N_start = 0;
                double Kp_S_start = 0;
                double Ka_P_end = 0;
                double Ka_N_end = 0;
                double Ka_S_end = 0;
                double Kp_P_end = 0;
                double Kp_N_end = 0;
                double Kp_S_end = 0;
                double K0_S_start = 0;
                double K0_S_end = 0;
                double Active_Vertical_Force_start = 0;
                double Active_Vertical_Force_end = 0;
                double Active_Horizontal_Force_start = 0;
                double Active_Horizontal_Force_end = 0;
                double Passive_Vertical_Force_start = 0;
                double Passive_Vertical_Force_end = 0;
                double Passive_Horizontal_Force_start = 0;
                double Passive_Horizontal_Force_end = 0;

                double Rest_Horizontal_Force_start = 0;
                double Rest_Horizontal_Force_end = 0;
                double soilLayerHeight = 0;

                //SoilData lastSoil = null;
                //ka
                foreach (var soilLayer in StaticVariables.viewModel.soilLayerDatas)
                {
                    soilLayerHeight += soilLayer.LayerHeight;
                    if (soilLayer.Soil != null)
                    {
                        double delta = soilLayer.Soil.WallSoilFrictionAngle * Math.PI / 180;
                        double K0 = soilLayer.Soil.K0;
                        double fi = soilLayer.Soil.SoilFrictionAngle * Math.PI / 180;
                        if (startLength < soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight <= startLength && exH_calc <= startLength)
                        {

                            if (WpfUtils.GetSoilState(soilLayer.Soil.SoilStressStateIndex) == SoilState.Drained)
                            {
                                double stress = frame.startNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Front_EffectiveStress).Item2;
                                double cPrime = soilLayer.Soil.EffectiveCohesion;
                                Front_Rest_Start(fi, beta_front, K0, stress, ref K0_S_start, ref Rest_Horizontal_Force_start);
                                switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.activeDrainedCoefficientIndex))
                                {
                                    case DrainedTheories.TBDY:
                                        Back_TBDY_Theory_Active_Start(exH_waterH2, delta, stress, cPrime, ksi, gamaw, beta_front, startLength, ref Ka_P_start, ref Ka_N_start, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Active_Start(exH_calc, delta, stress, frameLength, startLength, beta_front, cPrime, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Active_Start(delta, stress, cPrime, beta_front, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Active_Start(delta, stress, cPrime, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                                switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.passiveDrainedCoefficientIndex))
                                {
                                    case DrainedTheories.TBDY:
                                        Back_TBDY_Theory_Passive_Start(exH_waterH2, delta, stress, cPrime, ksi, gamaw, beta_front, startLength, ref Kp_P_start, ref Kp_N_start, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Passive_Start(exH_calc, delta, stress, frameLength, startLength, beta_front, cPrime, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Passive_Start(delta, stress, cPrime, beta_front, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Passive_Start(delta, stress, cPrime, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                double stress = frame.startNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Front_TotalStress).Item2;
                                double cPrime = soilLayer.Soil.UndrainedShearStrength;
                                Front_Rest_Start(fi, beta_front, K0, stress, ref K0_S_start, ref Rest_Horizontal_Force_start);
                                switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.activeUnDrainedCoefficientIndex))
                                {
                                    case UnDrainedTheories.TBDY:
                                        Back_TBDY_Theory_Active_Start(exH_waterH2, delta, stress, cPrime, ksi, gamaw, beta_front, startLength, ref Ka_P_start, ref Ka_N_start, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Active_Start(exH_calc, delta, stress, frameLength, startLength, beta_front, cPrime, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Active_Start(delta, stress, cPrime, beta_front, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Active_Start(delta, stress, cPrime, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TotalStress:
                                        Back_TotalStress_Theory_Active_Start(stress, ref Ka_S_start, ref Active_Vertical_Force_start, ref Active_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                                switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.passiveUnDrainedCoefficientIndex))
                                {
                                    case UnDrainedTheories.TBDY:
                                        Back_TBDY_Theory_Passive_Start(exH_waterH2, delta, stress, cPrime, ksi, gamaw, beta_front, startLength, ref Kp_P_start, ref Kp_N_start, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Passive_Start(exH_calc, delta, stress, frameLength, startLength, beta_front, cPrime, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Passive_Start(delta, stress, cPrime, beta_front, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Passive_Start(delta, stress, cPrime, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TotalStress:
                                        Back_TotalStress_Theory_Passive_Start(stress, ref Kp_S_start, ref Passive_Vertical_Force_start, ref Passive_Horizontal_Force_start, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }
                        if (endLength <= soilLayerHeight && soilLayerHeight - soilLayer.LayerHeight < endLength && exH_calc < endLength)
                        {
                            if (WpfUtils.GetSoilState(soilLayer.Soil.SoilStressStateIndex) == SoilState.Drained)
                            {
                                double stress = frame.endNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Front_EffectiveStress).Item2;
                                double cPrime = soilLayer.Soil.EffectiveCohesion;
                                Front_Rest_End(fi, beta_front, K0, stress, ref K0_S_end, ref Rest_Horizontal_Force_end);
                                switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.activeDrainedCoefficientIndex))
                                {
                                    case DrainedTheories.TBDY:
                                        Back_TBDY_Theory_Active_End(exH_waterH2, delta, stress, cPrime, ksi, gamaw, beta_front, endLength, ref Ka_P_end, ref Ka_N_end, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Active_End(exH_calc, delta, stress, endLength, beta_front, cPrime, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Active_End(delta, stress, cPrime, beta_front, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Active_End(delta, stress, cPrime, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                                switch (WpfUtils.GetDrainedTheoryType(StaticVariables.viewModel.passiveDrainedCoefficientIndex))
                                {
                                    case DrainedTheories.TBDY:
                                        Back_TBDY_Theory_Passive_End(exH_waterH2, delta, stress, cPrime, ksi, gamaw, beta_front, endLength, ref Kp_P_end, ref Kp_N_end, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Passive_End(exH_calc, delta, stress, endLength, beta_front, cPrime, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Passive_End(delta, stress, cPrime, beta_front, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case DrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Passive_End(delta, stress, cPrime, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                double stress = frame.endNodeLoadAndForce.Find(x => x.Item1.Type == LoadType.Front_TotalStress).Item2;
                                double cPrime = soilLayer.Soil.UndrainedShearStrength;
                                Front_Rest_End(fi, beta_front, K0, stress, ref K0_S_end, ref Rest_Horizontal_Force_end);
                                switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.activeUnDrainedCoefficientIndex))
                                {
                                    case UnDrainedTheories.TBDY:
                                        Back_TBDY_Theory_Active_End(exH_waterH2, delta, stress, cPrime, ksi, gamaw, beta_front, endLength, ref Ka_P_end, ref Ka_N_end, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Active_End(exH_calc, delta, stress, endLength, beta_front, cPrime, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Active_End(delta, stress, cPrime, beta_front, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Active_End(delta, stress, cPrime, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TotalStress:
                                        Back_TotalStress_Theory_Active_End(stress, ref Ka_S_end, ref Active_Vertical_Force_end, ref Active_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                                switch (WpfUtils.GetUnDrainedTheoryType(StaticVariables.viewModel.passiveUnDrainedCoefficientIndex))
                                {
                                    case UnDrainedTheories.TBDY:
                                        Back_TBDY_Theory_Passive_End(exH_waterH2, delta, stress, cPrime, ksi, gamaw, beta_front, endLength, ref Kp_P_end, ref Kp_N_end, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.MazindraniTheory:
                                        Back_Mazindrani_Theory_Passive_End(exH_calc, delta, stress, endLength, beta_front, cPrime, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TheColoumbTheory:
                                        Back_Coloumb_Theory_Passive_End(delta, stress, cPrime, beta_front, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.RankineTheory:
                                        Back_Rankine_Theory_Passive_End(delta, stress, cPrime, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    case UnDrainedTheories.TotalStress:
                                        Back_TotalStress_Theory_Passive_End(stress, ref Kp_S_end, ref Passive_Vertical_Force_end, ref Passive_Horizontal_Force_end, soilLayer.Soil);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        //lastSoil = soilLayer.Soil;
                    }
                }


                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Kactive, Ka_S_start, Ka_P_start, Ka_N_start));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Kactive, Ka_S_end, Ka_P_end, Ka_N_end));

                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Kpassive, Kp_S_start, Kp_P_start, Kp_N_start));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Kpassive, Kp_S_end, Kp_P_end, Kp_N_end));

                frame.startNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Krest, K0_S_start, 0, 0));
                frame.endNodeActivePassiveCoef_S_P_N.Add(new Tuple<Load, double, double, double>(Front_Krest, K0_S_end, 0, 0));

                double Active_vertical_startNodeForce = ((((Active_Vertical_Force_start + Active_Vertical_Force_end) / 2) + Active_Vertical_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Active_Vertical_Force, Active_Vertical_Force_start, Active_vertical_startNodeForce));
                double Active_vertical_endNodeForce = ((((Active_Vertical_Force_start + Active_Vertical_Force_end) / 2) + Active_Vertical_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Active_Vertical_Force, Active_Vertical_Force_end, Active_vertical_endNodeForce));

                double Passive_Vertical_startNodeForce = ((((Passive_Vertical_Force_start + Passive_Vertical_Force_end) / 2) + Passive_Vertical_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Passive_Vertical_Force, Passive_Vertical_Force_start, Passive_Vertical_startNodeForce));
                double Passive_Vertical_endNodeForce = ((((Passive_Vertical_Force_start + Passive_Vertical_Force_end) / 2) + Passive_Vertical_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Passive_Vertical_Force, Passive_Vertical_Force_end, Passive_Vertical_endNodeForce));

                double Active_horizontal_startNodeForce = ((((Active_Horizontal_Force_start + Active_Horizontal_Force_end) / 2) + Active_Horizontal_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Active_Horizontal_Force, Active_Horizontal_Force_start, Active_horizontal_startNodeForce));
                double Active_horizontal_endNodeForce = ((((Active_Horizontal_Force_start + Active_Horizontal_Force_end) / 2) + Active_Horizontal_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Active_Horizontal_Force, Active_Horizontal_Force_end, Active_horizontal_endNodeForce));

                double Passive_Horizontal_startNodeForce = ((((Passive_Horizontal_Force_start + Passive_Horizontal_Force_end) / 2) + Passive_Horizontal_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Passive_Horizontal_Force, Passive_Horizontal_Force_start, Passive_Horizontal_startNodeForce));
                double Passive_Horizontal_endNodeForce = ((((Passive_Horizontal_Force_start + Passive_Horizontal_Force_end) / 2) + Passive_Horizontal_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Passive_Horizontal_Force, Passive_Horizontal_Force_end, Passive_Horizontal_endNodeForce));

                double Rest_horizontal_startNodeForce = ((((Rest_Horizontal_Force_start + Rest_Horizontal_Force_end) / 2) + Rest_Horizontal_Force_start) / 2) * (frameLength / 2);
                frame.startNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Rest_Horizontal_Force, Rest_Horizontal_Force_start, Rest_horizontal_startNodeForce));
                double Rest_horizontal_endNodeForce = ((((Rest_Horizontal_Force_start + Rest_Horizontal_Force_end) / 2) + Rest_Horizontal_Force_end) / 2) * (frameLength / 2);
                frame.endNodeLoadAndForce.Add(new Tuple<Load, double, double>(Front_Rest_Horizontal_Force, Rest_Horizontal_Force_end, Rest_horizontal_endNodeForce));

            }
        }
        private static void Front_Rest_Start(double fi, double beta_back, double K0, double stress, ref double K0_S_start, ref double Rest_Horizontal_Force_start)
        {
            K0_S_start = K0;
            if (fi <= 0)
            {
                Rest_Horizontal_Force_start = K0 * stress;
            }
            else
            {
                if (beta_back > fi)
                {
                    beta_back = fi;
                }
                Rest_Horizontal_Force_start = K0 * stress * Math.Sin(fi) * Math.Cos(beta_back) / (Math.Sin(fi) - Math.Pow(Math.Sin(beta_back), 2.0));
            }
        }
        private static void Front_Rest_End(double fi, double beta_back, double K0, double stress, ref double K0_S_end, ref double Rest_Horizontal_Force_end)
        {
            K0_S_end = K0;
            if (fi <= 0)
            {
                Rest_Horizontal_Force_end = K0 * stress;
            }
            else
            {
                if (beta_back > fi)
                {
                    beta_back = fi;
                }
                Rest_Horizontal_Force_end = K0 * stress * Math.Sin(fi) * Math.Cos(beta_back) / (Math.Sin(fi) - Math.Pow(Math.Sin(beta_back), 2.0));
            }
        }
        private static void Back_TBDY_Theory_Active_Start(double waterH1, double delta, double stress, double cPrime, double ksi, double gamaw, double beta_back, double startLength, ref double Ka_P_start, ref double Ka_N_start, ref double Ka_S_start, ref double Active_Vertical_Force_start, ref double Active_Horizontal_Force_start, SoilData soil)
        {
            if (soil != null)
            {
                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double tetaS = 0 * Math.PI / 180;
                double tetaP = 0 * Math.PI / 180;
                double tetaN = 0 * Math.PI / 180;
                double gama = soil.NaturalUnitWeight;
                double gamad = soil.SaturatedUnitWeight;

                if (beta_back > fi - tetaS)
                {
                    Ka_S_start = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaS - delta));
                }
                else
                {
                    Ka_S_start = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) /
                        (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) *
                        Math.Sin(ksi - tetaS - delta) *
                        Math.Pow(1 + Math.Sqrt(Math.Sin(fi + delta) * Math.Sin(fi - beta_back - tetaS) / (Math.Sin(ksi - tetaS - delta) * Math.Sin(ksi + beta_back))), 2.0));
                }
                Active_Vertical_Force_start = (Ka_S_start * stress - cPrime * Math.Sqrt(Ka_S_start)) * Math.Cos(delta);
                Active_Horizontal_Force_start = (Ka_S_start * stress - cPrime * Math.Sqrt(Ka_S_start)) * Math.Sin(delta);

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
                        Ka_N_start = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaN - delta));
                        Ka_N_start = Ka_N_start - Ka_S_start;

                    }
                    else
                    {
                        Ka_N_start = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) /
                            (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                            Math.Sin(ksi - tetaN - delta) *
                            Math.Pow(1 + Math.Sqrt(Math.Sin(fi + delta) * Math.Sin(fi - beta_back - tetaN) / (Math.Sin(ksi - tetaN - delta) * Math.Sin(ksi + beta_back))), 2.0));
                        Ka_N_start = Ka_N_start - Ka_S_start;
                    }
                    if (beta_back > fi - tetaP)
                    {
                        Ka_P_start = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaP - delta));
                        Ka_P_start = Ka_P_start - Ka_S_start;
                    }
                    else
                    {
                        Ka_P_start = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) /
                            (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) *
                            Math.Sin(ksi - tetaP - delta) *
                            Math.Pow(1 + Math.Sqrt(Math.Sin(fi + delta) * Math.Sin(fi - beta_back - tetaP) / (Math.Sin(ksi - tetaP - delta) * Math.Sin(ksi + beta_back))), 2.0));
                        Ka_P_start = Ka_P_start - Ka_S_start;
                    }

                }

            }
        }
        private static void Back_TBDY_Theory_Active_End(double waterH1, double delta, double stress, double cPrime, double ksi, double gamaw, double beta_back, double endLength, ref double Ka_P_end, ref double Ka_N_end, ref double Ka_S_end, ref double Active_Vertical_Force_end, ref double Active_Horizontal_Force_end, SoilData soil)
        {
            if (soil != null)
            {

                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double tetaS = 0 * Math.PI / 180;
                double tetaP = 0 * Math.PI / 180;
                double tetaN = 0 * Math.PI / 180;
                double gama = soil.NaturalUnitWeight;
                double gamad = soil.SaturatedUnitWeight;

                if (beta_back > fi - tetaS)
                {
                    Ka_S_end = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaS - delta));
                }
                else
                {
                    Ka_S_end = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) /
                        (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) *
                        Math.Sin(ksi - tetaS - delta) *
                        Math.Pow(1 + Math.Sqrt(Math.Sin(fi + delta) * Math.Sin(fi - beta_back - tetaS) / (Math.Sin(ksi - tetaS - delta) * Math.Sin(ksi + beta_back))), 2.0));
                }
                Active_Vertical_Force_end = (Ka_S_end * stress - cPrime * Math.Sqrt(Ka_S_end)) * Math.Cos(delta);
                Active_Horizontal_Force_end = (Ka_S_end * stress - cPrime * Math.Sqrt(Ka_S_end)) * Math.Sin(delta);

                if (StaticVariables.viewModel.isEarthQuakeDesign) //dynamic ka
                {
                    double khValue = StaticVariables.viewModel.khValue;
                    double kvValue = StaticVariables.viewModel.kvValue;

                    if (endLength <= waterH1) //su olmayan kısım
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
                        Ka_N_end = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaN - delta));
                        Ka_N_end = Ka_N_end - Ka_S_end;
                    }
                    else
                    {
                        Ka_N_end = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) /
                           (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                           Math.Sin(ksi - tetaN - delta) *
                           Math.Pow(1 + Math.Sqrt(Math.Sin(fi + delta) * Math.Sin(fi - beta_back - tetaN) / (Math.Sin(ksi - tetaN - delta) * Math.Sin(ksi + beta_back))), 2.0));
                        Ka_N_end = Ka_N_end - Ka_S_end;
                    }
                    if (beta_back > fi - tetaP)
                    {
                        Ka_P_end = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi - tetaP - delta));
                        Ka_P_end = Ka_P_end - Ka_S_end;
                    }
                    else
                    {
                        Ka_P_end = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) /
                           (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) *
                           Math.Sin(ksi - tetaP - delta) *
                           Math.Pow(1 + Math.Sqrt(Math.Sin(fi + delta) * Math.Sin(fi - beta_back - tetaP) / (Math.Sin(ksi - tetaP - delta) * Math.Sin(ksi + beta_back))), 2.0));
                        Ka_P_end = Ka_P_end - Ka_S_end;
                    }

                }

            }
        }
        private static void Back_TBDY_Theory_Passive_Start(double waterH1, double delta, double stress, double cPrime, double ksi, double gamaw, double beta_back, double startLength, ref double Kp_P_start, ref double Kp_N_start, ref double Kp_S_start, ref double Passive_Vertical_Force_start, ref double Passive_Horizontal_Force_start, SoilData soil)
        {
            if (soil != null)
            {

                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double tetaS = 0 * Math.PI / 180;
                double tetaP = 0 * Math.PI / 180;
                double tetaN = 0 * Math.PI / 180;
                double gama = soil.NaturalUnitWeight;
                double gamad = soil.SaturatedUnitWeight;


                Kp_S_start = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaS) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_back - tetaS) / (Math.Sin(ksi + tetaS) * Math.Sin(ksi + beta_back))), 2));
                Passive_Vertical_Force_start = (Kp_S_start * stress + cPrime * Math.Sqrt(Kp_S_start)) * Math.Cos(delta);
                Passive_Horizontal_Force_start = (Kp_S_start * stress + cPrime * Math.Sqrt(Kp_S_start)) * Math.Sin(delta);
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

                    Kp_N_start = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                        Math.Sin(ksi + tetaN) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_back - tetaN) / (Math.Sin(ksi + tetaN) * Math.Sin(ksi + beta_back))), 2));

                    Kp_P_start = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaP) *
                        Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_back - tetaP) / (Math.Sin(ksi + tetaP) * Math.Sin(ksi + beta_back))), 2));
                    Kp_N_start = Kp_N_start - Kp_S_start;
                    Kp_P_start = Kp_P_start - Kp_S_start;

                }

            }
        }
        private static void Back_TBDY_Theory_Passive_End(double waterH1, double delta, double stress, double cPrime, double ksi, double gamaw, double beta_back, double endLength, ref double Kp_P_end, ref double Kp_N_end, ref double Kp_S_end, ref double Passive_Vertical_Force_end, ref double Passive_Horizontal_Force_end, SoilData soil)
        {
            if (soil != null)
            {
                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double tetaS = 0 * Math.PI / 180;
                double tetaP = 0 * Math.PI / 180;
                double tetaN = 0 * Math.PI / 180;
                double gama = soil.NaturalUnitWeight;
                double gamad = soil.SaturatedUnitWeight;


                Kp_S_end = Math.Pow(Math.Sin(ksi + fi - tetaS), 2.0) / (Math.Cos(tetaS) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaS) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_back - tetaS) / (Math.Sin(ksi + tetaS) * Math.Sin(ksi + beta_back))), 2));

                Passive_Vertical_Force_end = (Kp_S_end * stress + cPrime * Math.Sqrt(Kp_S_end)) * Math.Cos(delta);
                Passive_Horizontal_Force_end = (Kp_S_end * stress + cPrime * Math.Sqrt(Kp_S_end)) * Math.Sin(delta);

                if (StaticVariables.viewModel.isEarthQuakeDesign) //dynamic ka
                {
                    double khValue = StaticVariables.viewModel.khValue;
                    double kvValue = StaticVariables.viewModel.kvValue;

                    if (endLength <= waterH1) //su olmayan kısım
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

                    Kp_N_end = Math.Pow(Math.Sin(ksi + fi - tetaN), 2.0) / (Math.Cos(tetaN) * Math.Pow(Math.Sin(ksi), 2.0) *
                        Math.Sin(ksi + tetaN) * Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_back - tetaN) / (Math.Sin(ksi + tetaN) * Math.Sin(ksi + beta_back))), 2));

                    Kp_P_end = Math.Pow(Math.Sin(ksi + fi - tetaP), 2.0) / (Math.Cos(tetaP) * Math.Pow(Math.Sin(ksi), 2.0) * Math.Sin(ksi + tetaP) *
                        Math.Pow(1 - Math.Sqrt(Math.Sin(fi) * Math.Sin(fi + beta_back - tetaP) / (Math.Sin(ksi + tetaP) * Math.Sin(ksi + beta_back))), 2));
                    Kp_N_end = Kp_N_end - Kp_S_end;
                    Kp_P_end = Kp_P_end - Kp_S_end;
                }

            }
        }
        private static void Back_Rankine_Theory_Active_Start(double delta, double stress, double cPrime, ref double Ka_S_start, ref double Active_Vertical_Force_start, ref double Active_Horizontal_Force_start, SoilData soil)
        {
            if (soil != null)
            {
                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double zeta45 = 45.0 * Math.PI / 180;
                Ka_S_start = Math.Pow(Math.Tan(zeta45 - (fi / 2.0)), 2.0);
                Active_Horizontal_Force_start = (stress * Ka_S_start - cPrime * Math.Sqrt(Ka_S_start)) * Math.Cos(delta);
                Active_Vertical_Force_start = (stress * Ka_S_start - cPrime * Math.Sqrt(Ka_S_start)) * Math.Sin(delta);

            }
        }
        private static void Back_Rankine_Theory_Active_End(double delta, double stress, double cPrime, ref double Ka_S_end, ref double Active_Vertical_Force_end, ref double Active_Horizontal_Force_end, SoilData soil)
        {
            if (soil != null)
            {
                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double zeta45 = 45.0 * Math.PI / 180;
                Ka_S_end = Math.Pow(Math.Tan(zeta45 - (fi / 2.0)), 2.0);
                Active_Horizontal_Force_end = (stress * Ka_S_end - cPrime * Math.Sqrt(Ka_S_end)) * Math.Cos(delta);
                Active_Vertical_Force_end = (stress * Ka_S_end - cPrime * Math.Sqrt(Ka_S_end)) * Math.Sin(delta);
            }
        }
        private static void Back_Rankine_Theory_Passive_Start(double delta, double stress, double cPrime, ref double Kp_S_start, ref double Passive_Vertical_Force_start, ref double Passive_Horizontal_Force_start, SoilData soil)
        {
            if (soil != null)
            {
                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double zeta45 = 45.0 * Math.PI / 180;
                Kp_S_start = Math.Pow(Math.Tan(zeta45 + (fi / 2.0)), 2.0);
                Passive_Horizontal_Force_start = (stress * Kp_S_start + cPrime * Math.Sqrt(Kp_S_start)) * Math.Cos(delta);
                Passive_Vertical_Force_start = (stress * Kp_S_start + cPrime * Math.Sqrt(Kp_S_start)) * Math.Sin(delta);
            }
        }
        private static void Back_Rankine_Theory_Passive_End(double delta, double stress, double cPrime, ref double Kp_S_end, ref double Passive_Vertical_Force_end, ref double Passive_Horizontal_Force_end, SoilData soil)
        {
            if (soil != null)
            {
                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double zeta45 = 45.0 * Math.PI / 180;
                Kp_S_end = Math.Pow(Math.Tan(zeta45 + (fi / 2.0)), 2.0);
                Passive_Horizontal_Force_end = (stress * Kp_S_end + cPrime * Math.Sqrt(Kp_S_end)) * Math.Cos(delta);
                Passive_Vertical_Force_end = (stress * Kp_S_end + cPrime * Math.Sqrt(Kp_S_end)) * Math.Sin(delta);
            }
        }
        private static void Back_Coloumb_Theory_Active_Start(double delta, double stress, double cPrime, double beta_back, ref double Ka_S_start, ref double Active_Vertical_Force_start, ref double Active_Horizontal_Force_start, SoilData soil)
        {
            if (soil != null)
            {

                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double Delta = soil.WallSoilFrictionAngle * Math.PI / 180;
                double alfa = 0 * Math.PI / 180;
                Ka_S_start = Math.Pow(Math.Cos(fi - alfa), 2.0) / (Math.Pow(Math.Cos(alfa), 2.0) * Math.Cos(alfa + Delta) * (Math.Pow(1 + Math.Sqrt((Math.Sin(fi + Delta) * Math.Sin(fi - beta_back)) / (Math.Cos(alfa + Delta) * Math.Cos(alfa - beta_back))), 2.0)));
                double Kahc = Math.Cos(fi) * Math.Cos(beta_back) * Math.Cos(Delta - alfa) * (1 + Math.Tan(-alfa) * Math.Tan(beta_back)) / (1 + Math.Sin(fi + Delta - alfa - beta_back));
                double Kac = Kahc / Math.Cos(Delta + alfa);
                Active_Horizontal_Force_start = (stress * Ka_S_start - 2 * cPrime * Kac) * Math.Cos(delta);
                Active_Vertical_Force_start = (stress * Ka_S_start - 2 * cPrime * Kac) * Math.Sin(delta);
            }
        }
        private static void Back_Coloumb_Theory_Active_End(double delta, double stress, double cPrime, double beta_back, ref double Ka_S_end, ref double Active_Vertical_Force_end, ref double Active_Horizontal_Force_end, SoilData soil)
        {
            if (soil != null)
            {

                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double Delta = soil.WallSoilFrictionAngle * Math.PI / 180;
                double alfa = 0 * Math.PI / 180;
                Ka_S_end = Math.Pow(Math.Cos(fi - alfa), 2.0) / (Math.Pow(Math.Cos(alfa), 2.0) * Math.Cos(alfa + Delta) * (Math.Pow(1 + Math.Sqrt((Math.Sin(fi + Delta) * Math.Sin(fi - beta_back)) / (Math.Cos(alfa + Delta) * Math.Cos(alfa - beta_back))), 2.0)));
                double Kahc = Math.Cos(fi) * Math.Cos(beta_back) * Math.Cos(Delta - alfa) * (1 + Math.Tan(-alfa) * Math.Tan(beta_back)) / (1 + Math.Sin(fi + Delta - alfa - beta_back));
                double Kac = Kahc / Math.Cos(Delta + alfa);
                Active_Horizontal_Force_end = (stress * Ka_S_end - 2 * cPrime * Kac) * Math.Cos(delta);
                Active_Vertical_Force_end = (stress * Ka_S_end - 2 * cPrime * Kac) * Math.Sin(delta);
            }
        }
        private static void Back_Coloumb_Theory_Passive_Start(double delta, double stress, double cPrime, double beta_back, ref double Kp_S_start, ref double Passive_Vertical_Force_start, ref double Passive_Horizontal_Force_start, SoilData soil)
        {
            if (soil != null)
            {
                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double Delta = soil.WallSoilFrictionAngle * Math.PI / 180;
                double alfa = 0 * Math.PI / 180;
                Kp_S_start =0.8 * Math.Pow(Math.Cos(fi + alfa), 2.0) / (Math.Pow(Math.Cos(alfa), 2.0) * Math.Cos(Delta - alfa) * (Math.Pow(1 - Math.Sqrt((Math.Sin(fi + Delta) * Math.Sin(fi + beta_back)) / (Math.Cos(Delta - alfa) * Math.Cos(beta_back - alfa))), 2.0)));
                //Kp_S_start = Math.Pow(Math.Cos(fi + alfa), 2.0) / (Math.Pow(Math.Cos(alfa), 2.0) * Math.Cos(Delta - alfa) * (Math.Pow(1 - Math.Sqrt((Math.Sin(fi + Delta) * Math.Sin(fi + beta_back)) / (Math.Cos(Delta - alfa) * Math.Cos(beta_back - alfa))), 2.0)));
                Passive_Horizontal_Force_start = (stress * Kp_S_start + 2 * cPrime * Math.Sqrt(Kp_S_start)) * Math.Cos(delta);
                Passive_Vertical_Force_start = (stress * Kp_S_start + 2 * cPrime * Math.Sqrt(Kp_S_start)) * Math.Sin(delta);
            }
        }
        private static void Back_Coloumb_Theory_Passive_End(double delta, double stress, double cPrime, double beta_back, ref double Kp_S_end, ref double Passive_Vertical_Force_end, ref double Passive_Horizontal_Force_end, SoilData soil)
        {
            if (soil != null)
            {
                double fi = soil.SoilFrictionAngle * Math.PI / 180;
                double Delta = soil.WallSoilFrictionAngle * Math.PI / 180;
                double alfa = 0 * Math.PI / 180;
                Kp_S_end = 0.8 * Math.Pow(Math.Cos(fi + alfa), 2.0) / (Math.Pow(Math.Cos(alfa), 2.0) * Math.Cos(Delta - alfa) * (Math.Pow(1 - Math.Sqrt((Math.Sin(fi + Delta) * Math.Sin(fi + beta_back)) / (Math.Cos(Delta - alfa) * Math.Cos(beta_back - alfa))), 2.0)));
                //Kp_S_end = Math.Pow(Math.Cos(fi + alfa), 2.0) / (Math.Pow(Math.Cos(alfa), 2.0) * Math.Cos(Delta - alfa) * (Math.Pow(1 - Math.Sqrt((Math.Sin(fi + Delta) * Math.Sin(fi + beta_back)) / (Math.Cos(Delta - alfa) * Math.Cos(beta_back - alfa))), 2.0)));
                Passive_Horizontal_Force_end = (stress * Kp_S_end + 2 * cPrime * Math.Sqrt(Kp_S_end)) * Math.Cos(delta);
                Passive_Vertical_Force_end = (stress * Kp_S_end + 2 * cPrime * Math.Sqrt(Kp_S_end)) * Math.Sin(delta);
            }
        }
        private static void Back_TotalStress_Theory_Active_Start(double stress, ref double Ka_S_start, ref double Active_Vertical_Force_start, ref double Active_Horizontal_Force_start, SoilData soil)
        {
            if (soil != null)
            {
                double cu = soil.UndrainedShearStrength;
                double au = soil.WallSoilAdhesion;
                Ka_S_start = 2.0 * Math.Sqrt(1 + au / cu);
                Active_Horizontal_Force_start = stress - Ka_S_start * cu;
                Active_Vertical_Force_start = 0;
            }
        }
        private static void Back_TotalStress_Theory_Active_End(double stress, ref double Ka_S_end, ref double Active_Vertical_Force_end, ref double Active_Horizontal_Force_end, SoilData soil)
        {
            if (soil != null)
            {
                double cu = soil.UndrainedShearStrength;
                double au = soil.WallSoilAdhesion;
                Ka_S_end = 2.0 * Math.Sqrt(1 + au / cu);
                Active_Horizontal_Force_end = stress - Ka_S_end * cu;
                Active_Vertical_Force_end = 0;
            }
        }
        private static void Back_TotalStress_Theory_Passive_Start(double stress, ref double Kp_S_start, ref double Passive_Vertical_Force_start, ref double Passive_Horizontal_Force_start, SoilData soil)
        {
            if (soil != null)
            {
                double cu = soil.UndrainedShearStrength;
                double au = soil.WallSoilAdhesion;
                Kp_S_start = -2.0 * Math.Sqrt(1 + au / cu);
                Passive_Horizontal_Force_start = stress - Kp_S_start * cu;
                Passive_Vertical_Force_start = 0;
            }
        }
        private static void Back_TotalStress_Theory_Passive_End(double stress, ref double Kp_S_end, ref double Passive_Vertical_Force_end, ref double Passive_Horizontal_Force_end, SoilData soil)
        {
            if (soil != null)
            {
                double cu = soil.UndrainedShearStrength;
                double au = soil.WallSoilAdhesion;
                Kp_S_end = -2.0 * Math.Sqrt(1 + au / cu);
                Passive_Horizontal_Force_end = stress - Kp_S_end * cu;
                Passive_Vertical_Force_end = 0;
            }
        }
        private static void Back_Mazindrani_Theory_Active_Start(double exH, double delta, double stress, double frameLength, double startLength, double beta_back, double cPrime, ref double Ka_S_start, ref double Active_Vertical_Force_start, ref double Active_Horizontal_Force_start, SoilData soil)
        {
            double fi = soil.SoilFrictionAngle * Math.PI / 180;
            double gama = soil.NaturalUnitWeight;
            startLength -= exH;
            //start node
            if (startLength <= 0) startLength += frameLength;
            Ka_S_start = (1 / Math.Pow(Math.Cos(fi), 2.0)) *
                (2.0 * Math.Pow(Math.Cos(beta_back), 2.0) + 2.0 * (cPrime / (gama * startLength)) * Math.Cos(fi) * Math.Sin(fi) - Math.Sqrt((4.0 * Math.Pow(Math.Cos(beta_back), 2.0) * (Math.Pow(Math.Cos(beta_back), 2.0) - Math.Pow(Math.Cos(fi), 2.0))) + (4.0 * Math.Pow((cPrime / (gama * startLength)), 2.0) * Math.Pow(Math.Cos(fi), 2.0)) + (8.0 * (cPrime / (gama * startLength)) * Math.Pow(Math.Cos(beta_back), 2.0) * Math.Sin(fi) * Math.Cos(fi)))) - 1.0;
            Active_Horizontal_Force_start = stress * Ka_S_start * Math.Cos(delta);
            Active_Vertical_Force_start = stress * Ka_S_start * Math.Sin(delta);

        }
        private static void Back_Mazindrani_Theory_Active_End(double exH, double delta, double stress, double endLength, double beta_back, double cPrime, ref double Ka_S_end, ref double Active_Vertical_Force_end, ref double Active_Horizontal_Force_end, SoilData soil)
        {
            double fi = soil.SoilFrictionAngle * Math.PI / 180;
            double gama = soil.NaturalUnitWeight;
            endLength -= exH;
            //end node
            Ka_S_end = (1 / Math.Pow(Math.Cos(fi), 2.0)) *
                (2.0 * Math.Pow(Math.Cos(beta_back), 2.0) + 2.0 * (cPrime / (gama * endLength)) * Math.Cos(fi) * Math.Sin(fi) - Math.Sqrt((4.0 * Math.Pow(Math.Cos(beta_back), 2.0) * (Math.Pow(Math.Cos(beta_back), 2.0) - Math.Pow(Math.Cos(fi), 2.0))) + (4.0 * Math.Pow((cPrime / (gama * endLength)), 2.0) * Math.Pow(Math.Cos(fi), 2.0)) + (8.0 * (cPrime / (gama * endLength)) * Math.Pow(Math.Cos(beta_back), 2.0) * Math.Sin(fi) * Math.Cos(fi)))) - 1.0;
            Active_Horizontal_Force_end = stress * Ka_S_end * Math.Cos(delta);
            Active_Vertical_Force_end = stress * Ka_S_end * Math.Sin(delta);
        }
        private static void Back_Mazindrani_Theory_Passive_Start(double exH, double delta, double stress, double frameLength, double startLength, double beta_back, double cPrime, ref double Kp_S_start, ref double Passive_Vertical_Force_start, ref double Passive_Horizontal_Force_start, SoilData soil)
        {
            double fi = soil.SoilFrictionAngle * Math.PI / 180;
            double gama = soil.NaturalUnitWeight;
            startLength -= exH;
            //start node
            if (startLength <= 0) startLength += frameLength;
            Kp_S_start = (1 / Math.Pow(Math.Cos(fi), 2.0)) *
                (2.0 * Math.Pow(Math.Cos(beta_back), 2.0) + 2.0 * (cPrime / (gama * startLength)) * Math.Cos(fi) * Math.Sin(fi) + Math.Sqrt((4.0 * Math.Pow(Math.Cos(beta_back), 2.0) * (Math.Pow(Math.Cos(beta_back), 2.0) - Math.Pow(Math.Cos(fi), 2.0))) + (4.0 * Math.Pow((cPrime / (gama * startLength)), 2.0) * Math.Pow(Math.Cos(fi), 2.0)) + (8.0 * (cPrime / (gama * startLength)) * Math.Pow(Math.Cos(beta_back), 2.0) * Math.Sin(fi) * Math.Cos(fi)))) - 1.0;
            Passive_Horizontal_Force_start = stress * Kp_S_start * Math.Cos(delta);
            Passive_Vertical_Force_start = stress * Kp_S_start * Math.Sin(delta);
        }
        private static void Back_Mazindrani_Theory_Passive_End(double exH, double delta, double stress, double endLength, double beta_back, double cPrime, ref double Kp_S_end, ref double Passive_Vertical_Force_end, ref double Passive_Horizontal_Force_end, SoilData soil)
        {
            double fi = soil.SoilFrictionAngle * Math.PI / 180;
            double gama = soil.NaturalUnitWeight;
            endLength -= exH;
            //end node
            Kp_S_end = (1 / Math.Pow(Math.Cos(fi), 2.0)) *
                (2.0 * Math.Pow(Math.Cos(beta_back), 2.0) + 2.0 * (cPrime / (gama * endLength)) * Math.Cos(fi) * Math.Sin(fi) + Math.Sqrt((4.0 * Math.Pow(Math.Cos(beta_back), 2.0) * (Math.Pow(Math.Cos(beta_back), 2.0) - Math.Pow(Math.Cos(fi), 2.0))) + (4.0 * Math.Pow((cPrime / (gama * endLength)), 2.0) * Math.Pow(Math.Cos(fi), 2.0)) + (8.0 * (cPrime / (gama * endLength)) * Math.Pow(Math.Cos(beta_back), 2.0) * Math.Sin(fi) * Math.Cos(fi)))) - 1.0;
            Passive_Horizontal_Force_end = stress * Kp_S_end * Math.Cos(delta);
            Passive_Vertical_Force_end = stress * Kp_S_end * Math.Sin(delta);
        }
        private static void FrameToNodeForce()
        {
            NodeData.Nodes.Clear();

            for (int i = 0; i < FrameData.Frames.Count; i++)
            {

                if (i == 0)
                {
                    NodeData nodeStart = new NodeData(FrameData.Frames[i].StartPoint);
                    foreach (var load in FrameData.Frames[i].startNodeLoadAndForce)
                    {
                        nodeStart.AddForce(load);

                    }
                    NodeData nodeEnd = new NodeData(FrameData.Frames[i].EndPoint);
                    for (int j = 0; j < FrameData.Frames[i].startNodeLoadAndForce.Count; j++)
                    {
                        Tuple<Load, double, double> tuple = new Tuple<Load, double, double>(
                            FrameData.Frames[i].startNodeLoadAndForce[j].Item1,
                            0,
                            FrameData.Frames[i].endNodeLoadAndForce[j].Item3 + FrameData.Frames[i + 1].startNodeLoadAndForce[j].Item3
                            );
                        nodeEnd.AddForce(tuple);
                    }

                }
                else if (i == FrameData.Frames.Count - 1)
                {
                    NodeData nodeEnd = new NodeData(FrameData.Frames[i].EndPoint);
                    foreach (var load in FrameData.Frames[i].endNodeLoadAndForce)
                    {
                        nodeEnd.AddForce(load);
                    }

                }
                else
                {
                    NodeData nodeMid = new NodeData(FrameData.Frames[i].EndPoint);

                    for (int j = 0; j < FrameData.Frames[i].startNodeLoadAndForce.Count; j++)
                    {
                        if (FrameData.Frames[i].startNodeLoadAndForce[j].Item1.Type != FrameData.Frames[i + 1].startNodeLoadAndForce[j].Item1.Type)
                        {
                            MessageBox.Show("farklı");
                        }
                        Tuple<Load, double, double> tuple = new Tuple<Load, double, double>(
                            FrameData.Frames[i].startNodeLoadAndForce[j].Item1,
                            0,
                            FrameData.Frames[i].endNodeLoadAndForce[j].Item3 + FrameData.Frames[i + 1].startNodeLoadAndForce[j].Item3
                            );
                        nodeMid.AddForce(tuple);
                    }
                }
            }
            NodeData.Nodes.Sort();
            NodeData.Nodes.Reverse();
        }

        /// <summary>
        /// 1. back force
        /// 2. front force
        /// 3. total force
        /// 4. springs
        /// </summary>
        /// <param name="DisplacementID"></param>
        /// <param name="SpringID"></param>
        /// <param name="BackForceID"></param>
        /// <param name="FrontForceID"></param>
        /// <param name="isPlusedSpring"></param>
        /// <param name="isUsedFirstLoad"></param>
        /// <returns></returns>
        private static Tuple<Force, Force, Force> IterationForceFrameToNodeForce(Guid DisplacementID, Guid BackForceID, Guid FrontForceID, bool isBackForceStartWithK0 = false, bool isSpringOpenWithK0 = true)
        {
            Force Back_Iteration_Total_Node_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Back_First_Total_Force };
            Force Front_Iteration_Total_Node_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.Front_First_Total_Force };
            Force Iteration_Total_Node_Force = new Force() { ID = Guid.NewGuid(), Type = LoadType.First_Total_Force };

            foreach (var node in NodeData.Nodes)
            {
                double Back_Iteration_Total_Force = 0;
                double Front_Iteration_Total_Force = 0;
                double Iteration_Total_Force = 0;

                Tuple<Load, double> BackSpring = node.nodeForce.Find(x => x.Item1.Type == LoadType.Back_SubgradeModulusofSoil);
                Tuple<Load, double> Displacement = node.nodeForce.Find(x => x.Item1.ID == DisplacementID);

                double Back_Force_amp = BackSpring.Item2 * (Displacement!= null ? Displacement.Item2 : 0);
                
                Tuple<Load, double> BackK0_Force = node.nodeForce.Find(x => x.Item1.Type == LoadType.Back_Rest_Horizontal_Force);
                Tuple<Load, double> BackKp_Force = node.nodeForce.Find(x => x.Item1.Type == LoadType.Back_Passive_Horizontal_Force);
                Tuple<Load, double> BackKa_Force = node.nodeForce.Find(x => x.Item1.Type == LoadType.Back_Active_Horizontal_Force);
                double BackK0_Force_amp = 0;
                if (isBackForceStartWithK0)
                {
                    BackK0_Force_amp = BackK0_Force.Item2 - Back_Force_amp;
                }
                else
                {
                    BackK0_Force_amp = BackKa_Force.Item2 - Back_Force_amp;
                }
                //amp = ka ka = 0.1 k0 = 0.5 kp = 3
                
                if (BackK0_Force_amp >= BackKp_Force.Item2)
                {
                    BackK0_Force_amp = BackKp_Force.Item2;
                }
                else
                {
                    if (BackK0_Force_amp <= BackKa_Force.Item2)
                    {
                        BackK0_Force_amp = BackKa_Force.Item2;
                    }
                }
                if (BackK0_Force_amp >= BackKp_Force.Item2)
                {
                    node.isBackSpringOn = false;

                }
                if (BackK0_Force_amp <= BackKa_Force.Item2)
                {
                    node.isBackSpringOn = false;
                }
                if(isSpringOpenWithK0)
                {
                    if (BackK0_Force_amp > BackK0_Force.Item2 && BackK0_Force_amp < BackKp_Force.Item2)
                    {
                        node.isBackSpringOn = true;
                    }
                }
                else
                {
                    if (BackK0_Force_amp > BackKa_Force.Item2 && BackK0_Force_amp < BackKp_Force.Item2)
                    {
                        node.isBackSpringOn = true;
                    }
                }
                

                Back_Iteration_Total_Force = BackK0_Force_amp;
                foreach (var nodeForce in node.nodeForce)
                {
                    switch (nodeForce.Item1.Type)
                    {
                        case LoadType.StripLoad:
                            Back_Iteration_Total_Force += nodeForce.Item2;
                            break;
                        case LoadType.LineLoad:
                            Back_Iteration_Total_Force += nodeForce.Item2;
                            break;
                        case LoadType.PointLoad:
                            Back_Iteration_Total_Force += nodeForce.Item2;
                            break;
                        case LoadType.HydroStaticWaterPressure:
                            Back_Iteration_Total_Force += nodeForce.Item2;
                            break;
                        default:
                            break;
                    }
                }

                Tuple<Load, double> FrontSpring = node.nodeForce.Find(x => x.Item1.Type == LoadType.Front_SubgradeModulusofSoil);
                double Front_Force_amp = FrontSpring.Item2 * (Displacement != null ? Displacement.Item2 : 0);

                Tuple<Load, double> FrontK0_Force = node.nodeForce.Find(x => x.Item1.Type == LoadType.Front_Rest_Horizontal_Force);
                
                double FrontK0_Force_amp = FrontK0_Force.Item2 - Front_Force_amp;
                Tuple<Load, double> FrontKp_Force = node.nodeForce.Find(x => x.Item1.Type == LoadType.Front_Passive_Horizontal_Force);
                Tuple<Load, double> FrontKa_Force = node.nodeForce.Find(x => x.Item1.Type == LoadType.Front_Active_Horizontal_Force);
                //amp = -4 ka = -0.1 k0 = -0.5 kp = -3
                if (FrontK0_Force_amp >= FrontKa_Force.Item2)
                {
                    FrontK0_Force_amp = FrontKa_Force.Item2;
                }
                else
                {
                    if (FrontK0_Force_amp <= FrontKp_Force.Item2)
                    {
                        FrontK0_Force_amp = FrontKp_Force.Item2;
                    }
                }
                if (FrontK0_Force_amp >= FrontKa_Force.Item2)
                {
                    node.isFrontSpringOn = false;
                }
                if (FrontK0_Force_amp <= FrontKp_Force.Item2)
                {
                    node.isFrontSpringOn = false;
                }
                if (FrontK0_Force_amp > FrontKp_Force.Item2 && FrontK0_Force_amp < FrontKa_Force.Item2)
                {
                    node.isFrontSpringOn = true;
                }
                Front_Iteration_Total_Force = FrontK0_Force_amp;
                node.nodeForce.Add(new Tuple<Load, double>(Back_Iteration_Total_Node_Force, Back_Iteration_Total_Force));
                node.nodeForce.Add(new Tuple<Load, double>(Front_Iteration_Total_Node_Force, Front_Iteration_Total_Force));

                Iteration_Total_Force = Back_Iteration_Total_Force + Front_Iteration_Total_Force;
                node.nodeForce.Add(new Tuple<Load, double>(Iteration_Total_Node_Force, Iteration_Total_Force));
            }

            
            
            
            return new Tuple<Force, Force, Force>(Back_Iteration_Total_Node_Force, Front_Iteration_Total_Node_Force, Iteration_Total_Node_Force);
        }
        /// <summary>
        /// isplusedspring alıcak
        /// </summary>
        /// <param name="isPlusedSpring"></param>
        /// <returns></returns>
        private static Force IterationSpringFixing()
        {
            Force AnalysSprings = new Force() { ID = Guid.NewGuid(), Type = LoadType.Analys_SubgradeModulusofSoil };
            
            foreach (var node in NodeData.Nodes)
            {
                if (node.isFrontSpringOn && node.isBackSpringOn)
                {
                    Tuple<Load, double> backSpring = node.nodeForce.Find(x => x.Item1.Type == LoadType.Back_SubgradeModulusofSoil);
                    Tuple<Load, double> frontSpring = node.nodeForce.Find(x => x.Item1.Type == LoadType.Front_SubgradeModulusofSoil);
                    
                        node.nodeForce.Add(new Tuple<Load, double>(AnalysSprings, backSpring.Item2 + frontSpring.Item2));
                    
                }
                else if (node.isFrontSpringOn && !node.isBackSpringOn)
                {
                    Tuple<Load, double> frontSpring = node.nodeForce.Find(x => x.Item1.Type == LoadType.Front_SubgradeModulusofSoil);

                    node.nodeForce.Add(new Tuple<Load, double>(AnalysSprings, frontSpring.Item2));
                }
                else if (!node.isFrontSpringOn && node.isBackSpringOn)
                {
                    Tuple<Load, double> backSpring = node.nodeForce.Find(x => x.Item1.Type == LoadType.Back_SubgradeModulusofSoil);

                    node.nodeForce.Add(new Tuple<Load, double>(AnalysSprings, backSpring.Item2));
                }
                else
                {
                    node.nodeForce.Add(new Tuple<Load, double>(AnalysSprings, 0.0));
                }
            }
            return AnalysSprings;
        }

        /// <summary>
        /// 1. rotation
        /// 2. displacement
        /// </summary>
        /// <param name="TotalForceID"></param>
        /// <param name="SpringID"></param>
        /// <returns></returns>
        private static Tuple<Force,Force> IterationMatrixAnalys(Guid TotalForceID,Guid SpringID)
        {
            int m = NodeData.Nodes.Count * 2;
            int n = NodeData.Nodes.Count * 3 - 2;

            var matrixA = new DenseMatrix(m, n);
            for (int i = 0; i < NodeData.Nodes.Count; i++)
            {
                if (i < NodeData.Nodes.Count - 1)
                {
                    matrixA[i, 2 * i] = 1;
                }

                if (i > 0)
                {
                    matrixA[i, 2 * i - 1] = 1;
                }
            }
            for (int j = NodeData.Nodes.Count; j < m; j++)
            {
                matrixA[j, j + NodeData.Nodes.Count - 2] = -1;
            }

            for (int j = 0; j < NodeData.Nodes.Count - 1; j++)
            {
                double frameLength = Math.Sqrt((Math.Pow(FrameData.Frames[j].StartPoint.X - FrameData.Frames[j].EndPoint.X, 2) + Math.Pow(FrameData.Frames[j].StartPoint.Y - FrameData.Frames[j].EndPoint.Y, 2)));
                matrixA[j + NodeData.Nodes.Count, 2 * j] = 1 / frameLength;
                matrixA[j + NodeData.Nodes.Count, 2 * j + 1] = 1 / frameLength;
                matrixA[j + NodeData.Nodes.Count + 1, 2 * j] = -1 / frameLength;
                matrixA[j + NodeData.Nodes.Count + 1, 2 * j + 1] = -1 / frameLength;
            }

            var matrixS = new DenseMatrix(n, n);
            for (int j = 0; j < m - 2; j++)
            {
                int k = (int)Math.Round((decimal)(j / 2), 0, MidpointRounding.AwayFromZero);

                double frameLength = Math.Sqrt((Math.Pow(FrameData.Frames[k].StartPoint.X - FrameData.Frames[k].EndPoint.X, 2) + Math.Pow(FrameData.Frames[k].StartPoint.Y - FrameData.Frames[k].EndPoint.Y, 2)));

                var A = 4 * StaticVariables.viewModel.GetWallEI() / frameLength;
                var B = 2 * StaticVariables.viewModel.GetWallEI() / frameLength;
                matrixS[j, j] = A;
                if (j % 2 == 0)
                {
                    matrixS[j, j + 1] = B;
                    matrixS[j + 1, j] = B;
                }
            }

            for (int j = m - 2; j < n; j++)
            {
                Tuple<Load, double> tuple = NodeData.Nodes[j - m + 2].nodeForce.Find(x => x.Item1.ID == SpringID);
                matrixS[j, j] = tuple.Item2;
            }

            var matrixP = new DenseMatrix(m, 1);

            for (int i = NodeData.Nodes.Count; i < m; i++)
            {
                Tuple<Load, double> tuple = NodeData.Nodes[i - NodeData.Nodes.Count].nodeForce.Find(x => x.Item1.ID == TotalForceID);
                matrixP[i, 0] = tuple.Item2;
            }
            var matrixL = matrixA.Multiply(matrixS);

            var matrixM = matrixL.TransposeAndMultiply(matrixA);

            var matrixM_Inverse = matrixM.Inverse();

            var matrixU = matrixM_Inverse.Multiply(matrixP);

            Force rotation = new Force() { ID = Guid.NewGuid(), Type = LoadType.First_Rotation };
            Force displacement = new Force() { ID = Guid.NewGuid(), Type = LoadType.First_Displacement };
            for (int i = 0; i < matrixU.RowCount; i++)
            {
                if (i < NodeData.Nodes.Count)
                {
                    NodeData.Nodes[i].nodeForce.Add(new Tuple<Load, double>(rotation, matrixU[i, 0]));                    
                }
                else
                {
                    NodeData.Nodes[i - NodeData.Nodes.Count].nodeForce.Add(new Tuple<Load, double>(displacement, matrixU[i, 0]));
                }
            }
            return new Tuple<Force,Force>(rotation, displacement);
        }
                

    }
}
