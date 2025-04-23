using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Matchfinder : MonoBehaviour
{
    [SerializeField] GridManager gridManager;

    public Matchfinder(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public List<Block> FindMatches(Block startingBlock)
    {
        List<Block> result = new List<Block>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Block> queue = new Queue<Block>();

        queue.Enqueue(startingBlock);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current.gridPos)) continue;

            visited.Add(current.gridPos);
            result.Add(current);

            foreach (var direction in Directions)
            {
                var neighborPos = current.gridPos + direction;

                if (!gridManager.IsInBounds(neighborPos)) continue;
                var neighbor = gridManager.GetBlockAt(neighborPos);

                if (neighbor != null && neighbor.blockType == startingBlock.blockType)
                {
                    queue.Enqueue(neighbor);
                }
            }            
        }
        return result;
    }

    public static readonly List<Vector2Int> Directions = new()
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

}
