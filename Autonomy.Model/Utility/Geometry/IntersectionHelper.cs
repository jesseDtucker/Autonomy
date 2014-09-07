using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Utility.Geometry
{
    internal static class IntersectionHelper
    {
        #region Generic Intersection

        public static bool TestIntersection(this Bounds bounds, Bounds otherBounds)
        {
            if (otherBounds is BoundaryCircle)
            {
                return bounds.TestIntersection(otherBounds as BoundaryCircle);
            }
            else if (otherBounds is BoundaryRect)
            {
                return bounds.TestIntersection(otherBounds as BoundaryRect);
            }
            else
            {
                throw new InvalidOperationException("Attempting to test boundary on unknown types");
            }
        }

        public static bool TestIntersection(this Bounds bounds, BoundaryCircle circle)
        {
            if (bounds is BoundaryCircle)
            {
                return circle.TestIntersection(bounds as BoundaryCircle);
            }
            else if (bounds is BoundaryRect)
            {
                return circle.TestIntersection(bounds as BoundaryRect);
            }
            else
            {
                throw new InvalidOperationException("Attempting to test boundary on unknown types");
            }
        }

        public static bool TestIntersection(this Bounds bounds, BoundaryRect rect)
        {
            if (bounds is BoundaryCircle)
            {
                return rect.TestIntersection(bounds as BoundaryCircle);
            }
            else if (bounds is BoundaryRect)
            {
                return rect.TestIntersection(bounds as BoundaryRect);
            }
            else
            {
                throw new InvalidOperationException("Attempting to test boundary on unknown types");
            }
        }

        #endregion

        #region Circle / Rectangle intersection

        public static bool DoesCircleIntersectRect(BoundaryRect rect, BoundaryCircle circle)
        {
            // 2 cases:
            // case 1: the center of the circle is within the rect
            // case 2: the rectange has an edge that intersects the circle
            if (rect.ContainsPoint(circle.Center))
            {
                return true;
            }
            else
            {
                return  rect.Top.TestIntersection(circle) ||
                        rect.Bottom.TestIntersection(circle) ||
                        rect.Left.TestIntersection(circle) ||
                        rect.Right.TestIntersection(circle);
            }
        }

        public static bool TestIntersection(this BoundaryRect rect, BoundaryCircle circle)
        {
            return DoesCircleIntersectRect(rect, circle);
        }

        public static bool TestIntersection(this BoundaryCircle circle, BoundaryRect rect)
        {
            return DoesCircleIntersectRect(rect, circle);
        }

        #endregion

        #region Line / Circle intersection

        public static bool DoesLineIntersectCircle(BoundaryCircle circle, Line line)
        {
            return MathHelper.DistanceBetweenPointAndLineSegment(circle.Center, line) < circle.Radius;
        }

        public static bool TestIntersection(this BoundaryCircle circle, Line line)
        {
            return DoesLineIntersectCircle(circle, line);
        }

        public static bool TestIntersection(this Line line, BoundaryCircle circle)
        {
            return DoesLineIntersectCircle(circle, line);
        }

        #endregion
    }
}
