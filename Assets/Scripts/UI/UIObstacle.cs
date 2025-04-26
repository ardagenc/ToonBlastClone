using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIObstacle : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI amountText;
    BlockType blockType;
    int amount;
    private void OnEnable()
    {
        ObstacleBlock.onObstacleDestroyed += OnObstacleDestroyed;
    }
    private void OnDisable()
    {
        ObstacleBlock.onObstacleDestroyed -= OnObstacleDestroyed;
    }

    public void Init(Sprite sprite, int amount, BlockType blocktype)
    {
        this.blockType = blocktype;
        image.sprite = sprite;
        this.amount = amount;
        amountText.text = amount.ToString();
    }
    private void OnObstacleDestroyed(BlockType type)
    {
        if (blockType == type)
        {
            amount--;
            amountText.text = amount.ToString();
        }

    }
}
