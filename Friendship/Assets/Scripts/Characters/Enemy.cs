using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For standard level stats/physics
public static class StandardLevel{
    public static Vector2 sMoveSpeed = new Vector2(-10f, 0f);   // Move to the left
}

public class Enemy : CharacterStats {

    bool autoMove = true;
    bool onPlatform = false;    // On moving platform
    Vector2 moveSpeed = StandardLevel.sMoveSpeed;
    float speedModifier = 1;

    float lifeTimer = 3f;   // Testing purposes only. Please remove after
    Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame. Physics
    void FixedUpdate()
    {
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
        if ((autoMove) && (!onPlatform))
            rb.velocity = moveSpeed * speedModifier;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }
    public void SetAutoMove(bool setMove)
    {
        autoMove = setMove;
    }

    public void SetSpeedModifier(float newModifier)
    {
        speedModifier = newModifier;
        rb.velocity = moveSpeed * speedModifier;
    }
    public void SetVelocity(Vector2 newVelocity)
    {
        moveSpeed = newVelocity;
        rb.velocity = moveSpeed * speedModifier;
    }
    public float GetSpeedModifier()
    {
        return speedModifier;
    }
}
