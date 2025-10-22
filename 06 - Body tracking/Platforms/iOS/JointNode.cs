using SceneKit;

namespace ARKitDemo.Platforms.iOS;

public class JointNode : SCNNode
{
    public void Update(SCNVector3 position)
    {
        Position = position;
    }
}