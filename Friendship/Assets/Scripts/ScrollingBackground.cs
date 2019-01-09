using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    public float scrollSpeed = 0.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 offset = new Vector2(Time.deltaTime * scrollSpeed * StandardLevel.speedModifier, 0f);

        gameObject.GetComponent<Renderer>().material.mainTextureOffset += offset;
	}
}
