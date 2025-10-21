using ARKit;
using UIKit;

namespace ARKitDemo.Platforms.iOS;

public class MauiARView : UIView
{
    ARSCNView? _arView;
    ARSession? _arSession;

    public static bool IsARSupported()
    {
        return ARConfiguration.IsSupported;
    }

    public void StartARSession()
    {
        if (_arSession == null)
            return;

        _arSession.Run(new ARFaceTrackingConfiguration()
        {
            LightEstimationEnabled = true,
            MaximumNumberOfTrackedFaces = 1
        });
    }

    public void StopARSession()
    {
        _arSession?.Pause();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _arSession?.Dispose();
            _arView?.Dispose();

            _arSession = null;
            _arView = null;
        }
        base.Dispose(disposing);
    }

    internal void SetARView(ARSCNView view)
    {
        _arView = view;
        _arSession = view.Session;
    }
}
