using UnityEngine;

public class Grid : MonoBehaviour
{
    Block block;

    public int x;
    public int y;

    private void Awake()
    {
        block = GetComponent<Block>();
    }
    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;

        SetGridName();
    }

    public void SetGridName()
    {
        string gridName = x + "_" + y;
        gameObject.name = gridName;
    }

}
