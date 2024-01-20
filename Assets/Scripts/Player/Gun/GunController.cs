using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static Inventory;

public class GunController : MonoBehaviour
{
    public delegate void OnGunLoadComplete();
    public OnItemChanged onGunLoadComplete;

    [Header("Prefabs")]
    public GameObject bulletPrefab;
    public GameObject beamPrefab;
    public GameObject beamPsPrefab;
    public GameObject railgunPrefab;
    public GameObject railgunPsPrefab;
    public GameObject railgunLinePsPrefab;
    public GameObject wavePrefab;

    [Header("Testing")]
    public float weightScalar = 0.0001f;

    [SerializeField] Transform bulletHolderMaster;
    [SerializeField] GameObject bulletHolderPrefab;
    public EquipmentSlot[] weapons;
    public BulletController[] bc;
    public GunFire[] guns;
    public EquipmentSlot[] equipmentSlots;
    public EquipmentSlot[] weaponSlots;

    #region Singleton
    public static GunController Instance;

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

    private void Start()
    {
        weaponSlots = new EquipmentSlot[10];
        int j = 0;
        equipmentSlots = InventoryUI.Instance.eqipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].allowed != EquipmentTypes.Weapon) { }
            else { weaponSlots[j] = equipmentSlots[i]; j++; }
        }
        int counter = 0;
        for (int i = 0;i < weaponSlots.Length; i++) { if (weaponSlots[i] != null) { counter++; } }
        Array.Resize(ref weaponSlots, counter);
        weapons = new EquipmentSlot[weaponSlots.Length];
        bc = new BulletController[weaponSlots.Length];
        guns = new GunFire[weaponSlots.Length];
        //InventoryUI.Instance.onInventoryLoadCallback += 
        LoadBulletHolders();
        //InventoryUI.Instance.onInventoryLoadCallback += 
        LoadWeapons();
        //InventoryUI.Instance.onInventoryLoadCallback += 
        LoadGuns();
        Inventory.Instance.onItemChangedCallback += LoadWeapons;

        for (int i = 0; i < bc.Length ; i++)
        {
            bc[i].Copy(weapons[i].item as Weapon);
        }

        onGunLoadComplete.Invoke();
    }
    public void LoadBulletHolders()
    {
        Debug.Log("Loading bc");
        int j = 0;
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            Debug.Log("Checking slot");
            if (equipmentSlots[i].allowed == EquipmentTypes.Weapon)
            {
                Debug.Log("Accepted slot, inserting bullet controller");
                bc[j] = Instantiate(bulletHolderPrefab, bulletHolderMaster).GetComponent<BulletController>(); 
                j++;
            }
        }
    }

    public void LoadWeapons()
    {
        Debug.Log("Loading weapons");
        int j = 0;
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].allowed == EquipmentTypes.Weapon)
            {
                if (equipmentSlots[i] != null)
                { weapons[j] = equipmentSlots[i]; }
                else { weapons[j] = null; }
                j++;
            }
        }
        onGunLoadComplete.Invoke();
        for (int i = 0; i < bc.Length; i++)
        {
            bc[i].Copy(weapons[i].item as Weapon);
        }
    }

    public void LoadGuns()
    {
        Debug.Log("Loading guns");
        GameObject[] gunsGO = GameObject.FindGameObjectsWithTag("GunOnShip");
        for (int i = 0;i < gunsGO.Length;i++)
        {
            guns[i] = gunsGO[i].GetComponent<GunFire>();
        }
    }
}
