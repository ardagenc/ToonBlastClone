using System;
using System.Collections;
using UnityEngine;
public class Block : MonoBehaviour
{
    public static event Action<Block> onBlockClicked;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected BlockData blockData;
    protected BlockType blockType;

    private Vector2Int gridPos;


    public virtual bool CanFall => blockData != null && blockData.canFall;

    public BlockType BlockType { get => blockType; set => blockType = value; }
    public Vector2Int GridPos { get => gridPos; set => gridPos = value; }

    public virtual void Init(int x, int y, BlockData blockData)
    {
        GridPos = new Vector2Int(x, y);

        this.blockData = blockData;
        BlockType = blockData.blockType;
        spriteRenderer.sprite = blockData.defaultSprite;
    }
    public void MoveTo(Vector2Int newPos)
    {
        GridPos = newPos;
        //StartCoroutine(MoveAnimation(gridPos));
        //transform.position = new Vector2(gridPos.x, gridPos.y);
        
    }
    private IEnumerator MoveAnimation(Vector2Int targetGridPos)
    {
        while (Vector2.Distance(transform.position, targetGridPos) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, targetGridPos, Time.deltaTime * 10f);
            yield return null;
        }

        transform.position = new Vector2(GridPos.x, GridPos.y);
    }
    public void Fall()
    {
        if (((Vector2)transform.position - (Vector2)GridPos).sqrMagnitude > 0.001f)
        {
            transform.position = Vector2.MoveTowards(transform.position, GridPos, 15 * Time.deltaTime);
        }
    }
    private void OnMouseDown()
    {
        onBlockClicked?.Invoke(this);
    }

    private void Update()
    {
        if(!CanFall) return;
        Fall();
    }

}
