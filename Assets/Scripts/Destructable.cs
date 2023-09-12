using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] float delay = 0f;
    public IEnumerator DestroyMe()
    {
        Destroy(gameObject, delay);
        yield return null;
    }
}
