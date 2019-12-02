using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritFoxScript : MonoBehaviour {

    float lifeSpan = 5f;
    float maxLife = 5f;
    float limit = 15f;
    bool isActive = false;

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<PlayerMovement>();
        gameObject.layer = 9;
        Physics2D.IgnoreLayerCollision(9, 0, true);
        Activate(false);
	}

    private void Update()
    {
        lifeSpan -= Time.deltaTime;
        if ((isActive) && (lifeSpan <= 0))
        {
            maxLife = 5f;
            Activate(false);
        }
    }

    public void Activate(bool activate)
    {
        isActive = activate;
        gameObject.GetComponent<SpriteRenderer>().enabled = activate;
        gameObject.GetComponent<PlayerMovement>().enabled = activate;
        if (!activate)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.transform.position = Vector3.left * 900; // Some arbitrary value offscreen
        }
        else
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void ExtendMaxLife(float max)
    {
        if (maxLife + max > limit)
            maxLife = limit - max;
        maxLife += max;
        lifeSpan = maxLife;
    }

    public void SetLimit(float setLimit)
    {
        limit = setLimit;
    }

    public bool GetActive()
    {
        return isActive;
    }
}
