using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelect : MonoBehaviour
{
    public TMP_Dropdown leftDrop;
    public TMP_Dropdown rightDrop;
    public GunStats leftGun;
    public GunStats rightGun;
    private void Awake()
    {
        ChangeWeapon(true);
        ChangeWeapon(false);
    }
    public void ChangeWeapon(bool left)
    {
        TMP_Dropdown currentDrop = left ? leftDrop : rightDrop;
        switch (currentDrop.value)
        {
            case 0:
                SetWeapon(left, 0);
                break;
            case 1:
                SetWeapon(left, 13);
                break;
            case 2:
                SetWeapon(left, 5);
                break;
            default:
                break;
        }
    }

    void SetWeapon(bool left, int bTypeInt)
    {
        GunStats selectedGun = left ? leftGun : rightGun;
        selectedGun.bulletType = (BulletType)bTypeInt;
    }
}
