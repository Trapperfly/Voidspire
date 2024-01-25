using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New STL engine", menuName = "Inventory/Equipment/STL engine")]
public class STLEngine : Equipment
{
    public STLTypes stlType;
    public float speed;
    public float maxSpeed;
    public float turnSpeed;
    public float turnSpeedStored;
    public float maxTurnSpeed;
    public float turnSpeedBoostUpTo = 40f;
    public float turnBrakingSpeed;
    public float brakingSpeed;
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
