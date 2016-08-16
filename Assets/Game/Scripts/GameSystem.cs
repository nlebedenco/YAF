using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class GameSystem: MonoBehaviour
{
    private enum GameState
    {
        Play,
        Shop,
        Credits
    }

    #region Static

    private static GameSystem singleton;
    private static GameState state = GameState.Play;

    private static HUD hud;
    private static Menu menu;
    private static Options options;
    private static Level level;

    // private static AsyncOperation loadingSceneAsync;
    // 
    // private static void ChangeScene(string name)
    // {
    //     loadingSceneAsync = SceneManager.LoadSceneAsync(name);
    // }

    #endregion

    [SerializeField]
    private bool simulateMouseWithTouches = false;

    #region Overlays

    [SerializeField]
    private string hudOverlay = string.Empty;

    [SerializeField]
    private string menuOverlay = string.Empty;

    [SerializeField]
    private string optionsOverlay = string.Empty;

    #endregion

    #region Scenes

    [SerializeField]
    private string shopScene = string.Empty;

    [SerializeField]
    private string creditsScene = string.Empty;

    [SerializeField]
    private string[] levelScene = new string[0];

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
        {
            Debug.LogFormat("GameSystem already exists. Duplicate ('{0}') will be destroyed.", gameObject.name);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Input.simulateMouseWithTouches = simulateMouseWithTouches;
    }

    #endregion
}
