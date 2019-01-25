using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SkillProperties
{
    public SkillAssigner.SkillNames name;
    public int level;
    public int variant;
}

// Handles general flow of game, contains important stats
public class GameHandler : MonoBehaviour {

    // Something to handle cutscenes

    // Character stats
    Stats[] charStats; // Record of chracters' stats. Does not include special stats

    // REVISIT
    static Dictionary<int, SkillProperties[]> skillGroup = new Dictionary<int, SkillProperties[]>(); // Dictionary containing category that each skillName belongs to
    SkillProperties[] equippedSkills;  // Contains the equipped skills struct
    int[] equippedSkillsIndex;  // Contains the index for the equipped skills


    // float[,] specialStats;


    //Something to handle level
    LevelGenerator levelHandler;
    int level = 1;
    int gold = 0;

    // Something to handle character customization
    UIProfile ui;

    // Game state
    int gameState = 1;



    private void GenerateSkillGroupDictionary()
    {
        // Assigning skill stats and further grouping of skills into categories (Aerial, flurry, buff, perk, attack)

        // Create a list of skill names only first and create SkillProperties for each element in the list
        List<SkillAssigner.SkillNames[]> tempSkillGroups = new List<SkillAssigner.SkillNames[]>();

        // Aerial
        List<SkillAssigner.SkillNames> addNames = new List<SkillAssigner.SkillNames> {
            SkillAssigner.SkillNames.JUDGEMENT, SkillAssigner.SkillNames.BOLT, SkillAssigner.SkillNames.ICESPEAR, SkillAssigner.SkillNames.HRAESBEAT, SkillAssigner.SkillNames.WINGBOMB, SkillAssigner.SkillNames.NIGHTMARE };
        tempSkillGroups.Add(addNames.ToArray());
        addNames.Clear();

        // Flurry
        addNames.Add(SkillAssigner.SkillNames.PURGE);
        addNames.Add(SkillAssigner.SkillNames.ETERNALFLAME);
        addNames.Add(SkillAssigner.SkillNames.FREASTORM);
        addNames.Add(SkillAssigner.SkillNames.FEATHERDANCE);
        addNames.Add(SkillAssigner.SkillNames.KNIFETHROW);
        addNames.Add(SkillAssigner.SkillNames.SALVO);
        tempSkillGroups.Add(addNames.ToArray());
        addNames.Clear();

        // Buff
        addNames.Add(SkillAssigner.SkillNames.HEAL);
        addNames.Add(SkillAssigner.SkillNames.WARMTH);
        addNames.Add(SkillAssigner.SkillNames.SPIRITFOX);
        addNames.Add(SkillAssigner.SkillNames.REALLOCATE);
        addNames.Add(SkillAssigner.SkillNames.FEATHERSHIELD);
        addNames.Add(SkillAssigner.SkillNames.FALLENWINGS);
        tempSkillGroups.Add(addNames.ToArray());
        addNames.Clear();

        // Perk
        addNames.Add(SkillAssigner.SkillNames.ACTIVEHEALING);
        addNames.Add(SkillAssigner.SkillNames.GUARDIAN);
        addNames.Add(SkillAssigner.SkillNames.PURITY);
        addNames.Add(SkillAssigner.SkillNames.RAPIDRELOAD);
        addNames.Add(SkillAssigner.SkillNames.AGILITY);
        addNames.Add(SkillAssigner.SkillNames.ROOST);
        tempSkillGroups.Add(addNames.ToArray());
        addNames.Clear();

        // Active skills
        addNames.Add(SkillAssigner.SkillNames.FIREBALL);
        addNames.Add(SkillAssigner.SkillNames.ELEGANCE);
        addNames.Add(SkillAssigner.SkillNames.SHOOT);
        addNames.Add(SkillAssigner.SkillNames.TELEPORT);
        addNames.Add(SkillAssigner.SkillNames.SIXTHSENSE);
        addNames.Add(SkillAssigner.SkillNames.BOMBTOSS);
        tempSkillGroups.Add(addNames.ToArray());
        addNames.Clear();

        // Actual skill assignment and building of the dictionary
        for (int i = 0; i < tempSkillGroups.Count; i++)
        {
            /*
             * 
             * 
             * 
             * 
             *  TEMP ASSIGNMENT. Once saving and loading is up, will need to change this section
             *  
             *  
             *  
             *  
             *  */
            int defaultLevel = 1;

            List<SkillProperties> addSkillProperties = new List<SkillProperties>();
            for (int j = 0; j < tempSkillGroups[i].Length; j++)
            {
                SkillProperties newSkill = new SkillProperties
                {
                    name = tempSkillGroups[i][j],
                    variant = 0,
                    level = defaultLevel
                };

                addSkillProperties.Add(newSkill);
                // The bit with hardcoding. All skill groups except perks (**ASSUMED TO BE j = 3**) will have 2 variants
                if (i != 3)
                {
                    for(int k = 1; k < 3; k++)
                    {
                        SkillProperties newVariant = new SkillProperties();
                        newVariant.name = tempSkillGroups[i][j];
                        newVariant.variant = k;
                        newVariant.level = -3; //defaultLevel;
                        addSkillProperties.Add(newVariant);
                    }
                }
            }
            skillGroup[i] = addSkillProperties.ToArray();
            addSkillProperties.Clear();
        }
        return;
    }

    // Testing
    private SkillProperties FindEquippedSkill(int index, SkillAssigner.SkillNames findName)
    {
        for(int i = 0; i < skillGroup[index].Length; i++)
        {
            if (skillGroup[index][i].name == findName)
                return skillGroup[index][i];
        }

        return new SkillProperties();
    }

    // Use this for initialization
	void Start () {

        // Init character stats here
        charStats = new Stats[2];
        for(int i = 0; i < 2; i++)
        {
            float[] defaultStats = {5, 1000, 0, 0, 0, 1, 1 };//float[] defaultStats = { 1, 1, 0, 0, 0, 1, 1 };
            charStats[i].maxCharStats = defaultStats;
        }

        GenerateSkillGroupDictionary();

        equippedSkills = new SkillProperties[14];
        equippedSkillsIndex = new int[14];

        for(int i = 0; i < equippedSkillsIndex.Length; i++)
        {
            equippedSkillsIndex[i] = 0;
        }

        if (gameState == 0)
        {
            GameObject UIProfileObject = new GameObject();
            ui = UIProfileObject.AddComponent<UIProfile>();

            ui.Init(null, skillGroup, equippedSkillsIndex);
            // GameState = -99  //Testing
        }

        // Testing
        for(int i = 0; i < 14; i++)
        {
            equippedSkillsIndex[i] = 1;
        }

        if (gameState == 1)
        {
            // Revisit this section later
            // 3 block skills, 1 perk, 1 auto-skill, 2 active skills x 2

            // Testing
            equippedSkills[0] = FindEquippedSkill(0, SkillAssigner.SkillNames.JUDGEMENT);
            equippedSkills[1] = FindEquippedSkill(1, SkillAssigner.SkillNames.FREASTORM);
            equippedSkills[2] = FindEquippedSkill(2, SkillAssigner.SkillNames.HEAL);
            equippedSkills[3] = FindEquippedSkill(3, SkillAssigner.SkillNames.ACTIVEHEALING);
            equippedSkills[4] = FindEquippedSkill(4, SkillAssigner.SkillNames.FIREBALL);
            equippedSkills[5] = FindEquippedSkill(4, SkillAssigner.SkillNames.FIREBALL);
            equippedSkills[6] = FindEquippedSkill(4, SkillAssigner.SkillNames.FIREBALL);

            equippedSkills[7] = FindEquippedSkill(0, SkillAssigner.SkillNames.HRAESBEAT);
            equippedSkills[8] = FindEquippedSkill(1, SkillAssigner.SkillNames.KNIFETHROW);
            equippedSkills[9] = FindEquippedSkill(2, SkillAssigner.SkillNames.REALLOCATE);
            equippedSkills[10] = FindEquippedSkill(3, SkillAssigner.SkillNames.AGILITY);
            equippedSkills[11] = FindEquippedSkill(4, SkillAssigner.SkillNames.TELEPORT);
            equippedSkills[12] = FindEquippedSkill(4, SkillAssigner.SkillNames.FIREBALL);
            equippedSkills[13] = FindEquippedSkill(4, SkillAssigner.SkillNames.FIREBALL);


            GameObject levelHandlerObject = new GameObject();
            levelHandler = levelHandlerObject.AddComponent<LevelGenerator>();

            levelHandler.LoadPlayers(equippedSkills, charStats);
            LevelGenerator.EnemyNames[] loadEnemies = { LevelGenerator.EnemyNames.GOBLIN, LevelGenerator.EnemyNames.GARGOYLE };
            levelHandler.SetEnemies(loadEnemies);
            GameObject testBoss = new GameObject();
            levelHandler.SetBoss(testBoss.AddComponent<BossSpawn>());
            levelHandler.Init();
            //gameState = -99;            // Standby
        }
    }
	
	// Update is called once per frame
	void Update () {
	}
}
