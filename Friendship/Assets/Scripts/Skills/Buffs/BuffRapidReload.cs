using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRapidReload : StatusEffect {

    LevelGenerator reference;

    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
        // Disable when this character is active
        if(characterState == (int)CharacterStates.swapIn)
            reference.GetGenerator().SetBonusBlocks(reference.GetGenerator().GetBonusBlockAmount() - 1);    // If swapping out, decrease bonus block count by 1
        else if(characterState == (int)CharacterStates.swapOut) //Re-enable. Increase bonus block count by 1
        {
            if(gameObject.GetComponent<CharacterStats>().GetCurrentStat(1) > 0)
                reference.GetGenerator().SetBonusBlocks(reference.GetGenerator().GetBonusBlockAmount() + 1);
        }
    }

    public override string BuffDescription()
    {
        return "Too late";
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
    }

    public override void TransferBuff(GameObject target)
    {
    }

    private void Awake()
    {
        stackable = false;
        buffName = "Rapid Reload";
        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
    }
}
