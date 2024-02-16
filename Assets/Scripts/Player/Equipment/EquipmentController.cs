using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using static Inventory;

public class EquipmentController : MonoBehaviour
{
    public delegate void OnGunLoadComplete();
    public OnItemChanged onGunLoadComplete;

    public delegate void OnEquipmentLoadComplete();
    public OnEquipmentLoadComplete onEquipmentLoadComplete;

    [Header("Prefabs")]
    public GameObject bulletPrefab;
    public GameObject beamPrefab;
    public GameObject beamPsPrefab;
    public GameObject railgunPrefab;
    public GameObject railgunPsPrefab;
    public GameObject railgunLinePsPrefab;
    public GameObject laserPrefab;
    public GameObject wavePrefab;
    public GameObject rocketPrefab;
    public GameObject needlePrefab;
    public GameObject minePrefab;
    public GameObject hammerPrefab;
    public GameObject clusterPrefab;
    public GameObject arrowPrefab;
    public GameObject Prefab;
    public GameObject grandPrefab;


    [Header("Testing")]
    public float weightScalar = 0.0001f;

    [SerializeField] Transform bulletHolderMaster;
    [SerializeField] GameObject bulletHolderPrefab;
    public BulletController[] bc;
    public GunFire[] guns;
    public List<List<EquipmentSlot>> equipmentLists = new List<List<EquipmentSlot>>();
    public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();
    public List<EquipmentSlot> weaponSlots = new List<EquipmentSlot>();
    public List<EquipmentSlot> thrusterSlots = new List<EquipmentSlot>();
    //public List<EquipmentSlot> ftlSlots = new List<EquipmentSlot>();
    public List<EquipmentSlot> hullSlots = new List<EquipmentSlot>();
    public List<EquipmentSlot> cargoSlots = new List<EquipmentSlot>();
    public List<EquipmentSlot> collectorSlots = new List<EquipmentSlot>();
    public List<EquipmentSlot> scannerSlots = new List<EquipmentSlot>();
    public List<EquipmentSlot> shieldSlots = new List<EquipmentSlot>();
    public List<EquipmentSlot> relicSlots = new List<EquipmentSlot>();

    #region Singleton
    public static EquipmentController Instance;

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
        equipmentLists.Add(weaponSlots); 
        equipmentLists.Add(thrusterSlots); 
        //equipmentLists.Add(ftlSlots); 
        equipmentLists.Add(hullSlots);
        equipmentLists.Add(cargoSlots); 
        equipmentLists.Add(collectorSlots); 
        equipmentLists.Add(scannerSlots); 
        equipmentLists.Add(shieldSlots);
        equipmentLists.Add(relicSlots);

        equipmentSlots.AddRange(InventoryUI.Instance.eqipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>());
        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            switch (equipmentSlots[i].allowed)
            {
                case EquipmentTypes.None:
                    break;
                case EquipmentTypes.All:
                    break;
                case EquipmentTypes.Weapon:
                    weaponSlots.Add(equipmentSlots[i]);
                    break;
                case EquipmentTypes.Shield:
                    shieldSlots.Add(equipmentSlots[i]);
                    break;
                case EquipmentTypes.Thruster:
                    thrusterSlots.Add(equipmentSlots[i]);
                    break;
                //case EquipmentTypes.FTL:
                //    ftlSlots.Add(equipmentSlots[i]);
                //    break;
                case EquipmentTypes.Hull:
                    hullSlots.Add(equipmentSlots[i]);
                    break;
                case EquipmentTypes.Scanner:
                    scannerSlots.Add(equipmentSlots[i]);
                    break;
                case EquipmentTypes.Cargo:
                    cargoSlots.Add(equipmentSlots[i]);
                    break;
                case EquipmentTypes.Collector:
                    collectorSlots.Add(equipmentSlots[i]);
                    break;
                case EquipmentTypes.Relic:
                    relicSlots.Add(equipmentSlots[i]);
                    break;
                case EquipmentTypes.Default:
                    break;
                default:
                    break;
            }
        }

        bc = new BulletController[weaponSlots.Count];
        guns = new GunFire[weaponSlots.Count];
        //InventoryUI.Instance.onInventoryLoadCallback += 
        LoadBulletHolders();
        //InventoryUI.Instance.onInventoryLoadCallback += 
        LoadEquipment();
        //LoadWeapons();
        //InventoryUI.Instance.onInventoryLoadCallback += 
        LoadGuns();
        Inventory.Instance.onItemChangedCallback += LoadEquipment;

        for (int i = 0; i < bc.Length; i++)
        {
            bc[i].Copy(weaponSlots[i].item as Weapon);
        }
        onEquipmentLoadComplete.Invoke();
        onGunLoadComplete.Invoke();
    }

    public void LoadEquipment()
    {
        onEquipmentLoadComplete.Invoke();
        onGunLoadComplete.Invoke();
        for (int i = 0; i < bc.Length; i++)
        {
            bc[i].Copy(weaponSlots[i].item as Weapon);
        }
    }

    #region Loading weapons and attachments
    public void LoadBulletHolders()
    {
        //Debug.Log("Loading bc");
        int j = 0;
        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            //Debug.Log("Checking slot");
            if (equipmentSlots[i].allowed == EquipmentTypes.Weapon)
            {
                //Debug.Log("Accepted slot, inserting bullet controller");
                bc[j] = Instantiate(bulletHolderPrefab, bulletHolderMaster).GetComponent<BulletController>();
                j++;
            }
        }
    }

    //public void LoadWeapons()
    //{
    //    Debug.Log("Loading weapons");
    //    int j = 0;
    //    for (int i = 0; i < equipmentSlots.Count; i++)
    //    {
    //        if (equipmentSlots[i].allowed == EquipmentTypes.Weapon)
    //        {
    //            if (equipmentSlots[i] != null)
    //            { weapons[j] = equipmentSlots[i]; }
    //            else { weapons[j] = null; }
    //            j++;
    //        }
    //    }
    //    onGunLoadComplete.Invoke();
    //    for (int i = 0; i < bc.Length; i++)
    //    {
    //        bc[i].Copy(weapons[i].item as Weapon);
    //    }
    //}

    public void LoadGuns()
    {
        //Debug.Log("Loading guns");
        GameObject[] gunsGO = GameObject.FindGameObjectsWithTag("GunOnShip");
        for (int i = 0; i < gunsGO.Length; i++)
        {
            guns[i] = gunsGO[i].GetComponent<GunFire>();
        }
    }
    #endregion
}
