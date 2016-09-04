using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspTree.Base
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Point
    {
        [JsonProperty("x")]
        public double X { get; set; }
        [JsonProperty("y")]
        public double Y { get; set; }
        [JsonProperty("z")]
        public double Z { get; set; }

        public bool IsBetween(Point p1, Point p2)
        {
            var result = true;

            if (p1.X > p2.X)
            {
                result &= p2.X <= this.X && p1.X >= this.X;
            }
            else
            {
                result &= p1.X <= this.X && p2.X >= this.X;
            }

            if (p1.Y > p2.Y)
            {
                result &= p2.Y <= this.Y && p1.Y >= this.Y;
            }
            else
            {
                result &= p1.Y <= this.Y && p2.Y >= this.Y;
            }

            if (p1.Z > p2.Z)
            {
                result &= p2.Z <= this.Z && p1.Z >= this.Z;
            }
            else
            {
                result &= p1.Z <= this.Z && p2.Z >= this.Z;
            }

            return result;
        }

        public bool IsInTriangle(params Point[] points)
        {
            #region Validation
            //if there is no bounds, then return true
            if (points == null)
                return true;

            if (!points.Any())
                return true;

            if (points.Length != 3)
            {
                throw new ArgumentException("For this method using you should pass triangle");
            }
            #endregion
            //using barycentric coordinates for this
            //p = p0 + (p1 - p0)*s + (p2 - p0)*t
            //a == p1 - p0, b == p2 - p0, c = p - p0
            //с = a*s + b*t
            //true if 0<=s<=1, 0<=t<=1, s + t <= 1
            double s = double.MinValue;
            double t = double.MinValue;

            var a = LocalMath.CreateVector(points[0], points[1]);
            var b = LocalMath.CreateVector(points[0], points[2]);
            var c = LocalMath.CreateVector(points[0], this);

            if (a.X != 0)
            {
                if (b.Y - (b.X/a.X) != 0)
                {
                    t = (c.Y - ((a.Y * c.X) / a.X)) / (b.Y - b.X / a.X);
                }
                else if (b.Z - (b.X/a.X) != 0)
                {
                    t = (c.Z - ((a.Z * c.X) / a.X)) / (b.Z - b.X / a.X);
                }
                
                s = (c.X - b.X * t) / a.X;
            }
            else if (a.Y != 0)
            {
                if (b.X - (b.Y / a.Y) != 0)
                {
                    t = (c.X - ((a.X * c.Y) / a.Y)) / (b.X - b.Y / a.Y);
                }
                else if (b.Z - (b.Y / a.Y) != 0)
                {
                    t = (c.Z - ((a.Z * c.Y) / a.Y)) / (b.Z - b.Y / a.Y);
                }

                s = (c.Y - b.Y * t) / a.Y;
            }
            else if (a.Z != 0)
            {
                if (b.X - (b.Z / a.Z) != 0)
                {
                    t = (c.X - ((a.X * c.Z) / a.Z)) / (b.X - b.Z / a.Z);
                }
                else if (b.Y - (b.Z / a.Z) != 0)
                {
                    t = (c.Y - ((a.Y * c.Z) / a.Z)) / (b.Y - b.Z / a.Z);
                }

                s = (c.Z - b.Z * t) / a.Z;
            }

            return s >= 0 && t >= 0 && s + t <= 1;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Point))
                return false;

            var p = (Point)obj;
            if (p.X == this.X && p.Y == this.Y && p.Z == this.Z)
                return true;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (int)this.X * 100 + (int)this.Y * 100 + (int)this.Z * 100;
        }

        public override string ToString()
        {
            return $"({this.X} ; {this.Y} ; {this.Z})";
        }
    }
}
