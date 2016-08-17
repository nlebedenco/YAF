using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class MouseTrackerPanel: MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
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
            get { return horizontalVirtualAxis; }
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
    private InputControl drag;

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
        drag.Reset();

        tapEnabled = false;
        tap.Reset();
    }
#endif

    void OnEnable()
    {
        drag.Activate();

        if (tapEnabled)
            tap.Activate();
    }

    void OnDisable()
    {
        drag.Deactivate();
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

        if (drag.Pointer == int.MinValue)
        {
            drag.Pointer = data.pointerId;
            drag.Origin = point;
            Cursor.visible = false;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (drag.Pointer == data.pointerId)
        {
            Vector2 size = Vector2.Scale(dragRange.rect.size, dragRange.lossyScale);

            if (drag.UseX)
            {
                float deltaX = data.position.x - drag.Origin.x;
                drag.HorizontalVirtualAxis.Update(Mathf.Clamp(deltaX / size.x, -1, 1));
            }

            if (drag.UseY)
            {
                float deltaY = data.position.y - drag.Origin.y;
                drag.VerticalVirtualAxis.Update(Mathf.Clamp(deltaY / size.y, -1, 1));
            }
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (drag.Pointer == data.pointerId)
        {
            if (drag.UseX)
                drag.HorizontalVirtualAxis.Update(0);

            if (drag.UseY)
                drag.VerticalVirtualAxis.Update(0);

            drag.Pointer = int.MinValue;
            Cursor.visible = true;
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
