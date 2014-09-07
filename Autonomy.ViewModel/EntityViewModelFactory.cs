using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;

namespace Autonomy.ViewModel
{
    internal static class EntityViewModelFactory
    {
        internal static EntityViewModel CreateViewModel(IEntity entityModel)
        {
            if (entityModel is IFood)
            {
                return CreateFoodViewModel(entityModel as IFood);
            }
            else if (entityModel is ITrogo)
            {
                return CreateTrogoViewModel(entityModel as ITrogo);
            }
            else
            {
                return null;
            }
        }

        internal static EntityViewModel CreateFoodViewModel(IFood food)
        {
            return new FoodViewModel(food);
        }

        internal static EntityViewModel CreateTrogoViewModel(ITrogo trogo)
        {
            return new TrogoViewModel(trogo);
        }
    }
}
