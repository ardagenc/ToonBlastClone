using System.Collections;
using System.Net.Sockets;
using UnityEngine;

public enum RocketDirection { Horizontal, Vertical }
public class RocketBlock : Block
{
    private Vector2 rocketDirection;
    public RocketDirection Direction;
    public GameObject rocketPartPrefab;

    public override void Init(int x, int y, BlockData blockData) 
    {
        gridPos = new Vector2Int(x, y);

        this.blockData = blockData;
        blockType = blockData.blockType;
        spriteRenderer.sprite = blockData.defaultSprite;
        Direction = (RocketDirection)Random.Range(0, 2);

        if (Direction == RocketDirection.Horizontal)
        {
            rocketDirection = Vector2.right;
            transform.eulerAngles += Vector3.forward * -90;
        }
        else
        {
            rocketDirection = Vector2.up;
        }

    }
    public void Activate()
    {
        GameObject rocket1 = Instantiate(rocketPartPrefab, transform.position, Quaternion.identity);
        RocketPart rocketPart1 = rocket1.GetComponent<RocketPart>();
        rocketPart1.Initialize(rocketDirection, gridPos);

        GameObject rocket2 = Instantiate(rocketPartPrefab, transform.position, Quaternion.identity);
        RocketPart rocketPart2 = rocket2.GetComponent<RocketPart>();
        rocketPart2.Initialize(-rocketDirection, gridPos);
    }

    public void ComboRocketActivate()
    {
        Debug.Log("ComboRocketActivate");

        for (int i = 0; i < 3; i++)
        {
            GameObject rocket1 = Instantiate(rocketPartPrefab, transform.position - (Vector3.left + Vector3.right * i), Quaternion.identity);
            RocketPart rocketPart1 = rocket1.GetComponent<RocketPart>();
            rocketPart1.Initialize(Vector2.up, gridPos);

            GameObject rocket2 = Instantiate(rocketPartPrefab, transform.position - (Vector3.left + Vector3.right * i), Quaternion.identity);
            RocketPart rocketPart2 = rocket2.GetComponent<RocketPart>();
            rocketPart2.Initialize(-Vector2.up, gridPos);
        }
        for (int i = 0; i < 3; i++)
        {
            GameObject rocket1 = Instantiate(rocketPartPrefab, transform.position - (Vector3.down + Vector3.up * i), Quaternion.identity);
            RocketPart rocketPart1 = rocket1.GetComponent<RocketPart>();
            rocketPart1.Initialize(Vector2.right, gridPos);

            GameObject rocket2 = Instantiate(rocketPartPrefab, transform.position - (Vector3.down + Vector3.up * i), Quaternion.identity);
            RocketPart rocketPart2 = rocket2.GetComponent<RocketPart>();
            rocketPart2.Initialize(-Vector2.right, gridPos);
        }
    }
}
