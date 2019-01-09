using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireball : ActiveSkills {

    GameObject fireball;
    GameObject fireballClone;

    void Awake()
    {
        activeSkill = true; //This is an active skill
        maxSkillCooldown = 1f;
        fireball = (GameObject)Resources.Load("Fireball");
        power = 1f;
    }

    public override void Activate()
    {
        if (skillCooldown <= 0)
        {
            //Initiate cooldown
            skillCooldown = maxSkillCooldown;

            //Create fireball
            fireballClone = Instantiate(fireball, transform.position, transform.rotation);
            fireballClone.GetComponent<Projectile>().SetParent(gameObject);

            fireballClone.AddComponent<BuffHandler>();
            gameObject.GetComponent<BuffHandler>().TransferBuffs(fireballClone);

            fireballClone.GetComponent<Projectile>().SetPower(power * parent.GetComponent<CharacterStats>().GetCurrentStat(0));
        }
    }

    public override string SkillDescription()
    {
        return "Fiyah ball!";
    }
	
	// Update is called once per frame
	void Update () {
        skillCooldown -= Time.deltaTime;
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }
}
