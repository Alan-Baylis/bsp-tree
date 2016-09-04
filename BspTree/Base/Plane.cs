using BspTree.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BspTree.Base
{
    public class Plane
    {
        #region Fields
        private Polygon _polygon;
        #endregion

        #region Properties
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
        #endregion

        #region Methods
        public Point GetRandomPoint()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var multiplier = 1.0 / random.Next(2, 10);
            var vec1 = LocalMath.CreateVector(this.Points[0], this.Points[1]);
            var vec2 = LocalMath.CreateVector(this.Points[0], this.Points[2]);

            return new Point
            {
                X = this.Points[0].X + multiplier * (vec1.X + vec2.X),
                Y = this.Points[0].Y + multiplier * (vec1.Y + vec2.Y),
                Z = this.Points[0].Z + multiplier * (vec1.Z + vec2.Z)
            };
        }

        public Polygon GetPolygon()
        {
            this._polygon = new Polygon();
            this._polygon.Points.Add(new System.Windows.Point(this.Points[0].X, this.Points[0].Y));
            this._polygon.Points.Add(new System.Windows.Point(this.Points[1].X, this.Points[1].Y));
            this._polygon.Points.Add(new System.Windows.Point(this.Points[2].X, this.Points[2].Y));

            this._polygon.Stroke = Brushes.Black;
            this._polygon.Fill = Brushes.Red;

            return this._polygon;
        }
        #endregion
    }
}
