using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;

namespace Autonomy.Model.Utility.Geometry
{
    internal struct Vector2D : IPoint
    {
        static Vector2D()
        {
            ZeroVector = new Vector2D(0, 0);
        }

        public Vector2D(float x, float y) : this()
        {
            X = x;
            Y = y;
        }

        public float X
        {
            get;
            private set;
        }

        public float Y
        {
            get;
            private set;
        }

        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(MagnitudeSquared);
            }
        }

        public float MagnitudeSquared
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        public static Vector2D ZeroVector
        {
            get;
            private set;
        }

        public static float DotProduct(Vector2D v1, Vector2D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public Vector2D UnitVector
        {
            get
            {
                if (this.MagnitudeSquared > MathHelper.EPSILON)
                {
                    return this / this.Magnitude;
                }
                else
                {
                    throw new InvalidOperationException("Cannot construct a unit vector from a near zero vector!");
                }
            }
        }

        public static Vector2D Perpendicular(Vector2D vector)
        {
            return new Vector2D(vector.Y, -vector.X);
        }

        #region Operators

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator /(Vector2D vector, float scale)
        {
            return vector * (1 / scale);
        }

        public static Vector2D operator *(Vector2D vector, float scale)
        {
            return new Vector2D(vector.X * scale, vector.Y * scale);
        }

        public static Vector2D Project(Vector2D projectOnto, Vector2D otherVector)
        {
            return otherVector * DotProduct(projectOnto, otherVector) / DotProduct(otherVector, otherVector);
        }

        #endregion
    }
}
 