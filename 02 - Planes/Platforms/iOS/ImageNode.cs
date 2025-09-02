using SceneKit;
using UIKit;

namespace ARKitDemo.Platforms.iOS;

public class ImageNode : SCNNode
{
    public ImageNode(float width, float length, UIImage? image)
    {
        var rootNode = new SCNNode
        {
            Geometry = CreateGeometry(width, length, image),
            Constraints = new[] { new SCNBillboardConstraint() } // Make the node always face the camera
        };
        AddChildNode(rootNode);
    }

    static SCNGeometry CreateGeometry(float width, float length, UIImage? image)
    {
        var material = new SCNMaterial();
        material.Diffuse.Contents = image;
        material.DoubleSided = true;

        var geometry = SCNPlane.Create(width, length);
        geometry.Materials = new[] { material };

        return geometry;
    }
}