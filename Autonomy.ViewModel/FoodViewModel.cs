using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autonomy.Model.Interfaces;

namespace Autonomy.ViewModel
{
    class FoodViewModel : EntityViewModel
    {
        public FoodViewModel(IFood food) : base()
        {
            Food = food;
        }

        ~FoodViewModel()
        {
            if (Brush != null)
            {
                Brush.Dispose();
            }
        }

        #region Model Properties

        public override IEntity ViewedEntity
        {
            get 
            { 
                return Food; 
            }
        }

        protected IFood Food
        {
            get;
            set;
        }

        protected IPoint Location
        {
            get
            {
                return Food.Location;
            }
        }

        #endregion

        #region Render Properties

        public SharpDX.Direct2D1.Ellipse FoodRepresentation
        {
            get
            {
                // TODO::JT
                // caching would be nice here

                SharpDX.Direct2D1.Ellipse result = new SharpDX.Direct2D1.Ellipse();

                result.Point = Location.ToSharpDxPoint();

                result.RadiusX = 1;
                result.RadiusY = 1;

                return result;
            }
        }

        #region Render Overrides

        protected static SharpDX.Direct2D1.SolidColorBrush hackyBrush = null;

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
                    hackyBrush = new SharpDX.Direct2D1.SolidColorBrush(value, SharpDX.Color.IndianRed);
                }
            }
        }

        

        #endregion

        #endregion

        #region IRenderable

        protected override void Render()
        {
            Context2D.FillEllipse(FoodRepresentation, hackyBrush);
        }

        #endregion
    }
}
