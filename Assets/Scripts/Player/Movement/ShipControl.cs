using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControl : MonoBehaviour
{
    public bool stlEquipped;
    public bool ftlEquipped;
    [Header("Ship control")]

    float fuelPercent;
    public Image fuelMeter;
    public Transform fuelBar;

    ShipRbController sRb;
    Rigidbody2D rb;
    Collider2D col;

    int ftlCharge;
    bool ftlActive;
    bool ftlEnding;
    int duration;

    public STLEngine stl;
    public FTLEngine ftl;
    EquipmentController equipment;
    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
    }
    //private void Start()
    //{
    //    stat = GetComponent<EngineStats>();
    //    stat.fuel = stat.fuelMax;
    //    fuelPercent = stat.fuel / stat.fuelMax;
    //    fuelMeter.fillAmount = fuelPercent;
    //    sRb = GetComponent<ShipRbController>();
    //    col = GetComponent<Collider2D>();
    //    rb = sRb.rb;
    //}
    private void CustomStart()
    {
        Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        SetNewStats();
        if (ftlEquipped)
        {
            ftl.fuelCurrent = ftl.fuelMax;
            fuelPercent = ftl.fuelCurrent / ftl.fuelMax;
            fuelMeter.fillAmount = fuelPercent;

            UpdateSize();
        }
        else { fuelPercent = 0f; fuelMeter.fillAmount = fuelPercent; }
        sRb = GetComponent<ShipRbController>();
        col = GetComponent<Collider2D>();
        rb = sRb.rb;
        EquipmentController.Instance.onEquipmentLoadComplete -= CustomStart;
        EquipmentController.Instance.onEquipmentLoadComplete += SetNewStats;
    }
    private void SetNewStats()
    {
        stl = equipment.stlSlots[0].item as STLEngine;
        Debug.Log(stl);
        stlEquipped = stl != null;
        if (stl) {  } //Change some visuals
        else {  }

        ftl = equipment.ftlSlots[0].item as FTLEngine;
        Debug.Log("ftl is " + ftl);
        ftlEquipped = ftl != null;
        if (ftl) { UpdateSize(); } //Change some visuals
        else { if (ftlActive) ftlEnding = true; UpdateSize(); }

    }

    private void FixedUpdate()
    {
        if (ftlEquipped && ftlActive) FTL();
        else if (stlEquipped) Movement();

        if ( ftlEquipped && Input.GetKey(KeyCode.Space) && !ftlActive) ChargeFTL();
        else if (ftlCharge > 0) ftlCharge -= 3;
        else if (ftlCharge < 0) ftlCharge = 0;

        if (ftlEnding) StopFTL();
    }

    private void Update()
    {
        if (ftlActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.S)))
            ftlEnding = true;
    }

    void UseFuel(float modifier)
    {
        if (ftlActive)
        {
            if (ftl.fuelDrain != -1) ftl.fuelCurrent -= ftl.fuelDrain * modifier;
        }
        else if (ftl.fuelDrain != -1) ftl.fuelCurrent -= ftl.fuelDrain * modifier;
        UpdateFuel();
    }
    void UpdateFuel()
    {
        if (ftlEquipped)
        {
            if (ftl.fuelCurrent > ftl.fuelMax) ftl.fuelCurrent = ftl.fuelMax;
            fuelPercent = ftl.fuelCurrent / ftl.fuelMax;
            fuelMeter.fillAmount = fuelPercent;
        }
        else { fuelPercent = 0f; fuelMeter.fillAmount = fuelPercent; }
    }
    void UpdateSize()
    {
        foreach (RectTransform child in fuelBar)
        {
            float _backModifier = 0;
            if (child == fuelBar.GetChild(0)) _backModifier = 0.05f;
            float currentFuel;
            if (ftlEquipped) { currentFuel = ftl.fuelCurrent; } else { currentFuel = 0; }
            child.sizeDelta = new Vector2((currentFuel / 10000) + _backModifier, child.sizeDelta.y);
        }
        UpdateFuel();
    }
    void ChargeFTL()
    {
        ftlCharge++;
        if (ftlCharge >= ftl.chargeTime * 60) 
        {
            ftlCharge = 0;
            ActivateFTL();
        }
    }

    void ActivateFTL()
    {
        col.enabled = false;
        ftlActive = true;
    }

    void StopFTL()
    {
        col.enabled = true;
        duration = 0;
        ftlActive = false;
        rb.drag = 10000;
        ftlEnding = false;
    }

    void FTL()
    {
        rb.AddForce(transform.up * ftl.acceleration, ForceMode2D.Force);
        float drag = ftl.acceleration / ftl.maxSpeed;
        rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
        rb.angularDrag = (1 + ftl.rotSpeed) * 2;
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(ftl.rotSpeed, ForceMode2D.Force); //Add that force
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-ftl.rotSpeed, ForceMode2D.Force);
        }
        duration++;
        if (duration > ftl.maxDuration * 60  && ftl.maxDuration != -1) StopFTL();
        UseFuel(ftl.fuelDrain);
    }
    void Movement()
    {
        //Input and movement
        if (Input.GetKey(KeyCode.W)) //Move forward with force
        {
            rb.AddForce(transform.up * stl.speed, ForceMode2D.Force);
            float drag = stl.speed / stl.maxSpeed;
            rb.drag = drag / (drag * Time.fixedDeltaTime + 1);  //Adjust drag while accelerating
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.up * (stl.speed / 2), ForceMode2D.Force);
            float drag = (stl.speed * 2) / stl.maxSpeed;
            rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
        }
        else
        {
            if (rb.drag != stl.brakingSpeed)    //Reset drag to slider value
                rb.drag = stl.brakingSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //float angularVelocityNormalized = Mathf.Sqrt(Mathf.Pow(rb.angularVelocity, 2)); //Make sure the angular velocity is positive and assign that to a float
            //if (angularVelocityNormalized <= stl.turnSpeedBoostUpTo)    //Add more force at the start of rotation
            //{
            //    float multiplier = 1 + (stl.turnSpeedBoostUpTo - angularVelocityNormalized) / stl.turnSpeedBoostUpTo;
            //    if (stl.turnSpeed <= stl.turnSpeedStored * 2)
            //        stl.turnSpeed = stl.turnSpeedStored * multiplier;
            //}
            rb.AddTorque(stl.turnSpeed, ForceMode2D.Force); //Add that force
            rb.angularDrag = stl.maxTurnSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //float angularVelocityNormalized = Mathf.Sqrt(Mathf.Pow(rb.angularVelocity, 2)); //Ditto
            //if (angularVelocityNormalized <= stl.turnSpeedBoostUpTo)
            //{
            //    float multiplier = 1 + (stl.turnSpeedBoostUpTo - angularVelocityNormalized) / stl.turnSpeedBoostUpTo;
            //    if (stl.turnSpeed <= stl.turnSpeedStored * 2)
            //        stl.turnSpeed = stl.turnSpeedStored * multiplier;
            //}
            rb.AddTorque(-stl.turnSpeed, ForceMode2D.Force);
            rb.angularDrag = stl.maxTurnSpeed;
        }
        //if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))   //Reset turnspeed to slider value
        //    stl.turnSpeed = sliderRef.turnSpeedSlider.value;
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) //Reset angular drag to slider value
            rb.angularDrag = stl.turnBrakingSpeed;
    }
}