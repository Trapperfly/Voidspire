using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsLoot : Events
{
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
        if (valueFromSize)
            resourceSpawner.SpawnLoot(transform.localScale.x, transform.localScale.x, transform.position, transform.localScale.x, weaponDropChance);
        else resourceSpawner.SpawnLoot(value, lootExplosionStrength, transform.position, size, weaponDropChance);
    }
}
