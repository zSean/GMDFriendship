using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGargoyle : Enemy {

    protected override void Awake()
    {
        float[] mCharStats = { 3, 8, 1, 0, 0, 1, 1 };
        charStats.maxCharStats = mCharStats;
        currentCharStats = new float[mCharStats.Length];

        invincibilityTimer = maxInvincibilityTimer;

        for (int i = 0; i < mCharStats.Length; i++)
        {
            currentCharStats[i] = mCharStats[i];
        }

        buffHandler = gameObject.AddComponent<BuffHandler>();
    }

    // Use this for initialization
    void Start () {
        tag = "Enemy";
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        gameObject.AddComponent<CircleCollider2D>().radius = 0.5f;  // Temporary
        gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load("CharacterSprites/Enemies/Goblin", typeof(Sprite)) as Sprite;

        defaultScrolling = gameObject.AddComponent<ScrollingObject>();
        defaultScrolling.SetAutoMove(autoMove);
        rb.gravityScale = 0f;

        points = 3;
    }

    public override void SetOnPlatform(bool onPlatform)
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }
}
