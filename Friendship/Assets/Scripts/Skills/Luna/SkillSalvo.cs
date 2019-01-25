using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSalvo : Skills {

    int numActivations = 0;
    int numBullets = 3;
    float shootTimer = 0.08f;

    GameObject bullet;
    Sprite bulletSprite;

    LevelGenerator reference;

    public override void Activate()
    {
        numActivations++;
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Aim to kill";
    }

    // Use this for initialization
    void Start () {

        if (variation == 0)
            power = 0.4f + level * 0.04f;
        else
            power = 0.4f;

        manaCost = 5;

        animationTime = 3;

        bullet = (GameObject)Resources.Load("Fireball");
        bulletSprite = Resources.Load("CharacterSprites/Skills/Players/Bullet", typeof(Sprite)) as Sprite;

        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(numActivations > 0)
        {
            shootTimer -= Time.deltaTime;

            if(shootTimer < 0)
            {
                numBullets--;
                GameObject bulletClone = Instantiate(bullet, reference.GetPlayerPosition(reference.GetCurrentPlayerIndex()).position, transform.rotation);
                bulletClone.GetComponent<Projectile>().SetPower(power * gameObject.GetComponent<CharacterStats>().GetCurrentStat(0));
                bulletClone.GetComponent<SpriteRenderer>().sprite = bulletSprite;
                bulletClone.AddComponent<BuffHandler>();
                BuffApplySalvo debuffSalvo = bulletClone.AddComponent<BuffApplySalvo>();
                debuffSalvo.SetParent(bulletClone); // SET THE BULLET AS THE PARENT
                debuffSalvo.SetVariation(variation);
                bulletClone.GetComponent<BuffHandler>().AddBuff(debuffSalvo);

                shootTimer = 0.08f;

                if (numBullets <= 0)
                {
                    numActivations--;
                    numBullets = 3;
                }
            }
        }

	}

    public override void SwitchOut()
    {
        numActivations = 0;
        numBullets = 3;
    }
}

