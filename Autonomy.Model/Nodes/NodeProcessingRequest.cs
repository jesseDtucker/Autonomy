using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.Model.Nodes
{
    internal class NodeProcessingRequest
    {
        public NodeProcessingRequest(IEnumerable<TreeNode> nodesRequired, Action operation)
        {
            NodesRequired = nodesRequired;
            Operation = operation;
        }

        public IEnumerable<TreeNode> NodesRequired
        { 
            get;
            protected set;
        }

        public Action Operation
        { 
            get;
            protected set;
        }
    }
}
