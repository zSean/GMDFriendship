using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableChar : CharacterStats {

    // BurnChance, burnDamage
    PlayerMovement control;

    int playerState = 0;    // Active, inactive etc...
    int playerIndex = -1;
    float maxAutoAttackTimer = 10f;
    float autoAttackTimer;

    // Aerial, Flurry, Buff, Perk
    Skills[] skillEquip = new Skills[4];
    // 1 auto, 2 actives
    ActiveSkills[] activeSkillEquip = new ActiveSkills[3];
    LevelGenerator level;

    protected override void Awake()
    {
        float[] mCharStats = { 1, 1, 0, 0, 0, 1, 1 };
        charStats.maxCharStats = mCharStats;
        currentCharStats = new float[mCharStats.Length];

        SetMaxInvincibilityTimer(1f);

        for (int i = 0; i < mCharStats.Length; i++)
        {
            currentCharStats[i] = mCharStats[i];
        }

        buffHandler = gameObject.AddComponent<BuffHandler>();
        control = gameObject.AddComponent<PlayerMovement>();

        autoAttackTimer = maxAutoAttackTimer;
    }

    private void Update()
    {
        invincibilityTimer -= Time.deltaTime;
        playerState = GetPlayerState();
        switch(playerState)
        {
            case 0: // Active state
                if (Input.GetKeyDown(KeyCode.LeftControl))
                    activeSkillEquip[0].Activate();
                else if (Input.GetKeyDown(KeyCode.Space))
                    activeSkillEquip[1].Activate();
                else if (Input.GetKeyDown(KeyCode.LeftShift))
                    activeSkillEquip[2].Activate();
                break;
            case 1: // Getting ready to swap out (deactivation state)
                control.enabled = false;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                playerState = 2;
                break;
            case 2: // Standby. Attack when autoAttack timer < 0
                autoAttackTimer -= Time.deltaTime;
                if (autoAttackTimer < 0)
                    playerState = 4;
                break;
            case 3: // Getting ready to swap back in (init state)
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
                control.enabled = true;
                control.ResetJumps();
                playerState = 0;
                break;
            case 4: // Attack in the background
                autoAttackTimer = maxAutoAttackTimer;
                playerState = 2;
                print("Attack Launched!!!");
                break;
            case 5: // Dead
                control.enabled = false;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                playerState = 6;    // Move to stateless
                break;
            default:
                break;
        }
    }

    public float GetMaxAutoAttackTimer()
    {
        return maxAutoAttackTimer;
    }
    public float GetAutoAttackTimer()
    {
        return autoAttackTimer;
    }
    public void SetMaxAutoAttackTimer(float timer)
    {
        maxAutoAttackTimer = timer;
        autoAttackTimer = timer;
    }

    public void SetPlayerState(int state)
    {
        playerState = state;
    }
    public int GetPlayerState()
    {
        return playerState;
    }
    public PlayerMovement GetControl()
    {
        return control;
    }

    public void EquipActiveSkill(int skillIndex, ActiveSkills skill)
    {
        if(activeSkillEquip[skillIndex] != null)
            activeSkillEquip[skillIndex].DestroySkill();
        activeSkillEquip[skillIndex] = skill;
        activeSkillEquip[skillIndex].SetParent(gameObject);
    }

    public void EquipBlockSkill(int skillIndex, Skills skill)
    {
        skillEquip[skillIndex] = skill;
        skillEquip[skillIndex].SetParent(gameObject);
    }
    public Skills GetBlockSkill(int skillIndex)
    {
        return skillEquip[skillIndex];
    }

    public override void DestroyCharacter()
    {
        level.PlayerDead(playerIndex);
    }

    public void SetLevelGenerator(LevelGenerator setGenerator)
    {
        level = setGenerator;
    }

    public void SetPlayerIndex(int newIndex)
    {
        playerIndex = newIndex;
    }
    public int GetPlayerIndex()
    {
        return playerIndex;
    }
}