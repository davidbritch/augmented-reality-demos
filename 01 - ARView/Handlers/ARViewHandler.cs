using ARKit;
using Microsoft.Maui.Handlers;
using ARKitDemo.Controls;
using ARKitDemo.Services;

namespace ARKitDemo.Handlers;

public class ARViewHandler : ViewHandler<ARView, ARSCNView>
{
    ARService _arService;

    public static IPropertyMapper<ARView, ARViewHandler> Mapper = new PropertyMapper<ARView, ARViewHandler>(ViewMapper)
    {
        [nameof(ARView.IsSessionRunning)] = MapIsSessionRunning
    };

    public ARViewHandler(IARService arService) : base(Mapper)
    {
        _arService = (ARService)arService;
    }

    protected override ARSCNView CreatePlatformView()
    {
        var arView = new ARSCNView()
        {
            AutoenablesDefaultLighting = true,
            DebugOptions = ARSCNDebugOptions.ShowWorldOrigin,
            ShowsStatistics = true
        };

        _arService.SetARView(arView);

        return arView;
    }

    public static void MapIsSessionRunning(IViewHandler handler, IView view)
    {
        if (handler is ARViewHandler arHandler && view is ARView arView)
        {
            if (arView.IsSessionRunning)
                arHandler._arService?.StartARSession();
            else
                arHandler._arService?.StopARSession();
        }
    }

    protected override void ConnectHandler(ARSCNView platformView)
    {
        base.ConnectHandler(platformView);

        if (VirtualView != null)
            MapIsSessionRunning(this, VirtualView);
    }

    protected override void DisconnectHandler(ARSCNView platformView)
    {
        if (_arService != null)
            _arService.StopARSession();

        base.DisconnectHandler(platformView);
    }
}