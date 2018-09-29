using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Swipe : MonoBehaviour, IBeginDragHandler, IDragHandler , IEndDragHandler{

    [Serializable] public class UnityEvent_Swipe : UnityEvent<Vector2> { }

    [SerializeField] public UnityEvent_Swipe Swiping;
    [SerializeField] public UnityEvent OnSwipeEnd;
    [SerializeField] public UnityEvent OnSwipeBegin;


    Vector2 lastPosition = Vector2.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        OnSwipeBegin.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - lastPosition;
        lastPosition = eventData.position;

        Swiping.Invoke(direction);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnSwipeEnd.Invoke();
    }
}
