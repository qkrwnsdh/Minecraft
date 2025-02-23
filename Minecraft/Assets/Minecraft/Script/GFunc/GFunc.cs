using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class GFunc
{
    // 슬롯의 첫번째 자식의 Image 컴포넌트 두번째 자식을 TextMeshProUGUI 컴포넌트를 반환
    public static (Image, TextMeshProUGUI) GetSlotComponent(Transform slot)
        => (slot.GetChild(0).GetComponent<Image>(), slot.GetChild(1).GetComponent<TextMeshProUGUI>());
}
