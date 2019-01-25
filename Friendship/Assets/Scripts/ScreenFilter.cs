using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFilter : MonoBehaviour {

    SpriteRenderer filter;
    float maxFilterTimer = 0.3f;
    float currentFilterTimer = 0f;
    bool trigger = false;

    public void SetMaxFilterTimer(float newTimer)
    {
        maxFilterTimer = newTimer;
    }
    public void SetTrigger()
    {
        trigger = true;
        currentFilterTimer = maxFilterTimer;
    }
	// Use this for initialization
	void Start () {
        filter = gameObject.GetComponent<SpriteRenderer>();
        filter.color = new Color(filter.color.r, filter.color.g, filter.color.b, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(trigger)
        {
            currentFilterTimer -= Time.deltaTime;

            if(currentFilterTimer <= 0)
            {
                currentFilterTimer = 0;
                trigger = false;
            }
            filter.color = new Color(filter.color.r, filter.color.g, filter.color.b, currentFilterTimer/maxFilterTimer);
        }
	}
}
