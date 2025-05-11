using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelItem : MonoBehaviour
{
    LevelProgress levelProgress;
    int level;

    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI levelNumber;

    private void Start()
    {
        button.onClick.AddListener(SetSelectedLevel);
        button.onClick.AddListener(() => GameManager.Instance.NextLevel());
    }
    private void OnDisable()
    {
        button?.onClick.RemoveListener(SetSelectedLevel);
    }

    public void Init(int level, LevelProgress levelProgress)
    {
        this.level = level;
        levelNumber.text = this.level.ToString();
        this.levelProgress = levelProgress;
    }

    public void SetSelectedLevel()
    {
        levelProgress.SetLevel(level);
    }


}
