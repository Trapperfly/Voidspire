using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float startHealth = 0;
    public float currentHealth;
    public bool damageTaken;
    private void Start()
    {
        if (startHealth == 0)
        {
            float size = (transform.localScale.x + transform.localScale.y) / 2;
            startHealth = Mathf.Pow(size * 2 + 1f, 2);
        }
        currentHealth = startHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.collider.CompareTag("Bullet"))
            {
                TakeDamage(collision.collider.GetComponent<Bullet>()._localDamage);
            }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            if (collision.CompareTag("Bullet"))
            {
                TakeDamage(collision.GetComponent<Bullet>()._localDamage);
            }
            if (collision.CompareTag("AIBullet"))
            {
                TakeDamage(collision.GetComponent<AIBullet>().damage);
            }
    }

    public void TakeDamage(float value)
    {
        currentHealth -= value;
        HealthCheck();
    }

    public void HealthCheck()
    {
        damageTaken = true;
        if (currentHealth <= 0)
        {
            if (GetComponent<Destructable>() != null)
            {
                if (GetComponent<DropsLoot>() != null)
                    GetComponent<DropsLoot>().noDrop = false;
                GetComponent<Destructable>().StartCoroutine(nameof(Destructable.DestroyMe));
            }
        }
    }
}
