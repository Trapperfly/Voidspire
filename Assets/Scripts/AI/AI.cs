using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : ScriptableObject
{
    public string aiName;
    public string description;
    public Relation playerRelation;

}
public enum Relation
{
    None,
    Friendly,
    Aquainted,
    Neutral,
    Disliked,
    Enemy,
    ArchEnemy
}

public enum Faction
{
    None,
    Human,
    Traveler,
    SpacePirate,
    Unknown,
    Moth,
    Nano,
    Error
}
