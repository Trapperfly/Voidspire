using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectMaster : MonoBehaviour
{
    [SerializeField] GameObject lurkerHitEffect;
    [SerializeField] GameObject playerHitEffect;
    GameObject effect;
    [SerializeField] float effectSize;
    public void SpawnHitEffect(GameObject hit,Vector2 position, string tag)
    {
        switch (tag)
        {
            //NEED MASSIVE REWORK TO WORK WITH DIFFERENT PROJECTILE TYPES AND ENEMIES
            case "AIBullet":
                effect = Instantiate(lurkerHitEffect, position, new Quaternion(), hit.transform.GetChild(0).transform);
                //Use bullet size or other variable for this multiplier
                effect.transform.localScale *= effectSize;
                break;
            case "Bullet":
                effect = Instantiate(playerHitEffect, position, new Quaternion(), hit.transform.GetChild(0).transform);
                //Use bullet size or other variable for this multiplier
                effect.transform.localScale *= effectSize;
                break;
            default:
                break;
        }
    }
}
