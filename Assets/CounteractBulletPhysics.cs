using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounteractBulletPhysics : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 vel;
    private float angularVel;
    private Vector3 position;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
            if (collision.collider.CompareTag("Bullet"))
            {
                collision.collider.isTrigger = true;
            }
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
            collision.isTrigger = false;
        }
    }
}
