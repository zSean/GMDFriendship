using UnityEngine;

// Mod
public static class SkillAssigner{

    public enum SkillNames
    {
        JUDGEMENT, PURGE, HEAL, ACTIVEHEALING, BOLT, FREASTORM, GUARDIAN, ICESPEAR, ETERNALFLAME, SPIRITFOX, PURITY, WARMTH,
        HRAESBEAT, KNIFETHROW, REALLOCATE, AGILITY, FALLENWINGS, FEATHERDANCE, FEATHERSHIELD, NIGHTMARE, RAPIDRELOAD, ROOST, SALVO, WINGBOMB,
        FIREBALL, ELEGANCE, SHOOT, TELEPORT, SIXTHSENSE, BOMBTOSS
    };

    private static Skills newSkill;
    private static ActiveSkills newActiveSkill;

    public static Skills AssignSkill(GameObject parent, SkillNames skill, int skillLevel, int variant)
    {
        switch (skill)
        {
            case SkillNames.JUDGEMENT:
                newSkill = parent.AddComponent<SkillJudgement>();
                break;
            case SkillNames.PURGE:
                newSkill = parent.AddComponent<SkillPurge>();
                break;
            case SkillNames.HEAL:
                newSkill = parent.AddComponent<SkillHeal>();
                break;
            case SkillNames.ACTIVEHEALING:
                newSkill = parent.AddComponent<SkillActiveHealing>();
                break;
            case SkillNames.BOLT:
                newSkill = parent.AddComponent<SkillBolt>();
                break;
            case SkillNames.FREASTORM:
                newSkill = parent.AddComponent<SkillFreaStorm>();
                break;
            case SkillNames.GUARDIAN:
                newSkill = parent.AddComponent<SkillGuardian>();
                break;
            case SkillNames.ICESPEAR:
                newSkill = parent.AddComponent<SkillIcicles>();
                break;
            case SkillNames.ETERNALFLAME:
                newSkill = parent.AddComponent<SkillEternalFire>();
                break;
            case SkillNames.SPIRITFOX:
                newSkill = parent.AddComponent<SkillSpiritFox>();
                break;
            case SkillNames.PURITY:
                newSkill = parent.AddComponent<SkillPurity>();
                break;
            case SkillNames.WARMTH:
                newSkill = parent.AddComponent<SkillWarmth>();
                break;
            case SkillNames.HRAESBEAT:
                newSkill = parent.AddComponent<SkillHraesBeat>();
                break;
            case SkillNames.KNIFETHROW:
                newSkill = parent.AddComponent<SkillKnifeThrow>();
                break;
            case SkillNames.REALLOCATE:
                newSkill = parent.AddComponent<SkillReallocate>();
                break;
            case SkillNames.AGILITY:
                newSkill = parent.AddComponent<SkillAgility>();
                break;
            // NOT ADDED YET!!!
            case SkillNames.FALLENWINGS:
                newSkill = parent.AddComponent<SkillWingBomb>();
                break;
            case SkillNames.FEATHERDANCE:
                newSkill = parent.AddComponent<SkillKnifeThrow>();
                break;
            ///
            case SkillNames.FEATHERSHIELD:
                newSkill = parent.AddComponent<SkillFeatherShield>();
                break;
            case SkillNames.NIGHTMARE:
                newSkill = parent.AddComponent<SkillNightmare>();
                break;
            case SkillNames.RAPIDRELOAD:
                newSkill = parent.AddComponent<SkillRapidReload>();
                break;
                // NOT ADDED YET!!!
            case SkillNames.ROOST:
                newSkill = parent.AddComponent<SkillReallocate>();
                break;
            ///
            case SkillNames.SALVO:
                newSkill = parent.AddComponent<SkillSalvo>();
                break;
            case SkillNames.WINGBOMB:
                newSkill = parent.AddComponent<SkillWingBomb>();
                break;
            default:
                newSkill = null;
                break;
        }

        newSkill?.Init(variant, skillLevel);

        return newSkill;
    }

    public static ActiveSkills AssignActiveSkill(GameObject parent, SkillNames skill, int level, int variant)
    {
        newActiveSkill = null;
        switch (skill)
        {
            case SkillNames.FIREBALL:
                newActiveSkill = parent.AddComponent<SkillFireball>();
                break;
            case SkillNames.TELEPORT:
                newActiveSkill = parent.AddComponent<SkillTeleport>();
                break;
            default:
                newActiveSkill = null;
                break;
        }

        newActiveSkill?.Init(variant, level);

        return newActiveSkill;
    }
}
