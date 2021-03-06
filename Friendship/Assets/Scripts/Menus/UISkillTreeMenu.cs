﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTreeMenu : MenuHandler {

    // Main buttons
    int numSkillTypes = 4;
    MenuButton[] skillTypes;
    Sprite[] skillTypeImage;
    Font font;

    // Skills: Aerial, Flurry, Buff, Perk.
    Dictionary<int, SkillProperties[]> skills;
    Sprite[][] skillImage;
    MenuButton[] skillButtons;
    int[] equippedSkills;

    // Currently selected buttons for each skill
    int[] selectedButtons;
    int tracker = 0;    // Keeps track of which group of skills the page is on

    // Switching between characters
    int switchChar = 0;

    // Small Description box
    MenuButton descriptions;

    // Equipping and upgrading skills
    MenuButton equipButton;
    MenuButton upgradeButton;

    // Displays which skill is currently equipped
    Image equipImage;

    public override void Init(MenuHandler parent)
    {
        font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        skillTypeImage = Resources.LoadAll<Sprite>("SkillTabs");
        skillTypes = new MenuButton[numSkillTypes];

        string[] skillHeader = { "Aerial", "Flurry", "Buff", "Perk" };
      //  equippedSkills = new int[7];    // Aerial, Flurry, Buff, Perk, Active x2
        numSkillTypes = skillHeader.Length;

        for (int i = 0; i < numSkillTypes; i++)
        {
            // Create the button
            GameObject skillButtonTypes = new GameObject();
            skillButtonTypes.transform.SetParent(gameObject.transform);
            skillButtonTypes.AddComponent<Button>();
            skillTypes[i] = skillButtonTypes.AddComponent<MenuButton>();
            skillTypes[i].Init(i, this);
            skillTypes[i].SetTextProperties(skillHeader[i], font, 10, TextAnchor.MiddleCenter);
            skillButtonTypes.AddComponent<Image>().sprite = skillTypeImage[i];
            skillButtonTypes.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(skillButtonTypes.GetComponent<Image>().sprite.rect.width, skillButtonTypes.GetComponent<Image>().sprite.rect.height / 1.5f);
            skillTypes[i].SetTextboxSize(skillButtonTypes.GetComponent<Image>().rectTransform.sizeDelta);
        }

        skillButtons = new MenuButton[9];   // There will be at most 9 menu buttons for this game
        for(int i = 0; i < skillButtons.Length; i++)
        {
            // Create the button
            GameObject skillDisplay = new GameObject();
            skillDisplay.transform.SetParent(gameObject.transform);
            skillDisplay.AddComponent<Button>();
            skillButtons[i] = skillDisplay.AddComponent<MenuButton>();
            skillButtons[i].Init(i + 99, this);
            skillDisplay.AddComponent<Image>().sprite = Resources.Load("BlockColours", typeof(Sprite)) as Sprite;
            skillDisplay.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(skillDisplay.GetComponent<Image>().sprite.rect.width, skillDisplay.GetComponent<Image>().sprite.rect.height / 1.5f);
            skillButtons[i].SetTextboxSize(skillDisplay.GetComponent<Image>().rectTransform.sizeDelta);
        }


        // Loading all the sprite images
        skillImage = new Sprite[8][] {
            new Sprite[9], new Sprite[9], new Sprite[9], new Sprite[3], // Frea
            new Sprite[9], new Sprite[9], new Sprite[9], new Sprite[3]  // Luna
        };

        skillImage[3] = Resources.LoadAll<Sprite>("BlockColours");
        skillImage[7] = Resources.LoadAll<Sprite>("BlockColours");


        // Create the skill description box
        GameObject skillDescriptionBox = new GameObject();
        descriptions = skillDescriptionBox.AddComponent<MenuButton>();
        skillDescriptionBox.transform.SetParent(gameObject.transform);
        descriptions.SetTextProperties("", font, 10, TextAnchor.UpperLeft);
        descriptions.SetTextboxSize(new Vector2(250f, 50f));


        // Create the equip button
        GameObject equipButtonObject = new GameObject();
        equipButtonObject.AddComponent<Button>();
        equipButton = equipButtonObject.AddComponent<MenuButton>();
        equipButton.Init(numSkillTypes + 1, this);
        equipButtonObject.transform.SetParent(gameObject.transform);
        equipButton.SetButtonNum(numSkillTypes + 1);
        equipButton.SetTextProperties("Equip", font, 10, TextAnchor.MiddleCenter);
        equipButtonObject.AddComponent<Image>().sprite = Resources.Load("ButtonTabs", typeof(Sprite)) as Sprite;
        equipButtonObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(equipButtonObject.GetComponent<Image>().sprite.rect.width, equipButtonObject.GetComponent<Image>().sprite.rect.height / 1.5f);
        equipButton.SetTextboxSize(equipButtonObject.GetComponent<Image>().rectTransform.sizeDelta);

        // Create the sprite for the currently equipped skill
        GameObject equipImageObject = new GameObject();
        equipImage = equipImageObject.AddComponent<Image>();
        equipImageObject.transform.SetParent(gameObject.transform);
        equipImageObject.GetComponent<Image>().rectTransform.sizeDelta = skillButtons[0].gameObject.GetComponent<Image>().rectTransform.sizeDelta;

        // Create the upgrade button
        GameObject upgradeButtonObject = new GameObject();
        upgradeButtonObject.AddComponent<Button>();
        upgradeButton = upgradeButtonObject.AddComponent<MenuButton>();
        upgradeButton.Init(numSkillTypes + 2, this);
        upgradeButton.transform.SetParent(gameObject.transform);
        upgradeButton.SetButtonNum(numSkillTypes + 2);
        upgradeButton.SetTextProperties("Upgrade", font, 10, TextAnchor.MiddleCenter);
        upgradeButtonObject.AddComponent<Image>().sprite = Resources.Load("ButtonTabs", typeof(Sprite)) as Sprite;
        upgradeButtonObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(upgradeButtonObject.GetComponent<Image>().sprite.rect.width, upgradeButtonObject.GetComponent<Image>().sprite.rect.height / 1.5f);
        upgradeButton.SetTextboxSize(upgradeButtonObject.GetComponent<Image>().rectTransform.sizeDelta);
    }

    public void Init(MenuHandler parent, Dictionary<int, SkillProperties[]> loadedStats, int[] equippedSkills)
    {
        skills = loadedStats;
        selectedButtons = equippedSkills;
        this.equippedSkills = equippedSkills;
        Init(parent);
        return;
    }

    public Dictionary<int, SkillProperties[]> ReturnSkills()
    {
        return skills;
    }

    private void TreeArrangement(SkillProperties[] skillSet, int selectedButton)
    {
        if (skillSet.Length % 9 == 0) // 9 skills
            ThreeSkillTreeArrangement(skillSet, selectedButton);
        else // Assume 3 skills
            SkillArrangement(skillSet, selectedButton);
    }
    private void ThreeSkillTreeArrangement(SkillProperties[] skillSet, int selectedButton)
    {
        // 3 x 3 buttons
        for(int i = 0; i < 9; i++)
        {
            skillButtons[i].enabled = true;
            skillButtons[i].GetComponent<Image>().enabled = true;
            // Switch skillButton image
            skillButtons[i].SetText(SkillDescriptions.ReturnDescription(skillSet[i + skillSet.Length / 2 * switchChar]));
        }
        return;
    }
    private void SkillArrangement(SkillProperties[] skillSet, int selectedButton)
    {
        // 1 x 3 buttons. Due to the way the buttons are arranged, the enabled buttons are the multiples of 3
        for (int i = 0; i < 9; i++)
        {
            if(i % 3 == 0)
            {
                skillButtons[i].enabled = true;
                skillButtons[i].GetComponent<Image>().enabled = true;
                skillButtons[i].GetComponent<Image>().sprite = skillImage[3 /*selectedButton*/][3 - (9 - i) / 3];
                skillButtons[i].SetText(SkillDescriptions.ReturnDescription(skillSet[i / 3 + skillSet.Length / 2 * switchChar]));
            }
            else
            {
                skillButtons[i].enabled = false;
                skillButtons[i].GetComponent<Image>().enabled = false;
            }
        }
        return;
    }

    public override void ChangeMenu(int newButton)
    {
        // If a skill button was pressed
        if (newButton >= 99)
        {
            skillButtons[selectedButtons[tracker]].SelectButton(false);
            selectedButtons[tracker] = newButton - 99;
            skillButtons[selectedButtons[tracker]].SelectButton(true);
            descriptions.SetText(skillButtons[selectedButtons[tracker]].GetText());

            if (skills[tracker][selectedButtons[tracker]].level <= 0)
            {
                equipButton.gameObject.GetComponent<Button>().enabled = false;
                equipButton.gameObject.GetComponent<Image>().enabled = false;
                equipButton.enabled = false;
            }
            else
            {
                equipButton.gameObject.GetComponent<Button>().enabled = true;
                equipButton.gameObject.GetComponent<Image>().enabled = true;
                equipButton.enabled = true;
            }

            if ((tracker != 3) && ((newButton - 99) % 3 == 0) && (skills[tracker][selectedButtons[tracker]].level < 5))
            {
                upgradeButton.enabled = true;
                upgradeButton.gameObject.GetComponent<Button>().enabled = true;
                upgradeButton.gameObject.GetComponent<Image>().enabled = true;
            }
            else
            {
                upgradeButton.enabled = false;
                upgradeButton.gameObject.GetComponent<Button>().enabled = false;
                upgradeButton.gameObject.GetComponent<Image>().enabled = false;
            }
            return;
        }
        else if (newButton == numSkillTypes + 1) // If the equip button was clicked on
        {
            equippedSkills[tracker] = selectedButtons[tracker];
            equipImage.sprite = skillButtons[selectedButtons[tracker]].gameObject.GetComponent<Image>().sprite;
        }
        else if (newButton == numSkillTypes + 2)    // If the upgrade button was clicked on
        {
            // Max stat cap at level 5. Upgrading 1 skill upgrades ALL OF THAT SKILL'S VARIANTS
            if (skills[tracker][selectedButtons[tracker]].level < 5)
            {
                skills[tracker][selectedButtons[tracker]].level++;
                skills[tracker][selectedButtons[tracker] + 1].level++;
                skills[tracker][selectedButtons[tracker] + 2].level++;

                if(skills[tracker][selectedButtons[tracker]].level >= 5)
                {
                    skillButtons[selectedButtons[tracker] + 1].SetText(SkillDescriptions.ReturnDescription(skills[tracker][selectedButtons[tracker] + 1]));
                    skillButtons[selectedButtons[tracker] + 2].SetText(SkillDescriptions.ReturnDescription(skills[tracker][selectedButtons[tracker] + 2]));
                    upgradeButton.enabled = false;
                    upgradeButton.gameObject.GetComponent<Button>().enabled = false;
                    upgradeButton.gameObject.GetComponent<Image>().enabled = false;
                }
            }
            skillButtons[selectedButtons[tracker]].SetText(SkillDescriptions.ReturnDescription(skills[tracker][selectedButtons[tracker]]));
            descriptions.SetText(skillButtons[selectedButtons[tracker]].GetText());
        }
        else    // Otherwise it would have been a tab
        {
            tracker = newButton;
            TreeArrangement(skills[newButton], newButton);
            descriptions.SetText("");
            equipButton.gameObject.GetComponent<Button>().enabled = false;
            equipButton.gameObject.GetComponent<Image>().enabled = false;
            equipButton.enabled = false;

            equipImage.sprite = skillButtons[selectedButtons[tracker]].gameObject.GetComponent<Image>().sprite;

            upgradeButton.enabled = false;
            upgradeButton.gameObject.GetComponent<Button>().enabled = false;
            upgradeButton.gameObject.GetComponent<Image>().enabled = false;
            return;
        }
    }
    public override void Disable(bool enable)
    {
        for(int i = 0; i < numSkillTypes; i++)
        {
            skillTypes[i].gameObject.GetComponent<Button>().enabled = !enable;
            skillTypes[i].gameObject.GetComponent<Image>().enabled = !enable;
            skillTypes[i].enabled = !enable;
        }

        // Work on fixing this later
        for(int i = 0; i < 9; i++)
        {
            skillButtons[i].enabled = !enable;
            skillButtons[i].GetComponent<Image>().enabled = !enable;
        }

        equipButton.gameObject.GetComponent<Button>().enabled = !enable;
        equipButton.gameObject.GetComponent<Image>().enabled = !enable;
        equipButton.enabled = !enable;

        equipImage.enabled = !enable;

        upgradeButton.enabled = !enable;
        upgradeButton.gameObject.GetComponent<Button>().enabled = !enable;
        upgradeButton.gameObject.GetComponent<Image>().enabled = !enable;

        descriptions.enabled = !enable;
    }
    public void SetStartPoint(Vector2 start, float offset)
    {
        for(int i = 0; i < numSkillTypes; i++)
        {
            skillTypes[i].gameObject.transform.position = start - new Vector2(skillTypes[i].gameObject.GetComponent<Image>().sprite.rect.width / 2, offset * i);
        }

        for (int i = 0; i < 3; i++)
        {
            // Hardcoding. Revisit later
            skillButtons[3 * i].transform.position = start + new Vector2(80 + skillButtons[0].gameObject.GetComponent<Image>().sprite.rect.width * 2 * i, -60f); // Centre
            skillButtons[3 * i + 1].transform.position = start + new Vector2(80 + skillButtons[0].gameObject.GetComponent<Image>().sprite.rect.width * 2 * i, -10f); // Upper
            skillButtons[3 * i + 2].transform.position = start + new Vector2(80 + skillButtons[0].gameObject.GetComponent<Image>().sprite.rect.width * 2 * i, -110f); // Lower
        }

        equipImage.gameObject.transform.position = Vector3.right * 50f + Vector3.up * (skillButtons[0].transform.position.y + 90);
        descriptions.gameObject.transform.position = skillButtons[3].transform.position + Vector3.down * 90f;
        equipButton.gameObject.transform.position = descriptions.gameObject.transform.position + Vector3.down * 30f + Vector3.right * 50f;
        upgradeButton.gameObject.transform.position = descriptions.gameObject.transform.position + Vector3.down * 30f + Vector3.left * 50f;

    }

    public override void SwitchChar(int switchChar)
    {
        this.switchChar = switchChar;
        TreeArrangement(skills[tracker], tracker);
    }
}
