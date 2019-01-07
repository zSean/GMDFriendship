using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WIP. 15% heal
public class SkillHeal : Skills {

    public override void Activate()
    {
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
        power = 15f;
        manaCost = 5;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
