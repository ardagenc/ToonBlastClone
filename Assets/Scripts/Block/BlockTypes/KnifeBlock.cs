using System.Collections;
using System.Net.Sockets;
using UnityEngine;

public enum RocketDirection { Horizontal, Vertical }
public class KnifeBlock : Block
{    
    private Vector2 rocketDirection;
    public RocketDirection Direction;
    public GameObject rocketPartPrefab;

    public override void Init(int x, int y, BlockData blockData) 
    {
        GridPos = new Vector2Int(x, y);

        this.blockData = blockData;
        BlockType = blockData.blockType;
        BlockAnimation.Init(blockData);


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
        SpawnRocket(transform.position, rocketDirection, GridPos);
        SpawnRocket(transform.position, -rocketDirection, GridPos);
    }

    public void ComboKnifeActivate()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = (Vector3.left + Vector3.right * i);

            SpawnRocket(transform.position - offset, Vector2.up, GridPos);
            SpawnRocket(transform.position - offset, Vector2.down, GridPos);
        }
        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = (Vector3.down + Vector3.up * i);

            SpawnRocket(transform.position - offset, Vector2.right, GridPos);
            SpawnRocket(transform.position - offset, Vector2.left, GridPos);
        }
    }

    private void SpawnRocket(Vector3 position, Vector2 direction, Vector2Int gridPos)
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        GameObject rocket = Instantiate(rocketPartPrefab, position, rotation);
        KnifePart rocketPart = rocket.GetComponent<KnifePart>();
        rocketPart.Initialize(direction, gridPos);
    }

    public override IEnumerator OnSelected(MatchManager manager)
    {
        yield return manager.HandleKnifeSequence(this);
    }
}
