using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEvent : MonoBehaviour
{
    public bool active;

    public Event _event;
    public bool iTimer;
    public float timer;

    public GameObject mainTarget;
    public List<GameObject> sideTargets = new List<GameObject>();

    public Vector2 logPos;
    public GameObject SectorPortal;

    public bool complete;
    // Start is called before the first frame update
    void Start()
    {
        switch (_event.position)
        {
            case EventPosition.NearEventChunk:
                break;
            case EventPosition.RandomClose:
                break;
            case EventPosition.RandomFarAway:
                transform.position = new Vector2(Random.Range(-10000, 10000), Random.Range(-10000, 10000));
                break;
            default:
                break;
        }
        switch (_event.pingCondition)
        {
            case PingCondition.None:
                break;
            case PingCondition.AlwaysPing:
                gameObject.name = _event.eventName;
                PointerSystem.Instance.AddPing(transform);
                break;
            case PingCondition.PingWhenClose:
                break;
            case PingCondition.PingWhenActivated:
                break;
            case PingCondition.PingWhenConditionIsMet:
                gameObject.name = _event.eventName;
                StartCoroutine(nameof(CheckIfConditionIsMet));
                break;
            case PingCondition.PingAfterTimer:
                iTimer = true;
                break;
            default:
                break;
        }
        StartCoroutine(CheckPlayerDistanceToEvent());
    }

    IEnumerator CheckIfConditionIsMet()
    {
        yield return new WaitForSeconds(10);
        bool isMet = false;
        if (PointerSystem.Instance.bossHints >= _event.conditionValue)
        {
            PointerSystem.Instance.AddPing(transform);
            isMet = true;
        }
        if (!isMet) StartCoroutine(nameof(CheckIfConditionIsMet));
    }

    IEnumerator CheckPlayerDistanceToEvent()
    {
        if (Vector2.Distance(GlobalRefs.playerPos, transform.position) < ChunkLoader.viewDist)
        {
            active = true;
            StartEvent();
        }
        yield return new WaitForSeconds(1);
        if (!active)
            StartCoroutine(CheckPlayerDistanceToEvent());
    }

    void StartEvent()
    {
        switch (_event.category)
        {
            case EventCategory.None:
                break;
            case EventCategory.ContactThreaten:
                break;
            case EventCategory.ContactAsk:
                break;
            case EventCategory.ContactGiveInformation:
                break;
            case EventCategory.Store:
                break;
            case EventCategory.EnemyAmbush:
                break;
            case EventCategory.AllyAmbushed:
                break;
            case EventCategory.Derelict:
                break;
            case EventCategory.BossEvent:
                BossEvent b = _event as BossEvent;
                GameObject boss =
                Instantiate
                (
                    b.boss,
                    (Vector2)transform.position + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    SpawnEnemies.Instance.transform
                );
                boss.GetComponent<ShipAI>().level = GlobalRefs.Instance.currentSector * 10;
                mainTarget = boss;
                for (int i = 0; i < b.ads.Length; i++)
                {
                    GameObject ad =
                    Instantiate
                    (
                        b.ads[i],
                        (Vector2)transform.position + (Vector2)Random.insideUnitSphere,
                        Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                        SpawnEnemies.Instance.transform
                    );
                    ad.GetComponent<ShipAI>().level = GlobalRefs.Instance.currentSector * 10;
                    sideTargets.Add(ad);
                }

                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!active) return;
        if (complete) return;
        switch ((_event as BossEvent).winCondition)
        {
            case BossEventVictoryCondition.KillEverything:
                break;
            case BossEventVictoryCondition.KillBossOnly:
                if (!mainTarget)
                {
                    GlobalRefs.Instance.player.GetComponent<ShipControl>().ftlDisabled = false;
                    complete = true;
                    HandleVictory();
                }
                break;
            case BossEventVictoryCondition.Escape:
                break;
            default:
                break;
        }
        switch (_event.category)
        {
            case EventCategory.None:
                break;
            case EventCategory.ContactThreaten:
                break;
            case EventCategory.ContactAsk:
                break;
            case EventCategory.ContactGiveInformation:
                break;
            case EventCategory.Store:
                break;
            case EventCategory.EnemyAmbush:
                break;
            case EventCategory.AllyAmbushed:
                break;
            case EventCategory.Derelict:
                break;
            case EventCategory.BossEvent:
                if (mainTarget)
                    logPos = mainTarget.transform.position;
                    switch ((_event as BossEvent).playerIsTooFarAway)
                    {
                        case PlayerIsTooFarAwayAction.Despawn:
                            break;
                        case PlayerIsTooFarAwayAction.LimitPlayerMovement:
                            GlobalRefs.Instance.player.GetComponent<ShipControl>().ftlDisabled = true;
                            if (Vector2.Distance(GlobalRefs.playerPos, transform.position) > ChunkLoader.viewDist)
                            {
                                GlobalRefs.Instance.player.GetComponent<Rigidbody2D>().AddForce(
                                    ((Vector2)mainTarget.transform.position-GlobalRefs.playerPos).normalized * 10,ForceMode2D.Force);
                            }
                            break;
                        case PlayerIsTooFarAwayAction.JumpToPlayer:
                            break;
                        default:
                            break;
                    }
                break;
            default:
                break;
        }
    }

    void HandleVictory()
    {
        switch ((_event as BossEvent).reward)
        {
            case EventReward.SectorPortalSpawn:
                GameObject portal = Instantiate(SectorPortal, new Vector2(logPos.x, logPos.y + 4),new Quaternion(),null);
                GlobalRefs.Instance.clearThese.Add(portal);
                break;
            case EventReward.Loot:
                break;
            case EventReward.SpecialLoot:
                break;
            default:
                break;
        }
    }
}
