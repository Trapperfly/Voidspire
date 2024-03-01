using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : GameTrigger
{
    public float startHealth = 0;
    public float currentHealth;
    public bool damageTaken;
    public bool shipOverride;
    public GameObject damageTakenFromWhat;
    public float healthPercent;
    private void Start()
    {
        if (shipOverride) { return; }
        if (startHealth == 0)
        {
            float size = (transform.localScale.x + transform.localScale.y);
            startHealth = Mathf.Pow(size + 1f, 3);
        }
        currentHealth = startHealth;
        healthPercent = currentHealth / startHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Bullet hitBy = collision.collider.GetComponent<Bullet>();
            float damage = hitBy._localDamage;
            TakeDamage(damage, collision.transform.position, hitBy.bulletSender);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Bullet"))
        {
            Bullet hitBy = collision.GetComponent<Bullet>();
            float damage = hitBy._localDamage;
            TakeDamage(damage, collision.transform.position, hitBy.bulletSender);
        }
        if (collision.CompareTag("AIBullet"))
        {
            AIBullet hitBy = collision.GetComponent<AIBullet>();
            float damage = hitBy.damage;
            TakeDamage(damage, collision.transform.position, hitBy.bulletSender);

        }
    }

    public void TakeDamage(float value, Vector2 position, GameObject whatDealtDamage)
    {
        OnHitEvent(value, position);
        damageTakenFromWhat = whatDealtDamage;
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
        healthPercent = currentHealth / startHealth;
    }
    public override void OnHitEvent(float damage, Vector2 position)
    {
        base.OnHitEvent(damage, position);

    }
}
