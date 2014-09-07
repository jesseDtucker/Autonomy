using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;

namespace Autonomy.ViewModel
{
    public class TrogoViewModel : EntityViewModel
    {
        #region Init

        public TrogoViewModel(ITrogo trogoModel)
        {
            Trogo = trogoModel;
        }

        #endregion

        #region Model Properties

        public override IEntity ViewedEntity
        {
            get 
            {
                return Trogo;
            }
        }

        protected ITrogo Trogo
        {
            get;
            set;
        }

        protected IPoint Location
        {
            get
            {
                return Trogo.Location;
            }
        }

        #endregion

        #region Render Properties

        const float tempScale = 1.0f;

        public SharpDX.Direct2D1.Ellipse TrogoRepresentation
        {
            get
            {
                // TODO::JT
                // caching would be nice here

                SharpDX.Direct2D1.Ellipse result = new SharpDX.Direct2D1.Ellipse();

                result.Point = Location.ToSharpDxPoint();
                result.Point.X *= tempScale;
                result.Point.Y *= tempScale;

                result.RadiusX = 2.5f;
                result.RadiusY = 2.5f;

                return result;
            }
        }

        #region Render Overrides

        public static SharpDX.Direct2D1.SolidColorBrush hackyBrush = null;

        protected override SharpDX.Direct2D1.DeviceContext Context2D
        {
            get
            {
                return base.Context2D;
            }
            set
            {
                base.Context2D = value;

                if (hackyBrush == null)
                {
                    hackyBrush = new SharpDX.Direct2D1.SolidColorBrush(value, SharpDX.Color.SteelBlue);
                }
            }
        }

        #endregion

        #endregion

        #region Render

        protected override void Render()
        {
            Context2D.FillEllipse(TrogoRepresentation, hackyBrush);
        }

        #endregion
    }
}
