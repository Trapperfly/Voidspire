using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Damagable : GameTrigger
{
    public float startHealth = 0;
    public float currentHealth;
    public bool damageTaken;
    public bool shipOverride;
    public GameObject damageTakenFromWhat;
    public float healthPercent;

    public bool isBoss;
    public Transform hud;
    public GameObject healthBarAndLevel;
    public GameObject bossHealthBarAndLevel;
    Image healthBar;
    TMP_Text levelText;

    public EnemyHealthBar hb;
    private void Start()
    {
        if (isBoss) { Init(); }
        if (shipOverride) { return; }
        if (startHealth == 0)
        {
            float size = (transform.localScale.x + transform.localScale.y);
            startHealth = Mathf.Pow(size + 1f, 3);
        }
        currentHealth = startHealth;
        healthPercent = currentHealth / startHealth;
    }
    private void Update()
    {
        if (hb != null)
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, healthPercent, 0.1f);
    }

    public void Init()
    {
        TryGetComponent(out ShipAI s);
        if (s && isBoss)
        {
            gameObject.name = s.ship.aiName;
            hud = GameObject.FindGameObjectWithTag("Hud").transform;
            GameObject b = Instantiate(bossHealthBarAndLevel, hud);
            healthBar = b.transform.GetChild(2).GetChild(2).GetComponent<Image>();
            b.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = gameObject.name;
            levelText = b.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
            levelText.text = GetComponent<DropsLoot>().level.ToString();
            b.GetComponent<EnemyHealthBar>().target = transform;
            hb = b.GetComponent<EnemyHealthBar>();
            hb.isBoss = true;
            return;
        }
        else if (s)
        {
            gameObject.name = s.ship.aiName;
            hud = GameObject.FindGameObjectWithTag("Hud").transform;
            GameObject h = Instantiate(healthBarAndLevel, transform.position, new Quaternion(), hud);
            healthBar = h.transform.GetChild(1).GetChild(2).GetComponent<Image>();
            levelText = h.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
            levelText.text = GetComponent<DropsLoot>().level.ToString();
            h.GetComponent<EnemyHealthBar>().target = transform;
            hb = h.GetComponent<EnemyHealthBar>();
        }
        else
        {
            hud = GameObject.FindGameObjectWithTag("Hud").transform;
            GameObject h = Instantiate(healthBarAndLevel, transform.position, new Quaternion(), hud);
            healthBar = h.transform.GetChild(1).GetChild(2).GetComponent<Image>();
            levelText = h.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
            levelText.text = GetComponent<DropsLoot>().level.ToString();
            h.GetComponent<EnemyHealthBar>().target = transform;
            hb = h.GetComponent<EnemyHealthBar>();
        }
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
        if (hb != null) { hb.timer = 0; }
        else
        {
            if (!isBoss) { Init(); }
        }
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
