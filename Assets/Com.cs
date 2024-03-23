using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_ComEventResponsive", menuName = "ComEvent/Responsive")]
public class Com : ScriptableObject
{

    public List<ComResponse> responses = new List<ComResponse>();
}

[System.Serializable]
public class ComResponse
{
    public string name;
    public ComDependance dependance;
    public string responseText;
    public ComResult result;
    public float resultValue;
}

public enum ComDependance
{
    None,
    Friendly,
    Enemy,
    TakenDamage,
    PlayerHasResources,
    Error
}

public enum ComResult
{
    None,
    Nothing,
    DisengageCombat,
    EngageCombat,
    GiveLoot,
    RepairShip,
    RestoreFuel,
    DamageShip,
    OpenShop,
    GiveEventPing,
    GiveBossClue
}
