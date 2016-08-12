using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class DirectionalPad : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    // Options for which axes to use
    public enum AxisOption
    {
        Both,           // Use both
        OnlyHorizontal, // Only horizontal
        OnlyVertical    // Only vertical
    }

    [SerializeField]
    private AxisOption AxesToUse = AxisOption.Both;      // The options for the axes that the still will use

    [SerializeField]
    private string HorizontalAxisName = "Horizontal";    // The name given to the horizontal axis for the cross platform input

    [SerializeField]
    private string VerticalAxisName = "Vertical";        // The name given to the vertical axis for the cross platform input

    [SerializeField]
    private float fadeInDuration;

    [SerializeField]
    private float fadeOutDuration;

    [SerializeField]
    private float backgroundAlpha;

    [SerializeField]
    private RectTransform thumbstick;

    [SerializeField]
    private RectTransform limits;

    [SerializeField]
    private UnityEngine.UI.Image background;

    [SerializeField]
    private UnityEngine.UI.Image foreground;

    CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;    // Reference to the axis in the cross platform input
    CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;      // Reference to the axis in the cross platform input

    private bool useX { get { return (AxesToUse == AxisOption.Both || AxesToUse == AxisOption.OnlyHorizontal); } } 

    private bool useY {  get { return (AxesToUse == AxisOption.Both || AxesToUse == AxisOption.OnlyVertical); } }

    #region Monobehaviour

    void OnEnable()
    {
        // create new axes based on axes to use
        if (useX)
        {
            horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(HorizontalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(horizontalVirtualAxis);
        }

        if (useY)
        {
            verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VerticalAxisName);
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

    void Awake()
    {
        background.CrossFadeAlpha(0, 0, false);
    }

    #endregion

    #region IPointerUpHandler

    public void OnPointerUp(PointerEventData data)
    {
        background.CrossFadeAlpha(0, fadeOutDuration, false);
        foreground.CrossFadeAlpha(1, fadeOutDuration, false);

        thumbstick.anchoredPosition = Vector2.zero;

        if (useX)
            horizontalVirtualAxis.Update(0);

        if (useY)
            verticalVirtualAxis.Update(0);
    }

    #endregion

    #region IPointerDownHandler

    public void OnPointerDown(PointerEventData data)
    {
        background.CrossFadeAlpha(backgroundAlpha, fadeInDuration, false);
        foreground.CrossFadeAlpha(0.8f, fadeInDuration, false);

        //UpdateThumbstick(data.position.x, data.position.y);
    }

    #endregion

    #region IDragHandler

    public void OnDrag(PointerEventData data)
    {
        UpdateThumbstick(data.position.x, data.position.y);
    }

    #endregion  

    private void UpdateThumbstick(float x, float y)
    {
        float w = limits.rect.width;
        float h = limits.rect.height;
        float radius = Mathf.Sqrt(w * w + h * h) / 2;

        float deltaX = x - limits.position.x;
        float deltaY = y - limits.position.y;

        if (useX)
            horizontalVirtualAxis.Update(Mathf.Clamp(deltaX / w, -1, 1));

        if (useY)
            verticalVirtualAxis.Update(Mathf.Clamp(deltaY / h, -1, 1));

        thumbstick.anchoredPosition = Vector2.ClampMagnitude(
            new Vector2(
                useX ? deltaX : 0,
                useY ? deltaY : 0
            ),
            radius);
    }
}
