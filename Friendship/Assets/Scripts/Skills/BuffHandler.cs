using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mod
// Taken from previous project. Own work
public class BuffHandler : MonoBehaviour {

    List<StatusEffect> statusEffects;
    float resistance = -1;

    private void Awake()
    {
        statusEffects = new List<StatusEffect>();
    }

    // For any state that the object is in, trigger the buffs that apply
    public void ActivateBuff(int state, GameObject sender, ref float power)
    {
        for(int i = 0; i < statusEffects.Count; i++)
        {
            statusEffects[i].Activate(state, sender, ref power);
        }
    }
    public void SetResistChance(float setChance)
    {
        resistance = setChance;
    }
    public float GetResistChance()
    {
        return resistance;
    }
    public void AddBuff(StatusEffect buff)
    {
        for (int i = 0; i < statusEffects.Count; i++)
        {
            if (buff.GetBuffName() == statusEffects[i].GetBuffName())
            {
                statusEffects[i].Stack(buff);

                if((!statusEffects[i].IsStackable()))
                {
                    Destroy(buff);
                    return;
                }
                else if(!buff.IsBuff()) // Do resist chance calculation
                {
                    float resistChance = Random.Range(0, 1);
                    if (resistChance <= resistance)
                        Destroy(buff);
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





