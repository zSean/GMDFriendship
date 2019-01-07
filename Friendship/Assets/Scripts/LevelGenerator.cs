using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the action part of the game
public class LevelGenerator : MonoBehaviour {

    // Player
    PlayableChar[] player;
    int selectedPlayer = 0;
    int maxMana = 100;
    int mana = 0;
    float maxManaRegenTimer = 0.3f;
    float manaRegenTimer = 0f;
    BlockGenerator blockGenerator;  // Need to communicate w block generator when switching char
    LevelHUD levelHUD;  // Same as above

    // Environment
    public GameObject enemy;
    public GameObject platform;
    float spawnTimer = 0f;
    float pSpawnTimer = 0f;

    // Tracking enemies and player
    Transform[] enemyPos;

    public void Init()
    {
        enemy = (GameObject)Resources.Load("EnemyPrototype");
        platform = (GameObject)Resources.Load("Platform");

        tag = "LevelHandler";
        transform.position = new Vector3(Camera.main.transform.position.x + 2 * Camera.main.orthographicSize, Camera.main.transform.position.y - Camera.main.orthographicSize / 2);

        // Init the generator and HUD
        GameObject blockGeneratorObject = new GameObject();
        blockGenerator = blockGeneratorObject.AddComponent<BlockGenerator>();
        blockGenerator.Init();
        blockGeneratorObject.transform.position = new Vector3(Camera.main.transform.position.x - Camera.main.orthographicSize, Camera.main.transform.position.y - Camera.main.orthographicSize);
        blockGenerator.SetLevelGenerator(this);

        GameObject levelHUDObject = new GameObject();
        levelHUD = levelHUDObject.AddComponent<LevelHUD>();
        levelHUD.SetLevelGenerator(this);

        manaRegenTimer = maxManaRegenTimer;

        // Testing
        pSpawnTimer = Random.Range(1f, 3f); // Spawn Platforms
    }

    public void LoadPlayers(SkillAssigner.SkillNames[] playerSkills, Stats[] playerStats)
    {
        GameObject playerObject = (GameObject)Resources.Load("Player");
        player = new PlayableChar[playerSkills.Length / 7];
        for(int i = 0; i < player.Length; i++)
        {
            GameObject playerObjectClone = Instantiate(playerObject, new Vector3(0f, 0f), transform.rotation);
            player[i] = playerObjectClone.GetComponent<PlayableChar>();
 
            // Assign stats
            for(int j = 0; j < playerStats[i].maxCharStats.Length; j++)
            {
                player[i].SetMaxStat(j, playerStats[i].maxCharStats[j]);
            }

            // Equip block skills
            for(int j = i * playerSkills.Length / player.Length; j < playerSkills.Length / (player.Length - i); j++)
            {
                if(j % 7 < 4)
                    player[i].EquipBlockSkill(j % 7, SkillAssigner.AssignSkill(playerObjectClone, playerSkills[j]));
                else
                    player[i].EquipActiveSkill(j % 7 - 4, SkillAssigner.AssignActiveSkill(playerObjectClone, playerSkills[j]));
            }
        }


        /*
         * 
         * REMOVE
         * */
        player[0].gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite_scarecrow", typeof(Sprite)) as Sprite;
    }
    public int GetMana()
    {
        return mana;
    }
    public int GetMaxMana()
    {
        return maxMana;
    }
    public void AddMana(int amount)
    {
        mana += amount;
    }
    public float GetPlayerStat(int characterSelect, int index)
    {
        return player[characterSelect].GetCurrentStat(index);
    }

	// Update is called once per frame
	void Update () {
        if (mana < maxMana)
        {
            manaRegenTimer -= Time.deltaTime;
            if (manaRegenTimer <= 0)
            {
                mana++;
                manaRegenTimer = maxManaRegenTimer;
            }
        }
        else
            manaRegenTimer = maxManaRegenTimer;

        // Check if switching players
        if (Input.GetKeyDown(KeyCode.Q))
            SwapPlayers((selectedPlayer + 1) % 2);

        spawnTimer -= Time.deltaTime;
        pSpawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            Instantiate(enemy, transform.position, transform.rotation);
            spawnTimer = 2f;
        }
        if (pSpawnTimer <= 0)
        {
            Instantiate(platform, new Vector3(transform.position.x, transform.position.y + Random.Range(0, 3), 0f), transform.rotation);
            pSpawnTimer = Random.Range(0.5f, 5f);
        }
	}

    public void SwapPlayers(int select)
    {
        /*
         * 
         * Check if other player is able to swap first
         * 
         * 
         * */

        player[selectedPlayer].SetPlayerState(1);
        player[select].SetPlayerState(3);

        for (int i = 0; i < 3; i++)
        {
            blockGenerator.SetBlockSkill(i, player[selectedPlayer].GetBlockSkill(i));
        }

        selectedPlayer = select;
    }

    // Need transform component
    public Transform[] GetEnemyPositions()
    {
        return enemyPos;
    }

    public Transform GetPlayerPosition(int selectedPlayer)
    {
        return player[selectedPlayer].gameObject.transform;
    }
}
