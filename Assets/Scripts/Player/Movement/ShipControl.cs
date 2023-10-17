using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControl : MonoBehaviour
{
    [Header("Ship control")]
    public float speed;
    public float maxSpeed;
    public float turnSpeed;
    public float turnSpeedStored;
    public float maxTurnSpeed;
    public float turnSpeedBoostUpTo = 40f;
    public float turnBrakingSpeed;
    public bool braking;
    public float brakingSpeed;
    public float fuel;
    public float fuelMax;
    public float fuelDrain;
    public Image fuelMeter;

    [SerializeField] GUISlidersSet sliderRef;
    ShipRbController sRb;
    Rigidbody2D rb;

    int ftlCharge;
    bool ftlActive;
    FTLDrive ftl;
    int duration;

    private void Awake()
    {
        fuel = fuelMax;
        fuelMeter.fillAmount = fuel / fuelMax;
        ftl = GetComponent<FTLDrive>();
        sRb = GetComponent<ShipRbController>();
        rb = sRb.rb;
    }

    private void FixedUpdate()
    {
        if (ftlActive) FTL();
        else Movement();

        if (Input.GetKey(KeyCode.Space) && !ftlActive) ChargeFTL();
        else if (ftlCharge > 0) ftlCharge -= 3;
        else if (ftlCharge < 0) ftlCharge = 0;

        if (ftlActive && Input.GetKeyDown(KeyCode.Space) || ftlActive && Input.GetKey(KeyCode.S)) StopFTL();


    }

    void UseFuel(float modifier)
    {
        if (ftlActive)
        {
            if (ftl.fuelDrain != -1) fuel -= ftl.fuelDrain * modifier;
        }
        else if (fuelDrain != -1) fuel -= fuelDrain * modifier;
        fuelMeter.fillAmount = fuel / fuelMax;
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
        ftlActive = true;
    }

    void StopFTL()
    {
        duration = 0;
        rb.drag = 1000;
        ftlActive = false;
    }

    void FTL()
    {
        rb.AddForce(transform.up * ftl.acceleration, ForceMode2D.Force);
        float drag = ftl.acceleration / ftl.maxSpeed;
        rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
        rb.angularDrag = sliderRef.maxTurnSpeedSlider.value * (1 + ftl.rotSpeed) * 2;
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(turnSpeed * ftl.rotSpeed, ForceMode2D.Force); //Add that force
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-turnSpeed * ftl.rotSpeed, ForceMode2D.Force);
        }
        duration++;
        if (duration > ftl.maxDuration * 60  && ftl.maxDuration != -1) StopFTL();
        UseFuel(ftl.maxSpeed);
    }
    void Movement()
    {
        //Input and movement
        if (Input.GetKey(KeyCode.W)) //Move forward with force
        {
            rb.AddForce(transform.up * speed, ForceMode2D.Force);
            float drag = speed / maxSpeed;
            rb.drag = drag / (drag * Time.fixedDeltaTime + 1);  //Adjust drag while accelerating
            UseFuel(speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.up * (speed / 2), ForceMode2D.Force);
            float drag = (speed * 2) / maxSpeed;
            rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
            UseFuel(speed / 2);
        }
        else
        {
            if (rb.drag != sliderRef.brakingSpeedSlider.value)    //Reset drag to slider value
                rb.drag = sliderRef.brakingSpeedSlider.value;
        }

        if (Input.GetKey(KeyCode.A))
        {
            float angularVelocityNormalized = Mathf.Sqrt(Mathf.Pow(rb.angularVelocity, 2)); //Make sure the angular velocity is positive and assign that to a float
            if (angularVelocityNormalized <= turnSpeedBoostUpTo)    //Add more force at the start of rotation
            {
                float multiplier = 1 + (turnSpeedBoostUpTo - angularVelocityNormalized) / turnSpeedBoostUpTo;
                if (turnSpeed <= turnSpeedStored * 2)
                    turnSpeed = turnSpeedStored * multiplier;
            }
            rb.AddTorque(turnSpeed, ForceMode2D.Force); //Add that force
            rb.angularDrag = sliderRef.maxTurnSpeedSlider.value;
            UseFuel(turnSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            float angularVelocityNormalized = Mathf.Sqrt(Mathf.Pow(rb.angularVelocity, 2)); //Ditto
            if (angularVelocityNormalized <= turnSpeedBoostUpTo)
            {
                float multiplier = 1 + (turnSpeedBoostUpTo - angularVelocityNormalized) / turnSpeedBoostUpTo;
                if (turnSpeed <= turnSpeedStored * 2)
                    turnSpeed = turnSpeedStored * multiplier;
            }
            rb.AddTorque(-turnSpeed, ForceMode2D.Force);
            rb.angularDrag = sliderRef.maxTurnSpeedSlider.value;
            UseFuel(turnSpeed);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))   //Reset turnspeed to slider value
            turnSpeed = sliderRef.turnSpeedSlider.value;
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && sliderRef.turnBrakeSlider.value != 0) //Reset angular drag to slider value
            rb.angularDrag = sliderRef.turnBrakeSlider.value;
    }
}