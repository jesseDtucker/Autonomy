using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Entities;
using Autonomy.Model.Nodes;
using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.Region
{
    internal abstract class Region
    {
        #region Entity Update

        public abstract void Update(long milliseconds);

        #endregion

        public abstract IEnumerable<Entity> ContainedEntities
        { get; }

        public RegionNode ParentNode
        { get; set; }

        public abstract Bounds RegionBounds
        { get; protected set; }

        public abstract void AddEntity(Entity entity);

        public abstract void RemoveEntity(Entity entity);

    }
}
