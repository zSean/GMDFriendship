using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPurge : Skills {

    GameObject[] targets;
    //string targetTag = "Enemy";

    public override void Activate()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].GetComponent<CharacterStats>() != null)
                DamageCalculations.ApplyCalculation(targets[i], parent, power);
        }

        // Do the same with "Boss"
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Ignition: Explosive";
    }

    // Use this for initialization
    void Start () {
        power = 15f;
        manaCost = 8;
	}
}
