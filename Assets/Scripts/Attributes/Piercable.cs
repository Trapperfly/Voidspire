using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = hit.gameObject.GetComponent<Bullet>();
            if (hit.gameObject.CompareTag("Bullet"))
            {
                if (bullet._localPierce > 0)
                {
                    bullet._localPierce--;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = hit.gameObject.GetComponent<Bullet>();
            if (bullet._localBounce > 0 && bullet._localPierce < 1)
            {
                GetComponent<Collider2D>().isTrigger = false;
                Debug.Log("Turning off trigger // " + bullet._localBounce + " bounces");
            }
        }
    }
}