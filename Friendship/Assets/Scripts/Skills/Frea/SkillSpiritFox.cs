using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpiritFox : Skills {

    GameObject spiritFox;
    SpiritFoxScript foxClone;
    float extendLife = 2f;
    LevelGenerator reference;

    public override void Activate()
    {
        if (!foxClone.GetActive())
        {
            foxClone.Activate(true);
            foxClone.gameObject.transform.position = reference.GetCurrentPlayerObject().transform.position + Vector3.left * 5;
        }
        foxClone.ExtendMaxLife(extendLife);
    }

    public override void DestroySkill()
    {
        //Destroy(foxClone);
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Fox on the run!";
    }

    // Use this for initialization
    void Start () {

        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();

        spiritFox = (GameObject)Resources.Load("Fox");
        foxClone = Instantiate(spiritFox).GetComponent<SpiritFoxScript>();

        if (variation == 2)
            extendLife = 4f;

        foxClone.SetLimit(5 + extendLife * 5);
    }
	
}
