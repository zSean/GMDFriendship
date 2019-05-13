using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mod
public static class SkillDescriptions {

    public static string ReturnDescription(SkillProperties skill, int variant = 0)
    {
        // If the skill has not been unlocked yet
        if(skill.level[variant] <= 0)
            return "Locked.";
 
        // If the skill has been unlocked
        string skillName = "";
        switch (skill.name)
        {
            case SkillAssigner.SkillNames.ACTIVEHEALING:
                skillName = "Active Healing";
                return skillName + ": Heal " + skill.level[variant] + "% hp every 5 kills.";
            case SkillAssigner.SkillNames.AGILITY:
                skillName = "Agility";
                return skillName + ": Faster movement, higher jumps.";
            case SkillAssigner.SkillNames.BOLT:
                skillName = "Bolt";
                if (variant == 2)
                    return skillName + ": Instantly kills non-boss enemies, deals 120% damage to bosses. Additionally, summons 3 bolts randomly on the map dealing 100% damage. Luna's attack decreased by 50% for 5 seconds.";
                else if (variant == 1)
                    return skillName + ": Instantly kills non-boss enemies, deals 120% damage to bosses. Luna's attack decreased by 0%";
                else
                    return skillName + ": Instantly kills non-boss enemies, deals " + (100 + skill.level[variant] * 10) + "% damage to bosses. Luna's attack decreased by 50% for 5 seconds.";
            case SkillAssigner.SkillNames.BOMBTOSS:
                skillName = "Bomb toss";
                if (variant == 2)
                    return skillName + ": Throws an explosive knife forward, dealing 150% damage to the enemy and 40% damage to surrounding enemies. Enemies are inflicted with stun for 3 seconds (they deal no contact damage).";
                else if (variant == 1)
                    return skillName + ": Throws an explosive knife forward, dealing 150% damage to the enemy and 70% damage to surrounding enemies. Explosion radius is increased.";
                else
                    return skillName + ": Throws an explosive knife forward, dealing " + (130 + skill.level[variant] * 10) + "% damage to the enemy and " + (30 + skill.level[variant] * 5) + " damage to surrounding enemies.";
            case SkillAssigner.SkillNames.ELEGANCE:
                skillName = "Elegance";
                if (variant == 2)
                    return skillName + ": Gain invulnerability for 0.5 seconds. Reduce other active skill cooldown by 5 seconds and charge burst by 10% on successful dodge.";
                else if (variant == 1)
                    return skillName + ": Gain invulnerability for 0.5 seconds. Reset Elegance on successful dodge (max 3 times within 8 seconds).";
                else
                    return skillName + ": Gain invulnerability for " + (0.5 + skill.level[variant] * 0.1) + " seconds.";
            case SkillAssigner.SkillNames.ETERNALFLAME:
                skillName = "Fiyah!";
                if (variant == 2)
                    return skillName + ": Places a fire in front of Luna that lasts for 8 seconds and deals 35% damage per second with 40% chance to inflict 5% poison every attack.";
                else if (variant == 1)
                    return skillName + ": Places a fire in front of Luna that lasts for 8 seconds and deals 35% damage per second and inflicts 40% burn damage every attack.";
                else
                    return skillName + ": Places a fire in front of Luna that lasts for 8 seconds and deals " + (25 + skill.level[variant] * 5) + " % damage per second.";
            case SkillAssigner.SkillNames.FALLENWINGS:
                skillName = "Fallen wings";
                if (variant == 2)
                    return skillName + ": Short-ranged attack dealing 125% damage + 50% damage if Luna's hp is greater than 70%. Deal an additional 100% damage against bosses if Luna's hp is greater than 70%. Decrease Luna's hp by 15% of her current health. Mana regeneration is increased by 10% for 5 seconds";
                else if (variant == 1)
                    return skillName + ": Short-ranged attack dealing 125% damage + 50% damage if Luna's hp is greater than 70%. Decrease Luna's hp by 15% of her current health. Restore Frea's health by 10%. Mana regeneration is increased by 10% for 5 seconds";
                else
                    return skillName + ": Short-ranged attack dealing 125% damage + " + (50 + skill.level[variant] * 5) + "% damage if Luna's hp is greater than 70%. Decrease Luna's hp by 15% of her current health. Mana regeneration is increased by " + (10 + skill.level[variant] * 2) + "% for 5 seconds.";
            case SkillAssigner.SkillNames.FEATHERDANCE:
                skillName = "Feather dance";
                if (variant == 2)
                    return skillName + ": Deal damage based on chain number: 1 block = 20% damage x2, 2 blocks = 35% damage x2, 3 blocks = 60% damage x2. Mana is not depleted on using any chain. Defence is ignored.";
                else if (variant == 1)
                    return skillName + ": Deal 60% damage x4 to a single target. Defence is not ignored.";
                else
                    return skillName + ": Deal " + (60 + skill.level[variant] * 5) + "% damage x2 to a single target. Defence is ignored.";
            case SkillAssigner.SkillNames.FEATHERSHIELD:
                skillName = "Armor";
                if (variant == 2)
                    return skillName + ": Gain immunity to projectiles for 1.5 seconds. Deal 70% reflect damage to surrounding area every time a projectile hits during this period.";
                else if (variant == 1)
                    return skillName + ": Gain immunity to projectiles for 1.5 seconds. Regain 3% mp every time a projectile hits during this period";
                else
                    return skillName + ": Gain immunity to projectiles for " + (1.5 + 0.2 * skill.level[variant]) + " seconds.";
            case SkillAssigner.SkillNames.FIREBALL:
                skillName = "Fireball";
                if (variant == 2)
                    return skillName + ": Hurls a fireball that deals 80% damage on impact and 60% burn damage.";
                else if (variant == 1)
                    return skillName + ": Hurls a fireball that deals 80% damage on impact and inflicts 5% poison.";
                else
                    return skillName + ": Hurls a fireball that  deals " + (100 + 6 * skill.level[variant]) + " damage on impact and 30% burn damage.";
            case SkillAssigner.SkillNames.FREASTORM:
                skillName = "Frea Storm!";
                if (variant == 2)
                    return skillName + ": Summons 6 Frea clones to charge forward and deal 40% damage to each enemy hit.";
                else if (variant == 1)
                    return skillName + ": Summons 4 Frea clones to charge forward and deal 40% damage to each enemy hit. Each Frea has a burn aura that inflicts 30% burn damage to nearby enemies";
                else
                    return skillName + ": Summons 4 Frea clones to charge forward and deal " + (50 + skill.level[variant] * 5) + "% damage to each enemy hit.";
            case SkillAssigner.SkillNames.GUARDIAN:
                skillName = "Guardian";
                return skillName + ": On suffering a fatal hit, Luna's health stays at 1 and she is swapped out with Frea if Frea has health greater than 0. Cooldown 180 seconds.";
            case SkillAssigner.SkillNames.HEAL:
                skillName = "Heal";
                if (variant == 2)
                    return skillName + ": Immediately restores 5% health. Additionally, gain an aura for 3 seconds that causes 15% burn damage to nearby enemies.";
                else if (variant == 1)
                    return skillName + ": Restores 3% health per second over 5 seconds.";
                else
                    return skillName + ": Immediately restores " + (5 + skill.level[variant]) + "% health.";
            case SkillAssigner.SkillNames.HRAESBEAT:
                skillName = "Hraes beat";
                if (variant == 2)
                    return skillName + ": Deal 120% damage to the closest enemy. Deal an additional 100% damage to that enemy if their health is < 30%.";
                else if (variant == 1)
                    return skillName + ": Deal 120% damage to the closest, with a 35% chance for critical damage.";
                else
                    return skillName + ": Deal " + (120 + 8 * skill.level[variant]) + "% damage to the closest enemy.";
            case SkillAssigner.SkillNames.ICESPEAR:
                skillName = "Ice spears";
                if (variant == 2)
                    return skillName + ": Summons 8 spears to attack an enemy, dealing 25% damage per spear. Spears have piercing.";
                else if (variant == 1)
                    return skillName + ": Summons 4 spears to attack an enemy, dealing 25% damage per spear and inflicting slow. Spears have piercing.";
                else
                    return skillName + ": Summons 4 spears to attack an enemy, dealing " + (30 + skill.level[variant] * 4) + "% damage per spear. Spears have piercing.";
            case SkillAssigner.SkillNames.JUDGEMENT:
                skillName = "Pillar of fire";
                if (variant == 2)
                    return skillName + ": Summons a pillar of fire randomly dealing 50-200% damage to an enemy";
                else if (variant == 1)
                    return skillName + ": Summons a pillar of fire randomly dealing 1-200% damage to an enemy, with a 25% chance to reactivate, dealing 50% of the initial damage";
                else
                    return skillName + ": Summons a pillar of fire randomly dealing 1-" + (200 + skill.level[variant] * 15) + "% damage to an enemy.";
            case SkillAssigner.SkillNames.KNIFETHROW:
                skillName = "Knife throw";
                if (variant == 2)
                    return skillName + ": Throws a knife forward, dealing 90% damage to enemies. Piercing. Ignores defence.";
                else if (variant == 1)
                    return skillName + ": Throws a knife forward, dealing 90% damage to an enemy. After 1 second, it will detonate, dealing an additional 40% damage to surrounding enemies.";
                else
                    return skillName + ": Throws a knife forward, dealing " + (120 + 6 * skill.level[variant]) + "% damage to an enemy. Ignores defence.";
            case SkillAssigner.SkillNames.NIGHTMARE:
                skillName = "Nightmare";
                if (variant == 2)
                    return skillName + ": Stuns 2 enemies (they are unable to deal contact damage) and deals 25% daamge per second for 3 seconds.";
                else if (variant == 1)
                    return skillName + ": Stuns an enemy (they are unable to deal contact damage) and deals 25% damage per second for 3 seconds. If the enemy dies during this time, deal 115% damage to surrounding enemies.";
                else
                    return skillName + ": Stuns an enemy (they are unable to deal contact damage) and deals " + (30 + skill.level[variant] * 2) + "% damage per second for 3 seconds";
            case SkillAssigner.SkillNames.PURGE:
                skillName = "Ignition";
                if (variant == 2)
                    return skillName + ": Deals 80% damage to all enemies and inflicts 2% poison and 25% burn damage.";
                else if (variant == 1)
                    return skillName + ": Deals 80% damage to all enemies, with a 10% chance to kill non-boss enemies.";
                else
                    return skillName + ": Deals " + (80 + skill.level[variant] * 5) + "% damage to all enemies.";
            case SkillAssigner.SkillNames.RAPIDRELOAD:
                skillName = "Rapid reload";
                return skillName + ": Block skills can be activated with 2 chains (Luna only). Mana is still consumed.";
            case SkillAssigner.SkillNames.REALLOCATE:
                skillName = "Reallocate";
                if (variant == 2)
                    return skillName + ": Restore 8% mp. Dodge chance is increased by 10% for 5 seconds";
                else if (variant == 1)
                    return skillName + ": Restore 8% mp. Critical rate is increased by 5% for 5 seconds";
                else
                    return skillName + ": Restore " + (10 + skill.level[variant] * 2) + "% mp. Block generation speed is increased by 10% for 5 seconds";
            case SkillAssigner.SkillNames.ROOST:
                skillName = "Roost";
                    return skillName + ": Summon a shield every 20 seconds that can absorb 3 projectile hits. If a shield already exists, destroy it and heal Luna's health by 2-6% depending on the number of hits left on the destroyed shield.";
            case SkillAssigner.SkillNames.SALVO:
                skillName = "Salvo";
                if (variant == 2)
                    return skillName + ": Fires 3 shots dealing 40% damage per shot. Enemies hit will take 10% more damage from attacks for 5 seconds (max 8 stacks).";
                else if (variant == 1)
                    return skillName + ": Fires 3 shots dealing 40% damage per shot. Enemies hit will take 10% more damage from attacks for 5 seconds. On accumulating 5 stacks, deal 300% damage to the enemy and reset stacks.";
                else
                    return skillName + ": Fires 3 shots dealing" + (40 + skill.level[variant] * 4) + "% damage per shot. Enemies hit will take 10% more damage from attacks for 5 seconds (max 5 stacks).";
            case SkillAssigner.SkillNames.SHOOT:
                skillName = "Shoot";
                if (variant == 2)
                    return skillName + ": Fires a pistol, stunning enemies for 1 second (enemies are unable to deal contact damage). Additionally hurls a fireball, dealing 115% damage to an enemy and inflicting 20% burn damage";
                else if (variant == 1)
                    return skillName + ": Fires a pistol, stunning enemies for 1 second. Increases flurry and aerial block skill damage by 50% for 5 seconds";
                else
                    return skillName + ": Fires pistol, stunning enemies for " + (1.5 + 0.3 * skill.level[variant]) + " seconds (enemies are unable to deal contact damage).";
            case SkillAssigner.SkillNames.SIXTHSENSE:
                skillName = "Sixth sense";
                if (variant == 2)
                    return skillName + ": Dodge and counterattack for 80% damage if about to take damage from an enemy. Gain 3 seconds invulnerability.";
                else if (variant == 1)
                    return skillName + ": Dodge and counterattack for 80% damage if about to take damage from an enemy. Additionally, deal 80% damage to surrounding enemies. Gain 1 second invulnerability.";
                else
                    return skillName + ": Dodge and counterattack for " + (100 + skill.level[variant] * 8) + "% damage if about to take damage from an enemy. Gain 1 second invulnerability.";
            case SkillAssigner.SkillNames.SPIRITFOX:
                skillName = "Spirit fox";
                if (variant == 2)
                    return skillName + ": Summon a familiar to attack for 5 seconds, dealing 40% damage per attack. Able to pick up coins in its radius. Additional casts reset its summon duration and increases it by 4 seconds (max 5 seconds).";
                else if (variant == 1)
                    return skillName + ": Summon a familiar to attack for 5 seconds, dealing 40% damage per attack. Additionally, summons 2 orbitals that periodically attack for 50% damage. Able to pick up coins in its radius. Additional casts reset its summon duration and increases it by 2 seconds (max 5 stacks).";
                else
                    return skillName + ": Summon a familiar to attack for 5 seconds, dealing " + (50 + skill.level[variant] * 4) + "% damage per attack. Able to pick up coins in its radius. Additional casts resets its summon duration and increases it by 2 seconds (max 5 stacks).";
            case SkillAssigner.SkillNames.TELEPORT:
                skillName = "Teleport";
                if (variant == 2)
                    return skillName + ": Teleports a certain distance. Teleporting backwards will throw a knife that deals 50% damage to an enemy on contact.";
                else if (variant == 1)
                    return skillName + ": Teleports a certain distance. Cooldown significantly reduced.";
                else
                    return skillName + ": Teleports a certain distance. Distance increased with skill level.";
            case SkillAssigner.SkillNames.PURITY:
                skillName = "Purity";
                return skillName + ": Gain debuff resistance.";
            case SkillAssigner.SkillNames.WARMTH:
                skillName = "Warmth";
                if (variant == 2)
                    return skillName + ": Gain 20% damage reduction for 3 seconds. Gain 100% damage reduction every 5 times this skill is casted.";
                else if (variant == 1)
                    return skillName + ": Gain 20% damage reduction for 3 seconds. Additionally, generate 1 random block on activation.";
                else
                    return skillName + ": Gain " + (20 + skill.level[variant] * 2) + "% damage reduction for 3 seconds.";
            case SkillAssigner.SkillNames.WINGBOMB:
                skillName = "Demise";
                if (variant == 2)
                    return skillName + ": Launches a single missile that homes on the farthest enemy and deals 90% damage to each enemy it hits on the way. Piercing.";
                else if (variant == 1)
                    return skillName + ": Launches arcing missiles that deal 90% damage per missile. On hitting any ground surface, summons spikes dealing 60% damage. Piercing.";
                else
                    return skillName + ": Launches arcing missiles that deal " + (100 + skill.level[variant] * 6) + "% damage per missile. Piercing";
            default:
                return "";
        }
    }
}
