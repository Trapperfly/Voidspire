using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Collector", menuName = "Inventory/Equipment/Collector")]
public class Collector : Equipment
{
    public CollectorTypes collectorType;
    public float collectorSpeedTo;
    public float collectorSpeedFrom;
    public float recharge;
    public float range;
    public int amount;
}
public enum CollectorTypes
{
    Grabber, //Medium speed back and forth
    Harpoon, //Fast speed to, but slow back
    Tractor,
    Drone,
    Default
}