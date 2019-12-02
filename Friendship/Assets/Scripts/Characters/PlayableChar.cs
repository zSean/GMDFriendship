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
    float animTimer = 3;
    readonly float deactivateDistance = 5000f;

    // Aerial, Flurry, Buff, Perk, 3 active skills (1 auto, 2 active)
    Skills[] skillEquip = new Skills[7];
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
        control = gameObject.GetComponent<PlayerMovement>();

        autoAttackTimer = maxAutoAttackTimer;

        Physics2D.IgnoreLayerCollision(9, 0, true);
    }

    private void Update()
    {
        invincibilityTimer -= Time.deltaTime;
        playerState = GetPlayerState();
        switch(playerState)
        {
            case 0: // Active state
                if (Input.GetKeyDown(KeyCode.LeftControl))
                    skillEquip[4].Activate();
                else if (Input.GetKeyDown(KeyCode.Space))
                    skillEquip[5].Activate();
                else if (Input.GetKeyDown(KeyCode.LeftShift))
                    skillEquip[6].Activate();
                break;
            case 1: // Getting ready to swap out (deactivation state)
                autoAttackTimer = maxAutoAttackTimer;
                gameObject.transform.position += Vector3.left * deactivateDistance;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                // Temp move into diff layer to prevent colliding with enemy
                gameObject.layer = 9;
                CharEnable(false);
                playerState = 2;
                break;
            case 2: // Standby. Attack when autoAttack timer < 0
                autoAttackTimer -= Time.deltaTime;
                if (autoAttackTimer < 0)
                {
                    gameObject.transform.position -= Vector3.left * deactivateDistance;
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    animTimer = 3f; // TEMP
                    skillEquip[6].Activate();
                    playerState = 4;
                }
                break;
            case 3: // Getting ready to swap back in (init state)
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                CharEnable(true);
                control.ResetJumps();
                gameObject.layer = 0;
                playerState = 0;
                break;
            case 4: // Auto-attack animation
                // If swapped while auto-attacking, play a new swap animation??
                animTimer -= Time.deltaTime;
                if (animTimer <= 0)
                    playerState = 1;
                break;
            case 5: // Dead
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                CharEnable(false);
                playerState = -1;    // Move to stateless
                break;
            default:
                break;
        }
    }

    void CharEnable(bool enable)
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = enable;
        gameObject.GetComponent<SpriteRenderer>().enabled = enable;
        control.enabled = enable;
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

    public void SetPlayerState(int state, int setLayer=0)
    {
        playerState = state;
        gameObject.layer = setLayer;
    }
    public int GetPlayerState()
    {
        return playerState;
    }
    public PlayerMovement GetControl()
    {
        return control;
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