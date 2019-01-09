using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour {

    float lifeTimer = 5f;   // Destroy object after a point. Mostly testing
    bool autoMove = true;

    Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
        if (autoMove)
            rb.velocity = StandardLevel.sMoveSpeed * StandardLevel.speedModifier;
    }

    public void SetAutoMove(bool setMove)
    {
        autoMove = setMove;
    }
}
