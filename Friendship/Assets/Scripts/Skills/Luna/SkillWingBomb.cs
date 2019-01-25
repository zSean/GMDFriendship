using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWingBomb : Skills {

    GameObject demise;
    Projectile demiseProperty;
    Sprite demiseImage;
    int maxAtk = 3;

    public override void Activate()
    {
        for(int i = 0; i < maxAtk; i++)
        {
            GameObject demiseClone = Instantiate(demise, new Vector2(Camera.main.transform.position.x - Camera.main.orthographicSize, Camera.main.transform.position.y + Camera.main.orthographicSize), transform.rotation);
            demiseProperty = demiseClone.GetComponent<Projectile>();
            demiseClone.GetComponent<SpriteRenderer>().sprite = demiseImage;
            demiseProperty.SetLifeSpan(3f);
            demiseProperty.SetNumHits(100);
            demiseProperty.SetParent(gameObject);
            demiseProperty.SetPower(power * gameObject.GetComponent<CharacterStats>().GetCurrentStat(0));
            demiseProperty.SetTrajectory(demiseProperty.GetHSpeed() * 0.85f, 0, 0.5f, -5f - 1f * i);
        }
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Your end";
    }

    // Use this for initialization
    void Start () {

        manaCost = 11;

        if (variation == 0)
            power = 1 + 0.06f * level;
        else
            power = 0.9f;
        
        if (variation == 2)
            maxAtk = 1;

        demise = (GameObject)Resources.Load("Fireball");
        demiseImage = Resources.Load("CharacterSprites/Skills/Players/Demise", typeof(Sprite)) as Sprite;
	}

}
