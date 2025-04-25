using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    int totalObstacleAmount;

    private void OnEnable()
    {
        ObstacleBlock.onObstacleDestroyed += OnObstacleDestroyed;
        SelectionManager.onMatchFound += OnMatchFound;
        LevelManager.OnLevelGenerated += OnLevelGenerated;
        
    }
    private void OnDisable()
    {
        ObstacleBlock.onObstacleDestroyed -= OnObstacleDestroyed;
        SelectionManager.onMatchFound -= OnMatchFound;
        LevelManager.OnLevelGenerated -= OnLevelGenerated;

    }

    private void OnLevelGenerated()
    {
        totalObstacleAmount = levelManager.Level.obstacle1Amount +
                                    levelManager.Level.obstacle2Amount +
                                    levelManager.Level.obstacle3Amount;
    }


    private void OnObstacleDestroyed(BlockType blockType)
    {
        switch (blockType)
        {
            case BlockType.Obstacle1:
                levelManager.Level.obstacle1Amount--;
                break;
            case BlockType.Obstacle2:
                levelManager.Level.obstacle2Amount--;
                break;
            case BlockType.Obstacle3:
                levelManager.Level.obstacle3Amount--;
                break;
            default:
                break;
        }
        totalObstacleAmount = levelManager.Level.obstacle1Amount +
                            levelManager.Level.obstacle2Amount +
                            levelManager.Level.obstacle3Amount;
    }

    private void OnMatchFound()
    {
        levelManager.Level.moveCount--;

        Debug.Log(levelManager.Level.obstacle1Amount + " " + levelManager.Level.obstacle2Amount + " " + levelManager.Level.obstacle3Amount);

        if (totalObstacleAmount == 0 && levelManager.Level.moveCount >= 0)
        {
            Debug.Log("WON");
        }
        else if (totalObstacleAmount > 0 && levelManager.Level.moveCount == 0)
        {
            Debug.Log("LOST");
        }
    }

}
