using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercable : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 vel;
    private float angularVel;
    private Vector3 position;
    //List<Collider2D> inside = new List<Collider2D>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<Collider2D>();
    }
    private void FixedUpdate()
    {
        if (rb != null)
        {
            vel = rb.velocity;
            angularVel = rb.angularVelocity;
            position = transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            //if (!inside.Contains(collision.collider))
                StartCoroutine(WaitForIgnoreCollision(col, collision.collider));
            if (rb != null)
            {
                rb.velocity = vel;
                rb.angularVelocity = angularVel;
                transform.position = position;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            collision.GetComponent<Collider2D>().isTrigger = false;
        }
    }
    private IEnumerator WaitForIgnoreCollision(Collider2D a, Collider2D b)
    {
        //if (!inside.Contains(b))
            //inside.Add(b);
        float waitTime = 0.5f;
            //Physics2D.IgnoreCollision(a, b, true);
            b.isTrigger = true;
            yield return new WaitForSeconds(waitTime);
            //if (b != null)
                //Physics2D.IgnoreCollision(a, b, false);
    }
}