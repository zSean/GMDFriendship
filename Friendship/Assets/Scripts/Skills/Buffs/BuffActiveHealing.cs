using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should this buff be transferred to Spirit fox?
public class BuffActiveHealing : StatusEffect {

    int maxKillCounter = 5;
    int killCounter;

    // Only activates on character in play
    public override void Activate(int characterState, GameObject sender)
    {
        if(characterState == procType)
        {
            killCounter--;
            if ((killCounter <= 0) && (parent != null))
            {
                killCounter = maxKillCounter;
                parent.GetComponent<CharacterStats>().Attacked(-Mathf.CeilToInt(parent.GetComponent<CharacterStats>().GetMaxStat(1) * 0.02f));
                //parent.GetComponent<CharacterStats>().AddCurrentStat(1, gameObject.GetComponent<CharacterStats>().GetMaxStat(1) * 0.02f);
            }
        }
        return;
    }

    public override string buffDescription()
    {
        return "HoT";
    }

    public override void buffDestroy()
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

    // Revisit later
    public override void TransferBuff(GameObject target)
    {
        if (target.tag == "Projectile")
        {
            // Attach buff to projectile
            if (target.GetComponent<BuffHandler>() != null)
            {
                BuffActiveHealingP healing = target.AddComponent<BuffActiveHealingP>();
                healing.SetParent(parent);
                healing.SetActiveHealParent(this);
                target.GetComponent<BuffHandler>().AddBuff(healing);
            }
        }
    }

    // Use this for initialization
    void Start () {
        killCounter = maxKillCounter;
        procType = (int)CharacterStates.kill;
	}
}
