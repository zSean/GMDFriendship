using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculations
{
    public static void ApplyCalculation(GameObject target, GameObject sender, float power)
    {
        bool[] defaultValues = { true, true, true };
        ApplyCalculation(target, sender, power, defaultValues);
    }

    public static void ApplyCalculation(GameObject target, GameObject sender, float power, bool[] calculateModule)
    {
        if ((target != null) && (target.GetComponent<CharacterStats>() != null))
        {
            if (target.GetComponent<BuffHandler>() != null)
            {
                // OnDodge
                if (calculateModule[0])
                {
                    if (Random.Range(0, 101) < target.GetComponent<CharacterStats>().GetCurrentStat(4))
                    {
                        target.GetComponent<BuffHandler>().ActivateBuff((int)CharacterStates.dodge, sender);
                        sender.GetComponent<BuffHandler>().ActivateBuff((int)CharacterStates.dodged, target);
                        return;
                    }
                }
                // OnAttack, OnDefend
                if (calculateModule[1])
                {
                    target.GetComponent<BuffHandler>().ActivateBuff((int)CharacterStates.attacked, sender);
                    sender.GetComponent<BuffHandler>().ActivateBuff((int)CharacterStates.damaged, target);
                }
            }
            if (calculateModule[1])
            {
                target.GetComponent<CharacterStats>().Attacked((int)power);

                if (((target == null) || (target.GetComponent<CharacterStats>().GetCurrentStat(1) <= 0)) && sender.GetComponent<BuffHandler>() != null)
                    sender.GetComponent<BuffHandler>().ActivateBuff((int)CharacterStates.kill, sender);
            }
        }
    }
}
