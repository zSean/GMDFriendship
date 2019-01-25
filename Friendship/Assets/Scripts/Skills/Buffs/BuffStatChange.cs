using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStatChange : StatusEffect {

    int targettedStat = 0;

    public void AssignTargetStat(int targetStat)
    {
        targettedStat = targetStat;
    }

    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
    }

    public override string BuffDescription()
    {
        return "";
    }

    public override void BuffDestroy()
    {
        gameObject.GetComponent<CharacterStats>().SetCurrentStat(targettedStat, gameObject.GetComponent<CharacterStats>().GetCurrentStat(targettedStat) / power);
        gameObject.GetComponent<BuffHandler>().RemoveBuff(this);
        Destroy(this);
    }

    public override void Init()
    {
        gameObject.GetComponent<CharacterStats>().SetCurrentStat(targettedStat, gameObject.GetComponent<CharacterStats>().GetCurrentStat(targettedStat) * power);
    }

    public override void Stack(StatusEffect sameEffect)
    {
        return;
    }

    public override void TransferBuff(GameObject target)
    {
        return;
    }


    // Dirty way of classifying it as a buff for the buff handler
    public override void SetPower(float power)
    {
        if (power < 0)
            power = 1;
        if (power < 1)
            buff = false;
        this.power = power;
    }

    // Use this for initialization
    void Start () {
        buffTimer = maxBuffTimer;
        stackable = true;
	}
	
	// Update is called once per frame
	void Update () {
        buffTimer -= Time.deltaTime;

        if (buffTimer <= 0)
            BuffDestroy();
	}
}

