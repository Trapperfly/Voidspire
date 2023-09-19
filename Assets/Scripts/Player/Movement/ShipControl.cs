using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] GUISlidersSet sliderRef;
    ShipRbController sRb;
    Rigidbody2D rb;

    private void Awake()
    {
        sRb = GetComponent<ShipRbController>();
        rb = sRb.rb;
    }

    private void FixedUpdate()
    {
        //Input and movement
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) //Move forward with force
        {
            rb.AddForce(transform.up * speed, ForceMode2D.Force);
            float drag = speed / maxSpeed;
            rb.drag = drag / (drag * Time.fixedDeltaTime + 1);  //Adjust drag while accelerating
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.up * (speed / 2), ForceMode2D.Force);
            float drag = (speed * 2) / maxSpeed;
            rb.drag = drag / (drag * Time.fixedDeltaTime + 1);
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
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))   //Reset turnspeed to slider value
            turnSpeed = sliderRef.turnSpeedSlider.value;
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && sliderRef.turnBrakeSlider.value != 0) //Reset angular drag to slider value
            rb.angularDrag = sliderRef.turnBrakeSlider.value;
    }
}