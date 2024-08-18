using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapDTO
{
    public List<RegionDTO> regions = new List<RegionDTO>();
}

[Serializable]
public class RegionDTO
{
    public string regionType;
    public Vector3 regionPosition;
    public List<BlockDTO> blocks = new List<BlockDTO>();
}

[Serializable]
public class BlockDTO
{
    public string blockId;
    public Vector3 blockPosition;
    public bool blockVisible;
}

[Serializable]
public class BlockInfo
{
    public string id;
    public string name;
    public int health;
    public string drop;
    public string texture;
}

[Serializable]
public class BlockInfoArray
{
    public BlockInfo[] blockInfos;
}

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

            LoadBlockInfo();
        }
        else
        { Destroy(gameObject); }
    }
    #endregion

    private Dictionary<string, BlockInfo> blockInfoDictionary;

    private void LoadBlockInfo()
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