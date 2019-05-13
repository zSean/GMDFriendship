using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mod
// Modified from another project
public abstract class Skills : MonoBehaviour
{
    protected GameObject parent;
    protected int level;
    protected int variation = 0; // 0 being the base, -1 and 1 are the variants
    protected float power;  //Power of the skill, whether it's dmg or healing (1.00 = 100%)
    protected int manaCost = 0; // Mana cost at 2c. For 1c, the cost may be doubled, 3c has no mana cost
    public Sprite skillImage;
    protected int animationTime = 0;    // If animation time is required before next attack can proceed

    public float GetPower()
    {
        //Returns the power of the skill
        return power;
    }
    public void SetPower(float setPower)
    {
        //Sets the power of the skill
        power = setPower;
        if (power < 0)
            power = 0;
    }

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }
    // Getter, no setter?
    public int GetManaCost()
    {
        return manaCost;
    }

    public virtual void Init(int skillVariant, int skillLevel)
    {
        variation = skillVariant;
        level = skillLevel;
        return;
    }
    public virtual void SwitchOut()
    {
        return;
    }
    //All inherited classes will need to define what their activated effect is.
    public abstract void Activate();
    //All inherited classes will need to write their own skill description.
    public abstract string SkillDescription();
    //If there is anything that needs to be done before destroying skill.
    public abstract void DestroySkill();
}
