using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

using Autonomy.Model.Interfaces;

namespace Autonomy.Model.Nodes
{
    internal class TreeNode : Node, ITreeNode
    {
        #region Protected Members

        protected ConcurrentQueue<Action> m_processingList = new ConcurrentQueue<Action>();
        protected List<TreeNode> m_children = new List<TreeNode>();

        #endregion

        #region ITreeNode

        public INode ParentNodeReadonly
        {
            get 
            {
                return ParentNode;
            }
        }

        public IEnumerable<INode> ChildrenReadonly
        {
            get
            {
                return Children;
            }
        }

        #endregion

        #region Public Properties

        public TreeNode ParentNode
        {
            get;
            protected set;
        }

        public IEnumerable<TreeNode> Children
        {
            get
            {
                return m_children;
            }
        }

        #endregion

        #region Public Members

        public virtual void AddChild(TreeNode childNode)
        {
            childNode.ParentNode = this;
            m_children.Add(childNode);
        }

        /// <summary>
        /// Calculates the depth of this node in the tree
        /// </summary>
        public int GetDepth()
        {
            int result = 0;
            TreeNode currentNode = null;
            do
            {
                result++;
                currentNode = currentNode.ParentNode;
            } while (currentNode != null);

            return result;
        }

        public void EnqueueProcessRequest(NodeProcessingRequest request)
        {
            TreeNode requestedNode = FindCommonParent(request.NodesRequired);
            if (requestedNode != null)
            {
                requestedNode.EnqueueRequest(request);
            }
        }

        public virtual void Update(long millisecondsSinceLastUpdate, Utility.SimpleTaskProcessor taskProcessor)
        {
            while (!m_processingList.IsEmpty)
            {
                Action workUnit = null;
                if (m_processingList.TryDequeue(out workUnit))
                {
                    workUnit();
                }
            }

            if (Managers.GameStateManager.CurrentManager.IsAsynEnabled)
            {
                List<Task> childUpdates = new List<Task>();

                foreach (var child in Children)
                {
                    taskProcessor.EnqueueWork( () =>
                    {
                        child.Update(millisecondsSinceLastUpdate, taskProcessor);
                    });
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    child.Update(millisecondsSinceLastUpdate, taskProcessor);
                }
            }
        }
        
        #endregion

        #region Protected Methods

        protected void EnqueueRequest(NodeProcessingRequest request)
        {
            m_processingList.Enqueue(request.Operation);
        }

        /// <summary>
        /// Finds a common parent between all of the provided nodes. Returns null if the operation fails.
        /// Worst case complexity is O(nodeList.Count * treeDepth)
        /// </summary>
        public static TreeNode FindCommonParent(IEnumerable<TreeNode> nodeList)
        {
            List<List<TreeNode>> allPathsToRoot = new List<List<TreeNode>>();
            TreeNode result = null;

            foreach (TreeNode baseNode in nodeList)
            {
                TreeNode currentNode = null;
                List<TreeNode> pathToRoot = new List<TreeNode>();

                currentNode = baseNode;

                do
                {
                    pathToRoot.Add(currentNode);
                    currentNode = currentNode.ParentNode;
                }while(currentNode != null);

                // reorder the elements to have the first element be the root and the last
                // be the baseNode
                pathToRoot.Reverse();
                allPathsToRoot.Add(pathToRoot);
            }

            if (allPathsToRoot.Count > 0)
            {
                int shallowestPath = allPathsToRoot.Min((path) => path.Count);

                for (int depth = 0; depth < shallowestPath; ++depth)
                {
                    // go across each list until all at a particular depth do not match.
                    // the last matching one was correct
                    bool allMatch = true;
                    TreeNode currentNode = allPathsToRoot.First().First();
                    foreach (List<TreeNode> path in allPathsToRoot)
                    {
                        if (path[depth] != currentNode)
                        {
                            allMatch = false;
                            break;
                        }
                    }

                    if (allMatch)
                    {
                        result = currentNode;
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
