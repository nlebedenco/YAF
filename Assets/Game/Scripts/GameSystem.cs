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
        Loading = 1,
        Hud = 2,
        Menu = 4,
        Credits = 8,
        All = Loading | Hud | Menu | Credits
    }

    #region Static

    private static GameSystem instance;

    public static void RegisterModule(Loading module)
    {
        instance.registerModule(module);
    }

    public static void RegisterModule(Hud module)
    {
        instance.registerModule(module);
    }

    public static void RegisterModule(Menu module)
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
    private string loadingSceneName = string.Empty;

    [SerializeField]
    private string hudSceneName = string.Empty;

    [SerializeField]
    private string menuSceneName = string.Empty;

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

    private Loading Loading;
    private Hud Hud;
    private Menu Menu;
    private Credits Credits;
    private Level Level;

    private int levelIndex = 0;

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

    private void registerModule(Loading module)
    {
        Loading = module;
        setLoaded(Modules.Loading);
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
        module.PlayButtonClicked += menu_PlayButtonClicked;
        module.ResumeButtonClicked += menu_ResumeButtonClicked;
        module.CreditsButtonClicked += menu_CreditsButtonClicked;

        setLoaded(Modules.Menu);
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

        level.LevelCompleted += level_LevelCompleted;
        level.LevelFailed += level_LevelFailed;
        level.ScoreChanged += level_ScoreChanged;
        level.HealthChanged += level_HealthChanged;

        OnLevelLoaded();
    }

    #region HUD Handlers

    private void hud_PauseButtonClicked()
    {
        Level.Pause();
        Hud.Hide();
        Menu.Show();
    }

    #endregion

    #region Menu Handlers

    private void menu_RetryButtonClicked()
    {
        Menu.Hide();
        LoadLevel(levelIndex);
    }

    private void menu_PlayButtonClicked()
    {
        PlayLevel();
    }

    private void menu_ResumeButtonClicked()
    {
        PlayLevel();
    }

    private void menu_CreditsButtonClicked()
    {
        Menu.Hide();
        Credits.Show();
    }

    #endregion

    #region Credits Handler

    private void credits_BackButtonClicked()
    {
        Menu.Show();
        Credits.Hide();
    }

    #endregion

    #region Level Handlers

    private void level_LevelCompleted()
    {
        Hud.Hide();
        Menu.Show();
    }

    private void level_LevelFailed()
    {
        Hud.Hide();
        Menu.Show();
    }

    private void level_ScoreChanged(float value)
    {
        Hud.Score = value;
    }


    private void level_HealthChanged(float value)
    {
        Hud.Health = value;
    }

    #endregion

    private void LoadModules()
    {
        SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(hudSceneName, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(creditsSceneName, LoadSceneMode.Additive);
    }

    private void LoadLevel(int index)
    {
        if (Level != null)
            SceneManager.UnloadScene(Level.gameObject.scene);

        var operation = SceneManager.LoadSceneAsync(levelSceneName[index], LoadSceneMode.Additive);
        Loading.Show(operation);
    }

    private void PlayLevel()
    {
        Menu.Hide();
        switch (Level.State)
        {
            case LevelState.Paused:
                Hud.Show();
                Level.Resume();
                break;
            case LevelState.Completed:
                levelIndex++;
                levelIndex %= levelSceneName.Length;
                LoadLevel(levelIndex);
                break;
            case LevelState.Failed:
                LoadLevel(levelIndex);
                break;
            default:
                break;
        }
    }

    private void OnModulesLoaded()
    {
        ReleaseSplash();
        LoadLevel(levelIndex);
    }

    private void OnLevelLoaded()
    {
        SetLevelActive();
        Loading.Hide();
        Hud.Show();
        Level.Resume();
    }

    private IEnumerator delayedSetLevelActive()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.SetActiveScene(Level.gameObject.scene);
    }

    private void SetLevelActive()
    {
        StartCoroutine(delayedSetLevelActive());
    }

    #region Splash

    private void ShowSplash()
    {
        splashGraphic.gameObject.SetActive(true);
    }

    private void ReleaseSplash()
    {
        Destroy(splashGraphic.gameObject);
    }

    #endregion

    #region Fade 

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

    private void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private void OnFadeInComplete()
    {

    }

    private void OnFadeOutComplete()
    {
        
    }

    #endregion   

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

        ShowSplash();

        LoadModules();
    }

    #endregion
}
