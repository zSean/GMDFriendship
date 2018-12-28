using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTreeMenu : MenuHandler {

    int numSkillTypes = 4;
    MenuButton[] skillTypes;
    Sprite[] skillTypeImage;
    Font font;

    public override void Init(MenuHandler parent)
    {
        font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        skillTypeImage = Resources.LoadAll<Sprite>("SkillTabs");
        skillTypes = new MenuButton[numSkillTypes];

        string[] skillHeader = {"Aerial", "Flurry", "Buff", "Perk"};
        numSkillTypes = skillHeader.Length;

        for(int i = 0; i < numSkillTypes; i++)
        {
            // Create the button
            GameObject skillButtons = new GameObject();
            skillButtons.transform.SetParent(gameObject.transform);
            skillButtons.AddComponent<Button>();
            skillTypes[i] = skillButtons.AddComponent<MenuButton>();
            skillTypes[i].Init(i, this);
            skillTypes[i].SetText(skillHeader[i], font, 10, TextAnchor.MiddleCenter);
            skillButtons.AddComponent<Image>().sprite = skillTypeImage[i];
            skillButtons.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(skillButtons.GetComponent<Image>().sprite.rect.width, skillButtons.GetComponent<Image>().sprite.rect.height / 1.5f);
            skillTypes[i].SetTextboxSize(skillButtons.GetComponent<Image>().rectTransform.sizeDelta);
        }
    }

    public override void ChangeMenu(int newButton)
    {
        return;
    }

    public override void Disable(bool enable)
    {
        for(int i = 0; i < numSkillTypes; i++)
        {
            skillTypes[i].gameObject.GetComponent<Button>().enabled = !enable;
            skillTypes[i].gameObject.GetComponent<Image>().enabled = !enable;
            skillTypes[i].enabled = !enable;
        }
    }

    public void SetStartPoint(Vector2 start, float offset)
    {
        for(int i = 0; i < numSkillTypes; i++)
        {
            skillTypes[i].gameObject.transform.position = start - new Vector2(skillTypes[i].gameObject.GetComponent<Image>().sprite.rect.width / 2, offset * i);
        }
    }
}
