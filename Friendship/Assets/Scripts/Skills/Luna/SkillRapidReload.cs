using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRapidReload : Skills {

    public override void Activate()
    {
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Quick to the draw";
    }

    // Use this for initialization
    void Start () {
        // Add rapid reload buff to Luna only
        gameObject.GetComponent<BuffHandler>().AddBuff(gameObject.AddComponent<BuffRapidReload>());
	}
}
