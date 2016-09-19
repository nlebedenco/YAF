using UnityEngine;

public enum LevelState
{
    Paused = 0,
    Playing,
    Completed,
    Failed
}

public class Level: MonoBehaviour
{
    public delegate void HealthChangedEvent(float value);
    public delegate void ManaChangedEvent(float value);
    public delegate void ScoreChangedEvent(float value);

    public delegate void LevelCompletedEvent();
    public delegate void LevelFailedEvent();

    private LevelState state;
    public LevelState State
    {
        get { return state; }
    }

    public void Pause()
    {
        state = LevelState.Paused;
    }

    public void Resume()
    {
        state = LevelState.Playing;
    }

    #region MonoBehaviour

    void Awake()
    {
        GameSystem.RegisterLevel(this);
        OnHealthChanged(1.0f);
        OnManaChanged(1.0f);
        Camera.main.transform.position = new Vector3(0, 30, -5);
    }

    void OnDestroy()
    {
        LevelCompleted = null;
        LevelFailed = null;
    }

    #endregion

    #region Delegates

    public LevelCompletedEvent LevelCompleted;
    public LevelFailedEvent LevelFailed;

    public HealthChangedEvent HealthChanged;
    public ManaChangedEvent ManaChanged;
    public ScoreChangedEvent ScoreChanged;

    protected void OnLevelCompleted()
    {
        var handler = LevelCompleted;
        if (handler != null)
            handler();
    }

    protected void OnLevelFailed()
    {
        var handler = LevelFailed;
        if (handler != null)
            handler();
    }

    protected void OnHealthChanged(float value)
    {
        var handler = HealthChanged;
        if (handler != null)
            handler(value);
    }

    protected void OnManaChanged(float value)
    {
        var handler = ManaChanged;
        if (handler != null)
            handler(value);
    }

    protected void OnScoreChanged(float value)
    {
        var handler = ScoreChanged;
        if (handler != null)
            handler(value);
    }

    #endregion
}
