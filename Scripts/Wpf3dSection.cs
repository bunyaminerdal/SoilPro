﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace ExDesign.Scripts
{
    internal class Wpf3dSection
    {
        private Point3D origin;
        private double width_top;
        private double width_bottom;
        private double height;
        private double depth;
        private double width_top_distance;
        private double width_bottom_distance;
        public Wpf3dSection(Point3D P0,double soilHeight, double w_top, double w_bottom, double h, double d ,double w_top_dis,double w_bottom_dis)
        {
            width_top = w_top;
            width_bottom = w_bottom;
            height = h;
            depth = d;
            width_top_distance = w_top_dis;
            width_bottom_distance = w_bottom_dis;
            origin = P0;
        }
        public WpfRectangle Front()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X+width_top_distance,origin.Y,origin.Z),
                                                new Point3D(origin.X + width_top_distance+width_top,origin.Y,origin.Z),
                                                new Point3D(origin.X + width_bottom_distance + width_bottom,origin.Y-height,origin.Z),
                                                new Point3D(origin.X+ width_bottom_distance,origin.Y-height,origin.Z));

            return r;
        }

        public WpfRectangle Back()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X + width_top_distance + width_top, origin.Y, origin.Z +depth),
                                                new Point3D(origin.X + width_top_distance, origin.Y, origin.Z + depth),
                                                new Point3D(origin.X + width_bottom_distance , origin.Y - height, origin.Z + depth),
                                                new Point3D(origin.X + width_bottom_distance + width_bottom, origin.Y - height, origin.Z + depth));

            return r;
        }

        public WpfRectangle Left()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X + width_top_distance, origin.Y, origin.Z+depth),
                                                new Point3D(origin.X + width_top_distance, origin.Y, origin.Z),
                                                new Point3D(origin.X + width_bottom_distance , origin.Y - height, origin.Z),
                                                new Point3D(origin.X + width_bottom_distance, origin.Y - height, origin.Z+depth));

            return r;
        }

        public WpfRectangle Right()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X + width_top_distance+width_top, origin.Y, origin.Z ),
                                                new Point3D(origin.X + width_top_distance + width_top, origin.Y, origin.Z + depth),
                                                new Point3D(origin.X + width_bottom_distance +width_bottom, origin.Y - height, origin.Z + depth),
                                                new Point3D(origin.X + width_bottom_distance + width_bottom, origin.Y - height, origin.Z ));

            return r;
        }

        public WpfRectangle Top()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X+width_top_distance,origin.Y,origin.Z), width_top, 0, depth);

            return r;
        }

        public WpfRectangle Bottom()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X + width_bottom+width_bottom_distance, origin.Y - height, origin.Z),
                -width_bottom, 0, depth);

            return r;
        }

        public static void addTrapezoidtoMesh(Point3D p0, double w_top, double w_bottom, double h, double d, double w_top_dis , double w_bottom_dis ,
            MeshGeometry3D mesh, bool useTexture)
        {
            WpfTrapezoid trapezoid = new WpfTrapezoid(p0, w_top,w_bottom, h, d,w_top_dis,w_bottom_dis);

            double maxDimension = Math.Max(d, Math.Max(w_top, h));

            PointCollection textureCoordinatesCollection = new PointCollection();

            WpfRectangle front = trapezoid.Front();
            WpfRectangle back = trapezoid.Back();
            WpfRectangle right = trapezoid.Right();
            WpfRectangle left = trapezoid.Left();
            WpfRectangle top = trapezoid.Top();
            WpfRectangle bottom = trapezoid.Bottom();

            if (useTexture)
            {
                Point3D extents = front.getDimensions();

                addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                                         extents.Y / maxDimension);
                extents = back.getDimensions();
                addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                                         extents.Y / maxDimension);
                extents = right.getDimensions();
                addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                                         extents.Y / maxDimension);
                extents = left.getDimensions();
                addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                                         extents.Y / maxDimension);
                extents = top.getDimensions();
                addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                                         extents.Y / maxDimension);
                extents = bottom.getDimensions();
                addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                                         extents.Y / maxDimension);
            }

            front.addToMesh(mesh);
            back.addToMesh(mesh);
            right.addToMesh(mesh);
            left.addToMesh(mesh);
            top.addToMesh(mesh);
            bottom.addToMesh(mesh);

            if (useTexture)
            {
                mesh.TextureCoordinates = textureCoordinatesCollection;
            }

        }

        private static void addTextureCoordinates(PointCollection textureCoordinatesCollection,
                            double xFactor, double yFactor)
        {
            textureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
            textureCoordinatesCollection.Add(new System.Windows.Point(xFactor, 0));
            textureCoordinatesCollection.Add(new System.Windows.Point(xFactor, yFactor));

            textureCoordinatesCollection.Add(new System.Windows.Point(xFactor, yFactor));
            textureCoordinatesCollection.Add(new System.Windows.Point(0, yFactor));
            textureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
        }

        public static GeometryModel3D CreateTrapezoidModel(Point3D p0, double w_top, double w_bottom, double h, double d, double w_top_dis, double w_bottom_dis, Color color,Uri uri,bool useTexture)
        {
            return CreateTrapezoidModel(p0, w_top,w_bottom, h, d,w_top_dis,w_bottom_dis, color, useTexture,uri);
        }
        public static GeometryModel3D CreateTrapezoidModel(Wpf3dSection section, Color color,bool useTexture,Uri uri)
        {
            return CreateTrapezoidModel(section.origin, section.width_top,section.width_bottom, section.height, section.depth,section.width_top_distance,section.width_bottom_distance, color, useTexture,uri);
        }
        public static GeometryModel3D CreateTrapezoidModel(Point3D p0, double w_top, double w_bottom, double h, double d, double w_top_dis , double w_bottom_dis , Color color, bool useTexture,Uri uri)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addTrapezoidtoMesh(p0, w_top,w_bottom, h, d,w_top_dis,w_bottom_dis, mesh, useTexture);
            Material material;
            Brush brush = new SolidColorBrush(color);
            if (useTexture)
            {
                var imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(uri);
                //imageBrush.Stretch = Stretch.None;
                brush = imageBrush;
            }
            

            material = new DiffuseMaterial(brush);

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            return model;
        }
    }
}
