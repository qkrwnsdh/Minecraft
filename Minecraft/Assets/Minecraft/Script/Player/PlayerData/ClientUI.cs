using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Window { SETTING, INVENTORY, FURNACE, CRAFT, CHEST }

public class ClientUI : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;

    [SerializeField] private ClientSetting setting;
    [SerializeField] private ClientInventory inventory;
    [SerializeField] private ClientFurnace furnace;
    [SerializeField] private ClientChest chest;
    [SerializeField] private ClientCraft craft;
    [SerializeField] private ClientInteraction interaction;
    [SerializeField] private ClientState state;
    [SerializeField] private ClientQuick quick;

    public ClientData data;

    private OnPointerInSlot onPointer;
    private Window? currentWindow;
    private Dictionary<Window?, GameObject> windowDictionary;
    private Dictionary<Window?, object> windowDataDictionary;

    public Window? CurrentWindow { get => currentWindow; }

    #region Start and Initialization
    void Start()
    {
        InitializationComponent();
        InitializationInstances();
        InitializationSetups();
    }

    void InitializationComponent()
    {
        data = GetComponent<ClientData>();
        onPointer = GetComponent<OnPointerInSlot>();
    }

    void InitializationInstances()
    {
        windowDictionary = new Dictionary<Window?, GameObject>
        {
            { Window.SETTING, setting.playerSetting },
            { Window.INVENTORY, inventory.playerInventory },
            { Window.FURNACE, furnace.playerFurnace },
            { Window.CRAFT, craft.playerCraft },
            { Window.CHEST, chest.playerChest }
        };
        windowDataDictionary = new Dictionary<Window?, object>
        {
            { Window.INVENTORY, inventory },
            { Window.FURNACE, furnace },
            { Window.CRAFT, craft },
            { Window.CHEST, chest }
        };
    }

    void InitializationSetups()
    {
        InitializationClientData();
        InitializationOnPointerStrategy();
        InitializationWindowActive();

        CursorController(false);
    }

    void InitializationClientData()
    {
        inventory.InitializationClientData();
        inventory.InitializationInstanceMaterial();

        furnace.InitializationClientData();
        furnace.InitializationInstanceMaterial();

        craft.InitializationClientData();
        craft.InitializationInstanceMaterial();

        chest.InitializationClientData();
        chest.InitializationInstanceMaterial();

        quick.InitializationClientData();
        quick.InitializationInstanceMaterial();

        quick.UpdateSlotFull(new Item[][] { data.GetQuicks });

        onPointer.SetClientDataComponent = data;
        onPointer.SetClientQuickComponent = quick;
    }

    void InitializationOnPointerStrategy()
    {
        onPointer.SetStrategyInstance(inventory);
        onPointer.SetStrategyInstance(furnace);
        onPointer.SetStrategyInstance(craft);
        onPointer.SetStrategyInstance(chest);
    }

    void InitializationWindowActive()
    {
        crosshair.SetActive(true);
        state.playerState.SetActive(true);
        quick.playerQuick.SetActive(true);
    }
    #endregion

    #region Toggle Manager
    public void UpdateQuickSlot()
    {
        quick.UpdateSlotFull(new Item[][] { data.GetQuicks });
    }

    public void ToggleManager(Window window, object getData = null)
    {
        if (currentWindow == null)
        {
            OnPointerInit(window, getData);
            WindowUpdate(window, getData);
            WindowController(window, true);
        }
        else
        {
            if (window == Window.SETTING)
            {
                WindowController(currentWindow, false);
                onPointer.SetClearData();
            }
            if (window == Window.INVENTORY && currentWindow == Window.INVENTORY)
            {
                WindowController(window, false);
                onPointer.SetClearData();
            }
        }
    }

    void OnPointerInit(Window? window, object getData)
    {
        if (window == Window.SETTING || window == null) return;

        onPointer.SetClientWin = windowDataDictionary[window];
        onPointer.SetInitCompoent = getData;
        onPointer.SetCurrentData();
    }

    void WindowUpdate(Window window, object getData)
    {
        Item[][] items = GetSlotItems(window, getData);

        switch (window)
        {
            case Window.INVENTORY:
                inventory.UpdateSlotFull(items);
                break;
            case Window.FURNACE:
                furnace.UpdateSlotFull(items);
                break;
            case Window.CRAFT:
                craft.UpdateSlotFull(items);
                break;
            case Window.CHEST:
                chest.UpdateSlotFull(items);
                break;
        }
    }

    void WindowController(Window? window, bool isTrue)
    {
        ObjectController(isTrue);
        CursorController(isTrue);

        currentWindow = isTrue ? window : null;
        windowDictionary[window].SetActive(isTrue);
    }

    void ObjectController(bool isTrue)
    {
        crosshair.SetActive(!isTrue);
    }

    void CursorController(bool isTrue)
    {
        Cursor.lockState = isTrue ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isTrue;
    }

    Item[][] GetSlotItems(Window? window, object getData)
    {
        switch (window)
        {
            case Window.INVENTORY when getData is Inventory inventory:
                return new Item[][] { inventory.GetEquipments, inventory.GetCrafts, data.GetInventorys, data.GetQuicks };
            case Window.FURNACE when getData is Furnace furnace:
                return new Item[][] { furnace.GetFurnaces, data.GetInventorys, data.GetQuicks };
            case Window.CRAFT when getData is Craft craft:
                return new Item[][] { craft.GetCrafts, data.GetInventorys, data.GetQuicks };
            case Window.CHEST when getData is Chest chest:
                return new Item[][] { chest.GetChests, data.GetInventorys, data.GetQuicks };
            default:
                return null;
        }
    }
    #endregion

    #region Toggle Setting
    public void ButtonConfirm() => setting.Confirm();
    public void ButtonCancle() => setting.Cancle();
    #endregion

    #region Toggle Interaction
    public void ToggleInteraction(Block block)
    {
        interaction.Interaction(block);

        if (interaction.InteractionCoroutine != null)
        {
            StopCoroutine(interaction.InteractionCoroutine);
        }

        interaction.clientInteraction.SetActive(true);
        interaction.InteractionCoroutine = StartCoroutine(InteractionCoroutine());
    }

    IEnumerator InteractionCoroutine()
    {
        yield return new WaitForSeconds(Define.INTERACTION_INTERVAL);

        interaction.clientInteraction.SetActive(false);

        interaction.InteractionCoroutine = null;
    }
    #endregion

    #region Toggle State
    public void SetShields(int shield) => state.SetStatus(state.shields, shield);
    public void SetHealths(int health) => state.SetStatus(state.healths, health);
    public void SetFoods(int food) => state.SetStatus(state.foods, food);
    #endregion

    #region Toggle Quick
    public void ToggleItem(ClientPlayer player, string num, Block block = null, Vector3? hitDirection = null) => data.ToggleItem(player, num, block, hitDirection);
    #endregion
}