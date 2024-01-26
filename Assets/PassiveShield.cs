using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveShield : MonoBehaviour
{
    public bool shieldActive = false;
    [HideInInspector] public bool shieldActiveForColliders = false;
    public float shieldCurrent;
    float shieldPercent;
    public int rechargeTimer;

    int storedID;
    float animTime;

    public Collider2D col;
    Material mat;
    [ColorUsage(true, true)]

    public float offset;

    public GameObject spritesToBeHidden;
    public Transform shieldBar;
    Image shieldBarImage;

    float lastFrameCurrentShield;

    EquipmentController equipment;
    Shield shield;
    bool noShield;
    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
        mat = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().material;
    }

    private void CustomStart()
    {
        Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        shieldBarImage = shieldBar.GetChild(2).GetComponent<Image>();
        shieldActive = true;
        shieldActiveForColliders = true;
        SetNewStats();
        UpdateSize();

        EquipmentController.Instance.onEquipmentLoadComplete -= CustomStart;
        EquipmentController.Instance.onEquipmentLoadComplete += SetNewStats;
    }
    private void SetNewStats()
    {
        if (shield) { animTime = shield.shieldBreakAnimTime; storedID = shield.id; }
        shield = equipment.shieldSlots[0].item as Shield;
        if (shield) animTime = shield.shieldBreakAnimTime;
        if (!shield) { noShield = true; shieldCurrent = 0; }
        else
        {
            if (shield.id != storedID) { shieldCurrent = 0; }
            noShield = false;
        }
        UpdateSize();
        ShieldCheck();
    }

    private void FixedUpdate()
    {
        if (noShield) { }
        else
        {
            if (shieldCurrent < shield.shieldHealth)
            {
                rechargeTimer++;
                if (rechargeTimer >= shield.shieldRechargeDelay * 60)
                {
                    if (!shieldActive) StartCoroutine(RestoreShield());
                    if (shieldActive) { shieldCurrent += shield.shieldRechargeSpeed / 60; UpdateShield(); }
                }
            }
            else if (shieldCurrent > shield.shieldHealth) { shieldCurrent = shield.shieldHealth; }
        }
        //timer for refilling shield
    }

    void UpdateShield()
    {
        if (shieldCurrent > shield.shieldHealth) shieldCurrent = shield.shieldHealth;
        shieldPercent = shieldCurrent / shield.shieldHealth;
        shieldBarImage.fillAmount = shieldPercent;
        mat.SetFloat("_ShieldHealth", shieldPercent);
    }

    void UpdateSize()
    {
        foreach (RectTransform child in shieldBar)
        {
            float _backModifier = 0;
            if (child == shieldBar.GetChild(0)) _backModifier = 0.05f;
            if (!noShield) child.sizeDelta = new Vector2((shield.shieldHealth / 5) + _backModifier, child.sizeDelta.y);
            else child.sizeDelta = new Vector2(0 + _backModifier, child.sizeDelta.y);
        }
        if (!noShield)
        {
            mat.SetColor("_ShieldColor", shield.shieldColor);
            mat.SetColor("_BreakageColor", shield.breakColor);
            mat.SetFloat("_ShieldHealth", shieldPercent);
            mat.SetFloat("_BreathingOffset", offset);
            UpdateShield();
        }
        else if (shieldActive) { }
    }

    public void ShieldCheck()
    {
        Debug.Log("Checking if broken");
        //check if shield is broken
        UpdateShield();
        if (shieldActive && shieldCurrent <= 0)
        {
            Debug.Log("Breaking shield because of damage taken");
            StartCoroutine(BreakShield());
        }
    }

    IEnumerator BreakShield()
    {
        shieldCurrent = 0;
        shieldActive = false;
        rechargeTimer = 0;
        col.enabled = false;
        int timer = 0;
        shieldActiveForColliders = false;
        while (timer < 60f * animTime)
        {
            rechargeTimer = 0;
            spritesToBeHidden.transform.localScale = Vector3.Lerp(spritesToBeHidden.transform.localScale, new Vector3(0, 0, 0), timer / (60f * animTime));
            timer++;
            yield return new WaitForFixedUpdate();
        }
        spritesToBeHidden.transform.localScale = new Vector3(0, 0, 0);
        spritesToBeHidden.SetActive(false);
        yield return null;
    }

    IEnumerator RestoreShield()
    {
        spritesToBeHidden.SetActive(true);
        shieldActive = true;
        int timer = 0;
        shieldActiveForColliders = true;
        while (timer < 60f * shield.shieldRestoreAnimTime)
        {
            spritesToBeHidden.transform.localScale = Vector3.Lerp(spritesToBeHidden.transform.localScale, new Vector3(1,1,1), timer / (60f * shield.shieldRestoreAnimTime));
            timer++;
            yield return new WaitForFixedUpdate();
        }
        spritesToBeHidden.transform.localScale = new Vector3(1, 1, 1);
        col.enabled = true;
        yield return null;
    }
}
