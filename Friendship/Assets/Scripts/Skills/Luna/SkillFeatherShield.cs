using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFeatherShield : Skills {

    FeatherShield featherShield;
    LevelGenerator reference;

    public override void Activate()
    {
        featherShield.enabled = true;
        featherShield.Init(reference.GetCurrentPlayerObject(), power);
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Under my wing";
    }

    // Use this for initialization
    void Start () {
        manaCost = 7;

        if (variation == 0)
            power = 1.5f + 0.2f * level;
        else
            power = 1.5f;

        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();

        GameObject shieldObject = new GameObject();
        featherShield = shieldObject.AddComponent<FeatherShield>();
        featherShield.Init(reference.GetCurrentPlayerObject(), 0);
        string[] targetTags = { "eProjectile" };
        featherShield.SetTargetTags(targetTags);
	}
}
