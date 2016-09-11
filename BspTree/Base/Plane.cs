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
        public Point GetPointOnPlane()
        {
            var vec1 = LocalMath.CreateVector(this.Points[0], this.Points[1]);
            var vec2 = LocalMath.CreateVector(this.Points[0], this.Points[2]);

            double s = 1.0/2, t = 1.0/9;

            return new Point
            {
                X = this.Points[0].X + s * vec1.X + t * vec2.X,
                Y = this.Points[0].Y + s * vec1.Y + t * vec2.Y,
                Z = this.Points[0].Z + s * vec1.Z + t * vec2.Z
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
