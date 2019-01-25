using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBurn : StatusEffect {

    float burnTimer = 1f;

    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
    }

    public override string BuffDescription()
    {
        return power + " burn damage per second";
    }

    public override void BuffDestroy()
    {
        gameObject.GetComponent<BuffHandler>().RemoveBuff(this);
        Destroy(this);
    }

    private void Awake()
    {
        buff = false;
    }
    public override void Init()
    {
        maxBuffTimer = 5f;  // Always 5 seconds
        buffTimer = maxBuffTimer;
        stackable = false;  // Will only take the highest damage, but it does renew the timer regardless
        buffName = "Burn";
        return;
    }

    public override void Stack(StatusEffect sameEffect)
    {
        if (power < sameEffect.GetPower())
            power = sameEffect.GetPower();

        buffTimer = maxBuffTimer;
    }

    public override void TransferBuff(GameObject target)
    {
        return;
    }
	
	// Update is called once per frame
	void Update () {
        buffTimer -= Time.deltaTime;
        burnTimer -= Time.deltaTime;

        if(burnTimer < 0)
        {
            burnTimer = 1f;

            // Calculate attack, bypassing dodge and buffs/debuffs, but taking defence into account
            //parent.GetComponent<CharacterStats>().Attacked(Mathf.Max(0, (int)(power - parent.GetComponent<CharacterStats>().GetCurrentStat(2))));
            gameObject.GetComponent<CharacterStats>().Attacked(Mathf.Max(0, (int)(power - gameObject.GetComponent<CharacterStats>().GetCurrentStat(2))));
        }
        if(buffTimer <= 0)
            BuffDestroy();
	}
}
