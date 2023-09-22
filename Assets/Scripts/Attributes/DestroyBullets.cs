using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullets : MonoBehaviour
{
    Collider2D me;
    private void Awake()
    {
        me = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.collider.CompareTag("Bullet"))
            {
                Bullet bullet = collision.gameObject.GetComponent<Bullet>();
                if (bullet._localBounce < 1)
                {
                    Destroy(bullet.gameObject);
                }
            }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Bullet"))
            {
                Bullet bullet = collision.gameObject.GetComponent<Bullet>();
                if (bullet._localPierce < 1)
                    Destroy(bullet.gameObject);
            }
            if (collision.CompareTag("AIBullet"))
            {
                Destroy(collision.gameObject);
            }
    }
}
