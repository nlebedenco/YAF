using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private ProgessBar progress;

    public void Show(AsyncOperation operation = null)
    {
        canvas.gameObject.SetActive(true);
        progress.Operation = operation;
    }

    public void Hide()
    {
        canvas.gameObject.SetActive(false);
        progress.Operation = null;
    }

    public ProgessBar Progress
    {
        get { return progress; }
    }

    #region MonoBehaviour

    void Awake()
    {
        GameSystem.RegisterModule(this);

        Hide();
    }

    #endregion
}
