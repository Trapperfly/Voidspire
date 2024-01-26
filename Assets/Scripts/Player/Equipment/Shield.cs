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
    public Color shieldColor;
    public Color breakColor;
}
public enum ShieldType
{
    Hardlight,
    Energy,
    Phaze,
    Obliterator,
    Portal,
    Forcefield,
    Mirror,
    Default
}
