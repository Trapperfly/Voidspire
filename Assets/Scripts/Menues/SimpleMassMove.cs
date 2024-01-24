using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMassMove : MonoBehaviour
{
    [SerializeField] SimpleSelection selection;
    [SerializeField] bool startWhenAwake;
    [SerializeField] bool startAtStartPos;
    [SerializeField] bool currentposIsStartpos;
    [SerializeField] bool resetOnDisable;
    public GameObject[] objects;
    public Vector3[] positions;
    [SerializeField] float duration;
    [SerializeField] AnimationCurve moveSpeedCurve;
    [SerializeField] AnimationCurve moveBackSpeedCurve;
    float currTime;
    float progress;
    [HideInInspector] public bool active;
    [HideInInspector] public bool back;

    private void Awake()
    {
        progress = currTime / duration;
        foreach (var obj in objects)
        {
            if (currentposIsStartpos) positions[System.Array.IndexOf(objects, obj)] = obj.transform.localPosition;
            if (startAtStartPos) obj.transform.localPosition = Vector2.Lerp(positions[System.Array.IndexOf(objects, obj)], positions[3], CurveOverTime(moveSpeedCurve));
            if (startWhenAwake) StartMove(obj, 3);
        }
    }
    public void StartMove()
    {
        foreach (var obj in objects)
        {
            if (System.Array.IndexOf(objects, obj) == selection.selected) StartMove(obj, 3);
        }
    }
    public void StartMove(GameObject go, int to)
    {
        if (!active && back) StartCoroutine(Move(go, -1, to));
        else if (!active) StartCoroutine(Move(go, 1, to));
        active = true;

    }

    IEnumerator Move(GameObject go,int direction, int to)
    {
        currTime += direction * Time.deltaTime;
        progress = currTime / duration;
        if (direction == 1) go.transform.localPosition = Vector2.Lerp(positions[System.Array.IndexOf(objects, go)], positions[3], CurveOverTime(moveSpeedCurve));
        else go.transform.localPosition = Vector2.Lerp(positions[System.Array.IndexOf(objects, go)], positions[3], CurveOverTime(moveBackSpeedCurve));
        yield return new WaitForEndOfFrame();
        if (progress >= 1)
        {
            active = false;
            back = true;
        }
        else if (progress <= 0)
        {
            active = false;
            back = false;
        }
        else StartCoroutine(Move(go ,direction, to));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (resetOnDisable) foreach (var obj in objects)
            {
                obj.transform.localPosition = positions[System.Array.IndexOf(objects, obj)];
            }

    }

    float CurveOverTime(AnimationCurve curve)
    {
        return curve.Evaluate(progress);
    }
}
