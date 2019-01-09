using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillKnifeThrow : Skills {

    GameObject knifeObject;
    Projectile knifeProperties;
    float[] vSpeed = { 0f, 0f, 0f };

    public override void Activate()
    {
        GameObject knifeObjectClone = Instantiate(knifeObject, new Vector3(Camera.main.transform.position.x - Camera.main.orthographicSize * 2, transform.position.y), transform.rotation);
        knifeProperties = knifeObjectClone.GetComponent<Projectile>();
        knifeObjectClone.AddComponent<BuffHandler>();
        gameObject.GetComponent<BuffHandler>().TransferBuffs(knifeObjectClone);
        knifeProperties.SetPower(power);
        knifeProperties.SetTrajectory(knifeProperties.GetHSpeed() * 2.5f, vSpeed);
        return;
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Knife throw: Lacerate";
    }

    // Use this for initialization
    void Start()
    {
        power = 12f;
        manaCost = 9;
        knifeObject = (GameObject)Resources.Load("Fireball");
    }
}
