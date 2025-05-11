using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{   
    public string blockName;
    public BlockType blockType;
    public GameObject blockPrefab;
    public bool canFall;
    public Sprite defaultSprite;
    public Sprite[] frames;
}
