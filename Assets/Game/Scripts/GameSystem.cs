using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections;

public class GameSystem: MonoBehaviour
{
    [Flags]
    public enum Modules
    {
        None = 0,
        Hud = 1,
        Menu = 2,
        Shop = 4,
        Credits = 8,
        All = Hud | Menu | Shop | Credits
    }

    #region Static

    private static GameSystem instance;

    public static void RegisterModule(Hud module)
    {
        instance.registerModule(module);
    }

    public static void RegisterModule(Menu module)
    {
        instance.registerModule(module);
    }

    public static void RegisterModule(Shop module)
    {
        instance.registerModule(module);
    }

    public static void RegisterModule(Credits module)
    {
        instance.registerModule(module);
    }

    public static void RegisterLevel(Level level)
    {
        instance.registerLevel(level);
    }

    #endregion

    #region Scene Names

    [SerializeField]
    private string hudSceneName = string.Empty;

    [SerializeField]
    private string menuSceneName = string.Empty;

    [SerializeField]
    private string shopSceneName = string.Empty;

    [SerializeField]
    private string creditsSceneName = string.Empty;

    [SerializeField]
    private string[] levelSceneName = new string[0];

    #endregion

    [SerializeField]
    private UnityEngine.UI.Graphic splashGraphic;

    [SerializeField]
    private UnityEngine.UI.Graphic fadeGraphic;

    [SerializeField]
    private float fadeDuration = 1.0f;

    private Hud Hud;
    private Menu Menu;
    private Shop Shop;
    private Credits Credits;
    private Level Level;

    private bool loaded = false;
    private Modules loading = Modules.All;
    private void setLoaded(Modules module)
    {
        loading &= ~module;
        if (!loaded && loading == Modules.None)
        {
            loaded = true;
            OnModulesLoaded();
        }
    }

    private void registerModule(Hud module)
    {
        Hud = module;
        module.PauseButtonClicked += hud_PauseButtonClicked;
        setLoaded(Modules.Hud);
    }

    private void registerModule(Menu module)
    {
        Menu = module;

        module.RetryButtonClicked += menu_RetryButtonClicked;
        module.ShopButtonClicked += menu_ShopButtonClicked;
        module.PlayButtonClicked += menu_PlayButtonClicked;
        module.ResumeButtonClicked += menu_ResumeButtonClicked;
        module.CreditsButtonClicked += menu_CreditsButtonClicked;

        setLoaded(Modules.Menu);
    }

    private void registerModule(Shop module)
    {
        Shop = module;

        setLoaded(Modules.Shop);
    }

    private void registerModule(Credits module)
    {
        Credits = module;

        module.BackButtonClicked += credits_BackButtonClicked;

        setLoaded(Modules.Credits);
    }

    private void registerLevel(Level level)
    {
        Level = level;
        OnLevelLoaded();
    }

    #region HUD Handlers

    private void hud_PauseButtonClicked()
    {

    }

    #endregion

    #region Menu Handlers

    private void menu_RetryButtonClicked()
    {

    }

    private void menu_ShopButtonClicked()
    {

    }

    private void menu_PlayButtonClicked()
    {

    }

    private void menu_ResumeButtonClicked()
    {

    }

    private void menu_CreditsButtonClicked()
    {
        Menu.Hide();
        Credits.Show();
    }

    #endregion

    #region Shop Handler

    #endregion

    #region Credits Handler

    private void credits_BackButtonClicked()
    {
        Menu.Show();
        Credits.Hide();
    }

    #endregion

    private void LoadModules()
    {
        SceneManager.LoadSceneAsync(hudSceneName, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(shopSceneName, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(creditsSceneName, LoadSceneMode.Additive);
    }

    private void LoadLevel(int index)
    {
        SceneManager.LoadSceneAsync(levelSceneName[index], LoadSceneMode.Additive);
    }

    private void OnModulesLoaded()
    {
        FadeOut();
    }

    private void OnLevelLoaded()
    {

    }

    private void OnFadeInComplete()
    {

    }

    private void OnFadeOutComplete()
    {
        Menu.Show();
        DestroySplash();
        FadeIn();
    }

    private void DestroySplash()
    {
        var go = splashGraphic.gameObject;
        Destroy(go);
    }

    private void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        fadeGraphic.gameObject.SetActive(true);
        fadeGraphic.CrossFadeAlpha(1, fadeDuration, true);
        yield return new WaitForSecondsRealtime(fadeDuration);
        OnFadeOutComplete();
    }

    private IEnumerator FadeInCoroutine()
    {
        fadeGraphic.CrossFadeAlpha(0, fadeDuration, true);
        yield return new WaitForSecondsRealtime(fadeDuration);
        fadeGraphic.gameObject.SetActive(false);
        OnFadeInComplete();
    }

    #region MonoBehaviour

#if UNITY_EDITOR
    void OnValidate()
    {
        fadeDuration = Mathf.Clamp(fadeDuration, 0, float.PositiveInfinity);
    }
#endif

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogFormat("GameSystem already exists. Duplicate ('{0}') will be destroyed.", gameObject.name);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Input.simulateMouseWithTouches = false;

        splashGraphic.gameObject.SetActive(true);

        fadeGraphic.CrossFadeAlpha(0, 0, true);

        // TODO: Load save settings

        LoadModules();
    }

    #endregion
}
