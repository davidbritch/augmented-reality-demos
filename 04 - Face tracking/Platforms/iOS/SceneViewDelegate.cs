using ARKit;
using SceneKit;

namespace ARKitDemo.Platforms.iOS;

public class SceneViewDelegate : ARSCNViewDelegate
{
    public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
    {
        if (anchor is ARFaceAnchor faceAnchor)
        {
            ARSCNFaceGeometry? faceGeometry = ARSCNFaceGeometry.Create(renderer.Device!);
            node.Geometry = faceGeometry;
            node.Opacity = 0.8f;
        }
    }

    public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
    {
        if (anchor is ARFaceAnchor)
        {
            ARFaceAnchor? faceAnchor = anchor as ARFaceAnchor;
            ARSCNFaceGeometry? faceGeometry = node.Geometry as ARSCNFaceGeometry;
            faceGeometry?.Update(faceAnchor!.Geometry);
        }
    }
}