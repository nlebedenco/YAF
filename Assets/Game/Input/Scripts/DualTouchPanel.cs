using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;


[System.Serializable]
public class TapEvent : UnityEvent<Vector2>
{

}


public class DualTouchPanel: MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Options for which axes to use
    public enum AxisOption
    {
        Both,           // Use both
        OnlyHorizontal, // Only horizontal
        OnlyVertical    // Only vertical
    }


    [Serializable]
    public struct Side
    {
        public RectTransform limits;
        public string horizontalAxisName;
        public string verticalAxisName;
    }

    [SerializeField]
    private AxisOption axesToUse = AxisOption.Both;      // The options for the axes that the still will use

    [SerializeField]
    private Side leftSide;

    [SerializeField]
    private Side rightSide;

    [SerializeField]
    private bool tappingEnabled = true;

    [SerializeField]
    private TapEvent OnTap;

    private int tapPointer = int.MinValue;
    private Vector2 tapOrigin;

    private bool useX { get { return (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal); } }

    private bool useY { get { return (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical); } }


    private int leftDragPointer = int.MinValue;
    private int rightDragPointer = int.MinValue;

    private Vector2 leftOrigin;
    private Vector2 rightOrigin;

    // References to the axes in the cross platform input
    private CrossPlatformInputManager.VirtualAxis leftHorizontalVirtualAxis;
    private CrossPlatformInputManager.VirtualAxis leftVerticalVirtualAxis;

    private CrossPlatformInputManager.VirtualAxis rightHorizontalVirtualAxis;
    private CrossPlatformInputManager.VirtualAxis rightVerticalVirtualAxis;

    #region Monobehaviour

    void OnEnable()
    {
        // create new axes based on axes to use
        if (useX)
        {
            leftHorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(leftSide.horizontalAxisName);
            rightHorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(rightSide.horizontalAxisName);

            CrossPlatformInputManager.RegisterVirtualAxis(leftHorizontalVirtualAxis);
            CrossPlatformInputManager.RegisterVirtualAxis(rightHorizontalVirtualAxis);
        }

        if (useY)
        {
            leftVerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(leftSide.verticalAxisName);
            rightVerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(rightSide.verticalAxisName);

            CrossPlatformInputManager.RegisterVirtualAxis(leftVerticalVirtualAxis);
            CrossPlatformInputManager.RegisterVirtualAxis(rightVerticalVirtualAxis);
        }
    }

    void OnDisable()
    {
        // remove the joysticks from the cross platform input
        if (useX)
        {
            leftHorizontalVirtualAxis.Remove();
            rightHorizontalVirtualAxis.Remove();
        }

        if (useY)
        {
            leftVerticalVirtualAxis.Remove();
            rightVerticalVirtualAxis.Remove();
        }
    }

    #endregion

    #region Drag 

    public void OnBeginDrag(PointerEventData data)
    {
        RectTransform rectTransform = (RectTransform)transform;
        Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);

        Vector2 point;         
        if (tapPointer == data.pointerId)
        {
            tapPointer = int.MinValue;
            point = tapOrigin;
        }
        else
        {
            point = data.position;
        }

        float m = size.x / 2;
        float x = point.x;
        if (x < m)
        {
            if (leftDragPointer == int.MinValue)
            {
                leftDragPointer = data.pointerId;
                leftOrigin = point;
            }
        }
        else
        {
            if (rightDragPointer == int.MinValue)
            {
                rightDragPointer = data.pointerId;
                rightOrigin = point;
            }
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (leftDragPointer == data.pointerId)
        {
            RectTransform limits = leftSide.limits;
            Vector2 size = Vector2.Scale(limits.rect.size, limits.lossyScale);

            if (useX)
            {
                float deltaX = data.position.x - leftOrigin.x;
                leftHorizontalVirtualAxis.Update(Mathf.Clamp(deltaX / size.x, -1, 1));
            }

            if (useY)
            {
                float deltaY = data.position.y - leftOrigin.y;
                leftVerticalVirtualAxis.Update(Mathf.Clamp(deltaY / size.y, -1, 1));
            }
        }
        else if (rightDragPointer == data.pointerId)
        {
            RectTransform limits = rightSide.limits;
            Vector2 size = Vector2.Scale(limits.rect.size, limits.lossyScale);

            if (useX)
            {
                float deltaX = data.position.x - rightOrigin.x;
                rightHorizontalVirtualAxis.Update(Mathf.Clamp(deltaX / size.x, -1, 1));
            }

            if (useY)
            {
                float deltaY = data.position.y - rightOrigin.y;
                rightVerticalVirtualAxis.Update(Mathf.Clamp(deltaY / size.y, -1, 1));
            }
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (leftDragPointer == data.pointerId)
        {
            if (useX)
                leftHorizontalVirtualAxis.Update(0);

            if (useY)
                leftVerticalVirtualAxis.Update(0);

            leftDragPointer = int.MinValue;
        }
        else if (rightDragPointer == data.pointerId)
        {
            if (useX)
                rightHorizontalVirtualAxis.Update(0);

            if (useY)
                rightVerticalVirtualAxis.Update(0);

            rightDragPointer = int.MinValue;
        }
    }

    #endregion

    #region Pointer

    public void OnPointerDown(PointerEventData data)
    {
        if (tappingEnabled)
        {
            tapPointer = data.pointerId;
            tapOrigin = data.position;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (tappingEnabled && tapPointer == data.pointerId)
        {
            tapPointer = int.MinValue;
            OnTap.Invoke(data.position);
        }
    }

    #endregion
}
