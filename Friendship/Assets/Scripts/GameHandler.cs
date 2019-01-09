using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles general flow of game, contains important stats
public class GameHandler : MonoBehaviour {

    // Something to handle cutscenes

    // Character stats
    Stats[] charStats; // Record of chracters' stats. Does not include special stats
    Dictionary<SkillAssigner.SkillNames, int[]> skills;
    SkillAssigner.SkillNames[] equippedSkills;
    int currentPlayer = 1;


    // float[,] specialStats;


    //Something to handle level
    LevelGenerator levelHandler;
    int level = 1;
    int gold = 0;

    // Game state
    int gameState = 0;

	// Use this for initialization
	void Start () {

        // Init character stats here
        charStats = new Stats[2];
        for(int i = 0; i < 2; i++)
        {
            float[] defaultStats = {5, 1000, 0, 0, 0, 1, 1 };//float[] defaultStats = { 1, 1, 0, 0, 0, 1, 1 };
            charStats[i].maxCharStats = defaultStats;
        }

        // Assign skills
        skills = new Dictionary<SkillAssigner.SkillNames, int[]>();
        int[] defaultLevel = { 1, 1 };
        foreach (SkillAssigner.SkillNames name in System.Enum.GetValues(typeof(SkillAssigner.SkillNames)))
        {
            skills.Add(name, defaultLevel);
        }

        equippedSkills = new SkillAssigner.SkillNames[14];  // 3 block skills, 1 perk, 1 auto-skill, 2 active skills x 2


        // Testing
        equippedSkills[0] = SkillAssigner.SkillNames.JUDGEMENT;
        equippedSkills[1] = SkillAssigner.SkillNames.PURGE;
        equippedSkills[2] = SkillAssigner.SkillNames.HEAL;
        equippedSkills[3] = SkillAssigner.SkillNames.ACTIVEHEALING;
        equippedSkills[4] = SkillAssigner.SkillNames.FIREBALL;
        equippedSkills[5] = SkillAssigner.SkillNames.FIREBALL;
        equippedSkills[6] = SkillAssigner.SkillNames.FIREBALL;

        equippedSkills[7] = SkillAssigner.SkillNames.HRAESBEAT;
        equippedSkills[8] = SkillAssigner.SkillNames.KNIFETHROW;
        equippedSkills[9] = SkillAssigner.SkillNames.REALLOCATE;
        equippedSkills[10] = SkillAssigner.SkillNames.AGILITY;
        equippedSkills[11] = SkillAssigner.SkillNames.FIREBALL;
        equippedSkills[12] = SkillAssigner.SkillNames.TELEPORT;
        equippedSkills[13] = SkillAssigner.SkillNames.FIREBALL;

        GameObject levelHandlerObject = new GameObject();
        levelHandler = levelHandlerObject.AddComponent<LevelGenerator>();

        levelHandler.LoadPlayers(equippedSkills, charStats);
        LevelGenerator.EnemyNames[] loadEnemies = { LevelGenerator.EnemyNames.GOBLIN, LevelGenerator.EnemyNames.GARGOYLE};
        levelHandler.SetEnemies(loadEnemies);
        GameObject testBoss = new GameObject();
        levelHandler.SetBoss(testBoss.AddComponent<BossSpawn>());
        levelHandler.Init();
        levelHandler.SwapPlayers(currentPlayer);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
