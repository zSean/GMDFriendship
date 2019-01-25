using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPoison : StatusEffect {

    float poisonTimer = 1f;

    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
    }

    public override string BuffDescription()
    {
        return power + "% hp damage per second";
    }

    public override void BuffDestroy()
    {
        gameObject.GetComponent<BuffHandler>().RemoveBuff(this);
        Destroy(this);
    }

    public override void Init()
    {
        maxBuffTimer = 10f; // Always 10 seconds
        buffTimer = maxBuffTimer;
        stackable = false;
        buffName = "Poison";

        if (power > 0.9f)
            power = 0.9f;
    }

    public override void Stack(StatusEffect sameEffect)
    {
        if (power + sameEffect.GetPower() > 0.9f)
            power = 0.9f;

        buffTimer = maxBuffTimer;
    }

    public override void TransferBuff(GameObject target)
    {
        return;
    }

    private void Awake()
    {
        buff = false;
    }

    // Update is called once per frame
    void Update () {
        buffTimer -= Time.deltaTime;
        poisonTimer -= Time.deltaTime;

        if (poisonTimer < 0)
        {
            poisonTimer = 1f;

            // Calculate attack, bypassing dodge and buffs/debuffs
            gameObject.GetComponent<CharacterStats>().Attacked(Mathf.FloorToInt(gameObject.GetComponent<CharacterStats>().GetCurrentStat(1) * power));
        }
        if (buffTimer <= 0)
            BuffDestroy();
    }
}
