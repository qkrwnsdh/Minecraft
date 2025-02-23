using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class ClientContainer
{
    protected Transform[][] slots;

    public Transform[][] GetSlots { get => slots; }

    public abstract void InitializationClientData();

    public void InitializationInstanceMaterial()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            for (int j = 0; j < slots[i].Length; j++)
            {
                (Image image, TextMeshProUGUI text) = GFunc.GetSlotComponent(slots[i][j]);

                ItemManager.Instance.NewItemMaterial(image);
                text.text = "";
            }
        }
    }

    public void UpdateSlotFull(Item[][] items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            for (int j = 0; j < slots[i].Length; j++)
            {
                if (items[i][j] != null)
                {
                    ItemManager.Instance.SetSlotTrueMaterial(slots[i][j], items[i][j]);
                }
            }
        }
    }

    public void SetMaterialForTransform(Transform items, Item item = null)
    {
        if (item != null) ItemManager.Instance.SetSlotTrueMaterial(items, item);
        else ItemManager.Instance.SetSlotFalseMaterial(items);
    }

    public Transform[][] GetData() => slots;
}
