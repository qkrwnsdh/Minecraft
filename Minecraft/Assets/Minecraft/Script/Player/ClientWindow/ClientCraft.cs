using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ClientCraft : ClientContainer
{
    public GameObject playerCraft;

    public Transform[] crafts;
    public Transform[] inventorys;
    public Transform[] quicks;

    public override void InitializationClientData()
    {
        slots = new Transform[][] { crafts, inventorys, quicks };
    }
}
