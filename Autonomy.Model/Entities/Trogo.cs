using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Components.Interfaces;
using Autonomy.Model.Components.Colliders;
using Autonomy.Model.Interfaces;
using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.Entities
{
    internal class Trogo : Entity, ITrogo, ICollidable, IUpdatable
    {
        #region const members

        protected float TROGO_RADIUS = 2.5f;

        #endregion

        #region Protected Members

        // TODO::JT
        // consider building out a per action system
        protected const long TIME_BETWEEN_ACTIONS = 200;

        protected Vector2D m_currentDirection;
        protected Vector2D m_moveDelta;
        protected long m_timeSinceLastAction = 0;

        #endregion

        #region Init

        public Trogo(Vector2D location)
        {
            Collider = new CircularCollider(TROGO_RADIUS, location);

            MoveDelta = new Vector2D((float)MathHelper.NextDouble() - 0.5f, (float)MathHelper.NextDouble() - 0.5f);
        }

        #endregion

        #region ITrogo

        IPoint ITrogo.Location
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
            this.m_currentDirection *= -1;
        }

        #endregion

        #region IMoveable

        public Vector2D MoveDelta
        {
            get
            {
                return m_moveDelta;
            }
            set
            {
                if (float.IsNaN(value.X) || float.IsNaN(value.Y))
                {
                    System.Diagnostics.Debugger.Break();
                }
                m_moveDelta = value;
                if (value.MagnitudeSquared > MathHelper.EPSILON)
                {
                    m_currentDirection = value.UnitVector;
                }
                else
                {
                    //m_currentDirection = Vector2D.ZeroVector;
                }
            }
        }

        public Vector2D Location
        {
            get;
            set;
        }

        #endregion

        #region IUpdatable

        public void Update(long elapsedMilliseconds)
        {
            m_timeSinceLastAction += elapsedMilliseconds;

            if (m_timeSinceLastAction > Trogo.TIME_BETWEEN_ACTIONS)
            {
                m_timeSinceLastAction = 0;
                RandomMove();
            }

            MoveDelta = m_currentDirection * 10;
        }

        protected void RandomMove()
        {
            float newX = m_currentDirection.X;
            float newY = m_currentDirection.Y;

            //float currentTheta = (float)(Math.Tan(currentDirection.Y / currentDirection.X));

            //float theta = currentTheta + (float)(MathHelper.NextDouble() / 200.0f * Math.PI * 2.0) - (float)(MathHelper.NextDouble() / 200.0f * Math.PI * 2.0);

            newX = (float)(newX + (MathHelper.NextDouble() - 0.5f)/50.0f )*100.0f;
            newY = (float)(newY + (MathHelper.NextDouble() - 0.5f)/50.0f )*100.0f;

            this.m_currentDirection = new Vector2D(newX, newY);

            if (this.m_currentDirection.MagnitudeSquared > MathHelper.EPSILON)
            {
                this.m_currentDirection = this.m_currentDirection.UnitVector;
            }
            else
            {
                this.m_currentDirection = Vector2D.ZeroVector;
            }
        }

        #endregion
    }
}
