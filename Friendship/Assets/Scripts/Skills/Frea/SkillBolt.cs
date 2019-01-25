using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBolt : Skills {

    GameObject thunderBolt;
    Projectile boltProperties;
    Sprite thunder;
    RuntimeAnimatorController miniBoltAnim;
    //Vector3 defaultPosition;
    LevelGenerator reference;

    public override void Activate()
    {
        if(variation != 2)
        {
            BuffStatChange atkDown = reference.GetCurrentPlayerObject().AddComponent<BuffStatChange>();
            atkDown.SetBuffTimer(5f);
            atkDown.SetParent(reference.GetCurrentPlayerObject());
            atkDown.SetPower(0.5f);
            atkDown.SetBuffName("Attack Down");
            atkDown.AssignTargetStat(0);
            reference.GetCurrentPlayerObject().GetComponent<BuffHandler>().AddBuff(atkDown);
        }
 
        GameObject boltClone = Instantiate(thunderBolt, Vector3.zero, transform.rotation);
        boltProperties = boltClone.GetComponent<Projectile>();
        boltProperties.SetInstantKill(true);
        boltProperties.SetLifeSpan(0.3f);
        boltClone.GetComponent<SpriteRenderer>().sprite = thunder;
        reference.GetFilter().SetTrigger();
        boltProperties.SetPower(power * gameObject.GetComponent<CharacterStats>().GetCurrentStat(0));
        boltProperties.SetTrajectory(0f);
        boltProperties.SetNumHits(100);
        boltClone.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(boltClone.GetComponent<SpriteRenderer>().transform.localScale.x , 30);  // Hardcoding of y-value

        if (variation == 1)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject miniBolt = Instantiate(thunderBolt, new Vector3(Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize), 0f), transform.rotation);
                boltProperties = miniBolt.GetComponent<Projectile>();
                boltProperties.SetLifeSpan(0.3f);
                miniBolt.AddComponent<Animator>().runtimeAnimatorController = miniBoltAnim;
                boltProperties.SetPower(power * gameObject.GetComponent<CharacterStats>().GetCurrentStat(0));
                boltProperties.SetTrajectory(0f);
                boltProperties.SetNumHits(100);
                miniBolt.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(boltClone.GetComponent<SpriteRenderer>().transform.localScale.x * 2, 30);
            }
        }
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Ka-POW!!!";
    }

    // Use this for initialization
    void Start () {

        if (variation == 0)
            power = 1 + 0.1f * level;
        else
            power = 1.2f;

        thunderBolt = (GameObject)Resources.Load("Fireball");
        thunder = Resources.Load("CharacterSprites/Players/Bolt", typeof(Sprite)) as Sprite;
     //   defaultPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();

        miniBoltAnim = Resources.Load("CharacterSprites/Skills/Players/MiniBolt") as RuntimeAnimatorController;
	}
}
