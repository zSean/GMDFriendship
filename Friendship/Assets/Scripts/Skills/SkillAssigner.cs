using UnityEngine;

public static class SkillAssigner{

    public enum SkillNames
    {
        JUDGEMENT, PURGE, HEAL, ACTIVEHEALING, BOLT, FREASTORM, GUARDIAN, ICESPEAR, ETERNALFLAME, SPIRITFOX, PURITY, WARMTH,
        HRAESBEAT, KNIFETHROW, REALLOCATE, AGILITY, FALLENWINGS, FEATHERDANCE, FEATHERSHIELD, NIGHTMARE, RAPIDRELOAD, ROOST, SALVO, WINGBOMB,
        FIREBALL, ELEGANCE, SHOOT, TELEPORT, SIXTHSENSE, BOMBTOSS
    };

    public static Skills AssignSkill(GameObject parent, SkillNames skill)
    {
        switch (skill)
        {
            case SkillNames.JUDGEMENT:
                return parent.AddComponent<SkillJudgement>();
            case SkillNames.PURGE:
                return parent.AddComponent<SkillPurge>();
            case SkillNames.HEAL:
                return parent.AddComponent<SkillHeal>();
            case SkillNames.HRAESBEAT:
                return parent.AddComponent<SkillHraesBeat>();
            case SkillNames.KNIFETHROW:
                return parent.AddComponent<SkillKnifeThrow>();
            case SkillNames.REALLOCATE:
                return parent.AddComponent<SkillReallocate>();
            case SkillNames.ACTIVEHEALING:
                return parent.AddComponent<SkillActiveHealing>();
            case SkillNames.AGILITY:
                return parent.AddComponent<SkillAgility>();
            case SkillNames.FREASTORM:
                return parent.AddComponent<SkillFreaStorm>();
            default:
                return null;
        }
    }

    public static ActiveSkills AssignActiveSkill(GameObject parent, SkillNames skill)
    {
        switch (skill)
        {
            case SkillNames.FIREBALL:
                return parent.AddComponent<SkillFireball>();
            case SkillNames.TELEPORT:
                return parent.AddComponent<SkillTeleport>();
            default:
                return null;
        }
    }
}
