using System;
using System.Linq;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public static event Action<bool> onLevelFinished;

    [SerializeField] LevelManager levelManager;

    int totalObstacleAmount;

    private void OnEnable()
    {
        ObstacleBlock.onObstacleDestroyed += OnObstacleDestroyed;
        MatchManager.onMatchFound += OnMatchFound;
        LevelManager.onLevelGenerated += OnLevelGenerated;
        
    }
    private void OnDisable()
    {
        ObstacleBlock.onObstacleDestroyed -= OnObstacleDestroyed;
        MatchManager.onMatchFound -= OnMatchFound;
        LevelManager.onLevelGenerated -= OnLevelGenerated;

    }
    private void OnLevelGenerated()
    {
        totalObstacleAmount = levelManager.Level.obstacles.Sum(o => o.amount);
    }


    private void OnObstacleDestroyed(BlockType blockType)
    {
        Level.ObstacleData obstacle = levelManager.Level.obstacles.FirstOrDefault(o => o.blockType == blockType);

        obstacle.amount--;
        totalObstacleAmount--;
    }

    private void OnMatchFound()
    {
        levelManager.Level.moveCount--;
        
        if (totalObstacleAmount == 0 && levelManager.Level.moveCount >= 0)
        {
            onLevelFinished?.Invoke(true);
        }
        else if (totalObstacleAmount > 0 && levelManager.Level.moveCount == 0)
        {
            onLevelFinished?.Invoke(false);
        }
    }

}
