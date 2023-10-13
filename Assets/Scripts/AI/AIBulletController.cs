using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBulletController : MonoBehaviour
{
    private void FixedUpdate()
    {
        foreach (Transform child in transform)
        {
            AIBullet _aiBullet = child.GetComponent<AIBullet>();
            if (_aiBullet == null)
            {
                Debug.Log("Bullet is null, help!");
            }
            else
            {
                if (_aiBullet.currTime <= Time.time - 4f)
                {
                    Destroy(_aiBullet.gameObject);
                }
            }
        }
    }
}
