using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어의 머리와 카메라
    public Transform playerHead;
    public Transform playerCamera;

    // 회전 값
    private float xRotation = 0f;
    private float yRotation = 0f;

    // 이동 및 점프 속도
    public float forceWalk = 5f;
    public float forceRun = 7f;
    public float forceJump = 5f;

    // 지면 접촉 여부
    public bool isGrounded;

    // 키 바인딩
    public Dictionary<string, KeyCode> KeyBindings;

    // 레이어 및 컴포넌트
    private int blockLayer;
    public Rigidbody rigidbody;
    public Transform groundCheck;
    public Animator upperAnimator;
    public Animator lowerAnimator;

    // 상태 및 명령
    private IUpperBodyState currentUpperBodyState;
    private ILowerBodyState currentLowerBodyState;
    private IPlayerCommand moveCommand;
    private IPlayerCommand jumpCommand;

    // 상수
    private readonly float GROUND_CHECK_DISTANCE = 1.6f;
    private readonly float MOUSE_SENSITIVITY = 2.0f;

    private void Start()
    {
        InitializationComponents();
        InitializationInstances();
        InitializationSetups();
    }

    private void InitializationComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void InitializationInstances()
    {
        moveCommand = new MoveCommand();
        jumpCommand = new JumpCommand();
    }

    private void InitializationSetups()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        KeyBindings = GameManager.Instance.keyBindings;
        blockLayer = LayerMask.GetMask("Block");

        SetUpperBodyState(new IdleUpperState());
        SetLowerBodyState(new IdleLowerState());
    }

    private void Update()
    {
        UpdateStates();
        CheckGround();
        ExecuteCommands();
        HandleMouseLook();
    }

    private void UpdateStates()
    {
        currentUpperBodyState?.Update(this);
        currentLowerBodyState?.Update(this);
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, GROUND_CHECK_DISTANCE, blockLayer) ? true : false;
    }

    private void ExecuteCommands()
    {
        // 움직임 명령 실행
        moveCommand.Execute(this);

        // 점프 명령 실행
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpCommand.Execute(this);
        }
    }

    private void HandleMouseLook()
    {
        // 마우스 이동 입력
        float mouseX = Input.GetAxis("Mouse X") * MOUSE_SENSITIVITY;
        float mouseY = Input.GetAxis("Mouse Y") * MOUSE_SENSITIVITY;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        // 카메라의 회전
        playerCamera.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // 플레이어 몸체의 회전
        playerHead.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void SetUpperBodyState(IUpperBodyState newState)
    {
        currentUpperBodyState = newState;
        currentUpperBodyState.Enter(this);
    }

    public void SetLowerBodyState(ILowerBodyState newState)
    {
        currentLowerBodyState = newState;
        currentLowerBodyState.Enter(this);
    }

    public bool IsMovementKeyPressed() =>
    Input.GetKey(KeyBindings["Forward"]) ||
    Input.GetKey(KeyBindings["Backward"]) ||
    Input.GetKey(KeyBindings["Right"]) ||
    Input.GetKey(KeyBindings["Left"]);

    public bool IsLeftShiftKeyPressed() =>
        Input.GetKey(KeyCode.LeftShift);

    public bool IsLeftMouseKeyPressed() =>
        Input.GetMouseButton(0);
}