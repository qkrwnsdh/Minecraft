using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnPointerInSlot : MonoBehaviour, IPointerClickHandler
{
    #region Field
    [SerializeField]
    private GameObject dragObject;            // 드래그하는 아이템을 시각적으로 보여주기 위한 오브젝트

    private object clientWin;               // 클라이언트 창의 컴포넌트
    private object initData;                // 클라이언트 창의 상대 데이터
    private ClientData clientData;          // 클라이언트 창의 플레이어 데이터
    private ClientQuick clientQuick;        // 클라이언트 창의 퀵 슬롯 데이터 

    private Transform[][] currentTrans;     // 클라이언트의 창의 Transform 배열
    private Item[][] currentItems;          // 클라이언트의 창의 Item 배열

    private GraphicRaycaster graphcRay;     // UI Raycasting을 위한 GraphicRaycaster 컴포넌트
    private int slotLayer;                  // 슬롯 레이어의 레이어 번호
    private bool isDragging;                // 드래그 상태

    private IItemHandlerStrategy initInventroyStrategy;
    private IItemHandlerStrategy initFurnaceStrategy;
    private IItemHandlerStrategy initCraftStrategy;
    private IItemHandlerStrategy initChestStrategy;

    private IItemHandlerStrategy currentStrategy;
    #endregion

    #region Initialization
    void Start()
    {
        InitializationComponent();
        InitializationSetups();
    }

    void InitializationComponent()
    {
        graphcRay = GetComponent<GraphicRaycaster>();
    }

    void InitializationSetups()
    {
        slotLayer = LayerMask.NameToLayer("Slot");
        isDragging = false;
        Image image = dragObject.transform.GetChild(0).GetComponent<Image>();
        ItemManager.Instance.NewItemMaterial(image);
    }
    #endregion

    #region Update
    private void Update()
    {
        if (isDragging)
        {
            SetDraggingPosition();
        }
    }

    void SetDraggingPosition()
    {
        Vector2 mousePos = Input.mousePosition;

        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragObject.GetComponent<RectTransform>().parent.GetComponent<RectTransform>(),
            mousePos,
            Camera.main,
            out anchoredPosition
        );

        dragObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }
    #endregion

    #region Setting Feild Data
    public object SetClientWin { set => clientWin = value; }
    public object SetInitCompoent { set => initData = value; }
    public ClientData SetClientDataComponent { set => clientData = value; }
    public ClientQuick SetClientQuickComponent { set => clientQuick = value; }

    public void SetStrategyInstance(object clientData)
    {
        switch (clientData)
        {
            case ClientInventory clientInventory:
                initInventroyStrategy = new InventoryHandlerStrategy(clientInventory);
                break;
            case ClientFurnace clientFurnace:
                initFurnaceStrategy = new FurnaceHandlerStrategy(clientFurnace);
                break;
            case ClientCraft clientCraft:
                initCraftStrategy = new CraftHandlerStrategy(clientCraft);
                break;
            case ClientChest clientChest:
                initChestStrategy = new ChestHandlerStrategy(clientChest);
                break;
        }
    }

    public void SetCurrentData()
    {
        if (clientWin == null || initData == null || clientData == null)
        {
            Debug.LogError($"Componet: {typeof(ClientUI)} 에서 초기화가 재대로 이루어지지 않았습니다.");

            return;
        }

        switch (initData)
        {
            case Inventory dataInventory:
                initInventroyStrategy.SetClientData(dataInventory, clientData);
                initInventroyStrategy.SetContainerData(dragObject, clientQuick);
                currentStrategy = initInventroyStrategy;
                break;
            case Furnace dataFurnace:
                initFurnaceStrategy.SetClientData(dataFurnace, clientData);
                initFurnaceStrategy.SetContainerData(dragObject, clientQuick);
                currentStrategy = initFurnaceStrategy;
                break;
            case Craft dataCraft:
                initCraftStrategy.SetClientData(dataCraft, clientData);
                initCraftStrategy.SetContainerData(dragObject, clientQuick);
                currentStrategy = initCraftStrategy;
                break;
            case Chest dataChest:
                initChestStrategy.SetClientData(dataChest, clientData);
                initChestStrategy.SetContainerData(dragObject, clientQuick);
                currentStrategy = initChestStrategy;
                break;
        }

        (currentTrans, currentItems) = InstanceItemGetData();
        SetTransformMaterial(currentTrans, currentItems);
    }

    public void SetClearData()
    {
        clientWin = null;
        initData = null;
    }
    #endregion

    #region Event Handlers
    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> results = PerformRaycast(eventData);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandlePointerClick(results, eventData, true);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            HandlePointerClick(results, eventData, false);
        }
    }
    #endregion

    #region Helper Methods
    void HandlePointerClick(List<RaycastResult> results, PointerEventData eventData, bool isLeft)
    {
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == slotLayer)
            {
                (int i, int j) = FindPointerTransformIndex(result);

                if (isLeft)
                {
                    isDragging = OnLeftClickSlot(i, j);
                }
                else
                {
                    isDragging = OnRightClickSlot(i, j);
                }
            }
            else
            {
                DragObjectReset();
            }

            PerformRaycast(eventData);
        }
    }

    #endregion

    #region Utility Methods
    List<RaycastResult> PerformRaycast(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        graphcRay.Raycast(eventData, results);
        return results;
    }

    (int, int) FindPointerTransformIndex(RaycastResult targetResult)
    {
        Transform targetTransform = targetResult.gameObject.transform;

        for (int i = 0; i < currentTrans.Length; i++)
        {
            for (int j = 0; j < currentTrans[i].Length; j++)
            {
                if (currentTrans[i][j] == targetTransform)
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1);
    }
    #endregion

    #region Modify Item Data
    (Transform[][], Item[][]) InstanceItemGetData()
    {
        return currentStrategy.InstacneItemGetData();
    }

    void SetTransformMaterial(Transform[][] transforms, Item[][] items)
    {
        currentStrategy.SetTransformMaterial(transforms, items);
    }

    bool OnLeftClickSlot(int i, int j)
    {
        return currentStrategy.OnLeftClickSlot(i, j);
    }

    bool OnRightClickSlot(int i, int j)
    {
        return currentStrategy.OnRightClickSlot(i, j);
    }

    void DragObjectReset()
    {
        currentStrategy.DragObjectReset();
    }
    #endregion
}