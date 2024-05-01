using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : ScriptableObject
{
    public int level;
    public string aiName;
    public Faction faction;
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
    SpacePirate,
    Unknown,
    Chitin,
    Nano,
    Error
}
