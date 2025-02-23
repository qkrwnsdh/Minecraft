using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

            InitializationDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializationDictionary()
    {
        itemInfoDictionary = JsonReader.LoadToJson<string, ItemInfo, ItemInfoData>(Define.PATH_ITEM_INFO);
        itemFoodDictionary = JsonReader.LoadToJson<string, ItemFood, ItemFoodData>(Define.PATH_ITEM_FOOD);
        itemEquipmentDictionary = JsonReader.LoadToJson<string, ItemEquipment, ItemEquipmentData>(Define.PATH_ITEM_EQUIPMENT);
        itemFurnaceDictionary = JsonReader.LoadToJson<string, ItemFurnace, ItemFurnaceData>(Define.PATH_ITEM_FURNACE);
        itemCraftDictionary = JsonReader.LoadToJson<string, ItemCraft, ItemCraftData>(Define.PATH_ITEM_CRAFT);
    }
    #endregion

    #region New Item Setting
    public Material itemMaterial;

    public void NewItemMaterial(Image target)
    {
        Material material = new Material(itemMaterial);
        material.SetTextureOffset("_MainTex", new Vector2(0.875f, 0.125f));

        target.material = material;
    }

    public void SetSlotTrueMaterial(Transform slot, Item item)
    {
        (Image image, TextMeshProUGUI text) = GFunc.GetSlotComponent(slot);

        image.material.SetTextureOffset("_MainTex", GetItemInfoData(item.Name).offset);
        text.text = item.Stack.ToString();
    }

    public void SetSlotFalseMaterial(Transform slot)
    {
        (Image image, TextMeshProUGUI text) = GFunc.GetSlotComponent(slot);

        image.material.SetTextureOffset("_MainTex", new Vector2(0.875f, 0.125f));
        text.text = "";
    }

    public Item NewItem(string name)
    {
        ItemInfo itemInfo = GetItemInfoData(name);
        Item item = CreateItemFromInfo(itemInfo);

        return item;
    }

    Item CreateItemFromInfo(ItemInfo itemInfo)
    {
        switch (itemInfo.type)
        {
            case "Food":
                FoodItem foodItem = new FoodItem();
                foodItem.ExecuteGetSet(itemInfo);
                foodItem.InitializationProperties(itemInfo);
                return foodItem;
            case "Material":
                MaterialItem materialItem = new MaterialItem();
                materialItem.ExecuteGetSet(itemInfo);
                return materialItem;
            case "Equipment":
                EquipmentItem equipmentItem = new EquipmentItem();
                equipmentItem.ExecuteGetSet(itemInfo);
                equipmentItem.InitializationProperties(itemInfo);
                return equipmentItem;
            case "Block":
                BlockItem blockItem = new BlockItem();
                blockItem.ExecuteGetSet(itemInfo);
                return blockItem;
            default:
                return null;
        }
    }
    #endregion

    #region Valid In Slot
    public bool ValidSlotFurnaceFuel(Item item)
    {
        return item.Name == "Item_Coal_Raw" ? true : false;
    }

    public bool ValidSlotFurnaceMaterial(Item item)
    {
        return GetFurnaceItem(item) != null ? true : false;
    }

    public bool ValidSlotCraftMaterial(Item item)
    {
        return item.Type != "Equipment" ? true : false;
    }

    public bool ValidSlotEquipment(int j, Item item)
    {
        switch (j)
        {
            case 0 when item is EquipmentItem helmet:
                return helmet.Type == "Helmet" ? true : false;
            case 1 when item is EquipmentItem chest:
                return chest.Type == "Chest" ? true : false;
            case 2 when item is EquipmentItem leg:
                return leg.Type == "Leg" ? true : false;
            case 3 when item is EquipmentItem boots:
                return boots.Type == "Boots" ? true : false;
            case 4 when item is EquipmentItem weapone:
                return weapone.Type == "Weapone" ? true : false;
            default: return false;
        }
    }
    #endregion

    #region ItemData

    #region Item Info
    private Dictionary<string, ItemInfo> itemInfoDictionary;

    public ItemInfo GetItemInfoData(string name)
    {
        if (itemInfoDictionary.TryGetValue(name, out ItemInfo itemInfo))
        {
            return itemInfo;
        }

        Debug.LogError($"{name} Item name not found");
        return null;
    }
    #endregion

    #region Item Food
    private Dictionary<string, ItemFood> itemFoodDictionary;

    public ItemFood GetItemFoodData(string name)
    {
        if (itemFoodDictionary.TryGetValue(name, out ItemFood itemFood))
        {
            return itemFood;
        }

        Debug.LogError("Item name not found");
        return null;
    }
    #endregion

    #region Item Equipment
    private Dictionary<string, ItemEquipment> itemEquipmentDictionary;

    public ItemEquipment GetItemEquipmentData(string name)
    {
        if (itemEquipmentDictionary.TryGetValue(name, out ItemEquipment itemEquipment))
        {
            return itemEquipment;
        }

        Debug.LogError("Item name not found");
        return null;
    }
    #endregion

    #region Item Furnace
    private Dictionary<string, ItemFurnace> itemFurnaceDictionary;

    public ItemFurnace GetFurnaceItem(Item material)
    {
        foreach (var kvp in itemFurnaceDictionary)
        {
            if (kvp.Key == material.Name)
            {
                return kvp.Value;
            }
        }

        return null;
    }

    public ItemFurnace GetItemFurnaceData(string name)
    {
        if (itemFurnaceDictionary.TryGetValue(name, out ItemFurnace itemFurnace))
        {
            return itemFurnace;
        }

        Debug.LogError("Item name not found");
        return null;
    }
    #endregion

    #region Item Craft
    private Dictionary<string, ItemCraft> itemCraftDictionary;

    public ItemResult GetCraftResult(Item[] crafts)
    {
        int craftsLengthSqrt = MathUtility.CalculateSqrt(crafts.Length - 1);
        string[,] materials = new string[craftsLengthSqrt, craftsLengthSqrt];

        for (int i = 0; i < materials.Length; i++)
        {
            if (crafts[i] != null)
            {
                int x = i / craftsLengthSqrt;
                int y = i % craftsLengthSqrt;

                materials[x, y] = crafts[i].Name;
            }
        }

        ItemResult result = GetItemCraftResult(materials);

        return result != null ? result : null;
    }

    public ItemResult GetItemCraftResult(string[,] inputMaterials)
    {
        foreach (var kvp in itemCraftDictionary)
        {
            foreach (var material in kvp.Value.materials)
            {
                if (FindMatchingCraft(inputMaterials, material.material))
                {
                    return kvp.Value.result;
                }
            }
        }

        return null;
    }

    bool FindMatchingCraft(string[,] inputMaterials, List<ItemMaterial> recipeMaterials)
    {
        int filledCount = 0;
        int matchedCount = 0;

        for (int i = 0; i < inputMaterials.GetLength(0); i++)
        {
            for (int j = 0; j < inputMaterials.GetLength(1); j++)
            {
                string materialName = inputMaterials[i, j];

                if (!string.IsNullOrEmpty(materialName))
                {
                    filledCount++;

                    foreach (var recipeMaterial in recipeMaterials)
                    {
                        if (recipeMaterial.x == i && recipeMaterial.y == j && materialName == recipeMaterial.name)
                        {
                            matchedCount++;
                            break;
                        }
                    }
                }
            }
        }

        return recipeMaterials.Count == matchedCount && filledCount == matchedCount;
    }
    #endregion

    #endregion
}

#region Item Attribute

#region ItemInfo
[Serializable]
public class ItemInfo
{
    public string name;
    public string type;
    public Vector2 offset;
}

[Serializable]
public class ItemInfoData
{
    public ItemInfo[] itemInfos;
}
#endregion

#region ItemFood
[Serializable]
public class ItemFood
{
    public string name;
    public int health;
    public int food;
}

[Serializable]
public class ItemFoodData
{
    public ItemFood[] itemFoods;
}
#endregion

#region ItemEquipment
[Serializable]
public class ItemEquipment
{
    public string name;
    public string type;
    public int value;
}

[Serializable]
public class ItemEquipmentData
{
    public ItemEquipment[] itemEquipments;
}
#endregion

#region ItemFurnace
[Serializable]
public class ItemFurnace
{
    public string result;
    public string material;
    public int time;
}

[Serializable]
public class ItemFurnaceData
{
    public ItemFurnace[] itemFurnaces;
}
#endregion

#region ItemCraft
[Serializable]
public class ItemResult
{
    public string name;
    public string stack;
}

[Serializable]
public class ItemMaterial
{
    public int x;
    public int y;
    public string name;
}

[Serializable]
public class ItemMaterials
{
    public List<ItemMaterial> material;
}

[Serializable]
public class ItemCraft
{
    public ItemResult result;
    public List<ItemMaterials> materials;
}

[Serializable]
public class ItemCraftData
{
    public ItemCraft[] itemCrafts;
}
#endregion

#endregion