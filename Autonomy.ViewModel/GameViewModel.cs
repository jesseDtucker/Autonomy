using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Autonomy.Model.Factories;
using Autonomy.Model.Interfaces;

namespace Autonomy.ViewModel
{
    public class GameViewModel : IRenderable
    {
        #region Protected Members

        const int HISTORY_COUNT = 150;

        protected Queue<float> fpsHistory = new Queue<float>();
        protected Queue<float> tickTimeHistory = new Queue<float>();

        #endregion

        #region Init

        public GameViewModel()
        {
            IsAsyncEnabled = true;

            GameManager = ManagerFactory.CreateNewGameManager(IsAsyncEnabled);

            AllViewalbleEntities = new List<EntityViewModel>();
            AllRegions = new List<GridRegionViewModel>();

            foreach (var entity in GameManager.AllEntities)
            {
                EntityViewModel newEntityVM = EntityViewModelFactory.CreateViewModel(entity);
                AllViewalbleEntities.Add(newEntityVM);
            }

            foreach (var region in GameManager.AllGridRegions)
            {
                var newRegion = new GridRegionViewModel(region);
                AllRegions.Add(newRegion);
            }

            EntityCount = GameManager.AllEntities.Count;

            for (int i = 0; i < HISTORY_COUNT; ++i)
            {
                fpsHistory.Enqueue(60.0f);
                tickTimeHistory.Enqueue(50.0f);
            }
        }

        #endregion

        #region Model Properties

        protected IGameStateManager GameManager
        {
            get;
            set;
        }

        protected List<EntityViewModel> AllViewalbleEntities
        {
            get;
            private set;
        }

        protected List<GridRegionViewModel> AllRegions
        {
            get;
            private set;
        }

        public bool IsAsyncEnabled
        {
            get;
            private set;
        }

        public float FPS
        {
            get;
            protected set;
        }

        public float TickTime
        {
            get;
            protected set;
        }

        public int EntityCount
        {
            get;
            protected set;
        }

        #endregion

        #region Render

        public void Render(CommonDX.TargetBase target)
        {
            Render(target.DeviceManager.ContextDirect2D);
        }

        public void Render(SharpDX.Direct2D1.DeviceContext deviceContext2D)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            deviceContext2D.BeginDraw();

            deviceContext2D.Clear(SharpDX.Color.Black);

            deviceContext2D.AntialiasMode = SharpDX.Direct2D1.AntialiasMode.PerPrimitive;

            foreach (EntityViewModel entity in AllViewalbleEntities)
            {
                entity.Render(deviceContext2D);
            }

            foreach (var region in AllRegions)
            {
                region.Render(deviceContext2D);
            }

            deviceContext2D.EndDraw();

            stopwatch.Stop();

            fpsHistory.Dequeue();
            fpsHistory.Enqueue(1000.0f / (float)(stopwatch.ElapsedMilliseconds));

            //temp hack
            Task.Delay(50).Wait();

            tickTimeHistory.Dequeue();
            tickTimeHistory.Enqueue(GameManager.MillisecondsForLastTick);

            FPS = fpsHistory.Average();
            TickTime = tickTimeHistory.Average();
        }

        #endregion
    }
}
