using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.Components.Colliders
{
    internal class RectangularCollider : ColliderBase
    {
        #region Protected Members

        protected BoundaryRect m_boundsRect = null;

        #endregion

        #region Init

        public RectangularCollider(Vector2D location, float width, float height)
            : this(new BoundaryRect(location, width, height))
        {

        }

        public RectangularCollider(BoundaryRect bounds)
        {
            m_boundsRect = bounds;
        }

        #endregion

        #region Overrides

        public override Utility.Geometry.Bounds CollisionBoundary
        {
            get
            {
                return m_boundsRect;
            }
        }

        public override bool CheckCollision(ColliderBase otherCollider)
        {
            return otherCollider.CollisionBoundary.TestIntersection(m_boundsRect);
        }

        #endregion
    }
}
