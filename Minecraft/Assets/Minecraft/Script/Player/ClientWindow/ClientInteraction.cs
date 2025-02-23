using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ClientInteraction
{
    public GameObject clientInteraction;

    public TextMeshProUGUI text;
    public Slider slider;

    private Coroutine interactionCoroutine;

    public Coroutine InteractionCoroutine
    {
        get => interactionCoroutine;
        set => interactionCoroutine = value;
    }

    public void Interaction(Block block)
    {
        text.text = block.transform.name;
        slider.value = (float)(block.currentHealth / (float)block.blockInfo.health);
    }
}