using System.Collections.Generic;
using UnityEngine;

public static class JsonReader
{
    public static Dictionary<TType, TItem> LoadToJson<TType, TItem, TData>(string path)
    {
        TextAsset json = Resources.Load<TextAsset>(path);

        Dictionary<TType, TItem> dictionary = new Dictionary<TType, TItem>();

        if (json != null)
        {
            TData data = JsonUtility.FromJson<TData>(json.text);

            if (data is BlockInfoData blockInfoData)
            {
                foreach (var blockInfo in blockInfoData.blockInfos)
                {
                    if (blockInfo is TItem item && blockInfo.name is TType type)
                    {
                        dictionary[type] = item;
                    }
                }
            }
            else if (data is BlockCreateData blockCreateData)
            {
                foreach (var blockCreate in blockCreateData.blockCreates)
                {
                    if (blockCreate is TItem item && blockCreate.name is TType type)
                    {
                        dictionary[type] = item;
                    }
                }
            }
            else if (data is ItemInfoData itemInfoData)
            {
                foreach (var itemInfo in itemInfoData.itemInfos)
                {
                    if (itemInfo is TItem item && itemInfo.name is TType type)
                    {
                        dictionary[type] = item;
                    }
                }
            }
            else if (data is ItemFoodData itemFoodData)
            {
                foreach (var itemfood in itemFoodData.itemFoods)
                {
                    if (itemfood is TItem item && itemfood.name is TType type)
                    {
                        dictionary[type] = item;
                    }
                }
            }
            else if (data is ItemEquipmentData itemEquipmentData)
            {
                foreach (var itemEquipment in itemEquipmentData.itemEquipments)
                {
                    if (itemEquipment is TItem item && itemEquipment.name is TType type)
                    {
                        dictionary[type] = item;
                    }
                }
            }
            else if (data is ItemFurnaceData itemFurnaceData)
            {
                foreach (var itemFurnace in itemFurnaceData.itemFurnaces)
                {
                    if (itemFurnace is TItem item && itemFurnace.material is TType type)
                    {
                        dictionary[type] = item;
                    }
                }
            }
            else if (data is ItemCraftData itemCraftData)
            {
                foreach (var itemCraft in itemCraftData.itemCrafts)
                {
                    if (itemCraft is TItem item && itemCraft.result.name is TType type)
                    {
                        dictionary[type] = item;
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