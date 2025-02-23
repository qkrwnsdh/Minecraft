using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ClientInventory : ClientContainer
{
    public GameObject playerInventory;

    public Transform[] equipments;
    public Transform[] crafts;
    public Transform[] inventorys;
    public Transform[] quicks;

    public override void InitializationClientData()
    {
        slots = new Transform[][] { equipments, crafts, inventorys, quicks };
    }
}