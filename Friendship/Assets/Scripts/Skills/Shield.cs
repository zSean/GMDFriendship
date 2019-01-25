using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A shield isn't technically a skillAnimation, but it has so many properties similar to it
public class Shield : SkillAnimation {

    protected float radius = 0.4f;    // Shield radius
    protected int numHits = 1;
    protected string[] tags;

    public void SetTargetTags(string[] targets)
    {
        tags = targets;
    }
    public string[] GetTargetTags()
    {
        return tags;
    }
    public void SetRadius(float newRadius)
    {
        radius = newRadius;
    }
    public float GetRadius()
    {
        return radius;
    }

    protected virtual void Start()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null)
            gameObject.AddComponent<SpriteRenderer>();
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 9;

        //SET ANIMATION SPEED TO 0 (if exists) AND HIDE THE SPRITE
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        if (gameObject.GetComponent<Animator>() != null)
            gameObject.GetComponent<Animator>().speed = 0;

        gameObject.AddComponent<CircleCollider2D>().radius = radius;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
    }

    // LASERS SHOULD HAVE THEIR OWN TAGS AND NOT BE DESTROYED ON BLOCK
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (collision.gameObject.tag == tags[i])
            {
                Destroy(collision.gameObject);
                numHits--;
                if (numHits <= 0)
                    Destroy(gameObject);
            }
        }
    }
}
