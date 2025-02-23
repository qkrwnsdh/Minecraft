using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ClientSetting
{
    public GameObject playerSetting;

    public void Confirm()
    {
        Debug.Log("확인");

        // 확인 버튼을 눌렀을때 일어날 일
    }

    public void Cancle()
    {
        Debug.Log("취소");

        // 취소 버튼을 눌렀을때 일어날 일
    }
}