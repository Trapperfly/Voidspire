using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPS : MonoBehaviour
{
    ParticleSystem ps;

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        //var emis = ps.emission;
        //emis.rateOverTime = 0;
        ps.Stop(false);
        Destroy(gameObject, ps.main.startLifetime.Evaluate(0.5f));
    }
}
