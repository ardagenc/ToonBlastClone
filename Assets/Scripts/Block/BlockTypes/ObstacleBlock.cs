using System;
using UnityEngine;

public class ObstacleBlock : Block
{
    public static event Action<BlockType> onObstacleDestroyed;

    public bool damagable;
    public int health = 1;
    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {            
            DestroyBlock();
        }
    }
    private void DestroyBlock()
    {
        Debug.Log("DESTROYED");
        onObstacleDestroyed?.Invoke(blockType);
        Destroy(gameObject);
    }
}
