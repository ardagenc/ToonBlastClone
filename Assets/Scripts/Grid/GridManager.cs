using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public int width;
    public int height;

    [SerializeField] LevelManager levelManager;
    public LevelData levelData;
    [SerializeField] Grid gridPrefab;
    public Block[,] blocks;

    private void Start()
    {
        InitializeGrids();
    }
    public void InitializeGrids()
    {
        levelData = levelManager.GetLevelInfo();
        width = levelData.grid_width;
        height = levelData.grid_height;

        blocks = new Block[levelData.grid_width, levelData.grid_height];
    }
    public void CollapseColumn(int x)
    {
        for (int y = 0; y < height; y++)
        {
            if (blocks[x, y] == null)
            {
                for (int ny = y + 1; ny < height; ny++)
                {
                    if (blocks[x, ny] != null)
                    {                                                
                        Block movingBlock = blocks[x, ny];
                        

                        if (movingBlock != null && movingBlock.CanFall)
                        {
                            blocks[x, y] = movingBlock;
                            blocks[x, ny] = null;
                            movingBlock.MoveTo(new Vector2Int(x, y));
                        }
                        else
                        {
                            //Debug.Log(blocks[x, ny].gridPos);
                        }

                        break;
                    }
                }
            }
        }
    }
    public void CollapseAll()
    {
        for (int x = 0; x < width; x++)
        {
            CollapseColumn(x);
        }
    }
    public void SpawnNewBlocks(BlockFactory blockFactory)
    {        
        for (int x = 0; x < width; x++)
        {
            List<int> emptyRows = new List<int>();

            for (int y = height - 1; y >= 0; y--)
            {
                if (blocks[x, y] != null && !blocks[x, y].CanFall)
                {
                    break;
                }
                else if (blocks[x, y] == null)
                {
                    emptyRows.Add(y);
                }
            }


            if (emptyRows.Count == 0)
                continue;

            float spawnStartY = height;

            for (int y = height - 1; y >= 0; y--)
            {
                if (blocks[x, y] != null)
                {                   
                    spawnStartY = Mathf.Max(spawnStartY, blocks[x, y].transform.position.y + 1);
                    break;
                }
            }

            for (int i = 0; i < emptyRows.Count; i++)
            {
                int targetY = emptyRows[(emptyRows.Count - 1) - i];
                float spawnY = spawnStartY + i;

                Vector2 spawnGridPos = new Vector2(x, spawnY);
                Vector2Int targetGridPos = new Vector2Int(x, targetY);

                BlockData randomData = blockFactory.GetRandomBlockData();
                Block newBlock = blockFactory.CreateBlock(randomData, spawnGridPos);

                blocks[x, targetY] = newBlock;

                newBlock.MoveTo(targetGridPos);
            }
        }
        
    }
    public List<Block> GetAdjacentBlocks(Vector2Int gridPos)
    {
        List<Block> adjacentBlocks = new List<Block>();

        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
        };

        foreach (var dir in directions)
        {
            Vector2Int neighborPos = gridPos + dir;

            if (IsInBounds(neighborPos))
            {
                Block neighbor = blocks[neighborPos.x, neighborPos.y];
                if (neighbor != null && !adjacentBlocks.Contains(neighbor))
                {
                    adjacentBlocks.Add(neighbor);
                }
            }
        }

        return adjacentBlocks;
    }

    public void SetBlockAt(Vector2Int pos, Block block)
    {
        blocks[pos.x, pos.y] = block;
    }

    public Block GetBlockAt(Vector2Int pos)
    {
        return blocks[pos.x, pos.y];
    }
    public bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
    }
}
