using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    [Header("Top Panel")]
    [SerializeField] TextMeshProUGUI MoveCount;
    [SerializeField] GameObject uiObstacleParent;
    [SerializeField] UIObstacle uiObstaclePrefab;

    [Header("Win Lose Panel")]
    [SerializeField] Button nextLevelButton;
    [SerializeField] GameObject winLosePanel;
    [SerializeField] TextMeshProUGUI winLoseText;
    [SerializeField] TextMeshProUGUI winLoseButtonText;
    [SerializeField] float tweenTime = 0.4f;
    public struct ObstacleData
    {
        public Sprite sprite;
        public int amount;
    }

    public List<ObstacleData> obstacleDatas;

    private void OnEnable()
    {
        MatchManager.onMatchFound += OnMatchFound;
        LevelManager.onLevelGenerated += OnLevelGenerated;
        GoalManager.onLevelFinished += OnLevelFinished;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        MatchManager.onMatchFound -= OnMatchFound;
        LevelManager.onLevelGenerated -= OnLevelGenerated;
        GoalManager.onLevelFinished -= OnLevelFinished;

        SceneManager.sceneLoaded -= OnSceneLoaded;

        nextLevelButton.onClick.RemoveAllListeners();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SaveData saveData = SaveManager.LoadProgress();
        int level = GameManager.Instance.LevelProgress.SelectedLevel;
        int levelCount = GameManager.Instance.LevelProgress.LevelCount;
        Debug.Log(level);
        Debug.Log(saveData.unlockedLevel);
        Debug.Log(levelCount);

        if (level < saveData.unlockedLevel && level + 1 < levelCount)
        {
            nextLevelButton.onClick.AddListener(() => GameManager.Instance.LevelProgress.SetLevel(level + 1));
            nextLevelButton.onClick.AddListener(() => GameManager.Instance.NextLevel());
        }
    }


    private void Start()
    {
        winLosePanel.SetActive(false);
    }
    private void OnLevelGenerated()
    {
        winLosePanel.SetActive(false);
        winLosePanel.transform.localPosition = Vector3.up * 2100f;

        MoveCount.text = "Move Count: " + levelManager.Level.moveCount;

        foreach (var obstacleData in levelManager.Level.obstacles)
        {
            if (obstacleData.amount > 0)
            {
                UIObstacle uiObstacle = Instantiate(uiObstaclePrefab);                
                uiObstacle.transform.SetParent(uiObstacleParent.transform);
                uiObstacle.transform.localScale = Vector3.one;

                uiObstacle.Init(obstacleData.sprite, obstacleData.amount, obstacleData.blockType);
            }
        }
    }
    public void OnMatchFound()
    {
        MoveCount.text = "Move Count: " + levelManager.Level.moveCount;
    }

    private void OnLevelFinished(bool win)
    {
        winLosePanel.SetActive(true);
        winLosePanel.transform.DOLocalMoveY(0, tweenTime).SetEase(Ease.OutBounce);

        if (win)
        {            
            winLoseText.text = "Level Completed!";
            //winLoseButtonText.text = "Next Level";
        }
        else
        {            
            winLoseText.text = "Retry!!";
            //winLoseButtonText.text = "Retry Level";
        }
    }
}
