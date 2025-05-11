using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour, IPoolable
{
    public static event Action<Block> onBlockClicked;

    [SerializeField] private BlockAnimation blockAnimation;
    protected BlockData blockData;
    protected BlockType blockType;

    [SerializeField] Transform spriteTransform;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] float autoTiltAmount;
    [SerializeField] float tiltAmount;
    [SerializeField] float tiltSpeed;

    public Vector2Int gridPos;
    float tiltX;
    float tiltY;

    bool isHovering = false;
    bool isFalling = false;

    public virtual IEnumerator OnSelected(MatchManager manager) => null;

    public virtual bool CanFall => blockData != null && blockData.canFall;

    public BlockType BlockType { get => blockType; set => blockType = value; }
    public Vector2Int GridPos { get => gridPos; set => gridPos = value; }
    public BlockAnimation BlockAnimation { get => blockAnimation; set => blockAnimation = value; }

    public virtual void Init(int x, int y, BlockData blockData)
    {
        GridPos = new Vector2Int(x, y);

        this.blockData = blockData;
        transform.name = blockData.name;
        BlockType = blockData.blockType;
        BlockAnimation.Init(blockData);
    }
    public void MoveTo(Vector2Int newPos)
    {
        GridPos = newPos;
        Vector2 targetGridPos = GridPos;

        if (targetGridPos != GridPos)
        {
            isFalling = true;
        }
        if (gameObject != null)
        {
            transform.DOMove(targetGridPos, 0.4f).SetEase(Ease.OutBounce).OnComplete(() => isFalling = false);
        }
    }
    public void DisableCollider()
    {
        boxCollider.enabled = false;
    }

    private void OnMouseEnter()
    {
        if (gameObject == null) return;

        isHovering = true;
        if (!isFalling)
        {            
            transform.DOShakeScale(0.1f, 0.1f);
            transform.DOShakePosition(0.1f, 0.1f);
            transform.DOScale(1.1f, 0.1f);
        }
    }

    private void OnMouseExit()
    {
        if (gameObject == null) return;

        isHovering = false;
        transform.DOMove((Vector2)gridPos, 0.1f);
        transform.DOScale(1.0f, 0.1f);
    }

    private void OnMouseDown()
    {
        onBlockClicked?.Invoke(this);        
    }

    private void Update()
    {
        AutoTilt();
        //ManuelTilt();
    }

    public void AutoTilt()
    {
        if (isHovering) return;

        float sine = Mathf.Sin(Time.fixedTime + gridPos.x);
        float cosine = Mathf.Cos(Time.fixedTime + gridPos.y);

        spriteTransform.eulerAngles = new Vector3(sine * autoTiltAmount, cosine * autoTiltAmount, spriteTransform.eulerAngles.z);
    }
    public void ManuelTilt()
    {
        if (isFalling) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotationValue = (mousePosition - spriteTransform.position).normalized;
        rotationValue.z = 0;

        if (isHovering)
        {
            Vector3 tilt = new Vector3(rotationValue.y * tiltAmount * 10, rotationValue.x * -tiltAmount * 10, spriteTransform.eulerAngles.z);

            tiltX = Mathf.LerpAngle(spriteTransform.eulerAngles.x, tilt.x, tiltSpeed * Time.deltaTime);
            tiltY = Mathf.LerpAngle(spriteTransform.eulerAngles.y, tilt.y, tiltSpeed * Time.deltaTime);


        }
        else
        {
            tiltX = Mathf.LerpAngle(spriteTransform.eulerAngles.x, 0, tiltSpeed * Time.deltaTime);
            tiltY = Mathf.LerpAngle(spriteTransform.eulerAngles.y, 0, tiltSpeed * Time.deltaTime);

        }
        spriteTransform.eulerAngles = new Vector3(tiltX, tiltY, spriteTransform.eulerAngles.z);
    }

    public void OnReturnedToPool()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        GridPos = Vector2Int.zero;
        blockData = null;
    }

    public void OnTakenFromPool()
    {
        
    }
}
