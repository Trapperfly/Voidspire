using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : ScriptableObject
{
    public string eventName;
    public int id;
    public int difficulty;
    public EventCategory category;

    public PingCondition pingCondition;

    public EventPosition position;
}

public enum EventPosition
{
    None, 
    NearEventChunk,
    RandomClose,
    RandomFarAway
}
public enum EventCategory
{
    None,
    ContactThreaten,
    ContactAsk,
    ContactGiveInformation,
    Store,
    EnemyAmbush,
    AllyAmbushed,
    Derelict,
    BossEvent
}
public enum PingCondition
{
    None,
    AlwaysPing,
    PingWhenClose,
    PingWhenActivated,
    PingWhenConditionIsMet,
    PingAfterTimer
}

[CreateAssetMenu(fileName = "New boss event", menuName = "Events/BossEvent")]
public class BossEvent : Event
{
    public GameObject boss;
    public GameObject[] ads;
    public GameObject[] spawnEnemies;

    public PlayerIsTooFarAwayAction playerIsTooFarAway;

    public float range;
    public float timer;

    public BossEventVictoryCondition winCondition;

    public EventReward reward;
}

public enum PlayerIsTooFarAwayAction
{
    None,
    Despawn,
    LimitPlayerMovement,
    JumpToPlayer
}

public enum BossEventVictoryCondition
{
    None,
    KillEverything,
    KillBossOnly,
    Escape
}

public enum EventReward
{
    None,
    SectorPortalSpawn,
    Loot,
    SpecialLoot
}
