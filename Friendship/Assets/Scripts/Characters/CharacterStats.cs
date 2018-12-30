using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {


    // Attack, Health, Defence, CritChance, DodgeChance, AttackSpeed (multiplier), CritDamage (multiplier)
    protected float[] maxCharStats = { 1, 1, 0, 0, 0, 1, 1 };
    // For reference, may not use
    static string[] statNames = { "Attack", "Health", "Defence", "Crit. Chance", "Dodge", "Attack Speed", "Crit. Damage" };
    protected float[] currentCharStats;


	// Use this for initialization
	void Start () {
        currentCharStats = new float[maxCharStats.Length];

        for (int i = 0; i < maxCharStats.Length; i++)
        {
            currentCharStats[i] = maxCharStats[i];
        }
	}

    public void AddMaxStat(int index, float addStat)
    {
        /// Attack, Health, Defence, CritChance, DodgeChance, AttackSpeed, CritDamage
        if ((index <= maxCharStats.Length) && (index >= 0))
        {
            maxCharStats[index] += addStat;
            currentCharStats[index] += addStat;
        }
    }
    public void SetMaxStat(int index, float amount)
    {
        /// Attack, Health, Defence, CritChance, DodgeChance, AttackSpeed, CritDamage
        
        if ((index <= maxCharStats.Length) && (index >= 0))
            maxCharStats[index] = amount;
        // Do nothing with currentStats
    }

    // Getters
    public float GetMaxStat(int index)
    {
        if ((index < 0) || (index > maxCharStats.Length))
            index = 0;
        return maxCharStats[index];
    }
    public float GetCurrentStat(int index)
    {
        if ((index < 0) || (index > currentCharStats.Length))
            index = 0;
        return currentCharStats[index];
    }
    public string GetStatName(int index)
    {
        if ((index < 0) || (index > statNames.Length))
            index = 0;
        return statNames[index];
    }
}
