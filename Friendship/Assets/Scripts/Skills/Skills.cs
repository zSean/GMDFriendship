using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified from another project
public abstract class Skills : MonoBehaviour
{
    protected float power;  //Power of the skill, whether it's dmg or healing (1.00 = 100%)
    public Sprite skillImage;

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

    //All inherited classes will need to define what their activated effect is.
    public abstract void Activate();
    //All inherited classes will need to write their own skill description.
    public abstract string SkillDescription();
    //If there is anything that needs to be done before destroying skill.
    public abstract void DestroySkill();

    /*
    //Same as above, if angles are needed
    public virtual void Activate(Vector3 direction)
    {
        Activate();
        return;
    }*/
}
