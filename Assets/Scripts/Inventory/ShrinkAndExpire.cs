using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrinkAndExpire : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 endPos;
    public float canvasScale;
    private void Update()
    {
        
    }
    public IEnumerator Shrink()
    {
        float time = 0;
        Vector2 newStartPos = startPos;
        while (time < 1)
        {
            newStartPos = Vector2.Lerp(startPos, endPos, time);
            transform.position = Vector2.Lerp(newStartPos, endPos, 0.5f);
            transform.localScale = new Vector2(0.1f, Vector2.Distance(newStartPos, endPos) / canvasScale / 100);
            time += 5 * Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(transform.gameObject);
        yield return null;
    }

    public IEnumerator Expire()
    {

        Image imageComp = GetComponent<Image>();
        imageComp.color = Color.red;
        float time = 0;
        while (time < 1)
        {
            time += 2f * Time.unscaledDeltaTime;
            imageComp.color = new Color(1,0,0,1-time);
            yield return new WaitForEndOfFrame();
        }
        Destroy(transform.gameObject);
        yield return null;
    }
}
