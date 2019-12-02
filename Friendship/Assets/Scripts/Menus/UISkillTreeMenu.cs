using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Mod
public class UISkillTreeMenu : MenuHandler {

    // Main buttons
    int numSkillTypes = 4;
    int numberOfSkillTypes = 5; // TESTING PURPOSES ONLY. Mimics numSkillTypes, should be replaced with numSkillTypes once ActiveSkills are added
    MenuButton[] skillTypes;
    Sprite[] skillTypeImage;
    Font font;

    // Skills: Aerial, Flurry, Buff, Perk.
    Dictionary<int, SkillProperties[]> skills;
    Dictionary<int, List<Sprite[]>> skillImage = new Dictionary<int, List<Sprite[]>>();
    MenuButton[] skillButtons;
    int[,] equippedSkills;
    Sprite lockedSkill;

    // Currently selected buttons for each skill
    int[] selectedButtons;
    int tracker = 0;    // Keeps track of which group of skills the page is on

    // Switching between characters
    int switchChar = 0;
    readonly int totalChar = 2; //5;

    // Small Description box
    MenuButton descriptions;

    // Equipping and upgrading skills
    MenuButton equipButton;
    MenuButton upgradeButton;

    // Displays which skill is currently equipped
    Image equipImage;

    public override void Init(MenuHandler parent)
    {
        lockedSkill = Resources.Load("CharacterSprites/Skills/SkillIcons/LockedIcon", typeof(Sprite)) as Sprite;
        font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        skillTypeImage = Resources.LoadAll<Sprite>("SkillTabs");
        skillTypes = new MenuButton[numSkillTypes];

        string[] skillHeader = { "Aerial", "Flurry", "Buff", "Perk" };
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

        foreach(int key in skills.Keys)
        {
            List<Sprite[]> images = new List<Sprite[]>();
            for (int i = 0; i < skills[key].Length; i++)
            {
                images.Add(Resources.LoadAll<Sprite>(skills[key][i].skillImagePath));
            }
            skillImage[key] = images;
        }

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

    public void Init(MenuHandler parent, ref Dictionary<int, SkillProperties[]> loadedStats, ref int[,] equippedSkills /*ref SkillProperties[] equippedSkills*/)
    {
        skills = loadedStats;
        selectedButtons = new int[equippedSkills.Length / totalChar];

        for(int i = 0; i < selectedButtons.Length; i++)
        {
            selectedButtons[i] = equippedSkills[i, 0] + equippedSkills[i, 1];
        }

        this.equippedSkills = equippedSkills; // selectedButtons initially starts off the same as equippedSkills
        Init(parent);
    }

    public Dictionary<int, SkillProperties[]> ReturnSkills()
    {
        return skills;
    }

    public int[,] ReturnEquippedSkills()
    {
        return equippedSkills;
    }

    private void TreeArrangement(SkillProperties[] skillSet)
    {
        if (skillSet[0].level.Length % 3 != 0) // Skills without variants
            ThreeSkillTreeArrangement(skillSet);
        else // Assume skills with variants
            SkillArrangement(skillSet);
    }
    private void ThreeSkillTreeArrangement(SkillProperties[] skillSet)
    {
        // 1 x 3 buttons. Due to the way the buttons are arranged, the enabled buttons are the multiples of 3
        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0)
            {
                skillButtons[i].enabled = true;
                skillButtons[i].GetComponent<Image>().enabled = true;
                skillButtons[i].GetComponent<Image>().sprite = skillImage[3][i / 3 + switchChar * skills[tracker].Length / totalChar][0];
                skillButtons[i].SetText(SkillDescriptions.ReturnDescription(skillSet[i / 3 + skillSet.Length / totalChar * switchChar]));
            }
            else
            {
                skillButtons[i].enabled = false;
                skillButtons[i].GetComponent<Image>().enabled = false;
            }
        }
        return;
    }
    private void SkillArrangement(SkillProperties[] skillSet)
    {
        // 3 x 3 buttons
        for (int i = 0; i < 9; i++)
        {
            skillButtons[i].enabled = true;
            skillButtons[i].GetComponent<Image>().enabled = true;
            // Switch skillButton image
            if (skills[tracker][i / 3 + switchChar * skills[tracker].Length / 2].level[i % 3] > 0)
                skillButtons[i].GetComponent<Image>().sprite = skillImage[tracker][i / 3 + skillSet.Length / totalChar * switchChar][i % 3];
            else
                skillButtons[i].GetComponent<Image>().sprite = lockedSkill;
            skillButtons[i].SetText(SkillDescriptions.ReturnDescription(skillSet[i / 3 + skillSet.Length / totalChar * switchChar], i % 3));
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

            if (skills[tracker][selectedButtons[tracker] / 3 + switchChar * skills[tracker].Length / totalChar].level[selectedButtons[tracker] % 3] <= 0)
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

            if ((tracker != 3) && ((newButton - 99) % 3 == 0) && (skills[tracker][selectedButtons[tracker] / 3 + switchChar * skills[tracker].Length / totalChar].level[selectedButtons[tracker] % 3] < 5))
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
        }
        else if (newButton == numSkillTypes + 1) // If the equip button was clicked on
        {
            // ACTIVE SKILLS OFFSET
            equippedSkills[tracker + switchChar * (numberOfSkillTypes + 2), 0] = selectedButtons[tracker] - (selectedButtons[tracker] % 3);
            equippedSkills[tracker + switchChar * (numberOfSkillTypes + 2), 1] = selectedButtons[tracker] % 3;
            equipImage.sprite = skillButtons[selectedButtons[tracker]].gameObject.GetComponent<Image>().sprite;
        }
        else if (newButton == numSkillTypes + 2)    // If the upgrade button was clicked on
        {
            // Max stat cap at level 5. Upgrading 1 skill upgrades ALL OF THAT SKILL'S VARIANTS. ASSUME: Only the base skill [0] can be upgraded
            if (skills[tracker][selectedButtons[tracker] / 3 + switchChar * skills[tracker].Length / totalChar].level[0] < 5)
            {
                skills[tracker][selectedButtons[tracker] / 3 + switchChar * skills[tracker].Length / totalChar].level[0]++;
                skills[tracker][selectedButtons[tracker] / 3 + switchChar * skills[tracker].Length / totalChar].level[1]++;
                skills[tracker][selectedButtons[tracker] / 3 + switchChar * skills[tracker].Length / totalChar].level[2]++;

                if (skills[tracker][selectedButtons[tracker] / 3 + switchChar * skills[tracker].Length / totalChar].level[0] >= 5)
                {
                    skillButtons[selectedButtons[tracker] + 1].SetText(SkillDescriptions.ReturnDescription(skills[tracker][switchChar * skills[tracker].Length / 2 + selectedButtons[tracker] / 3], 1));
                    skillButtons[selectedButtons[tracker] + 2].SetText(SkillDescriptions.ReturnDescription(skills[tracker][switchChar * skills[tracker].Length / 2 + selectedButtons[tracker] / 3], 2));

                    skillButtons[selectedButtons[tracker] + 1].GetComponent<Image>().sprite = skillImage[tracker][switchChar * skills[tracker].Length / totalChar + selectedButtons[tracker] / 3][1];
                    skillButtons[selectedButtons[tracker] + 2].GetComponent<Image>().sprite = skillImage[tracker][switchChar * skills[tracker].Length / totalChar + selectedButtons[tracker] / 3][2];

                    upgradeButton.enabled = false;
                    upgradeButton.gameObject.GetComponent<Button>().enabled = false;
                    upgradeButton.gameObject.GetComponent<Image>().enabled = false;
                }
            }
            skillButtons[selectedButtons[tracker]].SetText(SkillDescriptions.ReturnDescription(skills[tracker][selectedButtons[tracker] / 3 + switchChar * skills[tracker].Length / totalChar]));
            descriptions.SetText(skillButtons[selectedButtons[tracker]].GetText());
        }
        else    // Otherwise it would have been a tab
        {
            tracker = newButton;
            TreeArrangement(skills[newButton]);
            descriptions.SetText("");
            equipButton.gameObject.GetComponent<Button>().enabled = false;
            equipButton.gameObject.GetComponent<Image>().enabled = false;
            equipButton.enabled = false;
            equipImage.sprite = skillButtons[equippedSkills[tracker + switchChar * numberOfSkillTypes, 0]].gameObject.GetComponent<Image>().sprite;

            upgradeButton.enabled = false;
            upgradeButton.gameObject.GetComponent<Button>().enabled = false;
            upgradeButton.gameObject.GetComponent<Image>().enabled = false;
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
        TreeArrangement(skills[tracker]);

        descriptions.SetText("");
        equipButton.gameObject.GetComponent<Button>().enabled = false;
        equipButton.gameObject.GetComponent<Image>().enabled = false;
        equipButton.enabled = false;

        upgradeButton.enabled = false;
        upgradeButton.gameObject.GetComponent<Button>().enabled = false;
        upgradeButton.gameObject.GetComponent<Image>().enabled = false;

        equipImage.sprite = skillButtons[equippedSkills[tracker + switchChar * numberOfSkillTypes, 0]].gameObject.GetComponent<Image>().sprite;
    }
}
