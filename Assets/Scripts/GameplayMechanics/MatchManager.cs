using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MatchManager : MonoBehaviour
{
    public static event Action onMatchFound;

    [SerializeField] private GridManager gridManager;
    [SerializeField] private Matchfinder matchFinder;
    [SerializeField] private BlockFactory blockFactory;
    [SerializeField] private BlockDatabase blockDatabase;

    [SerializeField] float effectDelay;

    private void OnEnable()
    {
        Block.onBlockClicked += OnBlockClicked;
        KnifePart.onKnifeMove += OnKnifeMove;
    }

    private void OnDisable()
    {
        Block.onBlockClicked -= OnBlockClicked;
        KnifePart.onKnifeMove -= OnKnifeMove;
    }

    private void OnBlockClicked(Block clickedBlock)
    {        
        StartCoroutine(clickedBlock.OnSelected(this));
    }

    public bool MatchAndDestroy(Block clickedBlock)
    {
        var matchedBlocks = matchFinder.FindMatches(clickedBlock);

        if (matchedBlocks.Count >= 2)
        {
            HashSet<Block> uniqueBlocks = new HashSet<Block>();

            foreach (var block in matchedBlocks)
            {
                foreach (var neighbor in gridManager.GetAdjacentBlocks(block.GridPos))
                {
                    if (!uniqueBlocks.Contains(neighbor))
                        uniqueBlocks.Add(neighbor);
                }             

            }
            foreach (Block neighbor in uniqueBlocks)
            {
                // Think about IObstacle interface and damagetype enum.
                if (neighbor is ObstacleBlock box)
                {
                    if (box.damagable)
                        box.TakeDamage();
                }
            }

            StartCoroutine(HandleBlockDestroyAnimation(matchedBlocks, clickedBlock));

            if (matchedBlocks.Count >= 4)
            {
                // Temporary
                if(clickedBlock.BlockType == BlockType.Apple)
                {
                    BlockData knifeData = blockDatabase.blockDataList[6];
                    KnifeBlock knife = (KnifeBlock)blockFactory.CreateBlock(knifeData, clickedBlock.GridPos);                   

                    gridManager.SetBlockAt(knife.GridPos, knife);
                }
                else if (clickedBlock is DefaultBlock && clickedBlock.BlockType != BlockType.Apple)
                {
                    BlockData juiceData = blockDatabase.blockDataList[7];
                    JuiceBlock juice = (JuiceBlock)blockFactory.CreateBlock(juiceData, clickedBlock.GridPos);

                    gridManager.SetBlockAt(juice.GridPos, juice);
                }
                
            }
            return true;
        }

        return false;
    }

    public IEnumerator HandleBlockDestroyAnimation(List<Block> matchingBlocks, Block clickedBlock)
    {
        bool isSpecialMatch = matchingBlocks.Count >= 4;

        foreach (var block in matchingBlocks)
        {
            gridManager.SetBlockAt(block.GridPos, null);
            block.DisableCollider(); // To avoid Dotween Errors.            
            if (isSpecialMatch)
            {
                
                block.MoveTo(clickedBlock.GridPos);
            }

        }

        yield return new WaitForSeconds(0.42f);


        foreach (var block in matchingBlocks)
        {
            if (block != null)
            {
                blockFactory.ReturnBlock(block);
            }
        }

        yield return null;
    }

    public IEnumerator HandleMatchSequence(Block clickedBlock)
    {
        bool matchFound = MatchAndDestroy(clickedBlock);        

        yield return CollapseAndRefill(matchFound);        
    }

    public IEnumerator HandleKnifeSequence(Block clickedBlock)
    {
        List<Block> adjacent = gridManager.GetAdjacentBlocks(clickedBlock.GridPos);

        KnifeBlock knife = (KnifeBlock)clickedBlock;
        bool hasAdjacentKnife = false;

        foreach (var block in adjacent)
        {
            if (block is KnifeBlock)
            {
                hasAdjacentKnife = true;
                break;
            }
        }

        if (hasAdjacentKnife)
        {
            knife.ComboKnifeActivate();
        }
        else
        {
            knife.Activate();
        }

        yield return CollapseAndRefill(true);
    }
    
    public IEnumerator HandleJuiceSequence(Block clickedBlock)
    {
        List<Block> adjacent = gridManager.GetAdjacentBlocks(clickedBlock.GridPos);
        JuiceBlock juice = (JuiceBlock)clickedBlock;

        bool hasAdjacentJuice = false;

        foreach (var block in adjacent)
        {
            if (block is JuiceBlock)
            {
                hasAdjacentJuice = true;
                break;
            }
        }

        int area = hasAdjacentJuice ? 3 : 2;
        int startPosition = hasAdjacentJuice ? -2 : -1;


        for (int x = startPosition; x < area; x++)
        {
            for(int y = startPosition; y < area; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (!gridManager.IsInBounds(clickedBlock.GridPos + pos)) continue;

                if (gridManager.GetBlockAt(clickedBlock.GridPos + pos) is ObstacleBlock obstacleBlock)
                {
                    obstacleBlock.TakeDamage();
                }
                else
                {
                    if (gridManager.GetBlockAt(clickedBlock.GridPos + pos) != null)
                    {
                        //Destroy(gridManager.GetBlockAt(clickedBlock.GridPos + pos).gameObject);
                        blockFactory.ReturnBlock(gridManager.GetBlockAt(clickedBlock.GridPos + pos));
                        gridManager.SetBlockAt(clickedBlock.GridPos + pos, null);
                    }
                }
            }
        }

        yield return CollapseAndRefill(true);

    }
    public void OnKnifeMove(Vector2Int gridPos, GameObject rocketPart)
    {
        if (!gridManager.IsInBounds(gridPos))
        {
            Destroy(rocketPart);
        }
        else
        {
            Block block = gridManager.GetBlockAt(gridPos);
            if (block != null)
            {
                if (block is ObstacleBlock obstacle)
                {
                    obstacle.TakeDamage();
                }
                else
                {
                    //Destroy(block.gameObject);
                    blockFactory.ReturnBlock(block);
                    gridManager.SetBlockAt(gridPos, null);
                }
                
            }
        }
    }
        
    public void HandleMatches()
    {
        var matches = matchFinder.FindAllMatches();
        
        foreach (var block in gridManager.Blocks)
        {
            if (block == null) continue;
            if (block.BlockAnimation.frames == null) continue;  

            if (block.BlockAnimation.frames.Length != 0)
            {
                block.BlockAnimation.Stop();
            }
        }

        foreach (List<Block> list in matches)
        {
            foreach (Block block in list)
            {
                if (block.BlockAnimation.frames.Length > 0)
                {
                    block.BlockAnimation.Play();
                }

            }
        }
    }

    IEnumerator CollapseAndRefill(bool matchFound)
    {
        yield return new WaitForSeconds(effectDelay);
        gridManager.CollapseAll();
        gridManager.SpawnNewBlocks(blockFactory);

        if (matchFound) onMatchFound?.Invoke();
        yield return new WaitForSeconds(effectDelay);
        HandleMatches();
    }

}
