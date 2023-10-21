using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveShieldCollider : MonoBehaviour
{
    [SerializeField] PassiveShield master;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            master.shieldCurrent -= collision.collider.GetComponent<Bullet>()._localDamage;
            master.ShieldCheck();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bullet"))
        {
            master.shieldCurrent -= collision.GetComponent<Bullet>()._localDamage;
            master.ShieldCheck();
        }
        if (collision.CompareTag("AIBullet"))
        {
            master.shieldCurrent -= collision.GetComponent<AIBullet>().damage;
            master.ShieldCheck();
        }
    }
}
