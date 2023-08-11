using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    public IEnumerator Shoot(float bulletSpeed)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.transform.parent = GameObject.FindGameObjectWithTag("BulletHolder").transform;
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
        Debug.Log("Shooting " + bulletPrefab.name + " with a velocity of " + bulletSpeed);
        yield return null;
    }
}
