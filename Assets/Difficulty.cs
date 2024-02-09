using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public float difficulty;
    public float difSetting = 1;
    public float hardcap;
    public float increasePer5Mins;

    #region Singleton
    public static Difficulty dif;
    private void Awake()
    {
        if (dif != null && dif != this)
        {
            Destroy(this);
        }
        else
        {
            dif = this;
        }
    }
    #endregion

    private void Update()
    {
        difficulty += ((increasePer5Mins - (difficulty / hardcap) ) * Time.deltaTime) / 300;
    }
}
