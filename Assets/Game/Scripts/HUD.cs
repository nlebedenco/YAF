using UnityEngine;
using System.Collections;

public class Hud: MonoBehaviour
{

    public delegate void PauseButtonClickedEvent();

    #region MonoBehaviour

    void Awake()
    {
        GameSystem.RegisterModule(this);
    }

    void OnDestroy()
    {
        PauseButtonClicked = null;
    }

    #endregion

    #region Delegates

    public PauseButtonClickedEvent PauseButtonClicked;

    protected void OnPauseButtonClicked()
    {
        var handler = PauseButtonClicked;
        if (handler != null)
            handler();
    }

    #endregion
}
