using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour {

    public GameObject block;
    GameObject blockClone;
    float blockTimer = 0f;//1.5f;
    int blockCount = 0;
    float blockOffset;
    float totalDistance;

    List<Block> blockList;  // List<GameObject> blockList;

    private void Start()
    {
        totalDistance = Camera.main.orthographicSize * 2 / 6;
        blockOffset = -Camera.main.orthographicSize * 2/3 + (block.GetComponent<SpriteRenderer>().bounds.size.x);
        blockList = new List<Block>();  // blockList = new List<GameObject>();
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
                    blockList[blockCount - 1].SetPrevBlock(blockClone);

                    blockClone.GetComponent<Block>().SetChainNumber(blockList[blockCount - 1].GetChainNumber() + 1);

                    // Check to see if there's a set of 3
                    blockClone.GetComponent<Block>().SetCompleteChain(blockClone.GetComponent<Block>().GetChainNumber() == 2);
                }
            }

            // Reset the block generation timer
            blockTimer = 0f;//1.5f;
            blockCount += 1;
        }
	}

    public void ActivateSkill(GameObject activatedBlock)
    {
        // First activate the block skill and chain number
        // blockType, chainNumber

        // Find the clicked block and its chain number
        int blockIndex = blockList.IndexOf(activatedBlock.GetComponent<Block>());
        int blocksRemoved = activatedBlock.GetComponent<Block>().GetChainNumber();

        // Shift all other blocks down by a fixed amount * chain number of clicked block
        for (int counter = blockIndex; counter < blockList.Count; counter++)
        {
            blockList[counter].SetDestination((blocksRemoved + 1) * totalDistance);
        }

        // Connect the blocks before and after the removed blocks
        if ((blockIndex - blocksRemoved > 0) && (blockIndex < blockList.Count - 1 - blocksRemoved) && (!blockList[blockIndex - blocksRemoved - 1].GetCompleteChain()))
        {
            // If the block before the clicked chain is the same type as the block after the activated chain
            if (blockList[blockIndex + 1].GetBlockType() == blockList[blockIndex - blocksRemoved - 1].GetBlockType())
            {
                Block tempBlock = blockList[blockIndex + 1];
                blockList[blockIndex - blocksRemoved - 1].SetPrevBlock(tempBlock.gameObject);
                tempBlock.SetChainNumber(blockList[blockIndex - blocksRemoved - 1].GetChainNumber() + 1);
                tempBlock.SetCompleteChain(tempBlock.GetChainNumber() == 2);

                // If successfully connected, then redo all the connections that the block before the clicked chain had
                while(tempBlock.GetPrevBlock() != null)
                {
                    tempBlock.GetPrevBlock().GetComponent<Block>().SetChainNumber((tempBlock.GetComponent<Block>().GetChainNumber() + 1) % 3);
                    tempBlock.GetPrevBlock().GetComponent<Block>().SetCompleteChain(tempBlock.GetPrevBlock().GetComponent<Block>().GetChainNumber() == 2);

                    if (tempBlock.GetPrevBlock().GetComponent<Block>().GetChainNumber() == 0)
                    {
                        Block tempBlock2 = tempBlock.GetPrevBlock().GetComponent<Block>();
                        tempBlock.SetPrevBlock(null);
                        tempBlock = tempBlock2;
                    }
                    else
                        tempBlock = tempBlock.GetPrevBlock().GetComponent<Block>();
                }
            }
        }

        // Removed the clicked blocks 
        for(int counter = blockIndex; counter > blockIndex - blocksRemoved - 1; counter--)
        {
            Destroy(blockList[counter].gameObject);
            blockList.Remove(blockList[counter]);
        }

        blockCount -= (blocksRemoved + 1);
        return;
    }
}
