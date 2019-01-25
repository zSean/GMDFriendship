using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherShield : Shield {

    LevelGenerator reference;
    int variation = 0;
    GameObject[] allTargets;

    public void SetVariation(int setVariant)
    {
        variation = setVariant;
    }

    protected override void Start()
    {
        base.Start();

        radius = 0.3f;
        gameObject.transform.localScale = new Vector2(8f, 8f);
        gameObject.GetComponent<CircleCollider2D>().radius = radius;
        if (gameObject.GetComponent<Animator>() == null)
            gameObject.AddComponent<Animator>().runtimeAnimatorController = Resources.Load("CharacterSprites/Skills/Players/FeatherShieldAnim") as RuntimeAnimatorController;
        reference = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelGenerator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        startTime -= Time.deltaTime;
        if (startTime <= 0)
        {
            if (!activated)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                activated = true;

                // SET ANIMATION SPEED TO 1 (may want to change this if animation speed is not always 1)
                if (gameObject.GetComponent<Animator>() != null)
                    gameObject.GetComponent<Animator>().speed = 1;
            }

            life -= Time.deltaTime;
            if (life <= 0)
                enabled = false;

            if (tracking != null)
                lastPosition = tracking.transform.position;
        }

        transform.position = lastPosition;
    }

    // LASERS SHOULD HAVE THEIR OWN TAGS AND NOT BE DESTROYED ON BLOCK
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool triggerCounter = false;
        for (int i = 0; i < tags.Length; i++)
        {
            if (collision.gameObject.tag == tags[i])
            {
                Destroy(collision.gameObject);

                if (variation == 2)
                {
                    triggerCounter = true;

                }
                else if (variation == 1)
                    reference.AddMana(Mathf.CeilToInt(reference.GetMaxMana() * 0.03f));

                numHits--;
                if (numHits <= 0)
                    enabled = false;
            }
        }

        if(triggerCounter)
        {
            // Enemy...might need to change to incorporate boss or even players later
            allTargets = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < allTargets.Length; i++)
            {
                // Take into account stats of the CHARACTER THIS SHIELD IS ATTACHED TO
                if(Mathf.Pow(gameObject.transform.position.x - allTargets[i].transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - allTargets[i].transform.position.y, 2) < 400)
                    DamageCalculations.ApplyCalculation(allTargets[i], gameObject, gameObject.GetComponent<CharacterStats>().GetCurrentStat(0) * 0.7f);
            }
        }
    }

    private void OnDisable()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    private void OnEnable()
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null)
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        if (gameObject.GetComponent<CircleCollider2D>() != null)
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }
}
