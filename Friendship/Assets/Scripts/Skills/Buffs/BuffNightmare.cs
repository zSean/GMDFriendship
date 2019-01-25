using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This will cause stun by disabling box/circle colliders. Be careful of enemies who use colliders for things other than contact dmg
public class BuffNightmare : StatusEffect {

    float attackTimer = 1f;
    GameObject debuffObject;
    RuntimeAnimatorController debuffAnim;
    int variation = 0;

    public override void Activate(int characterState, GameObject sender, ref float damagePower)
    {
    }

    public override string BuffDescription()
    {
        return "It will be your last";
    }

    public override void BuffDestroy()
    {
        gameObject.GetComponent<Enemy>().SetStun(false);
        gameObject.GetComponent<BuffHandler>().RemoveBuff(this);
        Destroy(this);
    }

    public override void Init()
    {
        gameObject.GetComponent<Enemy>().SetStun(true);
    }

    public override void Stack(StatusEffect sameEffect)
    {
        if (sameEffect.GetPower() > power)
            power = sameEffect.GetPower();
        buffTimer = maxBuffTimer;
    }

    public override void TransferBuff(GameObject target)
    {
    }

    private void Awake()
    {
        buff = false;
    }
    // Use this for initialization
    void Start () {
        buffName = "Nightmare";
        maxBuffTimer = 3f;
        buffTimer = maxBuffTimer;

        debuffObject = new GameObject();
        debuffObject.AddComponent<SpriteRenderer>().sortingOrder = 2;
        debuffAnim = Resources.Load("CharacterSprites/Skills/Players/NightmareAnim") as RuntimeAnimatorController;

        debuffObject.AddComponent<Animator>().runtimeAnimatorController = debuffAnim;
        debuffObject.transform.position = gameObject.transform.position;
        debuffObject.transform.localScale = new Vector3(10f, 10f);
        debuffObject.transform.SetParent(gameObject.transform);
	}
	
	// Update is called once per frame
	void Update () {
        buffTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        if(attackTimer <= 0)
        {
            attackTimer = 1f;
            gameObject.GetComponent<CharacterStats>().Attacked(Mathf.Max(0, (int)(power - gameObject.GetComponent<CharacterStats>().GetCurrentStat(2))));
        }
        if (buffTimer <= 0)
            BuffDestroy();
	}

    private void OnDestroy()
    {
        if (debuffObject != null)
            Destroy(debuffObject);
 
        if (variation == 0)
        {
            if (buffTimer >= 0)
            {
                GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

                // If enemies are in range
                for (int i = 0; i < allEnemies.Length; i++)
                {
                    if (Mathf.Pow(gameObject.transform.position.x - allEnemies[i].transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - allEnemies[i].transform.position.y, 2) < 400)
                    {
                        DamageCalculations.ApplyCalculation(allEnemies[i], parent, 1.2f * power);
                    }
                }
            }
        }
    }

    public void SetVariation(int setVariation)
    {
        variation = setVariation;
    }
}



