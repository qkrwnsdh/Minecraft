using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientData : Container
{
    #region Feild
    private ClientQuick quick;
    private Item[] inventorys = new Item[27];
    private Item[] quicks = new Item[9];
    #endregion

    #region Get Feild
    public Item[] GetInventorys { get => inventorys; }
    public Item[] GetQuicks { get => quicks; }
    public ClientQuick SetQuick { set => quick = value; }
    #endregion

    void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            AddItem("Item_Wood_Raw");
            AddItem("Item_Diamond_Raw");
            AddItem("Item_Coal_Raw");
            AddItem("Item_Iron_Raw");
            AddItem("Item_Stone_Raw");
        }
    }

    #region Core Method

    #region LeftClick
    // (Left) 컨테이너에 저장된 아이템을 드래그 오브젝트로 옮기는 메서드
    protected override void ProcessOnLeftClickContainerItemToDragItem(int i, int j)
    {
        if (GetItemByIndex(i) == inventorys)
        {
            DefaultSingleContainerItemToDragItem(this, i, j, inventorys);
        }
        else if (GetItemByIndex(i) == quicks)
        {
            DefaultSingleContainerItemToDragItem(this, i, j, quicks);
        }

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);
    }

    // (Left) 드래그 오브젝트에 저장된 아이템을 컨테이너로 옮기는 메서드
    protected override bool ProcessOnLeftClickDragItemToContainerItem(int i, int j)
    {
        if (!ValidItemForSlot(i, j)) return false;

        if (GetItemByIndex(i) == inventorys)
        {
            DefaultSingleDragItemToContainerItem(j, inventorys);
        }
        else if (GetItemByIndex(i) == quicks)
        {
            DefaultSingleDragItemToContainerItem(j, quicks);
        }

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);

        return true;
    }

    // (Left) 드래그 오브젝트에 저장된 아이템과 컨테이너 아이템의 상호작용 메서드
    protected override void ProcessOnLeftClickInteractionItem(int i, int j)
    {
        if (!ValidItemForSlot(i, j)) return;

        if (GetItemByIndex(i) == inventorys)
        {
            DefaultSingleInteractionItem(i, j, inventorys);
        }
        else if (GetItemByIndex(i) == quicks)
        {
            DefaultSingleInteractionItem(i, j, quicks);
        }

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);
    }
    #endregion

    #region RightClick
    // (Right) 컨테이너에 저장된 아이템을 드래그 오브젝트로 옮기는 메서드
    protected override void ProcessOnRightClickContainerItemToDragItem(int i, int j)
    {
        if (GetItemByIndex(i) == inventorys)
        {
            DefaultFullContainerItemToDragItem(this, i, j, inventorys);
        }
        else if (GetItemByIndex(i) == quicks)
        {
            DefaultFullContainerItemToDragItem(this, i, j, quicks);
        }

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);
    }

    // (Right) 드래그 오브젝트에 저장된 아이템을 컨테이너로 옮기는 메서드
    protected override bool ProcessOnRightClickDragItemToContainerItem(int i, int j)
    {
        if (!ValidItemForSlot(i, j)) return false;

        if (GetItemByIndex(i) == inventorys)
        {
            DefaultFullDragItemToContainerItem(j, inventorys);
        }
        else if (GetItemByIndex(i) == quicks)
        {
            DefaultFullDragItemToContainerItem(j, quicks);
        }

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);

        return true;
    }

    // (Right) 드래그 오브젝트에 저장된 아이템과 컨테이너 아이템의 상호작용 메서드
    protected override void ProcessOnRightClickInteractionItem(int i, int j)
    {
        if (!ValidItemForSlot(i, j)) return;

        if (GetItemByIndex(i) == inventorys)
        {
            DefaultFullInteractionItem(this, i, j, inventorys);
        }
        else if (GetItemByIndex(i) == quicks)
        {
            DefaultFullInteractionItem(this, i, j, quicks);
        }

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);
    }
    #endregion

    #endregion

    #region ExecuteMethod
    protected override void UpdateExecuteByIndex(int i, int j)
    {
        switch (clientWindow)
        {
            case ClientInventory inventory:
                if (i == 2)
                {
                    UpdateExecuteInventory();
                }
                else if (i == 3)
                {
                    UpdateExecuteQuick();
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");
                }

                break;
            case ClientFurnace furnace:
                if (i == 1)
                {
                    UpdateExecuteInventory();
                }
                else if (i == 2)
                {
                    UpdateExecuteQuick();
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");
                }

                break;
            case ClientCraft craft:
                if (i == 1)
                {
                    UpdateExecuteInventory();
                }
                else if (i == 2)
                {
                    UpdateExecuteQuick();
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");
                }

                break;
            case ClientChest chest:
                if (i == 1)
                {
                    UpdateExecuteInventory();
                }
                else if (i == 2)
                {
                    UpdateExecuteQuick();
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");
                }

                break;
            default:
                Debug.LogError("열려있는 윈도우 창이 올바르지 않습니다.");

                break;

        }
    }

    void UpdateExecuteInventory()
    {
        /* No Event */
    }

    void UpdateExecuteQuick()
    {
        for (int i = 0; i < quicks.Length; i++)
        {
            quick.SetMaterialForTransform(quick.quicks[i], quicks[i]);
        }
    }
    #endregion

    #region Add Item
    public void AddItem(string itemName)
    {
        Item newItem = ItemManager.Instance.NewItem(itemName);

        if (!TryInsertItem(newItem)) Debug.Log("Inventory is full");
    }

    bool TryInsertItem(Item item)
    {
        Item[][] items = { quicks, inventorys };

        if (item.Type == "Equipment")
        {
            if (TryPlaceItemInEmptySlot(item, items)) return true;
        }
        else
        {
            if (TryAddToExistingStack(item, items)) return true;
            if (TryPlaceItemInEmptySlot(item, items)) return true;
        }

        return false;
    }

    bool TryPlaceItemInEmptySlot(Item item, Item[][] slots)
    {
        foreach (var slotArray in slots)
        {
            for (int i = 0; i < slotArray.Length; i++)
            {
                if (slotArray[i] == null)
                {
                    slotArray[i] = item;
                    return true;
                }
            }
        }

        return false;
    }

    bool TryAddToExistingStack(Item item, Item[][] slots)
    {
        foreach (var slotArray in slots)
        {
            foreach (var slot in slotArray)
            {
                if (slot != null && slot.Name == item.Name && slot.Stack < Define.MAX_STACK)
                {
                    slot.Stack++;
                    return true;
                }
            }
        }

        return false;
    }
    #endregion

    #region Toggle Quick Item
    public void ToggleItem(ClientPlayer player, string num, Block block = null, Vector3? hitDirection = null)
    {
        if (int.TryParse(num.Replace("Num", ""), out int index))
        {
            ExecuteIfItemExists(player, index - 1, block, hitDirection);
        }
    }

    void ExecuteIfItemExists(ClientPlayer player, int num, Block block = null, Vector3? hitDirection = null)
    {
        if (quicks[num] != null)
        {
            if (quicks[num] is BlockItem blockItem)
            {
                if (block != null && hitDirection != null)
                {
                    string name = BlockManager.Instance.GetBlockCreateData(blockItem.Name).result;

                    if (block.CreateBlock(name, hitDirection))
                    {
                        SetBlockMinusCount(num);
                    }
                }
            }
            else
            {
                if (quicks[num].ToggleItem(player))
                {
                    SetBlockMinusCount(num);
                }
            }
        }
    }

    void SetBlockMinusCount(int num)
    {
        quicks[num].Stack--;

        if (quicks[num].Stack <= 0)
        {
            quicks[num] = null;
        }

        quick.SetMaterialForTransform(quick.quicks[num], quicks[num]);
    }
    #endregion

    #region Helper Method
    public void SetMaterialForAfterInteraction(int i, int j)
    {
        switch (clientWindow)
        {
            case ClientInventory inventory:
                inventory.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
                break;
            case ClientFurnace furnace:
                furnace.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
                break;
            case ClientCraft craft:
                craft.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
                break;
            case ClientChest chest:
                chest.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
                break;
        }
    }

    Item[] GetItemByIndex(int i)
    {
        switch (clientWindow)
        {
            case ClientInventory inventory:
                if (i == 2)
                {
                    return inventorys;
                }
                else if (i == 3)
                {
                    return quicks;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return null;
                }
            case ClientFurnace furnace:
                if (i == 1)
                {
                    return inventorys;
                }
                else if (i == 2)
                {
                    return quicks;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return null;
                }
            case ClientCraft craft:
                if (i == 1)
                {
                    return inventorys;
                }
                else if (i == 2)
                {
                    return quicks;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return null;
                }
            case ClientChest chest:
                if (i == 1)
                {
                    return inventorys;
                }
                else if (i == 2)
                {
                    return quicks;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return null;
                }
            default:
                Debug.LogError("열려있는 윈도우 창이 올바르지 않습니다.");

                return null;
        }
    }

    Transform[] GetTransformByIndex(int i)
    {
        switch (clientWindow)
        {
            case ClientInventory inventory:
                if (i == 2)
                {
                    return inventory.inventorys;
                }
                else if (i == 3)
                {
                    return inventory.quicks;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return null;
                }
            case ClientFurnace furnace:
                if (i == 1)
                {
                    return furnace.inventorys;
                }
                else if (i == 2)
                {
                    return furnace.quicks;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return null;
                }
            case ClientCraft craft:
                if (i == 1)
                {
                    return craft.inventorys;
                }
                else if (i == 2)
                {
                    return craft.quicks;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return null;
                }
            case ClientChest chest:
                if (i == 1)
                {
                    return chest.inventorys;
                }
                else if (i == 2)
                {
                    return chest.quicks;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return null;
                }
            default:
                Debug.LogError("열려있는 윈도우 창이 올바르지 않습니다.");

                return null;
        }
    }

    protected override bool HasItemInSlot(int i, int j)
    {
        switch (clientWindow)
        {
            case ClientInventory inventory:
                if (i == 2)
                {
                    return inventorys[j] != null;
                }
                else if (i == 3)
                {
                    return quicks[j] != null;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return false;
                }
            case ClientFurnace furnace:
                if (i == 1)
                {
                    return inventorys[j] != null;
                }
                else if (i == 2)
                {
                    return quicks[j] != null;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return false;
                }
            case ClientCraft craft:
                if (i == 1)
                {
                    return inventorys[j] != null;
                }
                else if (i == 2)
                {
                    return quicks[j] != null;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return false;
                }
            case ClientChest chest:
                if (i == 1)
                {
                    return inventorys[j] != null;
                }
                else if (i == 2)
                {
                    return quicks[j] != null;
                }
                else
                {
                    Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                    return false;
                }
            default:
                Debug.LogError($"{i}열려있는 윈도우 창이 올바르지 않습니다.");

                return false;
        }
    }

    protected override void SetMaterialSetup(int i, int j)
    {
        // 드래그 오브젝트 오프셋 조절
        dragObject.SetMaterialForDragObject();

        switch (clientWindow)
        {
            case ClientInventory inventory:
                inventory.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
                break;
            case ClientFurnace furnace:
                furnace.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
                break;
            case ClientCraft craft:
                craft.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
                break;
            case ClientChest chest:
                chest.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
                break;
        }
    }
    #endregion

    #region Valid Method
    protected override bool ValidItemForSlot(int i, int j)
    {
        return true;
    }
    #endregion
}