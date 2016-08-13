using System;
using System.Collections;

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MouseTracker : MonoBehaviour
{
    // Options for which axes to use
    public enum AxisOption
    {
        Both,           // Use both
        OnlyHorizontal, // Only horizontal
        OnlyVertical    // Only vertical
    }

    [SerializeField]
    private AxisOption axesToUse = AxisOption.Both;      // The options for the axes that the still will use

    [SerializeField]
    private string horizontalAxisName = "Horizontal";

    [SerializeField]
    private string verticalAxisName = "Vertical";

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float MovementRange = 100;

    // References to the axes in the cross platform input
    private CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;
    private CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;

    private bool useX { get { return (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal); } }

    private bool useY { get { return (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical); } }

    #region Monobehaviour

    void OnEnable()
    {
        // create new axes based on axes to use
        if (useX)
        {
            horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(horizontalVirtualAxis);
        }

        if (useY)
        {
            verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(verticalVirtualAxis);
        }
    }

    void OnDisable()
    {
        // remove the joysticks from the cross platform input
        if (useX)
            horizontalVirtualAxis.Remove();

        if (useY)
            verticalVirtualAxis.Remove();
    }

    private float lx;
    private float ly;
    private bool tracking;

    void Update()
    {
        if (target != null)
        {
            bool leftButtonDown = Input.GetMouseButton(0);

            if (leftButtonDown)
            {
                tracking = true;

                Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position);
                if (screenPosition.x > 0 && screenPosition.y > 0 && screenPosition.x < Screen.width && screenPosition.y < Screen.height)
                {
                    screenPosition.z = 0;
                    Vector3 delta = Input.mousePosition - screenPosition;
                    
                    if (useX)
                    {
                        float x = Mathf.Clamp(delta.x / MovementRange, -1, 1);
                        if (lx != x)
                        {
                            lx = x;
                            horizontalVirtualAxis.Update(lx);
                        }
                    }

                    if (useY)
                    {
                        float y = Mathf.Clamp(delta.y / MovementRange, -1, 1);
                        if (ly != y)
                        {
                            ly = y;
                            verticalVirtualAxis.Update(ly);
                        }
                    }
                }
            }
            else
            {
                if (tracking)
                {
                    lx = 0;
                    ly = 0;

                    if (useX)
                        horizontalVirtualAxis.Update(lx);

                    if (useY)
                        verticalVirtualAxis.Update(ly);

                    tracking = false;
                }
            }
        }
    }

    #endregion
}
