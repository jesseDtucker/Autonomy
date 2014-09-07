using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.Interfaces
{
    public interface ITrogo : IEntity
    {
        IPoint Location
        {
            get;
        }
    }
}
