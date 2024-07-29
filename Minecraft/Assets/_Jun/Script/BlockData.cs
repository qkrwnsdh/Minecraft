using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockInfo
{
    public string id;
    public string name;
    public int health;
    public string drop;
    public string texture;
}

[System.Serializable]
public class BlockInfoArray
{
    public BlockInfo[] blockInfos;
}

public class BlockData : MonoBehaviour
{
    public static BlockData Instance { get; private set; }

    private Dictionary<string, BlockInfo> blockInfoDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadData();
        }
        else
        { Destroy(gameObject); }
    }

    private void LoadData()
    {
        TextAsset blockListJson = Resources.Load<TextAsset>(Define.PATH_BLOCK_LIST);

        if (blockListJson != null)
        {
            BlockInfoArray blockInfoArray = JsonUtility.FromJson<BlockInfoArray>(blockListJson.text);
            blockInfoDictionary = new Dictionary<string, BlockInfo>();

            foreach (var blockInfo in blockInfoArray.blockInfos)
            {
                blockInfoDictionary[blockInfo.id] = blockInfo; 
            }
        }
        else
        {
            Debug.LogError("BlockList.json file not found in Resources"); 
        }
    }

    public BlockInfo GetBlock(string id)
    {
        BlockInfo blockInfo = GetBlockInfoData(id);

        if (blockInfo != null)
        {
            return blockInfo;
        }
        else
        {
            Debug.LogError("Block id not found");

            return null;
        }
    }

    private BlockInfo GetBlockInfoData(string id)
    {
        if (blockInfoDictionary.TryGetValue(id, out BlockInfo blockInfo))
        { return blockInfo; }

        return null;
    }
}