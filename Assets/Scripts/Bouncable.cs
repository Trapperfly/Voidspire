using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncable : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = hit.gameObject.GetComponent<Bullet>();
            Collider2D col = hit.gameObject.GetComponent<Collider2D>();
            Rigidbody2D rb = hit.gameObject.GetComponent<Rigidbody2D>();
            if (bullet._bounce > 0)
            {
                col.isTrigger = true;
                foreach (var contact in hit.contacts)
                {
                    Vector2 _bounceDir = Vector2.Reflect(bullet.lastVelocity, contact.normal).normalized;
                    rb.velocity = _bounceDir * bullet._speed;
                    bullet.bounced = true;
                    if (bullet._homing && bullet.target != null)
                        StartCoroutine(bullet.WaitAndSwitchHoming(0.01f));
                }
                rb.angularVelocity = 0;
                bullet._bounce--;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = hit.gameObject.GetComponent<Bullet>();
            if (bullet._bounce > 0 && bullet._pierce < 1)
            {
                GetComponent<Collider2D>().isTrigger = false;
                Debug.Log("Turning off trigger // " + bullet._bounce + " bounces");
            }
        }
    }
}
