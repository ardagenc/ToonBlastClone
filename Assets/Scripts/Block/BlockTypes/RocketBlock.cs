using System.Collections;
using UnityEngine;

public enum RocketDirection { Horizontal, Vertical }
public class RocketBlock : Block
{
    public RocketDirection Direction;
    
    public void Activate()
    {
        if (Direction == RocketDirection.Horizontal)
        {           
            Debug.Log("Horizontal Blast");
        }
        else
        {
            Debug.Log("Vertical Blast");
        }
    }
}
