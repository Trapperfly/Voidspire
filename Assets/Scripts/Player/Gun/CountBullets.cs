using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountBullets : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Transform bulletHolder;
    private void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        int bulletCount = 0;
        foreach (Transform child in bulletHolder.transform)
        {
            bulletCount += child.childCount;
        }
        text.text = "Proj: " + bulletCount.ToString();
    }
}
