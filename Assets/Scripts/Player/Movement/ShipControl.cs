using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControl : MonoBehaviour
{
    public bool ftlDisabled;
    public bool thrusterEquipped;
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

    public Thrusters thruster;
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
        //Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        SetNewStats();
        if (thrusterEquipped)
        {
            thruster.fuelCurrent = thruster.fuelMax;
            fuelPercent = thruster.fuelCurrent / thruster.fuelMax;
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
        thruster = equipment.thrusterSlots[0].item as Thrusters;
        //Debug.Log("ftl is " + ftl);
        thrusterEquipped = thruster != null;
        if (thruster) { UpdateSize(); } //Change some visuals
        else { if (ftlActive) ftlEnding = true; UpdateSize(); }

    }

    private void FixedUpdate()
    {
        if (GlobalRefs.Instance.playerIsDead) { rb.drag = 0; rb.angularDrag = 0; return; }

        if (!ftlDisabled && thrusterEquipped && ftlActive) FTL();
        else if (thrusterEquipped) Movement();

        if (!ftlDisabled && thrusterEquipped && Input.GetKey(KeyCode.Space) && !ftlActive) ChargeFTL();
        else if (ftlCharge > 0) ftlCharge -= 3;
        else if (ftlCharge < 0) ftlCharge = 0;

        if (ftlEnding) StopFTL();
    }

    private void Update()
    {
        if ((ftlDisabled && ftlActive) || (ftlActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.S))))
            ftlEnding = true;
    }

    void UseFuel(float modifier)
    {
        if (ftlActive)
        {
            if (thruster.fuelDrain != -1) thruster.fuelCurrent -= thruster.fuelDrain * modifier;
        }
        else if (thruster.fuelDrain != -1) thruster.fuelCurrent -= thruster.fuelDrain * modifier;
        UpdateFuel();
    }
    void UpdateFuel()
    {
        if (thrusterEquipped)
        {
            if (thruster.fuelCurrent > thruster.fuelMax) thruster.fuelCurrent = thruster.fuelMax;
            fuelPercent = thruster.fuelCurrent / thruster.fuelMax;
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
            if (thrusterEquipped) { currentFuel = thruster.fuelMax; } else { currentFuel = 0; }
            child.sizeDelta = new Vector2((currentFuel / 40000) + _backModifier, child.sizeDelta.y);
        }
        UpdateFuel();
    }
    void ChargeFTL()
    {
        ftlCharge++;
        if (ftlCharge >= thruster.chargeTime * 60) 
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
        rb.AddForce(transform.up * thruster.ftlAcc, ForceMode2D.Force);
        float drag = thruster.ftlAcc / thruster.ftlMaxSpeed;
        rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
        rb.angularDrag = (1 + thruster.ftlRotSpeed) * 2;
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(thruster.ftlRotSpeed, ForceMode2D.Force); //Add that force
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-thruster.ftlRotSpeed, ForceMode2D.Force);
        }
        duration++;
        if (duration > thruster.maxDuration * 60  && thruster.maxDuration != -1) StopFTL();
        UseFuel(thruster.fuelDrain);
    }
    void Movement()
    {
        //Input and movement
        if (Input.GetKey(KeyCode.W)) //Move forward with force
        {
            rb.AddForce(transform.up * thruster.speed, ForceMode2D.Force);
            float drag = thruster.speed / thruster.maxSpeed;
            rb.drag = drag / (drag * Time.fixedDeltaTime + 1);  //Adjust drag while accelerating
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.up * (thruster.speed * 0.8f), ForceMode2D.Force);
            float drag = (thruster.speed * 2) / thruster.maxSpeed;
            rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
        }
        else
        {
            if (rb.drag != thruster.brakingSpeed)    //Reset drag to slider value
                rb.drag = thruster.brakingSpeed;
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
            rb.AddTorque(thruster.turnSpeed, ForceMode2D.Force); //Add that force
            rb.angularDrag = thruster.maxTurnSpeed;
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
            rb.AddTorque(-thruster.turnSpeed, ForceMode2D.Force);
            rb.angularDrag = thruster.maxTurnSpeed;
        }
        //if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))   //Reset turnspeed to slider value
        //    stl.turnSpeed = sliderRef.turnSpeedSlider.value;
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) //Reset angular drag to slider value
            rb.angularDrag = thruster.turnBrakingSpeed;
    }
}