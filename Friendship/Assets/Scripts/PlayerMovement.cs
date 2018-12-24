using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    float speed = 0.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxis("Horizontal") == 1)
            transform.position += speed * Vector3.right;
        else if (Input.GetAxis("Horizontal") == -1)
            transform.position -= speed * Vector3.right;

        if ((Input.GetAxis("Vertical") == 1) && (gameObject.GetComponent<Rigidbody2D>().velocity == Vector2.zero))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 250f));
            transform.position += speed * Vector3.up;
        }
   //     else if (Input.GetAxis("Vertical") == -1)
        //    transform.position -= speed * Vector3.up;
	}
}
