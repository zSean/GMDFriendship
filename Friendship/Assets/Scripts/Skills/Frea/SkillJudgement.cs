using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillJudgement : Skills {

    GameObject[] targets;
    string targetTag = "Enemy"; // Temp. Maybe an enemy will be able to use this skill too

    private void Start()
    {
        power = 10f;
        manaCost = 15;
    }

    public override void Activate()
    {
        targets = GameObject.FindGameObjectsWithTag(targetTag);

        for(int i = 0; i < targets.Length; i++)
        {
            if (targets[i].GetComponent<CharacterStats>() != null)
                DamageCalculations.ApplyCalculation(targets[i], parent, power * Random.Range(0.01f, 4f));
        }
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Pillar of Fire: Burn down the house!";
    }
}
