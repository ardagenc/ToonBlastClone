using UnityEngine;

public class BoxBlock : Block
{
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
