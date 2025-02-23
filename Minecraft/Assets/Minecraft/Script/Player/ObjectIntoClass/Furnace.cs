using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : Container
{
    #region Feild
    private Coroutine furnaceCoroutine;

    private Item[] furnaces = new Item[3];
    #endregion

    #region Get Feild
    public Item[] GetFurnaces { get => furnaces; }
    #endregion

    #region Core Method

    #region LeftClick
    // (Left) 컨테이너에 저장된 아이템을 드래그 오브젝트로 옮기는 메서드
    protected override void ProcessOnLeftClickContainerItemToDragItem(int i, int j)
    {
        if (GetItemByIndex(i) == furnaces)
        {
            DefaultSingleContainerItemToDragItem(this, i, j, furnaces);
        }

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);
    }

    // (Left) 드래그 오브젝트에 저장된 아이템을 컨테이너로 옮기는 메서드
    protected override bool ProcessOnLeftClickDragItemToContainerItem(int i, int j)
    {
        if (!ValidItemForSlot(i, j)) return false;

        if (GetItemByIndex(i) == furnaces)
        {
            DefaultSingleDragItemToContainerItem(j, furnaces);
        }

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        return true;
    }

    // (Left) 드래그 오브젝트에 저장된 아이템과 컨테이너 아이템의 상호작용 메서드
    protected override void ProcessOnLeftClickInteractionItem(int i, int j)
    {
        if (!ValidItemForSlot(i, j)) return;

        if (GetItemByIndex(i) == furnaces)
        {
            DefaultSingleInteractionItem(i, j, furnaces);
        }

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);
    }
    #endregion

    #region RightClick

    // (Right) 컨테이너에 저장된 아이템을 드래그 오브젝트로 옮기는 메서드
    protected override void ProcessOnRightClickContainerItemToDragItem(int i, int j)
    {
        if (GetItemByIndex(i) == furnaces)
        {
            DefaultFullContainerItemToDragItem(this, i, j, furnaces);
        }

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);
    }

    // (Right) 드래그 오브젝트에 저장된 아이템을 컨테이너로 옮기는 메서드
    protected override bool ProcessOnRightClickDragItemToContainerItem(int i, int j)
    {
        if (!ValidItemForSlot(i, j)) return false;

        if (GetItemByIndex(i) == furnaces)
        {
            DefaultFullDragItemToContainerItem(j, furnaces);
        }

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);

        return true;
    }

    // (Right) 드래그 오브젝트에 저장된 아이템과 컨테이너 아이템의 상호작용 메서드
    protected override void ProcessOnRightClickInteractionItem(int i, int j)
    {
        if (!ValidItemForSlot(i, j)) return;

        if (GetItemByIndex(i) == furnaces)
        {
            DefaultFullInteractionItem(this, i, j, furnaces);
        }

        // 변경된 드래그 아이템과 컨테이너 아이템의 머티리얼 조절
        SetMaterialSetup(i, j);

        // 인덱스 번호에 해당하는 슬롯 업데이트
        UpdateExecuteByIndex(i, j);
    }
    #endregion

    #endregion

    #region Execute Method
    protected override void UpdateExecuteByIndex(int i, int j)
    {
        if (i == 0 && j == 0 || j == 1)
        {
            UpdateExecuteProcess();
        }
        else if (i == 0 && j == 2)
        {
            UpdateExecuteResult();
        }
        else
        {
            Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

            return;
        }
    }

    void UpdateExecuteProcess()
    {
        // 화로 0번 1번에 아이템이 존재할 때
        if (furnaces[0] != null && furnaces[1] != null)
        {
            // 재료에 해당하는 아이템 정보 가져오기
            ItemFurnace itemFurnace = ItemManager.Instance.GetFurnaceItem(furnaces[1]);

            // 화로 코루틴 시작
            furnaceCoroutine = StartCoroutine(FurnaceCoroutine(itemFurnace));
        }
        // 화로 0번 1번에 아이템이 존재하지 않을 때
        else
        {
            // 만약 진행중인 화로 코루틴이 있다면 정지 및 초기화
            if (furnaceCoroutine != null)
            {
                StopCoroutine(furnaceCoroutine);
                furnaceCoroutine = null;
            }
        }
    }

    IEnumerator FurnaceCoroutine(ItemFurnace itemFurnace)
    {
        // 만약 2번에 아이템이 존재하고 같은 아이템을 제작한다면 스택 추가
        if (furnaces[2] != null)
        {
            // 2번에 있는 아이템과 제작하는 아이템의 결과물이 다르거나
            // 2번에 있는 아이템 개수가 최대치 일 경우 코루틴 종료
            if (furnaces[2].Name != itemFurnace.result ||
                Define.MAX_STACK <= furnaces[2].Stack)
            {
                furnaceCoroutine = null;
                yield break;
            }
        }

        // 제작하는대 걸리는 시간 대기
        yield return new WaitForSeconds(itemFurnace.time);

        // 재료 소진
        furnaces[0].Stack--;
        furnaces[1].Stack--;

        if (furnaces[0].Stack == 0)
        {
            furnaces[0] = null;
        }
        if (furnaces[1].Stack == 0)
        {
            furnaces[1] = null;
        }

        // 결과물 생성
        if (furnaces[2] == null)
        {
            Item result = ItemManager.Instance.NewItem(itemFurnace.result);

            furnaces[2] = result;
        }
        else
        {
            furnaces[2].Stack++;
        }

        for (int i = 0; i < furnaces.Length; i++)
        {
            SetMaterialForAfterInteraction(0, i);
        }   

        furnaceCoroutine = null;
        UpdateExecuteProcess();
    }

    void UpdateExecuteResult()
    {
        /* No Event */
    }
    #endregion

    #region Helper Method
    public void SetMaterialForAfterInteraction(int i, int j)
    {
        if (clientWindow is ClientFurnace furnace)
        {
            furnace.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
        }
    }

    Item[] GetItemByIndex(int i)
    {
        if (i == 0)
        {
            return furnaces;
        }
        else
        {
            Debug.LogError("슬롯에 해당할 수 없는 값이 들어왔습니다.");

            return null;
        }
    }

    Transform[] GetTransformByIndex(int i)
    {
        if (clientWindow is ClientFurnace furnace)
        {
            if (i == 0)
            {
                return furnace.furnaces;
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
            return furnaces[j] != null;
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
        if (clientWindow is ClientFurnace furnace)
        {
            furnace.SetMaterialForTransform(GetTransformByIndex(i)[j], GetItemByIndex(i)[j]);
        }
    }
    #endregion

    #region Valid Method
    protected override bool ValidItemForSlot(int i, int j)
    {
        Item item = dragObject.GetItem;

        if (i == 0 && j == 0)
        {
            return ValidItemForFuel(j, item);
        }
        else if (i == 0 && j == 1)
        {
            return ValidItemForMaterial(j, item);
        }
        else if (i == 1 && j < 2)
        {
            return ValidItemForResult(j, item);
        }
        else
        {
            return false;
        }
    }

    bool ValidItemForFuel(int j, Item item)
    {
        // 1. 연료 슬롯에 속하는지
        // 2. 연료가 맞는지
        if (j == 0)
        {
            return item.Name == "Item_Coal_Raw";
        }

        return false;
    }

    bool ValidItemForMaterial(int j, Item item)
    {
        // 1. 재료 슬롯에 속하는지
        // 2. 아이템 타입이 장비 타입이 아닌지
        if (j == 1)
        {
            return ItemManager.Instance.ValidSlotFurnaceMaterial(item);
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