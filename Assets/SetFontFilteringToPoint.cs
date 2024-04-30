using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class SetFontFilteringToPoint : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().fontMaterial.mainTexture.filterMode = FilterMode.Point;
    }
}