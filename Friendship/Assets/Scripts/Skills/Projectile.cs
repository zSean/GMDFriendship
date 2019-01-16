using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    GameObject parent;  // Used if the projectile ever needs to be traced back to its parent

    protected float power = 10;
    protected float speed = 0.3f;
    protected int numHits = 0;    // Number of hits the projectile can take before destroying. < 0 = based on timer
    protected float life = 3f;

    // y = ax^2 + bx + c...if the projectile needs its y value modified. Set to 0 otherwise
    protected float[] speedY = { 0f, 0f, 0f };

    // What the projectile targets, based on tags. Default is Enemy
    protected string[] targets = { "Enemy" };

    // Dodge calculation, Defend calculation, Piercing, respectively
    protected bool[] actions = { true, true, true };


    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }
    public GameObject GetParent()
    {
        return parent;
    }
    public void SetTargetEnemy(string[] targetTags)
    {
        targets = targetTags;
    }
    public string[] GetTargetEnemy()
    {
        return targets;
    }
    public void SetPower(float setPower)
    {
        power = setPower;
        return;
    }
    public float GetPower()
    {
        return power;
    }
    public void SetNumHits(int hitNum)
    {
        numHits = hitNum;
    }
    public int GetNumHits()
    {
        return numHits;
    }
    public void SetTrajectory(float speedX, float[] speedY = null)
    {
        speed = speedX;
        if (speedY != null)
        {
            this.speedY[0] = speedY[0];
            this.speedY[1] = speedY[1];
            this.speedY[2] = speedY[2];
        }
    }
    public float GetHSpeed()
    {
        return speed;
    }
    public float[] GetVSpeed()
    {
        return speedY;
    }
    public void SetActions(bool calcDodge, bool calcReduction, bool piercing)
    {
        actions[0] = calcDodge;
        actions[1] = calcReduction;
        actions[2] = piercing;
    }
    public bool[] GetActions()
    {
        return actions;
    }

    private void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0)
            Destroy(gameObject);
        else
            transform.position += new Vector3(speed, speedY[0] * Mathf.Pow(speed, 2) + speedY[1] * speed + speedY[2]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 0; i < targets.Length; i++)
        {
            if(collision.gameObject.tag == targets[i])
            {
                DamageCalculations.ApplyCalculation(collision.gameObject, gameObject, power, actions);

                if (numHits == 0)
                    Destroy(gameObject);
                numHits--;
            }
        }
    }
}