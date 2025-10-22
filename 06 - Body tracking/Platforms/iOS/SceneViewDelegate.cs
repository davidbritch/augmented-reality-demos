using ARKit;
using CoreGraphics;
using Foundation;
using SceneKit;
using UIKit;

namespace ARKitDemo.Platforms.iOS;

public class SceneViewDelegate : ARSCNViewDelegate
{
    Dictionary<string, JointNode> _joints = new Dictionary<string, JointNode>();

    public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
    {
        if (!(anchor is ARBodyAnchor bodyAnchor))
            return;

        foreach (var jointName in ARSkeletonDefinition.DefaultBody3DSkeletonDefinition.JointNames)
        {
            JointNode jointNode = MakeJoint(jointName);
            var jointPosition = GetJointPosition(bodyAnchor, jointName);
            jointNode.Position = jointPosition;

            if (!_joints.ContainsKey(jointName))
            {
                node.AddChildNode(jointNode);
                _joints.Add(jointName, jointNode);
            }
        }
    }

    public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
    {
        if (!(anchor is ARBodyAnchor bodyAnchor))
            return;

        foreach (var jointName in ARSkeletonDefinition.DefaultBody3DSkeletonDefinition.JointNames)
        {
            var jointPosition = GetJointPosition(bodyAnchor, jointName);

            if (!_joints.ContainsKey(jointName))
                _joints[jointName].Update(jointPosition);
        }            
    }

    SCNVector3 GetJointPosition(ARBodyAnchor bodyAnchor, string jointName)
    {
        NMatrix4 jointTransform = bodyAnchor.Skeleton.GetModelTransform((NSString)jointName);
        return new SCNVector3(jointTransform.Column3);
    }

    UIColor GetJointColour(string jointName)
    {
        switch (jointName)
        {
            case "root":
            case "left_foot_joint":
            case "right_foot_joint":
            case "left_leg_joint":
            case "right_leg_joint":
            case "left_hand_joint":
            case "right_hand_joint":
            case "left_arm_joint":
            case "right_arm_joint":
            case "left_forearm_joint":
            case "right_forearm_joint":
            case "head_joint":
                return UIColor.Red;
        }

        return UIColor.Gray;
    }

    float GetJointRadius(string jointName)
    {
        switch (jointName)
        {
            case "root":
            case "left_foot_joint":
            case "right_foot_joint":
            case "left_leg_joint":
            case "right_leg_joint":
            case "left_hand_joint":
            case "right_hand_joint":
            case "left_arm_joint":
            case "right_arm_joint":
            case "left_forearm_joint":
            case "right_forearm_joint":
            case "head_joint":
                return 0.04f;
        }

        if (jointName.Contains("hand"))
            return 0.01f;

        return 0.02f;
    }

    JointNode MakeJoint(string jointName)
    {
        var jointNode = new JointNode();
        var material = new SCNMaterial();
        material.Diffuse.Contents = GetJointColour(jointName);

        var jointGeometry = SCNSphere.Create(GetJointRadius(jointName));
        jointGeometry.FirstMaterial = material;
        jointNode.Geometry = jointGeometry;

        return jointNode;
    }
}