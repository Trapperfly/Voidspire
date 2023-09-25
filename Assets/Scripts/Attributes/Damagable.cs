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
                currentHealth -= collision.collider.GetComponent<Bullet>()._localDamage;
                HealthCheck();
            }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            if (collision.CompareTag("Bullet"))
            {
                currentHealth -= collision.GetComponent<Bullet>()._localDamage;
                HealthCheck();
            }
            if (collision.CompareTag("AIBullet"))
            {
                currentHealth -= collision.GetComponent<AIBullet>().damage;
                HealthCheck();
            }
    }
    public void HealthCheck()
    {
        damageTaken = true;
        if (currentHealth <= 0)
        {
            if (GetComponent<Destructable>() != null)
            {
                if (GetComponent<DropsResources>() != null)
                    GetComponent<DropsResources>().noDrop = false;
                GetComponent<Destructable>().StartCoroutine(nameof(Destructable.DestroyMe));
            }
        }
    }
}
