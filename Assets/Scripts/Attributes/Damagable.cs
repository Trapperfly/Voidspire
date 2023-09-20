using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] float startHealth = 0;
    [SerializeField] float currentHealth;


    private void Awake()
    {
        float size = (transform.localScale.x + transform.localScale.y) / 2;
        if (startHealth == 0)
            startHealth = Mathf.Pow(size + 1, 3);
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
    }

    void HealthCheck()
    {
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
