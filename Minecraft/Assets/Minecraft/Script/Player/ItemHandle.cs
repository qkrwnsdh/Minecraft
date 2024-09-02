using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHandle : IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        //List<RaycastResult> results = PerformRaycast(eventData);

        //foreach (RaycastResult result in results)
        //{
        //    if (result.gameObject.layer == layerItem)
        //    {
        //        currentSlot = result.gameObject;

        //        ItemSwap(result.gameObject.transform, dragItem.transform);

        //        SetDraggedPosition(eventData);
        //        SetActiveDrag(true);

        //        break;
        //    }
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (isDragging)
        //{
        //    SetDraggedPosition(eventData);
        //}
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if (isDragging)
        //{
        //    List<RaycastResult> results = PerformRaycast(eventData);

        //    foreach (RaycastResult result in results)
        //    {
        //        if (result.gameObject.layer == layerItem)
        //        {
        //            ItemSwap(result.gameObject.transform, dragItem.transform);

        //            break;
        //        }
        //    }

        //    ItemSwap(currentSlot.gameObject.transform, dragItem.transform);
        //    SetActiveDrag(false);
        //}
    }
}
