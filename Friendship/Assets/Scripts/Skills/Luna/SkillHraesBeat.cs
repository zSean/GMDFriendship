using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Targets the nearest enemy to the player
public class SkillHraesBeat : Skills {

    LevelGenerator reference;

    public override void Activate()
    {
        /*
        Transform[] enemyPos = reference.GetEnemyPositions();
        Transform playerPos = reference.GetPlayerPosition(0);   // WIP

        int currentCounter = enemyPos.Length / 2;
        int min = 0;
        int max = enemyPos.Length;

        bool found = false;
 
        // Find closest target in terms of x-pos (testing). Improve on this later (look up Kd-tree etc)
        while((min < max) && (!found))
        {
            // Less than
            if (playerPos.position.x < enemyPos[currentCounter].position.x)
                min = currentCounter + 1;
            else if (playerPos.position.x > enemyPos[currentCounter].position.x)    // Greater than
                max = currentCounter - 1;
            else
                found = true;

            if (min >= max) // Keep currentCounter
                found = true;
            else if (!found)
                currentCounter = min + max / 2;
        }*/

        // Very crude
        GameObject[] enemyPositions = GameObject.FindGameObjectsWithTag("Enemy");
        Transform playerPosition = reference.GetPlayerPosition(0);
        float closestDistance = Mathf.Infinity;
        int closestEnemy = -1;

        // Targets whichever in front of the character is facing only. Ignores enemies behind
        for(int i = 0; i < enemyPositions.Length; i++)
        {
            if((enemyPositions[i].transform.position.x > playerPosition.position.x) && (Mathf.Pow(enemyPositions[i].transform.position.x - playerPosition.position.x, 2) + Mathf.Pow(enemyPositions[i].transform.position.y - playerPosition.position.y, 2) < closestDistance))
            {
                closestDistance = Mathf.Pow(enemyPositions[i].transform.position.x - playerPosition.position.x, 2) + Mathf.Pow(enemyPositions[i].transform.position.y - playerPosition.position.y, 2);
                closestEnemy = i;
            }
        }

        // Temporary
        if(closestEnemy != -1)
        {
            DamageCalculations.ApplyCalculation(enemyPositions[closestEnemy], parent, power);
            return;
        }
        else
        {
            print("No target");
        }
        return;
    }

    public override void DestroySkill()
    {
        Destroy(this);
    }

    public override string SkillDescription()
    {
        return "Hraes Beat: Mutilate";
    }

    // Use this for initialization
    void Start () {
        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
        power = 18f;
        manaCost = 12;
	}
}
