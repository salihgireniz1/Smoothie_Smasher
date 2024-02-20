using System;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public static event Action OnTransitionDone;

    public void CompleteTransition()
    {
        OnTransitionDone?.Invoke();
    }
}
