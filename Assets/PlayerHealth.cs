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
    public List<GameObject> nodes = new();
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

    public List<GameObject> healthNodesPrefabs = new();
    public List<GameObject> healthBarsPrefabs = new();

    public List<Sprite> borderActiveHealthBarSprites = new();
    public List<Sprite> filledActiveHealthBarSprites = new();
    public List<Sprite> borderIdleHealthBarSprites = new();
    public List<Sprite> filledIdleHealthBarSprites = new();

    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
    }

    private void CustomStart()
    {
        //Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        SetNewStats();
        if (!noHull)
        {
            hull.hullCurrentHealth = hull.hullHealth;
            hull.hullNodesCurrent = hull.hullNodesMax;
        }
        SetNodes();
        //healthBar.position = new Vector3(0.04f + healthBar.position.x + ((0.13f * healthNodesMax) + (0.04f * (healthNodesMax - 1))), healthBar.position.y, healthBar.position.z);

        EquipmentController.Instance.onEquipmentLoadComplete -= CustomStart;
        EquipmentController.Instance.onEquipmentLoadComplete += SetNewStats;
    }

    void SetNodes()
    {
        if (hull.hullNodesCurrent <= 0) { return; }
        foreach(Transform child in healthBar)
        {
            Destroy(child.gameObject);
        }
        foreach(GameObject node in nodes)
        {
            Destroy(node);
        }
        float nodeOffset = 0;
        float offset = 0;
        for (int i = 0; i < hull.hullNodesMax; i++)
        {
            GameObject node;
            if (i == hull.hullNodesCurrent - 1)
            {
                if (i == 0) { 
                    node = Instantiate(healthBarsPrefabs[0], healthBar);
                    nodeOffset += 34.39f;
                    offset = 221.9f - 34.39f;
                    healthBarImage = node.transform.GetChild(1).GetComponent<Image>();
                }
                else if (i == hull.hullNodesMax - 1) {
                    node = Instantiate(healthBarsPrefabs[2], healthBar);
                    node.transform.localPosition = new Vector3(node.transform.localPosition.x + nodeOffset, node.transform.localPosition.y, node.transform.localPosition.z);
                    healthBarImage = node.transform.GetChild(1).GetComponent<Image>();
                }
                else {
                    node = Instantiate(healthBarsPrefabs[1], healthBar);
                    node.transform.localPosition = new Vector3(node.transform.localPosition.x + nodeOffset, node.transform.localPosition.y, node.transform.localPosition.z);
                    nodeOffset += 46.89f;
                    offset = 234.2f - 46.89f;
                    healthBarImage = node.transform.GetChild(1).GetComponent<Image>();
                }
            }
            else
            {
                if (i == 0) { 
                    node = Instantiate(healthNodesPrefabs[0], healthBar);
                    nodeOffset += 34.39f;
                }
                else if (i == hull.hullNodesMax - 1)
                {
                    node = Instantiate(healthNodesPrefabs[2], healthBar);
                    node.transform.localPosition = new Vector3(node.transform.localPosition.x + offset + nodeOffset, node.transform.localPosition.y, node.transform.localPosition.z);
                    if (i > hull.hullNodesCurrent - 1)
                    {
                        node.transform.GetChild(1).GetComponent<Image>().fillAmount = 0;
                    }
                }
                else
                {
                    node = Instantiate(healthNodesPrefabs[1], healthBar);
                    node.transform.localPosition = new Vector3(node.transform.localPosition.x + offset + nodeOffset, node.transform.localPosition.y, node.transform.localPosition.z);
                    nodeOffset += 46.89f;
                    if (i > hull.hullNodesCurrent - 1)
                    {
                        node.transform.GetChild(1).GetComponent<Image>().fillAmount = 0;
                    }
                }
            }
            hull.hullCurrentHealth = hull.hullHealth;
            healthPercent = hull.hullCurrentHealth / hull.hullHealth;
        }
    }
    private void Update()
    {
        if (healthBarImage) healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, healthPercent, 0.1f);
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
        if(noHull) { return; }
        SetNodes();
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        if (hull.hullCurrentHealth > hull.hullHealth) hull.hullCurrentHealth = hull.hullHealth;
        healthPercent = hull.hullCurrentHealth / hull.hullHealth;
    }

    void UpdateSize()
    {
        //foreach (RectTransform child in healthBar)
        //{
        //    float _backModifier = 0;
        //    if (child == healthBar.GetChild(0)) _backModifier = 0.05f;
        //    if (noHull) child.sizeDelta = new Vector2(0 + _backModifier, child.sizeDelta.y);
        //    else child.sizeDelta = new Vector2((hull.hullHealth / healthBarDividedByInLength) + _backModifier, child.sizeDelta.y);
        //}
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

    public void TakeDamage(float damage)
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
            if (hull.hullNodesCurrent == 0) return;
            if (hull.hullNodesCurrent == 1)
            {
                hull.hullNodesCurrent = 0;
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
                hull.hullNodesCurrent--;
                SetNodes();
                iFrames = 1;
                
            }
        }
    }

    public override void OnHitEvent(float damage, Vector2 position)
    {
        base.OnHitEvent(damage, position);
    }
}
