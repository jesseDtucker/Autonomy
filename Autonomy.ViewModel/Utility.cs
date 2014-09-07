using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonomy.ViewModel
{
    public static class Utility
    {
        public static SharpDX.DrawingPointF ToSharpDxPoint(this Autonomy.Model.Interfaces.IPoint point)
        {
            return new SharpDX.DrawingPointF((float)point.X, (float)point.Y);
        }
    }
}
