using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifePart : MonoBehaviour
{
    public static event Action<Vector2Int, GameObject> onKnifeMove;
    public float speed = 5f;
    private Vector2 direction;
    private Vector2Int startPos;



    public void Initialize(Vector2 dir, Vector2Int startGridPos)
    {
        direction = dir;
        startPos = startGridPos;

        StartCoroutine(MoveAndDestroy());
    }

    IEnumerator MoveAndDestroy()
    {
        HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>();

        while (true)
        {
            Vector2 nextPos = transform.position + (Vector3)(direction * 1f);            
            transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

            Vector2Int gridPosToDestroy = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));

            if (!visitedPositions.Contains(gridPosToDestroy))
            {
                visitedPositions.Add(gridPosToDestroy);
                onKnifeMove?.Invoke(gridPosToDestroy, gameObject);
            }

            yield return null;
        }
    }
}
