using System.Collections;
using System.Collections.Generic;
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
        KeyBinding();
        InitializationSound();
    }

    public bool isLogin { get; private set; }
    public float bgm { get; private set; }
    public float sfx { get; private set; }

    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();

    #region Binding
    private void KeyBinding()
    {
        keyBindings["Forward"] = KeyCode.W;
        keyBindings["Backward"] = KeyCode.S;
        keyBindings["Right"] = KeyCode.D;
        keyBindings["Left"] = KeyCode.A;
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

    #endregion
}
