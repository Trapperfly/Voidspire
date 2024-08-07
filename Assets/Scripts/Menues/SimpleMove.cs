using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] bool startWhenAwake;
    [SerializeField] bool startAtStartPos;
    [SerializeField] bool currentposIsStartpos;
    [SerializeField] bool startOnEnable;
    [SerializeField] bool resetOnDisable;
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
        if (currentposIsStartpos) positions[0] = transform.localPosition;
        progress = currTime / duration;
        if (startAtStartPos) transform.localPosition = Vector2.Lerp(positions[0], positions[1], CurveOverTime(moveSpeedCurve));
        if (startWhenAwake) StartMove(0,1);
    }
    public void StartMove()
    {
        StartMove(0, 1);
    }
    public void StartMove(int from, int to)
    {
        if (!active && back) StartCoroutine(Move(-1, from, to));
        else if (!active)
        {
            if (currentposIsStartpos) positions[0] = transform.localPosition;
            StartCoroutine(Move(1, from, to));
        }

    }

    IEnumerator Move(int direction, int from, int to)
    {
        active = true;
        currTime += direction * Time.deltaTime;
        progress = currTime / duration;
        if (direction == 1) transform.localPosition = Vector2.Lerp(positions[from], positions[to], CurveOverTime(moveSpeedCurve));
        else transform.localPosition = Vector2.Lerp(positions[from], positions[to], CurveOverTime(moveBackSpeedCurve));
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
        else StartCoroutine(Move(direction, from, to));
    }

    private void OnEnable()
    {
        if (startOnEnable) StartMove(0, 1);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        if (resetOnDisable)
        {
            transform.localPosition = positions[0];
            currTime = 0;
            back = false;
        }
    }

    float CurveOverTime(AnimationCurve curve)
    {
        return curve.Evaluate(progress);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var pos in positions)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pos, 0.3f);
        }
    }
}
