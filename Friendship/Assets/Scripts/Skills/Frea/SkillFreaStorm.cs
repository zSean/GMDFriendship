using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFreaStorm : Skills {

    GameObject FreaClone;
    int maxClones = 4;
    int activationCount = 0;
    float summonTimer = 0f;
    int summClones;

    public override void Activate()
    {
        activationCount++;
        return;
    }

    private void Update()
    {
        if(activationCount > 0)
        {
            summonTimer -= Time.deltaTime;
            if(summonTimer < 0)
            {
                summClones--;
                GameObject FreaCloneObject = Instantiate(FreaClone, new Vector3(Camera.main.transform.position.x - Camera.main.orthographicSize * 2, Camera.main.transform.position.y - Camera.main.orthographicSize /2), transform.rotation);
                FreaCloneObject.GetComponent<Projectile>().SetNumHits(100);
                FreaCloneObject.GetComponent<Projectile>().SetPower(power);
                // No buff handler???
                FreaCloneObject.GetComponent<Projectile>().SetTrajectory(FreaCloneObject.GetComponent<Projectile>().GetHSpeed() * Random.Range(0.8f, 1f));
                summonTimer = 0.1f;

                if(summClones < 0)
                {
                    summClones = maxClones;
                    activationCount--;
                }
            }
        }
    }

    public override void DestroySkill()
    {
        Destroy(this);
        return;
    }

    public override string SkillDescription()
    {
        return "Riot!";
    }

    private void Start()
    {
        if (variation == 1)
            maxClones = 6;
        summClones = maxClones;
        power = 12f;
        manaCost = 9;
        FreaClone = (GameObject)Resources.Load("Fireball");
    }
}

