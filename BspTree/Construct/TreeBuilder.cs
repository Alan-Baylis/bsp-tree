using BspTree.Base;
using BspTree.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspTree.Construct
{
    public class TreeBuilder
    {
        #region Fields
        private List<Plane> _planes;
        #endregion

        #region Constructors
        public TreeBuilder(List<Plane> planes)
        {
            this._planes = planes;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructs bsp tree
        /// </summary>
        public Tree Construct()
        {
            this.CreateNormals();

            //setting the first plane as splitter
            var splitter = this._planes.First();

            var result = new Tree
            {
                Plane = splitter
            };

            this._planes.Remove(splitter);

            this.CreateLeft(result, this._planes);

            this.CreateRight(result, this._planes);

            return result;
        }

        /// <summary>
        /// Creates left (filled) subtree for the passed parent
        /// </summary>
        private void CreateLeft(Tree parent, List<Plane> planes)
        {
            var innerPlanes = new List<Plane>();
            foreach (var item in planes)
            {
                //attempt to find intersection line
                var line = this.IntersectLine(parent.Plane, item);

            }
        }

        /// <summary>
        /// Creates right (empty) subtree for the passed parent
        /// </summary>
        private void CreateRight(Tree parent, List<Plane> planes)
        {

        }

        private Line IntersectLine(Plane plane1, Plane plane2)
        {
            //finding direction vector
            var vec = Point.VectorProduct(plane1.NormVect, plane2.NormVect);
            //then assembled equations should be resolved in order to find point on intersect line
            //|A1*x + B1*y + C1*z + D1 = 0
            //|A2*z + B2*y + C2*z + D2 = 0
            //from first equation
            //x = 0 -> y = -(C1*z + D1)/B1, z = -(B1*y + D1)/C1
            //y = 0 -> x = -(C1*z + D1)/A1, z = -(A1*y + D1)/C1
            //z = 0 -> x = -(B1*y + D1)/A1, y = -(A1*y + D1)/B1
            //required to take maximum helpful equation
            if (plane1.NormVect.Y != 0)
            {
                //setting x to 0, solving equation relatively to z
                var den = plane1.NormVect.Y * plane2.NormVect.Z - plane1.NormVect.Z * plane2.NormVect.Y;
                if (den != 0)
                {
                    var z = (plane2.NormVect.Y * plane1.D - plane1.NormVect.Y * plane2.D) / den;
                    var y = -(plane1.NormVect.Z * z + plane1.D) / plane1.NormVect.Y;
                    return new Line
                    {
                        Point = new Point
                        {
                            X = 0,
                            Y = y,
                            Z = z
                        },
                        Vector = vec
                    };
                }
            }

            if (plane1.NormVect.X != 0)
            {
                //setting y to 0, solving equation relatively to z
                var den = plane1.NormVect.X * plane2.NormVect.Z - plane1.NormVect.Z * plane2.NormVect.X;
                if (den != 0)
                {
                    var z = (plane2.NormVect.X * plane1.D - plane1.NormVect.X * plane2.D) / den;
                    var x = -(plane1.NormVect.Z * z + plane1.D) / plane1.NormVect.X;
                    return new Line
                    {
                        Point = new Point
                        {
                            X = x,
                            Y = 0,
                            Z = z
                        },
                        Vector = vec
                    };
                }
            }

            if (plane1.NormVect.Z != 0)
            {
                //setting x to 0, solving equation relatively to y
                var den = plane1.NormVect.Z * plane2.NormVect.Y - plane1.NormVect.Y * plane2.NormVect.Z;
                if (den != 0)
                {
                    var y = (plane2.NormVect.Z * plane1.D - plane1.NormVect.Z * plane2.D) / den;
                    var z = -(plane1.NormVect.Y * y + plane1.D) / plane1.NormVect.Z;
                    return new Line
                    {
                        Point = new Point
                        {
                            X = 0,
                            Y = y,
                            Z = z
                        },
                        Vector = vec
                    };
                }
            }

            //if every coefficient is 0 then equation is not a plane or
            //planes are parallel
            return null;
        }

        /// <summary>
        /// Creates outward normal for each plane
        /// </summary>
        private void CreateNormals()
        {
            foreach (var item in this._planes)
            {
                //creating normal
                item.NormVect = this.GetNormal(item);

                //verifying that normal is outward
                this.VerifyNormal(item);
            }
        }

        private Point GetNormal(Plane plane)
        {
            var vec1 = Plane.CreateVector(plane.Points[0], plane.Points[1]);
            var vec2 = Plane.CreateVector(plane.Points[0], plane.Points[2]);

            return Point.VectorProduct(vec1, vec2);
        }

        /// <summary>
        /// Determines that the normal is outward. If it is not, reverting the normal
        /// </summary>
        private void VerifyNormal(Plane plane)
        {
            //testing count of intersection for both normals
            //taking normal, for which count of intersections is even (including 0)
            var normal = plane.NormVect;

            //if we take one of vertex points it can give the false positive result
            var straightCounter = this.TestNormal(plane.GetRandomPoint(), normal);

            if (straightCounter % 2 != 0)
            {
                plane.NormVect = new Point
                {
                    X = -normal.X,
                    Y = -normal.Y,
                    Z = -normal.Z
                };
            }
        }

        private int TestNormal(Point p, Point normal)
        {
            var result = 0;
            foreach (var item in this._planes)
            {
                if (item.NormVect.X * p.X +
                    item.NormVect.Y * p.Y +
                    item.NormVect.Z * p.Z +
                    item.D != 0)
                {
                    //attempt to find intersection point
                    //taking line equation as 
                    // | x = p.x + normal.x * t;
                    // | y = p.y + normal.y * t;
                    // | z = p.z + normal.z * t;
                    var den = item.NormVect.X * normal.X +
                        item.NormVect.Y * normal.Y +
                        item.NormVect.Z * normal.Z;
                    if (den != 0)
                    {
                        var t = -(item.NormVect.X * p.X + item.NormVect.Y * p.Y + item.NormVect.Z * p.Z + item.D) / den;
                        //checking that intersection point is in correct half-space
                        if (t > 0)
                        {
                            var intersectPoint = new Point
                            {
                                X = p.X + normal.X * t,
                                Y = p.Y + normal.Y * t,
                                Z = p.Z + normal.Z * t
                            };

                            //check that intersection point lays in required polygon
                            if (intersectPoint.IsBetween(item.Points[0], item.Points[1])
                                && intersectPoint.IsBetween(item.Points[0], item.Points[2]))
                            {
                                result++;
                            }
                        }
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
