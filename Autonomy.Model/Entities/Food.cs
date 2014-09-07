using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Components.Colliders;
using Autonomy.Model.Components.Interfaces;
using Autonomy.Model.Interfaces;
using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.Entities
{
    internal class Food : Entity, IFood, ICollidable
    {
        protected const float RADIUS = 5.0f;

        public Food(Vector2D location)
        {
            Collider = new CircularCollider(RADIUS, location);
        }

        public int FoodValue
        {
            get;
            protected set;
        }

        #region IFood

        IPoint IFood.Location
        {
            get
            {
                return this.Location;
            }
        }

        #endregion

        #region ICollidable

        public ColliderBase Collider
        {
            get;
            protected set;
        }

        public void OnCollided(ICollidable otherObject)
        {
            // do nothing for now
        }

        #endregion

        #region IMoveable

        public Vector2D MoveDelta
        {
            get;
            set;
        }

        public Vector2D Location
        {
            get;
            set;
        }

        #endregion
    }
}
