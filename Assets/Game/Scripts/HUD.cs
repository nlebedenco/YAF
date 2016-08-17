using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hud: MonoBehaviour
{
    public delegate void PauseButtonClickedEvent();

    [SerializeField]
    private GameObject controls;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Text textHealth;

    [SerializeField]
    private Text textScore;

    private float health;
    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            textHealth.text = value.ToString();
        }
    }

    private float score;
    public float Score
    {
        get { return score; }
        set
        {
            score = value;
            textScore.text = value.ToString();
        }
    }

    public void Show()
    {
        canvas.gameObject.SetActive(true);
        controls.gameObject.SetActive(true);
    }

    public void Hide()
    {
        canvas.gameObject.SetActive(false);
        controls.gameObject.SetActive(false);
    }

    #region GUI Event Handlers

    public void btnPause_OnClick()
    {
        OnPauseButtonClicked();
    }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        GameSystem.RegisterModule(this);

        Health = 0;
        Score = 0;

        Hide();
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
