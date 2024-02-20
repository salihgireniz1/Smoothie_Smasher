using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    BurstHandler fruit;
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<BurstHandler>(out fruit))
        {
            fruit.ReadyToBurst();
        }
    }
}
