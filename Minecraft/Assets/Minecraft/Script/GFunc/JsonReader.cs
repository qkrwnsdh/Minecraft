using System.Collections.Generic;
using UnityEngine;

public static class JsonReader
{
    public static Dictionary<string, TItem> LoadBlockInfoJson<TItem, TData>(string path)
        where TItem : class
        where TData : class
    {
        TextAsset json = Resources.Load<TextAsset>(path);
        Dictionary<string, TItem> dictionary = new Dictionary<string, TItem>();

        if (json != null)
        {
            Debug.Log("JSON file loaded successfully.");

            TData data = JsonUtility.FromJson<TData>(json.text);

            if (data is BlockInfoData blockInfoData)
            {
                Debug.Log("Data deserialized as BlockInfoData.");

                foreach (var blockInfo in blockInfoData.blockInfos)
                {
                    if (blockInfo is TItem item)
                    {
                        dictionary[blockInfo.name] = item;
                        Debug.Log($"Added {blockInfo.name} to dictionary.");
                    }
                    else
                    {
                        Debug.LogError($"Expected type {typeof(TItem)}, but got {blockInfo.GetType()}.");
                    }
                }
            }
            else
            {
                Debug.LogError("Unsupported data type: " + typeof(TData));
            }
        }
        else
        {
            Debug.LogError("JSON file not found in Resources: " + path);
        }

        return dictionary;
    }
}