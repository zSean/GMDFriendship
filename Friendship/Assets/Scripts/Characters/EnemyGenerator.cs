using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Likely a temporary class. Testing purposes
public class EnemyGenerator : MonoBehaviour {

    public GameObject enemy;
    float spawnTimer = 0f;
	
	// Update is called once per frame
	void Update () {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            Instantiate(enemy, transform.position, transform.rotation);
            spawnTimer = 3f;
        }
	}
}
