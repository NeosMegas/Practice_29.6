using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    [Header("Swipe")]
    public float swipeMinWidth = 100f;
    public float swipeMaxHeigth = 50f;
    private Vector2 swipeStartPosition;
    private Vector2 swipeEndPosition;
    private Vector2 swipeLastDeltaPositionSign;
    [Header("Zoom")]
    public float zoomMinAmountPercent = 10f;
    private Vector2[] zoomStartPosition = new Vector2[2];
    private Vector2[] zoomEndPosition = new Vector2[2];

    void Update()
    {
        int touchCount = Input.touchCount;
        switch (touchCount)
        {
            case 1:
                Swipe();
                break;
            case 2:
                Zoom();
                break;
        }
    }

    void Swipe()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
            swipeStartPosition = new Vector2(touch.position.x, touch.position.y);

        if (touch.phase == TouchPhase.Moved && Mathf.Sign(touch.deltaPosition.x) != 0 && Mathf.Sign(touch.deltaPosition.x) != swipeLastDeltaPositionSign.x)
        {
            swipeStartPosition = new Vector2(touch.position.x, touch.position.y);
            swipeLastDeltaPositionSign = new Vector2(Mathf.Sign(touch.deltaPosition.x), Mathf.Sign(touch.deltaPosition.y));
            //Debug.Log($"Направление свайпа изменено на {lastDeltaPositionSign.x}");
        }

        if (touch.phase == TouchPhase.Ended)
        {
            swipeEndPosition = new Vector2(touch.position.x, touch.position.y);
            Vector2 deltaPosition = swipeEndPosition - swipeStartPosition;
            if (Mathf.Abs(deltaPosition.x) >= swipeMinWidth && Mathf.Abs(deltaPosition.y) <= swipeMaxHeigth)
            {
                if (deltaPosition.x > 0)
                    Debug.Log($"Свайп вправо: {swipeEndPosition} - {swipeStartPosition} = {deltaPosition}");
                else if (deltaPosition.x < 0)
                    Debug.Log($"Свайп влево: {swipeEndPosition} - {swipeStartPosition} = {deltaPosition}");
            }
        }
    }

    private void Zoom()
    {
        Touch[] touch = new Touch[2] { Input.GetTouch(0), Input.GetTouch(1) };
        if (touch[0].phase == TouchPhase.Began)
        {
            zoomStartPosition[0] = new Vector2(touch[0].position.x, touch[0].position.y);
            //Debug.Log($"zoomStartPosition[0] = {zoomStartPosition[0]}");
        }
        if (touch[1].phase == TouchPhase.Began)
        {
            zoomStartPosition[1] = new Vector2(touch[1].position.x, touch[1].position.y);
            //Debug.Log($"zoomStartPosition[1] = {zoomStartPosition[1]}");
        }

        if (touch[0].phase == TouchPhase.Ended && touch[1].phase == TouchPhase.Ended)
        {
            zoomEndPosition[0] = new Vector2(touch[0].position.x, touch[0].position.y);
            //Debug.Log($"zoomEndPosition[0] = {zoomEndPosition[0]}");
            zoomEndPosition[1] = new Vector2(touch[1].position.x, touch[1].position.y);
            //Debug.Log($"zoomEndPosition[1] = {zoomEndPosition[1]}");
            float zoomStartDistance = Vector2.Distance(zoomStartPosition[0], zoomStartPosition[1]);
            //Debug.Log($"zoomStartDistance = {zoomStartDistance}");
            float zoomEndDistance = Vector2.Distance(zoomEndPosition[0], zoomEndPosition[1]);
            //Debug.Log($"zoomEndDistance = {zoomEndDistance}");
            Vector2[] zoomDirection = new Vector2[2];
            zoomDirection[0] = zoomEndPosition[0] - zoomStartPosition[0];
            //Debug.Log($"zoomDirection[0] = {zoomDirection[0]}");
            zoomDirection[1] = zoomEndPosition[1] - zoomStartPosition[1];
            //Debug.Log($"zoomDirection[1] = {zoomDirection[1]}");
            float zoomDirectionsAngle = Vector2.Angle(zoomDirection[0], zoomDirection[1]);
            //Debug.Log($"zoomDirectionsAngle = {zoomDirectionsAngle}");
            float zoomAmount = zoomEndDistance - zoomStartDistance;
            //Debug.Log($"zoomAmount = {zoomAmount}");
            float zoomAmountPercent = zoomEndDistance != 0 ? (zoomEndDistance - zoomStartDistance) / zoomEndDistance * 100f : 0;
            if (zoomAmount > 0 && zoomAmountPercent >= zoomMinAmountPercent && zoomDirectionsAngle >= 90)
                Debug.Log($"Жест увеличение: {zoomAmountPercent:F0}%");
        }
    }

}
