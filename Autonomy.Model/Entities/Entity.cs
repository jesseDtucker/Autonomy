using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;
using Autonomy.Model.Region;
using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.Entities
{
    internal abstract class Entity : IEntity
    {
        public Region.Region OccupiedRegion
        {
            get;
            set;
        }
    }
}
