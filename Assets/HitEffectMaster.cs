using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectMaster : MonoBehaviour
{
    [SerializeField] GameObject lurkerHitEffect;
    [SerializeField] GameObject electricHitEffect;
    [SerializeField] GameObject playerHitEffect;
    GameObject effect;
    public void SpawnHitEffect(Transform hit,Vector2 position, string type)
    {
        switch (type)
        {
            //NEED MASSIVE REWORK TO WORK WITH DIFFERENT PROJECTILE TYPES AND ENEMIES
            case "Void":
                effect = Instantiate(lurkerHitEffect, position, new Quaternion(), hit);
                //Use bullet size or other variable for this multiplier
                break;
            case "Bullet":
                effect = Instantiate(playerHitEffect, position, new Quaternion(), hit);
                //Use bullet size or other variable for this multiplier
                break;
            case "Electric":
                effect = Instantiate(electricHitEffect, position, new Quaternion(), hit);
                //Use bullet size or other variable for this multiplier
                break;
            default:
                break;
        }
    }
}
