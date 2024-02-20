using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WallRigger : MonoBehaviour
{
    [Header("Rigidbody Initializing Settings")]
    public Transform startPoint;
    public List<Rigidbody> sortedBodies = new();
    public Rigidbody[] bodies;
    public int ignoredBodyAmount = 1;
    private Rigidbody previousBody;

    [Header("SpringJoint Settings")]
    public float spring;
    public float damper;

    SpringJoint lastSpringJoint;
    List<Vector3> sortedPositions = new();

    void Start()
    {
        AddJointsToBones();
    }

    [Button]
    public void ResetJoints()
    {
        for (int i = 0; i < sortedBodies.Count; i++)
        {
            sortedBodies[i].isKinematic = true;
            if (sortedBodies[i].TryGetComponent(out CapsuleCollider collider))
            {
                collider.isTrigger = true;
            }
            sortedBodies[i].transform.position = sortedPositions[i];
        }

        SpringJoint[] springJointsToRemove = GetComponentsInChildren<SpringJoint>();
        FixedJoint[] fixedJointsToRemove = GetComponentsInChildren<FixedJoint>();
        
        foreach (var sj in springJointsToRemove)
        {
            Destroy(sj);
        }

        foreach (var fj in fixedJointsToRemove)
        {
            Destroy(fj);
        }
        
    }
    [Button]
    public void AddJointsToBones()
    {
        bodies = GetComponentsInChildren<Rigidbody>();
        lastSpringJoint = null;

        // Calculate distances for all bodies
        Dictionary<Rigidbody, float> bodyDistances = new Dictionary<Rigidbody, float>();
        
        for (int i = ignoredBodyAmount; i < bodies.Length; i++)
        {
            bodies[i].isKinematic = false;
            if (bodies[i].TryGetComponent(out CapsuleCollider collider))
            {
                collider.isTrigger = false;
            }
            float distance = Vector3.Distance(startPoint.position, bodies[i].transform.position);
            bodyDistances.Add(bodies[i], distance);
        }

        // Sort the bodies based on distance
        sortedBodies = new List<Rigidbody>(bodies.Length - ignoredBodyAmount);
        sortedPositions = new();

        foreach (var pair in bodyDistances.OrderBy(x => x.Value))
        {
            sortedBodies.Add(pair.Key);
            sortedPositions.Add(pair.Key.transform.position);
        }
        Rigidbody lastBody = sortedBodies[sortedBodies.Count - 1];
        for (int i = 0; i < sortedBodies.Count; i++)
        {
            //if (sortedBodies[i] == lastBody) continue;
            if (i == 0 || i == sortedBodies.Count - 1)
            {
                sortedBodies[i].AddComponent<FixedJoint>();
            }
            Rigidbody b = sortedBodies[i];
            lastSpringJoint = b.AddComponent<SpringJoint>();
            lastSpringJoint.spring = spring;
            lastSpringJoint.damper = damper;
            if(previousBody != null)
            {
                lastSpringJoint.connectedBody = previousBody;
            }

            previousBody = b;
        }
    }
    void AddJointsToMainObject()
    {
        foreach (Transform t in transform)
        {
            SpringJoint joint = gameObject.AddComponent<SpringJoint>();
            joint.connectedBody = t.GetComponent<Rigidbody>();
            joint.spring = spring;
            joint.damper = damper;
        }
    }
}
