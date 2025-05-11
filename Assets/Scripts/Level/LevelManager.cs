using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action onLevelGenerated;

    [SerializeField] BlockDatabase blockDatabase;
    [SerializeField] BlockFactory blockFactory;
    [SerializeField] GridManager gridManager;
    Level level;
    LevelData levelData;

    public Level Level { get => level; set => level = value; }
    public LevelData LevelData { get => levelData; set => levelData = value; }

    private void Awake()
    {
        //SaveData saveData = SaveManager.LoadProgress();
        //LevelData = GetLevelInfo(saveData.unlockedLevel);
        LevelData = GetLevelInfo(GameManager.Instance.LevelProgress.GetLevel());
    }
    private void Start()
    {
        GenerateLevel();
        onLevelGenerated?.Invoke();
    }
    public void GenerateLevel()
    {
        Level = new Level(LevelData, blockDatabase);

        for (int y = 0; y < LevelData.grid_height; y++)
        {
            for (int x = 0; x < LevelData.grid_width; x++)
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
