using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        gameObject.tag = "Player";
    }
}
