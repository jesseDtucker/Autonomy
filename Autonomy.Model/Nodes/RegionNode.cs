using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Region;

namespace Autonomy.Model.Nodes
{
    internal class RegionNode : TreeNode
    {
        #region Protected Members

        protected Region.Region m_containedRegion = null;

        #endregion

        #region Init

        public RegionNode()
        {

        }

        #endregion

        #region Public Members

        public Region.Region ContainedRegion
        {
            get
            {
                return m_containedRegion;
            }
            set
            {
                m_containedRegion = value;
                m_containedRegion.ParentNode = this;
            }
        }

        #endregion

        #region Overrides

        public override void Update(long millisecondsSinceLastUpdate, Utility.SimpleTaskProcessor taskProcessor)
        {
            while (!m_processingList.IsEmpty)
            {
                Action workUnit = null;
                if (m_processingList.TryDequeue(out workUnit))
                {
                    workUnit();
                }
            }

            ContainedRegion.Update(millisecondsSinceLastUpdate);
        }

        public override void AddChild(TreeNode childNode)
        {
            throw new NotSupportedException("RegionNodes are at the base of any tree structure and so cannot have children");
        }

        #endregion
    }

}
