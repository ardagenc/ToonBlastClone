using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Matchfinder : MonoBehaviour
{
    [SerializeField] GridManager gridManager;

    private void OnEnable()
    {
        LevelManager.onLevelGenerated += OnLevelGenerated;
    }
    private void OnDisable()
    {
        LevelManager.onLevelGenerated -= OnLevelGenerated;
    }

    private void OnLevelGenerated()
    {
        foreach (List<Block> list in FindAllMatches())
        {
            foreach(Block block in list)
            {
                if (block.BlockAnimation.frames == null)
                {
                    Debug.Log("frames is NULL");
                    continue;
                }
                if (block.BlockAnimation.frames.Length != 0)
                {
                    block.BlockAnimation.Play();
                }
            }
        }


    }

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
            if (visited.Contains(current.GridPos)) continue;

            visited.Add(current.GridPos);
            result.Add(current);

            foreach (var direction in Directions)
            {
                var neighborPos = current.GridPos + direction;

                if (!gridManager.IsInBounds(neighborPos)) continue;
                var neighbor = gridManager.GetBlockAt(neighborPos);

                if (neighbor != null && neighbor.BlockType == startingBlock.BlockType)
                {
                    queue.Enqueue(neighbor);
                }
            }            
        }
        return result;
    }

    public List<List<Block>> FindAllMatches()
    {
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        List<List<Block>> allMatches = new List<List<Block>>();

        for (int x = 0; x < gridManager.Width; x++)
        {
            for (int y = 0; y < gridManager.Height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (visited.Contains(pos)) continue;

                Block startingBlock = gridManager.GetBlockAt(pos);
                if (startingBlock == null) continue;

                // BFS ile aynı türdeki komşu blokları bul
                List<Block> matchGroup = new List<Block>();
                Queue<Block> queue = new Queue<Block>();
                queue.Enqueue(startingBlock);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    if (visited.Contains(current.GridPos)) continue;

                    visited.Add(current.GridPos);
                    matchGroup.Add(current);

                    foreach (var dir in Directions)
                    {
                        var neighborPos = current.GridPos + dir;
                        if (!gridManager.IsInBounds(neighborPos)) continue;

                        var neighbor = gridManager.GetBlockAt(neighborPos);
                        if (neighbor != null &&
                            neighbor.BlockType == startingBlock.BlockType &&
                            !visited.Contains(neighbor.GridPos))
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                }

                // 4 veya daha fazla blok varsa eşleşme olarak kaydet
                if (matchGroup.Count >= 4)
                {
                    allMatches.Add(matchGroup);
                }
            }
        }

        return allMatches;
    }

    public static readonly List<Vector2Int> Directions = new()
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

}
