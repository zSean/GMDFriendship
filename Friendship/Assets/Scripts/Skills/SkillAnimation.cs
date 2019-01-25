using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimation : MonoBehaviour {

    protected GameObject tracking;
    protected Vector3 lastPosition;
    protected float life = 0f;
    protected float startTime = 0;
    protected bool activated = false;

    public void SetLife(float setLife)
    {
        life = setLife;
    }
    public float GetLife()
    {
        return life;
    }

    public void Init(GameObject target, float lifespan)
    {
        tracking = target;
        life = lifespan;

        lastPosition = target.transform.position;
    }

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 9;

        //SET ANIMATION SPEED TO 0 (if exists) AND HIDE THE SPRITE
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        if (gameObject.GetComponent<Animator>() != null)
            gameObject.GetComponent<Animator>().speed = 0;
    }

    // Update is called once per frame
    protected virtual void Update () {

        startTime -= Time.deltaTime;
        if (startTime <= 0)
        {
            if(!activated)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                activated = true;

                // SET ANIMATION SPEED TO 1 (may want to change this if animation speed is not always 1)
                if (gameObject.GetComponent<Animator>() != null)
                    gameObject.GetComponent<Animator>().speed = 1;
            }

            life -= Time.deltaTime;
            if (life <= 0)
                Destroy(gameObject);

            if (tracking != null)
                lastPosition = tracking.transform.position;
        }

        transform.position = lastPosition;
	}

    public void DestroyAnim()
    {
        Destroy(gameObject);
    }
}
