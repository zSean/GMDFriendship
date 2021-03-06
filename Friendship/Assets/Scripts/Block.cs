﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    int blockType = 0;   // 0 = aerial, 1 = flurry, 2 = buff
    int chainNumber = 0;
    float blockSpeed = 3f;
    Vector3 vectorBlockSpeed;

    bool completeChain = false;
    public GameObject completeChainSprite;
    GameObject completeChainSpriteClone;
    float destination = 0;
    BlockGenerator parent;

    private void Awake()
    {
        vectorBlockSpeed = new Vector3(blockSpeed, 0f);
    }

    public void SetBlockGenerator(BlockGenerator parent)
    {
        this.parent = parent;
    }
	
	// Update is called once per frame
	void Update () {

        if (destination > 0)
        {
            if (destination < blockSpeed)
            {
                transform.position += new Vector3(destination, 0f);
                destination = 0;
            }
            else
            {
                transform.position += vectorBlockSpeed;
                destination -= blockSpeed;
            }
        }
	}

    public void SetBlockType(int setType)
    {
        if ((setType < 3) && (setType >= 0))
        {
            blockType = setType;
            Sprite[] subSprite = Resources.LoadAll<Sprite>("BlockColours");
            gameObject.GetComponent<SpriteRenderer>().sprite = subSprite[blockType];
        }
    }

    public int GetBlockType()
    {
        return blockType;
    }

    private void OnMouseDown()
    {
        ActivateBlock();
    }

    // Activate block. BlockGenerator will handle all the details
    public void ActivateBlock()
    {
        if (parent != null)
            parent.GetComponent<BlockGenerator>().ActivateSkill(gameObject);
    }

    public void SetCompleteChain(bool isMatch)
    {
        completeChain = isMatch;

        if (completeChainSpriteClone != null)
            Destroy(completeChainSpriteClone);
        if(isMatch)
        {
            completeChainSpriteClone = Instantiate(completeChainSprite);
            completeChainSpriteClone.transform.position = transform.position + Vector3.right * 1.5f;
            completeChainSpriteClone.transform.SetParent(gameObject.transform);
        }
    }

    public bool GetCompleteChain()
    {
        return completeChain;
    }

    public void SetDestination(float distance)
    {
        destination += distance;
    }

    public int GetChainNumber()
    {
        return chainNumber;
    }

    public void SetChainNumber(int cNumber)
    {
        chainNumber = cNumber;
    }
}
