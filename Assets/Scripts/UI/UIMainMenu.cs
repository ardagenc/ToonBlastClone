using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button closeSettingsButton;
    [SerializeField] Button closeLevelsButton;
    [SerializeField] Button musicButton;
    [SerializeField] Button sfxButton;


    [Header("Panels")]
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject levelPanel;
    [SerializeField] GameObject levelContainer;

    [SerializeField] UILevelItem levelObject;

    private void OnEnable()
    {
        LevelProgress.onLevelProgressLoad += OnLevelProgressLoad;
    }

    private void OnDisable()
    {
        LevelProgress.onLevelProgressLoad -= OnLevelProgressLoad;


        playButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        closeSettingsButton.onClick.RemoveAllListeners();
        closeLevelsButton.onClick.RemoveAllListeners();
        sfxButton.onClick.RemoveAllListeners();
        musicButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        musicButton.onClick.AddListener(() => GameManager.Instance.SoundManager.ToggleSound());
        sfxButton.onClick.AddListener(() => GameManager.Instance.SoundManager.ToggleSFX());

        playButton.onClick.AddListener(() => TogglePanel(mainPanel, new Vector2(-1700, 0),true));
        playButton.onClick.AddListener(() => TogglePanel(levelPanel, new Vector2(1700, 0), true));
        playButton.onClick.AddListener(() => GameManager.Instance.SoundManager.PlaySFX());

        closeLevelsButton.onClick.AddListener(() => TogglePanel(mainPanel, Vector2.zero, true));
        closeLevelsButton.onClick.AddListener(() => TogglePanel(levelPanel, new Vector2(-1700, 0), true));
        closeLevelsButton.onClick.AddListener(() => GameManager.Instance.SoundManager.PlaySFX());

        settingsButton.onClick.AddListener(() => TogglePanel(settingsPanel, Vector2.zero, true));
        settingsButton.onClick.AddListener(() => GameManager.Instance.SoundManager.PlaySFX());

        closeSettingsButton.onClick.AddListener(() => TogglePanel(settingsPanel, new Vector2(0, 2600), false));
        closeSettingsButton.onClick.AddListener(() => GameManager.Instance.SoundManager.PlaySFX());
    }

    private void OnLevelProgressLoad(int levelCount, int progress)
    {
        for (int i = 0; i < levelCount; i++)
        {
            UILevelItem uILevelItem = Instantiate(levelObject, levelContainer.transform);
            uILevelItem.Init(i, GameManager.Instance.LevelProgress);
            if (i < progress)
            {
                uILevelItem.GetComponent<Button>().interactable = true;
            }
        }
    }
    public void TogglePanel(GameObject panel, Vector2 pos, bool toggle)
    {        
        if (toggle)
        {
            panel.SetActive(true);
            
            panel.transform.DOLocalMove(pos, 0.4f).SetEase(Ease.OutBounce);
        }
        else
        {
            panel.transform.DOLocalMove(pos, 0.4f).SetEase(Ease.OutQuint).OnComplete(() => panel.SetActive(false));
        }

    }
}
