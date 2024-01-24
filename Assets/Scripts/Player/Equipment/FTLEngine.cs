using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New FTL engine", menuName = "Inventory/Equipment/FTL engine")]
public class FTLEngine : Equipment
{
    public FTLTypes ftlType;
    public float acceleration;
    public float maxSpeed;
    public float rotSpeed;
    public float chargeTime;
    public float fuelMax;
    public float fuelDrain;
    public float maxDuration;
}
public enum FTLTypes
{
    Ready,
    Burst,
    Flight,
    Crash,
    Scout,
    Default
}
