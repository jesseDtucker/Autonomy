using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.Components.Interfaces
{
    internal interface IMoveable
    {
        Vector2D MoveDelta
        {
            get;
            set;
        }

        Vector2D Location
        {
            get;
            set;
        }
    }
}
