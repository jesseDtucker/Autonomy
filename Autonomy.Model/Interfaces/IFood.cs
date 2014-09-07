using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Interfaces
{
    public interface IFood : IEntity
    {
        int FoodValue
        { get; }

        IPoint Location
        {
            get;
        }
    }
}
