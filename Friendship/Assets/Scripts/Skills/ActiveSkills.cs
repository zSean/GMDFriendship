using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkills : Skills {

    protected float maxSkillCooldown;    //Cooldown of the skill
    protected float skillCooldown;    //Current cooldown of the skill
    protected bool activeSkill = true;  //Whether the skill is active or passive

    public float GetSkillCooldown()
    {
        //Returns the current skill if on cooldown
        return skillCooldown;
    }
    public float GetMaxSkillCooldown()
    {
        //Returns the maximum cooldown of the skill
        return maxSkillCooldown;
    }
    public void SetMaxSkillCooldown(float cooldown)
    {
        //Sets the maximum cooldown of the skill to a new cooldown
        maxSkillCooldown = cooldown;

        //If the current skill cooldown is > max, will be brought down to max
        if (skillCooldown > maxSkillCooldown)
            skillCooldown = maxSkillCooldown;
    }
    public void AdjustCooldown(float adjustedAmount)
    {
        //Increments the current skill cooldown by the adjusted amount
        skillCooldown += adjustedAmount;
    }
}
