using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] BlockDatabase blockDatabase;
    [SerializeField] BlockFactory blockFactory;
    [SerializeField] GridManager gridManager;
    Level level;

    private void Start()
    {
        GenerateLevel();
    }
    public void GenerateLevel()
    {
        level = new Level(gridManager.levelData);

        for (int y = 0; y < gridManager.levelData.grid_height; y++)
        {
            for (int x = 0; x < gridManager.levelData.grid_width; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                BlockType blockType = level.blockType[x, y];
                BlockData blockData = blockDatabase.GetBlockData(blockType);

                Block block = blockFactory.CreateBlock(blockData, pos);
                gridManager.SetBlockAt(pos, block);
            }
        }
    }
    public LevelData GetLevelInfo()
    {
        TextAsset lvlJson = Resources.Load<TextAsset>("Levels/level_01");
        return JsonUtility.FromJson<LevelData>(lvlJson.text);
    }
}
