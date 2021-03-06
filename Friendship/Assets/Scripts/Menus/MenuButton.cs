﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerClickHandler {

    int buttonNum = 0;
    MenuHandler parent;
    GameObject text;
    bool selected = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (parent != null)
            parent.ChangeMenu(buttonNum);
    }

    private void Awake()
    {
        text = new GameObject();
        text.AddComponent<Text>();
        text.transform.position = gameObject.transform.position;
        text.transform.SetParent(gameObject.transform);
    }

    // Use this for initialization
    public void Init (int buttonNum, MenuHandler parent) {
        this.buttonNum = buttonNum;
        this.parent = parent;
    }
    public void SelectButton(bool selected)
    {
        this.selected = selected;
        return;
    }
    public int GetButtonNum()
    {
        return buttonNum;
    }
    public void SetButtonNum(int num)
    {
        buttonNum = num;
    }
    public void SetTextProperties(string text, Font font, int fSize, TextAnchor alignment)
    {
        this.text.GetComponent<Text>().font = font;
        this.text.GetComponent<Text>().text = text;
        this.text.GetComponent<Text>().fontSize = fSize;
        this.text.GetComponent<Text>().alignment = alignment;
    }
    public void SetTextboxSize(Vector2 size)
    {
        text.GetComponent<Text>().rectTransform.sizeDelta = size;
    }

    public void SetText(string text)
    {
        this.text.GetComponent<Text>().text = text;
    }
    public string GetText()
    {
        return text.GetComponent<Text>().text;
    }

    private void OnDisable()
    {
        if(gameObject.GetComponent<Button>() != null)
            gameObject.GetComponent<Button>().enabled = false;
        gameObject.GetComponentInChildren<Text>().enabled = false;
    }

    private void OnEnable()
    {
        if(gameObject.GetComponent<Button>() != null)
            gameObject.GetComponent<Button>().enabled = true;
        text.GetComponent<Text>().enabled = true;
    }
}
