using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;
using Autonomy.Model.Managers;

namespace Autonomy.Model.Factories
{
    public static class ManagerFactory
    {
        public static IGameStateManager CreateNewGameManager(bool isMultithreadingEnabled)
        {
            return new GameStateManager(isMultithreadingEnabled);
        }
    }
}
