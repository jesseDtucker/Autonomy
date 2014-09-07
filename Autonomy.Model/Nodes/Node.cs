using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Autonomy.Model.Interfaces;
using Autonomy.Model.Entities;

namespace Autonomy.Model.Nodes
{
    internal abstract class Node : INode
    {
        #region Protected Members

        protected List<Node> m_adjacentNodes = new List<Node>();

        #endregion

        #region INode

        public IEnumerable<INode> AdjacentNodesReadonly
        {
            get
            {
                return AdjacentNodes;
            }
        }

        public virtual IEnumerable<Node> AdjacentNodes
        {
            get
            {
                return m_adjacentNodes;
            }
        }

        #endregion

        #region Public Methods

        public void AddAdjacent(Node otherNode)
        {
            if (!(m_adjacentNodes.Contains(otherNode) || otherNode.m_adjacentNodes.Contains(this)))
            {
                m_adjacentNodes.Add(otherNode);
                otherNode.m_adjacentNodes.Add(this);
            }
            else
            {
                throw new InvalidOperationException("Cannot add this node, it is already adjacent!");
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void FirePropertyChanged([CallerMemberName] string callerName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(callerName));
            }
        }

        #endregion
    }
}
