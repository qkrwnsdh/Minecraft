using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Unity.Mathematics;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] private Transform regionParent;
    public Region[,,] regions;

    private void Start()
    {
        InitializationInstances();
        GeneratorRegions();

        //SaveMapToJson("mapData.json");
    }

    private void InitializationInstances()
    {
        regions = new Region[Define.REGION_DIM, Define.REGION_DIM, Define.REGION_DIM];
    }

    private void GeneratorRegions()
    {
        RegionAction((region, x, y, z) => { regions[x, y, z] = new Region(new Vector3(x, y, z), regionParent); });
        RegionAction((region, x, y, z) => { regions[x, y, z].GeneratorBlocksData(); });
        RegionAction((region, x, y, z) => { regions[x, y, z].BlockAdjacents(); });
        RegionAction((region, x, y, z) => { regions[x, y, z].BlockVisibles(); });
    }

    private void RegionAction(Action<Region, int, int, int> action)
    {
        int xLength = regions.GetLength(0);
        int yLength = regions.GetLength(1);
        int zLength = regions.GetLength(2);

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    action(regions[x, y, z], x, y, z);
                }
            }
        }
    }

    public Region GetAdjacencyRegion(int x, int y, int z)
    {
        if (0 <= x && x < Define.REGION_DIM &&
            0 <= y && y < Define.REGION_DIM &&
            0 <= z && z < Define.REGION_DIM)
        {
            return regions[x, y, z];
        }

        return null;
    }

    public void SaveMapToJson(string fileName)
    {
        MapDTO mapData = new MapDTO();

        int xLength = regions.GetLength(0);
        int yLength = regions.GetLength(1);
        int zLength = regions.GetLength(2);

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    Region region = regions[x, y, z];
                    RegionDTO regionDTO = new RegionDTO();
                    regionDTO.regionType = region.regionType;
                    regionDTO.regionPosition = region.regionPosition;

                    int blockXLength = region.regionBlocks.GetLength(0);
                    int blockYLength = region.regionBlocks.GetLength(1);
                    int blockZLength = region.regionBlocks.GetLength(2);

                    for (int bx = 0; bx < blockXLength; bx++)
                    {
                        for (int by = 0; by < blockYLength; by++)
                        {
                            for (int bz = 0; bz < blockZLength; bz++)
                            {
                                Block blockState = region.regionBlocks[bx, by, bz];
                                if (blockState != null)
                                {
                                    BlockDTO blockDTO = new BlockDTO();
                                    blockDTO.blockId = blockState.blockId;
                                    blockDTO.blockPosition = blockState.blockPosition;
                                    blockDTO.blockVisible = blockState.blockVisible;

                                    regionDTO.blocks.Add(blockDTO);
                                }
                            }
                        }
                    }

                    mapData.regions.Add(regionDTO);
                }
            }
        }

        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(Path.Combine(Application.dataPath, fileName), json);
    }
}
