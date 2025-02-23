using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private ClientUI ui;
    private ClientPlayer player;
    private Inventory inventory;

    private Dictionary<string, KeyCode> keyDictionary;

    private IPlayerCommand moveCommand;
    private IPlayerCommand jumpCommand;
    private IPlayerCommand attackCommand;
    private IPlayerCommand interactionCommand;
    private IPlayerCommand escapeCommand;
    private IPlayerCommand inventoryCommand;
    private IPlayerCommand quickCommand;
    private IPlayerCommand cameraCommand;

    private IUpperBodyState currentUpperBodyState;
    private ILowerBodyState currentLowerBodyState;

    public ClientUI GetUi { get => ui; }
    public ClientPlayer GetPlayer { get => player; }
    public Inventory GetInventory { get => inventory; }

    private void Start()
    {
        InitializationComponents();
        InitializationSetups();
        InitializationInstances();
    }

    void InitializationComponents()
    {
        ui = FindObjectOfType<ClientUI>();

        player = GetComponent<ClientPlayer>();
        inventory = GetComponent<Inventory>();
    }
    void InitializationSetups()
    {
        keyDictionary = GameManager.Instance.keyBindings;

        SetUpperBodyState(new IdleUpperState());
        SetLowerBodyState(new IdleLowerState());
    }
    void InitializationInstances()
    {
        moveCommand = new MoveCommand();
        jumpCommand = new JumpCommand();
        attackCommand = new AttackCommand();
        interactionCommand = new InteractionCommand();
        escapeCommand = new EscapeCommand();
        inventoryCommand = new InventoryCommand();
        quickCommand = new QuickCommand();
        cameraCommand = new CameraCommand();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        HandleInteraction();
        HandleState();
    }

    private void LateUpdate()
    {
        HandleCamera();
    }

    void HandleMovement()
    {
        if (Input.GetKey(keyDictionary["Forward"]) ||
            Input.GetKey(keyDictionary["Backward"]) ||
            Input.GetKey(keyDictionary["Right"]) ||
            Input.GetKey(keyDictionary["Left"]))
        {
            moveCommand.Execute(this);
        }
        if (Input.GetKey(keyDictionary["Jump"]))
        {
            jumpCommand.Execute(this);
        }
    }

    void HandleInteraction()
    {
        if (Input.GetKey(keyDictionary["LeftMouse"]))
        {
            attackCommand.Execute(this);
        }
        if (Input.GetKeyDown(keyDictionary["Interaction"]))
        {
            interactionCommand.Execute(this);
        }
        if (Input.GetKeyDown(keyDictionary["Escape"]))
        {
            escapeCommand.Execute(this);
        }
        if (Input.GetKeyDown(keyDictionary["Inventory"]))
        {
            inventoryCommand.Execute(this);
        }
        if (Input.GetKeyDown(keyDictionary["Num1"]))
        {
            quickCommand.Execute(this, "Num1");
        }
        if (Input.GetKeyDown(keyDictionary["Num2"]))
        {
            quickCommand.Execute(this, "Num2");
        }
        if (Input.GetKeyDown(keyDictionary["Num3"]))
        {
            quickCommand.Execute(this, "Num3");
        }
        if (Input.GetKeyDown(keyDictionary["Num4"]))
        {
            quickCommand.Execute(this, "Num4");
        }
        if (Input.GetKeyDown(keyDictionary["Num5"]))
        {
            quickCommand.Execute(this, "Num5");
        }
        if (Input.GetKeyDown(keyDictionary["Num6"]))
        {
            quickCommand.Execute(this, "Num6");
        }
        if (Input.GetKeyDown(keyDictionary["Num7"]))
        {
            quickCommand.Execute(this, "Num7");
        }
        if (Input.GetKeyDown(keyDictionary["Num8"]))
        {
            quickCommand.Execute(this, "Num8");
        }
        if (Input.GetKeyDown(keyDictionary["Num9"]))
        {
            quickCommand.Execute(this, "Num9");
        }
    }

    void HandleState()
    {
        currentUpperBodyState?.Update(this);
        currentLowerBodyState?.Update(this);
    }

    void HandleCamera()
    {
        cameraCommand.Execute(this);
    }

    #region State Management
    public void SetUpperBodyState(IUpperBodyState newState)
    {
        currentUpperBodyState = newState;
        currentUpperBodyState.Enter(player);
    }

    public void SetLowerBodyState(ILowerBodyState newState)
    {
        currentLowerBodyState = newState;
        currentLowerBodyState.Enter(player);
    }
    #endregion

    public bool IsRunToggleKeyPressed()
    {
        return
            Input.GetKey(keyDictionary["Run"]);
    }

    public bool IsMoveToggleKeyPressed()
    {
        return
            Input.GetKey(keyDictionary["Forward"]) ||
            Input.GetKey(keyDictionary["Backward"]) ||
            Input.GetKey(keyDictionary["Right"]) ||
            Input.GetKey(keyDictionary["Left"]);
    }

    public bool IsAttackToggleKeyPressed()
    {
        return
            Input.GetKey(keyDictionary["LeftMouse"]);
    }
}
