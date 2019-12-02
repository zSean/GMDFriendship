using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextCreator : MonoBehaviour
{
    public static DamageTextCreator tCreator;
    GameObject damageText;
    List<GameObject> damageTextPool = new List<GameObject>();
    readonly int limit = 10;
    int tracker = 0;

    private void Awake()
    {
        tCreator = this;
    }

    private void Start()
    {
        damageText = (GameObject)Resources.Load("DamageNum");

        for(int i = 0; i < limit; i++)
        {
            damageTextPool.Add(Instantiate(damageText));
            damageTextPool[i].SetActive(false);
        }
    }

    public GameObject CreateText(Vector3 textPosition, string damage)
    {
        damageTextPool[tracker].SetActive(true);
        damageTextPool[tracker].GetComponent<DamageText>().CreateText(textPosition, damage);
        tracker = (tracker + 1) % limit;

        return damageTextPool[tracker];
    }
}
