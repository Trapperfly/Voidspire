using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBullet : MonoBehaviour
{
    public GameObject bulletSender;
    public enum EnemyProjectileType
    {
        Void,
        Fire,
        Electric,
        Default
    }
    public EnemyProjectileType type;
    public bool indestructible;
    public float damageTimer;
    public float damage;
    public float currTime;
    public float timer = 0;
    public bool homing;
    public float homingStrength;
    public float speed;
    public Transform target;
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
            if (timer > damageTimer * 60)
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
