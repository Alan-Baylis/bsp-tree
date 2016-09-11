using BspTree.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspTree
{
    public enum WorkAxis
    {
        Ox = 1,
        Oy = 2,
        Oz = 3
    }

    /// <summary>
    /// Helpers for performing geometry operations
    /// </summary>
    public class Geometry
    {
        /// <summary>
        /// Performs transforming of point with matrix
        /// </summary>
        public Point MultipleOnMatrix(Point p, double[][] matrix)
        {
            #region Validation
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }

            if (matrix.Length != 4 || 
                (matrix[0] == null && matrix[1] == null && matrix[2] == null && matrix[3] == null) ||
                (matrix[0].Length != 4 || matrix[1].Length != 4 || matrix[2].Length != 4 || matrix[3].Length != 4))
            {
                throw new ArgumentException("matrix");
            }
            #endregion

            var h = Multiple(p, matrix[3]);

            var x = Multiple(p, matrix[0]) / h;
            var y = Multiple(p, matrix[1]) / h;
            var z = Multiple(p, matrix[2]) / h;
            return new Point
            {
                X = x,
                Y = y,
                Z = z
            };
        }

        /// <summary>
        /// Performs multiplication of point and transformation matrix row
        /// </summary>
        private double Multiple(Point p, double[] matrixColumn)
        {
            #region Validation
            if (matrixColumn.Length != 4)
            {
                throw new ArgumentException("matrixColumn");
            }
            #endregion

            return p.X * matrixColumn[0] + p.Y * matrixColumn[1] + p.Z * matrixColumn[2] + matrixColumn[3];
        }

        /// <summary>
        /// Creates transformation matrix for rotation around the axis with angle in degrees
        /// </summary>
        public double[][] CreateRotationMatrix(WorkAxis axis, double angle)
        {
            var modifiedAngle = ConvertToRad(angle);
            var result = CreateIdentityMatrix();

            switch (axis)
            {
                case WorkAxis.Ox:
                    result[1][1] = Math.Cos(modifiedAngle);
                    result[1][2] = Math.Sin(modifiedAngle);
                    result[2][1] = -Math.Sin(modifiedAngle);
                    result[2][2] = Math.Cos(modifiedAngle);
                    break;
                case WorkAxis.Oy:
                    result[0][0] = Math.Cos(modifiedAngle);
                    result[0][2] = Math.Sin(modifiedAngle);
                    result[2][0] = -Math.Sin(modifiedAngle);
                    result[2][2] = Math.Cos(modifiedAngle);
                    break;
                case WorkAxis.Oz:
                    result[0][0] = Math.Cos(modifiedAngle);
                    result[0][1] = Math.Sin(modifiedAngle);
                    result[1][0] = -Math.Sin(modifiedAngle);
                    result[1][1] = Math.Cos(modifiedAngle);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates transformation matrix for rotation around the axis passing through shift with angle in degrees
        /// </summary>
        public double[][] CreateRotationMatrix(WorkAxis axis, double shift, double angle)
        {
            var modifiedAngle = ConvertToRad(angle);
            var result = CreateIdentityMatrix();

            switch (axis)
            {
                case WorkAxis.Ox:
                    result[1][1] = Math.Cos(modifiedAngle);
                    result[1][2] = Math.Sin(modifiedAngle);
                    result[2][1] = -Math.Sin(modifiedAngle);
                    result[2][2] = Math.Cos(modifiedAngle);
                    break;
                case WorkAxis.Oy:
                    result[0][0] = Math.Cos(modifiedAngle);
                    result[0][2] = Math.Sin(modifiedAngle);
                    result[2][0] = -Math.Sin(modifiedAngle);
                    result[2][2] = Math.Cos(modifiedAngle);
                    break;
                case WorkAxis.Oz:
                    result[0][0] = Math.Cos(modifiedAngle);
                    result[0][1] = Math.Sin(modifiedAngle);
                    result[1][0] = -Math.Sin(modifiedAngle);
                    result[1][1] = Math.Cos(modifiedAngle);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates transformation matrix for transition along with the axis with coefficient
        /// </summary>
        public double[][] CreateTransitionMatrix(WorkAxis axis, double coeff)
        {
            var result = CreateIdentityMatrix();

            switch (axis)
            {
                case WorkAxis.Ox:
                    result[0][3] = coeff;
                    break;
                case WorkAxis.Oy:
                    result[1][3] = coeff;
                    break;
                case WorkAxis.Oz:
                    result[2][3] = coeff;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates transformation matrix for scale with coefficient
        /// </summary>
        public double[][] CreateScaleMatrix(double coeff)
        {
            var result = CreateIdentityMatrix();

            result[0][0] = coeff;
            result[1][1] = coeff;
            result[2][2] = coeff;

            return result;
        }

        private double ConvertToRad(double angle)
        {
            return angle * Math.PI / 180.0;
        }

        private double[][] CreateIdentityMatrix()
        {
            return new double[][]
            {
                new double[] { 1, 0, 0, 0 },
                new double[] { 0, 1, 0, 0 },
                new double[] { 0, 0, 1, 0 },
                new double[] { 0, 0, 0, 1 }
            };
        }
    }
}
