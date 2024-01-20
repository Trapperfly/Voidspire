using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStats : MonoBehaviour
{
    public int gunNumber;
    public bool active;
    public bool aimed = true;

    [Header("Type of weapon")]
    public BulletType bulletType;

    [Header("Stats")]
    public AnimationCurve spreadCurve;
    public AnimationCurve speedCurve;
}
