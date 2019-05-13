using UnityEngine;

// Mod
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
            case SkillNames.ACTIVEHEALING:
                return parent.AddComponent<SkillActiveHealing>();
            case SkillNames.BOLT:
                return parent.AddComponent<SkillBolt>();
            case SkillNames.FREASTORM:
                return parent.AddComponent<SkillFreaStorm>();
            case SkillNames.GUARDIAN:
                return parent.AddComponent<SkillGuardian>();
            case SkillNames.ICESPEAR:
                return parent.AddComponent<SkillIcicles>();
            case SkillNames.ETERNALFLAME:
                return parent.AddComponent<SkillEternalFire>();
            case SkillNames.SPIRITFOX:
                return parent.AddComponent<SkillSpiritFox>();
            case SkillNames.PURITY:
                return parent.AddComponent<SkillPurity>();
            case SkillNames.WARMTH:
                return parent.AddComponent<SkillWarmth>();
            case SkillNames.HRAESBEAT:
                return parent.AddComponent<SkillHraesBeat>();
            case SkillNames.KNIFETHROW:
                return parent.AddComponent<SkillKnifeThrow>();
            case SkillNames.REALLOCATE:
                return parent.AddComponent<SkillReallocate>();
            case SkillNames.AGILITY:
                return parent.AddComponent<SkillAgility>();
            // NOT ADDED YET!!!
            case SkillNames.FALLENWINGS:
                return parent.AddComponent<SkillWingBomb>();
            case SkillNames.FEATHERDANCE:
                return parent.AddComponent<SkillKnifeThrow>();
            ///
            case SkillNames.FEATHERSHIELD:
                return parent.AddComponent<SkillFeatherShield>();
            case SkillNames.NIGHTMARE:
                return parent.AddComponent<SkillNightmare>();
            case SkillNames.RAPIDRELOAD:
                return parent.AddComponent<SkillRapidReload>();
                // NOT ADDED YET!!!
            case SkillNames.ROOST:
                return parent.AddComponent<SkillReallocate>();
            ///
            case SkillNames.SALVO:
                return parent.AddComponent<SkillSalvo>();
            case SkillNames.WINGBOMB:
                return parent.AddComponent<SkillWingBomb>();

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
