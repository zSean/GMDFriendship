using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMultiHit : Projectile {

    float maxHitCooldown = 1f;
    float hitCooldown;

    private void Start()
    {
        hitCooldown = maxHitCooldown;
        numHits = 1000;
    }

    public void SetHitTimer(float timer)
    {
        maxHitCooldown = timer;
    }

    protected override void Update()
    {
        base.Update();

        hitCooldown -= Time.deltaTime;

        if (hitCooldown < 0 - Time.deltaTime)
            hitCooldown = maxHitCooldown;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(hitCooldown <= 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (collision.gameObject.tag == targets[i])
                {
                    if ((instantKill) && (targets[i] != "Boss"))
                        collision.gameObject.GetComponent<CharacterStats>().SetCurrentStat(1, 0);

                    DamageCalculations.ApplyCalculation(collision.gameObject, gameObject, power, actions);
                }
            }
        }
    }
}