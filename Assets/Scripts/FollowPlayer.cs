using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] bool follow;
    [SerializeField] float followSpeed;
    [SerializeField] float updateSpeed;
    GameObject targetPosition;

    private void Awake()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        StartCoroutine(nameof(Follow));
    }
    IEnumerator Follow()
    {
        while (true)
        {
            if (!follow)
            {
                yield return null;
            }
            else
            {
                Vector2 currentPosition = transform.position;
                Vector2 target = Extension.AsVector2(targetPosition.transform.position);
                transform.position = Vector2.Lerp(currentPosition, target, followSpeed);
                yield return new WaitForSeconds(updateSpeed);
            }
        }
    }
}
