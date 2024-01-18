using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeEquipment : MonoBehaviour
{
    [Header("Arrays")]
    [SerializeField] Sprite[] gunSprites;
    [SerializeField] Sprite[] hullSprites;
    [SerializeField] Sprite[] shieldSprites;
    [SerializeField] Sprite[] cargoSprites;
    [SerializeField] Sprite[] ftlSprites;
    [SerializeField] Sprite[] stlSprites;
    [Space]
    [SerializeField] string[] nameFirst;
    [SerializeField] string[] nameMid;
    [SerializeField] string[] nameLast;

    #region Singleton
    public static RandomizeEquipment Instance;

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
    #endregion

    public int R(int min, int max)
    {
        return (int)Random.Range((float)min, (float)max);
    }

    public Weapon RandomizeGun()
    {
        Weapon weapon = ScriptableObject.CreateInstance<Weapon>();
        weapon.itemName = new string
            (nameFirst[R(0, nameFirst.Length)]
            + " "
            + nameMid[R(0, nameMid.Length)] 
            + " " 
            + nameLast[R(0, nameLast.Length)]);
        weapon.name = weapon.itemName;
        weapon.id = Inventory.Instance.id;
        weapon.icon = gunSprites[R(0, gunSprites.Length)];
        return weapon;
    }
}
