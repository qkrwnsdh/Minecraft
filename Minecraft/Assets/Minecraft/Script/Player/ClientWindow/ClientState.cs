using System;
using UnityEngine;

[Serializable]
public class ClientState
{
    public GameObject playerState;

    public Transform[] shields;
    public Transform[] healths;
    public Transform[] foods;

    public void SetStatus(Transform[] elements, int value)
    {
        int setValue = Mathf.Max(0, value / 2);

        for (int i = 0; i < elements.Length; i++)
        {
            bool isHalf = i == setValue && value % 2 == 1;
            bool isFull = i < setValue;

            elements[i].GetChild(0).gameObject.SetActive(isHalf);
            elements[i].GetChild(1).gameObject.SetActive(isFull);
        }
    }
}