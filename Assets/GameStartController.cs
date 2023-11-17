using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    public float animTime;
    public bool inAnim = false;
    public Button[] buttons;
    float currTime;

    public void SetToAnim()
    {
        inAnim = true;
    }

    private void Update()
    {
        if (inAnim) foreach (var b in buttons)
            {
                b.enabled = false;
            }
        else foreach (var b in buttons)
            {
                b.enabled = true;
            }

        if (inAnim)
        {
            if (currTime >= animTime)
            {
                inAnim = false;
                currTime = 0;
            }
            else
            {
                currTime += 1 * Time.deltaTime;
            }
        }
    }
}
