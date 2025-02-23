//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public interface ISlotStrategy
//{
//    void AddSlot(Slot slot);
//    void RemoveSlot(Slot slot);
//}

//public enum Slot 
//{
//    Equipment,
//    Craft,
//    Furnace,
//    Chest,
//    Inventory,
//    Quick
//}

//public class EquipmentSlotStrategy : ISlotStrategy
//{
//    private readonly ClientInventory clientInventory;
//    Inventory inventory;

//    public EquipmentSlotStrategy(ClientInventory clientInventory)
//    { 
    
//    }

//    public void AddSlot(string type)
//    { 
    
//    }

//    public void RemoveSlot(string type)
//    { 
    
//    }
//}

////public class CraftSlotStrategy : ISlotStrategy
////{ 

////}

////protected override bool ValidInsertItem(Item[] datas, int j, Item item)
////{
////    if (datas == _Equipments) return ValidItemForEquipment(j, item);
////    else if (datas == _Crafts) return ValidItemForCraft(j, item);

////    try
////    {
////        Debug.Log($"Item Type = {item.Type}");
////    }
////    catch (Exception ex)
////    {
////        Debug.LogError("이상한 타입의 오브젝트가 들어왔습니다." + ex.Message);
////    }

////    return false;
////}

////bool ValidItemForEquipment(int j, Item item)
////{
////    return ItemManager.Instance.ValidSlotEquipment(j, item);
////}

////bool ValidItemForCraft(int j, Item item)
////{
////    if (j == _Crafts.Length - 1)
////    {
////        return false;
////    }

////    return ItemManager.Instance.ValidSlotCraftMaterial(item);
////}
