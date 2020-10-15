using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 sizePanel;

    // Местоположение пальца, когда происходит перемещение
    private Vector2 originalPosition;

    private Vector2 startPositionFinger;

    // Расстояние, на которое сместился палец относительно
    // исходного местоположения
    public Vector2 delta;

    void Start()
    {
        // В момент запуска запомнить исходные
        // координаты
        originalPosition
            = this.GetComponent<RectTransform>().localPosition;
        // Сбросить величину смещения в ноль
        delta = Vector2.zero;
    }
    private void OnDisable()
    {
        delta = Vector2.zero;
    }
    
    // Вызывается, когда начинается перемещение
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPositionFinger = eventData.position;
    }

    // Вызывается в ходе перемещения
    public void OnDrag(PointerEventData eventData)
    {
        var positionNow = eventData.position;
        var offset = startPositionFinger - positionNow;

        // Вычислить смещение от исходной позиции
        var x = Mathf.Clamp((offset.x) / (sizePanel.x / 2f), -1f, 1f);
        var y = Mathf.Clamp((offset.y) / (sizePanel.y / 2f), -1f, 1f);
        delta = new Vector2(-x, -y);
    }

    // Вызывается по окончании перемещения
    public void OnEndDrag(PointerEventData eventData)
    {
        // Сбросить величину смещения в ноль
        delta = Vector2.zero;
    }
}