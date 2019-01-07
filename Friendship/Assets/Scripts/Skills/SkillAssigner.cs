using UnityEngine;

public static class SkillAssigner{

    public enum SkillNames { FIREBALL, JUDGEMENT, PURGE, HEAL, HRAESBEAT, KNIFETHROW, REALLOCATE };

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
            default:
                return null;
        }
    }
}
