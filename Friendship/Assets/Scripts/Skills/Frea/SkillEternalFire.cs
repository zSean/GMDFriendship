using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEternalFire : Skills {

    GameObject fire;
    ProjectileMultiHit fireProperties;
    RuntimeAnimatorController anim;

    LevelGenerator reference;

    public override void Activate()
    {
        GameObject fireballClone = Instantiate(fire, reference.GetPlayerPosition(reference.GetCurrentPlayerIndex()).position + Vector3.right * 3f, transform.rotation);
        fireballClone.AddComponent<Animator>().runtimeAnimatorController = anim;
        fireballClone.transform.localScale *= 1.5f;
        fireProperties = fireballClone.GetComponent<ProjectileMultiHit>();
        fireProperties.SetLifeSpan(8f);
        fireProperties.SetPower(power * gameObject.GetComponent<CharacterStats>().GetCurrentStat(0));
        fireProperties.SetTrajectory(0f);
        fireProperties.SetHitTimer(0.03f);
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Remnants of the past";
    }

    // Use this for initialization
    void Start () {
        manaCost = 10;

        power = 0.35f;
        if (variation == 0)
            power = 0.25f + 0.05f * level;

        fire = (GameObject)Resources.Load("MultiHitter");
        anim = Resources.Load("CharacterSprites/Skills/Players/EternalFlameAnimation") as RuntimeAnimatorController;

        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
	}
}
