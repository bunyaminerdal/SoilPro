using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ExDesign.Scripts
{
    internal class ScreenSortingHelper
    {
        /// <summary>
        /// Sort Modelgroups in Farthest to Closest order, to enable transparency
        /// Should be applied whenever the scene is significantly re-oriented
        /// </summary>
        public static void AlphaSort(Point3D CameraPosition, Model3DCollection Models, Transform3D WorldTransform)
        {
            ArrayList list = new ArrayList();
            foreach (Model3D model in Models)
            {
                double distance = (Point3D.Subtract(CameraPosition, WorldTransform.Transform(model.Bounds.Location))).Length;
                list.Add(new ModelDistance(distance, model));
            }
            list.Sort(new DistanceComparer(SortDirection.FarToNear));
            Models.Clear();
            foreach (ModelDistance modelDistance in list)
            {
                Models.Add(modelDistance.model);
            }
        }

        private class ModelDistance
        {
            public ModelDistance(double distance, Model3D model)
            {
                this.distance = distance;
                this.model = model;
            }

            public double distance;
            public Model3D model;
        }

        private enum SortDirection
        {
            NearToFar,
            FarToNear
        }

        private class DistanceComparer : IComparer
        {
            public DistanceComparer(SortDirection sortDirection)
            {
                _sortDirection = sortDirection;
            }

            int IComparer.Compare(Object o1, Object o2)
            {
                double x1 = ((ModelDistance)o1).distance;
                double x2 = ((ModelDistance)o2).distance;
                if (_sortDirection == SortDirection.NearToFar)
                {
                    return (int)(x1 - x2);
                }
                else
                {
                    return (int)(-(x1 - x2));
                }
            }

            private SortDirection _sortDirection;
        }

    }
}
