using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Autonomy.Model.Interfaces;
using Autonomy.Model.Entities;
using Autonomy.Model.Nodes;
using Autonomy.Model.Region;

namespace Autonomy.Model.Managers
{
    internal class GameStateManager : IGameStateManager
    {
        #region Constants

        internal const double FOOD_CREATION_PROBABILITY = 0.7;
        internal const double TROGO_CREATION_PROBABILITY = 0.5;

        #endregion

        #region Protected Members

        protected Random rand = new Random();
        protected TreeNode m_root = null;
        protected volatile bool m_isRunning = false;

        #endregion

        #region Init

        internal GameStateManager(bool isMultithreaded)
        {
            AllEntities = new List<IEntity>();
            AllGridRegions = new List<IGridRegion>();
            PopulateRegion();

            IsAsynEnabled = isMultithreaded;

            Task.Factory.StartNew(MainLoop, TaskCreationOptions.LongRunning);

            CurrentManager = this;
        }

        // temp
        internal static GameStateManager CurrentManager
        {
            get;
            private set;
        }

        internal bool IsAsynEnabled
        {
            get;
            private set;
        }

        #endregion

        #region IGameStateManager

        public void ShutdownManager()
        {
            m_isRunning = false;
        }

        public IList<IEntity> AllEntities
        {
            get;
            set;
        }

        public List<IGridRegion> AllGridRegions
        {
            get;
            set;
        }

        public long MillisecondsForLastTick
        {
            get;
            protected set;
        }

        #endregion

        #region GameLoop

        protected void MainLoop()
        {
            m_isRunning = true;
            Stopwatch stopWatch = new Stopwatch();

            Utility.SimpleTaskProcessor taskProcessor = new Utility.SimpleTaskProcessor();

            while (m_isRunning)
            {
                stopWatch.Start();

                long similuationLenght = Math.Min(Math.Max(MillisecondsForLastTick, 50),500);

                ExecuteMainLoop(similuationLenght, taskProcessor);
                stopWatch.Stop();
                MillisecondsForLastTick = stopWatch.ElapsedMilliseconds;
                stopWatch.Reset();

                if (MillisecondsForLastTick < 50)
                {
                    stopWatch.Start();
                    Task.Delay(TimeSpan.FromMilliseconds(50 - MillisecondsForLastTick)).Wait();
                    stopWatch.Stop();

                    MillisecondsForLastTick += stopWatch.ElapsedMilliseconds;
                }
            }
        }

        protected void ExecuteMainLoop(long millisecondsSinceLastRun, Utility.SimpleTaskProcessor taskProcessor)
        {
            m_root.Update(millisecondsSinceLastRun, taskProcessor);

            if (IsAsynEnabled)
            {
                taskProcessor.AwaitAllWorkCompleted().Wait();
            }
        }

        #endregion

        #region Populate

        protected void PopulateRegion()
        {
            var regions = ConstructRegions();
            m_root = BuildTree(regions, regions[0].Count(), regions.Count());

            FillRegions(regions);
        }

        protected void FillRegions(RegionNode[][] regions)
        {
            foreach (RegionNode[] regionArray in regions)
            {
                foreach (RegionNode node in regionArray)
                {
                    Entity entityToAdd = null;

                    if (rand.NextDouble() < 0.00)
                    {
                        var newFood = new Food(node.ContainedRegion.RegionBounds.Location);
                        newFood.Location = node.ContainedRegion.RegionBounds.Location + new Autonomy.Model.Utility.Geometry.Vector2D(1, 1);
                        entityToAdd = newFood;
                    }
                    else if (rand.NextDouble() < 1.50)
                    {
                        var newTrogo = new Trogo(node.ContainedRegion.RegionBounds.Location);
                        newTrogo.Location = node.ContainedRegion.RegionBounds.Location + new Autonomy.Model.Utility.Geometry.Vector2D(1, 1);
                        entityToAdd = newTrogo;
                    }

                    if (entityToAdd != null)
                    {
                        node.ContainedRegion.AddEntity(entityToAdd);
                        AllEntities.Add(entityToAdd);
                    }
                }
            }
        }

        protected RegionNode[][] ConstructRegions()
        {
            const float LEFT = 0.0f;
            const float TOP = 0.0f;

            const float WIDTH = 25.0f;
            const float HEIGHT = 25.0f;

            const int NUM_WIDE = 60;
            const int NUM_TALL = 60;

            RegionNode[][] regions = new RegionNode[NUM_WIDE][];

            // first create all of the region nodes
            for (int i = 0; i < NUM_WIDE; i++)
            {
                regions[i] = new RegionNode[NUM_TALL];
                for (int j = 0; j < NUM_TALL; j++)
                {
                    regions[i][j] = new RegionNode() { ContainedRegion = new GridRegion(i * WIDTH + LEFT, j * HEIGHT + TOP, WIDTH, HEIGHT) };
                    
                    AllGridRegions.Add(regions[i][j].ContainedRegion as GridRegion);
                }
            }

            // now link them together
            for (int i = 0; i < NUM_WIDE; i++)
            {
                for (int j = 0; j < NUM_TALL; j++)
                {
                    if (i + 1 < NUM_WIDE)
                    {
                        regions[i][j].AddAdjacent(regions[i + 1][j]);
                    }

                    if (j + 1 < NUM_TALL)
                    {
                        regions[i][j].AddAdjacent(regions[i][j + 1]);
                    }
                }
            }

            return regions;
        }

        protected TreeNode BuildTree(TreeNode[][] nodesToJoin, int height, int width)
        {
            if (height > 1 && width > 1)
            {
                TreeNode[][] nextLayer = new TreeNode[(width + 1) / 2][];

                for (int i = 0; i < (width + 1) / 2; ++i)
                {
                    nextLayer[i] = new TreeNode[(height + 1) / 2];
                    for (int j = 0; j < (height + 1) / 2; ++j)
                    {
                        nextLayer[i][j] = new TreeNode();
                    }
                }

                for (int x = 0; x < nodesToJoin.Count(); ++x)
                {
                    for (int y = 0; y < nodesToJoin[x].Count(); ++y)
                    {
                        nextLayer[x / 2][y / 2].AddChild(nodesToJoin[x][y]);
                    }
                }

                return BuildTree(nextLayer, (height + 1) / 2, (width + 1) / 2);
            }
            else
            {
                return nodesToJoin[0][0];
            }
        }

        #endregion

        
    }
}
