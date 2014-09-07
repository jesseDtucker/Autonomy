using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Utility.Geometry
{
    internal static class MathHelper
    {
        public const float EPSILON = 0.01f;

        public static float DistanceBetweenPointAndLine(Vector2D point, Line line)
        {
            Vector2D pointDifference = new Vector2D(point.X - line.BasePoint.X, point.Y - line.BasePoint.Y);

            return (pointDifference - line.Direction * Vector2D.DotProduct(pointDifference, line.Direction)).Magnitude;
        }

        public static float DistanceBetweenPointAndLineSegment(Vector2D point, Line line)
        {
            float distanceToInfiniteLine = DistanceBetweenPointAndLine(point, line);

            Vector2D direction = Vector2D.Perpendicular(line.Direction).UnitVector;
            Vector2D pointOnLinePositiveDirection = point + direction * distanceToInfiniteLine;
            Vector2D pointOnLineNegativeDirection = point - direction * distanceToInfiniteLine;

            Vector2D baseToPositivePoint = (pointOnLinePositiveDirection - line.BasePoint);
            Vector2D baseToNegativePoint = (pointOnLineNegativeDirection - line.BasePoint);

            float lineLengthSquared = line.Length * line.Length;

            if (baseToPositivePoint.MagnitudeSquared < MathHelper.EPSILON ||
                baseToNegativePoint.MagnitudeSquared < MathHelper.EPSILON)
            {
                return distanceToInfiniteLine;
            }

            if (baseToPositivePoint.MagnitudeSquared < lineLengthSquared &&
                MathHelper.AreFloatsEqual(Vector2D.DotProduct(baseToPositivePoint.UnitVector, line.Direction), 1.0f) ||
                baseToNegativePoint.MagnitudeSquared < lineLengthSquared &&
                MathHelper.AreFloatsEqual(Vector2D.DotProduct(baseToNegativePoint.UnitVector, line.Direction), 1.0f))
            {
                // then the point exists on the line segment
                return distanceToInfiniteLine;
            }
            else
            {
                // point is not on the line segment, just choose the closest endPoint for the distance
                float distanceToStart = (point - line.BasePoint).Magnitude;
                float distanceToEnd = (point - (line.BasePoint + line.Direction * line.Length)).Magnitude;

                return Math.Min(distanceToStart, distanceToEnd);
            }
        }

        public static float DistanceBetweenPoints(Vector2D p1, Vector2D p2)
        {
            float delta_x = p2.X - p1.X;
            float delta_y = p2.Y - p1.Y;
            return (float)Math.Sqrt(delta_x*delta_x + delta_y*delta_y);
        }

        public static bool AreFloatsEqual(float f1, float f2)
        {
            float left = f1 - EPSILON;
            float right = f1 + EPSILON;

            if (f2 > left && f2 < right)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Rand

        // thread safe rand
        private static MathNet.Numerics.Random.MersenneTwister rand = new MathNet.Numerics.Random.MersenneTwister(true);

        public static double NextDouble()
        {
            return rand.NextDouble();
        }

        #endregion
    }
}
