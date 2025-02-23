using UnityEngine;

public interface IItemHandlerStrategy
{
    public void SetClientData(object data, ClientData clientData);
    public void SetContainerData(GameObject dragObject, ClientQuick quickComponent);
    (Transform[][], Item[][]) InstacneItemGetData();
    void SetTransformMaterial(Transform[][] transforms, Item[][] items);
    bool OnLeftClickSlot(int i, int j);
    bool OnRightClickSlot(int i, int j);
    void DragObjectReset();
}

public class InventoryHandlerStrategy : IItemHandlerStrategy
{
    private readonly ClientInventory clientInventory;
    private Inventory inventoryData;
    private ClientData clientData;

    public InventoryHandlerStrategy(ClientInventory clientInventory)
    {
        this.clientInventory = clientInventory;
    }

    public void SetClientData(object data, ClientData clientData)
    {
        if (data is Inventory inventoryData)
        {
            this.inventoryData = inventoryData;
        }

        this.clientData = clientData;
    }

    public void SetContainerData(GameObject dragObject, ClientQuick quickComponent)
    {
        OnPointerDragObject onPointerDragObject = dragObject.GetComponent<OnPointerDragObject>();

        inventoryData.SetDragObject = onPointerDragObject;
        inventoryData.SetClientWindow = clientInventory;
        clientData.SetDragObject = onPointerDragObject;
        clientData.SetClientWindow = clientInventory;
        clientData.SetQuick = quickComponent;
    }

    public (Transform[][], Item[][]) InstacneItemGetData()
    {
        Transform[][] trans = clientInventory.GetData();
        Item[][] items = new Item[][]
        {
            inventoryData.GetEquipments,
            inventoryData.GetCrafts,
            clientData.GetInventorys,
            clientData.GetQuicks
        };

        return (trans, items);
    }

    public void SetTransformMaterial(Transform[][] transforms, Item[][] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].Length; j++)
            {
                clientInventory.SetMaterialForTransform(transforms[i][j], items[i][j]);
            }
        }
    }

    public bool OnLeftClickSlot(int i, int j)
    {
        switch (i)
        {
            case 0: return inventoryData.OnLeftClickSlot(i, j);
            case 1: return inventoryData.OnLeftClickSlot(i, j);
            case 2: return clientData.OnLeftClickSlot(i, j);
            case 3: return clientData.OnLeftClickSlot(i, j);
            default: return false;
        }
    }

    public bool OnRightClickSlot(int i, int j)
    {
        switch (i)
        {
            case 0: return inventoryData.OnRightClickSlot(i, j);
            case 1: return inventoryData.OnRightClickSlot(i, j);
            case 2: return clientData.OnRightClickSlot(i, j);
            case 3: return clientData.OnRightClickSlot(i, j);
            default: return false;
        }
    }

    public void DragObjectReset()
    {
        inventoryData.DragObjectReset();
    }
}

public class FurnaceHandlerStrategy : IItemHandlerStrategy
{
    private readonly ClientFurnace clientFurnace;
    private Furnace furnaceData;
    private ClientData clientData;

    public FurnaceHandlerStrategy(ClientFurnace clientFurnace)
    {
        this.clientFurnace = clientFurnace;
    }

    public void SetClientData(object data, ClientData clientData)
    {
        if (data is Furnace furnaceData)
        {
            this.furnaceData = furnaceData;
        }

        this.clientData = clientData;
    }

    public void SetContainerData(GameObject dragObject, ClientQuick quickComponent)
    {
        OnPointerDragObject onPointerDragObject = dragObject.GetComponent<OnPointerDragObject>();

        furnaceData.SetDragObject = onPointerDragObject;
        furnaceData.SetClientWindow = clientFurnace;
        clientData.SetDragObject = onPointerDragObject;
        clientData.SetClientWindow = clientFurnace;
        clientData.SetQuick = quickComponent;
    }

    public (Transform[][], Item[][]) InstacneItemGetData()
    {
        Transform[][] trans = clientFurnace.GetData();
        Item[][] items = new Item[][]
        {
            furnaceData.GetFurnaces,
            clientData.GetInventorys,
            clientData.GetQuicks
        };

        return (trans, items);
    }

    public void SetTransformMaterial(Transform[][] transforms, Item[][] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].Length; j++)
            {
                clientFurnace.SetMaterialForTransform(transforms[i][j], items[i][j]);
            }
        }
    }

    public bool OnLeftClickSlot(int i, int j)
    {
        switch (i)
        {
            case 0: return furnaceData.OnLeftClickSlot(i, j);
            case 1: return clientData.OnLeftClickSlot(i, j);
            case 2: return clientData.OnLeftClickSlot(i, j);
            default: return false;
        }
    }

    public bool OnRightClickSlot(int i, int j)
    {
        switch (i)
        {
            case 0: return furnaceData.OnRightClickSlot(i, j);
            case 1: return clientData.OnRightClickSlot(i, j);
            case 2: return clientData.OnRightClickSlot(i, j);
            default: return false;
        }
    }

    public void DragObjectReset()
    {
        furnaceData.DragObjectReset();
    }
}

public class CraftHandlerStrategy : IItemHandlerStrategy
{
    private readonly ClientCraft clientCraft;
    private Craft craftData;
    private ClientData clientData;

    public CraftHandlerStrategy(ClientCraft clientCraft)
    {
        this.clientCraft = clientCraft;
    }

    public void SetClientData(object data, ClientData clientData)
    {
        if (data is Craft craftData)
        {
            this.craftData = craftData;
        }

        this.clientData = clientData;
    }

    public void SetContainerData(GameObject dragObject, ClientQuick quickComponent)
    {
        OnPointerDragObject onPointerDragObject = dragObject.GetComponent<OnPointerDragObject>();

        craftData.SetDragObject = onPointerDragObject;
        craftData.SetClientWindow = clientCraft;
        clientData.SetDragObject = onPointerDragObject;
        clientData.SetClientWindow = clientCraft;
        clientData.SetQuick = quickComponent;
    }

    public (Transform[][], Item[][]) InstacneItemGetData()
    {
        Transform[][] trans = clientCraft.GetData();
        Item[][] items = new Item[][]
        {
            craftData.GetCrafts,
            clientData.GetInventorys,
            clientData.GetQuicks
        };

        return (trans, items);
    }

    public void SetTransformMaterial(Transform[][] transforms, Item[][] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].Length; j++)
            {
                clientCraft.SetMaterialForTransform(transforms[i][j], items[i][j]);
            }
        }
    }

    public bool OnLeftClickSlot(int i, int j)
    {
        switch (i)
        {
            case 0: return craftData.OnLeftClickSlot(i, j);
            case 1: return clientData.OnLeftClickSlot(i, j);
            case 2: return clientData.OnLeftClickSlot(i, j);
            default: return false;
        }
    }

    public bool OnRightClickSlot(int i, int j)
    {
        switch (i)
        {
            case 0: return craftData.OnRightClickSlot(i, j);
            case 1: return clientData.OnRightClickSlot(i, j);
            case 2: return clientData.OnRightClickSlot(i, j);
            default: return false;
        }
    }

    public void DragObjectReset()
    {
        craftData.DragObjectReset();
    }
}

public class ChestHandlerStrategy : IItemHandlerStrategy
{
    private readonly ClientChest clientChest;
    private Chest chestData;
    private ClientData clientData;

    public ChestHandlerStrategy(ClientChest clientChest)
    {
        this.clientChest = clientChest;
    }

    public void SetClientData(object data, ClientData clientData)
    {
        if (data is Chest chestData)
        {
            this.chestData = chestData;
        }

        this.clientData = clientData;
    }

    public void SetContainerData(GameObject dragObject, ClientQuick quickComponent)
    {
        OnPointerDragObject onPointerDragObject = dragObject.GetComponent<OnPointerDragObject>();

        chestData.SetDragObject = onPointerDragObject;
        chestData.SetClientWindow = clientChest;
        clientData.SetDragObject = onPointerDragObject;
        clientData.SetClientWindow = clientChest;
        clientData.SetQuick = quickComponent;
    }

    public (Transform[][], Item[][]) InstacneItemGetData()
    {
        Transform[][] trans = clientChest.GetData();
        Item[][] items = new Item[][]
        {
            chestData.GetChests,
            clientData.GetInventorys,
            clientData.GetQuicks
        };

        return (trans, items);
    }

    public void SetTransformMaterial(Transform[][] transforms, Item[][] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].Length; j++)
            {
                clientChest.SetMaterialForTransform(transforms[i][j], items[i][j]);
            }
        }
    }

    public bool OnLeftClickSlot(int i, int j)
    {
        switch (i)
        {
            case 0: return chestData.OnLeftClickSlot(i, j);
            case 1: return clientData.OnLeftClickSlot(i, j);
            case 2: return clientData.OnLeftClickSlot(i, j);
            default: return false;
        }
    }

    public bool OnRightClickSlot(int i, int j)
    {
        switch (i)
        {
            case 0: return chestData.OnRightClickSlot(i, j);
            case 1: return clientData.OnRightClickSlot(i, j);
            case 2: return clientData.OnRightClickSlot(i, j);
            default: return false;
        }
    }

    public void DragObjectReset()
    {
        chestData.DragObjectReset();
    }
}