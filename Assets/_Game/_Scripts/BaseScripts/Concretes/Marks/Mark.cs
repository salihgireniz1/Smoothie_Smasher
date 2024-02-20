using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mark : MonoBehaviour
{
    public abstract void PlayAnimation();
    public abstract void SetMark(float value, string text);

}
