using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item
{
    private string name;
    private string type;
    private Vector2 offset;
    private int stack;

    public string Name { get => name; set => name = value; }
    public string Type { get => type; set => type = value; }
    public Vector2 Offset { get => offset; set => offset = value; }
    public int Stack { get => stack; set => stack = value; }

    public abstract bool ToggleItem(ClientPlayer player);
    public virtual void InitializationProperties(ItemInfo itemInfo) { }
    public void ExecuteGetSet(ItemInfo itemInfo) => GetSetItem(itemInfo);

    private void GetSetItem(ItemInfo itemInfo)
    {
        this.name = itemInfo.name;
        type = itemInfo.type;
        offset = itemInfo.offset;
        stack = 1;
    }
}

public class FoodItem : Item
{
    private int health;
    private int food;

    public override bool ToggleItem(ClientPlayer player)
    {
        Debug.Log("À½½Ä µé¾î¿È");

        player.Health += health;
        player.Food += food;

        return true;
    }

    public override void InitializationProperties(ItemInfo itemInfo)
    {
        ItemFood itemFood = ItemManager.Instance.GetItemFoodData(itemInfo.name);

        health = itemFood.health;
        food = itemFood.food;
    }
}

public class MaterialItem : Item
{
    public override bool ToggleItem(ClientPlayer player) 
    {
        return false;
    }
}

public class EquipmentItem : Item
{
    private string type;
    private int value;

    public override bool ToggleItem(ClientPlayer player)
    {
        return false;
    }

    public override void InitializationProperties(ItemInfo itemInfo)
    {
        ItemEquipment itemEquipment = ItemManager.Instance.GetItemEquipmentData(itemInfo.name);

        type = itemEquipment.type;
        value = itemEquipment.value;
    }

    public string Type { get => type; set => type = value; }
    public int Value { get => value; set => this.value = value; }
}

public class BlockItem : Item
{
    public override bool ToggleItem(ClientPlayer player)
    {
        return true;
    }
}