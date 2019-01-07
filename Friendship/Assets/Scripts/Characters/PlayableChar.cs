using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableChar : CharacterStats {

    // BurnChance, burnDamage
    PlayerMovement control;

    int playerState = 0;    // Active, inactive etc...

    // Aerial, Flurry, Buff, Perk
    Skills[] skillEquip = new Skills[4];
    // 1 auto, 2 actives
    ActiveSkills[] activeSkillEquip = new ActiveSkills[3];

    BuffHandler buffHandler;

    // Use this for initialization
    void Start ()
    {
        control = gameObject.AddComponent<PlayerMovement>();
        buffHandler = gameObject.AddComponent<BuffHandler>();
    }

    private void Update()
    {
        playerState = GetPlayerState();
        switch(playerState)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Space))
                    activeSkillEquip[0].Activate();
                break;
            case 1:
                control.ResetJumps();
                control.enabled = false;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                playerState = 2;
                break;
            case 2:
                break;
            case 3:
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
                control.enabled = true;
                playerState = 0;
                break;
        }
    }
    public void SetPlayerState(int state)
    {
        playerState = state;
    }
    public int GetPlayerState()
    {
        return playerState;
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
}