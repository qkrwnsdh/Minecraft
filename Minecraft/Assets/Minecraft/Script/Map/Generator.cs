using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using System.Linq;

public enum DirectionType { Front, Back, Left, Right, Up, Down };

public static class Generator
{
    public static NoiseData GeneratorNoise(MapData mapData, Vector2 offset, AnimationCurve curve, float multiplier)
    {
        int size = mapData.chunkSize + 2;
        int seed = mapData.seed;
        int octave = mapData.octave;
        float scale = mapData.scale;
        float persistance = mapData.persistance;
        float lacunarity = mapData.lacunarity;

        float[,] noiseMap = new float[size, size];
        int[,] normalizedNoiseMap = new int[size, size];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octave];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octave; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfSize = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octave; i++)
                {
                    float sampleX = (x - halfSize + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y - halfSize + octaveOffsets[i].y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                float normalizedHeight = (noiseHeight + 1) / (maxPossibleHeight / 0.9f);
                float currentHeight = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                float multiplierHeight = curve.Evaluate(currentHeight) * multiplier;
                float roundHeight = MathUtility.CalculateRound(multiplierHeight);

                noiseMap[x, y] = currentHeight;
                normalizedNoiseMap[x, y] = (int)roundHeight;
            }
        }

        return new NoiseData(noiseMap, normalizedNoiseMap);
    }

    public static Dictionary<Vector3, string> GeneratorBlock(NoiseData noiseData, RegionType[] regionTypes, int maxDepth, int transitionRange)
    {
        float[,] noiseHeight = noiseData.noiseHeight;
        int[,] normalizedNoiseHeight = noiseData.normalizedNoiseHeight;
        int blockPerLine = noiseHeight.GetLength(0) - 2;
        int normalizedDepth = -maxDepth;
        Dictionary<Vector3, string> chunkBlocks = new Dictionary<Vector3, string>();

        for (int z = 0; z < blockPerLine; z++)
        {
            for (int x = 0; x < blockPerLine; x++)
            {
                int index = 0;
                int height = normalizedNoiseHeight[x, z];

                for (int i = 0; i < regionTypes.Length; i++)
                {
                    if (regionTypes[i].height <= noiseHeight[x, z])
                    {
                        index = i;
                        break;
                    }
                }

                for (int y = height; normalizedDepth < y; y--)
                {
                    string baseBlockType = "Block_Stone";

                    int lowerDepth = height - transitionRange * (index + 1);

                    if (lowerDepth <= y)
                    {
                        baseBlockType = regionTypes[index].name;
                    }
                    if (lowerDepth == y && index < regionTypes.Length - 1)
                    {
                        index++;
                    }

                    chunkBlocks.Add(new Vector3(x, y, z), baseBlockType);
                }
            }
        }

        return chunkBlocks;
    }

    public static MeshData GeneratorMesh(Dictionary<Vector3, string> blockData)
    {
        MeshData meshData = new MeshData();

        DirectionType[] directionValues = (DirectionType[])Enum.GetValues(typeof(DirectionType));
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right,
            Vector3.up,
            Vector3.down
        };

        foreach (Vector3 position in blockData.Keys)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 neighborPosition = position + directions[i];

                if (!blockData.ContainsKey(neighborPosition))
                {
                    meshData.AddVertex(directionValues[i], position);
                    meshData.AddUV();
                    meshData.AddTriangle(directionValues[i], blockData[position]);
                }
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Mesh mesh;
    public Material[] materials;

    const int directionTypeLength = 6;

    List<Vector3> vertexList;
    List<Vector2> uvList;
    List<TriangleData> triangleList;

    int triangleIndex;

    public MeshData()
    {
        vertexList = new List<Vector3>();
        uvList = new List<Vector2>();
        triangleList = new List<TriangleData>();
    }

    public void AddVertex(DirectionType directionType, Vector3 blockPosition)
    {
        switch (directionType)
        {
            case DirectionType.Front:     // 앞
                vertexList.Add(blockPosition + new Vector3(0.0f, 0.0f, 1.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 0.0f, 1.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 1.0f, 1.0f));
                vertexList.Add(blockPosition + new Vector3(0.0f, 1.0f, 1.0f));
                break;
            case DirectionType.Back:     // 뒤
                vertexList.Add(blockPosition + new Vector3(1.0f, 0.0f, 0.0f));
                vertexList.Add(blockPosition + new Vector3(0.0f, 0.0f, 0.0f));
                vertexList.Add(blockPosition + new Vector3(0.0f, 1.0f, 0.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 1.0f, 0.0f));
                break;
            case DirectionType.Left:     // 왼쪽
                vertexList.Add(blockPosition + new Vector3(0.0f, 0.0f, 0.0f));
                vertexList.Add(blockPosition + new Vector3(0.0f, 0.0f, 1.0f));
                vertexList.Add(blockPosition + new Vector3(0.0f, 1.0f, 1.0f));
                vertexList.Add(blockPosition + new Vector3(0.0f, 1.0f, 0.0f));
                break;
            case DirectionType.Right:     // 오른쪽
                vertexList.Add(blockPosition + new Vector3(1.0f, 0.0f, 1.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 0.0f, 0.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 1.0f, 0.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 1.0f, 1.0f));
                break;
            case DirectionType.Up:     // 위
                vertexList.Add(blockPosition + new Vector3(0.0f, 1.0f, 1.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 1.0f, 1.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 1.0f, 0.0f));
                vertexList.Add(blockPosition + new Vector3(0.0f, 1.0f, 0.0f));
                break;
            case DirectionType.Down:     // 아래
                vertexList.Add(blockPosition + new Vector3(0.0f, 0.0f, 0.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 0.0f, 0.0f));
                vertexList.Add(blockPosition + new Vector3(1.0f, 0.0f, 1.0f));
                vertexList.Add(blockPosition + new Vector3(0.0f, 0.0f, 1.0f));
                break;
        }
    }

    public void AddUV()
    {
        uvList.Add(new Vector2(0, 0));
        uvList.Add(new Vector2(1, 0));
        uvList.Add(new Vector2(1, 1));
        uvList.Add(new Vector2(0, 1));
    }

    public void AddTriangle(DirectionType directionType, string blockType)
    {
        int index = triangleList.FindIndex(data => data.blockType == blockType);

        if (-1 != index)
        {
            triangleList[index].AddTriangleData(directionType, triangleIndex);
            triangleList[index].AddTriangleData(directionType, triangleIndex + 1);
            triangleList[index].AddTriangleData(directionType, triangleIndex + 2);
            triangleList[index].AddTriangleData(directionType, triangleIndex);
            triangleList[index].AddTriangleData(directionType, triangleIndex + 2);
            triangleList[index].AddTriangleData(directionType, triangleIndex + 3);

            triangleIndex += 4;
        }
        else
        {
            TriangleData newTriangleData = new TriangleData(blockType);

            triangleList.Add(newTriangleData);

            AddTriangle(directionType, blockType);
        }
    }

    public void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertexList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.subMeshCount = triangleList.Count * directionTypeLength;
        UpdateTriangles(mesh);
        mesh.RecalculateNormals();

        this.mesh = mesh;
    }

    void UpdateTriangles(Mesh mesh)
    {
        DirectionType[] directionValues = (DirectionType[])Enum.GetValues(typeof(DirectionType));

        for (int i = 0; i < triangleList.Count; i++)
        {
            for (int j = 0; j < directionTypeLength; j++)
            {
                mesh.SetTriangles(triangleList[i].triangleForDirection[directionValues[j]], i * directionTypeLength + j);
            }
        }
    }

    public void CreateMaterials()
    {
        Material[] materials = new Material[triangleList.Count * directionTypeLength];

        Material blockMaterial = BlockManager.Instance.blockMaterial;

        for (int i = 0; i < triangleList.Count; i++)
        {
            BlockInfo blockInfo = BlockManager.Instance.GetBlockInfoData(triangleList[i].blockType);

            for (int j = 0; j < directionTypeLength; j++)
            {
                int index = i * directionTypeLength + j;

                materials[index] = new Material(blockMaterial);
                materials[index].SetTextureOffset("_MainTex", blockInfo.offsets[0].offset[j]);
            }
        }

        this.materials = materials;
    }

    public class TriangleData
    {
        public string blockType;
        public Dictionary<DirectionType, List<int>> triangleForDirection;

        public TriangleData(string blockType)
        {
            this.blockType = blockType;
            triangleForDirection = new Dictionary<DirectionType, List<int>>();

            foreach (DirectionType direction in Enum.GetValues(typeof(DirectionType)))
            {
                triangleForDirection[direction] = new List<int>();
            }
        }

        public void AddTriangleData(DirectionType directionType, int triangleIndex)
        {
            triangleForDirection[directionType].Add(triangleIndex);
        }
    }
}