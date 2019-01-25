using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWarmth : Skills {

    LevelGenerator reference;

    int warmthCount = 0;
    float skillAnimCooldown = 0;
    GameObject redAnimObj;
    RuntimeAnimatorController anim;

    public override void Activate()
    {
        float setRed = power;
        if (variation == 2)
        {
            warmthCount++;

            if (warmthCount >= 5)
            {
                setRed = 1;
                warmthCount = 0;
            }
        }
        else if(variation == 1)
            reference.GetGenerator().SetBlockTimer(0);

        if(skillAnimCooldown <= 0)
        {
            GameObject redAnimClone = Instantiate(redAnimObj, reference.GetCurrentPlayerObject().transform.position, transform.rotation);
            redAnimClone.transform.localScale = new Vector3(10f, 10f);
            redAnimClone.GetComponent<SkillAnimation>().Init(reference.GetCurrentPlayerObject(), 0.32f);
            redAnimClone.AddComponent<Animator>().runtimeAnimatorController = anim;
            skillAnimCooldown = 1f;
        }

        BuffDmgReduction red = reference.GetCurrentPlayerObject().AddComponent<BuffDmgReduction>();
        red.SetPower(1 - setRed);
        red.SetBuffTimer(3);
        reference.GetCurrentPlayerObject().GetComponent<BuffHandler>().AddBuff(red);
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Embrace of light";
    }

    // Use this for initialization
    void Start()
    {
        manaCost = 5;

        power = 0.2f;
        if (variation == 0)
            power = 0.2f + 0.02f * level;

        redAnimObj = (GameObject)Resources.Load("SpriteAnimation");
        anim = Resources.Load("CharacterSprites/Skills/Players/DmgRedAnim") as RuntimeAnimatorController;
        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
    }

    private void Update()
    {
        skillAnimCooldown -= Time.deltaTime;
    }
}



