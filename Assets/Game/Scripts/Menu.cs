using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public delegate void RetryButtonClickedEvent();
    public delegate void ShopButtonClickedEvent();
    public delegate void PlayButtonClickedEvent();
    public delegate void ResumeButtonClickedEvent();
    public delegate void CreditsButtonClickedEvent();


    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private RectTransform menu;

    [SerializeField]
    private RectTransform options;

    public void Show()
    {
        canvas.gameObject.SetActive(true);
        camera.gameObject.SetActive(true);

        ShowMenu();
    }

    public void Hide()
    {
        menu.gameObject.SetActive(false);
        options.gameObject.SetActive(false);

        camera.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    private void ShowMenu()
    {
        menu.gameObject.SetActive(true);
        options.gameObject.SetActive(false);
    }

    private void ShowOptions()
    {
        menu.gameObject.SetActive(false);
        options.gameObject.SetActive(true);
    }

    #region MonoBehaviour

    void Awake()
    {
        GameSystem.RegisterModule(this);

        Hide();
    }

    void OnDestroy()
    {
        RetryButtonClicked = null;
        ShopButtonClicked = null;
        PlayButtonClicked = null;
        ResumeButtonClicked = null;
        CreditsButtonClicked = null;
    }

    #endregion

    #region GUI Event Handlers

    public void btnOptions_OnClick()
    {
        ShowOptions();
    }

    public void btnRetry_OnClick()
    {
        OnRetryButtonClicked();
    }

    public void btnShop_OnClick()
    {
        OnShopButtonClicked();
    }

    public void btnPlay_OnClick()
    {
        OnPlayButtonClicked();
    }

    public void btnResume_OnClick()
    {
        OnResumeButtonClicked();
    }

    public void btnBack_OnClick()
    {
        ShowMenu();
    }

    public void btnCredits_OnClick()
    {
        OnCreditsButtonClicked();
    }

    public void sliderMusicVolume_OnValueChanged(float value)
    {
        GameSystem.MusicVolume = value;    
    }

    public void sliderSfxVolume_OnValueChanged(float value)
    {
        GameSystem.SfxVolume = value;
    }

    public void sliderVoicesVolume_OnValueChanged(float value)
    {
        GameSystem.VoicesVolume = value;
    }

    #endregion

    #region Delegates

    public RetryButtonClickedEvent RetryButtonClicked;
    public ShopButtonClickedEvent ShopButtonClicked;
    public PlayButtonClickedEvent PlayButtonClicked;
    public ResumeButtonClickedEvent ResumeButtonClicked;
    public CreditsButtonClickedEvent CreditsButtonClicked;

    protected void OnRetryButtonClicked()
    {
        var handler = RetryButtonClicked;
        if (handler != null)
            handler();
    }

    protected void OnShopButtonClicked()
    {
        var handler = ShopButtonClicked;
        if (handler != null)
            handler();
    }

    protected void OnPlayButtonClicked()
    {
        var handler = PlayButtonClicked;
        if (handler != null)
            handler();
    }

    protected void OnResumeButtonClicked()
    {
        var handler = ResumeButtonClicked;
        if (handler != null)
            handler();
    }

    protected void OnCreditsButtonClicked()
    {
        var handler = CreditsButtonClicked;
        if (handler != null)
            handler();
    }

    #endregion
}
