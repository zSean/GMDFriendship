using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : CharacterStats {

    protected bool autoMove = true;
    protected bool onPlatform = false;    // On moving platform
    protected Vector2 moveSpeed = StandardLevel.sMoveSpeed;
    protected int points = 1;

    protected Rigidbody2D rb;
    protected ScrollingObject defaultScrolling;

    // The equivalent of a constructor
    public virtual void Init(int level = 1)
    { 
        // Hardcoding, but only need to consider atk, defence, hp, not the rest
        for(int i = 0; i < 3; i++)
        {
            SetMaxStat(i, charStats.maxCharStats[i] * level);
            currentCharStats[i] = charStats.maxCharStats[i] * level;
        }
    }

    private void Start()
    {
        tag = "Enemy";
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.mass = 50f;
        rb.gravityScale = 8f;
        gameObject.AddComponent<BoxCollider2D>().size = new Vector2(1f, 2f);
        gameObject.GetComponent<BoxCollider2D>().offset = Vector2.zero;
        gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load("CharacterSprites/Enemies/Goblin", typeof(Sprite)) as Sprite;
        defaultScrolling = gameObject.AddComponent<ScrollingObject>();
        defaultScrolling.SetAutoMove(autoMove);
    }

    protected virtual void Update()
    {
        invincibilityTimer -= Time.deltaTime;
    }

    public virtual void SetOnPlatform(bool onPlatform)
    {
        autoMove = onPlatform;
        defaultScrolling.SetAutoMove(autoMove);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }

    protected void OnDestroy()
    {
        if(currentCharStats[1] <= 0)
        {
            LevelGenerator reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
            if (reference != null)
                reference.AddPoints(points);
        }
    }
}
