using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgress : MonoBehaviour
{
    public static event Action<int, int> onLevelProgressLoad;

    int progress;
    int levelCount;

    int selectedLevel;

    public int Progress { get => progress; set => progress = value; }
    public int LevelCount { get => levelCount; set => levelCount = value; }
    public int SelectedLevel { get => selectedLevel; set => selectedLevel = value; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        SaveData save = SaveManager.LoadProgress();
        Progress = save.unlockedLevel;

        if (scene.name == "MainMenu")
        {
            onLevelProgressLoad?.Invoke(LevelCount, Progress);
        }
    }

    private void Start()
    {
        LevelCount = Resources.LoadAll<TextAsset>("Levels/").Length;

        onLevelProgressLoad?.Invoke(LevelCount, Progress);
    }     
    public void SetLevel(int level)
    {
        SelectedLevel = level;
    }

    public int GetLevel()
    {
        return SelectedLevel;
    }
}
