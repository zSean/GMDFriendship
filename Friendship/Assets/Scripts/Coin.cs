using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    LevelGenerator reference;

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<ScrollingObject>();
        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
	}

    public void GetCoin()
    {
        reference.AddPoints(3);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            GetCoin();
    }
}
