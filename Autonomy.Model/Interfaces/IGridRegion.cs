using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Interfaces
{
    public interface IGridRegion
    {
        IPoint Location
        {
            get;
        }

        float Width
        {
            get;
        }

        float Height
        {
            get;
        }

        int EntityCount
        {
            get;
        }
    }
}
