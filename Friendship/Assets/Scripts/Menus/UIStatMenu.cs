using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatMenu : MenuHandler {

    MenuButton[] statButtons;   // These are just the buttons. Will need something to store the actual stats
    int numStats = 5;   // 5,6 = Frea; 7,8 = Luna
    int specialStats = 2;
    int numCharacters = 2;
    int characterSelect = 0;
    Font font;
    string[] statText = { "Attack", "Health", "Damage Red", "Crit. Chance", "Dodge Chance", "Burn Chance", "Burn Dmg", "Attack Speed", "Crit. Dmg" };

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
        return;
    }

    // WIP. Need stats to be implemented first
    public override void ChangeMenu(int newButton)
    {
        characterSelect = newButton;

        // Print out stats for the selected character
        for (int i = 0; i < numStats; i++)
        {
            statButtons[i].SetTextProperties(statText[i] /*+ Mathf.Max(0, ((i - numStats - specialStats * (characterSelect + 1))))]*/, font, 10, TextAnchor.MiddleLeft);
        }
        return;
    }

    public override void Disable(bool enable)
    {
        for(int i = 0; i < numStats; i++)
        {
            statButtons[i].enabled = !enable;
        }
        return;
    }

    public void SetStartPoint(Vector2 start, float offset)
    {
        for(int i = 0; i < numStats; i++)
        {
            statButtons[i].gameObject.transform.position = start - new Vector2(0f, offset) * i;
        }
    }

    public override void SwitchChar(int switchChar)
    {
        ChangeMenu(switchChar);
    }
}
