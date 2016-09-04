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
        public Tree Inner { get; set; }
        public Tree Outer { get; set; }
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
                return p.IsInTriangle(this.Plane.Points.ToArray());
            }

            var contains = false;
            if (this.Inner != null)
            {
                contains |= this.Inner.Contains(point);
            }

            if (this.Outer != null)
            {
                contains |= this.Outer.Contains(point);
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

            if (this.Inner != null)
            {
                this.Inner.Rotate(axis, angle);
            }

            if (this.Outer != null)
            {
                this.Outer.Rotate(axis, angle);
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

            if (this.Inner != null)
            {
                this.Inner.Rotate(axis, angle);
            }

            if (this.Outer != null)
            {
                this.Outer.Rotate(axis, angle);
            }
        }

        public void MoveAlong(WorkAxis axis, double distance)
        {
            var transitionMatrix = Geometry.CreateTransitionMatrix(axis, distance);

            for (int i = 0; i < this.Plane.Points.Count; i++)
            {
                this.Plane.Points[i] = Geometry.MultipleOnMatrix(this.Plane.Points[i], transitionMatrix);
            }

            if (this.Inner != null)
            {
                this.Inner.MoveAlong(axis, distance);
            }

            if (this.Outer != null)
            {
                this.Outer.MoveAlong(axis, distance);
            }
        }

        public void Scale(double coeff)
        {
            var scaleMatrix = Geometry.CreateScaleMatrix(coeff);

            for (int i = 0; i < this.Plane.Points.Count; i++)
            {
                this.Plane.Points[i] = Geometry.MultipleOnMatrix(this.Plane.Points[i], scaleMatrix);
            }

            if (this.Inner != null)
            {
                this.Inner.Scale(coeff);
            }

            if (this.Outer != null)
            {
                this.Outer.Scale(coeff);
            }
        }

        public List<Plane> GetPlanes()
        {
            var result = new List<Plane>();

            GetPlane(this, result);

            return result;
        }

        private void GetPlane(Tree node, List<Plane> result)
        {
            if (node == null)
                return;

            result.Add(node.Plane);

            GetPlane(node.Inner, result);
            GetPlane(node.Outer, result);
        }
    }
}
