using ARKit;
using Microsoft.Maui.Handlers;
using ARKitDemo.Controls;
using ARKitDemo.Platforms.iOS;

namespace ARKitDemo.Handlers;

public class ARViewHandler : ViewHandler<ARView, ARSCNView>
{
    MauiARView? _mauiARView;

    public static IPropertyMapper<ARView, ARViewHandler> Mapper = new PropertyMapper<ARView, ARViewHandler>(ViewMapper)
    {
        [nameof(ARView.IsSessionRunning)] = MapIsSessionRunning
    };

    public ARViewHandler() : base(Mapper)
    {
    }

    protected override ARSCNView CreatePlatformView()
    {
        var arView = new ARSCNView()
        {
            AutoenablesDefaultLighting = true,
            ShowsStatistics = true,
            Delegate = new SceneViewDelegate(),
        };

        _mauiARView = new MauiARView();
        _mauiARView.SetARView(arView);

        return arView;
    }

    public static void MapIsSessionRunning(IViewHandler handler, IView view)
    {
        if (handler is ARViewHandler arHandler && view is ARView arView)
        {
            if (arView.IsSessionRunning)
                arHandler._mauiARView?.StartARSession();
            else
                arHandler._mauiARView?.StopARSession();
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
        _mauiARView?.StopARSession();

        platformView.Dispose();
        base.DisconnectHandler(platformView);
    }
}