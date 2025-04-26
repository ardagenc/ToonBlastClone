using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static event Action onMatchFound;

    [SerializeField] private GridManager gridManager;
    [SerializeField] private Matchfinder matchFinder;
    [SerializeField] private BlockFactory blockFactory;
    [SerializeField] private BlockDatabase blockDatabase;

    private void OnEnable()
    {
        Block.onBlockClicked += OnBlockClicked;
        RocketPart.onRocketMove += OnRocketMove;
    }

    private void OnDisable()
    {
        Block.onBlockClicked -= OnBlockClicked;
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

    public bool MatchAndDestroy(Block clickedBlock)
    {
        var matchedBlocks = matchFinder.FindMatches(clickedBlock);

        if (matchedBlocks.Count >= 3)
        {
            HashSet<Block> uniqueBlocks = new HashSet<Block>();

            foreach (var block in matchedBlocks)
            {
                foreach (var neighbor in gridManager.GetAdjacentBlocks(block.GridPos))
                {
                    if (!uniqueBlocks.Contains(neighbor))
                        uniqueBlocks.Add(neighbor);
                }

                Destroy(block.gameObject);
                gridManager.SetBlockAt(block.GridPos, null);

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
            

            //foreach (var block in matchedBlocks)
            //{
            //    List<Block> adjacent = gridManager.GetAdjacentBlocks(block.GridPos);                

            //    foreach (Block neighbor in adjacent)
            //    {   
            //        // Think about IObstacle interface and damagetype enum.
            //        if (neighbor is ObstacleBlock box)
            //        {
            //            if (box.damagable)
            //                box.TakeDamage();                        
            //        }                
            //    }

            //    Destroy(block.gameObject);
            //    gridManager.SetBlockAt(block.GridPos, null);
            //}

            if (matchedBlocks.Count >= 4)
            {
                // Temporary
                BlockData rocketData = blockDatabase.blockDataList[6];

                RocketBlock rocket = (RocketBlock)blockFactory.CreateBlock(rocketData, clickedBlock.GridPos);

                gridManager.SetBlockAt(rocket.GridPos, rocket);
            }
            return true;
        }

        return false;
    }

    IEnumerator HandleMatchSequence(Block clickedBlock)
    {
        bool matchFound = MatchAndDestroy(clickedBlock);        

        yield return new WaitForSeconds(0.2f);
        gridManager.CollapseAll();
        gridManager.SpawnNewBlocks(blockFactory);

        if (matchFound) onMatchFound?.Invoke();

        yield return null;
    }

    IEnumerator HandleRocketSequence(Block clickedBlock)
    {
        List<Block> adjacent = gridManager.GetAdjacentBlocks(clickedBlock.GridPos);

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

        onMatchFound?.Invoke();
        yield return null;        
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
