using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] float delay = 0f;
    [SerializeField] bool addParticleEffect;
    [SerializeField] GameObject psPrefab;
    [SerializeField] float psLifetime;
    ParticleSystem ps;
    public IEnumerator DestroyMe()
    {
        if (addParticleEffect) ps = Instantiate(psPrefab, transform).GetComponent<ParticleSystem>();
        Destroy(gameObject, delay);
        yield return null;
    }
    private void OnDestroy()
    {
        if (addParticleEffect)
        {
            var emis = ps.emission;
            emis.enabled = false;
            ps.transform.parent = null;
            Destroy(ps.gameObject, psLifetime);
        }
    }
}
