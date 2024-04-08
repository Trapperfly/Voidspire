using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsLoot : GameTrigger
{
    public int level = 1;
    public SpawnResourceOnDestroy resourceSpawner;
    [HideInInspector] public bool noDrop = true;
    [SerializeField] float value;
    [SerializeField] float size;
    [SerializeField] bool valueFromSize;
    [SerializeField] float lootExplosionStrength;
    [SerializeField] float weaponDropChance;
    private void Awake()
    {
        resourceSpawner = GameObject.Find("SpawnResourceHandler").GetComponent<SpawnResourceOnDestroy>();
    }

    private void Start()
    {
        if (GetComponent<ShipAI>())
            value = GetComponent<ShipAI>().ship.value;
    }

    private void OnDestroy()
    {
        if (noDrop)
        {
            return;
        }
        else
        {
            OnKillEvent();
        }
    }

    public override void OnKillEvent()
    {
        base.OnKillEvent();
        TryGetComponent(out ShipAI ship);
        if (ship != null) {
            level = ship.level;
            resourceSpawner.SpawnLoot(value, lootExplosionStrength, transform.position, size, weaponDropChance, level);
        } else
        {
            resourceSpawner.SpawnLoot(value, lootExplosionStrength, transform.position, size, weaponDropChance, level);
        }
    }
}
