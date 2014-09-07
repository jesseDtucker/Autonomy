using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Autonomy.Model.Interfaces
{
    public interface INode : INotifyPropertyChanged
    {
        IEnumerable<INode> AdjacentNodesReadonly
        { get; }
    }
}
