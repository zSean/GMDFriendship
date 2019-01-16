using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharDescriptionsMenu : MenuHandler {

    string[] characterText;
    //GameObject description;
    Text descriptionText;
    Font font;
    int numCharacters;
    int characterSelect;    // Bool works better, but saving up for if > 2 characters. 0 = Frea, 1 = Luna

    public override void Init(MenuHandler parent)
    {
        numCharacters = 2;
        characterSelect = 0;
        this.parent = parent;
        characterText = new string[numCharacters];
        font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        // Create the text
        //description = new GameObject();
        descriptionText = gameObject.AddComponent<Text>();
        //descriptionText = description.AddComponent<Text>();
        descriptionText.font = font;
        descriptionText.fontSize = 10;
        descriptionText.alignment = TextAnchor.UpperLeft;

        // Will probably load description from a text file
        characterText[0] = "Hai! I'm known as the Black Devil! But don't call me that, ok?";
        characterText[1] = "Hello. I am the Raven.";
    }

    public override void ChangeMenu(int newButton)
    {
        if (newButton < numCharacters)
        {
            characterSelect = newButton;
            descriptionText.text = characterText[characterSelect];
        }
        return;
    }

    public override void Disable(bool enable)
    {
        descriptionText.enabled = !enable;
    }

    public void ModifyTextBoxSize(float textWidth, float textHeight)
    {
        descriptionText.rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
    }

    public override void SwitchChar(int switchChar)
    {
        ChangeMenu(switchChar);
    }
}
