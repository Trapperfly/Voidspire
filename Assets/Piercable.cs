using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercable : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            collision.collider.isTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            collision.GetComponent<Collider2D>().isTrigger = false;
        }
    }
    //private IEnumerator WaitForIgnoreCollision(Collider2D bullet)
    //{
        //if (!inside.Contains(b))
        //inside.Add(b);
        //float waitTime = 0.5f;
        //Physics2D.IgnoreCollision(a, b, true);
        //bullet.isTrigger = true;
        //yield return new WaitForSeconds(waitTime);
        //if (b != null)
            //Physics2D.IgnoreCollision(a, b, false);
    //}
}