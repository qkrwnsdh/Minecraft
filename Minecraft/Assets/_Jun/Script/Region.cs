using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class Region
{
    public string regionType { get; private set; }
    public Region[,,] regionsAdjacents { get; private set; }
    public Block[,,] regionBlocks { get; private set; }

    public GameObject regionObject { get; private set; }
    public Vector3 regionPosition { get; private set; }
    private IRegionTypeCommand command;

    public Region(Vector3 regionPosition, Transform regionPaent)
    {
        this.regionPosition = regionPosition * Define.REGION_IN_BLOCK_DIM;

        SetInstance();
        SetRegionType();
        SetRegionObject(regionPaent);
    }

    private void SetInstance()
    {
        regionsAdjacents = new Region[3, 3, 3];
        regionBlocks = new Block[Define.REGION_IN_BLOCK_DIM, Define.REGION_IN_BLOCK_DIM, Define.REGION_IN_BLOCK_DIM];
    }

    private void SetRegionType()
    {
        if (regionPosition.y == Define.REGION_IN_BLOCK_DIM * (Define.REGION_DIM - 1))
        {
            string[] biomes = { "Desert", "Ocean", "Veld" };

            regionType = biomes[UnityEngine.Random.Range(0, biomes.Length)];
        }
        else
        {
            regionType = "Stone";
        }
    }

    private void SetRegionObject(Transform regionPaent)
    {
        regionObject = new GameObject
            ($"{regionType}_({(int)regionPosition.x}_{(int)regionPosition.y}_{(int)regionPosition.z})");
        regionObject.transform.position = regionPosition;
        regionObject.transform.parent = regionPaent;
    }

    public void GeneratorBlocksData()
    {
        SetRegionAdjacent();
        SetCommandExecute();
    }

    public void SetRegionAdjacent()
    {
        Vector3 thisRegion = regionPosition / Define.REGION_IN_BLOCK_DIM;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    int xAdj = (int)thisRegion.x + x;
                    int yAdj = (int)thisRegion.y + y;
                    int zAdj = (int)thisRegion.z + z;

                    regionsAdjacents[x + 1, y + 1, z + 1] = MapGenerator.Instance.GetAdjacencyRegion(xAdj, yAdj, zAdj);
                }
            }
        }
    }

    private void SetCommandExecute()
    {
        switch (regionType)
        {
            case "Desert": command = new DesertCommand(); break;
            case "Ocean": command = new OceanCommand(); break;
            case "Veld": command = new VeldCommand(); break;
            case "Stone": command = new StoneCommand(); break;
        }

        regionBlocks = command.Execute(this);
    }

    public void BlockAdjacents()
    {
        BlockAction(AddBlockAdjacents);
    }     
    public void BlockVisibles()
    {
        BlockAction(SetBlockVisibles);
    }     

    private void BlockAction(Action<Block> action)
    {
        int xLength = regionBlocks.GetLength(0);
        int yLength = regionBlocks.GetLength(1);
        int zLength = regionBlocks.GetLength(2);

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    if (regionBlocks[x, y, z] != null)
                    {
                        action(regionBlocks[x, y, z]);
                    }
                }
            }
        }
    }

    private void AddBlockAdjacents(Block block)
    {
        int rayLength = 1;
        int layerMask = LayerMask.GetMask("Block");

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;

            if (Physics.Raycast(block.blockPosition, direction, out hit, rayLength, layerMask))
            {
                Block adjacentBlock = hit.transform.GetComponent<Block>();

                if (adjacentBlock != null)
                {
                    block.AddAdjacent(adjacentBlock);
                }
            }
        }
    }

    private void SetBlockVisibles(Block block)
    {
        block.gameObject.SetActive(block.blockAdjacents.Count == 6 ? false : true);
    }
}