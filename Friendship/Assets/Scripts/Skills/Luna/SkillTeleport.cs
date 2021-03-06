﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTeleport : ActiveSkills {

    public override void Activate()
    {
        if(skillCooldown <= 0)
        {
            skillCooldown = maxSkillCooldown;

            // General movement horizontally and vertically, can jump multiple times depending on maxJumps
            gameObject.transform.position += Input.GetAxisRaw("Horizontal") * Vector3.right * power;
            gameObject.GetComponent<Rigidbody2D>().transform.position = gameObject.transform.position;
        }
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Nothing personal, kid";
    }

    // Use this for initialization
    void Start () {
        maxSkillCooldown = 4f;
        skillCooldown = maxSkillCooldown;
        power = 5f;
	}
	
	// Update is called once per frame
	void Update () {
        skillCooldown -= Time.deltaTime;
	}
}
