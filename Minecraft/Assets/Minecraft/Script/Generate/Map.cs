using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

[Serializable]
public struct BlockType
{
    public string name;
    public float height;
}

public class Map : MonoBehaviour
{
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public BlockType[] blockType;

    private int chunkSize = Define.MAP_CHUNK_SIZE;
    private int octaves = Define.MAP_OCTAVES;
    private float noiseScale = Define.MAP_NOISE_SCALE;
    private float persistance = Define.MAP_PERSISTANCE;
    private float lacunarity = Define.MAP_LACUNARITY;

    private int seed;

    private Dictionary<string, Vector2> blocks = new Dictionary<string, Vector2>();

    private void Start()
    {
        
    }
}
