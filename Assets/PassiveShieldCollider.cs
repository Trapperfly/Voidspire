using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveShieldCollider : Events
{
    [SerializeField] PassiveShield master;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            float damage = collision.collider.GetComponent<Bullet>()._localDamage;
            OnHitEvent(damage, collision.transform.position);
            master.shieldCurrent -= damage;
            master.ShieldCheck();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bullet"))
        {
            float damage = collision.GetComponent<Bullet>()._localDamage;
            OnHitEvent(damage, collision.transform.position);
            master.shieldCurrent -= damage;
            master.ShieldCheck();
        }
        if (collision.CompareTag("AIBullet"))
        {
            float damage = collision.GetComponent<AIBullet>().damage;
            OnHitEvent(damage, collision.transform.position);
            master.shieldCurrent -= damage;
            master.ShieldCheck();
        }
    }
    public override void OnHitEvent(float damage, Vector2 position)
    {
        base.OnHitEvent(damage, position);
    }
}
