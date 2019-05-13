using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mod
public class BlockGenerator : MonoBehaviour {

    GameObject block;
    GameObject blockClone;
    float blockTimer = 0f;
    int blockCount = 0;
    float blockOffset;
    float totalDistance;

    List<Block> blockList;
    Skills[] blockSkills;
    int reqBlocks = 3;
    int bonusBlocks = 0;

    LevelGenerator parent;

    public void Init()
    {
        block = (GameObject)Resources.Load("Block");
        blockSkills = new Skills[3];
        totalDistance = Camera.main.orthographicSize * 2 / 6;
        blockOffset = -Camera.main.orthographicSize * 2/3 + (block.GetComponent<SpriteRenderer>().bounds.size.x);
        blockList = new List<Block>();
    }
    public void SetLevelGenerator(LevelGenerator levelGenerator)
    {
        parent = levelGenerator;
    }
    public void SetBlockSkill(int index, Skills newSkill)
    {
        // GC?
        blockSkills[index] = newSkill;
    }
    public void SetBlockTimer(float newTime)
    {
        blockTimer = newTime;
    }
    public void SetBonusBlocks(int setBonus)
    {
        if (setBonus < 0)
            setBonus = 0;
        bonusBlocks = setBonus;
    }
    public int GetBonusBlockAmount()
    {
        return bonusBlocks;
    }

    // Update is called once per frame
    void Update () {
        blockTimer -= Time.deltaTime;

        if ((blockTimer <= 0) && (blockCount < 10))
        {
            // Initialize the block
            blockClone = Instantiate(block, new Vector3(Camera.main.transform.position.x - 2 * Camera.main.orthographicSize, transform.position.y), transform.rotation);
            int randomType = Random.Range(0, 3);
            blockClone.GetComponent<Block>().SetBlockType(randomType);
            blockClone.GetComponent<Block>().SetBlockGenerator(this);
            blockClone.GetComponent<Block>().SetDestination(totalDistance * (10 - blockCount) - (blockOffset));
            blockList.Add(blockClone.GetComponent<Block>());

            // Check if the newly created block matches with blockCount - 1 (nextBlock). If so, set the chain number and link blocks
            if(blockCount > 0)
            {
                if((blockList[blockCount - 1].GetBlockType() == randomType) && (!blockList[blockCount - 1].GetCompleteChain()))
                {
                    blockClone.GetComponent<Block>().SetChainNumber(blockList[blockCount - 1].GetChainNumber() + 1);

                    // Check to see if there's a set of 3
                    blockClone.GetComponent<Block>().SetCompleteChain(blockClone.GetComponent<Block>().GetChainNumber() == 2);
                }
            }

            // Reset the block generation timer
            blockTimer = 0f;
            blockCount += 1;
        }
	}

    public void ActivateSkill(GameObject activatedBlock)
    {
        // Find the clicked block and its chain number
        int blockIndex = blockList.IndexOf(activatedBlock.GetComponent<Block>());
        int blocksRemoved = activatedBlock.GetComponent<Block>().GetChainNumber();

        while((blockIndex + 1 < blockList.Count) && (blockList[blockIndex].GetChainNumber() < blockList[blockIndex + 1].GetChainNumber()))
        {
            blockIndex++;
            blocksRemoved++;
        }

        // First activate the block skill and chain number
        if (reqBlocks <= bonusBlocks + blocksRemoved + 1)
        {
            // Remove if statement later
            if (blockSkills[activatedBlock.GetComponent<Block>().GetBlockType()] != null)
            {
                print(blockSkills[activatedBlock.GetComponent<Block>().GetBlockType()].SkillDescription());
                blockSkills[activatedBlock.GetComponent<Block>().GetBlockType()].Activate();
            }
        }

        // Assuming parent != null
        if (parent.GetMana() > blockSkills[activatedBlock.GetComponent<Block>().GetBlockType()].GetManaCost() * (reqBlocks - blocksRemoved - 1))
        {
            parent.AddMana(-blockSkills[activatedBlock.GetComponent<Block>().GetBlockType()].GetManaCost() * (reqBlocks - blocksRemoved - 1));

            // Shift all other blocks down by a fixed amount * chain number of clicked block
            for (int counter = blockIndex; counter < blockList.Count; counter++)
            {
                blockList[counter].SetDestination((blocksRemoved + 1) * totalDistance);
            }

            // Connect the blocks before and after the removed blocks
            if ((blockIndex - blocksRemoved > 0) && (blockIndex < blockList.Count - 1) && (!blockList[blockIndex - blocksRemoved - 1].GetCompleteChain()))
            {
                int currentBlock = blockIndex + 1;

                // If the block before the clicked chain is the same type as the block after the activated chain
                if (blockList[currentBlock].GetBlockType() == blockList[blockIndex - blocksRemoved - 1].GetBlockType())
                {
                    blockList[currentBlock].SetChainNumber(blockList[blockIndex - blocksRemoved - 1].GetChainNumber() + 1);
                    blockList[currentBlock].SetCompleteChain(blockList[currentBlock].GetChainNumber() == 2);

                    // If successfully connected, then redo all the connections that the block before the clicked chain had
                    while ((currentBlock + 1 < blockList.Count) && (blockList[currentBlock].GetBlockType() == blockList[currentBlock + 1].GetBlockType()) && ((blockList[currentBlock].GetChainNumber() + 1) % 3 != blockList[currentBlock + 1].GetChainNumber()))
                    {
                        blockList[currentBlock + 1].SetChainNumber((blockList[currentBlock].GetChainNumber() + 1) % 3);
                        blockList[currentBlock + 1].SetCompleteChain(blockList[currentBlock + 1].GetChainNumber() == 2);
                        currentBlock++;
                    }
                }
            }

            // Removed the clicked blocks 
            for (int counter = blockIndex; counter > blockIndex - blocksRemoved - 1; counter--)
            {
                Destroy(blockList[counter].gameObject);
                blockList.Remove(blockList[counter]);
            }

            blockCount -= (blocksRemoved + 1);
        }
        return;
    }
}
