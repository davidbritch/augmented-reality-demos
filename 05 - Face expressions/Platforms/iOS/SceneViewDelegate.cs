using ARKit;
using SceneKit;
using UIKit;

namespace ARKitDemo.Platforms.iOS;

public class SceneViewDelegate : ARSCNViewDelegate
{
    public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
    {
        if (anchor is ARFaceAnchor)
        {
            ARSCNFaceGeometry? faceGeometry = ARSCNFaceGeometry.Create(renderer.Device!);
            node.Geometry = faceGeometry;
            node.Geometry.FirstMaterial.FillMode = SCNFillMode.Fill;
        }
    }

    public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
    {
        if (anchor is ARFaceAnchor)
        {
            ARFaceAnchor? faceAnchor = anchor as ARFaceAnchor;
            ARSCNFaceGeometry? faceGeometry = node.Geometry as ARSCNFaceGeometry;
            float expressionThreshold = 0.5f;

            faceGeometry?.Update(faceAnchor!.Geometry);

            if (faceAnchor?.BlendShapes.EyeWideLeft > expressionThreshold
                || faceAnchor?.BlendShapes.EyeWideRight > expressionThreshold)
            {
                ChangeFaceColour(node, UIColor.Blue);
                return;
            }

            if (faceAnchor?.BlendShapes.EyeBlinkLeft > expressionThreshold
                || faceAnchor?.BlendShapes.EyeBlinkRight > expressionThreshold)
            {
                ChangeFaceColour(node, UIColor.Green);
                return;
            }

            if (faceAnchor?.BlendShapes.MouthFrownLeft > expressionThreshold
                || faceAnchor?.BlendShapes.MouthFrownRight > expressionThreshold)
            {
                ChangeFaceColour(node, UIColor.SystemPink);
                return;
            }

            if (faceAnchor?.BlendShapes.MouthSmileLeft > expressionThreshold
                || faceAnchor?.BlendShapes.MouthSmileRight > expressionThreshold)
            {
                ChangeFaceColour(node, UIColor.Black);
                return;
            }

            if (faceAnchor?.BlendShapes.BrowOuterUpLeft > expressionThreshold
                || faceAnchor?.BlendShapes.BrowOuterUpRight > expressionThreshold)
            {
                ChangeFaceColour(node, UIColor.Magenta);
                return;
            }

            if (faceAnchor?.BlendShapes.EyeLookOutLeft > expressionThreshold
                || faceAnchor?.BlendShapes.EyeLookOutRight > expressionThreshold)
            {
                ChangeFaceColour(node, UIColor.Cyan);
                return;
            }

            if (faceAnchor?.BlendShapes.TongueOut > expressionThreshold)
            {
                ChangeFaceColour(node, UIColor.Yellow);
                return;
            }

            if (faceAnchor?.BlendShapes.MouthFunnel > expressionThreshold)
            {
                ChangeFaceColour(node, UIColor.Red);
                return;
            }

            if (faceAnchor?.BlendShapes.CheekPuff > expressionThreshold)
            {
                ChangeFaceColour(node, UIColor.Orange);
                return;
            }

            ChangeFaceColour(node, UIColor.White);
        }
    }

    void ChangeFaceColour(SCNNode faceGeometry, UIColor colour)
    {
        SCNMaterial material = new SCNMaterial();
        material.Diffuse.Contents = colour;
        material.FillMode = SCNFillMode.Fill;

        faceGeometry.Geometry.FirstMaterial = material;
    }
}