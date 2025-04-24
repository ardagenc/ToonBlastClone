using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Matchfinder matchFinder;
    [SerializeField] private BlockFactory blockFactory;
    [SerializeField] private BlockDatabase blockDatabase;

    private void OnEnable()
    {
        Block.BlockClicked += OnBlockClicked;
        RocketPart.onRocketMove += OnRocketMove;
    }

    private void OnDisable()
    {
        Block.BlockClicked -= OnBlockClicked;
        RocketPart.onRocketMove -= OnRocketMove;
    }

    private void OnBlockClicked(Block clickedBlock)
    {
        if (clickedBlock is DefaultBlock)
        {
            StartCoroutine(HandleMatchSequence(clickedBlock));
        }
        else if (clickedBlock is RocketBlock)
        {            
            StartCoroutine(HandleRocketSequence(clickedBlock));
        }
    }

    public void MatchAndDestroy(Block clickedBlock)
    {
        var matchedBlocks = matchFinder.FindMatches(clickedBlock);

        if (matchedBlocks.Count >= 3)
        {
            foreach (var block in matchedBlocks)
            {
                List<Block> adjacent = gridManager.GetAdjacentBlocks(block.gridPos);


                foreach (Block neighbor in adjacent)
                {
                    // Think about IObstacle interface and damagetype enum.
                    if (neighbor is ObstacleBlock box)
                    {         
                        if (box.damagable)
                            box.TakeDamage();                        
                    }
                }

                Destroy(block.gameObject);
                gridManager.SetBlockAt(block.gridPos, null);
            }

            if (matchedBlocks.Count >= 4)
            {
                // Temporary
                BlockData rocketData = blockDatabase.blockDataList[6];

                RocketBlock rocket = (RocketBlock)blockFactory.CreateBlock(rocketData, clickedBlock.gridPos);

                gridManager.SetBlockAt(rocket.gridPos, rocket);
            }
        }
    }

    IEnumerator HandleMatchSequence(Block clickedBlock)
    {
        MatchAndDestroy(clickedBlock);

        yield return new WaitForSeconds(0.2f);
        gridManager.CollapseAll();
        gridManager.SpawnNewBlocks(blockFactory);
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator HandleRocketSequence(Block clickedBlock)
    {
        List<Block> adjacent = gridManager.GetAdjacentBlocks(clickedBlock.gridPos);

        RocketBlock rocket = (RocketBlock)clickedBlock;
        bool hasAdjacentRocket = false;

        foreach (var block in adjacent)
        {
            if (block is RocketBlock)
            {
                hasAdjacentRocket = true;
                break;
            }
        }

        if (hasAdjacentRocket)
        {
            rocket.ComboRocketActivate();
        }
        else
        {
            rocket.Activate();
        }

        yield return new WaitForSeconds(0.2f);
        gridManager.CollapseAll();
        gridManager.SpawnNewBlocks(blockFactory);
        yield return new WaitForSeconds(0.2f);        
    }
    public void OnRocketMove(Vector2Int gridPos, GameObject rocketPart)
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
                    Destroy(block.gameObject);
                    gridManager.SetBlockAt(gridPos, null);
                }
                
            }
        }
    }

}
