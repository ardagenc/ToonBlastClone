using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    static GameManager instance;
    [SerializeField] LevelProgress levelProgress;
    [SerializeField] SoundManager soundManager;

    public static GameManager Instance { get => instance; set => instance = value; }
    public LevelProgress LevelProgress { get => levelProgress; set => levelProgress = value; }
    public SoundManager SoundManager { get => soundManager; set => soundManager = value; }

    private void OnEnable()
    {
        GoalManager.onLevelFinished += OnLevelFinished;        

    }

    private void OnDisable()
    {
        GoalManager.onLevelFinished -= OnLevelFinished;
    }

    private void OnLevelFinished(bool win)
    {
        if (win)
        {
            Debug.Log("Saved!");
            SaveLevel();
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {   
        SaveManager.LoadProgress();
    }    
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainMenu");        
    }

    public void SaveLevel()
    {
        SaveData saveData = SaveManager.LoadProgress();        

        if (saveData.unlockedLevel == levelProgress.GetLevel() + 1 && saveData.unlockedLevel < levelProgress.LevelCount)
        {
            saveData.unlockedLevel += 1;
            SaveManager.SaveProgress(saveData);
        }

        
    }
    public void NextLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
