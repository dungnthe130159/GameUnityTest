using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobilJoystick : MonoBehaviour , IPointerUpHandler , IDragHandler , IPointerDownHandler
{
    [SerializeField] RectTransform joystickTransform;

    [SerializeField] 
    private float dragThershold = 0.5f;

    [SerializeField]
    private int dragMovementDistance = 30;

    [SerializeField]
    private int dragOffsetDistance = 100;

    public event Action<Vector2> OnMove;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickTransform, eventData.position,
            null, out offset);
        offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance;
        joystickTransform.anchoredPosition = offset * dragMovementDistance;

        Vector2 inputVector = CalculateMovementInput(offset);
        OnMove?.Invoke(inputVector);
    }

    private Vector2 CalculateMovementInput(Vector2 offset)
    {
        float x = Mathf.Abs(offset.x) > dragThershold ? offset.x : 0;
        float y = Mathf.Abs(offset.y) > dragThershold ? offset.y : 0;
        return new Vector2(x, y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickTransform.anchoredPosition = Vector2.zero;
        OnMove?.Invoke(Vector2.zero);  
    }

    private void Awake()
    {
        joystickTransform = (RectTransform)transform;
}
}
