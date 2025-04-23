using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDatabase", menuName = "Scriptable Objects/BlockDatabase")]
public class BlockDatabase : ScriptableObject
{
    public List<BlockData> blockDataList;

    public BlockData GetBlockData(BlockType type)
    {
        return blockDataList.FirstOrDefault(b => b.blockType == type);
    }
}
