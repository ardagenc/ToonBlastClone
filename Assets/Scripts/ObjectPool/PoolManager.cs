using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{ 
    public static PoolManager Instance {  get; private set; }

    [System.Serializable]
    public class PoolEntry
    {
        public BlockType blockType;
        public Block prefab;
        public int initialSize;
    }

    [SerializeField] private List<PoolEntry> poolEntries;

    private Dictionary<BlockType, ObjectPool<Block>> pools = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (var entry in poolEntries)
        {
            var pool = new ObjectPool<Block>(entry.prefab, entry.initialSize, transform);
            pools[entry.blockType] = pool;
        }
    }

    public Block GetBlock(BlockType type)
    {
        if (pools.TryGetValue(type, out var pool))
        {
            return pool.Get();
        }

        Debug.LogError($"Pool for BlockType {type} not found!");
        return null;
    }

    public void ReturnBlock(Block block, BlockType type)
    {
        if (!pools.TryGetValue(type,out var pool))
        {
            pool.ReturnToPool(block);
        }
        else
        {
            Debug.LogWarning("Trying to return to non-existing pool, destroying instead.");
            Destroy(block.gameObject);
        }
    }

}
