using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mod
public struct SkillProperties
{
    public SkillAssigner.SkillNames name;
    public int[] level; // Levels of all the variants
    public int variant; // 0 = Original, 1 = Variant 1, 2 = Variant 2
    public int buttonNumber;    // Button number used for UISkillTreeMenu
    public string skillImagePath;   // Used for BlockGenerator and SkillUI -> (Resources.Load(skillImage))

    public SkillProperties(SkillAssigner.SkillNames name, int buttonNumber, int[] level = null, string skillImagePath = "BlockColours", int variant = 0)
    {
        this.name = name;
        this.level = new int[level.Length];
        level.CopyTo(this.level, 0);
        this.variant = variant;
        this.buttonNumber = buttonNumber;

        if (skillImagePath != "BlockColours")
            skillImagePath = "CharacterSprites/Skills/SkillIcons/" + skillImagePath;
        this.skillImagePath = skillImagePath;
    }
}

// Handles general flow of game, contains important stats
public class GameHandler : MonoBehaviour
{

    // Something to handle cutscenes

    // Character stats
    Stats[] charStats; // Record of chracters' stats. Does not include special stats

    Dictionary<int, SkillProperties[]> skillsList = new Dictionary<int, SkillProperties[]>();   // key int = group (aerial, flurry, buff etc), value SkillProperties = skills
    Dictionary<SkillAssigner.SkillNames, SkillProperties> skillsIndex = new Dictionary<SkillAssigner.SkillNames, SkillProperties>();    // For assigning equipped skills
    SkillProperties[] equippedSkills;  // Contains the equippped skills by name only
    List<int> equippedLevelVariants;

    // float[,] specialStats;


    //Something to handle level
    LevelGenerator levelHandler;
    int level = 1;
    int gold = 0;

    // Something to handle character customization
    UIProfile ui;

    // Game state
    int gameState = 0;

    private List<SkillProperties> GroupSkills(List<SkillProperties> newSkills, List<SkillAssigner.SkillNames> skillNames, int[] variants, List<string> path)
    {
        string tempPath = "";
        for (int i = 0; i < skillNames.Count; i++)
        {
            if (i < path.Count)
                tempPath = path[i];
            else
                tempPath = null;
            SkillProperties newSkill = new SkillProperties(skillNames[i], i, variants, tempPath);
            newSkills.Add(newSkill);    // Add new skill to the groups (aerial, flurry, buff etc)
            skillsIndex[skillNames[i]] = newSkill; // Also add that skill to a dictionary so it can be looked up easily when loading skills
        }
        return newSkills;
    }

    private Dictionary<int, SkillProperties[]> GenerateSkillGroupDictionary()
    {
        // Grouping of skills into categories (Aerial, flurry, buff, perk, attack). Tuple could be used for newSkills and pathNames, or move to external file
        List<SkillProperties> newSkills = new List<SkillProperties>();  // Temporary holder for skills in each group
        // Temporary holder for string paths of each skill (for loading sprite images).
        List<string> pathNames = new List<string> { "PillarofFireIcon", "BoltIcon", "IceSpearsIcon", "HraesBeatIcon", "WingBombIcon", "NightmareIcon" };


        int[] skillVariant = { 1, -3, -3 };  // Variants for all skills except perks
        int[] skillVariant2 = { 1 };    // Perks do not have variants

        // Aerial
        List<SkillAssigner.SkillNames> addSkillGroups = new List<SkillAssigner.SkillNames> {
            SkillAssigner.SkillNames.JUDGEMENT, SkillAssigner.SkillNames.BOLT, SkillAssigner.SkillNames.ICESPEAR, SkillAssigner.SkillNames.HRAESBEAT, SkillAssigner.SkillNames.WINGBOMB, SkillAssigner.SkillNames.NIGHTMARE };

        skillsList[0] = GroupSkills(newSkills, addSkillGroups, skillVariant, pathNames).ToArray();
        addSkillGroups.Clear();
        newSkills.Clear();
        pathNames.Clear();

        // Flurry
        addSkillGroups.Add(SkillAssigner.SkillNames.PURGE);
        addSkillGroups.Add(SkillAssigner.SkillNames.ETERNALFLAME);
        addSkillGroups.Add(SkillAssigner.SkillNames.FREASTORM);
        addSkillGroups.Add(SkillAssigner.SkillNames.FEATHERDANCE);
        addSkillGroups.Add(SkillAssigner.SkillNames.SALVO);
        addSkillGroups.Add(SkillAssigner.SkillNames.KNIFETHROW);
        // Flurry, pathnames
        pathNames.Add("IgnitionIcon");
        pathNames.Add("LastingFireIcon");
        pathNames.Add("FreaStormIcon");
        pathNames.Add("FeatherDanceIcon");
        pathNames.Add("SalvoIcon");
        pathNames.Add("KnifeTossIcon");

        skillsList[1] = GroupSkills(newSkills, addSkillGroups, skillVariant, pathNames).ToArray();
        addSkillGroups.Clear();
        newSkills.Clear();
        pathNames.Clear();

        // Buff
        addSkillGroups.Add(SkillAssigner.SkillNames.HEAL);
        addSkillGroups.Add(SkillAssigner.SkillNames.WARMTH);
        addSkillGroups.Add(SkillAssigner.SkillNames.SPIRITFOX);
        addSkillGroups.Add(SkillAssigner.SkillNames.REALLOCATE);
        addSkillGroups.Add(SkillAssigner.SkillNames.FEATHERSHIELD);
        addSkillGroups.Add(SkillAssigner.SkillNames.FALLENWINGS);
        // Buff, pathnames
        pathNames.Add("RestorationIcon");
        pathNames.Add("WarmthIcon");
        pathNames.Add("SpiritFoxIcon");
        pathNames.Add("ReallocateIcon");
        pathNames.Add("FeatherShieldIcon");
        pathNames.Add("FallenWingsIcon");

        skillsList[2] = GroupSkills(newSkills, addSkillGroups, skillVariant, pathNames).ToArray();
        addSkillGroups.Clear();
        newSkills.Clear();
        pathNames.Clear();

        // Perk
        addSkillGroups.Add(SkillAssigner.SkillNames.ACTIVEHEALING);
        addSkillGroups.Add(SkillAssigner.SkillNames.GUARDIAN);
        addSkillGroups.Add(SkillAssigner.SkillNames.PURITY);
        addSkillGroups.Add(SkillAssigner.SkillNames.RAPIDRELOAD);
        addSkillGroups.Add(SkillAssigner.SkillNames.AGILITY);
        addSkillGroups.Add(SkillAssigner.SkillNames.ROOST);
        // Perk, pathnames
        pathNames.Add("ActiveHealingIcon");
        pathNames.Add("GuardianIcon");
        pathNames.Add("Purity");
        pathNames.Add("RapidReloadIcon");
        pathNames.Add("AgilityIcon");
        pathNames.Add("RoostIcon");

        skillsList[3] = GroupSkills(newSkills, addSkillGroups, skillVariant2, pathNames).ToArray();
        addSkillGroups.Clear();
        newSkills.Clear();
        pathNames.Clear();

        // Active Skills
        addSkillGroups.Add(SkillAssigner.SkillNames.FIREBALL);
        addSkillGroups.Add(SkillAssigner.SkillNames.TELEPORT);
        
        // Path elements = 0

        skillsList[4] = GroupSkills(newSkills, addSkillGroups, skillVariant2, pathNames).ToArray();
        addSkillGroups.Clear();
        newSkills.Clear();
        pathNames.Clear();

        return skillsList;
    }

    // Use this for initialization
    void Start()
    {

        // Init character stats here
        charStats = new Stats[2];
        for (int i = 0; i < 2; i++)
        {
            float[] defaultStats = { 5, 1000, 0, 0, 0, 1, 1 };//float[] defaultStats = { 1, 1, 0, 0, 0, 1, 1 };
            charStats[i].maxCharStats = defaultStats;
        }

        skillsList = GenerateSkillGroupDictionary();
        equippedLevelVariants = new List<int>();

        // Revisit this section later
        equippedSkills = new SkillProperties[14];  // 3 block skills, 1 perk, 1 auto-skill, 2 active skills x 2

        // Testing
        equippedSkills[0] = skillsIndex[SkillAssigner.SkillNames.JUDGEMENT];
        equippedSkills[1] = skillsIndex[SkillAssigner.SkillNames.PURGE];
        equippedSkills[2] = skillsIndex[SkillAssigner.SkillNames.HEAL];
        equippedSkills[3] = skillsIndex[SkillAssigner.SkillNames.ACTIVEHEALING];
        equippedSkills[4] = skillsIndex[SkillAssigner.SkillNames.FIREBALL];
        equippedSkills[5] = skillsIndex[SkillAssigner.SkillNames.FIREBALL];
        equippedSkills[6] = skillsIndex[SkillAssigner.SkillNames.FIREBALL];

        equippedSkills[7] = skillsIndex[SkillAssigner.SkillNames.HRAESBEAT];
        equippedSkills[8] = skillsIndex[SkillAssigner.SkillNames.KNIFETHROW];
        equippedSkills[9] = skillsIndex[SkillAssigner.SkillNames.REALLOCATE];
        equippedSkills[10] = skillsIndex[SkillAssigner.SkillNames.AGILITY];
        equippedSkills[11] = skillsIndex[SkillAssigner.SkillNames.FIREBALL];
        equippedSkills[12] = skillsIndex[SkillAssigner.SkillNames.TELEPORT];
        equippedSkills[13] = skillsIndex[SkillAssigner.SkillNames.FIREBALL];

        if (gameState == 0)
        {
            GameObject UIProfileObject = new GameObject();
            ui = UIProfileObject.AddComponent<UIProfile>();

            ui.Init(null, skillsList, equippedSkills);
            // GameState = -99  //Testing
        }

        if (gameState == 1)
        {
            GameObject levelHandlerObject = new GameObject();
            levelHandler = levelHandlerObject.AddComponent<LevelGenerator>();

           // levelHandler.LoadPlayers(equippedSkills, charStats, equippedLevelVariants);

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
    void Update()
    {
    }
}
