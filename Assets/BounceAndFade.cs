using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.PlayerSettings;

public class BounceAndFade : MonoBehaviour
{
    public bool doAnim;
    public TMP_Text text;
    public AnimationCurve fadeCurve;
    public float strength = 1;
    public float fadeTime = 1;
    float timer = 0;
    Vector2 dir = Vector2.zero;
    float percent = 0;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        dir = new Vector2 (Random.Range(-0.5f, 1f), Random.Range(0.2f, 1f));
    }

    void Update()
    {
        if (doAnim) {
            percent = timer / fadeTime;
            text.color = new Color(1, 1, 1, fadeCurve.Evaluate(percent));
            Vector2 pos = transform.position;
            if (timer < fadeTime)
            {
                pos += strength * Time.deltaTime * dir;
                transform.position = pos;
            }
            else { Destroy(gameObject); }
        }
        else
        {
            if (timer > fadeTime)
            {
                Destroy(gameObject);
            }
        }
        timer += Time.deltaTime;
    }
}
