using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Utility.Geometry
{
    internal class BoundaryCircle : Bounds
    {
        public BoundaryCircle(float radius, Vector2D center)
        {
            Radius = radius;
            Center = center;
        }

        public float Radius
        { get; set; }

        public Vector2D Center
        { get; set; }

        public override Vector2D Location
        {
            get
            {
                return Center;
            }
            set
            {
                Center = value;
            }
        }

        public bool TestIntersection(BoundaryCircle otherCircle)
        {
            return (otherCircle.Center - this.Center).MagnitudeSquared < Radius * Radius;
        }

        public override bool ContainsPoint(Vector2D point)
        {
            return (point - Center).MagnitudeSquared < (Radius * Radius);
        }
    }
}
