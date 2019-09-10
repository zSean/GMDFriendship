using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CharacterStates
{
    none, attacked, damaged, dodge, dodged, kill, dead, swapIn, swapOut
}

public struct Stats
{
    public float[] maxCharStats;
    public static string[] statNames = { "Attack", "Health", "Defence", "Crit. Chance", "Dodge", "Attack Speed", "Crit. Damage" };
}

public class CharacterStats : MonoBehaviour {

    protected Stats charStats;
    protected float maxInvincibilityTimer = 0f;
    protected float invincibilityTimer;

    protected float[] currentCharStats;
    protected BuffHandler buffHandler;

    // Use this for initialization
    protected virtual void Awake () {
        float[] mCharStats = { 1, 1, 0, 0, 0, 1, 1 };
        charStats.maxCharStats = mCharStats;
        currentCharStats = new float[mCharStats.Length];

        invincibilityTimer = maxInvincibilityTimer;

        for (int i = 0; i < mCharStats.Length; i++)
        {
            currentCharStats[i] = mCharStats[i];
        }

        buffHandler = gameObject.AddComponent<BuffHandler>();
    }

    public void SetCurrentStat(int index, float newStat)
    {
        currentCharStats[index] = newStat;
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
    public void SetMaxInvincibilityTimer(float newMax)
    {
        maxInvincibilityTimer = newMax;
        invincibilityTimer = newMax;
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
    public float GetMaxInvincibilityTimer()
    {
        return maxInvincibilityTimer;
    }
    public float GetInvincibilityTimer()
    {
        return invincibilityTimer;
    }

    // What happens when this object receives damage (can occur w or w/o calculation)
    public void Attacked(int damage)
    {
        if ((damage <= 0) || (invincibilityTimer < 0))
        {
            if (damage >= 0)
            {
                print(damage + " damage!");
                invincibilityTimer = maxInvincibilityTimer;
            }
            else
                print("Heal " + -damage + "!");

            currentCharStats[1] -= damage;
            if (currentCharStats[1] > GetMaxStat(1))
                currentCharStats[1] = GetMaxStat(1);

            if (currentCharStats[1] <= 0)
                DestroyCharacter();
        }
    }

    // Set to public. Who knows? There may be skills/cutscenes that insta-kill. Function to be overridden for main characters
    public virtual void DestroyCharacter()
    {
        Destroy(gameObject);
    }
}
