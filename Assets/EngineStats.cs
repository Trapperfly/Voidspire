using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStats : MonoBehaviour
{
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
}
