//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public interface IRegionTypeCommand
//{
//    public Block[,,] Execute(Region region);
//}

//public class StoneCommand : IRegionTypeCommand
//{
//    private readonly int dim = Define.REGION_IN_BLOCK_DIM;
//    private Block[,,] regionBlocks;

//    public Block[,,] Execute(Region region)
//    {
//        regionBlocks = new Block[dim, dim, dim];

//        for (int x = 0; x < dim; x++)
//        {
//            for (int y = 0; y < dim; y++)
//            {
//                for (int z = 0; z < dim; z++)
//                {
//                    Vector3 position = new Vector3(x, y, z);
//                    GameObject block = Object.Instantiate(
//                        Resources.Load<GameObject>(Define.PATH_BLOCK_PREFAB),
//                        position + region.regionPosition,
//                        Quaternion.identity,
//                        region.regionObject.transform);

//                    regionBlocks[x, y, z] = block.GetComponent<Block>();
//                    regionBlocks[x, y, z].Initialization("Block_02",block.transform.position);
//                }
//            }
//        }

//        return regionBlocks;
//    }
//}

//public abstract class GroundBaseCommand : IRegionTypeCommand
//{
//    protected readonly int dim = Define.REGION_IN_BLOCK_DIM;
//    protected Block[,,] regionBlocks;

//    public Block[,,] Execute(Region region)
//    {
//        regionBlocks = new Block[dim, dim, dim];

//        GenerateBlocks(region);

//        return regionBlocks;
//    }

//    protected abstract void GenerateBlocks(Region region);
//}

//public class DesertCommand : GroundBaseCommand
//{
//    protected override void GenerateBlocks(Region region)
//    {
//        for (int x = 0; x < dim; x++)
//        {
//            for (int z = 0; z < dim; z++)
//            {
//                float xPos = (float)x / dim * Define.BIOME_DESERT_VIORATION;
//                float zPos = (float)z / dim * Define.BIOME_DESERT_VIORATION;

//                int height = (int)(Mathf.PerlinNoise(xPos, zPos) * Define.BIOME_DESERT_AMPLITUDE + Define.BIOME_SMOOTH_MAX);
//                int smooth = Random.Range(Define.BIOME_SMOOTH_MIN, Define.BIOME_SMOOTH_MAX);

//                for (int y = 0; y <= height; y++)
//                {
//                    Vector3 position = new Vector3(x, y, z);
//                    string blockId = SetBlockId(x, y, z, height, smooth);
//                    GameObject block = Object.Instantiate(
//                        Resources.Load<GameObject>(Define.PATH_BLOCK_PREFAB),
//                        position + region.regionPosition,
//                        Quaternion.identity,
//                        region.regionObject.transform);

//                    regionBlocks[x, y, z] = block.GetComponent<Block>();
//                    regionBlocks[x, y, z].Initialization(blockId, block.transform.position);
//                }
//            }
//        }
//    }

//    private string SetBlockId(int x, int y, int z, int height, int smooth)
//    {
//        string blockType = y <= height && height - smooth < y ? "Block_01" : "Block_02";

//        return blockType;
//    }
//}

//public class OceanCommand : GroundBaseCommand
//{
//    protected override void GenerateBlocks(Region region)
//    {
//        for (int x = 0; x < dim; x++)
//        {
//            for (int z = 0; z < dim; z++)
//            {
//                float xPos = (float)x / dim * Define.BIOME_OCEAN_VIORATION;
//                float zPos = (float)z / dim * Define.BIOME_OCEAN_VIORATION;

//                int height = (int)(Mathf.PerlinNoise(xPos, zPos) * Define.BIOME_OCEAN_AMPLITUDE + Define.BIOME_SMOOTH_MAX);
//                int smooth = Random.Range(Define.BIOME_SMOOTH_MIN, Define.BIOME_SMOOTH_MAX);

//                for (int y = 0; y <= height; y++)
//                {
//                    Vector3 position = new Vector3(x, y, z);
//                    string blockId = SetBlockId(x, y, z, height, smooth);
//                    GameObject block = Object.Instantiate(
//                        Resources.Load<GameObject>(Define.PATH_BLOCK_PREFAB),
//                        position + region.regionPosition,
//                        Quaternion.identity,
//                        region.regionObject.transform);

//                    regionBlocks[x, y, z] = block.GetComponent<Block>();
//                    regionBlocks[x, y, z].Initialization(blockId, block.transform.position);
//                }
//            }
//        }
//    }

//    private string SetBlockId(int x, int y, int z, int height, int smooth)
//    {
//        string blockType = y <= height && height - smooth < y ? "Block_01" : "Block_02";

//        return blockType;
//    }
//}

//public class VeldCommand : GroundBaseCommand
//{
//    protected override void GenerateBlocks(Region region)
//    {
//        for (int x = 0; x < dim; x++)
//        {
//            for (int z = 0; z < dim; z++)
//            {
//                float xPos = (float)x / dim * Define.BIOME_VELD_VIORATION;
//                float zPos = (float)z / dim * Define.BIOME_VELD_VIORATION;

//                int height = (int)(Mathf.PerlinNoise(xPos, zPos) * Define.BIOME_VELD_AMPLITUDE + Define.BIOME_SMOOTH_MAX);
//                int smooth = Random.Range(Define.BIOME_SMOOTH_MIN, Define.BIOME_SMOOTH_MAX);

//                for (int y = 0; y <= height; y++)
//                {
//                    Vector3 position = new Vector3(x, y, z);
//                    string blockId = SetBlockId(x, y, z, height, smooth);
//                    GameObject block = Object.Instantiate(
//                        Resources.Load<GameObject>(Define.PATH_BLOCK_PREFAB),
//                        position + region.regionPosition,
//                        Quaternion.identity,
//                        region.regionObject.transform);

//                    regionBlocks[x, y, z] = block.GetComponent<Block>();
//                    regionBlocks[x, y, z].Initialization(blockId, block.transform.position);
//                }
//            }
//        }
//    }

//    private string SetBlockId(int x, int y, int z, int height, int smooth)
//    {
//        string blockType = y <= height && height - smooth < y ? "Block_01" : "Block_02";

//        return blockType;
//    }
//}