using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public static Vector3 startPosition;
    Transform startParent;
    
    Slicer slicerOfGame;
    private void Start() {
        slicerOfGame=GameObject.Find("Builder").GetComponent<Slicer>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slicerOfGame.isCompleted) return;

        itemBeingDragged=gameObject;
        startPosition=transform.position;
        startParent=transform.parent;
        transform.SetParent(startParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (slicerOfGame.isCompleted) return;

        transform.position=eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (slicerOfGame.isCompleted) return;

        transform.position=startPosition;
        transform.SetParent(startParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        itemBeingDragged=null;
        slicerOfGame.checkIfPuzzleCompleted();
        slicerOfGame.saveTilePositionsProperly();
    }
}
