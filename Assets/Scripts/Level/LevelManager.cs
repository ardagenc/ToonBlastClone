using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action onLevelGenerated;

    [SerializeField] BlockDatabase blockDatabase;
    [SerializeField] BlockFactory blockFactory;
    [SerializeField] GridManager gridManager;
    Level level;

    public Level Level { get => level; set => level = value; }

    private void Start()
    {
        GenerateLevel();
        onLevelGenerated?.Invoke();
    }
    public void GenerateLevel()
    {
        Level = new Level(gridManager.levelData, blockDatabase);

        for (int y = 0; y < gridManager.levelData.grid_height; y++)
        {
            for (int x = 0; x < gridManager.levelData.grid_width; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                BlockType blockType = Level.blockType[x, y];
                BlockData blockData = blockDatabase.GetBlockData(blockType);

                Block block = blockFactory.CreateBlock(blockData, pos);
                gridManager.SetBlockAt(pos, block);
            }
        }
    }
    public LevelData GetLevelInfo(int level)
    {
        TextAsset lvlJson = Resources.Load<TextAsset>("Levels/level_" + level.ToString("00"));
        return JsonUtility.FromJson<LevelData>(lvlJson.text);
    }
}
