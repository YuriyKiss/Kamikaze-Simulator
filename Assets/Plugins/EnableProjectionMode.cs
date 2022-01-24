using UnityEngine;

public class EnableProjectionMode : MonoBehaviour
{
    private void Start()
    {
        ConfigurableJoint[] joints = GetComponentsInChildren<ConfigurableJoint>();

        foreach (ConfigurableJoint joint in joints)
        {
            joint.projectionMode = JointProjectionMode.PositionAndRotation;
        }
    }
}
