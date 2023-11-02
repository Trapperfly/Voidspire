using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTarget : MonoBehaviour
{
    public Transform target = null;
    public Rigidbody2D targetRB = null;

    public IEnumerator InitTargetValues(Transform tt)
    {
        StartCoroutine(InitTargetValues(tt, null));
        yield return null;
    }
    public IEnumerator InitTargetValues(Transform tt, Rigidbody2D rb)
    {
        target = tt;
        Debug.Log(rb);
        if (rb != null) targetRB = rb;
        yield return null;
    }
    

    public IEnumerator ClearTarget()
    {
        target = null;
        targetRB = null;
        yield return null;
    }
}
