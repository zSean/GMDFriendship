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
    void Start () {
        control = gameObject.AddComponent<PlayerMovement>();
        buffHandler = gameObject.AddComponent<BuffHandler>();

        activeSkillEquip[0] = gameObject.AddComponent<SkillFireball>();
 
        currentCharStats = new float[maxCharStats.Length];

        for (int i = 0; i < maxCharStats.Length; i++)
        {
            currentCharStats[i] = maxCharStats[i];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            activeSkillEquip[0].Activate();
    }

    public void EquipActiveSkill(int skillIndex, ActiveSkills skill)
    {
        activeSkillEquip[skillIndex].DestroySkill();
        activeSkillEquip[skillIndex] = skill;
    }
}