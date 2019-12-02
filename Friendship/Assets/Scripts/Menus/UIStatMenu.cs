using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatMenu : MenuHandler {

    MenuButton[] statButtons;   // These are just the buttons. Will need something to store the actual stats
    StatProperties[] characterStats; // Stats of each character. Ref from GameHandler.cs
    int numStats = 7;   // 5,6 = Frea; 7,8 = Luna
    int specialStats = 2;
    int numCharacters = 2;
    int characterSelect = 0;

    Sprite[] loadUpgradeSprite;
    List<GameObject> upgradeSegment = new List<GameObject>();
    List<GameObject> boostUpgrade = new List<GameObject>();

    Font font;
    List<string> statText = new List<string> { "Attack", "Health", "Defence", "Crit. Chance", "Dodge Chance" };

    public override void Init(MenuHandler parent)
    {
        statButtons = new MenuButton[numStats];
        font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        for(int i = 0; i < numStats; i++)
        {
            GameObject stats = new GameObject();
            stats.transform.SetParent(gameObject.transform);
            stats.AddComponent<Button>();
            statButtons[i] = stats.AddComponent<MenuButton>();
            statButtons[i].Init(i, this);
            statButtons[i].SetTextProperties(statText[i], font, 10, TextAnchor.MiddleLeft);
            statButtons[i].SetTextboxSize(new Vector2(100f, 50f));   // Placeholder value
        }

        // Load upgrade segment sprites
        loadUpgradeSprite = Resources.LoadAll<Sprite>("Menus/UpgradeSeg");

        GameObject tempSegment = new GameObject();
        tempSegment.AddComponent<Image>().sprite = loadUpgradeSprite[0];
        tempSegment.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(12f, 24f);

        for (int i = 0; i < 15; i++)
        {
            upgradeSegment.Add(Instantiate(tempSegment));
            upgradeSegment[i].transform.SetParent(gameObject.transform);
        }
        tempSegment.GetComponent<Image>().sprite = Resources.Load("Menus/UpgradeSeg2", typeof(Sprite)) as Sprite;
        for (int i = 0; i < 5; i++)
        {
            boostUpgrade.Add(Instantiate(tempSegment));
            boostUpgrade[i].transform.SetParent(gameObject.transform);
        }

        return;
    }

    public void Init(MenuHandler parent, StatProperties[] charStats)
    {
        characterStats = charStats;
        for(int i = 0; i < characterStats.Length; i++)
        {
            statText.AddRange(characterStats[i].specialStatNames);
        }

        Init(parent);
    }

    // WIP. Need stats to be implemented first
    public override void ChangeMenu(int newButton)
    {
        characterSelect = newButton;

        // Print out stats for the selected character
        for (int i = 0; i < numStats; i++)
        {
            if (i < numStats - specialStats)
                statButtons[i].SetTextProperties(statText[i] + ": " + characterStats[characterSelect].stats.maxCharStats[i], font, 10, TextAnchor.MiddleLeft);
            else
                statButtons[i].SetTextProperties(statText[i + specialStats * (characterSelect)] + ": " + characterStats[characterSelect].stats.maxCharStats[i], font, 10, TextAnchor.MiddleLeft);
        }

        SetStatSegmentPositions();
        return;
    }

    public override void Disable(bool enable)
    {
        for(int i = 0; i < numStats; i++)
        {
            statButtons[i].enabled = !enable;
        }

        for(int i = 0; i < upgradeSegment.Count; i++)
        {
            upgradeSegment[i].SetActive(!enable);
        }
        for(int i = 0; i < boostUpgrade.Count; i++)
        {
            boostUpgrade[i].SetActive(!enable);
        }

        return;
    }

    public void SetStartPoint(Vector2 start, float offset)
    {
        for(int i = 0; i < numStats; i++)
        {
            if (i < numStats - specialStats)
                statButtons[i].gameObject.transform.position = start - new Vector2(0f, offset) * i;
            else
                statButtons[i].gameObject.transform.position = Vector2.right * start.x * 2 / 3  + start - new Vector2(0f, offset * (i - numStats + specialStats));
        }
    }

    private void SetStatSegmentPositions()
    {
        // Object pooling
        int maxSegments = upgradeSegment.Count - 1;
        int maxBoostSegments = boostUpgrade.Count - 1;
        int currentSegmentCount = 0;
        int currentBoostSegmentCount = 0;
        int levelDifference = 0;

        for (int i = 0; i < maxBoostSegments + 1; i++)
        {
            boostUpgrade[i].transform.position = Vector2.left * 5000;
        }
        for (int i = 0; i < maxSegments + 1; i++)
        {
            upgradeSegment[i].transform.position = Vector2.left * 5000;
        }

        for (int i = 0; i < characterStats[characterSelect].maxUpgradeLevel.Length; i++)
        {
            levelDifference = characterStats[characterSelect].currentUpgradeLevel[i];

            for (int j = 0; j < characterStats[characterSelect].maxUpgradeLevel[i]; j++)
            {
                if (currentSegmentCount > maxSegments)
                {
                    upgradeSegment.Add(Instantiate(upgradeSegment[0]));
                    upgradeSegment[currentSegmentCount].transform.SetParent(gameObject.transform);
                }
                upgradeSegment[currentSegmentCount].transform.position = statButtons[i].gameObject.transform.position + Vector3.right * 12 * j + Vector3.left * 35 + Vector3.down * 20;
                if (j < levelDifference)
                    upgradeSegment[currentSegmentCount].GetComponent<Image>().sprite = loadUpgradeSprite[1];
                else
                    upgradeSegment[currentSegmentCount].GetComponent<Image>().sprite = loadUpgradeSprite[0];
                currentSegmentCount++;
            }
            for (int j = 0; j < levelDifference - characterStats[characterSelect].maxUpgradeLevel[i]; j++)
            {
                if (currentBoostSegmentCount > maxBoostSegments)
                {
                    boostUpgrade.Add(Instantiate(boostUpgrade[0]));
                    boostUpgrade[currentBoostSegmentCount].transform.SetParent(gameObject.transform);
                }
                boostUpgrade[currentBoostSegmentCount].transform.position = statButtons[i].gameObject.transform.position + Vector3.right * 12 * j + Vector3.left * 35 + Vector3.down * 20;
                currentBoostSegmentCount++;
            }
        }
    }

    public override void SwitchChar(int switchChar)
    {
        ChangeMenu(switchChar);
    }
}
