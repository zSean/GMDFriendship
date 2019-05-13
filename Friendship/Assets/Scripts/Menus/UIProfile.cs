using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIProfile : MenuHandler
{

    int characterSelect = 0; // Bool works, but may want to add more char. 0 = Frea, 1 = Luna

    // Canvas
    Canvas canvas;  // Canvas on which the UI skills will be placed
    RectTransform canvasPosition;   // Required for setting buttons relative to the canvas
    private GameObject parentCanv;  // The gameobject that the canvas will attach to. Cannot create canvas directly

    // Panel. Use GameObject

    // Description box
    Image descriptionBox;
    GameObject descriptionBoxObject;

    // Buttons
    int numProfileOptions = 3;
    MenuButton[] profileOptions;
    Image[] profileOptionsImage;
    int selectedButton = 0;

    // Default sprite
    Sprite defaultSprite;
    // May have to make a static class containing all the sprite skills that can be assigned to buttons???

    // Load up the submenus
    MenuHandler[] submenus;
    int submenuCount;

    UICharDescriptionsMenu charDescriptions;    // UI for character descriptions
    UIStatMenu statMenu;    // UI for stat menu
    UISkillTreeMenu skillTreeMenu;  // UI for skill tree menu

    // Used to detect mouse events when hovering over the skill icons (tooltip display)
    private EventSystem eventSystem;
    private GraphicRaycaster rayCaster;

    private void Start()
    {
    }

    public override void Init(MenuHandler parent)
    {
        return;
    }

    public void Init(MenuHandler parent, Dictionary<int, SkillProperties[]> loadSkills, SkillProperties[] equippedSkills)
    {
        // Initialize the defaults
        defaultSprite = Resources.Load("ButtonTabs", typeof(Sprite)) as Sprite;
        Font font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        // Initialize profile options
        profileOptions = new MenuButton[numProfileOptions];
        profileOptionsImage = new Image[numProfileOptions];

        // Create the canvas
        parentCanv = new GameObject();
        canvas = parentCanv.AddComponent<Canvas>();
        canvasPosition = canvas.GetComponent<RectTransform>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;  // Set the canvas to cover the viewable screen
        rayCaster = parentCanv.AddComponent<GraphicRaycaster>();    // Allow the mouse position to be translated to canvas

        descriptionBoxObject = new GameObject();
        descriptionBox = descriptionBoxObject.AddComponent<Image>();
        descriptionBox.sprite = defaultSprite;
        float descriptionBoxWidth = descriptionBox.sprite.rect.width * 6;        // Scaling. Hardcoded
        float descriptionBoxHeight = descriptionBox.sprite.rect.height * 9;
        descriptionBoxObject.transform.SetParent(canvas.transform);
        descriptionBox.rectTransform.sizeDelta = new Vector2(descriptionBoxWidth, descriptionBoxHeight);
        descriptionBoxObject.transform.position = new Vector3(canvasPosition.rect.width - (descriptionBoxWidth / 2), descriptionBoxHeight / 2, 0f);

        string[] profileOptionsHeader = { "Bio", "Stats", "Skills" };

        // Start adding in all the options
        for (int i = 0; i < numProfileOptions; i++)
        {
            // Create the button
            GameObject button = new GameObject();
            button.AddComponent<Button>();
            profileOptions[i] = button.AddComponent<MenuButton>();
            profileOptions[i].Init(i, this);
            profileOptions[i].SetTextProperties(profileOptionsHeader[i], font, 10, TextAnchor.MiddleCenter);
            button.transform.SetParent(canvas.transform);
            profileOptionsImage[i] = button.AddComponent<Image>();
            profileOptionsImage[i].sprite = defaultSprite;
            profileOptionsImage[i].rectTransform.sizeDelta = new Vector2(profileOptionsImage[i].sprite.rect.width, profileOptionsImage[i].sprite.rect.height);
            button.transform.position = new Vector3(descriptionBox.transform.position.x - (descriptionBoxWidth / 2) + (profileOptionsImage[i].sprite.rect.width * (i + 0.5f)), descriptionBox.transform.position.y + (descriptionBoxHeight / 2) + (profileOptionsImage[i].sprite.rect.height / 2));
            profileOptions[i].SetTextboxSize(profileOptionsImage[i].rectTransform.sizeDelta);
        }


        // Create the submenus (3)
        submenuCount = 3;
        submenus = new MenuHandler[submenuCount];

        // Character bio submenu
        GameObject descriptionObject = new GameObject();
        descriptionObject.transform.position = descriptionBox.transform.position;
        descriptionObject.transform.SetParent(canvas.transform);
        charDescriptions = descriptionObject.AddComponent<UICharDescriptionsMenu>();
        charDescriptions.Init(this);
        charDescriptions.ModifyTextBoxSize(descriptionBox.rectTransform.sizeDelta.x, descriptionBox.rectTransform.sizeDelta.y);
        charDescriptions.ChangeMenu(characterSelect);
        submenus[0] = charDescriptions;

        // Stats submenu
        GameObject statMenuObject = new GameObject();
        statMenuObject.transform.position = descriptionBox.transform.position;
        statMenuObject.transform.SetParent(canvas.transform);
        statMenu = statMenuObject.AddComponent<UIStatMenu>();
        statMenu.Init(this);
        statMenu.SetStartPoint(new Vector2(descriptionBox.transform.position.x - (descriptionBoxWidth / 3), descriptionBox.transform.position.y + descriptionBoxHeight * 5 / 12), 30f);
        statMenu.ChangeMenu(characterSelect);
        statMenu.Disable(true);
        submenus[1] = statMenu;

        // Skills submenu
        GameObject skillTreeObject = new GameObject();
        skillTreeObject.transform.position = descriptionBox.transform.position;
        skillTreeObject.transform.SetParent(canvas.transform);
        skillTreeMenu = skillTreeObject.AddComponent<UISkillTreeMenu>();

        skillTreeMenu.Init(this, loadSkills, equippedSkills);
        skillTreeMenu.SetStartPoint(new Vector2(descriptionBox.transform.position.x - (descriptionBoxWidth / 2), descriptionBox.transform.position.y + descriptionBoxHeight * 5 / 12), 30f);
        skillTreeMenu.ChangeMenu(characterSelect);
        skillTreeMenu.Disable(true);
        submenus[2] = skillTreeMenu;

        // Create the event system so that the UI can detect if the mouse is hovering over it
        GameObject eventSys = new GameObject();
        eventSystem = eventSys.AddComponent<EventSystem>();
        eventSys.transform.SetParent(canvas.transform, true);
        eventSys.AddComponent<StandaloneInputModule>();
    }

    public override void ChangeMenu(int newButton)
    {
        if (newButton != selectedButton)
        {
            if (submenus[selectedButton] != null)
                submenus[selectedButton].Disable(true);

            selectedButton = newButton;

            // Change selectedButton depending on which one
            switch (selectedButton)
            {
                case 0:
                    charDescriptions.Disable(false);
                    charDescriptions.ChangeMenu(characterSelect);
                    break;
                case 1:
                    statMenu.Disable(false);
                    statMenu.ChangeMenu(characterSelect);
                    break;
                case 2:
                    skillTreeMenu.Disable(false);
                    skillTreeMenu.ChangeMenu(characterSelect);
                    break;
            }
        }
    }

    public override void Disable(bool enable)
    {
        return;
    }
}


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIProfile : MenuHandler
{

    int characterSelect = 0; // Bool works, but may want to add more char. 0 = Frea, 1 = Luna

    // Canvas
    Canvas canvas;  // Canvas on which the UI skills will be placed
    RectTransform canvasPosition;   // Required for setting buttons relative to the canvas
    private GameObject parentCanv;  // The gameobject that the canvas will attach to. Cannot create canvas directly

    // Panel. Use GameObject

    // Description box
    Image descriptionBox;
    GameObject descriptionBoxObject;

    // Buttons
    int numProfileOptions = 3;
    MenuButton[] profileOptions;
    Image[] profileOptionsImage;
    int selectedButton = 0;

    // Default sprite
    Sprite defaultSprite;
    // May have to make a static class containing all the sprite skills that can be assigned to buttons???

    // Load up the submenus
    MenuHandler[] submenus;
    int submenuCount;

    UICharDescriptionsMenu charDescriptions;    // UI for character descriptions
    UIStatMenu statMenu;    // UI for stat menu
    UISkillTreeMenu skillTreeMenu;  // UI for skill tree menu

    // Used to detect mouse events when hovering over the skill icons (tooltip display)
    private EventSystem eventSystem;
    private GraphicRaycaster rayCaster;

    private void Start()
    {
    }

    public override void Init(MenuHandler parent)
    {
        return;
    }

    public void Init(MenuHandler parent, Dictionary<int, SkillProperties[]> loadSkills, int[] equippedSkillsIndex)
    {
        // Initialize the defaults
        defaultSprite = Resources.Load("ButtonTabs", typeof(Sprite)) as Sprite;
        Font font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        // Initialize profile options
        profileOptions = new MenuButton[numProfileOptions];
        profileOptionsImage = new Image[numProfileOptions];

        // Create the canvas
        parentCanv = new GameObject();
        canvas = parentCanv.AddComponent<Canvas>();
        canvasPosition = canvas.GetComponent<RectTransform>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;  // Set the canvas to cover the viewable screen
        rayCaster = parentCanv.AddComponent<GraphicRaycaster>();    // Allow the mouse position to be translated to canvas

        descriptionBoxObject = new GameObject();
        descriptionBox = descriptionBoxObject.AddComponent<Image>();
        descriptionBox.sprite = defaultSprite;
        float descriptionBoxWidth = descriptionBox.sprite.rect.width * 6;        // Scaling. Hardcoded
        float descriptionBoxHeight = descriptionBox.sprite.rect.height * 9;
        descriptionBoxObject.transform.SetParent(canvas.transform);
        descriptionBox.rectTransform.sizeDelta = new Vector2(descriptionBoxWidth, descriptionBoxHeight);
        descriptionBoxObject.transform.position = new Vector3(canvasPosition.rect.width - (descriptionBoxWidth / 2), descriptionBoxHeight / 2, 0f);

        string[] profileOptionsHeader = { "Bio", "Stats", "Skills" };

        // Start adding in all the options
        for (int i = 0; i < numProfileOptions; i++)
        {
            // Create the button
            GameObject button = new GameObject();
            button.AddComponent<Button>();
            profileOptions[i] = button.AddComponent<MenuButton>();
            profileOptions[i].Init(i, this);
            profileOptions[i].SetText(profileOptionsHeader[i], font, 10, TextAnchor.MiddleCenter);
            button.transform.SetParent(canvas.transform);
            profileOptionsImage[i] = button.AddComponent<Image>();
            profileOptionsImage[i].sprite = defaultSprite;
            profileOptionsImage[i].rectTransform.sizeDelta = new Vector2(profileOptionsImage[i].sprite.rect.width, profileOptionsImage[i].sprite.rect.height);
            button.transform.position = new Vector3(descriptionBox.transform.position.x - (descriptionBoxWidth / 2) + (profileOptionsImage[i].sprite.rect.width * (i + 0.5f)), descriptionBox.transform.position.y + (descriptionBoxHeight / 2) + (profileOptionsImage[i].sprite.rect.height / 2));
            profileOptions[i].SetTextboxSize(profileOptionsImage[i].rectTransform.sizeDelta);
        }


        // Create the submenus (3)
        submenuCount = 3;
        submenus = new MenuHandler[submenuCount];

        // Character bio submenu
        GameObject descriptionObject = new GameObject();
        descriptionObject.transform.position = descriptionBox.transform.position;
        descriptionObject.transform.SetParent(canvas.transform);
        charDescriptions = descriptionObject.AddComponent<UICharDescriptionsMenu>();
        charDescriptions.Init(this);
        charDescriptions.ModifyTextBoxSize(descriptionBox.rectTransform.sizeDelta.x, descriptionBox.rectTransform.sizeDelta.y);
        charDescriptions.ChangeMenu(characterSelect);
        submenus[0] = charDescriptions;

        // Stats submenu
        GameObject statMenuObject = new GameObject();
        statMenuObject.transform.position = descriptionBox.transform.position;
        statMenuObject.transform.SetParent(canvas.transform);
        statMenu = statMenuObject.AddComponent<UIStatMenu>();
        statMenu.Init(this);
        statMenu.SetStartPoint(new Vector2(descriptionBox.transform.position.x - (descriptionBoxWidth / 3), descriptionBox.transform.position.y + descriptionBoxHeight * 5 / 12), 30f);
        statMenu.ChangeMenu(characterSelect);
        statMenu.Disable(true);
        submenus[1] = statMenu;

        // Skills submenu
        GameObject skillTreeObject = new GameObject();
        skillTreeObject.transform.position = descriptionBox.transform.position;
        skillTreeObject.transform.SetParent(canvas.transform);
        skillTreeMenu = skillTreeObject.AddComponent<UISkillTreeMenu>();
        // Conversion of SkillAssigner to int via mapping (intermediary tables). Maybe a tree would work?
        foreach (KeyValuePair<int, SkillAssigner.SkillNames[]> map in skillMapping)
        {
            List<int> concat = new List<int>();

            for (int i = 0; i < map.Value.Length; i++)
            {
                concat.AddRange(loadSkills[map.Value[i]]);
            }
            skillTreeMenu.LoadStats(map.Key, concat.ToArray());
        }
        skillTreeMenu.Init(this);
        skillTreeMenu.SetStartPoint(new Vector2(descriptionBox.transform.position.x - (descriptionBoxWidth / 2), descriptionBox.transform.position.y + descriptionBoxHeight * 5 / 12), 30f);
        skillTreeMenu.ChangeMenu(characterSelect);
        skillTreeMenu.Disable(true);
        submenus[2] = skillTreeMenu;

        // Create the event system so that the UI can detect if the mouse is hovering over it
        GameObject eventSys = new GameObject();
        eventSystem = eventSys.AddComponent<EventSystem>();
        eventSys.transform.SetParent(canvas.transform, true);
        eventSys.AddComponent<StandaloneInputModule>();
    }

    public override void ChangeMenu(int newButton)
    {
        if (newButton != selectedButton)
        {
            if (submenus[selectedButton] != null)
                submenus[selectedButton].Disable(true);

            selectedButton = newButton;

            // Change selectedButton depending on which one
            switch (selectedButton)
            {
                case 0:
                    charDescriptions.Disable(false);
                    charDescriptions.ChangeMenu(characterSelect);
                    break;
                case 1:
                    statMenu.Disable(false);
                    statMenu.ChangeMenu(characterSelect);
                    break;
                case 2:
                    skillTreeMenu.Disable(false);
                    skillTreeMenu.ChangeMenu(characterSelect);
                    break;
            }
        }
    }

    public override void Disable(bool enable)
    {
        return;
    }
}
*/
