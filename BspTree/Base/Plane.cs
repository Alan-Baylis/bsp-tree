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
        public List<Point> Points { get; set; }

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

        public Point GetRandomPoint()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var multiplier = 1 / random.Next(2, 100);
            var vec1 = LocalMath.CreateVector(this.Points[0], this.Points[1]);
            var vec2 = LocalMath.CreateVector(this.Points[0], this.Points[2]);

            return new Point
            {
                X = this.Points[0].X + multiplier * vec1.X + multiplier * vec2.X,
                Y = this.Points[0].Y + multiplier * vec1.Y + multiplier * vec2.Y,
                Z = this.Points[0].Z + multiplier * vec1.Z + multiplier * vec2.Z
            };
        }
    }
}
