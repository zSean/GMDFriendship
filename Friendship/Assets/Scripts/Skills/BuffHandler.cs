using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Taken from previous project. Own work
public class BuffHandler : MonoBehaviour {

    List<StatusEffect> statusEffects;

    private void Awake()
    {
        statusEffects = new List<StatusEffect>();
    }

    public void AddBuff(StatusEffect buff)
    {
        for (int i = 0; i < statusEffects.Count; i++)
        {
            if (buff.GetBuffName() == statusEffects[i].GetBuffName())
            {
                statusEffects[i].Stack(buff);

                if (!statusEffects[i].IsStackable())
                {
                    Destroy(buff);
                    return;
                }
            }
        }
        statusEffects.Add(buff);
        buff.Init();
    }
    public void RemoveBuff(StatusEffect buff)
    {
        statusEffects.Remove(buff);
    }

    public void TransferBuffs(GameObject target)
    {
        for (int i = 0; i < statusEffects.Count; i++)
        {
            statusEffects[i].TransferBuff(target);
        }
    }
}





