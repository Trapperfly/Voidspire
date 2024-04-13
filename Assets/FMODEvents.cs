using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player movement")]
    [field: SerializeField] public EventReference mainThruster {  get; private set; }
    [field: SerializeField] public EventReference sideThruster { get; private set; }
    [field: SerializeField] public EventReference rotateThruster { get; private set; }
    [field: SerializeField] public EventReference backThruster { get; private set; }
    [field: SerializeField] public EventReference ftlAmbient { get; private set; }
    [field: SerializeField] public EventReference ftlCharge { get; private set; }
    [field: SerializeField] public EventReference ftlExit { get; private set; }

    [field: Header("Gun")]
    [field: SerializeField] public EventReference gunFire { get; private set; }

    [field: Header("Com")]
    [field: SerializeField] public EventReference comEventStart { get; private set; }

    [field: Header("Hit")]
    [field: SerializeField] public EventReference playerHit { get; private set; }
    [field: SerializeField] public EventReference shieldHit { get; private set; }
    [field: SerializeField] public EventReference enemyHit { get; private set; }
    [field: SerializeField] public EventReference explosion { get; private set; }

    [field: Header("Shield")]
    [field: SerializeField] public EventReference shieldDown { get; private set; }
    [field: SerializeField] public EventReference shieldUp { get; private set; }


    [field: Header("UI")]
    [field: SerializeField] public EventReference hover { get; private set; }
    [field: SerializeField] public EventReference click { get; private set; }


    public static FMODEvents Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


}
