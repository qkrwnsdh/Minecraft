using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ClientFurnace : ClientContainer
{
    public GameObject playerFurnace;

    public Transform[] furnaces;
    public Transform[] inventorys;
    public Transform[] quicks;

    public Slider arrow;
    public Slider time;

    public override void InitializationClientData()
    {
        slots = new Transform[][] { furnaces, inventorys, quicks };
    }
}