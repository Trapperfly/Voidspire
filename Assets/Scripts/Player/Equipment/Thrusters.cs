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
    Drive, //All round
    Agile, //Slower but better rotation
    Pulse, //Faster but slower rotation
    Boost, //Even faster but less control in rotation
    Sail, //Generalized but less friction
    Crawler, //Very slow but great control
    Default
}
public enum FTLTypes
{
    Ready, //Very short FTL charge-up
    Burst, //Very fast but short FTL duration
    Flight, //Generalized but cannot rotate during FTL
    Scout, //Good rotation during FTL but more fuel used
    Crash,
    Default
}
