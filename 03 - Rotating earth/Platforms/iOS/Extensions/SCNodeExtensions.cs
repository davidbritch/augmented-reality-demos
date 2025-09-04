using SceneKit;

namespace ARKitDemo.Platforms.iOS;

public static class SCNNodeExtensions
{
    public static void AddRotationAction(this SCNNode node, SCNActionTimingMode mode, double secs, bool loop = false)
    {
        SCNAction rotateAction = SCNAction.RotateBy(0, (float)Math.PI, 0, secs);
        rotateAction.TimingMode = mode;

        if (loop)
        {
            SCNAction indefiniteRotation = SCNAction.RepeatActionForever(rotateAction);
            node.RunAction(indefiniteRotation, "rotation");
        }
        else
            node.RunAction(rotateAction, "rotation");
    }
}