using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public float difficulty;
    public float difSetting = 1;
    public float hardcap;
    public float increasePer5Mins;

    public float AIDamageIncreasePerLevel;
    public float AIFireRateIncreasePerLevel;
    public float AISpeedIncreasePerLevel;
    public float AICombatTimeIncreasePerLevel;
    public float AIHealthIncreasePerLevel;

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
