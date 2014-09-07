using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Interfaces
{
    public interface IGameStateManager
    {
        IList<IEntity> AllEntities
        { get; }

        void ShutdownManager();

        long MillisecondsForLastTick
        { get; }

        
        List<IGridRegion> AllGridRegions
        {
            get;
        }
    }
}
