using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Due to how similar this is to Salvo, perhaps combine it into one class later
public class BuffDmgReduction : StatusEffect {

    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
        if (procType == characterState)
            damagePower *= power;
    }

    public override string BuffDescription()
    {
        return "Damage reduction";
    }

    public override void BuffDestroy()
    {
        gameObject.GetComponent<BuffHandler>().RemoveBuff(this);
        Destroy(this);
    }

    public override void Init()
    {
    }

    public override void Stack(StatusEffect sameEffect)
    {
        if (sameEffect.GetPower() > power)
        {
            power = sameEffect.GetPower();
            buffTimer = sameEffect.GetBuffTimer();
        }
        else
        {
            buffTimer += sameEffect.GetBuffTimer();

            if (buffTimer > maxBuffTimer)
                buffTimer = maxBuffTimer;
        }
    }

    public override void TransferBuff(GameObject target)
    {
    }

    // Use this for initialization
    void Awake () {
        buffName = "Damage Reduction";
        procType = (int)CharacterStates.attacked;
	}
	
	// Update is called once per frame
	void Update () {
        buffTimer -= Time.deltaTime;

        if(buffTimer <= 0)
            BuffDestroy();
	}
}
