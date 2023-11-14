using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Follow : MonoBehaviour
{
    [SerializeField] bool follow;
    [SerializeField] bool followMouse;
    [SerializeField] bool away;
    [SerializeField] float magnitude;
    [SerializeField] Transform followTransform;
    [SerializeField] float followSpeed;
    [SerializeField] float updateSpeed;
    int modifier;
    private void Start()
    {
        modifier = away ? -1 : 1; 
        
    }
    private void OnEnable()
    {
        StartCoroutine(nameof(FollowThing));
    }
    IEnumerator FollowThing()
    {
        while (true)
        {
            if (!follow)
            {
                yield return null;
            }
            else
            {
                if (followMouse)
                {
                    Vector2 currentPosition = transform.position;
                    Vector2 target = Extension.AsVector2
                        (transform.parent.position + Camera.main.ScreenToWorldPoint(Input.mousePosition) * modifier * magnitude);
                    transform.position = Vector2.Lerp(currentPosition, target, followSpeed);
                    yield return new WaitForSeconds(1 / updateSpeed);
                }
                else
                {
                    Vector2 currentPosition = transform.position;
                    Vector2 target = Extension.AsVector2(followTransform.position);
                    transform.position = Vector2.Lerp(currentPosition, target, followSpeed);
                    yield return new WaitForSeconds(1 / updateSpeed);
                }
            }
        }
    }

}
