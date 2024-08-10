using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class MapDTO
{
    public List<RegionDTO> regions = new List<RegionDTO>();
}

[System.Serializable]
public class RegionDTO
{
    public string regionType;
    public Vector3 regionPos;
    public List<BlockDTO> blocks = new List<BlockDTO>();
}

[System.Serializable]
public class BlockDTO
{
    public string blockId;
    public Vector3 blockPos;
    public bool isVis;
}

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

        SaveMapToJson("mapData.json");
    }

    private void InitializationInstances()
    {
        regions = new Region[Define.REGION_DIM, Define.REGION_DIM, Define.REGION_DIM];
    }

    private void GeneratorRegions()
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
                    regions[x, y, z] = new Region(new Vector3(x, y, z), regionParent);
                }
            }
        }

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    regions[x, y, z].GeneratorBlocksData();
                }
            }
        }

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    regions[x, y, z].GeneratorBlocks();
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
                    regionDTO.regionPos = region.regionPos;

                    int blockXLength = region.regionBlockStates.GetLength(0);
                    int blockYLength = region.regionBlockStates.GetLength(1);
                    int blockZLength = region.regionBlockStates.GetLength(2);

                    for (int bx = 0; bx < blockXLength; bx++)
                    {
                        for (int by = 0; by < blockYLength; by++)
                        {
                            for (int bz = 0; bz < blockZLength; bz++)
                            {
                                BlockState blockState = region.regionBlockStates[bx, by, bz];
                                if (blockState != null)
                                {
                                    BlockDTO blockDTO = new BlockDTO();
                                    blockDTO.blockId = blockState.blockId;
                                    blockDTO.blockPos = blockState.blockPos;
                                    blockDTO.isVis = blockState.isVis;

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
