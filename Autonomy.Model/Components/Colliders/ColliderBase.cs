using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.Components.Colliders
{
    internal abstract class ColliderBase
    {
        /// <summary>
        /// Represents the boundary of an object that is collidable
        /// </summary>
        public abstract Bounds CollisionBoundary
        {
            get;
        }

        public abstract bool CheckCollision(ColliderBase otherCollider);

        /// <summary>
        /// True if the object can be stopped by other solid objects, false if it cannot be stopped.
        /// Does not impact whether or not a collision event is fired.
        /// </summary>
        public bool IsSolid
        {
            get;
            protected set;
        }
    }
}
