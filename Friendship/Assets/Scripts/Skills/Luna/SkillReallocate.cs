using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WIP. Restore 20% mp and do other stuff
public class SkillReallocate : Skills {

    LevelGenerator reference;

    public override void Activate()
    {
        reference.AddMana(Mathf.CeilToInt(reference.GetMaxMana() * 0.2f));
        return;
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "MP restore";
    }

    // Use this for initialization
    void Start () {
        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
        power = 5f;
        manaCost = 8;
	}
}