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
    int selectedPlayer = 1;

    // Level
    float maxSwapCooldown = 5f;    // 10 second cooldown before swapping
    float swapCooldown = 0f;
    readonly int maxMana = 100;
    int mana = 0;
    readonly float maxManaRegenTimer = 0.3f;
    float manaRegenTimer = 0f;
    BlockGenerator blockGenerator;  // Need to communicate w block generator when switching char
    LevelHUD levelHUD;  // Same as above

    // Environment
    public GameObject enemy;
    public GameObject platform;
    float spawnTimer = 0f;
    float pSpawnTimer = 0f;
    float levelDistance = 2500f; // On endless, make distance = Mathf.infinity???
    float slowDownRate = StandardLevel.speedModifier / 5;
    bool endGame = false;   // True once distance < 0
    GameObject boss;  // Only works with bossSpawn
    BossSpawn bossSpawner = null; // Set up a boss

    // Filter
    ScreenFilter filter;

    // Tracking enemies and player
    public enum EnemyNames { GOBLIN, GARGOYLE };
    List<EnemyNames> levelEnemyTypes = new List<EnemyNames>() { EnemyNames.GOBLIN };  // For loading particular enemies onto the level
    Transform[] enemyPos;

    // Use a tuple instead
    Dictionary<Skills, SkillProperties> skills = new Dictionary<Skills, SkillProperties>();
    Skills ptr = null;  // Temp holder when swapping skills

    public void Init()
    {
        // Default values

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

        GameObject filterObject = (GameObject)Resources.Load("FilterObject");
        filter = filterObject.GetComponent<ScreenFilter>();

        // Testing
        pSpawnTimer = Random.Range(1f, 3f); // Spawn Platforms

        StandardLevel.speedModifier = StandardLevel.originalSpeedModifier;

        SwapPlayers(0);
    }

    public void LoadPlayers(SkillProperties[] playerSkills, Stats[] playerStats)
    {
        GameObject playerObject = (GameObject)Resources.Load("Player");
        player = new PlayableChar[playerSkills.Length / 7];
        for(int i = 0; i < player.Length; i++)
        {
            GameObject playerObjectClone = Instantiate(playerObject, new Vector3(0f, 0f), transform.rotation);
            player[i] = playerObjectClone.GetComponent<PlayableChar>();
            player[i].SetPlayerState(2);    // For init purposes. This is required
            player[i].SetLevelGenerator(this);
            player[i].SetPlayerIndex(i);

            // Assign stats
            for (int j = 0; j < playerStats[i].maxCharStats.Length; j++)
            {
                player[i].SetMaxStat(j, playerStats[i].maxCharStats[j]);
            }

            // Equip block skills
            int temp = playerSkills.Length;

            for(int j = i * temp / player.Length; j < playerSkills.Length / (player.Length - i); j++)
            {
                player[i].EquipBlockSkill(j % 7, SkillAssigner.AssignSkill(playerObjectClone, playerSkills[j].name, playerSkills[j].level[playerSkills[j].variant], playerSkills[j].variant));
                skills[player[i].GetBlockSkill(j % 7)] = playerSkills[j];
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
	void Update ()
    {
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

        // If not reached the end of the level yet, keep spawning enemies and platforms
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

                spawnTimer = 2f;
            }
            if (pSpawnTimer <= 0)
            {
                Instantiate(platform, new Vector3(transform.position.x, transform.position.y + Random.Range(0, 3), 0f), transform.rotation);
                pSpawnTimer = Random.Range(0.5f, 5f);
            }
        }
	}

    public void PlayerDead(int index)
    {
        if ((player[(index + 1) % 2].GetCurrentStat(1) > 0))
            SwapPlayers((index + 1) % 2);
        else
            print("DEAD!!!");
    }

    public void SwapPlayers(int select)
    {
        // If the player to be swapped has hp > 0 and is on standby
        if ((swapCooldown <= 0) && (player[select].GetCurrentStat(1) > 0) && (player[select].GetPlayerState() == 2))
        {
            swapCooldown = maxSwapCooldown;
            player[selectedPlayer].SetPlayerState(1);

            player[select].gameObject.transform.position = player[selectedPlayer].gameObject.transform.position;

            player[select].SetPlayerState(3);

            for (int i = 0; i < 3; i++)
            {
                ptr = player[selectedPlayer].GetBlockSkill(i);
                blockGenerator.SetBlockSkill(i, ptr, skills[ptr].skillImagePath);
                blockGenerator.UpdateBlockSprites();
            }

            selectedPlayer = select;
        }
    }

    public ScreenFilter GetFilter()
    {
        return filter;
    }

    public int GetCurrentPlayerIndex()
    {
        return selectedPlayer;
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
    public GameObject GetCurrentPlayerObject()
    {
        return player[selectedPlayer].gameObject;
    }
    public BlockGenerator GetGenerator()
    {
        return blockGenerator;
    }

    public void AddPoints(int add)
    {
        if(levelHUD != null)
            levelHUD.AddPoints(add);
    }
}
