using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;

namespace Autonomy.ViewModel
{
    public class GridRegionViewModel : IRenderable
    {
        private SharpDX.RectangleF? m_viewedRect = null;

        public GridRegionViewModel(IGridRegion viewedRegion)
        {
            ViewedRegion = viewedRegion;
        }

        public IGridRegion ViewedRegion
        {
            get;
            protected set;
        }

        const float tempScaler = 1.0f;

        private SharpDX.RectangleF GetRectangle()
        {
            // caching because the value never changes
            if (m_viewedRect == null)
            {
                m_viewedRect = new SharpDX.RectangleF(ViewedRegion.Location.X * tempScaler,
                                                        ViewedRegion.Location.Y * tempScaler,
                                                        (ViewedRegion.Location.X + ViewedRegion.Width) * tempScaler,
                                                        (ViewedRegion.Location.Y + ViewedRegion.Height) * tempScaler);
            }

            return m_viewedRect.Value;
        }

        public static SharpDX.Direct2D1.SolidColorBrush hackyBrush = null;
        public static SharpDX.Direct2D1.SolidColorBrush hackyHighlightedBrush = null;

        public void Render(SharpDX.Direct2D1.DeviceContext deviceContext2D)
        {
            if(hackyBrush == null)
            {
                hackyBrush = new SharpDX.Direct2D1.SolidColorBrush(deviceContext2D, SharpDX.Color4.White);
            }

            if (hackyHighlightedBrush == null)
            {
                hackyHighlightedBrush = new SharpDX.Direct2D1.SolidColorBrush(deviceContext2D, new SharpDX.Color4(0xFFFF0000));
            }

            if (ViewedRegion.EntityCount > 0)
            {
                deviceContext2D.DrawRectangle(GetRectangle(), hackyBrush);
            }
            else
            {
                deviceContext2D.DrawRectangle(GetRectangle(), hackyHighlightedBrush);
            }
        }
    
    }
}
