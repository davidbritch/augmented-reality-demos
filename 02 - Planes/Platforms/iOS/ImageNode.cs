using SceneKit;
using UIKit;

namespace ARKitDemo.Platforms.iOS;

public class ImageNode : SCNNode
{
    public ImageNode(UIImage? image, float width, float height)
    {
        var rootNode = new SCNNode
        {
            Geometry = CreateGeometry(image, width, height),
            Constraints = new[] { new SCNBillboardConstraint() } // Make the node always face the camera
        };
        AddChildNode(rootNode);
    }

    static SCNGeometry CreateGeometry(UIImage? image, float width, float height)
    {
        var material = new SCNMaterial();
        material.Diffuse.Contents = image;
        material.DoubleSided = true;

        var geometry = SCNPlane.Create(width, height);
        geometry.Materials = new[] { material };

        return geometry;
    }
}