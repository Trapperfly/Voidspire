using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public int healthNodes;
    public int healthNodesMax;
    float healthPercent;

    public Transform healthBar;
    Image healthBarImage;

    public PassiveShield shield;
    public GameObject deathMenu;

    EquipmentController equipment;
    Hull hull;
    bool noHull;
    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
    }

    private void CustomStart()
    {
        Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        healthBarImage = healthBar.GetChild(2).GetComponent<Image>();
        SetNewStats();
        if (!noHull)
        {
            hull.hullCurrentHealth = hull.hullHealth;
        }

        EquipmentController.Instance.onEquipmentLoadComplete -= CustomStart;
        EquipmentController.Instance.onEquipmentLoadComplete += SetNewStats;
    }

    private void SetNewStats()
    {
        hull = equipment.hullSlots[0].item as Hull;
        if (!hull) { noHull = true; } else
        {
            noHull = false;
            GetComponent<Rigidbody2D>().mass = hull.hullWeight;
        }
        UpdateSize();
    }

    void UpdateHealth()
    {
        if (hull.hullCurrentHealth > hull.hullHealth) hull.hullCurrentHealth = hull.hullHealth;
        healthPercent = hull.hullCurrentHealth / hull.hullHealth;
        Debug.Log(healthPercent);
        Debug.Log(healthBarImage.fillAmount);
        healthBarImage.fillAmount = healthPercent;
    }

    void UpdateSize()
    {
        foreach (RectTransform child in healthBar)
        {
            float _backModifier = 0;
            if (child == healthBar.GetChild(0)) _backModifier = 0.05f;
            if (noHull) child.sizeDelta = new Vector2(0 + _backModifier, child.sizeDelta.y);
            else child.sizeDelta = new Vector2((hull.hullHealth / 5) + _backModifier, child.sizeDelta.y);
        }
        if (!noHull) UpdateHealth();
    }

    #region collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            if (shield != null && shield.shieldActiveForColliders)
            {

            } else TakeDamage(collision.collider.GetComponent<Bullet>()._localDamage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bullet"))
        {
            if (shield != null && shield.shieldActiveForColliders)
            {

            }else TakeDamage(collision.GetComponent<Bullet>()._localDamage);
        }
        if (collision.CompareTag("AIBullet"))
        {
            if (shield != null && shield.shieldActiveForColliders)
            {

            }else TakeDamage(collision.GetComponent<AIBullet>().damage);
        }
    }
    #endregion

    void TakeDamage(float damage)
    {
        if (!noHull)
        {
            float incomingDamage = damage - (damage * (hull.hullDamageNegation / 100));
            hull.hullCurrentHealth -= incomingDamage;
        }
        HealthCheck();
    }
    public void HealthCheck()
    {
        shield.rechargeTimer = 0;
        if (!noHull) UpdateHealth();
        if (noHull || hull.hullCurrentHealth <= 0)
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
