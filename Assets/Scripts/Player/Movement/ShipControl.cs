using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class ShipControl : MonoBehaviour
{
    public bool ftlDisabled;
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

    public Thrusters ftl; //Change with ship
    public Thrusters thruster;
    EquipmentController equipment;

    EventInstance thrusterMain;
    EventInstance thrusterSide;
    EventInstance thrusterRotate;
    EventInstance thrusterBack;

    EventInstance ftlAmbient;
    EventInstance ftlStart;
    EventInstance ftlStop;
    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
    }
    private void Start()
    {
        thrusterMain = AudioManager.Instance.CreateInstance(FMODEvents.Instance.mainThruster);
        thrusterSide = AudioManager.Instance.CreateInstance(FMODEvents.Instance.sideThruster);
        thrusterRotate = AudioManager.Instance.CreateInstance(FMODEvents.Instance.rotateThruster);
        thrusterBack = AudioManager.Instance.CreateInstance(FMODEvents.Instance.backThruster);

        ftlAmbient = AudioManager.Instance.CreateInstance(FMODEvents.Instance.ftlAmbient);
        ftlStart = AudioManager.Instance.CreateInstance(FMODEvents.Instance.ftlCharge);
        ftlStop = AudioManager.Instance.CreateInstance(FMODEvents.Instance.ftlExit);
    }
    private void CustomStart()
    {
        //Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        SetNewStats();

        ftl.fuelCurrent = ftl.fuelMax;
        fuelPercent = ftl.fuelCurrent / ftl.fuelMax;
        fuelMeter.fillAmount = fuelPercent;

        UpdateSize();
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
        UpdateSize(); //Change some visuals
    }

    private void FixedUpdate()
    {
        if (GlobalRefs.Instance.playerIsDead) { rb.drag = 0; rb.angularDrag = 0; return; }

        if (!ftlDisabled && ftlActive) FTL();
        else if (thruster != null) Movement();

        if (ftl.fuelCurrent > 0 && !ftlDisabled && Input.GetKey(KeyCode.Space) && !ftlActive) ChargeFTL();
        else { ftlCharge = 0; }

        if (ftlEnding) StopFTL();
    }

    private void Update()
    {
        if ((ftlDisabled && ftlActive) || (ftlActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.S))))
            ftlEnding = true;
        if (!ftlActive && Input.GetKeyUp(KeyCode.Space) && ftlCharge < ftl.chargeTime * 60)
            StopPlayback(ftlStart, STOP_MODE.ALLOWFADEOUT);
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
    public void UpdateFuel()
    {
         if (ftl.fuelCurrent > ftl.fuelMax) ftl.fuelCurrent = ftl.fuelMax;
         fuelPercent = ftl.fuelCurrent / ftl.fuelMax;
         fuelMeter.fillAmount = fuelPercent;
    }
    void UpdateSize()
    {
        //foreach (RectTransform child in fuelBar)
        //{
        //    float _backModifier = 0;
        //    if (child == fuelBar.GetChild(0)) _backModifier = 0.05f;
        //    float currentFuel;
        //    currentFuel = ftl.fuelMax;
        //    child.sizeDelta = new Vector2((currentFuel / 5000) + _backModifier, child.sizeDelta.y);
        //}
        UpdateFuel();
    }
    void ChargeFTL()
    {
        ftlCharge++;
        StartPlayback(ftlStart);
        if (!ftlActive && ftlCharge >= ftl.chargeTime * 60) 
        {
            //GlobalRefs.Instance.playerIsInFtl = true;
            ActivateFTL();
        }
    }

    void ActivateFTL()
    {
        StopPlayback(thrusterMain, STOP_MODE.ALLOWFADEOUT);
        StartPlayback(ftlAmbient);
        col.enabled = false;
        ftlActive = true;
    }

    void StopFTL()
    {
        StopPlayback(ftlAmbient, STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.ftlExit, transform.position);
        col.enabled = true;
        duration = 0;
        ftlActive = false;
        rb.drag = 10000;
        ftlEnding = false;
        //GlobalRefs.Instance.playerIsInFtl = false;
    }

    void FTL()
    {
        rb.AddForce(transform.up * ftl.ftlAcc, ForceMode2D.Force);
        float drag = ftl.ftlAcc / ftl.ftlMaxSpeed;
        rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
        rb.angularDrag = (1 + ftl.ftlRotSpeed) * 2;
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(ftl.ftlRotSpeed, ForceMode2D.Force); //Add that force
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-ftl.ftlRotSpeed, ForceMode2D.Force);
        }
        duration++;
        if (duration > ftl.maxDuration * 60  && ftl.maxDuration != -1) StopFTL();
        UseFuel(ftl.fuelDrain);
        if (ftl.fuelCurrent < 0) { StopFTL(); }
    }
    void Movement()
    {
        //Input and movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) //Move forward with force
        {

            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.up * thruster.speed, ForceMode2D.Force);
                float drag = thruster.speed / thruster.maxSpeed;
                rb.drag = drag / (drag * Time.fixedDeltaTime + 1);  //Adjust drag while accelerating
                StartPlayback(thrusterMain);
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-transform.up * (thruster.speed * 0.8f), ForceMode2D.Force);
                float drag = (thruster.speed * 2) / thruster.maxSpeed;
                rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
                StartPlayback(thrusterBack);
            }
            else 

            if (Input.GetKey(KeyCode.Q))
            {
                rb.AddForce(-transform.right * (thruster.speed * 0.7f), ForceMode2D.Force);
                float drag = (thruster.speed * 2) / thruster.maxSpeed;
                rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
                StartPlayback(thrusterSide);
            }
            if (Input.GetKey(KeyCode.E))
            {
                rb.AddForce(transform.right * (thruster.speed * 0.7f), ForceMode2D.Force);
                float drag = (thruster.speed * 2) / thruster.maxSpeed;
                rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
                StartPlayback(thrusterSide);
            }
            if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E)) StopPlayback(thrusterSide, STOP_MODE.ALLOWFADEOUT);
        }
        if (!Input.GetKey(KeyCode.W)) StopPlayback(thrusterMain, STOP_MODE.ALLOWFADEOUT);
        if (!Input.GetKey(KeyCode.S)) StopPlayback(thrusterBack, STOP_MODE.ALLOWFADEOUT);
        if (!Input.GetKey(KeyCode.A)|| !Input.GetKey(KeyCode.D)) StopPlayback(thrusterSide, STOP_MODE.ALLOWFADEOUT);

        else
        {
            if (rb.drag != thruster.brakingSpeed)    //Reset drag to slider value
                rb.drag = thruster.brakingSpeed * 3;
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
            StartPlayback(thrusterRotate);
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
            StartPlayback(thrusterRotate);
        }
        //if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))   //Reset turnspeed to slider value
        //    stl.turnSpeed = sliderRef.turnSpeedSlider.value;
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) //Reset angular drag to slider value
        {
            StopPlayback(thrusterRotate, STOP_MODE.ALLOWFADEOUT);
            rb.angularDrag = thruster.turnBrakingSpeed * 3;
        }
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