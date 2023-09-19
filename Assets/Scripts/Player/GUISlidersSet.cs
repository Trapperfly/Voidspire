using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUISlidersSet : MonoBehaviour
{
    [Header("Testing Sliders")]
    public Slider maxSpeedSlider;
    public Slider speedSlider;
    public Slider brakingSpeedSlider;
    public Slider turnSpeedSlider;
    public Slider maxTurnSpeedSlider;
    public Slider turnBrakeSlider;
    [SerializeField] ShipControl statsRef;

    private void Awake()
    {
        StartCoroutine(nameof(SetSliders)); //Set the values to the slider values
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
        statsRef.maxSpeed = maxSpeedSlider.value;
        maxSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = maxSpeedSlider.value.ToString("F0");
    }
    void Acceleration()
    {
        statsRef.speed = speedSlider.value;
        speedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = speedSlider.value.ToString("F0");
    }
    void BrakingSpeed()
    {
        statsRef.brakingSpeed = brakingSpeedSlider.value;
        brakingSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = brakingSpeedSlider.value.ToString();
    }
    void TurnSpeed()
    {
        statsRef.turnSpeed = turnSpeedSlider.value;
        statsRef.turnSpeedStored = statsRef.turnSpeed;
        turnSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = turnSpeedSlider.value.ToString();
    }
    void MaxTurnSpeed()
    {
        statsRef.maxTurnSpeed = maxTurnSpeedSlider.value;
        maxTurnSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = maxTurnSpeedSlider.value.ToString();
    }
    void TurnBraking()
    {
        statsRef.turnBrakingSpeed = turnBrakeSlider.value;
        turnBrakeSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = turnBrakeSlider.value.ToString();
    }
    #endregion

    IEnumerator SetSliders()
    {
        statsRef.maxSpeed = maxSpeedSlider.value;
        maxSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = maxSpeedSlider.value.ToString();

        statsRef.speed = speedSlider.value;
        speedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = speedSlider.value.ToString();

        statsRef.brakingSpeed = brakingSpeedSlider.value;
        brakingSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = brakingSpeedSlider.value.ToString();

        statsRef.turnSpeed = turnSpeedSlider.value;
        statsRef.turnSpeedStored = statsRef.turnSpeed;
        turnSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = turnSpeedSlider.value.ToString();

        statsRef.maxTurnSpeed = maxTurnSpeedSlider.value;
        maxTurnSpeedSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = maxTurnSpeedSlider.value.ToString();

        statsRef.turnBrakingSpeed = turnBrakeSlider.value;
        turnBrakeSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = turnBrakeSlider.value.ToString();
        yield return null;
    }
}
