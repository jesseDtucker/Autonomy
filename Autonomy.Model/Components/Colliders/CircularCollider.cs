using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.Components.Colliders
{
    internal class CircularCollider : ColliderBase
    {
        #region Protected Members

        BoundaryCircle m_circleBounds = null;

        #endregion

        #region Init

        public CircularCollider(float radius, Vector2D location)
            : this(new BoundaryCircle(radius, location))
        {

        }

        public CircularCollider(BoundaryCircle bounds)
        {
            m_circleBounds = bounds;
        }

        #endregion

        #region Overrides

        public override Utility.Geometry.Bounds CollisionBoundary
        {
            get
            {
                return m_circleBounds;
            }
        }

        public override bool CheckCollision(ColliderBase otherCollider)
        {
            return otherCollider.CollisionBoundary.TestIntersection(m_circleBounds);
        }

        #endregion
    }
}
