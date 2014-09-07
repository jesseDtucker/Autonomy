using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.ViewModel
{
    public abstract class BaseRenderableViewModel : IRenderable
    {
        public BaseRenderableViewModel()
        {

        }

        protected virtual SharpDX.Direct2D1.DeviceContext Context2D
        {
            get;
            set;
        }

        protected virtual SharpDX.Direct2D1.Brush Brush
        {
            get;
            set;
        }

        public void Render(SharpDX.Direct2D1.DeviceContext deviceContext2D)
        {
            if (deviceContext2D != Context2D)
            {
                Context2D = deviceContext2D;
            }

            Render();
        }

        protected abstract void Render();
    }
}
