using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    public TextMeshProUGUI MoveCount;

    public GameObject uiObstacleParent;
    public UIObstacle uiObstaclePrefab;
    public struct ObstacleData
    {
        public Sprite sprite;
        public int amount;
    }

    public List<ObstacleData> obstacleDatas;

    private void OnEnable()
    {
        SelectionManager.onMatchFound += OnMatchFound;
        LevelManager.onLevelGenerated += OnLevelGenerated;
    }

    private void OnDisable()
    {
        SelectionManager.onMatchFound -= OnMatchFound;
        LevelManager.onLevelGenerated -= OnLevelGenerated;
    }
    private void OnLevelGenerated()
    {
        MoveCount.text = "Move Count: " + levelManager.Level.moveCount;

        foreach (var obstacleData in levelManager.Level.obstacles)
        {
            if (obstacleData.amount > 0)
            {
                UIObstacle uiObstacle = Instantiate(uiObstaclePrefab);
                uiObstacle.transform.SetParent(uiObstacleParent.transform);

                uiObstacle.Init(obstacleData.sprite, obstacleData.amount, obstacleData.blockType);
            }
        }
    }
    public void OnMatchFound()
    {
        MoveCount.text = "Move Count: " + levelManager.Level.moveCount;
    }
}
