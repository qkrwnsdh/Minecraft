using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRegionTypeCommand
{
    public BlockState[,,] Execute(Region region, int dim);
}

public class StoneCommand : IRegionTypeCommand
{
    private BlockState[,,] regionBlockStates = new BlockState[100, 100, 100];
    private Region region;

    public BlockState[,,] Execute(Region region, int dim)
    {
        this.region = region;
        regionBlockStates = new BlockState[dim, dim, dim];

        for (int x = 0; x < dim; x++)
        {
            for (int y = 0; y < dim; y++)
            {
                for (int z = 0; z < dim; z++)
                {
                    Vector3 pos = new Vector3(x, y, z);

                    regionBlockStates[x, y, z] = new BlockState("Block_01", pos);
                }
            }
        }

        return regionBlockStates;
    }
}

public abstract class GroundBaseCommand : IRegionTypeCommand
{
    protected BlockState[,,] _regionBlockStates;
    protected Region _region;
    protected readonly int MAX_ADD_SMOOTH = 5;
    protected readonly int MIN_ADD_SMOOTH = 3;

    public BlockState[,,] Execute(Region region, int dim)
    {
        _regionBlockStates = new BlockState[dim, dim, dim];
        _region = region;

        GenerateBlocks(dim);
        return _regionBlockStates;
    }

    protected abstract void GenerateBlocks(int dim);
    protected abstract string SetBlockId(int x, int y, int z, int height, int smooth);
}

public class DesertCommand : GroundBaseCommand
{
    protected override void GenerateBlocks(int dim)
    {
        for (int x = 0; x < dim; x++)
        {
            for (int z = 0; z < dim; z++)
            {
                float xPos = (float)x / dim * Define.BIOME_DESERT_VIORATION;
                float zPos = (float)z / dim * Define.BIOME_DESERT_VIORATION;

                int height = (int)(Mathf.PerlinNoise(xPos, zPos) * Define.BIOME_DESERT_AMPLITUDE + MAX_ADD_SMOOTH);
                int smooth = Random.Range(MIN_ADD_SMOOTH, MAX_ADD_SMOOTH);

                for (int y = 0; y <= height; y++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    string blockId = SetBlockId(x, y, z, height, smooth);
                    _regionBlockStates[x, y, z] = new BlockState(blockId, pos);
                }
            }
        }
    }

    protected override string SetBlockId(int x, int y, int z, int height, int smooth)
    {
        string blockType = y <= height && height - smooth < y ? "Block_01" : "Block_02";

        return blockType;
    }
}

public class OceanCommand : GroundBaseCommand
{
    protected override void GenerateBlocks(int dim)
    {
        for (int x = 0; x < dim; x++)
        {
            for (int z = 0; z < dim; z++)
            {
                float xPos = (float)x / dim * Define.BIOME_OCEAN_VIORATION;
                float zPos = (float)z / dim * Define.BIOME_OCEAN_VIORATION;

                int height = (int)(Mathf.PerlinNoise(xPos, zPos) * Define.BIOME_OCEAN_AMPLITUDE + MAX_ADD_SMOOTH);
                int smooth = Random.Range(MIN_ADD_SMOOTH, MAX_ADD_SMOOTH);

                for (int y = 0; y <= height; y++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    string blockId = SetBlockId(x, y, z, height, smooth);
                    _regionBlockStates[x, y, z] = new BlockState(blockId, pos);
                }
            }
        }
    }

    protected override string SetBlockId(int x, int y, int z, int height, int smooth)
    {
        string blockType = y <= height && height - smooth < y ? "Block_01" : "Block_02";

        return blockType;
    }
}

public class VeldCommand : GroundBaseCommand
{
    protected override void GenerateBlocks(int dim)
    {
        for (int x = 0; x < dim; x++)
        {
            for (int z = 0; z < dim; z++)
            {
                float xPos = (float)x / dim * Define.BIOME_VELD_VIORATION;
                float zPos = (float)z / dim * Define.BIOME_VELD_VIORATION;

                int height = (int)(Mathf.PerlinNoise(xPos, zPos) * Define.BIOME_VELD_AMPLITUDE + MAX_ADD_SMOOTH);
                int smooth = Random.Range(MIN_ADD_SMOOTH, MAX_ADD_SMOOTH);

                for (int y = 0; y <= height; y++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    string blockId = SetBlockId(x, y, z, height, smooth);
                    _regionBlockStates[x, y, z] = new BlockState(blockId, pos);
                }
            }
        }
    }

    protected override string SetBlockId(int x, int y, int z, int height, int smooth)
    {
        string blockType = y <= height && height - smooth < y ? "Block_01" : "Block_02";

        return blockType;
    }
}
