using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;

namespace Autonomy.ViewModel
{
    public abstract class EntityViewModel : BaseRenderableViewModel
    {
        public EntityViewModel()
        {

        }

        public abstract IEntity ViewedEntity
        {
            get;
        }
    }
}
