using ARKit;
using UIKit;
using CoreGraphics;
using SceneKit;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ARKitDemo.Platforms.iOS;

public class MauiARView : UIView
{
    ARSCNView? _arView;
    ARSession? _arSession;
    UITapGestureRecognizer? _tapGesture;
    UIPinchGestureRecognizer? _pinchGesture;
    UIRotationGestureRecognizer? _rotateGesture;
    bool _gestureRecognizersAdded;
    SCNNode? _node;
    bool _isAnimating;
    float _zAngle;

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
        RemoveGestureRecognizers();
        
        if (_isAnimating)
        {
            _node?.RemoveAction("rotation");
            _isAnimating = false;
        }
        _node?.RemoveFromParentNode();

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

        _rotateGesture = new UIRotationGestureRecognizer(HandleRotateGesture);
        _arView?.AddGestureRecognizer(_rotateGesture);

        _gestureRecognizersAdded = true;
    }

    void RemoveGestureRecognizers()
    {
        if (_gestureRecognizersAdded)
        {
            _arView?.RemoveGestureRecognizer(_tapGesture!);
            _arView?.RemoveGestureRecognizer(_pinchGesture!);
            _arView?.RemoveGestureRecognizer(_rotateGesture!);
            _gestureRecognizersAdded = false;
        }
    }

    void AddContent()
    {
        UIImage? image = UIImage.FromFile("worldmap.jpg");
        _node = new SphereNode(image, 0.005f);
        _arView?.Scene.RootNode.AddChildNode(_node);
    }

    void HandleTapGesture(UITapGestureRecognizer? sender)
    {
        SCNView? areaTapped = sender?.View as SCNView;
        CGPoint? location = sender?.LocationInView(areaTapped);
        SCNHitTestResult[]? hitTestResults = areaTapped?.HitTest((CGPoint)location!, new SCNHitTestOptions());
        SCNHitTestResult? hitTest = hitTestResults?.FirstOrDefault();

        SCNNode? node = hitTest?.Node;
        if (!_isAnimating)
        {
            node?.AddRotationAction(SCNActionTimingMode.Linear, 10, true);
            _isAnimating = true;
        }
        else
        {
            node?.RemoveAction("rotation");
            _isAnimating = false;
        }
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

    void HandleRotateGesture(UIRotationGestureRecognizer? sender)
    {
        SCNView? areaPinched = sender?.View as SCNView;
        CGPoint? location = sender?.LocationInView(areaPinched);
        SCNHitTestResult[]? hitTestResults = areaPinched?.HitTest((CGPoint)location!, new SCNHitTestOptions());
        SCNHitTestResult? hitTest = hitTestResults?.FirstOrDefault();

        if (hitTest == null)
            return;

        SCNNode node = hitTest.Node;
        _zAngle += (float)-sender!.Rotation;
        node.EulerAngles = new SCNVector3(node.EulerAngles.X, node.EulerAngles.Y, _zAngle);
    }
}
