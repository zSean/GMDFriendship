using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    TextMeshPro textMesh;
    float textTimer = 0f;

    void Awake()
    {
        textMesh = gameObject.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        textTimer -= Time.deltaTime;

        if (textTimer > 0.17f)
            gameObject.transform.position += Vector3.up * 0.4f;
        //gameObject.transform.position += Vector3.up * 0.1f;


        if (textTimer <= 0)
            gameObject.SetActive(false);
    }

    public void CreateText(Vector3 pos, string damage)
    {
        textTimer = 0.25f;
        gameObject.transform.position = pos;
        textMesh.text = damage;
    }
}
