using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should this buff be transferred to Spirit fox?
public class BuffActiveHealingP : StatusEffect
{
    BuffActiveHealing activeHealParent;

    // Only activates on character in play
    public override void Activate(int characterState, GameObject sender, ref float power)
    {
        if ((characterState == procType) && (activeHealParent != null))
            activeHealParent.Activate(procType, parent, ref power);
        return;
    }

    public override string BuffDescription()
    {
        return "HoT counter";
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
                healing.SetActiveHealParent(activeHealParent);
                target.GetComponent<BuffHandler>().AddBuff(healing);
            }
        }
    }

    public void SetActiveHealParent(BuffActiveHealing parent)
    {
        activeHealParent = parent;
    }

    // Use this for initialization
    void Start()
    {
        procType = (int)CharacterStates.kill;
    }
}
