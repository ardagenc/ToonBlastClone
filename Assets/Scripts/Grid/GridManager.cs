using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    private int width;
    private int height;

    LevelData levelData;
    Block[,] blocks;

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public Block[,] Blocks { get => blocks; set => blocks = value; }

    private void Start()
    {
        DOTween.SetTweensCapacity(500, 50);
        InitializeGrids();
        GetAdjacentBlocks(new Vector2Int(0, 1));
    }

    public void InitializeGrids()
    {
        levelData = levelManager.LevelData;
        Width = levelData.grid_width;
        Height = levelData.grid_height;

        Blocks = new Block[levelData.grid_width, levelData.grid_height];
    }
    public void CollapseColumn(int x)
    {
        for (int y = 0; y < Height; y++)
        {
            if (Blocks[x, y] == null)
            {
                for (int ny = y + 1; ny < Height; ny++)
                {
                    if (Blocks[x, ny] != null)
                    {                                                
                        Block movingBlock = Blocks[x, ny];
                        

                        if (movingBlock != null && movingBlock.CanFall)
                        {
                            Blocks[x, y] = movingBlock;
                            Blocks[x, ny] = null;
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
        for (int x = 0; x < Width; x++)
        {
            CollapseColumn(x);
        }
    }
    public void SpawnNewBlocks(BlockFactory blockFactory)
    {        
        for (int x = 0; x < Width; x++)
        {
            List<int> emptyRows = new List<int>();

            for (int y = Height - 1; y >= 0; y--)
            {
                if (Blocks[x, y] != null && !Blocks[x, y].CanFall)
                {
                    break;
                }
                else if (Blocks[x, y] == null)
                {
                    emptyRows.Add(y);
                }
            }


            if (emptyRows.Count == 0)
                continue;

            float spawnStartY = Height;

            for (int y = Height - 1; y >= 0; y--)
            {
                if (Blocks[x, y] != null)
                {                   
                    spawnStartY = Mathf.Max(spawnStartY, Blocks[x, y].transform.position.y + 1);
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

                Blocks[x, targetY] = newBlock;

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
                Block neighbor = Blocks[neighborPos.x, neighborPos.y];
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
        Blocks[pos.x, pos.y] = block;
    }

    public Block GetBlockAt(Vector2Int pos)
    {
        return Blocks[pos.x, pos.y];
    }
    public bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < Width && pos.y < Height;
    }
}
