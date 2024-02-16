using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New thrusters", menuName = "Inventory/Equipment/Thrusters")]
public class Thrusters : Equipment
{
    public STLTypes stlType;
    public FTLTypes ftlType;
    public float speed;
    public float maxSpeed;
    public float turnSpeed;
    public float maxTurnSpeed;
    public float turnBrakingSpeed;
    public float brakingSpeed;

    public float ftlAcc; // speed * 2 * 10
    public float ftlMaxSpeed; // max speed / 2 * 10
    public float ftlRotSpeed; // turn speed / 5

    //public float ftlSpeedMulti;
    //public float ftlRotMulti;
    public float chargeTime;
    public float fuelCurrent;
    public float fuelMax;
    public float fuelDrain;
    public float maxDuration;


}
public enum STLTypes
{
    Drive,
    Agile,
    Pulse,
    Boost,
    Sail,
    Crawler,
    Default
}
public enum FTLTypes
{
    Ready,
    Burst,
    Flight,
    Scout,
    Crash,
    Default
}
