using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class Thumbstick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
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
    private float fadeInDuration = 0.1f;

    [SerializeField]
    private float fadeOutDuration = 0.1f;

    [SerializeField]
    private float backgroundAlpha = 0.25f;

    [SerializeField]
    private float foregroundAlpha = 0.5f;

    [SerializeField]
    private RectTransform grip;

    [SerializeField]
    private RectTransform limits;

    [SerializeField]
    private UnityEngine.UI.Image background;

    [SerializeField]
    private UnityEngine.UI.Image foreground;

    // References to the axes in the cross platform input
    private CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;
    private CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;      

    private bool useX { get { return (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal); } } 

    private bool useY {  get { return (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical); } }

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

        grip.anchoredPosition = Vector2.zero;

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
        foreground.CrossFadeAlpha(foregroundAlpha, fadeInDuration, false);

        UpdateGrip(data.position.x, data.position.y);
    }

    #endregion

    #region IPointerClickHandler

    public void OnPointerClick(PointerEventData data)
    {
        Debug.LogFormat("Click on: {0}", data.position);
    }
    
    #endregion

    #region IDragHandler

    public void OnDrag(PointerEventData data)
    {
        UpdateGrip(data.position.x, data.position.y);
    }

    #endregion  

    private void UpdateGrip(float x, float y)
    {
        Vector2 size = Vector2.Scale(limits.rect.size, new Vector2(limits.lossyScale.x, limits.lossyScale.y));

        Vector2 delta = Vector2.zero;
        
        if (useX)
        {
            delta.x = x - limits.position.x;
            horizontalVirtualAxis.Update(Mathf.Clamp(delta.x / size.x, -1, 1));
        }

        if (useY)
        {
            delta.y = y - limits.position.y;
            verticalVirtualAxis.Update(Mathf.Clamp(delta.y / size.y, -1, 1));
        }

        float w = limits.rect.width;
        float h = limits.rect.height;
        float radius = Mathf.Sqrt(w * w + h * h) / 2;
        grip.anchoredPosition = Vector2.ClampMagnitude(
            Vector2.Scale(delta, new Vector2(1 / limits.lossyScale.x, 1 / limits.lossyScale.y)),
            radius);
    }
}
