using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Revisit the HUD to make adjustments once assets are modified/added
public class LevelHUD : MonoBehaviour {

    int characterActive = 0;    // 0 = Frea, 1 = Luna, -1 = GameOver???
    LevelGenerator parent;  // Needs to draw data from somewhere

    // Canvas
    Canvas canvas;  // Canvas on which the UI skills will be placed
    RectTransform canvasPosition;   // Required for setting buttons relative to the canvas
    private GameObject parentCanv;  // The gameobject that the canvas will attach to. Cannot create canvas directly

    // Pause button. WIP
    MenuButton pauseButton;

    // Images galore
    Image[] characterPortraits; // Character portraits, top left of screen. Active portrait is larger
    static Sprite[] dCharacterPortraits;
    Image[] healthBar;  // Healthbar displayed for the characters
    Text[] healthText; // Displays health

    GameObject manaGaugeAnchor;
    Image manaBar;  // Mana bar, bottom left of screen
    Image manaGauge;    // The actual mana image
    float manaGaugeStandardSize;

    Image pointsImage;  // Displays image box containing points, beside pause button on top right
    Text points;    // Displays points

    Image chargeImage;  // Displays the charge gauge
    Text chargeGaugeText;   // Displays the % of charge

    // Default sprite and font
    Sprite defaultSprite;
    Font font;

    // Used to detect mouse event of clicking pause button
    private EventSystem eventSystem;
    private GraphicRaycaster raycaster;
    
    // Use this for initialization
    void Start () {
        dCharacterPortraits = new Sprite[2];
        dCharacterPortraits[0] = Resources.Load("LevelHUD/CharPortrait1", typeof(Sprite)) as Sprite;
        dCharacterPortraits[1] = Resources.Load("LevelHUD/CharPortrait2", typeof(Sprite)) as Sprite;

        defaultSprite = Resources.Load("LevelHUD/ManaBar", typeof(Sprite)) as Sprite;
        font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        parentCanv = new GameObject();
        canvas = parentCanv.AddComponent<Canvas>();
        canvasPosition = parentCanv.GetComponent<RectTransform>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        raycaster = parentCanv.AddComponent<GraphicRaycaster>();

        // Create the pause button. No button component?
        GameObject pauseButtonObject = new GameObject();
        pauseButtonObject.AddComponent<Image>();
        pauseButtonObject.GetComponent<Image>().sprite = Resources.Load("BlockColours", typeof(Sprite)) as Sprite;
        pauseButtonObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(canvasPosition.rect.width / 10, canvasPosition.rect.height / 10);
        pauseButton = pauseButtonObject.AddComponent<MenuButton>();
        pauseButtonObject.transform.position = new Vector3(canvasPosition.rect.width - pauseButtonObject.GetComponent<Image>().rectTransform.rect.width / 2, canvasPosition.rect.height - pauseButtonObject.GetComponent<Image>().rectTransform.rect.height / 2);
        pauseButtonObject.transform.SetParent(parentCanv.transform);

        // Create the character portraits
        characterPortraits = new Image[2];
        healthBar = new Image[2];
        healthText = new Text[2];
        for(int i = 0; i < characterPortraits.Length; i++)
        {
            GameObject characterPortraitObject = new GameObject();
            characterPortraits[i] = characterPortraitObject.AddComponent<Image>();
            characterPortraits[i].sprite = dCharacterPortraits[i];
            characterPortraits[i].rectTransform.sizeDelta = new Vector2(canvasPosition.rect.width / (8 + 2 * i) , canvasPosition.rect.height * 16 / ((8 + 2 * i) * 9));   // 16:9 aspect ratio. May turn into variable
            characterPortraitObject.transform.position = new Vector3(characterPortraits[i].rectTransform.rect.width / 2 + canvasPosition.transform.position.x - canvasPosition.rect.width / 2, canvasPosition.transform.position.y + canvasPosition.rect.height / 2 - (canvasPosition.rect.height * i) / 5 - characterPortraits[i].rectTransform.rect.height / 2);
            characterPortraitObject.transform.SetParent(parentCanv.transform);

            GameObject healthBarObject = new GameObject();
            healthBar[i] = healthBarObject.AddComponent<Image>();
            healthBar[i].sprite = Resources.Load("LevelHUD/HealthBar", typeof(Sprite)) as Sprite;
            healthBar[i].rectTransform.sizeDelta = new Vector2(characterPortraits[i].rectTransform.sizeDelta.x, characterPortraits[i].rectTransform.sizeDelta.y / 3);
            healthBarObject.transform.position = characterPortraitObject.transform.position + Vector3.right * ((healthBar[i].rectTransform.rect.width + characterPortraits[i].rectTransform.rect.width) / 2);
            healthBarObject.transform.SetParent(characterPortraitObject.transform);

            GameObject healthTextObject = new GameObject();
            healthText[i] = healthTextObject.AddComponent<Text>();
            healthText[i].font = font;
            healthText[i].fontSize = 10;
            healthText[i].text = "Watashi wa numbah wan!";
            healthText[i].rectTransform.sizeDelta = healthBar[i].rectTransform.sizeDelta;
            healthTextObject.transform.position = healthBarObject.transform.position;
            healthTextObject.transform.SetParent(healthBarObject.transform);
        }

        // Mana bar
        GameObject manaGaugeObject = new GameObject();
        manaGauge = manaGaugeObject.AddComponent<Image>();
        manaGauge.sprite = Resources.Load("LevelHUD/Mana", typeof(Sprite)) as Sprite;
        manaGaugeStandardSize = canvasPosition.rect.width / 4;
        manaGauge.rectTransform.sizeDelta = new Vector2(manaGaugeStandardSize, 8f);
        manaGaugeObject.transform.position = new Vector3(manaGauge.rectTransform.rect.width / 2, canvasPosition.rect.height / 5);

        manaGaugeAnchor = new GameObject();
        manaGaugeAnchor.transform.position = new Vector3(manaGauge.transform.position.x - manaGauge.rectTransform.rect.width / 2, manaGauge.transform.position.y);
        manaGaugeAnchor.transform.SetParent(parentCanv.transform);
        manaGaugeObject.transform.SetParent(manaGaugeAnchor.transform);

        GameObject manaBarObject = new GameObject();
        manaBar = manaBarObject.AddComponent<Image>();
        manaBar.sprite = Resources.Load("LevelHUD/ManaBar", typeof(Sprite)) as Sprite;
        manaBar.rectTransform.sizeDelta = manaGauge.rectTransform.sizeDelta;
        manaBarObject.transform.position = manaGaugeObject.transform.position;
        //manaBarObject.transform.SetParent(manaGaugeObject.transform);
        manaBarObject.transform.SetParent(parentCanv.transform);

        // Points
        GameObject pointsObject = new GameObject();
        pointsImage = pointsObject.AddComponent<Image>();
        pointsImage.sprite = Resources.Load("ButtonTabs", typeof(Sprite)) as Sprite;
        pointsImage.rectTransform.sizeDelta = pauseButtonObject.GetComponent<Image>().rectTransform.sizeDelta;
        pointsObject.transform.position = new Vector3(canvasPosition.rect.width - pointsImage.rectTransform.rect.width / 2- pauseButtonObject.GetComponent<Image>().rectTransform.rect.width / 2, canvasPosition.rect.height - pointsImage.rectTransform.rect.height / 2);
        pointsObject.transform.SetParent(parentCanv.transform);

        GameObject pointsTextObject = new GameObject();
        points = pointsTextObject.AddComponent<Text>();
        points.font = font;
        points.fontSize = 10;
        points.text = "1337";
        points.rectTransform.sizeDelta = pointsImage.rectTransform.sizeDelta;
        pointsTextObject.transform.position = pointsObject.transform.position;
        pointsTextObject.transform.SetParent(pointsObject.transform);

        // Charge
        GameObject chargeObject = new GameObject();
        chargeImage = chargeObject.AddComponent<Image>();
        chargeImage.sprite = Resources.Load("LevelHUD/ChargeMeter", typeof(Sprite)) as Sprite;
        chargeObject.transform.position = new Vector3(canvasPosition.transform.position.x, canvasPosition.rect.height);
        chargeObject.transform.SetParent(parentCanv.transform);

        GameObject chargeTextObject = new GameObject();
        chargeGaugeText = chargeTextObject.AddComponent<Text>();
        chargeGaugeText.text = "42";
        chargeTextObject.transform.position = chargeObject.transform.position;
        chargeTextObject.transform.SetParent(chargeObject.transform);
    }

    public void SetLevelGenerator(LevelGenerator parent)
    {
        this.parent = parent;
    }
	
	// Update is called once per frame
	void Update () {
        manaGaugeAnchor.transform.localScale = new Vector3((float)parent.GetMana() / parent.GetMaxMana(), 1f);
        //manaGauge.rectTransform.sizeDelta = new Vector2(manaGaugeStandardSize * parent.GetMana() / parent.GetMaxMana(), manaGauge.rectTransform.sizeDelta.y);
	}
}