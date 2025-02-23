using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : Container
{
    #region Feild
    private Item[] crafts = new Item[10];
    #endregion

    #region Get Feild
    public Item[] GetCrafts { get => crafts; }
    #endregion

    #region Core Method

    #region LeftClick
    // (Left) 컨테이너에 저장된 아이템을 드래그 오브젝트로 옮기는 메서드
    protected override void ProcessOnLeftClickContainerItemToDragItem(int i, int j)
    {
        if (GetItemByIndex(i) == crafts && j < crafts.Length - 1)
        {
            DefaultSingleContainerItemToDragItem(this, i, j, crafts);
        }
        else if (GetItemByIndex(i) == crafts && j == crafts.Length - 1)
        {
            CraftSingleContainerItemToDragItem(i, j, crafts);
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

        if (GetItemByIndex(i) == crafts)
        {
            DefaultSingleDragItemToContainerItem(j, crafts);
        }

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        Debug.Log($"ProcessOnLeftClickDragItemToContainerItem i = {i}, j = {j}");

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);

        return true;
    }

    // (Left) 드래그 오브젝트에 저장된 아이템과 컨테이너 아이템의 상호작용 메서드
    protected override void ProcessOnLeftClickInteractionItem(int i, int j)
    {
        if (!ValidItemForSlot(i, j)) return;

        if (GetItemByIndex(i) == crafts)
        {
            DefaultSingleInteractionItem(i, j, crafts);
        }

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);
    }

    private void CraftSingleContainerItemToDragItem(int i, int j, Item[] items)
    {
        // 드래그 오브젝트에 아이템을 추가
        dragObject.SetItem = items[j];

        // 호출된 인덱스 번호 저장
        dragObject.SetSaveData(this, items, i, j);

        // 재료 소모
        bool allMaterialsAvailable = true;  // 재료가 모두 있는지 여부

        for (int index = 0; index < crafts.Length - 1; index++)
        {
            if (crafts[index] != null)
            {
                crafts[index].Stack--;

                if (crafts[index].Stack == 0)
                {
                    crafts[index] = null;
                    allMaterialsAvailable = false;
                }
            }
        }

        // 재료 소모 후 처리
        if (!allMaterialsAvailable)
        {
            items[j] = null;
        }

        // 상호작용 후에 재료 상태 설정
        for (int index = 0; index < crafts.Length; index++)
        {
            SetMaterialForAfterInteraction(i, index);
        }
    }
    #endregion

    #region RightClick

    // (Right) 컨테이너에 저장된 아이템을 드래그 오브젝트로 옮기는 메서드
    protected override void ProcessOnRightClickContainerItemToDragItem(int i, int j)
    {
        if (GetItemByIndex(i) == crafts && j < crafts.Length - 1)
        {
            DefaultFullContainerItemToDragItem(this, i, j, crafts);
        }
        else if (GetItemByIndex(i) == crafts && j == crafts.Length - 1)
        {
            CraftFullContainerItemToDragItem(i, j, crafts);
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

        if (GetItemByIndex(i) == crafts)
        {
            DefaultFullDragItemToContainerItem(j, crafts);
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

        if (GetItemByIndex(i) == crafts)
        {
            DefaultFullInteractionItem(this, i, j, crafts);
        }

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);
    }

    private void CraftFullContainerItemToDragItem(int i, int j, Item[] items)
    {
        // 드래그 오브젝트에 아이템을 추가 및 호출된 인덱스 번호 저장
        dragObject.NewDragObjectItem(items[j].Name);
        dragObject.SetSaveData(this, items, i, j);

        int initStack = items[j].Stack;  // 결과물 아이템의 초기 스택 값
        int totalStack = initStack;

        for (; totalStack < Define.MAX_STACK; totalStack += initStack)
        {
            bool allMaterialsAvailable = true;  // 재료가 모두 있는지 여부

            // 재료 슬롯을 순회하며 재료 소모
            for (int index = 0; index < crafts.Length - 1; index++)
            {
                if (crafts[index] != null)
                {
                    // 재료 하나 소모
                    crafts[index].Stack--;

                    // 재료가 없다면 해당 재료 비워주기
                    if (crafts[index].Stack == 0)
                    {
                        crafts[index] = null;
                        allMaterialsAvailable = false;
                    }
                }
            }

            if (!allMaterialsAvailable)
            {
                items[j] = null;
                break;
            }
        }

        dragObject.GetItem.Stack = totalStack;

        // 상호작용 후에 재료 상태 설정
        for (int index = 0; index < crafts.Length; index++)
        {
            SetMaterialForAfterInteraction(i, index);
        }
    }
    #endregion

    #endregion

    #region Execute Method
    protected override void UpdateExecuteByIndex(int i, int j)
    {
        if (i == 0 && j < crafts.Length - 1)
        {
            UpdateExecuteMaterial();
        }
        else if (i == 0 && j == crafts.Length - 1)
        {
            UpdateExecuteResult();
        }
        else
        {
            Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

            return;
        }
    }

    void UpdateExecuteMaterial()
    {
        ItemResult result = ItemManager.Instance.GetCraftResult(crafts);

        if (result != null)
        {
            Item resultItem = ItemManager.Instance.NewItem(result.name);
            resultItem.Stack = int.Parse(result.stack);

            crafts[crafts.Length - 1] = resultItem;
        }
        else
        {
            crafts[crafts.Length - 1] = null;
        }

        SetMaterialForAfterInteraction(0, crafts.Length - 1);
    }

    void UpdateExecuteResult()
    {
        /* No Event */
    }
    #endregion

    #region Helper Method
    public void SetMaterialForAfterInteraction(int i, int j)
    {
        if (clientWindow is ClientCraft craft)
        {
            craft.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
        }
    }

    Item[] GetItemByIndex(int i)
    {
        if (i == 0)
        {
            return crafts;
        }
        else
        {
            Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

            return null;
        }
    }

    Transform[] GetTransformByIndex(int i)
    {
        if (clientWindow is ClientCraft craft)
        {
            if (i == 0)
            {
                return craft.crafts;
            }
            else
            {
                Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

                return null;
            }
        }
        else
        {
            Debug.LogError("열려있는 윈도우 창이 올바르지 않습니다.");

            return null;
        }
    }

    protected override bool HasItemInSlot(int i, int j)
    {
        if (i == 0)
        {
            return crafts[j] != null;
        }
        else
        {
            Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

            return false;
        }
    }

    protected override void SetMaterialSetup(int i, int j)
    {
        // 드래그 오브젝트 오프셋 조절
        dragObject.SetMaterialForDragObject();

        // 열려있는 창에 해당하는 슬롯 아이템 오프셋 조절
        if (clientWindow is ClientCraft craft)
        {
            craft.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
        }
    }
    #endregion

    #region Valid Method
    protected override bool ValidItemForSlot(int i, int j)
    {
        Item item = dragObject.GetItem;

        if (i == 0 && j < crafts.Length - 1)
        {
            return ValidItemForMaterial(j, item);
        }
        else if (i == 0 && j < crafts.Length - 1)
        {
            return ValidItemForResult(j, item);
        }
        else
        {
            return false;
        }
    }

    bool ValidItemForMaterial(int j, Item item)
    {
        // 1. 재료 슬롯에 속하는지
        // 2. 아이템 타입이 장비 타입이 아닌지
        if (0 <= j && j < crafts.Length)
        {
            return item.Type != "Equipment";
        }

        return false;
    }

    bool ValidItemForResult(int j, Item item)
    {
        // 결과란에는 아이템을 추가할 수 없습니다.
        return false;
    }
    #endregion
}