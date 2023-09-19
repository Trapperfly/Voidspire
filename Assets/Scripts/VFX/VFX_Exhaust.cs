using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class VFX_Exhaust : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called before the first frame update
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) //Move forward with force
        {
            var psEmis = ps.emission;
            psEmis.enabled = true;
        }
        else
        {
            var psEmis = ps.emission;
            psEmis.enabled = false;
        }
    }
}
