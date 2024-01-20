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
    float zPos;

    private void Awake()
    {
        zPos = transform.position.z;
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
                Vector3 target = new Vector3(targetPosition.transform.position.x, targetPosition.transform.position.y, zPos);
                transform.position = Vector3.Lerp(new Vector3(currentPosition.x, currentPosition.y, zPos), target, followSpeed);
                yield return new WaitForSeconds(updateSpeed);
            }
        }
    }
}
