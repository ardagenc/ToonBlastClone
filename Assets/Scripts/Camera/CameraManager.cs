using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float shakeDuration = 0.1f;
    [SerializeField] float shakeStrength = 0.5f;

    [SerializeField] GridManager gridManager;
    [SerializeField] Transform background;
    private void OnEnable()
    {
        Block.onBlockClicked += OnBlockClicked;
        LevelManager.onLevelGenerated += OnLevelGenerated;
    }
    private void OnDisable()
    {
        Block.onBlockClicked -= OnBlockClicked;
        LevelManager.onLevelGenerated -= OnLevelGenerated;
    }

    private void OnLevelGenerated()
    {
        Vector2 camPos = (gridManager.Blocks[0, 0].transform.position + gridManager.Blocks[gridManager.Width - 1, gridManager.Height - 1].transform.position) / 2;
        transform.position = (Vector3)camPos - Vector3.forward;
        background.position = camPos;
    }

    private void OnBlockClicked(Block block)
    {
        transform.DOShakePosition(shakeDuration, shakeStrength);
    }
}
