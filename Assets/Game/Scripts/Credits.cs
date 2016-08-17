using UnityEngine;

public class Credits: MonoBehaviour
{
    public delegate void BackButtonClickedEvent();

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private AutoScroll scroll;

    public void Show()
    {
        canvas.gameObject.SetActive(true);
        scroll.Restart();
    }

    public void Hide()
    {
        canvas.gameObject.SetActive(false);
    }

    #region GUI Event Handlers

    public void btnBack_OnClick()
    {
        OnBackButtonClicked();
    }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        GameSystem.RegisterModule(this);

        Hide();
    }

    void OnDestroy()
    {
        BackButtonClicked = null;
    }

    #endregion

    #region Delegates

    public BackButtonClickedEvent BackButtonClicked;

    protected void OnBackButtonClicked()
    {
        var handler = BackButtonClicked;
        if (handler != null)
            handler();
    }

    #endregion
}
