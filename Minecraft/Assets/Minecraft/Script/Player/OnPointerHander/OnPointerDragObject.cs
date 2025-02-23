using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPointerDragObject : MonoBehaviour
{
    private Item item;
    private object saveContainer;
    private Item[] saveArray;
    private int saveIndexI;
    private int saveIndexJ;

    public Item GetItem { get => item; }
    public Item SetItem { set => item = value; }
    public Item[] GetSaveArray { get => saveArray; }
    public Item[] SetSaveArray { set => saveArray = value; }
    public int GetSaveIndexI { get => saveIndexI; }
    public int GetSaveIndexJ { get => saveIndexJ; }

    public void SetMaterialForDragObject()
    {
        if (item != null) ItemManager.Instance.SetSlotTrueMaterial(transform, item);
        else ItemManager.Instance.SetSlotFalseMaterial(transform);
    }

    public void NewDragObjectItem(string name)
    {
        item = ItemManager.Instance.NewItem(name);
    }

    public void SetSaveData(object container, Item[] array, int i, int j)
    {
        saveContainer = container;
        saveArray = array;
        saveIndexI = i;
        saveIndexJ = j;
    }

    public bool SetVisibleDragObject()
    {
        gameObject.SetActive(item != null);
        return gameObject.activeSelf;
    }

    public void SetCurrentContainerMaterial()
    {
        switch (saveContainer)
        {
            case ClientData data:
                data.SetMaterialForAfterInteraction(saveIndexI, saveIndexJ);
                break;
            case Inventory inventory:
                inventory.SetMaterialForAfterInteraction(saveIndexI, saveIndexJ);
                break;
            case Furnace furnace:
                furnace.SetMaterialForAfterInteraction(saveIndexI, saveIndexJ);
                break;
            case Craft craft:
                craft.SetMaterialForAfterInteraction(saveIndexI, saveIndexJ);
                break;
            case Chest chest:
                chest.SetMaterialForAfterInteraction(saveIndexI, saveIndexJ);
                break;

        }
    }
}