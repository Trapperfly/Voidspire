using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : Events
{
    public float startHealth = 0;
    public float currentHealth;
    public bool damageTaken;
    private void Start()
    {
        if (startHealth == 0)
        {
            float size = (transform.localScale.x + transform.localScale.y);
            startHealth = Mathf.Pow(size * 2 + 1f, 3);
        }
        currentHealth = startHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.collider.CompareTag("Bullet"))
            {
                float damage = collision.collider.GetComponent<Bullet>()._localDamage;
                TakeDamage(damage, collision.transform.position);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            if (collision.CompareTag("Bullet"))
            {
                float damage = collision.GetComponent<Bullet>()._localDamage;
                TakeDamage(damage, collision.transform.position);
            }
            if (collision.CompareTag("AIBullet"))
            {
                float damage = collision.GetComponent<AIBullet>().damage;
                TakeDamage(damage, collision.transform.position);

            }
    }

    public void TakeDamage(float value, Vector2 position)
    {
        OnHitEvent(value, position);
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
    public override void OnHitEvent(float damage, Vector2 position)
    {
        base.OnHitEvent(damage, position);

    }
}
