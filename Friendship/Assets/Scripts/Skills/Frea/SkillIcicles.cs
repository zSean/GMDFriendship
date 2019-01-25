using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIcicles : Skills {

    GameObject icicle;
    int maxIcicles = 4;
    int icicleCount;
    int activationCount = 0;
    float summonTimer = 0f;
    Sprite icicleSprite;
    float piAngle;

    GameObject enemy;
    Vector2 defaultPosition;
    Vector2 lastEnemyPosition;
    LevelGenerator reference;

    public override void Activate()
    {
        if (activationCount <= 0)
            enemy = FindClosestEnemy();
        activationCount++;
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Ice cold!";
    }

    // Use this for initialization
    void Start () {
        icicleCount = maxIcicles;

        if (variation != 0)
            power = 0.25f;
        else
            power = 0.3f + 0.04f * level;

        if (variation == 2)
            maxIcicles = 8;

        manaCost = 12;

        icicle = (GameObject)Resources.Load("Fireball");
        icicleSprite = Resources.Load("CharacterSprites/Skills/Players/Icicle", typeof(Sprite)) as Sprite;

        defaultPosition = Camera.main.transform.position;
        lastEnemyPosition = defaultPosition;

        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (activationCount > 0)
        {
            if (enemy != null)
                lastEnemyPosition = enemy.transform.position;

            summonTimer -= Time.deltaTime;
            if (summonTimer < 0)
            {
                icicleCount--;

                piAngle = Mathf.PI / 2 * (maxIcicles - icicleCount) - (Mathf.PI / 4 * ((maxIcicles - icicleCount - 1) / 4));
      
                // Note that sign(0) in unity will give a positive num. Also, piAngle won't be exactly 0. Unintended offset, but it looks nice enough to keep
                GameObject icicleObject = Instantiate(icicle, lastEnemyPosition + Vector2.up * Mathf.Sign(Mathf.Sin(piAngle)) + Vector2.right * Mathf.Sign(Mathf.Cos(piAngle)), Quaternion.AngleAxis(-piAngle * Mathf.Rad2Deg, Vector3.back));
                icicleObject.GetComponent<SpriteRenderer>().sprite = icicleSprite;
                icicleObject.GetComponent<Projectile>().SetNumHits(3);
                icicleObject.GetComponent<Projectile>().SetPower(gameObject.GetComponent<CharacterStats>().GetCurrentStat(0) * power);
                icicleObject.GetComponent<Projectile>().SetParent(gameObject);

                icicleObject.GetComponent<Projectile>().SetTrajectory(-Mathf.Cos(piAngle) * icicleObject.GetComponent<Projectile>().GetHSpeed() / 2, -Mathf.Sin(piAngle) * icicleObject.GetComponent<Projectile>().GetHSpeed() / 2);

                // Was planning to add a delay between each individual icicle, but it looks more visually appealing to have them all at once
                // Change into a simple loop later on. May want to keep this around for a bit
                summonTimer = 0.00f;

                if(icicleCount <= 0)
                {
                    icicleCount = maxIcicles;
                    activationCount--;

                    if (activationCount > 0)
                        enemy = FindClosestEnemy();
                }
            }
        }

	}

    // Clean this up later
    GameObject FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        int closestEnemy = -1;
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform playerPos = reference.GetPlayerPosition(reference.GetCurrentPlayerIndex());   // Fix this later

        // Targets whichever in front of the character is facing only. Ignores enemies behind
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (Mathf.Pow(allEnemies[i].transform.position.x - playerPos.position.x, 2) + Mathf.Pow(allEnemies[i].transform.position.y - playerPos.position.y, 2) < closestDistance)
            {
                closestDistance = Mathf.Pow(allEnemies[i].transform.position.x - playerPos.position.x, 2) + Mathf.Pow(allEnemies[i].transform.position.y - playerPos.position.y, 2);
                closestEnemy = i;
            }
        }

        if ((closestEnemy != -1) && (allEnemies[closestEnemy].transform.position.x > Camera.main.transform.position.x - Camera.main.orthographicSize * 2))
            return allEnemies[closestEnemy];
        else
        {
            lastEnemyPosition = defaultPosition;
            return null;
        }
    }
}


