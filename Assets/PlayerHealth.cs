using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : GameTrigger
{
    public float iFrames;


    public float currentHealth;
    public int healthNodes;
    public int healthNodesMax;
    public GameObject healthNode;
    public List<Image> nodes = new();
    float healthPercent;

    public int healthBarDividedByInLength;

    public Transform hud;
    public RectTransform origo;

    public Transform healthBar;
    Image healthBarImage;

    public PassiveShield shield;
    public GameObject deathMenu;

    EquipmentController equipment;
    public Hull hull;
    bool noHull;
    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
    }

    private void CustomStart()
    {
        //Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        healthBarImage = healthBar.GetChild(2).GetComponent<Image>();
        SetNewStats();
        if (!noHull)
        {
            hull.hullCurrentHealth = hull.hullHealth;
        }

        for (int i = 0; i < healthNodesMax; i++) // link to ship stats when starting game
        {
            RectTransform node = Instantiate(healthNode).GetComponent<RectTransform>();
            node.SetParent(hud);
            Image nodeImage = node.GetChild(2).GetComponent<Image>();
            nodes.Insert(i, nodeImage);
            node.localPosition = new Vector3(origo.localPosition.x, origo.localPosition.y, origo.localPosition.z);
            node.position = new Vector3(node.position.x + (i * 0.17f), node.position.y, node.position.z);
            node.localScale = healthBar.localScale;
        }
        healthBar.position = new Vector3(0.04f + healthBar.position.x + ((0.13f * healthNodesMax) + (0.04f * (healthNodesMax - 1))), healthBar.position.y, healthBar.position.z);

        EquipmentController.Instance.onEquipmentLoadComplete -= CustomStart;
        EquipmentController.Instance.onEquipmentLoadComplete += SetNewStats;
    }
    private void Update()
    {
        healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, healthPercent, 0.1f);
        iFrames -= Time.deltaTime;
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

    public void UpdateHealth()
    {
        if (hull.hullCurrentHealth > hull.hullHealth) hull.hullCurrentHealth = hull.hullHealth;
        healthPercent = hull.hullCurrentHealth / hull.hullHealth;
    }

    void UpdateSize()
    {
        foreach (RectTransform child in healthBar)
        {
            float _backModifier = 0;
            if (child == healthBar.GetChild(0)) _backModifier = 0.05f;
            if (noHull) child.sizeDelta = new Vector2(0 + _backModifier, child.sizeDelta.y);
            else child.sizeDelta = new Vector2((hull.hullHealth / healthBarDividedByInLength) + _backModifier, child.sizeDelta.y);
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

            }
            else
            {
                float damage = collision.collider.GetComponent<Bullet>()._localDamage;
                TakeDamage(damage);
                OnHitEvent(damage, collision.collider.transform.position);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bullet"))
        {
            if (shield != null && shield.shieldActiveForColliders)
            {

            }
            else
            {
                float damage = collision.GetComponent<Bullet>()._localDamage;
                TakeDamage(damage);
                OnHitEvent(damage, collision.transform.position);
            }
        }
        if (collision.CompareTag("AIBullet"))
        {
            if (shield != null && shield.shieldActiveForColliders)
            {

            }
            else
            {
                float damage = collision.GetComponent<AIBullet>().damage;
                TakeDamage(damage);
                OnHitEvent(damage, collision.transform.position);
            }
        }
    }
    #endregion

    void TakeDamage(float damage)
    {
        if (!noHull)
        {
            if (iFrames >= 0) { return; }
            
            float incomingDamage = damage - (damage * (hull.hullDamageNegation / 100));
            hull.hullCurrentHealth -= incomingDamage;
        }
        if (iFrames <= 0)
            HealthCheck();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.playerHit, transform.position);
    }
    public void HealthCheck()
    {
        shield.rechargeTimer = 0;
        if (!noHull) UpdateHealth();
        if (noHull || hull.hullCurrentHealth <= 0)
        {
            if (healthNodes == 0) return;
            if (healthNodes == 1)
            {
                healthNodes = 0;
                SetNodes();
                //TMP solution
                Time.timeScale = 1;
                GlobalRefs.Instance.playerIsDead = true;
                deathMenu.SetActive(true);
                //Start taking damage to nodes
                //Check if all nodes are destroyed
                //Then death and end screen
            }
            else
            {
                iFrames = 1; healthNodes--;
                SetNodes();
            }
        }
    }

    public void SetNodes()
    {
        nodes.ForEach(node => { node.fillAmount = 0; });
        for (int i = 0; i < healthNodes; i++)
        {
            nodes[i].fillAmount = 1;
        }
    }

    public override void OnHitEvent(float damage, Vector2 position)
    {
        base.OnHitEvent(damage, position);
    }
}
