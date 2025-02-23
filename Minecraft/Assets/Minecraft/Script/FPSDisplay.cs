using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;  // 프레임 시간 추적
    public int fontSize = 20;        // 폰트 크기 설정
    private GUIStyle guiStyle = new GUIStyle();  // GUI 스타일

    void Update()
    {
        // 프레임 시간 계산 (델타 타임)
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // FPS 계산
        float fps = 1.0f / deltaTime;

        // 화면에 FPS 표시
        guiStyle.fontSize = fontSize;
        guiStyle.normal.textColor = Color.white;

        // 화면 좌측 상단에 FPS 표시
        GUI.Label(new Rect(10, 10, 100, 50), $"FPS: {fps:0}", guiStyle);
    }
}
