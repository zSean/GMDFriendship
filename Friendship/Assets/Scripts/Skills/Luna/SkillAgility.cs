using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Don't actually need a buff for this. +80% speed, +0.15 s jump duration
public class SkillAgility : Skills {

    public override void Activate()
    {
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "(Perk) Agility: Fine-tuning";
    }

    // Use this for initialization
    void Start () {
        if(gameObject.GetComponent<PlayableChar>() != null)
        {
            gameObject.GetComponent<PlayableChar>().GetControl().SethSpeed(gameObject.GetComponent<PlayableChar>().GetControl().GethSpeed() * 1.8f);
            gameObject.GetComponent<PlayableChar>().GetControl().SetJumpDuration(gameObject.GetComponent<PlayableChar>().GetControl().GetMaxJumpDuration() + 0.15f);
        }
	}
}
