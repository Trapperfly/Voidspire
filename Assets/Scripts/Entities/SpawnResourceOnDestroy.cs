using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class SpawnResourceOnDestroy : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab;
    [SerializeField] GameObject equipmentPlaceholderPrefab;
    public int amountScalar = 10;
    int amount;
    public int resourceWorth;

    
    public void SpawnLoot(float value, float explosionStrength, Vector2 position, float size, float equipmentDropChance, int level)
    {
        amount = Mathf.Clamp((int)(value * amountScalar), 1, 1000);

        for (int i = 0; i < amount; i++)
        {
            if (Random.value < equipmentDropChance)
            {
                EquipmentDrop(explosionStrength, position, size, level);
            }
            else ResourceDrop(value, explosionStrength, position, size);
        }
    }

    void ResourceDrop(float value, float explosionStrength, Vector2 position, float size)
    {
        Quaternion _rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        GameObject _resource = Instantiate(resourcePrefab, RandomVector2InsideCircle(position, size), _rotation, transform);
        Rigidbody2D _resourceRb = _resource.GetComponent<Rigidbody2D>();
        Resource _ResourceValues = _resource.GetComponent<Resource>();
        float _sv = Mathf.Clamp(Mathf.Sqrt(value) / 2, 0.4f, 1f); //sv = smallValue

        _resource.GetComponentInChildren<SpriteRenderer>().color = new UnityEngine.Color(1, Mathf.Clamp(1f - ((value * 0.1f) * Random.Range(0f, 3f)), 0f, 1f), 0);
        _resource.transform.GetChild(0).transform.localScale = new(Random.Range(_sv * 0.75f, _sv * 0.75f), Random.Range(_sv * 0.75f, _sv * 0.75f));
        _ResourceValues.worth = Mathf.Clamp((int)value, 1, 1000);
        _resourceRb.AddExplosionForce(explosionStrength, position, size, 0, ForceMode2D.Impulse);
    }

    void EquipmentDrop(float explosionStrength, Vector2 position, float size, int level)
    {
        Quaternion _rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        GameObject _equipment = Instantiate(equipmentPlaceholderPrefab, RandomVector2InsideCircle(position, size), _rotation, transform);
        Rigidbody2D _equipmentRb = _equipment.GetComponent<Rigidbody2D>();

        _equipmentRb.AddExplosionForce(explosionStrength, position, size, 0, ForceMode2D.Impulse);
        _equipment.GetComponent<ItemInfo>().level = level;
    }

    Vector2 RandomVector2InsideCircle(Vector2 middle, float radius)
    {
        Vector2 _placement = middle + (Random.insideUnitCircle * radius) / 2;
        return _placement;
    }
}