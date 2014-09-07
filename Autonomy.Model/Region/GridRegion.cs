using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;
using Autonomy.Model.Entities;
using Autonomy.Model.Utility.Geometry;
using Autonomy.Model.Components.Interfaces;
using Autonomy.Model.Nodes;

namespace Autonomy.Model.Region
{
    internal class GridRegion : Region, IGridRegion
    {
        #region Protected Members

        protected BoundaryRect m_bounds = null;
        protected List<Entity> m_allContainedEntities = new List<Entity>();

        protected List<ICollidable> m_collidableEntities = new List<ICollidable>();
        protected List<IUpdatable> m_updateableEntities = new List<IUpdatable>();

        #endregion

        #region Init

        public GridRegion(float x, float y, float width, float height)
        {
            m_bounds = new BoundaryRect(new Vector2D(x, y), width, height);
        }

        #endregion

        #region IGridRegion

        public IPoint Location
        {
            get 
            {
                return m_bounds.Location;
            }
        }

        public float Width
        {
            get 
            {
                return m_bounds.Width;
            }
        }

        public float Height
        {
            get 
            {
                return m_bounds.Height;
            }
        }

        public int EntityCount
        {
            get
            {
                return m_allContainedEntities.Count;
            }
        }

        #endregion

        #region Overrides

        public override IEnumerable<Entity> ContainedEntities
        {
            get
            {
                return m_allContainedEntities;
            }
        }

        public override Bounds RegionBounds
        {
            get
            {
                return m_bounds;
            }
            protected set
            {
                if (value is BoundaryRect)
                {
                    m_bounds = value as BoundaryRect;
                }
                else
                {
                    throw new InvalidOperationException("Grid Regions may only have BoundaryRects for their boundaries!");
                }
            }
        }

        #region Update

        public override void Update(long milliseconds)
        {
            MoveAndCollideAll(milliseconds);
            UpdateAll(milliseconds);
        }

        protected void MoveAndCollideAll(long milliseconds)
        {
            // first determine what nodes are affected by this calculation
            HashSet<RegionNode> allNodesRequiredForOperation = GetAllAffectedRegions();

            // ensure operation is performed in a thread safe manner
            if (allNodesRequiredForOperation.Count > 0)
            {
                this.ParentNode.EnqueueProcessRequest(new NodeProcessingRequest(allNodesRequiredForOperation,
                () =>
                {
                    MoveAndCheckCollisions(milliseconds, allNodesRequiredForOperation);
                }));
            }

            if (allNodesRequiredForOperation.Count > 20)
            {
                //System.Diagnostics.Debugger.Break();
            }
            
        }

        protected void MoveAndCheckCollisions(long milliseconds, HashSet<RegionNode> affectedRegions)
        {
            float elapsedSeconds = milliseconds / 1000.0f;

            // move em all
            foreach (ICollidable mover in m_collidableEntities)
            {
                if(mover.MoveDelta.MagnitudeSquared > MathHelper.EPSILON)
                {
                    mover.Location += mover.MoveDelta * elapsedSeconds;
                    // TODO::JT have the location be reference based to the additional update is not needed
                    mover.Collider.CollisionBoundary.Location = mover.Location;
                }
            }

            // check collisions
            Dictionary<ICollidable, HashSet<ICollidable>> collisions = CheckCollisions(affectedRegions);

            // move those that have collided back to their previous positions
            foreach (var collisionSet in collisions)
            {
                collisionSet.Key.Location -= collisionSet.Key.MoveDelta * elapsedSeconds;

                foreach (ICollidable collider in collisionSet.Value)
                {
                    collider.Location -= collider.MoveDelta * elapsedSeconds;
                }
            }

            // fire collision events
            foreach (var collisionSet in collisions)
            {
                foreach (ICollidable otherCollider in collisionSet.Value)
                {
                    collisionSet.Key.OnCollided(otherCollider);
                    otherCollider.OnCollided(collisionSet.Key);
                }
            }

            // move entities that are now in other regions to those regions
            foreach (ICollidable collider in m_collidableEntities)
            {
                UpdateColliderRegionLocations(collider, elapsedSeconds);
            }

            // clear move deltas
            foreach (ICollidable mover in m_collidableEntities)
            {
                if (mover.MoveDelta.MagnitudeSquared > MathHelper.EPSILON)
                {
                    mover.MoveDelta = Vector2D.ZeroVector;
                }
            }
        }

        protected void UpdateColliderRegionLocations(ICollidable collider, float seconds)
        {
            if (!RegionBounds.ContainsPoint(collider.Location))
            {
                // find the region it is now in
                // use a breadth first search outwards

                HashSet<RegionNode> nodesChecked = new HashSet<RegionNode>() { this.ParentNode };
                Queue<RegionNode> openNodes = new Queue<RegionNode>();

                RegionNode newRegion = null;

                foreach (var node in this.ParentNode.AdjacentNodes.OfType<RegionNode>())
                {
                    openNodes.Enqueue(node);
                }

                while (openNodes.Count > 0 && newRegion == null)
                {
                    var possibleNode = openNodes.Dequeue();

                    if (possibleNode.ContainedRegion.RegionBounds.ContainsPoint(collider.Location))
                    {
                        newRegion = possibleNode;
                        break;
                    }
                    else
                    {
                        foreach (var node in possibleNode.AdjacentNodes.OfType<RegionNode>())
                        {
                            if (!nodesChecked.Contains(node))
                            {
                                openNodes.Enqueue(node);
                                nodesChecked.Add(node);
                            }
                        }
                    }

                    nodesChecked.Add(possibleNode);
                }

                if (newRegion != null && collider is Entity)
                {
                    this.ParentNode.EnqueueProcessRequest(new NodeProcessingRequest(new List<RegionNode>() { newRegion, this.ParentNode }
                        , () =>
                        {
                            this.RemoveEntity(collider as Entity);
                            newRegion.ContainedRegion.AddEntity(collider as Entity);
                        }));
                }
                else
                {
                    // an object  stepped out of bounds
                    //System.Diagnostics.Debug.WriteLine("ICollidable attempted to move out of bounds, moving back in bounds");

                    // cludgy as hell for now
                    collider.Location -= collider.MoveDelta * seconds * 2;
                    collider.MoveDelta *= -1;
                }
            }
        }

        protected Dictionary<ICollidable, HashSet<ICollidable>> CheckCollisions(HashSet<RegionNode> affectedRegions)
        {
            List<ICollidable> possibleCollisions = new List<ICollidable>();
            Dictionary<ICollidable, HashSet<ICollidable>> collidedElements = new Dictionary<ICollidable, HashSet<ICollidable>>();

            foreach (RegionNode region in affectedRegions)
            {
                possibleCollisions.AddRange(region.ContainedRegion.ContainedEntities.OfType<ICollidable>());
            }

            foreach (ICollidable collider in m_collidableEntities)
            {
                foreach (ICollidable otherCollider in possibleCollisions)
                {
                    if (collider != otherCollider && collider.Collider.CheckCollision(otherCollider.Collider))
                    {
                        // then mark the pair for collision
                        if (collidedElements.ContainsKey(collider))
                        {
                            collidedElements[collider].Add(otherCollider);
                        }
                        else if (collidedElements.ContainsKey(otherCollider))
                        {
                            collidedElements[otherCollider].Add(collider);
                        }
                        else
                        {
                            collidedElements.Add(collider, new HashSet<ICollidable>() { otherCollider });
                        }
                    }
                }
            }

            return collidedElements;
        }

        protected HashSet<RegionNode> GetAllAffectedRegions()
        {
            HashSet<RegionNode> result = new HashSet<RegionNode>();

            foreach (ICollidable collidableEntity in m_collidableEntities)
            {
                var overlappedRegions = GetRegionsEntityOverlapsWith(collidableEntity);
                foreach (RegionNode region in overlappedRegions)
                {
                    result.Add(region);
                }
            }

            return result;
        }

        protected HashSet<RegionNode> GetRegionsEntityOverlapsWith(ICollidable collidableEntity)
        {
            HashSet<RegionNode> result = new HashSet<RegionNode>();
            Stack<RegionNode> openNodes = new Stack<RegionNode>();

            openNodes.Push(this.ParentNode);

            while (openNodes.Count > 0)
            {
                var possibleNode = openNodes.Pop();

                // temp, going to include nodes that overlap + 1, not a perfect solution but will function for now
                result.Add(possibleNode);

                // TODO::JT Profiler says this is bad. Need to re-evalute how entities move between regions
                if (collidableEntity.Collider.CollisionBoundary.TestIntersection(possibleNode.ContainedRegion.RegionBounds))
                {
                    // result.Add(possibleNode);

                    foreach (RegionNode adjacentNode in possibleNode.AdjacentNodes.OfType<TreeNode>())
                    {
                        if (!result.Contains(adjacentNode))
                        {
                            openNodes.Push(adjacentNode);
                        }
                    }
                }
            }

            return result;
        }

        protected void UpdateAll(long milliseconds)
        {
            foreach (IUpdatable updater in m_updateableEntities)
            {
                updater.Update(milliseconds);
            }
        }

        #endregion

        private Object m_tempLock = new object();

        public override void AddEntity(Entity entity)
        {
            m_allContainedEntities.Add(entity);
            entity.OccupiedRegion = this;

            if (entity is ICollidable)
            {
                m_collidableEntities.Add(entity as ICollidable);
            }
            if (entity is IUpdatable)
            {
                m_updateableEntities.Add(entity as IUpdatable);
            }
        }

        public override void RemoveEntity(Entity entity)
        {
            m_allContainedEntities.Remove(entity);

            if (entity is ICollidable)
            {
                m_collidableEntities.Remove(entity as ICollidable);
            }
            if (entity is IUpdatable)
            {
                m_updateableEntities.Remove(entity as IUpdatable);
            }
        }

        #endregion
    }
}
