using System.Collections.Generic;
using UnityEngine;

public class BlockState
{
    public bool isVis;
    public List<BlockState> blocksAdj { get; private set; }
    public string blockId { get; private set; }
    public Vector3 blockPos { get; private set; }

    public BlockState(string id, Vector3 pos)
    {
        blockId = id;
        blockPos = pos;

        blocksAdj = new List<BlockState>();
    }

    public void ResetBlock()
    { 
    
    }

    public void AddAdjacent(BlockState blockState)
    {
        blocksAdj.Add(blockState);
    }
}