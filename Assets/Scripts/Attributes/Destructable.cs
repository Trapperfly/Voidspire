using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Destructable : MonoBehaviour
{
    //[SerializeField] float delay = 0f;
    [SerializeField] bool addParticleEffect;
    [SerializeField] GameObject psPrefab;
    [SerializeField] float psLifetime;
    ParticleSystem ps;
    public IEnumerator DestroyMe()
    {
        if (addParticleEffect) ps = Instantiate(psPrefab, transform).GetComponent<ParticleSystem>();
        Destroy(gameObject, 0);
        yield return null;
    }
    private void OnDestroy()
    {
        if (Random.value < 0.1f) AudioManager.Instance.explorationTime = 100;
        TryGetComponent(out Damagable health);
        if (health && health.currentHealth <= 0) {
            AudioManager.Instance.PlayEmitter(FMODEvents.Instance.enemyActions, transform, 1);
            //emitter.transform.SetParent(null);
        }
        if (addParticleEffect)
        {
            var emis = ps.emission;
            emis.enabled = false;
            ps.transform.parent = null;
            Destroy(ps.gameObject, psLifetime);
        }
    }
}
