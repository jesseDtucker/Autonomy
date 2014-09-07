using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonDX;

namespace Autonomy.ViewModel
{
    public interface IRenderable
    {
        void Render(SharpDX.Direct2D1.DeviceContext deviceContext2D);
    }
}
