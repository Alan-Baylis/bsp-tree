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

            }

            return result;
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

        public static double ScalarProduct(Point vec1, Point vec2)
        {
            return vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Z;
        }
    }
}
