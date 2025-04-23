using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Matchfinder matchFinder;
    [SerializeField] private BlockFactory blockFactory;
    [SerializeField] private BlockDatabase blockDatabase;

    private void OnEnable()
    {
        Block.BlockClicked += OnBlockClicked;
    }

    private void OnDisable()
    {
        Block.BlockClicked -= OnBlockClicked;
    }

    private void OnBlockClicked(Block clickedBlock)
    {
        if (clickedBlock is DefaultBlock)
        {
            StartCoroutine(HandleMatchSequence(clickedBlock));
        }
        else if (clickedBlock is RocketBlock)
        {
            Debug.Log("ClickedBlock is ROCKET");
            StartCoroutine(BlastColumn(clickedBlock));
        }


        //MatchAndDestroy(clickedBlock);
        //gridManager.CollapseAll();
        //gridManager.SpawnNewBlocks(blockFactory);
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
                    if (neighbor is BoxBlock box)
                    {                        
                        box.TakeDamage();                        
                    }
                }

                Destroy(block.gameObject);
                gridManager.SetBlockAt(block.gridPos, null);
            }

            if (matchedBlocks.Count >= 4)
            {
                // Temporary
                BlockData rocketData = blockDatabase.blockDataList[5];

                Block rocket = blockFactory.CreateBlock(rocketData, clickedBlock.gridPos);
                gridManager.SetBlockAt(rocket.gridPos, rocket);
            }
        }
    }

    IEnumerator HandleMatchSequence(Block clickedBlock)
    {
        MatchAndDestroy(clickedBlock);
        yield return new WaitForSeconds(0.2f);          // patlama efekti varsa bekle

        gridManager.CollapseAll();
        //yield return new WaitForSeconds(1f);          // blok düşme animasyonu varsa bekle

        gridManager.SpawnNewBlocks(blockFactory);
        yield return new WaitForSeconds(0.2f);

        //yield return StartCoroutine(CheckForMatchesAgain()); // zincirleme varsa
    }

    IEnumerator BlastColumn(Block clickedBlock)
    {
        int x = clickedBlock.gridPos.x;
        Destroy(clickedBlock.gameObject);
        // Yukarı
        for (int y = clickedBlock.gridPos.y + 1; y < gridManager.height; y++)
        {
            Vector2Int pos = new Vector2Int(x, y);
            Block b = gridManager.GetBlockAt(pos);

            if (b != null)
            {
                Destroy(b.gameObject);
                gridManager.SetBlockAt(pos, null);
            }

            yield return new WaitForSeconds(0.05f);
        }

        // Aşağı
        for (int y = clickedBlock.gridPos.y - 1; y >= 0; y--)
        {
            Vector2Int pos = new Vector2Int(x, y);
            Block b = gridManager.GetBlockAt(pos);

            if (b != null)
            {
                Destroy(b.gameObject);
                gridManager.SetBlockAt(pos, null);
            }

            yield return new WaitForSeconds(0.05f);
        }

        gridManager.CollapseAll();
        gridManager.SpawnNewBlocks(blockFactory);
    }

}
