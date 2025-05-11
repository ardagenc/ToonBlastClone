using System;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    //[SerializeField] GameObject blockPrefab;
    [SerializeField] BlockDatabase blockDatabase;

    public Block CreateBlock(BlockData blockData, Vector2 gridPos)
    {
        Block block = PoolManager.Instance.GetBlock(blockData.blockType);
        block.transform.position = gridPos;
        block.gameObject.SetActive(true);

        block.Init((int)gridPos.x, (int)gridPos.y,blockData);
        return block;
    }

    public void ReturnBlock(Block block)
    {
        PoolManager.Instance.ReturnBlock(block, block.BlockType);
    }

    public BlockData GetRandomBlockData()
    {
        BlockType blockType = ((BlockType[])Enum.GetValues(typeof(BlockType)))[UnityEngine.Random.Range(0, 3)];
        BlockData blockData = blockDatabase.GetBlockData(blockType);

        return blockData;
    }
}
