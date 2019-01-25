using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffApplySalvo : StatusEffect {

    int variation = 0;

    public void SetVariation(int setVariation)
    {
        variation = setVariation;
    }
    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
        return;
    }

    public override string BuffDescription()
    {
        return "Apply salvo";
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
        return;
    }

    public override void TransferBuff(GameObject target)
    {
        // On the assumption that this cannot be transferred to other projectiles. Maybe it will later on
        if ((targetEnemy) && ((target.tag == "Enemy") || (target.tag == "Boss")))
        {
            if (target.GetComponent<BuffHandler>() != null)
            {
                BuffSalvo salvo = target.AddComponent<BuffSalvo>();
                salvo.SetParent(parent);
                salvo.SetBuffName("Salvo");
                target.GetComponent<BuffHandler>().AddBuff(salvo);
            }
        }
    }

    // Use this for initialization
    void Start () {
        buffName = "Apply Salvo";
	}
	
}

