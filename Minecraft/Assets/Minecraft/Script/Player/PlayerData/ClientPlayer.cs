using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ClientPlayer : MonoBehaviour
{
    // 클래스의 필드 및 변수
    #region Fields
    // 플레이어 기본 정보
    private int _Shild;
    private int _Health;
    private int _Food;
    private int _Damage = 1;

    // 플레이어의 머리와 카메라 및 지면 체크 지점
    public Transform playerHead;
    public Transform playerCamera;
    public Transform[] groundChecks;

    // 컴포넌트
    public Rigidbody rb;
    #endregion

    public Animator upperAnimator;
    public Animator lowerAnimator;

    // 초기화 관련
    #region Initialization
    private void Start()
    {
        InitializationComponents();
    }

    private void InitializationComponents()
    {
        rb = GetComponent<Rigidbody>();
    }
    #endregion

    public int Shild { get => _Shild; set => _Shild = value; }
    public int Health { get => _Health; set => _Health = value; }
    public int Food { get => _Food; set => _Food = value; }
    public int Damage { get => _Damage; set => _Damage = value; }

    public void SetShild(int value)
    {
        Shild = value;
    }

    public void SetDamage(int value)
    {
        Damage = value;
    }
}