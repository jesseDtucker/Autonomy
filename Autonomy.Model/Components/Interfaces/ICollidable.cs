using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Utility.Geometry;
using Autonomy.Model.Components.Colliders;

namespace Autonomy.Model.Components.Interfaces
{
    internal interface ICollidable: IMoveable
    {
        ColliderBase Collider
        {
            get;
        }

        /// <summary>
        /// Method called when the object collides with another object
        /// </summary>
        void OnCollided(ICollidable otherObject);
    }
}
