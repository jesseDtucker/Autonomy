using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics.Display;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Autonomy.ViewModel;
using CommonDX;

namespace Autonomy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainRenderScreen : Page
    {
        public MainRenderScreen()
        {
            this.InitializeComponent();

            MainViewModel = new GameViewModel();
        }

        private GameViewModel MainViewModel
        {
            get;
            set;
        }

        private SurfaceImageSourceTarget D2DTarget
        {
            get;
            set;
        }

        private DeviceManager DeviceManager
        {
            get;
            set;
        }

        private ImageBrush D2DBrush
        {
            get;
            set;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DeviceManager = new CommonDX.DeviceManager();
            D2DBrush = new ImageBrush();
            v_renderTarget.Fill = D2DBrush;

            D2DTarget = new SurfaceImageSourceTarget((int) (v_renderTarget.Width * DisplayProperties.LogicalDpi / 96.0),
                                                     (int) (v_renderTarget.Height * DisplayProperties.LogicalDpi / 96.0));

            D2DBrush.ImageSource = D2DTarget.ImageSource;

            DeviceManager.OnInitialize += D2DTarget.Initialize;

            D2DTarget.OnRender += MainViewModel.Render;

            DeviceManager.Initialize(DisplayProperties.LogicalDpi);

            CompositionTarget.Rendering += PerformRender;  
        }

        private void PerformRender(object sender, object args)
        {
            D2DTarget.RenderAll();
            v_FPS.Text = MainViewModel.FPS.ToString("##.#");
            v_tickTime.Text = MainViewModel.TickTime.ToString("##.#");
            v_isAsyncEnable.Text = MainViewModel.IsAsyncEnabled.ToString();
            v_entityCount.Text = MainViewModel.EntityCount.ToString();
        }
    }
}
