using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGuardian : Skills {


    public override void Activate()
    {
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "I'll protect you, so don't worry ok?";
    }

    // Use this for initialization
    void Start()
    {
        GameObject[] findPlayers = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < findPlayers.Length; i++)
        {
            if ((findPlayers[i] != gameObject) && (findPlayers[i].GetComponent<BuffHandler>() != null))
            {
                BuffGuardian guardian = findPlayers[i].AddComponent<BuffGuardian>();
                guardian.SetParent(gameObject);
                findPlayers[i].GetComponent<BuffHandler>().AddBuff(guardian);
            }
        }
        return;
    }
}

