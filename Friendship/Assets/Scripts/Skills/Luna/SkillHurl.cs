using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHurl : Skills {

    GameObject knife;
    Sprite knifeSprite;

    public override void Activate()
    {
        if (cooldownTimer <= 0)
        {
            //Initiate cooldown
            cooldownTimer = animationTime;

            GameObject knifeClone = Instantiate(knife, transform.position, transform.rotation);
            knifeClone.GetComponent<Projectile>().SetParent(gameObject);
            knifeClone.AddComponent<BuffHandler>();
            gameObject.GetComponent<BuffHandler>().TransferBuffs(knifeClone);
            knifeClone.GetComponent<SpriteRenderer>().sprite = knifeSprite;
            knifeClone.GetComponent<Projectile>().SetPower(power * parent.GetComponent<CharacterStats>().GetCurrentStat(0));
        }
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Lacerate";
    }

    // Use this for initialization
    void Start () {
        animationTime = 0.7f;
        knife = (GameObject)Resources.Load("Fireball");
        knifeSprite = Resources.Load("CharacterSprites/Skills/Players/Knife", typeof(Sprite)) as Sprite;
        power = 0.85f;
    }
	
	// Update is called once per frame
	void Update () {
        cooldownTimer -= Time.deltaTime;
	}
}
