using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int healthNodes;
    public int healthNodesMax;
    float healthPercent;

    public Transform healthBar;
    Image healthBarImage;
    float lastFrameHealth;
    float lastFrameMaxHealth;

    public PassiveShield shield;
    public GameObject deathMenu;

    private void Awake()
    {
        healthBarImage = healthBar.GetChild(2).GetComponent<Image>();
        currentHealth = maxHealth;
        UpdateSize();
        UpdateHealth();
    }

    private void Update()
    {
        if (lastFrameMaxHealth != maxHealth) UpdateSize();
        if (lastFrameHealth != currentHealth) UpdateHealth();
        lastFrameHealth = currentHealth;
        lastFrameMaxHealth = maxHealth;
    }

    void UpdateHealth()
    {
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        healthPercent = currentHealth / maxHealth;
        healthBarImage.fillAmount = healthPercent;
    }

    void UpdateSize()
    {
        foreach (RectTransform child in healthBar)
        {
            float _backModifier = 0;
            if (child == healthBar.GetChild(0)) _backModifier = 0.05f;
            child.sizeDelta = new Vector2((maxHealth / 10) + _backModifier, child.sizeDelta.y);
        }
        UpdateHealth();
    }

    #region collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            if (shield != null && shield.shieldActiveForColliders)
            {

            } else currentHealth -= collision.collider.GetComponent<Bullet>()._localDamage;
            HealthCheck();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bullet"))
        {
            if (shield != null && shield.shieldActiveForColliders)
            {

            }else currentHealth -= collision.GetComponent<Bullet>()._localDamage;
            HealthCheck();
        }
        if (collision.CompareTag("AIBullet"))
        {
            if (shield != null && shield.shieldActiveForColliders)
            {

            }else currentHealth -= collision.GetComponent<AIBullet>().damage;
            HealthCheck();
        }
    }
    #endregion
    public void HealthCheck()
    {
        shield.rechargeTimer = 0;
        if (currentHealth <= 0)
        {
            //TMP solution
            Time.timeScale = 0;
            deathMenu.SetActive(true);
            //Start taking damage to nodes
            //Check if all nodes are destroyed
            //Then death and end screen
        }
    }
}
