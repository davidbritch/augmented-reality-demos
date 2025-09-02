using ARKit;
using UIKit;
using CoreGraphics;
using SceneKit;

namespace ARKitDemo.Platforms.iOS;

public class MauiARView : UIView
{
    ARSCNView? _arView;
    ARSession? _arSession;
    UITapGestureRecognizer? _tapGesture;
    UIPinchGestureRecognizer? _pinchGesture;
    UIPanGestureRecognizer? _panGesture;
    float _currentAngleZ;
    float _newAngleZ;

    public static bool IsARSupported()
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
            PlaneDetection = ARPlaneDetection.None,
            WorldAlignment = ARWorldAlignment.GravityAndHeading
        }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

        AddGestureRecognizers();
        AddContent();
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

    void AddGestureRecognizers()
    {
        _tapGesture = new UITapGestureRecognizer(HandleTapGesture);
        _arView?.AddGestureRecognizer(_tapGesture);

        _pinchGesture = new UIPinchGestureRecognizer(HandlePinchGesture);
        _arView?.AddGestureRecognizer(_pinchGesture);

        _panGesture = new UIPanGestureRecognizer(HandlePanGesture);
        _arView?.AddGestureRecognizer(_panGesture);
    }

    void AddContent()
    {
        float width = 0.1f;
        float height = 0.1f;

        UIImage? image = UIImage.FromFile("dotnet_bot.png");
        var imageNode = new ImageNode(image, width, height);
        _arView?.Scene.RootNode.AddChildNode(imageNode);
    }

    void HandleTapGesture(UITapGestureRecognizer? sender)
    {
        SCNView? areaTapped = sender?.View as SCNView;
        CGPoint? location = sender?.LocationInView(areaTapped);
        SCNHitTestResult[]? hitTestResults = areaTapped?.HitTest((CGPoint)location!, new SCNHitTestOptions());
        SCNHitTestResult? hitTest = hitTestResults?.FirstOrDefault();

        SCNNode? node = hitTest?.Node;
        node?.RemoveFromParentNode();
        _arView?.RemoveGestureRecognizer(_tapGesture!);
    }

    void HandlePinchGesture(UIPinchGestureRecognizer? sender)
    {
        SCNView? areaPinched = sender?.View as SCNView;
        CGPoint? location = sender?.LocationInView(areaPinched);
        SCNHitTestResult[]? hitTestResults = areaPinched?.HitTest((CGPoint)location!, new SCNHitTestOptions());
        SCNHitTestResult? hitTest = hitTestResults?.FirstOrDefault();

        if (hitTest == null)
            return;

        SCNNode node = hitTest.Node;
        float scaleX = (float)sender.Scale * node.Scale.X;
        float scaleY = (float)sender.Scale * node.Scale.Y;
        float scaleZ = (float)sender.Scale * node.Scale.Z;

        node.Scale = new SCNVector3(scaleX, scaleY, scaleZ);
        sender.Scale = 1;
    }

    void HandlePanGesture(UIPanGestureRecognizer? sender)
    {
        SCNView? areaPanned = sender?.View as SCNView;
        CGPoint? location = sender?.LocationInView(areaPanned);
        SCNHitTestResult[]? hitTestResults = areaPanned?.HitTest((CGPoint)location!, new SCNHitTestOptions());
        SCNHitTestResult? hitTest = hitTestResults?.FirstOrDefault();

        SCNNode? node = hitTest?.Node;
        if (sender?.State == UIGestureRecognizerState.Changed)
        {
            CGPoint translate = sender.TranslationInView(areaPanned);
            node?.LocalTranslate(new SCNVector3((float)-translate.X / 10000, (float)-translate.Y / 10000, 0.0f));
        }        
    }
}
