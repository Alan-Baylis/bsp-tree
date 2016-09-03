using BspTree.Base;
using BspTree.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspTree
{
    public static class LocalMath
    {
        /// <summary>
        /// Creates a definition of plane, which describes by a subset of parent plane
        /// </summary>
        public static Plane CreatePlaneFrom(Plane plane, Point p1, Point p2, Point p3)
        {
            var planePoints = new List<Point>();
            planePoints.Add(p1);
            planePoints.Add(p2);
            planePoints.Add(p3);

            return new Plane
            {
                Points = planePoints,
                NormVect = plane.NormVect
            };
        }

        /// <summary>
        /// Creates vector from two points
        /// </summary>
        public static Point CreateVector(Point p1, Point p2)
        {
            return new Point
            {
                X = p2.X - p1.X,
                Y = p2.Y - p1.Y,
                Z = p2.Z - p1.Z
            };
        }

        /// <summary>
        /// Returns a vector, that is result of vector multiplication of two input vectors
        /// </summary>
        public static Point VectorProduct(Point vec1, Point vec2)
        {
            var x = vec1.Y * vec2.Z - vec1.Z * vec2.Y;
            var y = vec1.Z * vec2.X - vec1.X * vec2.Z;
            var z = vec1.X * vec2.Y - vec1.Y * vec2.X;

            var max = new[] { Math.Abs(x), Math.Abs(y), Math.Abs(z) }.Max();
            if (max == 0)
                throw new ArgumentException();

            return new Point
            {
                X = x / max,
                Y = y / max,
                Z = z / max
            };
        }

        /// <summary>
        /// Returns a number, that is result of scalar multiplication of two input vectors
        /// </summary>
        public static double ScalarProduct(Point vec1, Point vec2)
        {
            return vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Z;
        }
    }
}
