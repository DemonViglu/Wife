using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSetting : MonoBehaviour
{
    public int lowRange;
    public int highRange;

    public float autoPlayTime;
    public int TheTimeToChat()
    {
        return Random.Range(lowRange, highRange);
    }
}
