using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Utility.Geometry
{
    // TODO::JT
    // this whole class is buggered... it won't update when the location changes
    internal class BoundaryRect : Bounds
    {
        protected Vector2D m_topLeft;
        protected Vector2D m_bottomRight;
        protected float m_width;
        protected float m_height;

        public BoundaryRect(Vector2D topLeft, float width, float height)
        {
            Location = topLeft;
            m_topLeft = topLeft;
            m_bottomRight = new Vector2D(topLeft.X + width, topLeft.Y + height);
            m_width = width;
            m_height = height;

            Vector2D bottomLeft = new Vector2D(topLeft.X, topLeft.Y + height);
            Vector2D topRight = new Vector2D(m_topLeft.X + width, m_topLeft.Y);

            Top = new Line(m_topLeft, topRight);
            Bottom = new Line(bottomLeft, m_bottomRight);
            Left = new Line(m_topLeft, bottomLeft);
            Right = new Line(topRight, m_bottomRight);
        }

        public override bool ContainsPoint(Vector2D p)
        {
            return p.X > m_topLeft.X &&
                    p.X < m_bottomRight.X &&
                    p.Y > m_topLeft.Y &&
                    p.Y < m_bottomRight.Y;
        }

        public float Width
        {
            get
            {
                return m_width;
            }
        }

        public float Height
        {
            get
            {
                return m_height;
            }
        }

        public Line Top
        {
            get;
            protected set;
        }

        public Line Left
        {
            get;
            protected set;
        }

        public Line Right
        {
            get;
            protected set;
        }

        public Line Bottom
        {
            get;
            protected set;
        }
    }
}
