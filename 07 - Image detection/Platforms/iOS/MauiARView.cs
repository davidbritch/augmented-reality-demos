using ARKit;
using Foundation;
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

        NSSet<ARReferenceImage>? images = ARReferenceImage.GetReferenceImagesInGroup("AR Resources", null);

        _arSession.Run(new ARWorldTrackingConfiguration()
        {
            AutoFocusEnabled = true,
            LightEstimationEnabled = true,
            DetectionImages = images
        }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);
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
