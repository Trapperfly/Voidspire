using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComEventResponsive", menuName = "ComEvent/Responsive")]
public class Com : ScriptableObject
{
    public string storyTitle;
    [TextArea]
    public string storyText;
    public List<ComResultTotal> result = new();
    [Space]
    public List<ComResponse> responses = new();
}

[System.Serializable]
public class ComResponse
{
    public string name;
    public ComDependance dependance;
    public string responseText;
    public List<Com> resultsBetween = new();
    public bool isExit;
}

[System.Serializable]
public class ComResultTotal
{
    public ComResultEnum result;
    public float resultValue;
}

public enum ComDependance
{
    None,
    RelationshipBetterThanBad,
    RelationshipWorseThanGood,
    RelationshipVeryGood,
    RelationshipVeryBad,
    PlayerTakenDamage,
    TakenDamage,
    LowDamage,
    PlayerHasResources,
    PlayerIsHuman,
    Error
}

public enum ComResultEnum
{
    None,
    Nothing,
    DisengageCombat,
    EngageCombat,
    ResourceChange,
    LootChange,
    PlayerHealthChange,
    PlayerFuelChange,
    ShipHealthChange,
    OpenShop,
    GiveEventPing,
    GiveBossClue
}
