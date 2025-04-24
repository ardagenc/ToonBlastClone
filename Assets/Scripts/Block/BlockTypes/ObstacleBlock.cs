using UnityEngine;

public class ObstacleBlock : Block
{
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
        Destroy(gameObject);
    }
}
