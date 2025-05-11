using System;
using UnityEngine;

public class ObstacleBlock : Block
{
    public static event Action<BlockType> onObstacleDestroyed;

    public bool damagable;
    public int health = 1;

    bool isDestroyed = false;
    public void TakeDamage()
    {
        health--;

        if (health <= 0 && !isDestroyed)
        {            
            isDestroyed = true;
            DestroyBlock();
        }
    }
    private void DestroyBlock()
    {        
        onObstacleDestroyed?.Invoke(blockType);
        //Destroy(gameObject);
        PoolManager.Instance.ReturnBlock(this, BlockType);
    }
}
