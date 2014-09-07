using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;

namespace Autonomy.Model.Utility.Geometry
{
    internal abstract class Bounds
    {
        public virtual Vector2D Location
        {
            get;
            set;
        }

        public abstract bool ContainsPoint(Vector2D point);
    }
}
