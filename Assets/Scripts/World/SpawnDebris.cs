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
                GameObject debris = Instantiate(debrisPrefab, spawnLocation, Quaternion.Euler(0,0,(Random.value * 360) - 180), transform);
                Rigidbody2D debrisRb = debris.GetComponent<Rigidbody2D>();

                SetSize(debris.transform);
                float size = (debris.transform.localScale.x + debris.transform.localScale.y) / 2;
                debrisRb.mass = Mathf.Pow(size, 3);

                SetDirection();
                debrisRb.velocity = SetDirection() * Random.Range(0, debrisRb.mass / 5);
                debrisRb.angularVelocity = Random.Range(-debrisRb.mass, debrisRb.mass);
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
    void SetSize(Transform instance)
    {
        float weight = Random.Range(1, 5);
        if (weight < 4)
        {
            float size = Random.Range(minSize, maxSize / 3);
            instance.localScale = new Vector2(size, size);
        }
        else if (weight == 4)
        {
            float size = Random.Range(minSize, maxSize);
            instance.localScale = new Vector2(size, size);
        }
        else
        {
            float size = Random.Range(minSize * 200, maxSize * 2);
            instance.localScale = new Vector2(size, size);
        }
    }
    Vector2 SetDirection()
    {
        float _x = Random.Range(-1, 2);
        float _y = Random.Range(-1, 2);
        return new Vector2(_x, _y);
    }
}
