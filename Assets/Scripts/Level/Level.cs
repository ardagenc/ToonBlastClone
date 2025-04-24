using System;
using UnityEngine;

public class Level
{
    public BlockType[,] blockType;

    public Level(LevelData levelData)
    {
        blockType = new BlockType[levelData.grid_width, levelData.grid_height];
        int gridIndex = 0;

        for (int y = 0; y < levelData.grid_height; y++)
            for (int x = 0; x < levelData.grid_width; x++)
            {
                switch (levelData.grid[gridIndex++])
                {
                    case "r":
                        blockType[x, y] = BlockType.Red;
                        break;
                    case "g":
                        blockType[x, y] = BlockType.Green;
                        break;
                    case "b":
                        blockType[x, y] = BlockType.Blue;
                        break;
                    case "y":
                        blockType[x, y] = BlockType.Blue;
                        break;
                    case "ob1":
                        blockType[x, y] = BlockType.Obstacle1;
                        break;
                    case "ob2":
                        blockType[x, y] = BlockType.Obstacle2;
                        break;
                    case "ob3":
                        blockType[x, y] = BlockType.Obstacle3;
                        break;
                    case "rand":
                        blockType[x, y] = ((BlockType[])Enum.GetValues(typeof(BlockType)))[UnityEngine.Random.Range(0, 3)];
                        break;
                    default:
                        break;
                }
            }
    }

}
