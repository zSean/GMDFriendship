using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffGuardian : StatusEffect {

    LevelGenerator reference;

    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
        if ((parent.GetComponent<CharacterStats>().GetCurrentStat(1) > 0) && (buffTimer <= 0) && (procType == characterState))
        {
            gameObject.GetComponent<CharacterStats>().SetCurrentStat(1, 1);
            buffTimer = maxBuffTimer;
    //        reference.PlayerKnockedOut();
        }
    }

    public override string BuffDescription()
    {
        return "I will protect you";
    }

    public override void BuffDestroy()
    {
        Destroy(this);
    }

    public override void Init()
    {
    }

    public override void Stack(StatusEffect sameEffect)
    {
    }

    public override void TransferBuff(GameObject target)
    {
    }

    // Use this for initialization
    void Awake () {
        maxBuffTimer = 180;
        buffTimer = 0;

        dispellable = false;
        procType = (int)CharacterStates.dead;

        buffName = "Guardian";

        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
        buffTimer -= Time.deltaTime;
	}
}
