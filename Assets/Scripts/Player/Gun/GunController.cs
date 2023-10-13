using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] bool equipped;
    [SerializeField] Transform bulletHolderMaster;
    [SerializeField] GameObject bcPrefab;
    GameObject connectedBulletController;
    public BulletController bc;
    private void Awake()
    {
        //Instant equip for starter weapons and equipped weapons whel loading game
        if (equipped) Equip();
    }

    public void Equip()
    {
        connectedBulletController = Instantiate(bcPrefab, bulletHolderMaster);
        bc = connectedBulletController.GetComponent<BulletController>();
    }

    public void Swap()
    {
        Destroy(connectedBulletController);
        connectedBulletController = Instantiate(bcPrefab, bulletHolderMaster);
        bc = connectedBulletController.GetComponent<BulletController>();
    }

    public void Unequip()
    {
        Destroy(connectedBulletController);
    }
}
