using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSelection : MonoBehaviour
{
    [SerializeField] GameObject[] selection;
    public int selected;

    public void SetSelected(int select)
    {
        selected = select;
    }
    public void RemoveOtherThanSelected()
    {
        int count = 0;
        foreach (var possible in selection)
        {
            if (count != selected) possible.SetActive(false);
            count++;
        }
    }

    public void ReturnAll()
    {
        foreach (var possible in selection)
        {
            possible.SetActive(true);
        }
    }
}
