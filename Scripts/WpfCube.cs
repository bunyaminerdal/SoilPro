﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace ExDesign.Scripts
{
    internal class WpfCube
    {
        private Point3D origin;
        private double width;
        private double height;
        private double depth;

        public Point3D centerBottom()
        {
            Point3D c = new Point3D(
                origin.X + (width / 2),
                origin.Y + height,
                origin.Z + (depth / 2)
                );

            return c;
        }

        public Point3D centerTop()
        {
            Point3D c = new Point3D(
                origin.X + (width / 2),
                origin.Y,
                origin.Z + (depth / 2)
                );

            return c;
        }

        public WpfCube(Point3D P0, double w, double h, double d)
        {
            width = w;
            height = h;
            depth = d;

            origin = P0;
        }
        
        public WpfCube(WpfCube cube)
        {
            width = cube.width;
            height = cube.height;
            depth = cube.depth;

            origin = new Point3D(cube.origin.X, cube.origin.Y, cube.origin.Z);
        }

        public WpfRectangle Front()
        {
            WpfRectangle r = new WpfRectangle(origin, width, height, 0);

            return r;
        }

        public WpfRectangle Back()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X + width, origin.Y, origin.Z + depth), -width, height, 0);

            return r;
        }

        public WpfRectangle Left()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X, origin.Y, origin.Z + depth),
                0, height, -depth);

            return r;
        }

        public WpfRectangle Right()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X + width, origin.Y, origin.Z),
                0, height, depth);

            return r;
        }

        public WpfRectangle Top()
        {
            WpfRectangle r = new WpfRectangle(origin, width, 0, depth);

            return r;
        }

        public WpfRectangle Bottom()
        {
            WpfRectangle r = new WpfRectangle(new Point3D(origin.X + width, origin.Y - height, origin.Z),
                -width, 0, depth);

            return r;
        }

        public static void addCubeToMesh(Point3D p0, double w, double h, double d,
            MeshGeometry3D mesh, bool useTexture)
        {
            WpfCube cube = new WpfCube(p0, w, h, d);

            double maxDimension = Math.Max(d, Math.Max(w, h));

            PointCollection textureCoordinatesCollection = new PointCollection();

            WpfRectangle front = cube.Front();
            WpfRectangle back = cube.Back();
            WpfRectangle right = cube.Right();
            WpfRectangle left = cube.Left();
            WpfRectangle top = cube.Top();
            WpfRectangle bottom = cube.Bottom();

            if (useTexture)
            {
                Point3D extents;
                //= front.getDimensions();

                //addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                //                         extents.Y / maxDimension);
                //extents = back.getDimensions();
                //addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                //                         extents.Y / maxDimension);
                extents = right.getDimensions();
                addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                                         extents.Y / maxDimension);
                extents = left.getDimensions();
                addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                                         extents.Y / maxDimension);
                //extents = top.getDimensions();
                //addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                //                         extents.Y / maxDimension);
                //extents = bottom.getDimensions();
                //addTextureCoordinates(textureCoordinatesCollection, extents.X / maxDimension,
                //                         extents.Y / maxDimension);
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

        
        public static GeometryModel3D CreateCubeModel(WpfCube cube, Color color,bool useTexture,Uri uri)
        {
            return CreateCubeModel(cube.origin, cube.width, cube.height, cube.depth, color, useTexture,uri);
        }
        public static GeometryModel3D CreateCubeModel(Point3D p0, double w, double h, double d, Color color, bool useTexture,Uri uri)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addCubeToMesh(p0, w, h, d, mesh, useTexture);
            
            Material material;
            Brush brush = new SolidColorBrush(color);
            if (useTexture)
            {
                var imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(uri);
                imageBrush.TileMode = TileMode.Tile;
                imageBrush.Viewport = new System.Windows.Rect(0, 0, imageBrush.ImageSource.Width/6/w, imageBrush.ImageSource.Width /6/w);
                imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                //imageBrush.Stretch = Stretch.None;
                brush = imageBrush;
            }
            
            material = new DiffuseMaterial(brush);
            GeometryModel3D model = new GeometryModel3D(mesh,material);

            return model;
        }
    }
}
