using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    float power = 10;
    float speed = 0.3f;
    float life = 3f;

    public void SetPower(float setPower)
    {
        power = setPower;
        return;
    }
    public float GetPower()
    {
        return power;
    }

    private void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0)
            Destroy(gameObject);
        else
            transform.position += new Vector3(speed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
