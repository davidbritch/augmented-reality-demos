using SceneKit;
using UIKit;

namespace ARKitDemo.Platforms.iOS;

public class SphereNode : SCNNode
{
    public SphereNode(UIImage? image, float size)
    {
        var node = new SCNNode
        {
            Geometry = CreateGeometry(image, size),
            Opacity = 0.975f
        };

        AddChildNode(node);
    }

    SCNGeometry CreateGeometry(UIImage? image, float size)
    {
        SCNMaterial material = new SCNMaterial();
        material.Diffuse.Contents = image;
        material.DoubleSided = true;

        SCNSphere geometry = SCNSphere.Create(size);
        geometry.Materials = new[] { material };

        return geometry;
    }
}