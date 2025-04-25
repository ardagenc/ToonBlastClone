using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    public TextMeshProUGUI MoveCount;

    private void OnEnable()
    {
        SelectionManager.onMatchFound += OnMatchFound;
        LevelManager.OnLevelGenerated += OnMatchFound;
    }

    private void OnDisable()
    {
        SelectionManager.onMatchFound -= OnMatchFound;
        LevelManager.OnLevelGenerated -= OnMatchFound;
    }
    private void OnMatchFound()
    {
        MoveCount.text = "Move Count: " + levelManager.Level.moveCount;
    }
}
