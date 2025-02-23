using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    #region Singleton and Awake()
    public static BlockManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializationDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializationDictionary()
    {
        blockInfoDictionary = JsonReader.LoadToJson<string, BlockInfo, BlockInfoData>(Define.PATH_BLOCK_INFO);
        blockCreateDictionary = JsonReader.LoadToJson<string, BlockCreate, BlockCreateData>(Define.PATH_BLOCK_CREATE);
    }

    #endregion

    #region Block Info
    public Material blockMaterial;

    const int materialCount = 6;

    public Material[] SetBlockMaterial(BlockInfo blockInfo, int value)
    {
        Material[] materials = new Material[materialCount];

        if (!(blockInfo.offsets.Count < value + 1))
        {
            for (int i = 0; i < materialCount; i++)
            {
                materials[i] = new Material(blockMaterial);
                materials[i].SetTextureOffset("_MainTex", blockInfo.offsets[value].offset[i]);
            }
        }

        return materials;
    }

    private Dictionary<string, BlockInfo> blockInfoDictionary;

    public BlockInfo GetBlockInfoData(string name)
    {
        if (blockInfoDictionary.TryGetValue(name, out BlockInfo blockInfo))
        {
            return blockInfo;
        }

        Debug.LogError($"{name} Item name not found");
        return null;
    }
    #endregion

    #region Block Create
    private Dictionary<string, BlockCreate> blockCreateDictionary;

    public BlockCreate GetBlockCreateData(string name)
    {
        if (blockCreateDictionary.TryGetValue(name, out BlockCreate blockCreate))
        {
            return blockCreate;
        }

        Debug.LogError($"{name} Item name not found");
        return null;
    }
    #endregion
}

#region BlockInfo
[Serializable]
public class Offset
{
    public Vector2[] offset;
}

[Serializable]
public class BlockInfo
{
    public string name;
    public int health;
    public string drop;
    public List<Offset> offsets;
}

[Serializable]
public class BlockInfoData
{
    public BlockInfo[] blockInfos;
}
#endregion

#region BlockCreate
[Serializable]
public class BlockCreate
{
    public string name;
    public string result;
}

[Serializable]
public class BlockCreateData
{
    public BlockCreate[] blockCreates;
}
#endregion