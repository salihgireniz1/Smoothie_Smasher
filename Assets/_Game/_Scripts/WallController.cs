using PAG.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class WallController : MonoSingleton<WallController>
{
    public WallRigger[] wallsToInitialize;

    void Start()
    {
        //InitializeWalls();
    }

    [Button]
    public void InitializeWalls()
    {
        foreach (var wallRigger in wallsToInitialize)
        {
            wallRigger.AddJointsToBones();
        }
        //Debug.Log("WALL RIGS REINITIALIZED!");
    }

    [Button]
    public void ResetWalls()
    {
        StartCoroutine(ResetWallsPerFrame());
    }
    [Button]
    public IEnumerator ResetWallsPerFrame()
    {
        foreach (var wallRigger in wallsToInitialize)
        {
            wallRigger.ResetJoints();
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log("WALL RIGS RESET!");
    }
}