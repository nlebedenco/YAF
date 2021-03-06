﻿using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class DualTouchPanel: MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Options for which axes to use
    public enum AxisOption
    {
        Both = 0,       // Use both
        OnlyHorizontal, // Only horizontal
        OnlyVertical    // Only vertical
    }

    [Serializable]
    public struct InputControl
    {
        public AxisOption AxesToUse;

        public string HorizontalAxisName;

        public string VerticalAxisName;

        public bool UseX
        {
            get { return (AxesToUse == AxisOption.Both || AxesToUse == AxisOption.OnlyHorizontal); }
        }

        public bool UseY
        {
            get { return (AxesToUse == AxisOption.Both || AxesToUse == AxisOption.OnlyVertical); }
        }

        [NonSerialized]
        public int Pointer;

        [NonSerialized]
        public Vector2 Origin;

        private CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;
        public CrossPlatformInputManager.VirtualAxis HorizontalVirtualAxis
        {
            get {  return horizontalVirtualAxis; }
        }

        private CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;
        public CrossPlatformInputManager.VirtualAxis VerticalVirtualAxis
        {
            get { return verticalVirtualAxis; }
        }

        private void createHorizontalAxis()
        {
            horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(HorizontalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(horizontalVirtualAxis);
        }

        private void releaseHorizontalAxis()
        {
            if (horizontalVirtualAxis != null)
            {
                horizontalVirtualAxis.Remove();
                horizontalVirtualAxis = null;
            }
        }

        private void createVerticalAxis()
        {
            verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VerticalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(verticalVirtualAxis);
        }

        private void releaseVerticalAxis()
        {
            if (verticalVirtualAxis != null)
            {
                verticalVirtualAxis.Remove();
                verticalVirtualAxis = null;
            }
        }

        public void Reset()
        {
            AxesToUse = AxisOption.Both;
            HorizontalAxisName = null;
            VerticalAxisName = null;

            releaseHorizontalAxis();
            releaseVerticalAxis();
        }

        public void Activate()
        {
            Pointer = int.MinValue;
            Origin = Vector2.zero;

            if (UseX)
                createHorizontalAxis();

            if (UseY)
                createVerticalAxis();
        }

        public void Deactivate()
        {
            releaseHorizontalAxis();
            releaseVerticalAxis();
        }
    }
   
    [SerializeField]
    private InputControl dragLeft;
   
    [SerializeField]
    private InputControl dragRight;

    [SerializeField]
    private bool tapEnabled;

    [SerializeField]
    private InputControl tap;

    [SerializeField]
    private RectTransform dragRange;

    private bool clearTap = false;

    #region Monobehaviour

#if UNITY_EDITOR
    void Reset()
    {
        dragLeft.Reset();
        dragRight.Reset();

        tapEnabled = false;
        tap.Reset();
    }
#endif

    void OnEnable()
    {
        dragLeft.Activate();

        dragRight.Activate();

        if (tapEnabled)
            tap.Activate();
    }

    void OnDisable()
    {
        dragLeft.Deactivate();
        dragRight.Deactivate();
        tap.Deactivate();
    }

    void LateUpdate()
    {
        if (clearTap)
        {
            clearTap = false;

            if (tap.UseX)
                tap.HorizontalVirtualAxis.Update(0);

            if (tap.UseY)
                tap.VerticalVirtualAxis.Update(0);
        }
    }

    #endregion

    #region Drag 

    public void OnBeginDrag(PointerEventData data)
    {
        RectTransform rectTransform = (RectTransform)transform;
        Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);

        Vector2 point;         
        if (tap.Pointer == data.pointerId)
        {
            tap.Pointer = int.MinValue;
            point = tap.Origin;
        }
        else
        {
            point = data.position;
        }

        float m = size.x / 2;
        float x = point.x;
        if (x < m)
        {
            if (dragLeft.Pointer == int.MinValue)
            {
                dragLeft.Pointer = data.pointerId;
                dragLeft.Origin = point;
            }
        }
        else
        {
            if (dragRight.Pointer == int.MinValue)
            {
                dragRight.Pointer = data.pointerId;
                dragRight.Origin = point;
            }
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (dragLeft.Pointer == data.pointerId)
        {
            Vector2 size = Vector2.Scale(dragRange.rect.size, dragRange.lossyScale);

            if (dragLeft.UseX)
            {
                float deltaX = data.position.x - dragLeft.Origin.x;
                dragLeft.HorizontalVirtualAxis.Update(Mathf.Clamp(deltaX / size.x, -1, 1));
            }

            if (dragLeft.UseY)
            {
                float deltaY = data.position.y - dragLeft.Origin.y;
                dragLeft.VerticalVirtualAxis.Update(Mathf.Clamp(deltaY / size.y, -1, 1));
            }
        }
        else if (dragRight.Pointer == data.pointerId)
        {
            Vector2 size = Vector2.Scale(dragRange.rect.size, dragRange.lossyScale);

            if (dragRight.UseX)
            {
                float deltaX = data.position.x - dragRight.Origin.x;
                dragRight.HorizontalVirtualAxis.Update(Mathf.Clamp(deltaX / size.x, -1, 1));
            }

            if (dragRight.UseY)
            {
                float deltaY = data.position.y - dragRight.Origin.y;
                dragRight.VerticalVirtualAxis.Update(Mathf.Clamp(deltaY / size.y, -1, 1));
            }
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (dragLeft.Pointer == data.pointerId)
        {
            if (dragLeft.UseX)
                dragLeft.HorizontalVirtualAxis.Update(0);

            if (dragLeft.UseY)
                dragLeft.VerticalVirtualAxis.Update(0);

            dragLeft.Pointer = int.MinValue;
        }
        else if (dragRight.Pointer == data.pointerId)
        {
            if (dragRight.UseX)
                dragRight.HorizontalVirtualAxis.Update(0);

            if (dragRight.UseY)
                dragRight.VerticalVirtualAxis.Update(0);

            dragRight.Pointer = int.MinValue;
        }
    }

    #endregion

    #region Pointer

    public void OnPointerDown(PointerEventData data)
    {
        if (tapEnabled)
        {
            tap.Pointer = data.pointerId;
            tap.Origin = data.position;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (tap.Pointer == data.pointerId)
        {
            if (tap.UseX)
                tap.HorizontalVirtualAxis.Update(data.position.x);

            if (tap.UseY)
                tap.VerticalVirtualAxis.Update(data.position.y);

            tap.Pointer = int.MinValue;

            clearTap = true;
        }
    }

    #endregion
}
