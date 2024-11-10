using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform knob;
    public RectTransform containerRect;
    public RectTransform bgRect;

    [SerializeField]
    private Vector2 pointerDownPosition = Vector2.zero;

    [SerializeField]
    private Vector2 currentPointerPosition = Vector2.zero;

    [SerializeField]
    public Vector2 value;


    public bool isDown = false;
    public bool isDragging = false;

    public bool isUp = false;

    private void Awake()
    {
        containerRect = containerRect ? containerRect : GetComponent<RectTransform>();
        bgRect = bgRect ? bgRect : GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        knob.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUp) isUp = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        isDragging = true;
        isUp = false;


        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position,
            eventData.pressEventCamera, out pointerDownPosition);

        value = pointerDownPosition.normalized;

        knob.localPosition = pointerDownPosition;
        // knob.localPosition = eventData.position;
        // value = knob.anchoredPosition.normalized;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDown = false;
        isUp = false;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position,
            eventData.pressEventCamera, out currentPointerPosition);

        value = currentPointerPosition.normalized;

        knob.localPosition = currentPointerPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        isDragging = false;
        isUp = true;

        knob.localPosition = Vector3.zero;
        value = Vector2.zero;
        
    }
}