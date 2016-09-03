using BspTree.Base;
using BspTree.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspTree
{
    public class Tree
    {
        public Plane Plane { get; set; }
        public Tree Left { get; set; }
        public Tree Right { get; set; }
        public bool IsEmpty { get; set; }

        public bool Contains(System.Windows.Point point)
        {
            //setting x and y in plane equation.
            //return true if z exist
            var z = -(this.Plane.NormVect.X * point.X + this.Plane.NormVect.Y * point.Y + this.Plane.D) / this.Plane.NormVect.Z;
            z = Math.Round(z, 2);

            var result = this.Plane.NormVect.X * point.X + this.Plane.NormVect.Y * point.Y + this.Plane.NormVect.Z * z + this.Plane.D;

            var p = new Point { X = point.X, Y = point.Y, Z = z };

            if (Math.Round(result, 2) == 0)
            {
                return p.IsInBounds(this.Plane.Points.ToArray());
            }

            var contains = false;
            if (this.Left != null)
            {
                contains |= this.Left.Contains(point);
            }

            if (this.Right != null)
            {
                contains |= this.Right.Contains(point);
            }

            return contains;
        }

        public void Rotate(WorkAxis axis, double angle)
        {
            var rotationMatrix = Geometry.CreateRotationMatrix(axis, angle);
            this.Plane.NormVect = Geometry.MultipleOnMatrix(this.Plane.NormVect, rotationMatrix);

            for (int i = 0; i < this.Plane.Points.Count; i++)
            {
                this.Plane.Points[i] = Geometry.MultipleOnMatrix(this.Plane.Points[i], rotationMatrix);
            }

            if (this.Left != null)
            {
                this.Left.Rotate(axis, angle);
            }

            if (this.Right != null)
            {
                this.Right.Rotate(axis, angle);
            }
        }

        public void Rotate(WorkAxis axis, double shift, double angle)
        {
            var rotationMatrix = Geometry.CreateRotationMatrix(axis, angle);
            this.Plane.NormVect = Geometry.MultipleOnMatrix(this.Plane.NormVect, rotationMatrix);

            for (int i = 0; i < this.Plane.Points.Count; i++)
            {
                this.Plane.Points[i] = Geometry.MultipleOnMatrix(this.Plane.Points[i], rotationMatrix);
            }

            if (this.Left != null)
            {
                this.Left.Rotate(axis, angle);
            }

            if (this.Right != null)
            {
                this.Right.Rotate(axis, angle);
            }
        }

        public void MoveAlong(WorkAxis axis, double distance)
        {
            var transitionMatrix = Geometry.CreateTransitionMatrix(axis, distance);

            for (int i = 0; i < this.Plane.Points.Count; i++)
            {
                this.Plane.Points[i] = Geometry.MultipleOnMatrix(this.Plane.Points[i], transitionMatrix);
            }

            if (this.Left != null)
            {
                this.Left.MoveAlong(axis, distance);
            }

            if (this.Right != null)
            {
                this.Right.MoveAlong(axis, distance);
            }
        }

        public void Scale(double coeff)
        {
            var scaleMatrix = Geometry.CreateScaleMatrix(coeff);

            for (int i = 0; i < this.Plane.Points.Count; i++)
            {
                this.Plane.Points[i] = Geometry.MultipleOnMatrix(this.Plane.Points[i], scaleMatrix);
            }

            if (this.Left != null)
            {
                this.Left.Scale(coeff);
            }

            if (this.Right != null)
            {
                this.Right.Scale(coeff);
            }
        }
    }
}
