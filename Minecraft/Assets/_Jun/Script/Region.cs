using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public string regionType { get; private set; }
    public Region[,,] regionsAdj { get; private set; }
    public BlockState[,,] regionBlockStates { get; private set; }

    private GameObject regionObj;
    private Vector3 regionPos;
    private IRegionTypeCommand command;

    public Region(Vector3 regionPos, Transform regionPaent)
    {
        this.regionPos = regionPos * Define.REGION_IN_BLOCK_DIM;

        SetInstance();
        SetRegionType();
        SetRegionObject(regionPaent);
    }

    private void SetInstance()
    {
        regionsAdj = new Region[3, 3, 3];
        regionBlockStates = new BlockState[Define.REGION_IN_BLOCK_DIM, Define.REGION_IN_BLOCK_DIM, Define.REGION_IN_BLOCK_DIM];
    }

    private void SetRegionType()
    {
        if (regionPos.y == Define.REGION_IN_BLOCK_DIM * (Define.REGION_DIM - 1))
        {
            string[] biomes = { "Desert", "Ocean", "Veld" };

            regionType = biomes[Random.Range(0, biomes.Length)];
        }
        else
        {
            regionType = "Stone";
        }
    }

    private void SetRegionObject(Transform regionPaent)
    {
        regionObj = new GameObject($"{regionType}_({(int)regionPos.x}_{(int)regionPos.y}_{(int)regionPos.z})");
        regionObj.transform.position = regionPos;
        regionObj.transform.parent = regionPaent;
    }

    public void GeneratorBlocksData()
    {
        SetRegionAdjacent();
        SetCommandExecute();
        SetBlockVisible();
    }

    private void SetRegionAdjacent()
    {
        Vector3 thisRegion = regionPos / Define.REGION_IN_BLOCK_DIM;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    int xAdj = (int)thisRegion.x + x;
                    int yAdj = (int)thisRegion.y + y;
                    int zAdj = (int)thisRegion.z + z;

                    regionsAdj[x + 1, y + 1, z + 1] = MapGenerator.Instance.GetAdjacencyRegion(xAdj, yAdj, zAdj);
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

        regionBlockStates = command.Execute(this, Define.REGION_IN_BLOCK_DIM);
    }

    private void SetBlockVisible()
    {
        int xLength = regionBlockStates.GetLength(0);
        int yLength = regionBlockStates.GetLength(1);
        int zLength = regionBlockStates.GetLength(2);

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    if (regionBlockStates[x, y, z] != null)
                    {
                        AddBlockAdjacent(regionBlockStates[x, y, z], x, y, z, Define.REGION_IN_BLOCK_DIM);
                        SetBlockVisible(regionBlockStates[x, y, z], x, y, z, Define.REGION_IN_BLOCK_DIM);
                    }
                }
            }
        }
    }

    private void AddBlockAdjacent(BlockState block, int x, int y, int z, int dim)
    {
        if (x == 0)
        {
            if (regionsAdj[2, 1, 1] != null && regionsAdj[2, 1, 1].regionBlockStates[dim - 1, y, z] != null)
            {
                block.AddAdjacent(regionsAdj[2, 1, 1].regionBlockStates[dim - 1, y, z]);
            }
        }
        if (x == dim - 1)
        {
            if (regionsAdj[0, 1, 1] != null && regionsAdj[0, 1, 1].regionBlockStates[0, y, z] != null)
            {
                block.AddAdjacent(regionsAdj[0, 1, 1].regionBlockStates[0, y, z]);
            }
        }
        if (y == 0)
        {
            if (regionsAdj[1, 2, 1] != null && regionsAdj[1, 2, 1].regionBlockStates[x, dim - 1, z] != null)
            {
                block.AddAdjacent(regionsAdj[1, 2, 1].regionBlockStates[x, dim - 1, z]);
            }
        }
        if (y == dim - 1)
        {
            if (regionsAdj[1, 0, 1] != null && regionsAdj[1, 0, 1].regionBlockStates[x, 0, z] != null)
            {
                block.AddAdjacent(regionsAdj[1, 0, 1].regionBlockStates[x, 0, z]);
            }
        }
        if (z == 0)
        {
            if (regionsAdj[1, 1, 2] != null && regionsAdj[1, 1, 2].regionBlockStates[x, y, dim - 1] != null)
            {
                block.AddAdjacent(regionsAdj[1, 1, 2].regionBlockStates[x, y, dim - 1]);
            }
        }
        if (z == dim - 1)
        {
            if (regionsAdj[1, 1, 0] != null && regionsAdj[1, 1, 0].regionBlockStates[x, y, 0] != null)
            {
                block.AddAdjacent(regionsAdj[1, 1, 0].regionBlockStates[x, y, 0]);
            }
        }
        if (0 < x && x < dim - 1 && 0 < y && y < dim - 1 && 0 < z && z < dim - 1)
        {

            int[] xD = { -1, 1, 0, 0, 0, 0 };
            int[] yD = { 0, 0, -1, 1, 0, 0 };
            int[] zD = { 0, 0, 0, 0, -1, 1 };

            for (int i = 0; i < 6; i++)
            {
                int xA = x + xD[i];
                int yA = y + yD[i];
                int zA = z + zD[i];

                if (regionsAdj[1, 1, 1].regionBlockStates[xA, yA, zA] != null)
                {
                    block.AddAdjacent(regionsAdj[1, 1, 1].regionBlockStates[xA, yA, zA]);
                }
            }
        }
    }

    private void SetBlockVisible(BlockState block, int x, int y, int z, int dim)
    {
        int allAdjacentsPresent = 0;

        if (x == 0)
        {
            if (regionsAdj[2, 1, 1] != null && regionsAdj[2, 1, 1].regionBlockStates[dim - 1, y, z] != null)
            {
                allAdjacentsPresent += 1;
            }
        }
        if (x == dim - 1)
        {
            if (regionsAdj[0, 1, 1] != null && regionsAdj[0, 1, 1].regionBlockStates[0, y, z] != null)
            {
                allAdjacentsPresent += 1;
            }
        }
        if (y == 0)
        {
            if (regionsAdj[1, 2, 1] != null && regionsAdj[1, 2, 1].regionBlockStates[x, dim - 1, z] != null)
            {
                allAdjacentsPresent += 1;
            }
        }
        if (y == dim - 1)
        {
            if (regionsAdj[1, 0, 1] != null && regionsAdj[1, 0, 1].regionBlockStates[x, 0, z] != null)
            {
                allAdjacentsPresent += 1;
            }
        }
        if (z == 0)
        {
            if (regionsAdj[1, 1, 2] != null && regionsAdj[1, 1, 2].regionBlockStates[x, y, dim - 1] != null)
            {
                allAdjacentsPresent += 1;
            }
        }
        if (z == dim - 1)
        {
            if (regionsAdj[1, 1, 0] != null && regionsAdj[1, 1, 0].regionBlockStates[x, y, 0] != null)
            {
                allAdjacentsPresent += 1;
            }
        }
        if (0 < x && x < dim - 1 && 0 < y && y < dim - 1 && 0 < z && z < dim - 1)
        {

            int[] xD = { -1, 1, 0, 0, 0, 0 };
            int[] yD = { 0, 0, -1, 1, 0, 0 };
            int[] zD = { 0, 0, 0, 0, -1, 1 };

            for (int i = 0; i < 6; i++)
            {
                int xA = x + xD[i];
                int yA = y + yD[i];
                int zA = z + zD[i];

                if (regionsAdj[1, 1, 1].regionBlockStates[xA, yA, zA] != null)
                {
                    allAdjacentsPresent += 1;
                }
            }
        }

        block.isVis = allAdjacentsPresent != 6 ? true : false;
    }

    //private void SetBlockVisible(BlockState block, int x, int y, int z, int dim)
    //{
    //    bool allAdjacentsPresent = true;

    //    int[] xD = { -1, 1, 0, 0, 0, 0 };
    //    int[] yD = { 0, 0, -1, 1, 0, 0 };
    //    int[] zD = { 0, 0, 0, 0, -1, 1 };

    //    for (int i = 0; i < 6; i++)
    //    {
    //        int xA = x + xD[i];
    //        int yA = y + yD[i];
    //        int zA = z + zD[i];

    //        if (xA < 0 || xA >= dim || yA < 0 || yA >= dim || zA < 0 || zA >= dim)
    //        {
    //            // Out of bounds, check adjacent regions
    //            if (xA < 0 && (regionsAdj[2, 1, 1] == null || regionsAdj[2, 1, 1].regionBlockStates[dim - 1, y, z] == null))
    //            {
    //                allAdjacentsPresent = false;
    //                break;
    //            }
    //            if (xA >= dim && (regionsAdj[0, 1, 1] == null || regionsAdj[0, 1, 1].regionBlockStates[0, y, z] == null))
    //            {
    //                allAdjacentsPresent = false;
    //                break;
    //            }
    //            if (yA < 0 && (regionsAdj[1, 2, 1] == null || regionsAdj[1, 2, 1].regionBlockStates[x, dim - 1, z] == null))
    //            {
    //                allAdjacentsPresent = false;
    //                break;
    //            }
    //            if (yA >= dim && (regionsAdj[1, 0, 1] == null || regionsAdj[1, 0, 1].regionBlockStates[x, 0, z] == null))
    //            {
    //                allAdjacentsPresent = false;
    //                break;
    //            }
    //            if (zA < 0 && (regionsAdj[1, 1, 2] == null || regionsAdj[1, 1, 2].regionBlockStates[x, y, dim - 1] == null))
    //            {
    //                allAdjacentsPresent = false;
    //                break;
    //            }
    //            if (zA >= dim && (regionsAdj[1, 1, 0] == null || regionsAdj[1, 1, 0].regionBlockStates[x, y, 0] == null))
    //            {
    //                allAdjacentsPresent = false;
    //                break;
    //            }
    //        }
    //        else
    //        {
    //            // Within bounds, check current region
    //            if (regionsAdj[1, 1, 1].regionBlockStates[xA, yA, zA] == null)
    //            {
    //                allAdjacentsPresent = false;
    //                break;
    //            }
    //        }
    //    }

    //    block.isVis = !allAdjacentsPresent;
    //}

    public void GeneratorBlocks()
    {
        SetCreateBlocks();
    }

    private void SetCreateBlocks()
    {
        int xLength = regionBlockStates.GetLength(0);
        int yLength = regionBlockStates.GetLength(1);
        int zLength = regionBlockStates.GetLength(2);

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    if (regionBlockStates[x, y, z] != null)
                    { CreateBlock(regionBlockStates[x, y, z]); }
                }
            }
        }
    }

    private void CreateBlock(BlockState block)
    {
        GameObject blockPrefab = Object.Instantiate(
            Resources.Load<GameObject>(block.blockId),
            block.blockPos + regionPos,
            Quaternion.identity);
        blockPrefab.transform.parent = regionObj.transform;
        blockPrefab.SetActive(block.isVis);
    }
}