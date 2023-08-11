using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    bool reachedTarget = false;
    bool failed = false;
    bool reachedHome = false;
    float backHomeSpeed;
    float progress;
    float relativity;
    GameObject player;
    GameObject wallet;
    LineRenderer line;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wallet = GameObject.Find("PickupCollider");
        line = GetComponent<LineRenderer>();
    }
    public IEnumerator GoToTarget(GameObject target, int ID, float forwardSpeed, float backSpeed, float range)
    {
        float progressTarget = forwardSpeed * 60;
        relativity = forwardSpeed / backSpeed;
        backHomeSpeed = backSpeed;
        float progressPercent = 0;
        while (!reachedTarget && failed == false)
        {
            progress += 1;
            progressPercent = (Mathf.Clamp(progress / progressTarget, 0, 1));
            if (target != null)
            {
                transform.position = player.transform.position + (progressPercent * (target.transform.position - player.transform.position));
            }
            else
            {
                failed = true;
            }
            if (Vector2.Distance(target.transform.position, player.transform.position) > range + 0.2f)
            {
                failed = true;
            }
            if (progressPercent == 1)
            {
                reachedTarget = true;
            }
            line.SetPosition(0, player.transform.position);
            line.SetPosition(1, transform.position);
            yield return new WaitForFixedUpdate();
        }
        if (failed)
        {
            Debug.Log("Failed to grab");
        }
        else if (target != null)
        {
            target.transform.parent.transform.parent.SetParent(transform);
        }
        if (transform.childCount > 0)
        {
            GameObject GrabbedResource = transform.GetChild(0).gameObject;
            GrabbedResource.transform.localPosition = new Vector3(0, 0, 0);
            GrabbedResource.GetComponent<Rigidbody2D>().isKinematic = true;
            GrabbedResource.GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
        StartCoroutine(GoHome(target.transform.position));
        yield return null;
    }

    IEnumerator GoHome(Vector3 position)
    {
        float progressMultiplier = 1f;
        if (failed)
            progressMultiplier = 2f;
        float progressTarget = backHomeSpeed * 60;
        progress /= relativity;
        while (!reachedHome)
        {
            float progressPercent = (Mathf.Clamp(progress / progressTarget, 0, 1));
            progress -= 1 * progressMultiplier;
            transform.position = player.transform.position + (progressPercent * (position - player.transform.position));
            if (progressPercent == 0)
            {
                reachedHome = true;
            }
            line.SetPosition(0, player.transform.position);
            line.SetPosition(1, transform.position);
            yield return new WaitForFixedUpdate();
        }
        if (transform.childCount != 0)
            wallet.GetComponent<StartPickup>().wallet += transform.GetComponentInChildren<Resource>().worth;
        DestroyMe();
        yield return null;
    }
    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
