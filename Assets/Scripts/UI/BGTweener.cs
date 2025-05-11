using DG.Tweening;
using UnityEngine;

public class BGTweener : MonoBehaviour
{
    [SerializeField] float endPoint = 2200;
    [SerializeField] float duration = 30;
    private void Start()
    {
        Vector3[] loopPositions = { new Vector2(0, transform.localPosition.y),
            new Vector2(transform.localPosition.x, transform.localPosition.y),
            new Vector2(endPoint, transform.localPosition.y)};


        Sequence loopSequence = DOTween.Sequence();

        float actualDuration = (duration * (endPoint - transform.localPosition.x)) / endPoint;

        loopSequence.Append(transform.DOLocalMoveX(endPoint, actualDuration).SetEase(Ease.Linear).OnComplete(() => transform.localPosition = loopPositions[0]));
        loopSequence.Append(transform.DOLocalPath(loopPositions, duration, PathType.Linear, PathMode.Ignore).SetEase(Ease.Linear).SetLoops(-1));
    }
}
