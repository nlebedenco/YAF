using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class Touchpad : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Drag 

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Pointer

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    #endregion
}
