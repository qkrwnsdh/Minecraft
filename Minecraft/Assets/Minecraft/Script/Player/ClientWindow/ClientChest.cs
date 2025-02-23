using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ClientChest : ClientContainer
{
    public GameObject playerChest;

    public Transform[] chests;
    public Transform[] inventorys;
    public Transform[] quicks;

    public override void InitializationClientData()
    {
        slots = new Transform[][] { chests, inventorys, quicks };
    }
}
