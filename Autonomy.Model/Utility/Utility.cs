using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Nodes;

namespace Autonomy.Model.Utility
{
    internal static class Utility
    {
        internal static Random rand = new Random();

        public static T SelectRandom<T>(this IEnumerable<T> enumerable)
            where T : class
        {
            if (enumerable.Count() > 0)
            {
                return enumerable.ElementAt(rand.Next(enumerable.Count()));
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<Node> SelectAvailableNodesForMovement(this IEnumerable<Node> nodeList)
        {
            return (from node in nodeList
                    //where node.ContainedEntity == null || node.ContainedEntity.IsSolid == false
                    select node);
        }
    }
}
