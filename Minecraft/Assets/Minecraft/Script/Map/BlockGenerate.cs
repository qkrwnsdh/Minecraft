using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockGenerator
{
    public static BlockData BlockGenerate(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve)
    {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);

        int heightMapX = heightMap.GetLength(0);
        int heightMapY = heightMap.GetLength(1);
        float topLeftX = (heightMapX - 1) / -2f;
        float topLeftZ = (heightMapY - 1) / 2f;

        for (int y = 0; y < heightMapY; y++)
        {
            for (int x = 0; x < heightMapX; x++)
            {
                Debug.Log(heightMap[x, y]);
            }
        }
        return null;
    }
}

public class BlockData
{

}