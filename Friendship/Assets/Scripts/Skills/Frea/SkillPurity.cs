using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPurity : Skills {
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
        return "To err is...to forgive, divine";
    }

    private void Start()
    {
        gameObject.GetComponent<BuffHandler>().SetResistChance(1);
    }
}
