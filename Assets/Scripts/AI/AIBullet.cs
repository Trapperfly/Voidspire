using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBullet : MonoBehaviour
{
    public enum EnemyProjectileType
    {
        Void,
        Electric,
        Default
    }
    public EnemyProjectileType type;
    public bool indestructible;
    public float damageTimer;
    public float damage;
    public float currTime;
    public float timer;
    Collider2D col;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        currTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (damageTimer > 0)
        {
            if (timer > damageTimer)
            {
                StartCoroutine(OneFrameCollider());
                timer = 0;
            }
            timer++;
        }
    }

    IEnumerator OneFrameCollider()
    {
        col.enabled = true;
        yield return new WaitForFixedUpdate();
        col.enabled = false;
        yield return null;
    }
}
