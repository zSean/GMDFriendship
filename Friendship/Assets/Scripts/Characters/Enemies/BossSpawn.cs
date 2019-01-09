using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour {

    Enemy boss;

	// Use this for initialization
	public virtual GameObject Init ()
    {
        GameObject bossObject = new GameObject();
        boss = bossObject.AddComponent<Enemy>();
        boss.Init(30);
        bossObject.transform.position = new Vector3(Camera.main.transform.position.x + 4 * Camera.main.orthographicSize, Camera.main.transform.position.y - Camera.main.orthographicSize / 2);
        return bossObject;
	}
}
