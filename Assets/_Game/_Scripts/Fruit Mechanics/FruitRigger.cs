using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitRigger : MonoBehaviour
{
    public Rigidbody centerRigidBody;
    public float damper;
    public float spring;
    public Rigidbody[] boneBodies;
    FixedJoint fixedJoint;
    SpringJoint springJoint;
    public Dictionary<Rigidbody, Vector3> defPositionDict = new();
    public Dictionary<Rigidbody, Quaternion> defRotationDict = new();

    [Button]
    public void InitRigs()
    {
        AddFixedToCenter();
        //AddFixedToParent();

        foreach (var bone in boneBodies)
        {
            if (!defRotationDict.ContainsKey(bone))
            {
                defPositionDict.Add(bone, bone.position);
                defRotationDict.Add(bone, bone.rotation);
            }
            if (!bone.TryGetComponent<SpringJoint>(out springJoint))
            {
                springJoint = bone.AddComponent<SpringJoint>();
            }
            springJoint.connectedBody = centerRigidBody;
            springJoint.spring = spring;
            springJoint.damper = damper;
        }
    }
    [Button]
    public void ResetRigs()
    {
        foreach (var bone in defPositionDict)
        {
            if (bone.Key.TryGetComponent<SpringJoint>(out springJoint))
            {
                Destroy(springJoint);
            }
            bone.Key.position = bone.Value;
            bone.Key.rotation = defRotationDict[bone.Key];
        }
        if (centerRigidBody.TryGetComponent(out fixedJoint))
        {
            Destroy(fixedJoint);
        }
    }
    void AddFixedToCenter()
    {
        if (!centerRigidBody.TryGetComponent(out fixedJoint))
        {
            fixedJoint = centerRigidBody.gameObject.AddComponent<FixedJoint>();

        }

        if (!gameObject.TryGetComponent(out Rigidbody body))
        {
            body = gameObject.AddComponent<Rigidbody>();
        }
        fixedJoint.connectedBody = body;

        fixedJoint.connectedBody.drag = 5f;
        fixedJoint.connectedBody.mass = 0.15f;
    }
    void AddFixedToParent()
    {
        fixedJoint = this.gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = centerRigidBody;
    }
}
