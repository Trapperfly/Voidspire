using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTarget : MonoBehaviour
{
    public bool isPlayer;
    public Transform target = null;
    public Rigidbody2D targetRB = null;
    Damagable targetDMG;

    public IEnumerator InitTargetValues(Transform tt)
    {
        StartCoroutine(InitTargetValues(tt, null));
        yield return null;
    }
    public IEnumerator InitTargetValues(Transform tt, Rigidbody2D rb)
    {
        target = tt;
        target.TryGetComponent(out targetDMG);
        if (targetDMG && !targetDMG.isBoss && !targetDMG.hb)
        {
             targetDMG.Init();
        }
        Debug.Log(rb);
        if (rb != null) targetRB = rb;
        yield return null;
    }
    
    public IEnumerator ClearTarget()
    {
        target = null;
        targetRB = null;
        targetDMG = null;
        yield return null;
    }
    private void FixedUpdate()
    {
        if (!isPlayer) { return; }
        if (target == null) { return; }
        if (targetDMG)
        {
            if (targetDMG.hb)
                targetDMG.hb.timer = 0;
        }
    }
}
