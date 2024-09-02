using System.Collections.Generic;
using UnityEngine;
using System;

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

            blockInfoDictionary = JsonReader.LoadBlockInfoJson<BlockInfo, BlockInfoData>(Define.PATH_BLOCK_INFO);
            blockDatas = new Dictionary<string, BlockData>();

            // BlockData 객체들을 초기화하여 blockDatas에 저장
            foreach (var blockInfo in blockInfoDictionary.Values)
            {
                blockDatas[blockInfo.name] = new BlockData(blockInfo);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private Dictionary<string, BlockInfo> blockInfoDictionary;
    public Dictionary<string, BlockData> blockDatas;

    public BlockInfo GetBlockInfoData(string id)
    {
        if (blockInfoDictionary.TryGetValue(id, out BlockInfo blockInfo))
        {
            return blockInfo;
        }

        Debug.LogError("Block id not found");
        return null;
    }
}

public struct BlockData
{
    public readonly string name;
    public readonly bool type;
    public readonly int health;
    public readonly string drop;
    public readonly List<Offset> offsets;

    public BlockData(BlockInfo blockInfo)
    {
        name = blockInfo.name;
        type = blockInfo.type;
        health = blockInfo.health;
        drop = blockInfo.drop;
        offsets = blockInfo.offsets;
    }
}

#region BlockInfo Attribute
[Serializable]
public class Offset
{
    public List<Vector2> offset;
}

[Serializable]
public class BlockInfo
{
    public string name;
    public bool type;
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