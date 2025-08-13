using SceneKit;
using UIKit;

namespace ARKitDemo.Services;

public class CubeNode : SCNNode
{
    public CubeNode(float size, UIColor color)
    {
        var rootNode = new SCNNode
        {
            Geometry = CreateGeometry(size, color)
        };
        AddChildNode(rootNode);
    }

    static SCNGeometry CreateGeometry(float size, UIColor color)
    {
        var material = new SCNMaterial();
        material.Diffuse.Contents = color;

        var geometry = SCNBox.Create(size, size, size, 0);
        geometry.Materials = new[] { material };

        return geometry;
    }
}