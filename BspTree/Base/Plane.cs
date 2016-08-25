using BspTree.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspTree.Base
{
    public class Plane
    {
        public List<PointImport> Points { get; set; }

        public Point NormVect { get; set; }
        public double D
        {
            get
            {
                return -this.NormVect.X * this.Points[0].X
                    - this.NormVect.Y * this.Points[0].Y
                    - this.NormVect.Z * this.Points[0].Z;
            }
        }

        public bool IsParallelOxy
        {
            get
            {
                var z = this.Points.First().Z;
                return this.Points.TrueForAll(point => point.Z == z);
            }
        }

        public bool IsParallelOxz
        {
            get
            {
                var y = this.Points.First().Y;
                return this.Points.TrueForAll(point => point.Y == y);
            }
        }

        public bool IsParallelOyz
        {
            get
            {
                var x = this.Points.First().X;
                return this.Points.TrueForAll(point => point.X == x);
            }
        }

        public Point GetRandomPoint()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var multiplier = 1 / random.Next(2, 100);
            var vec1 = CreateVector(this.Points[0], this.Points[1]);
            var vec2 = CreateVector(this.Points[0], this.Points[2]);

            return new Point
            {
                X = this.Points[0].X + multiplier * vec1.X + multiplier * vec2.X,
                Y = this.Points[0].Y + multiplier * vec1.Y + multiplier * vec2.Y,
                Z = this.Points[0].Z + multiplier * vec1.Z + multiplier * vec2.Z
            };
        }

        public static Point CreateVector(Point p1, Point p2)
        {
            return new Point
            {
                X = p2.X - p1.X,
                Y = p2.Y - p1.Y,
                Z = p2.Z - p1.Z
            };
        }

        public static Plane CreatePlaneFrom(Plane plane, Point p1, Point p2, Point p3)
        {
            var planePoints = new List<Point>();
            planePoints.Add(p1);
            planePoints.Add(p2);
            planePoints.Add(p3);

            return new Plane
            {
                Points = planePoints.OfType<PointImport>().ToList(),
                NormVect = plane.NormVect
            };
        }
    }
}
