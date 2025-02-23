using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    public MapData mapData;
    public RegionType[] regionTypes;

    public int chunkHeightMultiplier;
    public AnimationCurve chunkHeightCurve;

    public int seed;
    public Vector2 offset;
    public int maxDepth;
    public int transitionRange;

    Queue<ThreadInfo<NoiseData>> noiseDataThreadInfoQueue = new Queue<ThreadInfo<NoiseData>>();
    Queue<ThreadInfo<Dictionary<Vector3, string>>> blockDataThreadInfoQueue = new Queue<ThreadInfo<Dictionary<Vector3, string>>>();
    Queue<ThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<ThreadInfo<MeshData>>();

    void Update()
    {
        if (noiseDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < noiseDataThreadInfoQueue.Count; i++)
            {
                ThreadInfo<NoiseData> threadInfo = noiseDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
        if (blockDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < blockDataThreadInfoQueue.Count; i++)
            {
                ThreadInfo<Dictionary<Vector3, string>> threadInfo = blockDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                ThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    public void RequestNoiseData(Vector2 center, Action<NoiseData> callback)
    {
        ThreadStart threadStart = delegate
        {
            NoiseDataThread(center, callback);
        };

        new Thread(threadStart).Start();
    }

    void NoiseDataThread(Vector2 center, Action<NoiseData> callback)
    {
        NoiseData noiseData = Generator.GeneratorNoise(mapData, center, chunkHeightCurve, chunkHeightMultiplier);

        lock (noiseDataThreadInfoQueue)
        {
            noiseDataThreadInfoQueue.Enqueue(new ThreadInfo<NoiseData>(callback, noiseData));
        }
    }

    public void RequestBlockData(NoiseData noiseData, Action<Dictionary<Vector3, string>> callback)
    {
        ThreadStart threadStart = delegate
        {
            BlockDataThread(noiseData, callback);
        };

        new Thread(threadStart).Start();
    }

    void BlockDataThread(NoiseData noiseData, Action<Dictionary<Vector3, string>> callback)
    {
        Dictionary<Vector3, string> blockData = Generator.GeneratorBlock(noiseData, regionTypes, maxDepth, transitionRange);

        lock (blockDataThreadInfoQueue)
        {
            blockDataThreadInfoQueue.Enqueue(new ThreadInfo<Dictionary<Vector3, string>>(callback, blockData));
        }
    }

    public void RequestMeshData(Dictionary<Vector3, string> blockData, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(blockData, callback);
        };

        new Thread(threadStart).Start();
    }

    void MeshDataThread(Dictionary<Vector3, string> blockData, Action<MeshData> callback)
    {
        MeshData meshData = Generator.GeneratorMesh(blockData);

        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new ThreadInfo<MeshData>(callback, meshData));
        }
    }

    struct ThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public ThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

[Serializable]
public struct MapData
{
    public int seed;
    public int chunkSize;
    public float scale;
    public int octave;
    public float persistance;
    public float lacunarity;
}

[Serializable]
public struct RegionType
{
    public string name;
    public float height;
}

public struct NoiseData
{
    public readonly float[,] noiseHeight;
    public readonly int[,] normalizedNoiseHeight;

    public NoiseData(float[,] noiseHeight, int[,] normalizedNoiseHeight)
    {
        this.noiseHeight = noiseHeight;
        this.normalizedNoiseHeight = normalizedNoiseHeight;
    }
}