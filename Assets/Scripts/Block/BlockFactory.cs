using System;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    //[SerializeField] GameObject blockPrefab;
    [SerializeField] BlockDatabase blockDatabase;

    public Block CreateBlock(BlockData blockData, Vector2 gridPos)
    {
        GameObject blockGO = Instantiate(blockData.blockPrefab, gridPos, Quaternion.identity);

        Block block = blockGO.GetComponent<Block>();
        block.Init((int)gridPos.x, (int)gridPos.y,blockData);
        return block;
    }

    public BlockData GetRandomBlockData()
    {
        BlockType blockType = ((BlockType[])Enum.GetValues(typeof(BlockType)))[UnityEngine.Random.Range(0, 3)];
        BlockData blockData = blockDatabase.GetBlockData(blockType);

        return blockData;
    }
}
