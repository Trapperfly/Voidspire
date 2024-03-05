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
