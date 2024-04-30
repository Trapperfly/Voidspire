using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class PassiveShield : GameTrigger
{
    public bool shieldActive = false;
    [HideInInspector] public bool shieldActiveForColliders = false;
    public float shieldCurrent;
    float shieldPercent;
    public int rechargeTimer;

    public int shieldBarDividedByInLength;

    int storedID;
    float animTime;

    public Collider2D col;
    Material mat;
    [ColorUsage(true, true)]

    public float offset;

    public GameObject spritesToBeHidden;
    public Transform shieldBar;
    Image shieldBarImage;
    public Transform shieldRechargeBar;
    Image shieldRechargeImage;

    float lastFrameCurrentShield;

    EquipmentController equipment;
    Shield shield;
    public bool noShield;
    bool newShieldCheck;
    bool start = false;

    EventInstance shieldUp;
    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
        mat = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().material;
    }

    private void CustomStart()
    {
        shieldUp = AudioManager.Instance.CreateInstance(FMODEvents.Instance.shieldUp);
        //Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        shieldBarImage = shieldBar.GetChild(1).GetComponent<Image>();
        shieldRechargeImage = shieldRechargeBar.GetChild(1).GetComponent<Image>();
        shieldActive = true;
        shieldActiveForColliders = true;
        start = true;
        SetNewStats();
        UpdateSize();
        start = false;

        EquipmentController.Instance.onEquipmentLoadComplete -= CustomStart;
        EquipmentController.Instance.onEquipmentLoadComplete += SetNewStats;
    }
    private void SetNewStats()
    {
        if (shield) { animTime = shield.shieldBreakAnimTime; storedID = shield.id; }
        shield = equipment.shieldSlots[0].item as Shield;
        if (shield) animTime = shield.shieldBreakAnimTime;
        if (!shield) { noShield = true; }
        else
        {
            if (start) shieldCurrent = shield.shieldHealth;
            else if (shield.id != storedID) shieldCurrent = 0;
            noShield = false;
        }
        UpdateSize();
        if (shield && shield.id == storedID) newShieldCheck = true;
        ShieldCheck();
    }

    private void Update()
    {
        shieldBarImage.fillAmount = Mathf.Lerp(shieldBarImage.fillAmount, shieldPercent, 0.1f);
        shieldRechargeImage.fillAmount = Mathf.Lerp(shieldRechargeImage.fillAmount, rechargeTimer / (shield.shieldRechargeDelay * 60), 0.5f);
    }
    private void FixedUpdate()
    {
        if (GlobalRefs.Instance.playerIsDead) return;
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
        mat.SetFloat("_ShieldHealth", shieldBarImage.fillAmount);
    }

    void UpdateSize()
    {
        //foreach (RectTransform child in shieldBar)
        //{
        //    float _backModifier = 0;
        //    if (child == shieldBar.GetChild(0)) _backModifier = 0.05f;
        //    if (!noShield) child.sizeDelta = new Vector2((shield.shieldHealth / shieldBarDividedByInLength) + _backModifier, child.sizeDelta.y);
        //    else child.sizeDelta = new Vector2(0 + _backModifier, child.sizeDelta.y);
        //}
        if (!noShield)
        {
            shieldBarImage.color = shield.shieldColor;
            shieldRechargeImage.color = shield.shieldColor * new Color(1,1,1,0.5f);
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
        //Debug.Log("Checking if broken");
        if (newShieldCheck) { }
        else { rechargeTimer = 0; }
        newShieldCheck = false;
        //check if shield is broken
        UpdateSize();
        if (shieldActive && shieldCurrent <= 0)
        {
            //Debug.Log("Breaking shield because of damage taken");
            StartCoroutine(BreakShield());
        }
    }

    IEnumerator BreakShield()
    {
        StopPlayback(shieldUp, STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.shieldDown, transform.position);
        shieldCurrent = 0;
        shieldActive = false;
        rechargeTimer = 0;
        col.enabled = false;
        int timer = 0;
        
        while (timer < 60f * animTime)
        {
            rechargeTimer = 0;
            spritesToBeHidden.transform.localScale = Vector3.Lerp(spritesToBeHidden.transform.localScale, new Vector3(0, 0, 0), timer / (60f * animTime));
            timer++;
            yield return new WaitForFixedUpdate();
        }
        shieldActiveForColliders = false;
        spritesToBeHidden.transform.localScale = new Vector3(0, 0, 0);
        spritesToBeHidden.SetActive(false);
        yield return null;
    }

    IEnumerator RestoreShield()
    {
        StartPlayback(shieldUp);
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
    void StartPlayback(EventInstance audio)
    {
        PLAYBACK_STATE state;
        audio.getPlaybackState(out state);
        if (state.Equals(PLAYBACK_STATE.STOPPED))
        {
            audio.start();
        }
    }
    void StopPlayback(EventInstance audio, STOP_MODE stopMode)
    {
        audio.stop(stopMode);
    }
}
