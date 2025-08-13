using ARKit;
using UIKit;

namespace ARKitDemo.Services;

public class ARService : IARService
{
    ARSCNView? _arView;
    ARSession? _arSession;

    public bool IsARSupported()
    {
        return ARConfiguration.IsSupported;
    }

    public void StartARSession()
    {
        if (_arSession == null)
            return;

        _arSession.Run(new ARWorldTrackingConfiguration
        {
            AutoFocusEnabled = true,
            LightEstimationEnabled = true,
            PlaneDetection = ARPlaneDetection.Horizontal,
            WorldAlignment = ARWorldAlignment.GravityAndHeading
        }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

        AddContent();
    }

    public void StopARSession()
    {
        _arSession?.Pause();
    }

    internal void SetARView(ARSCNView view)
    {
        _arView = view;
        _arSession = view.Session;
    }

    void AddContent()
    {
        float size = 0.05f;
        var sphereNode = new CubeNode(size, UIColor.Blue);
        _arView?.Scene.RootNode.AddChildNode(sphereNode);
    }
}
