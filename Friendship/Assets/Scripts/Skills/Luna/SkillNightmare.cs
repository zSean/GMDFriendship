using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNightmare : Skills {

    int maxAttacks = 1;

    int[] closestEnemies;
    float[] closestDistances;
    Vector2 defaultPosition;
    LevelGenerator reference;

    public override void Activate()
    {
        // Clean this up later. Strongly consider using Kd-trees with n-nearest neighbours approach when time is available
        for (int i = 0; i < maxAttacks; i++)
        {
            closestEnemies[i] = -1;
            closestDistances[i] = Mathf.Infinity;
        }
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform playerPos = reference.GetPlayerPosition(reference.GetCurrentPlayerIndex());
        float enemyDistance = 0f;

        // Seems really inefficient, akin to bubble sort. Revisit on own time
        for (int i = 0; i < allEnemies.Length; i++)
        {
            enemyDistance = Mathf.Pow(allEnemies[i].transform.position.x - playerPos.position.x, 2) + Mathf.Pow(allEnemies[i].transform.position.y - playerPos.position.y, 2);
            for (int j = 0; j < closestEnemies.Length; j++)
            {
                if (closestEnemies[j] == -1)
                {
                    closestEnemies[j] = i;
                    closestDistances[j] = enemyDistance;
                    j = closestEnemies.Length;
                }
                else if (closestDistances[j] > enemyDistance)
                {
                    float temp = closestDistances[j];
                    closestDistances[j] = enemyDistance;
                    closestEnemies[j] = i;
                    enemyDistance = temp;
                }
            }
        }

        for(int i = 0; i < maxAttacks; i++)
        {
            if (closestEnemies[i] == -1)
                i = maxAttacks;
            else
            {
                if (allEnemies[closestEnemies[i]].GetComponent<BuffHandler>() != null)
                {
                    BuffNightmare nightmareDebuff = allEnemies[closestEnemies[i]].AddComponent<BuffNightmare>();
                    nightmareDebuff.SetParent(gameObject);
                    nightmareDebuff.SetVariation(variation);
                    nightmareDebuff.SetPower(power * gameObject.GetComponent<CharacterStats>().GetCurrentStat(0));
                    allEnemies[closestEnemies[i]].GetComponent<BuffHandler>().AddBuff(nightmareDebuff);
                }
            }
        }
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Sweet dreams";
    }

    // Use this for initialization
    void Start () {
        if (variation != 0)
            power = 0.25f;
        else
            power = 0.3f + 0.04f * level;

        if (variation == 2)
            maxAttacks = 2;

        closestEnemies = new int[maxAttacks];
        closestDistances = new float[maxAttacks];

        manaCost = 8;

        defaultPosition = Camera.main.transform.position;

        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
    }

}





