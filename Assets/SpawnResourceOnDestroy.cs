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
            Quaternion _rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            GameObject _resource = Instantiate(resourcePrefab, RandomVector2InsideCircle(position, value), _rotation, transform);
            Rigidbody2D _resourceRb = _resource.GetComponent<Rigidbody2D>();
            Resource _ResourceValues = _resource.GetComponent<Resource>();
            float _sv = Mathf.Clamp(Mathf.Sqrt(value) / 2, 0.4f, 1f); //sv = smallValue

            _resource.GetComponentInChildren<SpriteRenderer>().color = new Color(1, Mathf.Clamp(1f - ((value * 0.1f) * Random.Range(0f, 3f)), 0f, 1f), 0);
            _resource.transform.GetChild(0).transform.localScale = new(Random.Range(_sv * 0.75f, _sv * 0.75f), Random.Range(_sv * 0.75f, _sv * 0.75f));
            _ResourceValues.worth = Mathf.Clamp((int)value, 1, 1000);
            _resourceRb.AddExplosionForce(value / 5, position, value, 0, ForceMode2D.Impulse);
        }

        yield return null;
    }

    Vector2 RandomVector2InsideCircle(Vector2 middle, float size)
    {
        Vector2 _placement = middle + Random.insideUnitCircle;
        return _placement;
    }
}