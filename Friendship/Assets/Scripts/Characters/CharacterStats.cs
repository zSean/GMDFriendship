using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CharacterStates
{
    none, attacked, damaged, dodge, dodged
}

public struct Stats
{
    public float[] maxCharStats;
    public static string[] statNames = { "Attack", "Health", "Defence", "Crit. Chance", "Dodge", "Attack Speed", "Crit. Damage" };
}

public class CharacterStats : MonoBehaviour {

    protected Stats charStats;
    protected float[] currentCharStats;

	// Use this for initialization
	public void Awake () {
        float[] mCharStats = { 1, 1, 0, 0, 0, 1, 1 };
        charStats.maxCharStats = mCharStats;
        currentCharStats = new float[mCharStats.Length];

        for (int i = 0; i < mCharStats.Length; i++)
        {
            currentCharStats[i] = mCharStats[i];
        }
    }

    public void AddCurrentStat(int index, float addStat)
    {
        /// Attack, Health, Defence, CritChance, DodgeChance, AttackSpeed, CritDamage
        if ((index <= currentCharStats.Length) && (index >= 0))
            currentCharStats[index] += addStat;
    }
    public void SetMaxStat(int index, float amount)
    {
        /// Attack, Health, Defence, CritChance, DodgeChance, AttackSpeed, CritDamage
        if ((index <= charStats.maxCharStats.Length) && (index >= 0))
            charStats.maxCharStats[index] = amount;
        // Do nothing with currentStats
    }

    // Getters
    public float GetMaxStat(int index)
    {
        if ((index < 0) || (index > charStats.maxCharStats.Length))
            index = 0;
        return charStats.maxCharStats[index];
    }
    public float GetCurrentStat(int index)
    {
        if ((index < 0) || (index > currentCharStats.Length))
            index = 0;
        return currentCharStats[index];
    }
    public string GetStatName(int index)
    {
        if ((index < 0) || (index > Stats.statNames.Length))
            index = 0;
        return Stats.statNames[index];
    }

    // What happens when this object receives damage (can occur w or w/o calculation)
    public void Attacked(int damage)
    {
        print(damage + " damage!");
        currentCharStats[1] -= damage;

        if (currentCharStats[1] <= 0)
            DestroyCharacter();
    }

    // Set to public. Who knows? There may be skills/cutscenes that insta-kill. Function to be overridden for main characters
    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }
}
