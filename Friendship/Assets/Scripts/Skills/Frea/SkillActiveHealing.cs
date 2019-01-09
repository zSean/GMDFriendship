using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Perk will not keep track of characters given buff?
public class SkillActiveHealing : Skills {

    public override void Activate()
    {
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "(Perk) Active healing: Fighting spirit!";
    }

    // Use this for initialization
    void Start () {
        GameObject[] findPlayers = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < findPlayers.Length; i++)
        {
            if (findPlayers[i].GetComponent<BuffHandler>() != null)
            {
                BuffActiveHealing activeHealing = findPlayers[i].AddComponent<BuffActiveHealing>();
                activeHealing.SetParent(findPlayers[i]);
                findPlayers[i].GetComponent<BuffHandler>().AddBuff(activeHealing);
            }
        }
        return;
    }
}
