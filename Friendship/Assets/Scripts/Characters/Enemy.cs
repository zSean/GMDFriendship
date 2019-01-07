using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For standard level stats/physics
public static class StandardLevel{
    public static Vector2 sMoveSpeed = new Vector2(-10f, 0f);   // Move to the left
    public static float speedModifier = 1;  // Scale of moving
}

public class Enemy : CharacterStats {

    bool autoMove = true;
    bool onPlatform = false;    // On moving platform
    Vector2 moveSpeed = StandardLevel.sMoveSpeed;

    Rigidbody2D rb;
    ScrollingObject defaultScrolling;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        defaultScrolling = gameObject.AddComponent<ScrollingObject>();
        defaultScrolling.SetAutoMove(autoMove);
    }

    // Update is called once per frame. Physics
    void FixedUpdate()
    {
    }

    public void SetOnPlatform(bool onPlatform)
    {
        autoMove = onPlatform;
        defaultScrolling.SetAutoMove(autoMove);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }
}
