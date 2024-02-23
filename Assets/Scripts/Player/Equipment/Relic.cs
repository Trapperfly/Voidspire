using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(fileName = "New Relic", menuName = "Inventory/Equipment/Relic")]
//[System.Serializable]
//public class Relic
//{

//}

[CreateAssetMenu(fileName = "QuantumTargeting", menuName = "Inventory/Equipment/Relic/QuantumTargeting")]
public class QuantumTargeting : Equipment
{
    public static bool setHomingToThis = true;
    public static float aHomingStrength = 100f;
    public static float mProjectileSpeed = 0.75f;
}

[CreateAssetMenu(fileName = "FissionBarrel", menuName = "Inventory/Equipment/Relic/FissionBarrel")]
public class FissionBarrel : Equipment
{
    public static float mProjectileCount = 100f;
    public static float mSpread = 1.25f;
    public static float mRotSpeed = 0.75f;
}

[CreateAssetMenu(fileName = "FriendModule", menuName = "Inventory/Equipment/Relic/FriendModule")]
public class FriendModule : Equipment
{
    public static bool setFriendActive = true;
}

