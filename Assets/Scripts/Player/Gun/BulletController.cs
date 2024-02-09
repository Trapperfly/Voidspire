using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public WeaponType type;
    public bool isExplosive;
    public float explosiveDamage;
    public float damage;
    public float speed;
    public float bulletLongevity;
    public int pierce;
    public int bounce;
    public bool homing;
    public float homingStrength;
    public float punch;
    //public bool trigger;
    public float weightScalar;
    public Transform target;

    int currentID;

    public float splashRange;
    public float splashDamage;

    public void Copy(Weapon weapon)
    {
        if (weapon == null) { }
        else
        {
            if (weapon.id != currentID )
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
            }
            currentID = weapon.id;
            weightScalar = EquipmentController.Instance.weightScalar;
            type = weapon.weaponType;
            isExplosive = weapon.isExplosive;
            splashDamage = weapon.splashDamage;
            splashRange = weapon.splashRange;
            damage = weapon.damage;
            speed = weapon.speed;
            bulletLongevity = weapon.longevity;
            pierce = weapon.pierce;
            bounce = weapon.bounce;
            homing = weapon.homing;
            homingStrength = weapon.homingStrength;
            punch = weapon.punch;
        }
    }

    private void FixedUpdate()
    {
        foreach (Transform child in transform)
        {
            if (!child.TryGetComponent<Bullet>(out var _bullet))
            {
                Debug.Log("Bullet is null, Help!");
            }
            else
            {
                if (_bullet._localHoming && target != null)
                {
                    Vector2 direction = (Vector2)target.transform.position - _bullet.rb.position;
                    float rotateAmount = Vector3.Cross(direction.normalized, _bullet.transform.up).z;
                    _bullet.rb.angularVelocity = -homingStrength * rotateAmount;
                    _bullet.rb.velocity = _bullet.transform.up * speed;
                }
                else
                {
                    _bullet.transform.up = _bullet.rb.velocity;
                    _bullet.rb.angularVelocity = 0;
                }
                if(type == WeaponType.Wave)
                {
                    _bullet.transform.localScale = new Vector2(_bullet.transform.localScale.x + (bulletLongevity / 10 / 60), _bullet.transform.localScale.y);
                }
                if (_bullet.currTime <= Time.time - bulletLongevity)
                {
                    Destroy(_bullet.gameObject);
                }
            }
        }
    }
}
