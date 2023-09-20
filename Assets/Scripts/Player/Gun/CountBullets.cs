using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountBullets : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    Transform bulletHolder;
    private void Awake()
    {
        bulletHolder = GameObject.FindGameObjectWithTag("BulletHolder").transform;
    }
    private void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        text.text = bulletHolder.childCount.ToString();
    }
}
