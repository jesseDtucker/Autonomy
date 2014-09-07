using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Utility.Geometry
{
    internal class Line
    {
        protected Vector2D m_direction;

        public Line(Vector2D p1, Vector2D p2)
        {
            float deltaX = p2.X - p1.X;
            float deltaY = p2.Y - p1.Y;

            BasePoint = p1;
            Direction = new Vector2D(deltaX, deltaY);

            Length = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public Vector2D BasePoint
        {
            get;
            set;
        }

        public Vector2D Direction
        {
            get
            {
                return m_direction;
            }
            set
            {
                if (value.MagnitudeSquared > MathHelper.EPSILON)
                {
                    m_direction = value.UnitVector;
                }
                else
                {
                    throw new InvalidOperationException("cannot set a line to have a zero (or near zero) direction");
                }
            }
        }

        public float Length
        {
            get;
            set;
        }

        public bool TestIntersection(Line otherLine)
        {
            float directionTest = Vector2D.DotProduct(Direction.UnitVector, otherLine.Direction.UnitVector);
            bool result = false;

            // check if the lines are paralell
            if (!MathHelper.AreFloatsEqual(Math.Abs(directionTest), 1.0f))
            {
                float v1x = Direction.X;
                float v1y = Direction.Y;
                float p1y = BasePoint.Y;
                float p1x = BasePoint.X;

                float v2x = otherLine.Direction.X;
                float v2y = otherLine.Direction.Y;
                float p2y = otherLine.BasePoint.Y;
                float p2x = otherLine.BasePoint.X;

                float otherLineDistanceToIntersection = (v1x * (p2y - p1y) - v1y * (p1x - p2x)) / (v2x * v1y - v2y * v1x);
                float thisLineDistanceToIntersection;

                // ensure that the other line intersection is within the bounds of the other line
                if (otherLineDistanceToIntersection > 0 && otherLineDistanceToIntersection < otherLine.Length)
                {
                    // use the case that will result in the least error, ie the division value is as far from zero as possible
                    if (Math.Abs(v1x) > Math.Abs(v1y))
                    {
                        thisLineDistanceToIntersection = (p2x + otherLineDistanceToIntersection * v2x - p1x) / v1x;
                    }
                    else
                    {
                        thisLineDistanceToIntersection = (p2y + otherLineDistanceToIntersection * v2y - p1y) / v1y;
                    }

                    if (thisLineDistanceToIntersection > 0 && thisLineDistanceToIntersection < this.Length)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                // the lines are parellel. Check if they are close enough to have been considered to have collided
                float distance = MathHelper.DistanceBetweenPointAndLine(this.BasePoint, otherLine);

                if (distance < MathHelper.EPSILON)
                {
                    // they are close enough to have collided, provided their bounds overlap
                    // if the shorter line contains a point within the bounds of the other line then it is valid
                    Line shorterLine = Length < otherLine.Length ? this : otherLine;
                    Line longerLine = Length >= otherLine.Length ? this : otherLine;

                    Vector2D shorterLineStart = shorterLine.BasePoint;
                    Vector2D shorterLineEnd = shorterLine.BasePoint + shorterLine.Direction * shorterLine.Length;

                    if (MathHelper.DistanceBetweenPoints(shorterLineStart, longerLine.BasePoint) < longerLine.Length ||
                        MathHelper.DistanceBetweenPoints(shorterLineEnd, longerLine.BasePoint) < longerLine.Length)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
