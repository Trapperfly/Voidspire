using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Ship control")]
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float turnSpeed;
    float turnSpeedStored;
    [SerializeField] float maxTurnSpeed;
    [SerializeField] float turnSpeedBoostUpTo = 40f;
    [SerializeField] float turnBrakingSpeed;
    [SerializeField] bool braking;
    [SerializeField] float brakingSpeed;

    [SerializeField] GunController[] gunArray;

    [Header("Physics")]
    [SerializeField] bool resetOnAwake = true;
    Rigidbody2D rb;

    [Header("Targeting")]
    [SerializeField] GameObject targetTransformPrefab;
    AdjustToTarget target;
    GameObject targetInstance;

    [Header("Testing Sliders")]
    [SerializeField] Slider maxSpeedSlider;
    [SerializeField] Slider speedSlider;
    [SerializeField] Slider brakingSpeedSlider;
    [SerializeField] Slider turnSpeedSlider;
    [SerializeField] Slider maxTurnSpeedSlider;
    [SerializeField] Slider turnBrakeSlider;

    private void Awake()
    {
        StartCoroutine(nameof(SetSliders)); //Set the values to the slider values
        rb = GetComponent<Rigidbody2D>();
        if (resetOnAwake)   //Resets center if askew
        {
            Vector2 newCenter = new(0, 0);
            rb.centerOfMass = newCenter;
        }
        gunArray = GetComponentsInChildren<GunController>();
        target = GetComponentInChildren<AdjustToTarget>();
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
            if (rb.drag != brakingSpeedSlider.value)    //Reset drag to slider value
            rb.drag = brakingSpeedSlider.value;
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
            rb.angularDrag = maxTurnSpeedSlider.value;
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
            rb.angularDrag = maxTurnSpeedSlider.value;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))   //Reset turnspeed to slider value
            turnSpeed = turnSpeedSlider.value;
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && turnBrakeSlider.value != 0) //Reset angular drag to slider value
            rb.angularDrag = turnBrakeSlider.value;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))   //Targeting system, to be fixed to be separate method
        {
            SetTarget();
        }
    }

    void SetTarget()
    {
        Debug.Log("Mouse 1 clicked");
        Destroy(GameObject.FindGameObjectWithTag("Target"));    //Remove previous target
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit != false && hit.collider.CompareTag("Targetable"))
        {
            Debug.Log("Hit targetable");
            target.target = hit.transform.gameObject;   //If hit something hittable, make that the target
        }
        else
        {
            Debug.Log("Spawning target transform");
            targetInstance = Instantiate    //If hit nothing, make a vector 2 and use that as target
            (
                targetTransformPrefab,
                new Vector3
                (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                0), new Quaternion()
            );
            target.target = targetInstance;
        }
    }
    float CalculateBulletVelocity(float inputVelocity)
    {
        float outputVelocity = inputVelocity / 10000;
        return outputVelocity;
    }
    public void ChangeSlider(string whatToChange)
    {
        switch (whatToChange)
        {
            case "MaxSpeed":
                MaxSpeed();
                break;
            case "Acceleration":
                Acceleration();
                break;
            case "BrakingSpeed":
                BrakingSpeed();
                break;
            case "TurnSpeed":
                TurnSpeed();
                break;
            case "MaxTurnSpeed":
                MaxTurnSpeed();
                break;
            case "TurnBraking":
                TurnBraking();
                break;
            default:
                Debug.Log(whatToChange + " is not a valid slider");
                break;
        }
    }
    #region SliderValueChange
    void MaxSpeed()
    {
        maxSpeed = maxSpeedSlider.value;
        maxSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = maxSpeedSlider.value.ToString("F0");
    }
    void Acceleration()
    {
        speed = speedSlider.value;
        speedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = speedSlider.value.ToString("F0");
    }
    void BrakingSpeed()
    {
        brakingSpeed = brakingSpeedSlider.value;
        brakingSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = brakingSpeedSlider.value.ToString();
    }
    void TurnSpeed()
    {
        turnSpeed = turnSpeedSlider.value;
        turnSpeedStored = turnSpeed;
        turnSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = turnSpeedSlider.value.ToString();
    }
    void MaxTurnSpeed()
    {
        maxTurnSpeed = maxTurnSpeedSlider.value;
        maxTurnSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = maxTurnSpeedSlider.value.ToString();
    }
    void TurnBraking()
    {
        turnBrakingSpeed = turnBrakeSlider.value;
        turnBrakeSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = turnBrakeSlider.value.ToString();
    }
    #endregion

    IEnumerator SetSliders()
    {
        maxSpeed = maxSpeedSlider.value;
        maxSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = maxSpeedSlider.value.ToString();

        speed = speedSlider.value;
        speedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = speedSlider.value.ToString();

        brakingSpeed = brakingSpeedSlider.value;
        brakingSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = brakingSpeedSlider.value.ToString();

        turnSpeed = turnSpeedSlider.value;
        turnSpeedStored = turnSpeed;
        turnSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = turnSpeedSlider.value.ToString();

        maxTurnSpeed = maxTurnSpeedSlider.value;
        maxTurnSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = maxTurnSpeedSlider.value.ToString();

        turnBrakingSpeed = turnBrakeSlider.value;
        turnBrakeSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = turnBrakeSlider.value.ToString();
        yield return null;
    }
}
