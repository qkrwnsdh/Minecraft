using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public GameObject regionObject;
    public List<BlockState> regionBlockStates = new List<BlockState>();

    private string type;

    private readonly string[] biomes = { "Desert", "Ocean", "Veld" };

    private readonly int xDim = 100;
    private readonly int yDim = 100;
    private readonly int zDim = 100;

    private int waveScale;
    private int amplitude;

    private GameObject blockPrefab;

    public Region(Vector3 position, List<BlockState> regionBlockStates)
    {
        this.regionBlockStates = regionBlockStates;

        regionObject = new GameObject($"Region_{(int)position.x}_{(int)position.y}_{(int)position.z}");
        regionObject.transform.position = position;

        SetupRegion();
        GenerateRegion();
    }

    private void SetupRegion()
    {
        type = biomes[Random.Range(0, biomes.Length)];

        switch (type)
        {
            case "Desert":
                waveScale = Random.Range(1, 5);
                amplitude = Random.Range(1, 5);
                blockPrefab = Resources.Load<GameObject>("Block_00");
                break;
            case "Ocean":
                waveScale = Random.Range(3, 8);
                amplitude = Random.Range(3, 8);
                blockPrefab = Resources.Load<GameObject>("Block_01");
                break;
            case "Veld":
                waveScale = Random.Range(5, 10);
                amplitude = Random.Range(5, 10);
                blockPrefab = Resources.Load<GameObject>("Block_02");
                break;
            default:
                Debug.LogError($"not found {this.type}");
                break;
        }

        Debug.Log($"Region type: {type}, WaveScale: {waveScale}, Amplitude: {amplitude}");
    }

    private void GenerateRegion()
    {
        float[,] heightMap = GenerateNoiseMap(xDim, zDim, waveScale);

        for (int x = 0; x < xDim; x++)
        {
            for (int z = 0; z < zDim; z++)
            {
                int y = (int)(heightMap[x, z] * amplitude);
                Vector3Int blockPosition = new Vector3Int(x, y, z);

                BlockState state = new BlockState(blockPosition, type, true);
                regionBlockStates.Add(state);
                GameObject block = Object.Instantiate(blockPrefab, blockPosition + regionObject.transform.position, Quaternion.identity, regionObject.transform);
            }
        }
    }

    private float[,] GenerateNoiseMap(int width, int height, float scale)
    {
        float[,] noiseMap = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float xCoord = (float)x / width * scale;
                float zCoord = (float)z / height * scale;
                noiseMap[x, z] = Mathf.PerlinNoise(xCoord, zCoord);
            }
        }
        return noiseMap;
    }
}

public class MapData
{

}

public class BlockState
{
    public Vector3 position { get; set; }
    public string id { get; set; }
    public bool isVisible { get; set; }

    public BlockState(Vector3 _position, string _id, bool _isVisible)
    {
        position = _position;
        id = _id;
        isVisible = _isVisible;
    }
}

public class MapGenerator : MonoBehaviour
{
    private List<BlockState> blockStates = new List<BlockState>();

    static private readonly int xDim = 100;
    static private readonly int yDim = 100;
    static private readonly int zDim = 100;

    public int waveScale = 10;
    public int amplitude = 10;

    private void Start()
    {
        StartCoroutine(LoadMap());
    }

    private IEnumerator LoadMap()
    {
        yield return StartCoroutine(LoadMapData());
    }

    private IEnumerator LoadMapData()
    {
        for (int x = -xDim / 2; x < xDim / 2 + 1; x++)
        {
            for (int z = -zDim / 2; z < zDim / 2 + 1; z++)
            {
                float xPos = (float)x / xDim * waveScale;
                float zPos = (float)z / zDim * waveScale;

                int y = (int)(Mathf.PerlinNoise(xPos, zPos) * amplitude);

                Vector3 pos = new Vector3(x, y, z);

                blockStates.Add(new BlockState(pos, "Block_01", true));
            }
        }

        new Region(new Vector3(0, 0, 0), blockStates);

        yield return null;
    }
}
