using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffApplyBurn : StatusEffect {

    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
    }
    public override string BuffDescription()
    {
        return "Apply burn";
    }
    public override void BuffDestroy()
    {
        if (gameObject.GetComponent<BuffHandler>() != null)
            gameObject.GetComponent<BuffHandler>().RemoveBuff(this);
        Destroy(this);
    }
    public override void Init()
    {
    }
    public override void Stack(StatusEffect sameEffect)
    {
    }

    public override void TransferBuff(GameObject target)
    {
        if(((targetEnemy) && ((target.tag == "Enemy") || (target.tag == "Boss"))) || ((!targetEnemy) && (target.tag == "Player")))
        {
            if(target.GetComponent<BuffHandler>() != null)
            {
                BuffBurn burn = target.AddComponent<BuffBurn>();
                burn.SetPower(power);
                burn.SetParent(target);
                target.GetComponent<BuffHandler>().AddBuff(burn);
            }
        }
    }

    private void Start()
    {
        dispellable = false;
        buffName = "Apply burn";
    }
}



