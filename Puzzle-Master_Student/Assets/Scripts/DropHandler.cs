using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
        if(DragHandler.itemBeingDragged!=null && DragHandler.itemBeingDragged!=transform){
            Vector3 tmpPos=DragHandler.startPosition;
            DragHandler.startPosition=transform.position;
            transform.position=tmpPos;
        }
    }

}
