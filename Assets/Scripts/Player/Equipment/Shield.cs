using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Shield", menuName = "Inventory/Equipment/Shield")]
public class Shield : Equipment
{
    public ShieldType shieldType;
    public float shieldHealth;
    public float shieldRechargeSpeed;
    public float shieldRechargeDelay;
    public float shieldBreakAnimTime;
    public float shieldRestoreAnimTime;
}
public enum ShieldType
{
    Hardlight,
    Energy,
    Obliterator,
    Portal,
    Forcefield,
    Mirror,
    Phaze,
    Default
}
