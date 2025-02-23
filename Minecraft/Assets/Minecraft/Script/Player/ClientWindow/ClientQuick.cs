using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ClientQuick : ClientContainer
{
    public GameObject playerQuick;

    public Transform[] quicks;

    public override void InitializationClientData()
    {
        slots = new Transform[][] { quicks };
    }
}