using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            AwakeSetup();
        }

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    private void AwakeSetup()
    {
        InitializationKeyBindings();
        InitializationSound();
    }

    public bool isLogin { get; private set; }
    public float bgm { get; private set; }
    public float sfx { get; private set; }

    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();

    #region Binding
    private void InitializationKeyBindings()
    {
        keyBindings["Forward"] = KeyCode.W;
        keyBindings["Backward"] = KeyCode.S;
        keyBindings["Right"] = KeyCode.D;
        keyBindings["Left"] = KeyCode.A;
        keyBindings["Run"] = KeyCode.LeftShift;
        keyBindings["Jump"] = KeyCode.Space;
        keyBindings["LeftMouse"] = KeyCode.Mouse0;
        keyBindings["Interaction"] = KeyCode.E;
        keyBindings["Escape"] = KeyCode.Escape;
        keyBindings["Inventory"] = KeyCode.I;
        keyBindings["Num1"] = KeyCode.Alpha1;
        keyBindings["Num2"] = KeyCode.Alpha2;
        keyBindings["Num3"] = KeyCode.Alpha3;
        keyBindings["Num4"] = KeyCode.Alpha4;
        keyBindings["Num5"] = KeyCode.Alpha5;
        keyBindings["Num6"] = KeyCode.Alpha6;
        keyBindings["Num7"] = KeyCode.Alpha7;
        keyBindings["Num8"] = KeyCode.Alpha8;
        keyBindings["Num9"] = KeyCode.Alpha9;
    }

    private void InitializationSound()
    {
        bgm = 0.5f;
        sfx = 0.5f;
    }

    public void SetKey(KeyCode forward, KeyCode backward, KeyCode right, KeyCode left)
    {

        keyBindings["Forward"] = forward;
        keyBindings["Backward"] = backward;
        keyBindings["Right"] = right;
        keyBindings["Left"] = left;
    }

    public void SetSound(float bgm, float sfx)
    {
        this.bgm = bgm;
        this.sfx = sfx;
    }

    //public void StartHost(Action<Transform> callback)
    //{
    //    NetworkManager networkManager = FindObjectOfType<NetworkManager>();

    //    // 서버 시작
    //    networkManager.StartHost();

    //    // 서버가 시작되면 OnServerStarted 콜백 등록
    //    networkManager.OnServerStarted += () =>
    //    {
    //        if (networkManager.IsHost)
    //        {
    //            // 클라이언트가 연결되었을 때 호출되는 메서드 등록
    //            networkManager.OnClientConnectedCallback += OnClientConnected;
    //        }
    //    };

    //    // 클라이언트 연결 시 실행될 메서드
    //    void OnClientConnected(ulong clientId)
    //    {
    //        // 호스트의 로컬 클라이언트 ID와 일치하는지 확인
    //        if (clientId == networkManager.LocalClientId)
    //        {
    //            NetworkObject hostPlayerNetworkObject = networkManager.SpawnManager.GetPlayerNetworkObject(clientId);
    //            if (hostPlayerNetworkObject != null)
    //            {
    //                Transform hostPlayer = hostPlayerNetworkObject.transform;

    //                // 호스트 플레이어 오브젝트가 준비되었을 때 콜백 호출
    //                callback?.Invoke(hostPlayer);
    //            }
    //            else
    //            {
    //                Debug.LogError("Host player object not found.");
    //            }

    //            // 이벤트 해제하여 중복 호출 방지
    //            networkManager.OnClientConnectedCallback -= OnClientConnected;
    //        }
    //    }
    //}
    #endregion
}