using UnityEngine;

public abstract class Container : MonoBehaviour
{
    protected OnPointerDragObject dragObject;
    protected object clientWindow;

    public OnPointerDragObject SetDragObject { set => dragObject = value; }
    public object SetClientWindow { set => clientWindow = value; }

    #region Core Method
    public bool OnLeftClickSlot(int i, int j)
    {
        // 드래그 아이템에 아이템이 null
        if (dragObject.GetItem == null)
        {
            // 슬롯에 아이템이 !null
            if (HasItemInSlot(i, j))
            {
                // Drag = null, Slot = !null
                // Container -> Drag
                ProcessOnLeftClickContainerItemToDragItem(i, j);
            }
            // 슬롯에 아이템이 null
            else
            {
                // Drag  = null, Slot Item = null
                /* No Event */
            }
        }
        // 드래그 아이템에 아이템이 !null
        else
        {
            // 슬롯에 아이템이 !null
            if (HasItemInSlot(i, j))
            {
                // Drag = !null, Slot = !null
                // Drag && Container
                ProcessOnLeftClickInteractionItem(i, j);
            }
            // 슬롯에 아이템이 null
            else
            {
                // Drag = !null, Slot = null
                // Drag -> Container
                ProcessOnLeftClickDragItemToContainerItem(i, j);
            }

        }

        return dragObject.SetVisibleDragObject();
    }

    public bool OnRightClickSlot(int i, int j)
    {
        // 드래그 아이템에 아이템이 null
        if (dragObject.GetItem == null)
        {
            // 슬롯에 아이템이 !null
            if (HasItemInSlot(i, j))
            {
                // Drag = null, Slot = !null
                ProcessOnRightClickContainerItemToDragItem(i, j);
            }
            // 슬롯에 아이템이 null
            else
            {
                // Drag  = null, Slot Item = null
                /* No Event */
            }
        }
        // 드래그 아이템에 아이템이 !null
        else
        {
            // 슬롯에 아이템이 !null
            if (HasItemInSlot(i, j))
            {
                // Drag = !null, Slot = !null
                ProcessOnRightClickInteractionItem(i, j);

            }
            // 슬롯에 아이템이 null
            else
            {
                // Drag = !null, Slot = null
                ProcessOnRightClickDragItemToContainerItem(i, j);
            }

        }

        return dragObject.SetVisibleDragObject();
    }

    public void DragObjectReset()
    {
        if (dragObject.GetItem != null)
        {
            dragObject.GetSaveArray[dragObject.GetSaveIndexJ] = dragObject.GetItem;
        }
    }
    #endregion

    #region Helper Method
    protected void DefaultSingleContainerItemToDragItem(object container, int i, int j, Item[] items)
    {
        // 드래그 오브젝트에 아이템을 추가 및 호출된 데이터 저장
        dragObject.NewDragObjectItem(items[j].Name);
        dragObject.SetSaveData(container, items, i, j);

        // 호출된 인덱스에 해당하는 아이템 Stack--
        RemoveItemToItemStack(j, items);
    }

    protected void DefaultSingleDragItemToContainerItem(int j, Item[] items)
    {
        // 새로운 아이템을 생성하여 호출된 인덱스에 저장
        Item newItem = ItemManager.Instance.NewItem(dragObject.GetItem.Name);
        items[j] = newItem;

        // 드래그 아이템 Stack--
        RemoveItemToItemStack();
    }

    protected void DefaultSingleInteractionItem(int i, int j, Item[] items)
    {
        // 호출된 인덱스와 인덱스에 해당하는 아이템 이름이 드래그 오브젝트에 저장된 데이터와 같을 경우
        if (items[j].Name == dragObject.GetItem.Name && (i, j) == (dragObject.GetSaveIndexI, dragObject.GetSaveIndexJ))
        {
            // 드래그 아이템 Stack == Max 일 경우 리턴
            if (dragObject.GetItem.Stack == Define.MAX_STACK) return;

            // 호출된 인덱스에 해당하는 아이템 Stack--
            RemoveItemToItemStack(j, items);
            // 드래그 아이템 Stack++
            AddItemToItemStack();
        }
        // 호출된 인덱스에 해당하는 아이템 이름이 드래그 오브젝트에 저장된 아이템 이름과 같은 경우
        else if (items[j].Name == dragObject.GetItem.Name)
        {
            // 호출된 인덱스에 해당하는 아이템 Stack == Max 일 경우 리턴
            if (items[j].Stack == Define.MAX_STACK) return;

            // 호출된 인덱스에 해당하는 아이템 Stack++
            AddItemToItemStack(j, items);
            // 드래그 아이템 Stack--
            RemoveItemToItemStack();
        }
    }

    protected void DefaultFullContainerItemToDragItem(object container, int i, int j, Item[] items)
    {
        // 호출된 인덱스에 해당하는 아이템을 드래그 오브젝트에 저장
        dragObject.SetItem = items[j];
        // 호출된 데이터 저장
        dragObject.SetSaveData(container, items, i, j);
        // 호출된 인덱스에 해당하는 아이템 제거
        items[j] = null;
    }

    protected void DefaultFullDragItemToContainerItem(int j, Item[] items)
    {
        // 드래그 오브젝트에 저장된 아이템을 호출된 인덱스에 저장
        items[j] = dragObject.GetItem;
        // 드래그 오브젝트에 저장된 아이템 제거
        dragObject.SetItem = null;
    }

    protected void DefaultFullInteractionItem(object container, int i, int j, Item[] items)
    {
        // 호출된 인덱스에 해당하는 아이템 이름과 드래그 오브젝트에 저장된 아이템 이름이 같을 경우
        // 호출된 인덱스에 해당하는 이아템 타입이 장비아이템이 아닐 경우
        if (items[j].Name == dragObject.GetItem.Name && dragObject.GetItem.Type != "Equipment")
        {

            // 호출된 인덱스 번호 저장
            dragObject.SetSaveData(container, items, i, j);

            (dragObject.SetItem, items[j]) = CombineItemStack(dragObject.GetItem, items[j]);
        }
        else
        {
            Item temp = items[j];
            items[j] = dragObject.GetItem;
            dragObject.GetSaveArray[dragObject.GetSaveIndexJ] = temp;
            dragObject.SetItem = null;

            dragObject.SetCurrentContainerMaterial();
        }
    }

    protected bool AddItemToItemStack(int j, Item[] items)
    {
        if (items[j].Stack < Define.MAX_STACK)
        {
            items[j].Stack++;

            return true;
        }

        return false;
    }

    protected bool AddItemToItemStack()
    {
        if (dragObject.GetItem.Stack < Define.MAX_STACK)
        {
            dragObject.GetItem.Stack++;

            return true;
        }

        return false;
    }

    protected void RemoveItemToItemStack(int j, Item[] items)
    {
        items[j].Stack--;

        if (items[j].Stack <= 0)
        {
            items[j] = null;
        }
    }

    protected void RemoveItemToItemStack()
    {
        dragObject.GetItem.Stack--;

        if (dragObject.GetItem.Stack <= 0)
        {
            dragObject.SetItem = null;
        }
    }

    (Item, Item) CombineItemStack(Item i1, Item i2)
    {
        i2.Stack += i1.Stack;

        if (Define.MAX_STACK < i2.Stack)
        {
            i1.Stack = i2.Stack - Define.MAX_STACK;
            i2.Stack = Define.MAX_STACK;
        }
        else
        {
            i1 = null;
        }

        return (i1, i2);
    }
    #endregion

    #region Abstract Method
    // (Left) 슬롯 -> 드래그 오브젝트
    protected abstract void ProcessOnLeftClickContainerItemToDragItem(int i, int j);
    // (Left) 드래그 오브젝트 -> 슬롯
    protected abstract bool ProcessOnLeftClickDragItemToContainerItem(int i, int j);
    // (Left) 슬롯 & 드래그 오브젝트
    protected abstract void ProcessOnLeftClickInteractionItem(int i, int j);
    // (Right) 슬롯 -> 드래그 오브젝트
    protected abstract void ProcessOnRightClickContainerItemToDragItem(int i, int j);
    // (Right) 드래그 오브젝트 -> 슬롯
    protected abstract bool ProcessOnRightClickDragItemToContainerItem(int i, int j);
    // (Right) 슬롯 & 드래그 오브젝트
    protected abstract void ProcessOnRightClickInteractionItem(int i, int j);
    // 슬롯에 아이템이 상호작용 했을 때 해당 슬롯에 대한 이벤트 발생 메서드
    protected abstract void UpdateExecuteByIndex(int i, int j);
    // 슬롯에 아이템을 추가할 수 있는지 확인하는 메서드
    protected abstract bool ValidItemForSlot(int i, int j);
    // 슬롯에 아이템이 존재하는지 확인하는 메서드
    protected abstract bool HasItemInSlot(int i, int j);
    // 열려있는 윈도우에 맞는 슬롯 오프셋 조절 메서드
    protected abstract void SetMaterialSetup(int i, int j);
    #endregion
}