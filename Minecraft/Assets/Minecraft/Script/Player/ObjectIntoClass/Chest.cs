using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Container
{
    #region Feild
    private Item[] chests = new Item[27];
    #endregion

    #region Get Feild
    public Item[] GetChests { get => chests; }
    #endregion

    #region Core Method

    #region LeftClick
    // (Left) 컨테이너에 저장된 아이템을 드래그 오브젝트로 옮기는 메서드
    protected override void ProcessOnLeftClickContainerItemToDragItem(int i, int j)
    {
        if (GetItemByIndex(i) == chests)
        {
            DefaultSingleContainerItemToDragItem(this, i, j, chests);
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

        if (GetItemByIndex(i) == chests)
        {
            DefaultSingleDragItemToContainerItem(j, chests);
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

        if (GetItemByIndex(i) == chests)
        {
            DefaultSingleInteractionItem(i, j, chests);
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
        if (GetItemByIndex(i) == chests)
        {
            DefaultFullContainerItemToDragItem(this, i, j, chests);
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

        if (GetItemByIndex(i) == chests)
        {
            DefaultFullDragItemToContainerItem(j, chests);
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

        if (GetItemByIndex(i) == chests)
        {
            DefaultFullInteractionItem(this, i, j, chests);
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
        if (i == 0)
        {
            UpdateExecuteChest();
        }
    }

    void UpdateExecuteChest()
    {
        /* No Event */
    }
    #endregion

    #region Helper Method
    public void SetMaterialForAfterInteraction(int i, int j)
    {
        if (clientWindow is ClientChest chest)
        {
            chest.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
        }
    }
    
    Item[] GetItemByIndex(int i)
    {
        if (i == 0)
        {
            return chests;
        }
        else
        {
            Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

            return null;
        }
    }

    Transform[] GetTransformByIndex(int i)
    {
        if (clientWindow is ClientChest chest)
        {
            if (i == 0)
            {
                return chest.chests;
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
            return chests[j] != null;
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

        if (clientWindow is ClientChest chest)
        {
            chest.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
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