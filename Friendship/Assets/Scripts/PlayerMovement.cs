using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Set collider to the character's feet only
public class PlayerMovement : MonoBehaviour {

    float hSpeed = 15f;  // Horizontal movement speed

    int onGround = 0;   // If the player is on the ground
    int maxJumps = 1;   // How many jumps the player may make
    int jumpsRemaining = 0; // How many jumps the player has left
    bool jumpHeld = false;  // Whether the player is holding down the jump button or has released it
    float jumpSpeed = 7.5f;   // Vertical speed of jumps
    float jumpDuration = 0.2f;  // How long the player can hold a jump for
    float currentJumpDuration = 0f; // How long the player has held the jump for

    Rigidbody2D rb;
	
    // Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame. Physics
	void FixedUpdate () {

        // General movement horizontally and vertically, can jump multiple times depending on maxJumps
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * hSpeed, rb.velocity.y);

        if((Input.GetAxisRaw("Vertical") == 1))
        {
            if (currentJumpDuration > 0f)
            {
                currentJumpDuration -= Time.deltaTime;  // Change to fit time
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpHeld = true;
            }
            else if((!jumpHeld) && (jumpsRemaining - onGround > 0))
            {
                jumpsRemaining -= 1;
                currentJumpDuration = jumpDuration;
            }
        }
        else if((Input.GetAxisRaw("Vertical") != 1) && (jumpHeld))
        {
            jumpHeld = false;
            currentJumpDuration = 0;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Untagged")
        {
            onGround = 0;
            jumpsRemaining = maxJumps;
        }
    }

    public void ResetJumps()
    {
        currentJumpDuration = jumpDuration;
        jumpsRemaining = maxJumps;
    }

    // Getters and Setters
    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = 1;
    }
    public void SethSpeed(float speed)
    {
        hSpeed = speed;
    }
    public float GethSpeed()
    {
        return hSpeed;
    }
    public void SetMaxJumps(int maxNum)
    {
        maxJumps = maxNum;
    }
    public int GetMaxJumps()
    {
        return maxJumps;
    }
}
