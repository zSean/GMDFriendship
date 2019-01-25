using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSalvo : StatusEffect {

    int maxStacks = 5;
    int stackCount = 1;
    int variation = 0;
  
    public void SetVariation(int setVariation)
    {
        variation = setVariation;

        if(variation == 2)
            maxStacks = 8;
    }
    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
        if (procType == characterState)
            damagePower *= power;
    }

    public override string BuffDescription()
    {
        return "Targetted";
    }

    public override void BuffDestroy()
    {
        gameObject.GetComponent<BuffHandler>().RemoveBuff(this);
        Destroy(this);
    }

    public override void Init()
    {
        return;
    }

    public override void Stack(StatusEffect sameEffect)
    {
        power += 0.1f;

        if(stackCount < maxStacks)
            stackCount++;

        if(variation == 1)
        {
            stackCount = 0;
            power = 1;  // Damage calculation occurs without the extra damage

            if((parent != null) && (parent.GetComponent<Projectile>() != null))
                DamageCalculations.ApplyCalculation(gameObject, parent, parent.GetComponent<Projectile>().GetPower() * 3);
        }
    }

    public override void TransferBuff(GameObject target)
    {
    }

    // Use this for initialization
    void Awake () {
        buff = false;
        stackable = false;  // Technically it is stackable, but the buff handler will keep additional buffs if stackable = true

        maxBuffTimer = 5f;
        buffTimer = maxBuffTimer;
        buffName = "Salvo";
        power = 1.1f;

        procType = (int)CharacterStates.attacked;
	}
}
