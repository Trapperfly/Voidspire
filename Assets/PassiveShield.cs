using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveShield : MonoBehaviour
{
    public bool shieldActive = true;
    [HideInInspector] public bool shieldActiveForColliders = true;
    public float shieldCapacity;
    public float shieldCurrent;
    float shieldPercent;
    public float shieldRechargeDelay;
    public int rechargeTimer;
    public float shieldRechargeSpeed;
    public float shieldBreakTime;
    public float shieldRestoreTime;

    public Collider2D col;
    Material mat;
    public Color32 shieldColor;
    [ColorUsage(true, true)]
    public Color32 breakageColor;

    public float offset;

    public GameObject spritesToBeHidden;
    public Transform shieldBar;
    Image shieldBarImage;

    float lastFrameCurrentShield;
    float lastFrameMaxShield;
    private void Awake()
    {
        mat = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().material;
        mat.SetColor("_ShieldColor", shieldColor);
        mat.SetColor("_BreakageColor", breakageColor);
        mat.SetFloat("_ShieldHealth", shieldPercent);
        mat.SetFloat("_BreathingOffset", offset);
        shieldBarImage = shieldBar.GetChild(2).GetComponent<Image>();
        shieldCurrent = shieldCapacity;
        UpdateSize();
        UpdateShield();
    }

    private void FixedUpdate()
    {
        //timer for refilling shield
        if (shieldCurrent < shieldCapacity)
        {
            rechargeTimer++;
            if (rechargeTimer >= shieldRechargeDelay * 60)
            {
                if (!shieldActive) StartCoroutine(RestoreShield());
                if (shieldActive) shieldCurrent += shieldRechargeSpeed / 60;
            }
        }
        else if (shieldCurrent > shieldCapacity) shieldCurrent = shieldCapacity;
    }

    private void Update()
    {
        if (lastFrameMaxShield != shieldCapacity) UpdateSize();
        if (lastFrameCurrentShield != shieldCurrent) UpdateShield();
        lastFrameMaxShield = shieldCapacity;
        lastFrameCurrentShield = shieldCurrent;
    }

    void UpdateShield()
    {
        if (shieldCurrent > shieldCapacity) shieldCurrent = shieldCapacity;
        shieldPercent = shieldCurrent / shieldCapacity;
        shieldBarImage.fillAmount = shieldPercent;
        mat.SetFloat("_ShieldHealth", shieldPercent);
    }

    void UpdateSize()
    {
        foreach (RectTransform child in shieldBar)
        {
            float _backModifier = 0;
            if (child == shieldBar.GetChild(0)) _backModifier = 0.05f;
            child.sizeDelta = new Vector2((shieldCapacity / 5) + _backModifier, child.sizeDelta.y);
        }
        UpdateShield();
    }

    public void ShieldCheck()
    {
        Debug.Log("Checking if broken");
        //check if shield is broken
        rechargeTimer = 0;
        if (shieldActive && shieldCurrent <= 0)
        {
            shieldCurrent = 0;
            StartCoroutine(BreakShield());
        }
    }

    IEnumerator BreakShield()
    {
        shieldActive = false;
        rechargeTimer = 0;
        col.enabled = false;
        int timer = 0;
        while (timer < 60f * shieldBreakTime)
        {
            rechargeTimer = 0;
            spritesToBeHidden.transform.localScale = Vector3.Lerp(spritesToBeHidden.transform.localScale, new Vector3(0, 0, 0), timer / (60f * shieldBreakTime));
            timer++;
            yield return new WaitForFixedUpdate();
            shieldActiveForColliders = false;
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
        while (timer < 60f * shieldRestoreTime)
        {
            spritesToBeHidden.transform.localScale = Vector3.Lerp(spritesToBeHidden.transform.localScale, new Vector3(1,1,1), timer / (60f * shieldRestoreTime));
            timer++;
            yield return new WaitForFixedUpdate();
            shieldActiveForColliders = true;
        }
        spritesToBeHidden.transform.localScale = new Vector3(1, 1, 1);
        col.enabled = true;
        yield return null;
    }
}