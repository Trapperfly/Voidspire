using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class SpawnResourceOnDestroy : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab;
    public int amountScalar = 10;
    int amount;
    public int resourceWorth;
    public IEnumerator SpawnResources(float value, float overkill, Vector2 position)
    {
        amount = Mathf.Clamp((int)(value * amountScalar), 1, 1000);

        for (int i = 0; i < amount; i++)
        {
            GameObject resource = Instantiate(resourcePrefab, RandomVector2InsideCircle(position, value), new Quaternion(0, 0, 0, 0), transform);
            Rigidbody2D resourceRb = resource.GetComponent<Rigidbody2D>();
            Resource ResourceValues = resource.GetComponent<Resource>();
            resource.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            resourceRb.AddExplosionForce(value/5, position, value, 0, ForceMode2D.Impulse);
            resource.GetComponentInChildren<SpriteRenderer>().color = new Color(1, Mathf.Clamp(1f - ((value * 0.1f) * Random.Range(0f, 3f)), 0f, 1f), 0);
            float smallValue = Mathf.Clamp(Mathf.Sqrt(value)/2, 0.4f, 1f);
            resource.transform.GetChild(0).transform.localScale = 
                new Vector2(Random.Range(smallValue - (smallValue / 4), smallValue + (smallValue / 2)), Random.Range(smallValue - (smallValue / 4), smallValue + (smallValue / 2)));
            ResourceValues.worth = Mathf.Clamp((int)value, 1, 1000);
        }

        yield return null;
    }

    Vector2 RandomVector2InsideCircle(Vector2 middle, float size)
    {
        Vector2 _position = middle;
        Vector2 _placement = new(0f,0f);
        while(Vector2.Distance(middle,_placement) >= size/2)
        {
            _placement = new Vector2(_position.x + Random.Range(0 - size/2, 0 + size/2), _position.y + Random.Range(0 - size/2, 0 + size/2));
        }
        return _placement;
    }
}
