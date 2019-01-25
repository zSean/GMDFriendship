using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffApplyPoison : StatusEffect {

    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
    }

    public override string BuffDescription()
    {
        return "Apply poison";
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
        if (((targetEnemy) && ((target.tag == "Enemy") || (target.tag == "Boss"))) || ((!targetEnemy) && (target.tag == "Player")))
        {
            if (target.GetComponent<BuffHandler>() != null)
            {
                BuffPoison poison = target.AddComponent<BuffPoison>();
                poison.SetParent(target);
                poison.SetPower(power);
                target.GetComponent<BuffHandler>().AddBuff(poison);
            }
        }
    }

    // Use this for initialization
    void Start () {
        buffName = "Apply Poison";
	}
}




