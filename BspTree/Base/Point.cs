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
            //true if 0<=s<=1, 0<=t<=1, s + 1 <= 1
            double s = double.MinValue;
            double t = double.MinValue;

            if (points[1].X - points[0].X != 0)
            {
                var den1 = (points[0].X - points[2].X) / (points[1].X - points[0].X);
                if (den1 + (points[2].Y - points[0].Y)*(points[1].Y - points[0].Y) != 0)
                {
                    t = (this.Y - points[0].Y + (points[1].Y - points[0].Y * (this.X - points[0].X) / (points[1].X - points[0].X))) / (den1 + (points[2].Y - points[0].Y) * (points[1].Y - points[0].Y));
                }
                else //will hope that everything will be ok with Z
                {
                    t = (this.Z - points[0].Z + (points[1].Z - points[0].Z * (this.X - points[0].X) / (points[1].X - points[0].X))) / (den1 + (points[2].Z - points[0].Z) * (points[1].Z - points[0].Z));
                }

                s = (this.X - points[0].X - (points[2].X - points[0].X) * t) / (points[1].X - points[0].X);
            }
            else if (points[1].Y - points[0].Y != 0)
            {
                var den1 = (points[0].Y - points[2].Y) / (points[1].Y - points[0].Y);
                if (den1 + (points[2].X - points[0].X)* (points[1].X - points[0].X) != 0)
                {
                    t = (this.X - points[0].X + (points[1].X - points[0].X * (this.Y - points[0].Y) / (points[1].Y - points[0].Y))) / (den1 + (points[2].X - points[0].X) * (points[1].X - points[0].X));
                }
                else //will hope that everything will be ok with Z
                {
                    t = (this.Z - points[0].Z + (points[1].Z - points[0].Z * (this.Y - points[0].Y) / (points[1].Y - points[0].Y))) / (den1 + (points[2].Z - points[0].Z) * (points[1].Z - points[0].Z));
                }

                s = (this.Y - points[0].Y - (points[2].Y - points[0].Y) * t) / (points[1].Y - points[0].Y);
            }
            else if (points[1].Z - points[0].Z != 0)
            {
                var den1 = (points[0].Z - points[2].Z) / (points[1].Z - points[0].Z);
                if (den1 + (points[2].X - points[0].X) * (points[1].X - points[0].X) != 0)
                {
                    t = (this.X - points[0].X + (points[1].X - points[0].X * (this.Z - points[0].Z) / (points[1].Z - points[0].Z))) / (den1 + (points[2].X - points[0].X) * (points[1].X - points[0].X));
                }
                else //will hope that everything will be ok with y
                {
                    t = (this.Y - points[0].Y + (points[1].Y - points[0].Y * (this.Z - points[0].Z) / (points[1].Z - points[0].Z))) / (den1 + (points[2].Y - points[0].Y) * (points[1].Y - points[0].Y));
                }

                s = (this.Z - points[0].Z - (points[2].Z - points[0].Z) * t) / (points[1].Z - points[0].Z);
            }

            return s >= 0 && s <= 1 && t >= 0 && t <= 1 && s + t <= 1;
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
