using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

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
}