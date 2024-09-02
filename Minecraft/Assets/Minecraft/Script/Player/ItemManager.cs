using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region Singleton and Awake()
    public static ItemManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //blockInfoDictionary = JsonReader.LoadBlockInfoJson<BlockInfo, BlockInfoData>(Define.PATH_BLOCK_INFO);
            //blockDatas = new Dictionary<string, BlockData>();

            //// BlockData 객체들을 초기화하여 blockDatas에 저장
            //foreach (var blockInfo in blockInfoDictionary.Values)
            //{
            //    blockDatas[blockInfo.name] = new BlockData(blockInfo);
            //}
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    //private Dictionary<string, ItemInfo> ItemInfoDictionary;
}


