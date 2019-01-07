using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WIP. Restore 5% hp and do other stuff
public class SkillReallocate : Skills {

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
        return "WIP";
    }

    // Use this for initialization
    void Start () {
        power = 5f;
        manaCost = 8;
	}
}