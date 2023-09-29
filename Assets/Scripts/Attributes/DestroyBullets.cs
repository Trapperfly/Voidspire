using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullets : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            TrailRenderer bulletTR = bullet.GetComponent<TrailRenderer>();
            if (bullet._localBounce < 1)
            {
                if (bulletTR != null)
                {
                    StartCoroutine(DestroyTrailedBullet(bullet.gameObject, bulletTR));
                }
                else Destroy(bullet.gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            TrailRenderer bulletTR = bullet.GetComponentInChildren<TrailRenderer>();
            if (bullet._localPierce < 1)
            {
                if (bulletTR != null)
                {
                    StartCoroutine(DestroyTrailedBullet(bullet.gameObject, bulletTR));
                }
                else Destroy(bullet.gameObject);
            }
        }
        if (collision.CompareTag("AIBullet"))
        {
            AIBullet bullet = collision.gameObject.GetComponent<AIBullet>();
            TrailRenderer bulletTR = bullet.GetComponentInChildren<TrailRenderer>();
            if (bulletTR != null)
            {
                StartCoroutine(DestroyTrailedBullet(bullet.gameObject, bulletTR));
            }
            else
            {
                Destroy(bullet.gameObject);
            }
        }
    }

    IEnumerator DestroyTrailedBullet(GameObject bullet, TrailRenderer tr)
    {
        /*
        Debug.Log("Found trail renderer on bullet, turning off all components except the trail renderer");
        if ( bullet.GetComponent<SpriteRenderer>() != null)
            bullet.GetComponent<SpriteRenderer>().enabled = false;
        bullet.GetComponent<Collider2D>().enabled = false;
        bullet.GetComponent<Rigidbody2D>().simulated = false;
        Debug.Log("Waiting until trail renderer is done rendering");
        yield return new WaitForSeconds(tr.time);
        Debug.Log("Destroying bullet");
        Destroy(bullet);
        yield return null;
        */
        tr.transform.parent = null;
        tr.autodestruct = true;
        Destroy(bullet);
        yield return null;
    }
}
