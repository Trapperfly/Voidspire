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
            if (bullet._bounce < 1)
                if (!hit.gameObject.CompareTag("Bullet"))
                {
                    if (bullet._pierce > 0)
                    {
                        bullet._pierce--;
                    }
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