using System;
using System.Collections;
using UnityEngine;
public class Block : MonoBehaviour
{
    public static event Action<Block> BlockClicked;
    BlockData blockData;

    public SpriteRenderer spriteRenderer;
    public BlockType blockType;

    public Vector2Int gridPos;

    public virtual bool CanFall => blockData != null && blockData.canFall;

    public void Init(int x, int y, BlockData blockData)
    {
        gridPos = new Vector2Int(x, y);

        this.blockData = blockData;
        blockType = blockData.blockType;
        spriteRenderer.sprite = blockData.defaultSprite;
    }
    public void MoveTo(Vector2Int newPos)
    {
        gridPos = newPos;
        //transform.position = new Vector2(gridPos.x, gridPos.y);
        // Coroutine anim (Temporary)
        //StartCoroutine(MoveAnimation(gridPos));
    }
    private IEnumerator MoveAnimation(Vector2Int targetGridPos)
    {
        while (Vector2.Distance(transform.position, targetGridPos) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, targetGridPos, Time.deltaTime * 10f);
            yield return null;
        }

        transform.position = new Vector2(gridPos.x, gridPos.y);
    }
    public void Fall()
    {
        if (((Vector2)transform.position - (Vector2)gridPos).sqrMagnitude > 0.001f)
        {
            transform.position = Vector2.MoveTowards(transform.position, gridPos, 15 * Time.deltaTime);
        }
    }
    private void OnMouseDown()
    {
        BlockClicked?.Invoke(this);
    }

    private void Update()
    {
        if(!CanFall) return;
        Fall();
    }

}
