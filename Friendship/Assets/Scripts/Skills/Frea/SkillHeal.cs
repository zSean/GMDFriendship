using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WIP. 15% heal
public class SkillHeal : Skills {

    LevelGenerator reference;

    public override void Activate()
    {
        reference.GetPlayerPosition(1).gameObject.GetComponent<CharacterStats>().Attacked(-Mathf.CeilToInt(parent.GetComponent<CharacterStats>().GetMaxStat(1) * 0.15f));
        return;
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Allan, please add details";
    }

    // Use this for initialization
    void Start()
    {
        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
        power = 15f;
        manaCost = 5;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
