using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDebris : MonoBehaviour
{
    [SerializeField] float spawnRateInSeconds = 0.2f;
    [SerializeField] int maxAmount;
    public float minSize = 0.1f;
    public float maxSize = 2f;
    [SerializeField] GameObject debrisPrefab;
    public float maxDistance;


    private void Awake()
    {
        StartCoroutine(nameof(Spawn));
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if (transform.childCount <= maxAmount)
            {
                Vector3 spawnLocation = Camera.main.ViewportToWorldPoint(RandomOutsideView());
                GameObject debris = Instantiate(debrisPrefab, spawnLocation, Quaternion.Euler(0,0,0), transform);
            }
            yield return new WaitForSeconds(spawnRateInSeconds);
        }
    }

    Vector3 RandomOutsideView()
    {
        int whichSide = Random.Range(1, 5);
        float _x = 0;
        float _y = 0;
        switch (whichSide)
        {
            case 1: //Up
                _x = Random.Range(-0.4f, 1.4f);
                _y = Random.Range(1.2f, 1.4f);
                break;
            case 2: //Right
                _x = Random.Range(1.2f, 1.4f);
                _y = Random.Range(-0.4f, 1.4f);
                break;
            case 3: //Down
                _x = Random.Range(-0.4f, 1.4f);
                _y = Random.Range(-0.2f, -0.4f);
                break;
            case 4: //Left
                _x = Random.Range(-0.2f, -0.4f);
                _y = Random.Range(-0.4f, 1.4f);
                break;
            default:
                Debug.Log("Something went wrong");
                break;
        }
        return new Vector3(_x, _y, 10);
    }
}
