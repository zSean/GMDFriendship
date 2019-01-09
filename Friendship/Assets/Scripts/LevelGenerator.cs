using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For standard level stats/physics
public static class StandardLevel
{
    public static float originalSpeedModifier = 1;
    public static Vector2 sMoveSpeed = new Vector2(-10f, 0f);   // Move to the left
    public static float speedModifier = 1;  // Scale of moving
}

// Handles the action part of the game
public class LevelGenerator : MonoBehaviour {

    // Players
    PlayableChar[] player;
    int selectedPlayer = 0;

    // Level
    float maxSwapCooldown = 10f;    // 10 second cooldown before swapping
    float swapCooldown = 0f;
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
    float levelDistance = 100f;
    float slowDownRate = StandardLevel.speedModifier / 5;
    bool endGame = false;   // True once distance < 0
    GameObject boss;  // Only works with bossSpawn
    BossSpawn bossSpawner = null; // Set up a boss

    // Tracking enemies and player
    public enum EnemyNames { GOBLIN, GARGOYLE };
    List<EnemyNames> levelEnemyTypes = new List<EnemyNames>();  // For loading particular enemies onto the level
    Transform[] enemyPos;

    public void Init()
    {
        // Default values
        levelEnemyTypes.Add(EnemyNames.GOBLIN);

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

        StandardLevel.speedModifier = StandardLevel.originalSpeedModifier;
    }

    public void LoadPlayers(SkillAssigner.SkillNames[] playerSkills, Stats[] playerStats)
    {
        GameObject playerObject = (GameObject)Resources.Load("Player");
        player = new PlayableChar[playerSkills.Length / 7];
        for(int i = 0; i < player.Length; i++)
        {
            GameObject playerObjectClone = Instantiate(playerObject, new Vector3(0f, 0f), transform.rotation);
            player[i] = playerObjectClone.GetComponent<PlayableChar>();
            player[i].SetPlayerState(2);    // For init purposes. This is required
            // Assign stats
            for (int j = 0; j < playerStats[i].maxCharStats.Length; j++)
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

        // Assumption that Frea is player[0]. Need another way to address this
        player[0].GetControl().SetMaxJumps(2);

        /*
         * 
         * REMOVE
         * */
        player[0].gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("CharacterSprites/Sprite_scarecrow", typeof(Sprite)) as Sprite;
    }
    public void SetEnemies(EnemyNames[] setEnemies)
    {
        levelEnemyTypes.Clear();
        levelEnemyTypes.AddRange(setEnemies);
    }
    public void SetBoss(BossSpawn setBoss)
    {
        bossSpawner = setBoss;
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
        if (mana > maxMana)
            mana = maxMana;
    }
    public float GetPlayerStat(int characterSelect, int index)
    {
        return player[characterSelect].GetCurrentStat(index);
    }

	// Update is called once per frame
	void Update () {
        // sMoveSpeed is negative, so add a negative value. Once levelDistance <= 0, will either move to boss or end game
        levelDistance += StandardLevel.sMoveSpeed.x * StandardLevel.speedModifier * Time.deltaTime;

        if(levelDistance <= 0)
        {
            if ((boss == null) && (bossSpawner != null))
            {
                boss = bossSpawner.Init();
                bossSpawner = null;
            }
            StandardLevel.speedModifier -= (slowDownRate * Time.deltaTime);

            if(StandardLevel.speedModifier < 0)
            {
                StandardLevel.speedModifier = 0;
                slowDownRate = 0;
                endGame = true;
            }
        }

        swapCooldown -= Time.deltaTime;
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

        if (levelDistance > 0)
        {
            // Level and platform generation
            spawnTimer -= Time.deltaTime;
            pSpawnTimer -= Time.deltaTime;

            if ((spawnTimer <= 0) && (levelEnemyTypes.Count > 0))
            {
                GameObject newEnemy = new GameObject();
                newEnemy.transform.position = transform.position + Vector3.right * 3;   // Some sort of offset
                switch (levelEnemyTypes[Random.Range(0, levelEnemyTypes.Count)])
                {
                    case EnemyNames.GOBLIN:
                        newEnemy.AddComponent<Enemy>();
                        break;
                    case EnemyNames.GARGOYLE:
                        newEnemy.AddComponent<EnemyGargoyle>();
                        newEnemy.transform.position = new Vector3(newEnemy.transform.position.x, Camera.main.transform.position.y + 0.5f * Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize));
                        break;
                    default:
                        Destroy(newEnemy);
                        break;
                }
                //Instantiate(enemy, transform.position, transform.rotation);
                spawnTimer = 2f;
            }
            if (pSpawnTimer <= 0)
            {
                Instantiate(platform, new Vector3(transform.position.x, transform.position.y + Random.Range(0, 3), 0f), transform.rotation);
                pSpawnTimer = Random.Range(0.5f, 5f);
            }
        }
	}

    public void SwapPlayers(int select)
    {
        // If the player to be swapped has hp > 0 and is on standby
        if ((select != selectedPlayer) && (swapCooldown <= 0) && (player[select].GetCurrentStat(1) > 0) && (player[select].GetPlayerState() == 2))
        {
            swapCooldown = maxSwapCooldown;
            player[selectedPlayer].SetPlayerState(1);

            // Move the other player off screen (w/o animation)
            player[select].gameObject.transform.position = player[selectedPlayer].gameObject.transform.position;
            player[selectedPlayer].gameObject.transform.position = Camera.main.transform.position + Vector3.left * Camera.main.orthographicSize * 4;

            player[select].SetPlayerState(3);

            for (int i = 0; i < 3; i++)
            {
                blockGenerator.SetBlockSkill(i, player[selectedPlayer].GetBlockSkill(i));
            }

            selectedPlayer = select;
        }
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

    public void AddPoints(int add)
    {
        if(levelHUD != null)
            levelHUD.AddPoints(add);
    }
}
