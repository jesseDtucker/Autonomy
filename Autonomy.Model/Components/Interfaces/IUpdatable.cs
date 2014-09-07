using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Components.Interfaces
{
    interface IUpdatable
    {
        void Update(long elapsedMilliseconds);
    }
}
