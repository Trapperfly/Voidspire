using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = hit.gameObject.GetComponent<Bullet>();
            if (GetComponent<Rigidbody2D>() != null)
                GetComponent<Rigidbody2D>().AddForce(bullet.gameObject.transform.up * bullet._punch);
            else if (GetComponentInChildren<Rigidbody2D>() != null)
                GetComponentInChildren<Rigidbody2D>().AddForce(bullet.gameObject.transform.up * bullet._punch, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = hit.gameObject.GetComponent<Bullet>();
            if (GetComponent<Rigidbody2D>() != null)
                GetComponent<Rigidbody2D>().AddForce(bullet.gameObject.transform.up * bullet._punch);
            else if (GetComponentInChildren<Rigidbody2D>() != null)
                GetComponentInChildren<Rigidbody2D>().AddForce(bullet.gameObject.transform.up * bullet._punch, ForceMode2D.Impulse);
        }
    }
}
