using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 클래스의 필드 및 변수
    #region Fields
    // 플레이어 기본 정보
    public int health;
    public int damage = 1;

    // 플레이어의 머리와 카메라
    public Transform playerHead;
    public Transform playerCamera;

    // 키 바인딩
    public Dictionary<string, KeyCode> KeyBindings;

    // 레이어 및 컴포넌트
    public int enemyLayer;
    public int blockLayer;
    public PlayerUI playerUi;
    public Rigidbody rigidbody;
    public Animator upperAnimator;
    public Animator lowerAnimator;
    public Transform[] groundChecks;

    // 코루틴
    public Coroutine attackCoroutine;

    // 상태 및 명령
    private IUpperBodyState currentUpperBodyState;
    private ILowerBodyState currentLowerBodyState;
    private IPlayerCommand visionCommand;
    private IPlayerCommand moveCommand;
    private IPlayerCommand jumpCommand;
    private IPlayerCommand attackCommand;
    private IPlayerCommand interactionCommand;
    #endregion

    // 초기화 관련
    #region Initialization
    private void Start()
    {
        InitializationComponents();
        InitializationSetups();
        InitializationInstances();
    }

    private void InitializationComponents()
    {
        playerUi = GetComponent<PlayerUI>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void InitializationSetups()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        KeyBindings = GameManager.Instance.keyBindings;
        enemyLayer = LayerMask.GetMask("Enemy");
        blockLayer = LayerMask.GetMask("Block");

        SetUpperBodyState(new IdleUpperState());
        SetLowerBodyState(new IdleLowerState());
    }

    private void InitializationInstances()
    {
        visionCommand = new VisionCommand(Define.MOUSE_SENSITIVITY);
        moveCommand = new MoveCommand(rigidbody, transform, Define.SPEED_WALK, Define.SPEED_RUN);
        jumpCommand = new JumpCommand(Define.FORCE_JUMP, rigidbody, groundChecks, Define.GROUND_CHECK_RANGE, blockLayer);
        attackCommand = new AttackCommand(Define.INTERACTION_RANGE, playerCamera);
        interactionCommand = new InteractionCommand(Define.INTERACTION_RANGE, playerCamera);
    }
    #endregion

    // 프레임 업데이트
    #region Update

    // 플레이어 움직임
    #region FixedUpdate
    private void FixedUpdate()
    {
        ExecuteFixedCommands();
    }

    private void ExecuteFixedCommands()
    {
        // 움직임 명령 실행
        moveCommand.Execute(this);
        // 점프 명령 실행
        if (Input.GetKey(KeyCode.Space)) { jumpCommand.Execute(this); }
    }
    #endregion

    // 상태 변화, 지면 접촉 확인
    #region Update
    private void Update()
    {
        ExecuteOtherCommands();
        UpdateStates();
    }

    private void ExecuteOtherCommands()
    {
        // 시야 명령 실행
        visionCommand.Execute(this);
        // 공격 명령 실행
        if (Input.GetMouseButton(0)) { attackCommand.Execute(this); }
        // 설정 창 호출
        if (Input.GetKeyDown(KeyCode.Escape)) { }
        // 인벤토리 창 호출
        if (Input.GetKeyDown(KeyCode.I)) { }
    }

    private void UpdateStates()
    {
        currentUpperBodyState?.Update(this);
        currentLowerBodyState?.Update(this);
    }
    #endregion

    #endregion

    // 상태 관리
    #region State Management
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
    #endregion

    // 플레이어 UI
    #region Player UI

    #endregion

    // 입력 확인
    #region Input Checks
    public bool IsMovementKeyPressed() =>
    Input.GetKey(KeyBindings["Forward"]) ||
    Input.GetKey(KeyBindings["Backward"]) ||
    Input.GetKey(KeyBindings["Right"]) ||
    Input.GetKey(KeyBindings["Left"]);

    public bool IsLeftShiftKeyPressed() =>
        Input.GetKey(KeyCode.LeftShift);

    public bool IsLeftMouseKeyPressed() =>
        Input.GetMouseButton(0);
    #endregion
}